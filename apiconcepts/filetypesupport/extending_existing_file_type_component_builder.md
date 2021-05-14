Extending existing File Type Component Builder
==

>**Note**
>
>This functionality is only available in Studio 2011 SP3.

This chapter provides basic information on extending existing XML File Type Component Builder so your native verifier will be used when processing XML files.

Extending an existing XML File Type Component Builder
As XML is one of the standard formats supported by <Var:ProductName>, the corresponding File Type Component Builder already exists. The XML File Type Component Builder will be used to create an extension of the XML File Type Component Builder that uses your native verifier.

A File Type Component Builder is defined by a filter component builder that implements [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml). A filter component builder knows how to create parsers, writers, and so on for the corresponding file type. XML File Type Component Builder has a filter component builder.

The XML File Type Component Builder can be inherited from indirectly by implementing the [IFileTypeComponentBuilderAdapter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilderAdapter.yml) interface. To create an extension for the XML File Type Component Builder you need to add the ```FileTypeComponentBuilderExtension``` attribute to your extension component builder class.You must set the ```OriginalFileType``` property of the attribute to ```XML: Any v 1.2.0.0```. You can now access the original XML component builder methods through the ```Original``` property as shown in the code example at the end of this page.

>**Note**
>
>In <Var:ProductName> all the file type plug-in components are designed in a way that you can extend all the functionality.

Every extension filter component builder needs to have a [FileTypeComponentBuilderExtensionAttribute](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.FileTypeComponentBuilderExtensionAttribute.yml) that describes the file type component builder.

```cs
[FileTypeComponentBuilderExtension(
    Id = "XML_FilterComponentBuilderExtension_Verifier_Id",
    Name = "XML_FilterComponentBuilderExtension_Verifier_Name",
    Description = "XML_FilterComponentBuilderExtension_Verifier_Description",
    OriginalFileType = "XML: Any v 1.2.0.0")]
```


"XML_FilterComponentBuilderExtension_Verifier_Name" and "XML_FilterComponentBuilderExtension_Verifier_Description" refers to entries in the **PlugInResources.resx** file.

```xml
<data name="XML_FilterComponentBuilderExtension_Verifier_Description" xml:space="preserve">
  <value>Length Check XML Filter Component Builder</value>
</data>
<data name="XML_FilterComponentBuilderExtension_Verifier_Name" xml:space="preserve">
  <value>Length Check XML Filter Component Builder</value>
</data>
```

At this point, we have a new file type definition that is identical to the standard XML File Type Component Builder. This new file type definition needs to be changed so it includes the new native verifier. This can be accomplished simply by calling the original **BuildVerifierCollection** and adding the new verifier to this collection.

```cs
public  IVerifierCollection BuildVerifierCollection(string name)
{
    var verifierCollection = Original.BuildVerifierCollection(name);
    verifierCollection.NativeVerifiers.Add(new XMLCheckerMain());
    return verifierCollection;
}
```

This new file type definition also needs to be changed so it includes the new native verifier settings page. This is accomplished by calling the original **BuildFileTypeInformation** and adding the settings page to **WinFormSettingsPageIds**.

```cs
public  IFileTypeInformation BuildFileTypeInformation(string name)
{
    var fileTypeInformation = Original.BuildFileTypeInformation(name);
    // add "XMLVerifier_Settings" to existing WinFormSettingsPageIds
    var winFormSettingsPageIds = new List<string>(fileTypeInformation.WinFormSettingsPageIds);
    winFormSettingsPageIds.Add("XMLVerifier_Settings");
    fileTypeInformation.WinFormSettingsPageIds = winFormSettingsPageIds.ToArray();
    return fileTypeInformation;
}
```

Here is the complete code for creating a new file type definition based upon the XML file type definition using the new native verifier.


```cs
using System.Collections.Generic;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.FileTypeSupport.Native.Xml;

namespace Sdl.Sdk.FileTypeSupport.Samples.XMLChecker
{
    [FileTypeComponentBuilderExtension(
        Id = "XML_FilterComponentBuilderExtension_Verifier_Id",
        Name = "XML_FilterComponentBuilderExtension_Verifier_Name",
        Description = "XML_FilterComponentBuilderExtension_Verifier_Description",
        OriginalFileType = "XML: Any v 1.2.0.0")]
    public class VerifierFilterComponentBuilder : IFileTypeComponentBuilderAdapter
    {
        public  IFileTypeInformation BuildFileTypeInformation(string name)
        {
            var fileTypeInformation = Original.BuildFileTypeInformation(name);
            // add "XMLVerifier_Settings" to existing WinFormSettingsPageIds
            var winFormSettingsPageIds = new List<string>(fileTypeInformation.WinFormSettingsPageIds);
            winFormSettingsPageIds.Add("XMLVerifier_Settings");
            fileTypeInformation.WinFormSettingsPageIds = winFormSettingsPageIds.ToArray();
            return fileTypeInformation;
        }

        public  IVerifierCollection BuildVerifierCollection(string name)
        {
            var verifierCollection = Original.BuildVerifierCollection(name);
            verifierCollection.NativeVerifiers.Add(new XMLCheckerMain());
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

See Also
--

**Other Resources**

[Implement the User Interface](implement_the_user_interface_native.md)

[Implement the UI Controller Class](implement_the_ui_controller_class_native.md)

[Implement the Verification Logic](implement_the_verification_logic_native.md)

>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.