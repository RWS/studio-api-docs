# Performing a Scheduled Import or Export

This section shows how to schedule imports and exports for a translation memory instead of performing them remotely.

## Scheduled Import

There are two ways to import bilingual content into a server-based translation memory. You can use the [TranslationMemoryImporter](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryImporter.yml) class to import the content synchronously into the translation memory. However, this approach causes a roundtrip to the server for every batch of translation units imported (see [ChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_ChunkSize)). Over a slower network connection, that is not the most efficient option.

Instead, consider a scheduled import. This approach uploads the entire import file to the server and schedules a background operation for the Execution Server to import the file into the translation memory. It uses a direct database connection, which avoids the network overhead of a remote import. Scheduled imports use the `ScheduledTranslationMemoryImportOperation` class. For an example, see [Scheduled TMX Imports](scheduled_tmx_imports.md).

## Scheduled Export

There are two ways to export bilingual content from a server-based translation memory. You can use the [TranslationMemoryExporter](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml) class to export the content synchronously from the translation memory. However, this approach causes a roundtrip to the server for every batch of translation units exported (see [ChunkSize](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryExporter_ChunkSize)). Over a slower network connection, that is not the most efficient option.

Instead, consider a scheduled export. This approach schedules a background operation for the Execution Server to perform the export. After the export completes, download the export file from the server. It uses a direct database connection, which avoids the network overhead of a remote export. Scheduled exports use the `ScheduledTranslationMemoryExportOperation` class. For an example, see [Scheduled TMX Exports](scheduled_tmx_exports.md).
