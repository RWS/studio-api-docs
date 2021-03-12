Exporting Content from a Translation Memory
======
This section describes how to export content from a translation memory.

Export
-----
Any translation memory allows exporting translation units to a bilingual TMX file.

The export functionality is exposed by the TranslationMemoryExporter class. In order to export a translation memory, create a TranslationMemoryExporter object and specify the ITranslationMemoryLanguageDirection to export from and the TMX file to export to. Optionally specify a filter (**FilterExpression**), see Working with Filters) and subsequently call Export .

The BatchExported event is raised repeatedly during export (after evey batch of translation units has been exported, see ChunkSize). This is a cancelable event, so the export can be canceled.




<img style="display:block; " src="images/Export.png"/>