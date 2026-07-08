# Opening the File for Parsing

Implement a class that extracts translatable text from a bilingual source (BIL) file.

## Add the Parser Class

Create a class called **BilParser1.cs** in your project. Your parser class requires references to the following namespaces:

* Sdl.FileTypeSupport.Framework.BilingualApi
* Sdl.FileTypeSupport.Framework.NativeApi
* System.Xml

Bilingual files are typically XML-compliant, which simplifies parsing through the standard XML DOM API. Derive your parser from these classes and interfaces:

* [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml)
* [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml)
* [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml)

## Open and Close the Input File

Add the following global class members:

# [C#](#tab/tabid-1)
```cs
private IPersistentFileConversionProperties _fileProperties;
private XmlDocument _document;
public event EventHandler<ProgressEventArgs> Progress;
```
***

The ```_fileProperties``` object retrieves important information about the input file, primarily the file path and name. An XML document object (```_document```) allows parsing using the XML DOM API. A progress reporter event is recommended (but not required) to inform users of parsing progress for large files.

Add the following members of the [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml) interface:

# [C#](#tab/tabid-2)
```cs
public void SetFileProperties(IFileProperties properties)
{
    _fileProperties = properties.FileConversionProperties;

    Output.Initialize(DocumentProperties);

    IFileProperties fileInfo = ItemFactory.CreateFileProperties();
    fileInfo.FileConversionProperties = _fileProperties;
    Output.SetFileProperties(fileInfo);
}
```
***

This member creates objects to retrieve file information such as file name and creation date. It initializes the ```DocumentProperties``` used to create properties and output file properties. The parser initializes a document and a file properties object because the File Type Support Framework merges multiple files (e.g., **.bil* documents) into a single SDLXliff file. A global document properties object handles the SDLXliff master document, while separate file properties handle each merged file.

Add the following member, which loads the input file into the XML DOM object and sets the progress reporter to 0%:

# [C#](#tab/tabid-3)
```cs
public void StartOfInput()
{
    OnProgress(0);
    _document = new XmlDocument();
    _document.Load(_fileProperties.OriginalFilePath);
}
```
***

Add the following member, which implements logic to apply when reaching the end of the input file. Set the XML DOM object to null, set the progress reporter to 100%, and call the FileComplete and Complete functions of the output object to finalize the file:

# [C#](#tab/tabid-4)
```cs
public void EndOfInput()
{
    Output.FileComplete();
    Output.Complete();

    OnProgress(100);
    _document = null;
}
```
***

Add the following optional ```OnProgress()``` method, which takes a byte parameter to report parsing progress from 0 to 100%:

# [C#](#tab/tabid-5)
```cs
protected virtual void OnProgress(byte percent)
{
    if (Progress != null)
    {
        Progress(this, new ProgressEventArgs(percent));
    }
}
```
***

## Add the Bilingual Parser Members

Add the following required members of the bilingual parser interface:

# [C#](#tab/tabid-6)
```cs
public IDocumentProperties DocumentProperties
{
    get;
    set;
}

public IBilingualContentHandler Output
{
    get;
    set;
}
```
***

The ```DocumentProperties``` member provides access to information about the bilingual file, such as source and target language, source segment count, and save path. The ```Output``` member handles output to the bilingual document and determines paragraphs, segment pairs, and other content written to the bilingual file.

## Parse the File

Add the following member, which handles the actual file parsing operation. File content parsing is deferred to helper functions added later:

# [C#](#tab/tabid-7)
```cs
public bool ParseNext()
{
    return false;
}
```
***

Your project compiles at this point, but generates an empty bilingual document in Var:ProductName:

![EmptyBilFile](images/EmptyBilFile.jpg)

The intermediate SDLXliff file contains only basic header information from the document and file properties objects: original file name, source/target language, File Type Support Framework version, and similar metadata.

## Add the Component Reference to the Component Builder

Add the parser component to the File Type Component Builder by implementing the [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) interface. Failure to do so prevents the file type plug-in from using this component, even if the parser is in the assembly:

# [C#](#tab/tabid-8)
```cs
public IFileExtractor BuildFileExtractor(string name)
{
    var parser = new BilParser();
    var extractor = this.FileTypeManager.BuildFileExtractor(parser, this);
    return extractor;
}
```

## See Also

- [Outputting Segment Pairs](outputting_segment_pairs.md)
- [Processing Inline Tags](processing_inline_tags.md)
- [Applying Character Formatting](applying_character_formatting.md)
- [Applying the Segment Pair Confirmation Levels](applying_the_segment_pair_confirmation_levels.md)
- [Adding Context Information](adding_context_information.md)
- [Extracting Comments](extracting_comments.md)
- [Putting it all Together](putting_it_all_together.md)
- [Merging files](merging_files.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
