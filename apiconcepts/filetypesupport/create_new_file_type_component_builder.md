Create a New File Type Component Builder
===

>[!NOTE]
>
>This functionality is only available in Studio 2011 SP3.

This chapter provides basic information on extending a standard Microsoft Word 2007 File Type Component Builder so the WordArt bilingual verifier will be used.

Extending a Microsoft Word 2007 File Type Component Builder
--

As Microsoft Word 2007 is one of the standard formats supported by Var:ProductName, the corresponding File Type Component Builder already exists. This File Type Component Builder will be used to create an extension for Microsoft Word 2007 File Type Component Builder that uses the WordArt bilingual verifier.

A File Type Component Builder is defined by a filter component builder that implements [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml). A filter component builder knows how to create parsers, writers, and so on for the corresponding file type. Microsoft Word 2007 File Type Component Builder has a filter component builder.

The Microsoft Word 2007 File Type Component Builder can be inherited from indirectly. To create an extension for the The Microsoft Word 2007 File Type Component Builder you need to implement the [IFileTypeComponentBuilderAdapter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilderAdapter.yml) interface which is used to define *Word 2007 v 2.0.0.0* documents.

>[!NOTE]
>
>In Var:ProductName all the file type plug-in components are designed in a way that you can extend all the functionality.

Every filter component builder extension needs to have a [FileTypeComponentBuilderExtensionAttribute](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.FileTypeComponentBuilderExtensionAttribute.yml) that describes the file type component builder.

# [C#](#tab/tabid-1)
```cs
[FileTypeComponentBuilderExtension(
    Id = "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Id",
    Name = "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Name",
    Description = "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Description",
    OriginalFileType = "Word 2000-2003 v 1.0.0.0")]
```
***

"Word2007_FilterComponentBuilderExtension_WordArtVerifier_Name" and "Word2007_FilterComponentBuilderExtension_WordArtVerifier_Description" refers to entries in the **PlugInResources.resx** file.

# [Xml](#tab/tabid-2)
```xml
<data name="Word2007_FilterComponentBuilderExtension_WordArtVerifier_Description">
  <value>Word 2007 Filter Component Builder WordArt Verifier</value>
</data>
<data name="Word2007_FilterComponentBuilderExtension_WordArtVerifier_Name">
  <value>Word 2007 Filter Component Builder WordArt Verifier</value>
</data>
```
***

This new component builder needs to be changed so it includes the new WordArt bilingual verifier. This can be accomplished simply by calling the original **BuildVerifierCollection** and adding the new verifier to this collection.

# [C#](#tab/tabid-3)
```cs
public IVerifierCollection BuildVerifierCollection(string name)
{
    IVerifierCollection verifierCollection = Original.BuildVerifierCollection(name);
    verifierCollection.BilingualVerifiers.Add(new VerifierMain());
    return verifierCollection;
}
```
***

This new file type definition also needs to be changed so it includes the new WordArt bilingual verifier settings page. This is accomplished by calling the original **BuildFileTypeInformation** and adding the settings page to **WinFormSettingsPageIds**.

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
***

Here is the complete code for creating a new file type definition based upon the Microsoft Word 2007 file type definition using the new WordArt bilingual verifier.

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
****

Adding a verifier by creating a new file type definition based upon an existing file type definition can be quite effective but suffers one important limitation. In this example, if the user created an *sdlxliff* file based upon a Microsoft Word 2007 document **before** the new file type definition was added then any operations performed on this *sdlxliff* file will use the old Microsoft Word 2007 file type definition and that means that it will not use the WordArt bilingual verifier. Only if the user created an *sdlxliff* file based upon a Microsoft Word 2007 document **after** the new file type definition was added will the new file type definition and therefore the new bilingual verifier be used.

See Also
--



[Implement the Verification Logic](implement_the_verification_logic_bil.md)

[Extending the Filter Component Builder](extending_existing_file_type_component_builder.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
