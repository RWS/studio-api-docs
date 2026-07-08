# Used terminology

## File Type Support Framework terminology

This table defines the terms used in the File Type Support Framework.

| Term | Explanation | Synonym, older term, or related concept |
| --- | --- | --- |
| File Type Support Framework | File Type Support Framework 2 API. | Trados Filter Framework 1.0 |
| Source | The source language that provides the localizable content. | Original |
| Target | A target language that receives the translated content. | Translated |
| Native | Content in a single language, either source or target, usually before or after localization. | Monolingual; original file type |
| Bilingual | Content under localization, usually as paired source and target content. | TRADOStag, XLIFF, Workbench RTF |
| Text | Localizable text content. |  |
| Tag | Content that must remain unchanged during localization. Editing tools such as TagEditor or SDLEdit protect tags from accidental changes. |  |
| Inline Tag | A tag that appears inside localizable content and that users might move during localization. Typical examples include links, local formatting overrides, and embedded graphics. | Internal Tag, tw4winInternal |
| Structure Tag | A tag that appears outside localizable content. These tags usually represent document structure, such as paragraphs, tables, headers, and footers. | External Tag, tw4winExternal |
| (Inline) Placeholder Tag | An inline tag that acts as a stand-alone element. Typical examples include index markers and inline graphics. |  |
| (Inline) Start Tag | An inline tag that works with a corresponding end tag to enclose a localizable string. |  |
| (Inline) End Tag | An inline tag that works with a corresponding start tag to enclose a localizable string. |  |
| Paragraph Unit | A unit of bilingual content that translators can process as one continuous unit during localization. A paragraph unit usually maps to a single paragraph, heading, or table cell. It can contain both source and target content, or only source content before translation starts. | No direct equivalent in TRADOS or SDLX; trans-unit in XLIFF |
| Segment | A unit of source or target content inside a paragraph unit that localization tools can process independently, for example through translation memory. In most cases, a segment is a sentence. | Comparable to the source or target segment of a translation unit |
| Extract | The process of extracting localizable content from native source-language files, usually to create bilingual files for translation. | Convert, Forward Convert, to Filter |
| Generate | The process of converting translated bilingual files into native target-language files. | Back-convert, Save Target As, Clean-up |
| Semi-WYSIWYG Formatting | Display properties that help translators visualize the document layout during translation. This information remains informational only and does not apply actual formatting to the translated files. The native-format file stores the actual formatting in the tags. | elements in TRADOStag, formatting in TagEditor |
