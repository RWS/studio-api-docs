# Create a New File Type Component Builder

> [!NOTE]
> This functionality is only available in Studio 2011 SP3.

Extend the standard Microsoft Word 2007 File Type Component Builder so the WordArt bilingual verifier is used.

## Extend a Microsoft Word 2007 File Type Component Builder

Var:ProductName includes the standard Microsoft Word 2007 format, so the corresponding File Type Component Builder already exists. Use this as the basis for creating an extension that includes the WordArt bilingual verifier.

A File Type Component Builder is defined by a filter component builder that implements [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml). The filter component builder knows how to create parsers, writers, and other components for the corresponding file type. The Microsoft Word 2007 File Type Component Builder has its own filter component builder.

To extend the Microsoft Word 2007 File Type Component Builder, implement the [IFileTypeComponentBuilderAdapter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilderAdapter.yml) interface, which is used to define *Word 2007 v 2.0.0.0* documents.

> [!NOTE]
> In Var:ProductName, all file type plug-in components are designed to be extensible, so you can extend all functionality.

Every filter component builder extension requires a [FileTypeComponentBuilderExtensionAttribute](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.FileTypeComponentBuilderExtensionAttribute.yml) that describes the file type component builder:

# [C#](#tab/tabid-1)
```cs
[FileTypeComponentBuilderExtension(
    Id = "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Id",
    Name = "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Name",
    Description = "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Description",
    OriginalFileType = "Word 2000-2003 v 1.0.0.0")]
```

The `Word2007_FilterComponentBuilderExtension_WordArtVerifier_Name` and `Word2007_FilterComponentBuilderExtension_WordArtVerifier_Description` values refer to entries in the **PlugInResources.resx** file:

# [Xml](#tab/tabid-2)
```xml
<data name="Word2007_FilterComponentBuilderExtension_WordArtVerifier_Description">
  <value>Word 2007 Filter Component Builder WordArt Verifier</value>
</data>
<data name="Word2007_FilterComponentBuilderExtension_WordArtVerifier_Name">
  <value>Word 2007 Filter Component Builder WordArt Verifier</value>
</data>
```

Modify the new component builder to include the WordArt bilingual verifier. Call the original `BuildVerifierCollection` and add the new verifier to the collection:

# [C#](#tab/tabid-3)
```cs
public IVerifierCollection BuildVerifierCollection(string name)
{
    IVerifierCollection verifierCollection = Original.BuildVerifierCollection(name);
    verifierCollection.BilingualVerifiers.Add(new VerifierMain());
    return verifierCollection;
}
```

Modify the file type definition to include the WordArt bilingual verifier settings page. Call the original `BuildFileTypeInformation` and add the settings page to `WinFormSettingsPageIds`:

# [C#](#tab/tabid-4)
```cs
public IFileTypeInformation BuildFileTypeInformation(string name)
{
    var fileTypeInformation = Original.BuildFileTypeInformation(name);
    // add "WordArtVerifier_Settings" to existing WinFormSettingsPageIds
    var winFormSettingsPageIds = new List<string>(fileTypeInformation.WinFormSettingsPageIds);
    winFormSettingsPageIds.Add("WordArtVerifier_Settings");
    fileTypeInformation.WinFormSettingsPageIds = winFormSettingsPageIds.ToArray();

    return fileTypeInformation;
}
```

The following code shows the complete implementation of a new file type definition based on the Microsoft Word 2007 file type definition with the WordArt bilingual verifier:

# [C#](#tab/tabid-5)
```cs
using System.Collections.Generic;
using Sdl.FileTypeSupport.Filters.Word;
using Sdl.FileTypeSupport.Framework;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.Core.Globalization;

namespace Sdk.FileTypeSupport.Samples.WordArtVerifier
{
    [FileTypeComponentBuilderExtension(
        Id = "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Id",
        Name = "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Name",
        Description = "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Description",
        OriginalFileType = "Word 2000-2003 v 1.0.0.0")]
    public class VerifierFilterComponentBuilder : IFileTypeComponentBuilderAdapter
    {
        public IFileTypeInformation BuildFileTypeInformation(string name)
        {
            var fileTypeInformation = Original.BuildFileTypeInformation(name);
            // add "WordArtVerifier_Settings" to existing WinFormSettingsPageIds
            var winFormSettingsPageIds = new List<string>(fileTypeInformation.WinFormSettingsPageIds);
            winFormSettingsPageIds.Add("WordArtVerifier_Settings");
            fileTypeInformation.WinFormSettingsPageIds = winFormSettingsPageIds.ToArray();

            return fileTypeInformation;
        }

        public IVerifierCollection BuildVerifierCollection(string name)
        {
            IVerifierCollection verifierCollection = Original.BuildVerifierCollection(name);
            verifierCollection.BilingualVerifiers.Add(new VerifierMain());
            return verifierCollection;
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

        public IBilingualDocumentGenerator BuildBilingualGenerator(string name)
        {
            return Original.BuildBilingualGenerator(name);
        }

        public IFileExtractor BuildFileExtractor(string name)
        {
            return Original.BuildFileExtractor(name);
        }

        public IFileGenerator BuildFileGenerator(string name)
        {
            return Original.BuildFileGenerator(name);
        }

        public Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer BuildFileSniffer(string name)
        {
            return Original.BuildFileSniffer(name);
        }

        public IAbstractPreviewApplication BuildPreviewApplication(string name)
        {
            return Original.BuildPreviewApplication(name);
        }

        public IAbstractPreviewControl BuildPreviewControl(string name)
        {
            return Original.BuildPreviewControl(name);
        }

        public IPreviewSetsFactory BuildPreviewSetsFactory(string name)
        {
            return Original.BuildPreviewSetsFactory(name);
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

## Important Considerations

Creating a new file type definition based on an existing file type definition offers an effective approach but has one important limitation. If users create an SDLXliff file from a Microsoft Word 2007 document **before** you add the new file type definition, operations on that SDLXliff file use the old Microsoft Word 2007 file type definition. The new WordArt bilingual verifier won't be used.

Only if users create an SDLXliff file from a Microsoft Word 2007 document **after** you add the new file type definition will Var:ProductName use the new file type definition and the new bilingual verifier.

## See Also

- [Implement the Verification Logic](implement_the_verification_logic_bil.md)
- [Extending the Filter Component Builder](extending_existing_file_type_component_builder.md)

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
