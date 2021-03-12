Used Terminology

File Type Support Framework Terminology

| Term        | Explanation | Synonym, Older Term or Related Concept|
| ----------- | ----------- |---------------------------------------|
|SDL File Type Support Framework     |  Trados SDL File Type Support Framework 2 API      | Trados>Filter Framework 1.0
| Source  | Refers to the source language, i.e. the language the localizable material is translated from      | 	Original|
| Target  | Refers to a target language, i.e. one of the languages that is translated into      | 	Translated|
| Native  | 	Content in a single language (source or target), normally before or after it is being localized.     | 	monolingual; original file type|
| Bilingual  | 	Content as it is being processed during localization, normally in source and target language pairs.    | 	TRADOStag, XLIFF, Workbench RTF|
| Text  | 	Localizable text content.    | 	|
| Tag  |Content that must not be changed during localization. Tags are protected from accidental modification while editing localizable content in tools such as TagEditor or SDLEdit.      | 	|
| Inline Tag	  |	A tag that appears inside localizable content and that may need to be placed in a different location as part of the localization effort. Examples of elements that are represented as inline tags include links, local formatting overrides, embedded graphics, etc.| 	Internal Tag, tw4winInternal|
| Structure Tag  | A tag that appears outside of the localizable content. These tags typically represent structural information (thus the name) in the document, such as paragraphs, tables, headers, footers, etc.| 	External Tag, tw4winExternal|
| (Inline) Placeholder Tag  | An inline tag that can be treated as a stand-alone element. Examples include index markers, inline graphics, etc.    | 	|
| (Inline) Start Tag  | An inline tag that together with a corresponding end tag enclose a localizable string. | 	|
| (Inline) End Tag  | An inline tag that together with a corresponding start tag enclose a localizable string.   | 	|
| Paragraph Unit  | A piece of bilingual content that can be translated as one continous content unit during localization. This is typically a single paragraph, heading, table cell, etc. It may have both a source and a target language component (or only a source component before it has been translated).  | 	(no real equivalent in TRADOS or SDLX); trans-unit in XLIFF|
| Segment  | A piece of source or target language content inside a paragraph unit that can be treated as one unit when being processed with a localization tool such as a translation memory. A segment is in most cases a sentence.  | 	Comparable to the source or target segment of a translation unit.|
| Extract  | The process of extracting localizable content from native source language files, typically to produce bilingual files for translation.      | 		Convert, Forward Convert, to Filter|
| Generate  | The process of converting translated bilingual files into native target language files.      | 	Back-convert, Save Target As, Clean-up|
| Semi-WYSIWYG Formatting  | Display properties of the text that help translators to better visualize the layout of the document that is being translated. The semi-WYSIWYG formatting information is purely informational, it carries no actual formatting that would affect the translated files. The formatting information that applies to the actual document in its native format is only contained in the tags.    | 	<df> elements in TRADOStag, formatting in TagEditor|
