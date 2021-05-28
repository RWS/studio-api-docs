Adding the File Writer Class
===

In this chapter you will learn how to implement the file writer component, which generates the target BIL file from an intermediary (SDLXliff) document.

Add the Writer Class
--

Start by adding a class called e.g. **BilWriter.cs** to your project. Like the parser component, this class uses the following namespaces of the bilingual and the native APIs:

* Sdl.FileTypeSupport.Framework.BilingualApi
* Sdl.FileTypeSupport.Framework.NativeApi

Since the writer is going to generate XML target files, you should also add the ```System.Xml``` namespace.
The class needs to be derived from the following class and interfaces:

* [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml)
* [IBilingualWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualWriter.yml)
* [INativeOutputSettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputSettingsAware.yml)

Add the Members for the File Properties
--

Start by adding the following global class members:

# [C#](#tab/tabid-1)
```cs
private IPersistentFileConversionProperties _originalFileProperties;
private INativeOutputFileProperties _nativeFileProperties;
private XmlDocument _targetFile;
```
***

The ```_originalFileProperties``` object provides access to the source BIL file. From this object you can retrieve any information on the original file (e.g. the original source language, encoding, file path and name, etc.).

The ```_nativeFileProperties``` object provides access to the (proposed) target BIL file information - most importantly to the target file output path.

The ```_targetFile``` object is an XML DOM object that we will use for constructing the actual target BIL file output later.

Now add the following (required) members of the [INativeOutputSettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputSettingsAware.yml) interface to initialize the objects that provide access to the original file information and the native output file information:

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
***

Next, add the members of the [IBilingualWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualWriter.yml) interface. Start with the ```SetFileProperties()``` method, which we use to load the original BIL document into our ```_targetFile``` DOM object:

# [C#](#tab/tabid-3)
```cs
public void SetFileProperties(IFileProperties fileInfo)
{
    _targetFile = new XmlDocument();
    _targetFile.Load(_originalFileProperties.OriginalFilePath);
}

#region "initialize"
public void Initialize(IDocumentProperties documentInfo)
{

}
#endregion
```
***


Then add the ```Initialize()``` method, which we leave empty for the moment. We will use this member later (see [Adding the Text Extractor Class](adding_the_text_extractor_class.md)) to create another object through with we will access the segment content:

# [C#](#tab/tabid-4)
```cs
public void Initialize(IDocumentProperties documentInfo)
{

}
```
***


Create the Paragraph Units in the Output File
--

Add another (required) member of the [IBilingualWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualWriter.yml) interface. This method is used to loop through the paragraph units of the intermediary (SDLXliff) file and output the unit elements to our target BIL file. We will leave this method empty for the moment and fill it with the required application logic later.

# [C#](#tab/tabid-5)
```cs
public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
{

}
```
***

Save and Complete the Process
--

Add the following (required) members of the [IBilingualWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualWriter.yml) interface, which are used to complete the native output file and to complete the entire parsing process. We will use the ```FileComplete()``` method to save the target BIL file and to set the DOM object to ```null```.

We will leave the ```Complete()``` method empty, as it is not required in this simple implementation. You may wonder why the interface offers two (seemingly similar) members, i.e. one to complete a file, another one to complete the writing process. Remember that <Var:ProductName> allows you to merge several native files into one big intermediary document, from which the individual target files can be generated when saving the document as target. You can use these two methods, for example, to first generate each native target file individually and then to conclude the entire writing process.

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
***

Putting it All Together
--

The skeleton writer class looks as shown below. This is the minimum amount of code required to build a file type plug-in with a writer component. You could actually build your project now, and the file writer will even produce a target BIL file. However, this target file will do nothing more than output the content of the original BIL document, which is embedded in the intermediary document as a dependency file. In the following chapters you will learn how to generate a BIL target file that includes the actual translations and comments entered in <Var:ProductName> by manipulating the target file DOM object programmatically.

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
        #region "global"
        private IPersistentFileConversionProperties _originalFileProperties;
        private INativeOutputFileProperties _nativeFileProperties;
        private XmlDocument _targetFile;
        #endregion

        #region "INativeOutputSettingsAware members"
        public void GetProposedOutputFileInfo(IPersistentFileConversionProperties fileProperties, IOutputFileInfo proposedFileInfo)
        {
            _originalFileProperties = fileProperties;
        }

        public void SetOutputProperties(INativeOutputFileProperties properties)
        {
            _nativeFileProperties = properties;
        }
        #endregion


        #region "IBilingualWriter members"


        #region "load file"
        public void SetFileProperties(IFileProperties fileInfo)
        {
            _targetFile = new XmlDocument();
            _targetFile.Load(_originalFileProperties.OriginalFilePath);
        }

        #region "initialize"
        public void Initialize(IDocumentProperties documentInfo)
        {

        }
        #endregion

        #endregion

        #region "paragraphs"
        public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
        {

        }
        #endregion

        #region "save file and complete"
        public void FileComplete()
        {
            _targetFile.Save(_nativeFileProperties.OutputFilePath);
            _targetFile = null;
        }

        public void Complete()
        {

        }
        #endregion

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
```
***

Reference the Component in the File Type Component Builder
--

Do not forget to reference the file writer class by adding the following code to the [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) implementation:

# [C#](#tab/tabid-8)
```cs
public IFileGenerator BuildFileGenerator(string name)
{
    return this.FileTypeManager.BuildFileGenerator(new BilWriter());
}
```
***

See Also
--
[Adding the Text Extractor Class](adding_the_text_extractor_class.md)

[Generating the Paragraph Units](generating_the_paragraph_units.md)

[Mapping the Segment Confirmation Levels](mapping_the_segment_confirmation_levels.md)

[Outputting all Comments](outputting_all_comments.md)


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.