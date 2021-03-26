Importing Content into a Translation Memory
=====
This section describes how to import content into a translation memory.

Import
----
Any translation memory allows importing translation units from the following file formats:

* TMX or TMX.GZ (compressed TMX)
* Supported bilingual document formats: SDLXLIFF, TTX and ITD.
Only bilingual formats are supported, which means in effect the import happens into a specific translation memory language direction (`ITranslationMemoryLanguageDirection`). Multilingual TMX files are not supported.

The import functionality is available through the TranslationMemoryImporter class. To import a file into a translation memory, set the language direction into which to import (`TranslationMemoryLanguageDirection`), specify import settings (`ImportSetting`s) and call the Import method.

The `BatchImported` event is raised repeatedly during import (after evey batch of translation units has been imported, see `ChunkSize`). The import can be canceled.

When the import is complete, the results are available through the Statistics property. This indicates how many segments how many of the translation units were added, discarded, merged, overwritten or failed to import. When you set the `InvalidTranslationUnitsExportPath` property, all translation units that failed to export are written to a TMX file in that location.


<img style="display:block; " src="images/Import.png"/>


Server-based translation memories support scheduling an import, which means the file to be imported is uploaded to the server and the server subsequently imports the file. This is often a quicker way to import data because it avoids the network latency and additional overheads when importing remotely. For more information, see [Performing a Scheduled Import or Export](performing_a_scheduled_import_or_export.md).