Opening the File for Parsing
===

In this step you will learn how to implement the class that extracts translatable text from a given bilingual source (BIL) file.

Add the Parser Class
--

Start by adding a class called e.g. **BilParser1.cs** to your project. Your parser class needs to reference the following namespaces:

* Sdl.FileTypeSupport.Framework.BilingualApi
* Sdl.FileTypeSupport.Framework.NativeApi

Bilingual files will often be XML-compliant, which makes parsing relatively easy, as you can leverage the standard XML DOM API. Therefore, you should also add the``` System.Xml``` namespace to your class.
Then, your parser needs to be derived from the following class and interfaces:

* [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml)
* [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml)
* [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml)

Open and Close the Input File
--

First, add the following global class members:

# [C#](#tab/tabid-1)
```cs
private IPersistentFileConversionProperties _fileProperties;
private XmlDocument _document;
public event EventHandler<ProgressEventArgs> Progress;
```
***

We require the ```_fileProperties``` object from which we can retrieve important information on a given input file, primarily the file path and name. We also require an XML document object (```_document```), which we can parse using the XML DOM API. It is recommended (but not required) that you implement a progress reporter event. Depending on the size of your input file, parsing may take time. Therefore it is useful to inform the end user of the file parsing progress by implementing a progress report mechanism.

Now you need to add the following members of the [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml)
interface:

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

The above member creates the object from which you can retrieve various items of information on the input file such as the file name, creation date, etc. It also initializes the ```DocumentProperties``` - used to create properties - and the output file properties. You may wonder why the parser initializes two distinct objects, i.e. a document and a file properties object. The reason for this is that the  File Type Support Framework allows you to merge several files (e.g. several **.bil* documents) into a single SDLXliff file. In this case there would be a global document properties object for the SDLXliff master document and several file properties for the different files that have been merged into one intermediate (e.g. SDLXliff) file.
Then, add the following member, which the input file into the XML DOM object. At the same time, you can use this member to set the progress reporter to 0%:

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

The interface also requires you to add the following member, in which you implement the logic that should be applied when reaching the end of the input file. For example, you can set the XML DOM object to null and the progress reporter to 100%, as the file has been fully parsed. Since the plugin also uses an Output object, which is for example used by Studio to write .sdlxliff files, you also need to call the FileComplete and Complete functions of the writer, to properly finalize the created file.

# [C#](#tab/tabid-4)
```cs
public void EndOfInput()
{
    // done with the file
    Output.FileComplete();
    // done with the document
    Output.Complete();

    OnProgress(100);
    _document = null;
}
```
***

Next, add the following (optional) ```OnProgress()``` method, which takes a byte parameter to set the progress within the range of 0 and 100%:

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

Add the Bilingual Parser Members
--

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

The document properties member provides access to important information on the bilingual (i.e. not the native) file, e.g. the source and target language, the source segment count, the path to which the document was last saved, etc.
The output member handles the output to the bilingual document. Through this member you will later determine the paragraphs, segment pairs, etc. that are written to the bilingual file.

Parse the File
--

Add the following member, which takes care of the actual file parsing operation. Note that no actual file content is parsed for the moment. We will implement the logic for parsing the file content later by adding helper functions to our parser component.

# [C#](#tab/tabid-7)
```cs
public bool ParseNext()
{
    return false;
}
```
***

At this point you could already build your project, however, your file type plug-in will only generate an empty bilingual document, which would look in <Var:ProductName> as shown below:

![EmptyBilFile](images/EmptyBilFile.jpg)

The resulting intermediary (SDLXliff) file would merely contain the basic header information that was retrieved from the document and file properties objects such as the original file name, the source/target language, the File Type Support Framework version, etc.

Add the Component Reference to the Component Builder
--

Do not forget to add the parser component to the File Type Component Builder by inserting the following code to your implementation of the [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) interface. Remember that failure to do so will mean that this component will never be used by the file type plug-in, even if the parser has been implemented in the assembly.

# [C#](#tab/tabid-8)
```cs
public IFileExtractor BuildFileExtractor(string name)
{
    var parser = new BilParser();
    var extractor = this.FileTypeManager.BuildFileExtractor(parser, this);
    return extractor;
}
```
**

See Also
--



[Outputting Segment Pairs](outputting_segment_pairs.md)

[Processing Inline Tags](processing_inline_tags.md)

[Applying Character Formatting](applying_character_formatting.md)

[Applying the Segment Pair Confirmation Levels](applying_the_segment_pair_confirmation_levels.md)

[Adding Context Information](adding_context_information.md)

[Extracting Comments](extracting_comments.md)

[Putting it all Together](putting_it_all_together.md)

[Merging files](merging_files.md)


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.