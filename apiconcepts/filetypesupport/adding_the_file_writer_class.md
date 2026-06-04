# Adding the File Writer Class

This section explains how to implement the file writer component that generates a target BIL file from an intermediary SDLXLIFF document.

## Add the Writer Class

Start by adding a class such as **BilWriter.cs** to your project. Like the parser component, this class uses the following namespaces from the bilingual and native APIs:

- `Sdl.FileTypeSupport.Framework.BilingualApi`
- `Sdl.FileTypeSupport.Framework.NativeApi`

Because the writer generates XML target files, add the `System.Xml` namespace as well.

Derive the class from the following base class and interfaces:

- [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml)
- [IBilingualWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualWriter.yml)
- [INativeOutputSettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputSettingsAware.yml)

## Add the Members for the File Properties

Start by adding the following class members:

# [C#](#tab/tabid-1)
```cs
private IPersistentFileConversionProperties _originalFileProperties;
private INativeOutputFileProperties _nativeFileProperties;
private XmlDocument _targetFile;
```

The `_originalFileProperties` object provides access to the source BIL file. You can use it to retrieve information about the original file, such as the source language, encoding, file path, and file name.

The `_nativeFileProperties` object provides access to the proposed target BIL file information, especially the target output path.

The `_targetFile` object is an XML DOM object that you will use later to construct the target BIL output file.

Next, add the required members of the [INativeOutputSettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputSettingsAware.yml) interface to initialize the objects that provide access to the original file information and the native output file information:

# [C#](#tab/tabid-2)
```cs
public void GetProposedOutputFileInfo(IPersistentFileConversionProperties fileProperties, IOutputFileInfo proposedFileInfo)
{
    _originalFileProperties = fileProperties;
}

public void SetOutputProperties(INativeOutputFileProperties properties)
{
    _nativeFileProperties = properties;
}
```

Then add the members of the [IBilingualWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualWriter.yml) interface. Start with the `SetFileProperties()` method, which loads the original BIL document into the `_targetFile` DOM object:

# [C#](#tab/tabid-3)
```cs
public void SetFileProperties(IFileProperties fileInfo)
{
    _targetFile = new XmlDocument();
    _targetFile.Load(_originalFileProperties.OriginalFilePath);
}

public void Initialize(IDocumentProperties documentInfo)
{

}
```

For now, leave the `Initialize()` method empty. In a later step, you will use this member to create another object that provides access to the segment content. For more information, see [Adding the Text Extractor Class](adding_the_text_extractor_class.md).

# [C#](#tab/tabid-4)
```cs
public void Initialize(IDocumentProperties documentInfo)
{

}
```
## Create the Paragraph Units in the Output File

Add another required member of the [IBilingualWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualWriter.yml) interface. This method loops through the paragraph units of the intermediary SDLXLIFF file and writes the corresponding `unit` elements to the target BIL file. Leave the method empty for now and add the implementation later.

# [C#](#tab/tabid-5)
```cs
public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
{

}
```
## Save and Complete the Process

Add the following required members of the [IBilingualWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualWriter.yml) interface. These members complete the native output file and the overall writing process. Use the `FileComplete()` method to save the target BIL file and set the DOM object to `null`.

Leave the `Complete()` method empty because this simple implementation does not require it. The interface offers both methods because Var:ProductName can merge several native files into a single intermediary document. You can use `FileComplete()` to finish each native target file individually and `Complete()` to conclude the overall writing process.

# [C#](#tab/tabid-6)
```cs
public void FileComplete()
{
    _targetFile.Save(_nativeFileProperties.OutputFilePath);
    _targetFile = null;
}

public void Complete()
{

}
```
## Putting It All Together

The following skeleton writer class contains the minimum code required to build a file type plug-in with a writer component. At this stage, you can build the project and the writer can produce a target BIL file. However, that file only outputs the content of the original BIL document, which the intermediary document embeds as a dependency file. In the following sections, you will learn how to manipulate the target file DOM object so that the generated BIL file includes the translations and comments entered in Var:ProductName.

# [C#](#tab/tabid-7)
```cs
using System;
using System.Xml;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.Snippets.Bilingual
{
    class BilWriter : AbstractBilingualFileTypeComponent, IBilingualWriter, INativeOutputSettingsAware
    {
        private IPersistentFileConversionProperties _originalFileProperties;
        private INativeOutputFileProperties _nativeFileProperties;
        private XmlDocument _targetFile;

        public void GetProposedOutputFileInfo(IPersistentFileConversionProperties fileProperties, IOutputFileInfo proposedFileInfo)
        {
            _originalFileProperties = fileProperties;
        }

        public void SetOutputProperties(INativeOutputFileProperties properties)
        {
            _nativeFileProperties = properties;
        }

        public void SetFileProperties(IFileProperties fileInfo)
        {
            _targetFile = new XmlDocument();
            _targetFile.Load(_originalFileProperties.OriginalFilePath);
        }

        public void Initialize(IDocumentProperties documentInfo)
        {

        }

        public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
        {

        }

        public void FileComplete()
        {
            _targetFile.Save(_nativeFileProperties.OutputFilePath);
            _targetFile = null;
        }

        public void Complete()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
```

## Reference the Component in the File Type Component Builder

Reference the file writer class by adding the following code to the [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) implementation:

# [C#](#tab/tabid-8)
```cs
public IFileGenerator BuildFileGenerator(string name)
{
    return this.FileTypeManager.BuildFileGenerator(new BilWriter());
}
```
## See Also

- [Adding the Text Extractor Class](adding_the_text_extractor_class.md)
- [Generating the Paragraph Units](generating_the_paragraph_units.md)
- [Mapping the Segment Confirmation Levels](mapping_the_segment_confirmation_levels.md)
- [Outputting all Comments](outputting_all_comments.md)


>[!NOTE]
>
> This content may be out of date. To verify the latest information on this topic, inspect the libraries in the Visual Studio Object Browser.
