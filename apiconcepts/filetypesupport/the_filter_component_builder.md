# The Filter Component Builder

This section provides information on an example Filter Component Builder. For more details, see the File Type Support Framework administration documentation.

The default File Type Manager implementation is PocoFilterManager, a simple hardcoded implementation. To use it, your filter must implement [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml). This interface requires you to implement methods that generate objects implementing [IFileTypeInformation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeInformation.yml), [INativeFileSniffer](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml), [IFileExtractor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileExtractor.yml), and [IFileGenerator](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileGenerator.yml).

The example below shows a Filter Component Builder class for the SimpleText filter. Use this as a template and insert your own component information.

This component builder uses the classes `SimpleTextSniffer`, `SimpleTextParser`, and `SimpleTextWriter` defined in the SimpleText assembly. The implementation references many types from the `Sdl.FileTypeSupport.Framework.Implementation` assembly, which contains the core File Type Support Framework implementation.

# [C#](#tab/tabid-1)
```cs
using System;
using Sdl.Core.Globalization;
using Sdl.FileTypeSupport.Framework;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.Snippets.Native
{
    /// <summary>
    /// Define Simple Text filter component builder.
    /// </summary> 
    [FileTypeComponentBuilderAttribute(Id = "SimpleText_FilterComponentBuilderExtension_Id",
                                       Name = "SimpleText_FilterComponentBuilderExtension_Name",
                                       Description = "SimpleText_FilterComponentBuilderExtension_Description")]
    public class SimpleTextFilterComponentBuilder : IFileTypeComponentBuilder
    {
        /// <summary>
        /// Gets or sets the file type manager.
        /// </summary>
        public IFileTypeManager FileTypeManager { get; set; }

        /// <summary>
        /// Gets or sets the file type definition.
        /// </summary>
        public IFileTypeDefinition FileTypeDefinition { get; set; }

        /// <summary>
        /// Returns a file type information object.
        /// </summary>
        /// <param name="name">The file type definition passes an empty string for this parameter.</param>
        /// <returns>A SimpleText file type information object.</returns>
        public virtual IFileTypeInformation BuildFileTypeInformation(string name)
        {
            var info = this.FileTypeManager.BuildFileTypeInformation();

            info.FileTypeDefinitionId = new FileTypeDefinitionId("Simple Text Filter 1.0.0.0");
            info.FileTypeName = new LocalizableString("Simple text files");
            info.FileTypeDocumentName = new LocalizableString("Test text files");
            info.FileTypeDocumentsName = new LocalizableString("Simple text files");
            info.Description = new LocalizableString("This sample filter is used to process simple text files.");
            info.FileDialogWildcardExpression = "*.text";
            info.DefaultFileExtension = "text";
            info.Icon = new IconDescriptor("assembly://Sdl.Sdk.FileTypeSupport.Samples.SimpleText/Sdl.Sdk.FileTypeSupport.Samples.SimpleText.SimpleText.ico");

            info.WinFormSettingsPageIds = new string[]
            {
                "SimpleText_Settings",
                "QuickInserts_Settings",
            };

            return info;
        }

        /// <summary>
        /// Gets the file sniffer for this component.
        /// </summary>
        /// <param name="name">Not used.</param>
        /// <returns>A SimpleText file sniffer.</returns>
        public virtual INativeFileSniffer BuildFileSniffer(string name)
        {
            return new SimpleTextSniffer();
        }

        /// <summary>
        /// Gets the file extractor for this component.
        /// </summary>
        /// <param name="name">Not used.</param>
        /// <returns>A file extractor containing a SimpleText parser.</returns>
        public virtual IFileExtractor BuildFileExtractor(string name)
        {
            var parser = new SimpleTextParser();
            var extractor = this.FileTypeManager.BuildFileExtractor(this.FileTypeManager.BuildNativeExtractor(parser), this);
            return extractor;
        }

        /// <summary>
        /// Gets the file generator for this component.
        /// </summary>
        /// <param name="name">Not used.</param>
        /// <returns>A file generator containing a SimpleText writer.</returns>
        public virtual IFileGenerator BuildFileGenerator(string name)
        {
            return this.FileTypeManager.BuildFileGenerator(this.FileTypeManager.BuildNativeGenerator(new SimpleTextWriter()));
        }

        /// <summary>
        /// Gets the preview sets supported for this component.
        /// </summary>
        /// <param name="name">Not used.</param>
        /// <returns>A preview factory containing preview sets.</returns>
        public virtual IPreviewSetsFactory BuildPreviewSetsFactory(string name)
        {
            IPreviewSetsFactory previewFactory = FileTypeManager.BuildPreviewSetsFactory();

            //preview in external application
            IPreviewSet previewSet = previewFactory.CreatePreviewSet();
            previewSet.Id = new PreviewSetId("ExternalPreview");
            previewSet.Name = new LocalizableString("assembly://Sdl.Sdk.FileTypeSupport.Samples.SimpleText/Sdl.Sdk.FileTypeSupport.Samples.SimpleText.Properties.Resources/ExternalPreview_Name");

            IApplicationPreviewType sourceAppPreviewType = previewFactory.CreatePreviewType<IApplicationPreviewType>() as IApplicationPreviewType;

            if (sourceAppPreviewType != null)
            {
                sourceAppPreviewType.SingleFilePreviewApplicationId = new PreviewApplicationId("ExternalPreview");
                previewSet.Source = sourceAppPreviewType;
            }

            IApplicationPreviewType targetAppPreviewType = previewFactory.CreatePreviewType<IApplicationPreviewType>() as IApplicationPreviewType;
            if (targetAppPreviewType != null)
            {
                targetAppPreviewType.SingleFilePreviewApplicationId = new PreviewApplicationId("ExternalPreview");
                previewSet.Target = targetAppPreviewType;
            }

            previewFactory.GetPreviewSets(null).Add(previewSet);

            //internal static preview
            IPreviewSet internalPreviewSet = previewFactory.CreatePreviewSet();
            internalPreviewSet.Id = new PreviewSetId("InternalStaticPreview");
            internalPreviewSet.Name = new LocalizableString("assembly://Sdl.Sdk.FileTypeSupport.Samples.SimpleText/Sdl.Sdk.FileTypeSupport.Samples.SimpleText.Properties.Resources/InternalStaticPreview_Name");

            IControlPreviewType sourceCtrlPreviewType = previewFactory.CreatePreviewType<IControlPreviewType>() as IControlPreviewType;
            if (sourceCtrlPreviewType != null)
            {
                sourceCtrlPreviewType.TargetGeneratorId = new GeneratorId("InternalPreview");
                internalPreviewSet.Source = sourceCtrlPreviewType;
            }

            IControlPreviewType targetCtrlPreviewType = previewFactory.CreatePreviewType<IControlPreviewType>() as IControlPreviewType;
            if (targetCtrlPreviewType != null)
            {
                targetCtrlPreviewType.TargetGeneratorId = new GeneratorId("InternalPreview");
                internalPreviewSet.Target = targetCtrlPreviewType;
            }
            previewFactory.GetPreviewSets(null).Add(internalPreviewSet);

            //source and target real-time navigable HTML browser control preview
            IPreviewSet internalHtmlPreviewSet = previewFactory.CreatePreviewSet();
            internalHtmlPreviewSet.Id = new PreviewSetId("InternalRealTimeNavigableHtmlPreview");
            internalHtmlPreviewSet.Name = new LocalizableString("assembly://Sdl.Sdk.FileTypeSupport.Samples.SimpleText/Sdl.Sdk.FileTypeSupport.Samples.SimpleText.Properties.Resources/InternalRealTimeNavigablePreview_Name");

            IControlPreviewType sourceInternalHtmlCtrlPreviewType = previewFactory.CreatePreviewType<IControlPreviewType>() as IControlPreviewType;
            if (sourceInternalHtmlCtrlPreviewType != null)
            {
                sourceInternalHtmlCtrlPreviewType.TargetGeneratorId = new GeneratorId("InternalPreview");
                sourceInternalHtmlCtrlPreviewType.SingleFilePreviewControlId = new PreviewControlId("InternalRealtimeNavigableBrowser");
                internalHtmlPreviewSet.Source = sourceInternalHtmlCtrlPreviewType;
            }

            IControlPreviewType targetInternalHtmlCtrlPreviewType = previewFactory.CreatePreviewType<IControlPreviewType>() as IControlPreviewType;
            if (targetInternalHtmlCtrlPreviewType != null)
            {
                targetInternalHtmlCtrlPreviewType.TargetGeneratorId = new GeneratorId("InternalPreview");
                targetInternalHtmlCtrlPreviewType.SingleFilePreviewControlId =
                    new PreviewControlId("InternalRealtimeNavigableBrowser");
                internalHtmlPreviewSet.Target = targetCtrlPreviewType;
            }
            previewFactory.GetPreviewSets(null).Add(internalHtmlPreviewSet);

            return previewFactory;
        }

        /// <summary>
        /// Creates a new instance of the preview control with the specified name.
        /// This method must be called from the main thread to ensure controls are
        /// instantiated on the same thread as the application message pump.
        /// </summary>
        /// <param name="name">Not used.</param>
        /// <returns>A preview control instance, or null if not implemented.</returns>
        public virtual IAbstractPreviewControl BuildPreviewControl(string name)
        {
            if (name == "PreviewControl_InternalStaticPreviewControl")
            {
                return new InternalPreviewController();
            }
            else if (name == "PreviewControl_InternalRealtimeNavigableBrowser")
            {
                //return GenericInternalWebBrowserPreviewControl();
                throw new NotImplementedException();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the quick tags factory for this component.
        /// </summary>
        /// <param name="name">Not used.</param>
        /// <returns>A quick tags factory.</returns>
        public virtual IQuickTagsFactory BuildQuickTagsFactory(string name)
        {
            IQuickTagsFactory quickTags = FileTypeManager.BuildQuickTagsFactory();
            quickTags.GetQuickTags(null).SetStandardQuickTags(QuickInsertBuilder.BuildStandardQuickTags());
            return quickTags;
        }

        /// <summary>
        /// Gets the verifier collection for this component.
        /// The verifier collection is an optional component for a file type.
        /// </summary>
        /// <param name="name">Not used.</param>
        /// <returns>A verifier collection, or null if not implemented.</returns>
        public virtual IVerifierCollection BuildVerifierCollection(string name)
        {
            return null;
        }

        /// <summary>
        /// Gets a native or bilingual document generator of the type specified by the name parameter.
        /// </summary>
        /// <param name="name">The generator name.</param>
        /// <returns>A generator instance, or null if not implemented.</returns>
        public virtual IAbstractGenerator BuildAbstractGenerator(string name)
        {
            if (name == "Generator_DefaultPreview")
            {
                return FileTypeManager.BuildFileGenerator(FileTypeManager.BuildNativeGenerator(new SimpleTextWriter()));
            }
            if (name == "Generator_InternalPreview")
            {
                INativeGenerator nativeGenerator = FileTypeManager.BuildNativeGenerator(new InternalPreviewWriter());
                IFileGenerator fileGenerator = FileTypeManager.BuildFileGenerator(nativeGenerator);
                return fileGenerator;
            }

            return null;
        }

        /// <summary>
        /// Gets the additional generator information for this file type.
        /// </summary>
        /// <param name="name">Not used.</param>
        /// <returns>Additional generator information, or null if not applicable.</returns>
        public virtual IAdditionalGeneratorsInfo BuildAdditionalGeneratorsInfo(string name)
        {
            return null;
        }

        /// <summary>
        /// Gets the bilingual document generator for this component, if available.
        /// </summary>
        /// <param name="name">Not used.</param>
        /// <returns>A bilingual generator, or null if not implemented.</returns>
        public virtual IBilingualDocumentGenerator BuildBilingualGenerator(string name)
        {
            return null;
        }

        /// <summary>
        /// Creates a new instance of the preview application with the specified name.
        /// Currently only supports building an external preview application.
        /// </summary>
        /// <param name="name">The preview application name.</param>
        /// <returns>An external preview application, or null if not implemented.</returns>
        public virtual IAbstractPreviewApplication BuildPreviewApplication(string name)
        {
            if (name == "PreviewApplication_ExternalPreview")
            {
               // GenericExteralPreviewApplication genericExteralPreviewApplication = new GenericExteralPreviewApplication();
               // genericExteralPreviewApplication.ApplicationPath = @"c:\Windows\System32\notepad.exe";
               // return genericExteralPreviewApplication;
                throw new NotImplementedException();
            }
            else
            {
                return null;
            }
        }
    }
}
```

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
