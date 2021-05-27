What a File Type Plug-in Must Do
==

Suppose you want to develop a filter that allows you to translate files <Var:ProductName> which are not supported by the default installation of the product.

Your proposed filter should do the following:

* The new file type must be listed in the File type dropdown list of the SDL Trados Studio 2017 *Open dialog*, see [Opening a document for translation](opening_a_document_for_translation.md) chapter.
* <Var:ProductName> must be able to open the new file type and display it, see [The File Parser](the_file_parser.md) chapter (extraction).
* Upon opening the file, the original format needs to be converted into SDL XLIFF by the SDL File Type Support Framework.
* It must be possible to batch-translate/analyze the new file format in <Var:ProductName>.
* The view of <Var:ProductName> needs to display the text to translate. Any untranslatable bits need to be displayed as tags or hidden from the translator.
* The display should also include (semi-)WYSIWYG formatting, i.e. bold, italic, underline, etc., see [Text Formatting](text_formatting.md)
* Optional: <Var:ProductName> should be able to display the document in its native format using the Realtime Preview feature.
* The user must be able to translate the text using <Var:ProductName>.
* The user must be able to save the file in SDL XLIFF format.
* The user must be able to save the fully translated file in the target format (generation), see  [The File Writer](the_file_writer.md).
* The filter should be able check the validity of the target document during generation (verification).
* The user must be able to change any required filter settings, see [The text formatting](filter_ui_settings.md).
* If any problem occurs during any step of translation workflow, the filter should notify the user accordingly, see [Reporting problems](reporting_problems.md). Since filters may also be used in a server envinroment, the filter should use the built-in feature of the SDL File Type Support Framework for displaying messages, where no user interaction is required.

Based on FFAPI we can create [monolingual](https://en.wikipedia.org/wiki/Monolingualism) (native) parsers, which can read in the translatable source language for a file type during the content extracttion phase and write the translated text back to a copy of the original document during a content generation phase, or a bilingual parser, which can read both the source and the target text from a bilingual document that contains the original source language text and the translated target language text. A typical example of a monolingual file type is an MS Word (* *.doc*)document; a typical example of a [bilingual](https://en.wikipedia.org/wiki/Multilingualism) file type would be a TradosTag (* *.ttx*) document.

As you can see, the list of things that the filter will ultimately accomplish is rather comprehensive. In the next chapters we will take you step-by-step through the process of developing a native and a bilingual filter (highly simplified filters) and demonstrate most of the required functions that a filter should implement.


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.