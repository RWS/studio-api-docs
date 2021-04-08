Opening a document for translation
=====
In <Var:ProductName> translators can open single documents for translation and editing.

When processing single documents, the file to translate is opened in <Var:ProductName>, e.g. a Microsoft Word (*.doc) document. In this step, the File Type Support Framework decides which file type plug-in best supports the given document format.

<img style="display:block; " src="images/OpenDoc.jpg"/>

The user then sets the required source and target language and selects one or more translation memories (TM). Note that users are not required to select a TM. They can open and process the file in the editor of <Var:ProductName> without a TM.

<img style="display:block; " src="images/OpenDoc2.jpg"/>

Conversion into the Intermediary Format
------
In the background, an intermediary, bilingual file (e.g. SDL XLIFF) is generated from the native document. This step is called **extraction**, as the translatable content gets extracted and exposed for translation/editing purposes. The original native file can be embedded as a dependency file inside the intermediary file. This dependency might be required to generate the native target document from the intermediary file. This might be necessary, as native documents often contain elements such as images, diagrams, etc., which need to be included in the native target document.

The source and target content is presented in a side-by-side editor. The target content is entered (segment by segment) into the column on the right-hand side. The last column contains the so-called document structure information, which indicates whether a segment is e.g. a heading (**H**), a footnote (**FN**), etc. (see [Using Context Information](using_context_information.md)). The file type plug-in extracts localizable content for translation/editing. The editor view of <Var:ProductName> will also show any special elements as inline placeholder tags, for example footnote references, fields, index markers, etc. (see [Tag display modes](tag_display_modes.md)). Those elements have to be inserted into the target text by the translator, otherwise the corresponding footnote reference, field, etc. would be missing from the resulting target document.

<img style="display:block; " src="images/SampleBilingual.jpg"/>

During the translation process the document is saved in the intermediary bilingual format (e.g. SDL XLIFF). While the translation is in progress, all changes occur within the intermediary file, i.e. the native document stays untouched.


<img style="display:block; " src="images/SaveAsXLIFF.jpg"/>

Generating the Native Target File
------
At the end of the translation phase, the translator uses a command such as **Save Target As** to generate the target-language version in the native document format. This last step is called **generation** (i.e. generation of the native target file).

<img style="display:block; " src="images/SaveAsNative.jpg"/>

See Also
-----
[Creating projects](creating_projects.md)

[File type settings](file_type_settings.md)

[Using context information](using_context_information.md)

[Tag display modes](tag_display_modes.md)

[Saving to different file types](saving_to_different_file_types.md)

[Implementing the File Parser]()