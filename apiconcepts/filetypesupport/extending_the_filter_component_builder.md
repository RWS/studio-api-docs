Extending the Filter Component Builder
==
This section shows how to extend an existing file type plug-in

>**Note**
>
>Please note that this functionality is only available in Studio 2011 SP3.

Existing file type plug-in can be extended by implementing the ```IFileTypeComponentBuilderAdapter``` interface and applying the ```FileTypeComponentBuilderExtensionAttribute``` attribute to the component builder class. Using this technique it is easy to add new [Pre and Post Tweakers](native_file_tweakers.md), add new Native and Bilingual Content Processors to the [Parser](the_file_parser.md) and to the [Writer]the_file_writer.md) and add new Verifiers (Native or Bilingual). It also allows to redefine the file type plug-ins [Sniffer](the_file_sniffer.md) and the [IFileTypeInformation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeInformation.yml) associated to the file type plug-in. For more advanced users it is also possible to define a new [File Preview mechanism](the_filter_preview.md) to existing file type plug-in.

How to do it
--

The Filter extension implementation consists of a class that implements the ```IFileTypeComponentBuilderAdapter``` interface. The original File Type Component Builder can be accessed by the ```Original``` property. For example to add a new [IFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IFilePreTweaker.yml) to the existing SimpleText Filter provided with the examples of this SDK it is enough to implement the [BuildFileExtractor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.DynamicFilterComponentBuilder.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_DynamicFilterComponentBuilder_BuildFileExtractor_System_String_) method, call the original method and add the new tweaker to the [INativeExtractor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeExtractor.yml) using [AddFileTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileExtractor.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_IFileExtractor_AddFileTweaker_Sdl_FileTypeSupport_Framework_NativeApi_IFilePreTweaker_) method. The following code shows how to do this:

```cs
public  IFileExtractor BuildFileExtractor(string name)
{
    // remember to call the original component builder method
    var extractor = Original.BuildFileExtractor(name);

    // add a pre tweaker
    extractor.AddFileTweaker(new SimpleTextExtensionPreTweaker { RequireValidEncoding = false });

    // add a native content processor
    if (extractor.NativeExtractor != null)
    {
        extractor.NativeExtractor.AddProcessor(new NativeContentProcessorStub());
    }

    // add a bilingual content processor
    extractor.AddBilingualProcessor(new BilingualContentProcessorStub());

    return extractor;
}
```

where ```SimpleTextExtensionPreTweaker``` is the class that implements the new Tweaker that is being added.

Note that extending an extension and extending a filter template are not supported by the filter framework and can have unexpected results.

The next code shows part of the example (SimpleTextExtension) included with the sample projects. Note that the class has an attribute called ```FileTypeComponentBuilderExtensionAttribute``` to enable the plug-in mechanism. The property ```OriginalFileType``` must be set to match the FileTypeId of the FileType you wish to extend. Please refer to the [Build the File Type Plug-in](build_the_file_type_plug_in.md) topic for more information.

```cs
namespace Sdl.Sdk.FileTypeSupport.Samples.SimpleTextExtension
{
    using Sdl.Sdk.FileTypeSupport.Samples.SimpleText;
    using Sdl.FileTypeSupport.Framework.IntegrationApi;
    using Sdl.FileTypeSupport.Framework.NativeApi;
    using Sdl.FileTypeSupport.Framework;
    using Sdl.Core.Globalization;

    [FileTypeComponentBuilderExtensionAttribute(Id = "SimpleTextFilterExtension_Id",
                                       Name = "SimpleTextFilterExtension_Name",
                                       Description = "SimpleTextFilterExtension_Description",
                                       OriginalFileType = "Simple Text Filter 1.0.0.0")]
    public sealed class SimpleTextExtensionComponentBuilder : IFileTypeComponentBuilderAdapter
    {
        public  IFileTypeInformation BuildFileTypeInformation(string name)
        {
            var info = Original.BuildFileTypeInformation(name);

            info.FileTypeDefinitionId = new FileTypeDefinitionId("SimpleTextExtension 1.0.0.0");
            info.FileTypeName = new LocalizableString("SimpleTextExtension");
            //info.FileTypeDocumentName = new LocalizableString("SimpleTextExtension");
            //info.FileTypeDocumentsName = new LocalizableString("SimpleTextExtensions");
            //info.Description = new LocalizableString("Simple Extension Filter");
            //info.FileDialogWildcardExpression = "*.txtExtension";
            //info.DefaultFileExtension = "txtExtension";

            return info;
        }

        public INativeFileSniffer BuildFileSniffer(string name)
        {
            return Original.BuildFileSniffer(name);
        }

        #region BuildFileExtractor
        public  IFileExtractor BuildFileExtractor(string name)
        {
            // remember to call the original component builder method
            var extractor = Original.BuildFileExtractor(name);

            // add a pre tweaker
            extractor.AddFileTweaker(new SimpleTextExtensionPreTweaker { RequireValidEncoding = false });

            // add a native content processor
            if (extractor.NativeExtractor != null)
            {
                extractor.NativeExtractor.AddProcessor(new NativeContentProcessorStub());
            }

            // add a bilingual content processor
            extractor.AddBilingualProcessor(new BilingualContentProcessorStub());

            return extractor;
        }
        #endregion

        public  IFileGenerator BuildFileGenerator(string name)
        {
            // remember to call the base class method
            var generator = Original.BuildFileGenerator(name);

            // add a post tweaker
            generator.AddFileTweaker(new SimpleTextExtensionPostTweaker() { RequireValidEncoding = false });

            // add a native processor to the generator
            if (generator.NativeGenerator != null)
            {
                generator.NativeGenerator.AddProcessor(new NativeContentProcessorStub());
            }

            // add a bilingual content processor
            generator.AddBilingualProcessor(new BilingualContentProcessorStub());

            return generator;
        }

        public  IVerifierCollection BuildVerifierCollection(string name)
        {
            // remember to call the base class method
            var verifierCollection = Original.BuildVerifierCollection(name);
            verifierCollection.NativeVerifiers.Add(new NativeVerifierStub());
            verifierCollection.BilingualVerifiers.Add(new BilingualVerifierStub());
            return verifierCollection;
        }

        public  IBilingualDocumentGenerator BuildBilingualGenerator(string name)
        {
            return Original.BuildBilingualGenerator(name);
        }

        public  IPreviewSetsFactory BuildPreviewSetsFactory(string name)
        {
            return Original.BuildPreviewSetsFactory(name);
        }

        public  IAbstractPreviewApplication BuildPreviewApplication(string name)
        {
            return Original.BuildPreviewApplication(name);
        }

        public  IAbstractPreviewControl BuildPreviewControl(string name)
        {
            return Original.BuildPreviewControl(name);
        }

        public IFileTypeComponentBuilder Original { get; set; }

        public IAbstractGenerator BuildAbstractGenerator(string name)
        {
            return Original.BuildAbstractGenerator(name);
        }

        public IAdditionalGeneratorsInfo BuildAdditionalGeneratorsInfo(string name)
        {
            return Original.BuildAdditionalGeneratorsInfo(name);
        }

        public IQuickTagsFactory BuildQuickTagsFactory(string name)
        {
            return Original.BuildQuickTagsFactory(name);
        }

        public IFileTypeManager FileTypeManager { get; set; }

        public IFileTypeDefinition FileTypeDefinition { get; set; }
    }
}
```

Note that if the ```FileTypeDefinitionId``` is changed in the ```BuildFileTypeInformation``` section, then the extension will appear as a new filter in the Studio file type options dialog. Otherwise it will simply override the original one (i.e. no new filter will appear in U.I.)

>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.