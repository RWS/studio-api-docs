# Extending the Filter Component Builder

This page shows how to extend an existing file type plug-in.

You can extend an existing file type plug-in by implementing the `IFileTypeComponentBuilderAdapter` interface and applying the `FileTypeComponentBuilderExtensionAttribute` attribute to the component builder class. This approach lets you add [pre- and post-tweakers](native_file_tweakers.md), add native and bilingual content processors to the [parser](the_file_parser.md) and [writer](the_file_writer.md), and add native or bilingual verifiers. It also lets you redefine the plug-in [sniffer](the_file_sniffer.md), update the associated [IFileTypeInformation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeInformation.yml), or define a new [file preview mechanism](the_filter_preview.md) for an existing file type plug-in.

## How to extend a builder

A filter extension is implemented as a class that implements `IFileTypeComponentBuilderAdapter`. You can access the original File Type Component Builder through the `Original` property.

For example, to add a new [IFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IFilePreTweaker.yml) to the existing SimpleText filter from the SDK samples, implement the [BuildFileExtractor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.DynamicFilterComponentBuilder.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_DynamicFilterComponentBuilder_BuildFileExtractor_System_String_) method, call the original method, and add the tweaker to the [INativeExtractor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeExtractor.yml) by using [AddFileTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileExtractor.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_IFileExtractor_AddFileTweaker_Sdl_FileTypeSupport_Framework_NativeApi_IFilePreTweaker_). The following example shows this pattern:

# [C#](#tab/tabid-1)
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

In this example, `SimpleTextExtensionPreTweaker` implements the new tweaker.

The filter framework does not support extending an extension or extending a filter template. Those approaches can produce unexpected results.

The next example shows part of the `SimpleTextExtension` sample project. The class uses the `FileTypeComponentBuilderExtensionAttribute` attribute to enable the plug-in mechanism. Set the `OriginalFileType` property to the `FileTypeId` of the file type that you want to extend. For more information, see [Build the File Type Plug-in](build_the_file_type_plug_in.md).

## Full example

# [C#](#tab/tabid-2)
```cs
namespace Sdk.FileTypeSupport.Samples.SimpleTextExtension
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

If you change `FileTypeDefinitionId` in `BuildFileTypeInformation`, the extension appears as a new filter in the Studio file type options dialog. Otherwise, it overrides the original filter and no new filter appears in the UI.

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
