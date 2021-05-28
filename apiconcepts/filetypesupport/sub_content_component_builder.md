Sub Content Component Builder
==

An example of a sub-content component builder is shown below - note the implementation of [ISubContentComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISubContentComponentBuilder.yml)

# [C#](#tab/tabid-1)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.FileTypeSupport.Framework;
using Sdl.Core.Globalization;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.Snippets.Native
{
    [FileTypeComponentBuilderAttribute(Id = "SimpleText_SubContentComponentBuilder_Id",
                                       Name = "SimpleText_FilterComponentBuilderExtension_Name",
                                       Description = "SimpleText_FilterComponentBuilderExtension_Description")]
    class SimpleTextSubContentComponentBuilder : IFileTypeComponentBuilder, ISubContentComponentBuilder
    {
        // AbstractGenerator not supported by SubContentComponentBuilder
        public IAbstractGenerator BuildAbstractGenerator(string name)
        {
            return null;
        }

        // AdditionalGeneratorsInfo not supported by SubContentComponentBuilder
        public IAdditionalGeneratorsInfo BuildAdditionalGeneratorsInfo(string name)
        {
            return null;
        }

        // BilingualGenerator not supported by SubContentComponentBuilder
        public IBilingualDocumentGenerator BuildBilingualGenerator(string name)
        {
            return null;
        }

        // FileExtractor not supported by SubContentComponentBuilder
        public IFileExtractor BuildFileExtractor(string name)
        {
            return null;
        }

        // FileGenerator not supported by SubContentComponentBuilder
        public IFileGenerator BuildFileGenerator(string name)
        {
            return null;
        }

        // FileSniffer not supported by SubContentComponentBuilder
        public INativeFileSniffer BuildFileSniffer(string name)
        {
            return null;
        }

        public IFileTypeInformation BuildFileTypeInformation(string name)
        {
            var info = this.FileTypeManager.BuildFileTypeInformation();

            info.FileTypeDefinitionId = new FileTypeDefinitionId("Simple Text Embedded Content 1.0.0.0");
            info.FileTypeName = new LocalizableString("Simple text embedded content");
            info.FileTypeDocumentName = new LocalizableString("Test text embedded content");
            info.FileTypeDocumentsName = new LocalizableString("Simple text embedded content");
            info.Description = new LocalizableString("This sample sub-content processor handles simple text");
            info.Icon = new IconDescriptor("assembly://Sdl.Sdk.FileTypeSupport.Samples.SimpleText/Sdl.Sdk.FileTypeSupport.Samples.SimpleText.SimpleText.ico");

            info.WinFormSettingsPageIds = new string[]
            {
                "SimpleText_Settings",
            };

            return info;
        }

        // PreviewApplication not supported by SubContentComponentBuilder
        public IAbstractPreviewApplication BuildPreviewApplication(string name)
        {
            return null;
        }

        // PreviewControl not supported by SubContentComponentBuilder
        public IAbstractPreviewControl BuildPreviewControl(string name)
        {
            return null;
        }

        // PreviewSetsFactory not supported by SubContentComponentBuilder
        public IPreviewSetsFactory BuildPreviewSetsFactory(string name)
        {
            return null;
        }

        // QuickTagsFactory not supported by SubContentComponentBuilder
        public IQuickTagsFactory BuildQuickTagsFactory(string name)
        {
            return null;
        }

        // VerifierCollection not supported by SubContentComponentBuilder
        public IVerifierCollection BuildVerifierCollection(string name)
        {
            return null;
        }

        public IFileTypeManager FileTypeManager { get; set; }

        public IFileTypeDefinition FileTypeDefinition { get; set; }

        public ISubContentExtractor BuildSubContentExtractor(string name)
        {
            var parser = new SimpleSubContentTextParser();
            var extractor = this.FileTypeManager.BuildSubContentExtractor(this.FileTypeManager.BuildNativeSubContentExtractor(parser), this);
            return extractor;
        }

        public ISubContentGenerator BuildSubContentGenerator(string name)
        {
            return this.FileTypeManager.BuildSubContentGenerator(this.FileTypeManager.BuildNativeSubContentGenerator(new SimpleSubContentTextWriter()));
        }
    }
}
```
***

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
