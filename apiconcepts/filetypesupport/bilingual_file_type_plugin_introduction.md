# Bilingual File Type Plug-in Overview

The File Type Support Framework processes bilingual documents containing both source and target language content. Var:ProductName supports Bilingual Excel, Bilingual Word Documents, and XLIFF 2.0 formats.

## How Bilingual File Type Plug-ins Work

Bilingual file processing follows the same concept as monolingual (native) file processing:

1. Var:ProductName opens the bilingual file and converts it to an intermediary format (SDLXliff).
2. The system processes the intermediary document through translation, analysis, or review.
3. The intermediary file converts back to the original bilingual format.

Bilingual files store strings in multiple languages. Each source-language segment pairs with a corresponding target-language segment. These files often arrive partly pre-translated.

## Supported Bilingual Formats

Var:ProductName supports the following bilingual file formats:

* **Bilingual Excel (*.xlsx)**: Organizes source and target segments in designated columns side-by-side. This format suits spreadsheet-based translation workflows.

* **Bilingual Word Document (*.doc, *.docx)**: Presents source and target text in a word processing environment familiar to translators.

* **XLIFF 2.0 (*.xlf, *.xliff)**: Provides an industry-standard XML-based format for exchanging localizable data between tools. XLIFF 2.0 delivers robust support for translation metadata, inline markup, and segmentation.

Users open partly-translated files in Var:ProductName, complete the translation, and generate fully translated output for downstream workflows.
