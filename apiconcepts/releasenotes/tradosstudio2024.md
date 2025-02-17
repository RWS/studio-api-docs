Release Notes for Var:ProductNameWithEdition
===================

# Trados Studio Automation
The changes are included in `Sdl.TranslationStudioAutomation.IntegrationApi`.

## Classes
### Added Classes
* [EditDistanceComputeParams](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditDistanceComputeParams.yml)
* [SegmentOperations](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.SegmentOperations.yml)
    * New method `Task<EditDistance> GetEditDistanceAsync(ISegment original, ISegment updated, CultureCode cultureCode, EditDistanceComputeParams computeParams)` to compute the edit distance between two segments.
    * New method `Task<WordCounts> GetWordCountAsync(ISegment segment, CultureCode cultureCode)`to retrieve the word count for a given segment and culture.
* [TermRecognitionResultsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Editor.TermRecognitionResults.TermRecognitionResultsController.yml) to return the results displayed in Studio TermRecognition view.

### Updated Classes
* Added event handler `TranslationFinished` in [TranslationResultsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Editor.TranslationResults.TranslationResultsController.yml) to identify when a translation is done.

## Interfaces
### Added Interfaces
* [ITermRecognitionResultsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Editor.TermRecognitionResults.ITermRecognitionResultsController.yml)

### Updated Interfaces
* Added new event handler `SegmentTranslated` in [IStudioDocument](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml) to identify when an Apply Translation action is executed.
* Added new event handler `TranslationFinished` in [ITranslationResultsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Editor.TranslationResults.ITranslationResultsController.yml) to identify when a translation is done.

# Terminology Provider
The following changes are included in `Sdl.Terminology.TerminologyProvider.Core`.

## [DescriptiveField](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.DescriptiveField.yml)
* Added new property `ShowInUI` to determine whether the field should be shown in the UI (e.g. term recognition results window).

## [ITerminologyExport](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyExport.yml)
* Added new method `void Export(TerminologyExportType exportFormatType, string path, Dictionary<string, string> exportProperties)` to perform a termbase export to a specific format.

## [ITerminologyImport](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyImport.yml)
* Added new method `void Import(TerminologyImportType importType, List<string> importFiles, Dictionary<string, string> importProperties = null)` to import multiple files in a single call.

## [TermbaseExportException](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Exceptions.TermbaseExportException.yml)
* Added new exception to be used when a termbase export fails.

# Editor API changes
The following changes are included in `Sdl.DesktopEditor.EditorApi`:
* Added new interface [IInteractiveAccessibleCustomColumn](../../api/integration/Sdl.DesktopEditor.EditorApi.IInteractiveAccessibleCustomColumn.yml) that exposes Accessibility Details.
