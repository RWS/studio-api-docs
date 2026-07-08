# What a File Type Plug-in Must Do

If you want to develop a filter for a file format that Var:ProductName does not support by default, your plug-in should provide the following capabilities.

## Required capabilities

* List the new file type in the **File type** drop-down in the Var:ProductName **Open** dialog. For more information, see [Opening a document for translation](opening_a_document_for_translation.md).
* Open the new file type in Var:ProductName and display its content. For more information, see [The File Parser](the_file_parser.md).
* Convert the original file format to SDLXLIFF during extraction.
* Support batch tasks such as translation and analysis for the new file format.
* Display the translatable text in Var:ProductName. Render non-translatable content as tags or hide it from the translator.
* Preserve semi-WYSIWYG formatting such as bold, italic, and underline. For more information, see [Text Formatting](text_formatting.md).
* Optionally display the document in its native format through the Realtime Preview feature.
* Let users translate the text in Var:ProductName.
* Let users save the file in SDLXLIFF format.
* Generate the fully translated target file. For more information, see [The File Writer](the_file_writer.md).
* Validate the target document during generation.
* Let users change any required filter settings. For more information, see [Filter UI settings](filter_ui_settings.md).
* Report problems at any stage of the translation workflow. For more information, see [Reporting problems](reporting_problems.md). Because filters can also run in server environments, use the built-in File Type Support Framework messaging features that do not require user interaction.

## Parser models

The File Type Support Framework API (FFAPI) lets you create monolingual and bilingual parsers.

* A [monolingual](https://en.wikipedia.org/wiki/Monolingualism) parser reads the translatable source text during extraction and writes the translated text back during generation. A typical monolingual file type is a Microsoft Word `*.doc` document.
* A [bilingual](https://en.wikipedia.org/wiki/Multilingualism) parser reads both source and target text from a bilingual document. A typical bilingual file type is a TradosTag `*.ttx` document.

## Next steps

The following chapters walk through the process of developing simplified native and bilingual filters. They also demonstrate most of the functions that a file type plug-in should implement.


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
