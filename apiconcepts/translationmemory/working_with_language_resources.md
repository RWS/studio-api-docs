Working with Language Resources
=====
This section describes how to work with language resources within translation memories and fields templates.

Language Resources
------
Translation memories have support for storing custom language resources, such as segmentation rules, abbreviations, ordinal followers, variables, dates, times, numbers, measurements and currency recognizers. Storing these resources in the translation memory ensures that they are consistenly being used throughout translation or when segmenting content that is targeting the translation memory. This ensures consistency and optimal translation re-use.

The language resources within a translation memory or language resources template are represented by a [LanguageResourceBundleCollection](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.LanguageResourceBundleCollection.yml). This is a collection of [LanguageResourceBundle](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.LanguageResourceBundle.yml) objects, each of which represents language resources for a specific language. A translation memory can store language resources for its source language or languages.

Server-based translation memories have the ability to inherit their language resources from a language resources template ([ServerBasedLanguageResourcesTemplate](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml)). When that is the case, the field definitions can be centrally managed via the language resources template, but not on the translation memory itself. For more information, see [Working with Language Resource Templates](working_with_language_resource_templates.md).

There are nine types of customizable language resources:

1. **Abbreviations**: The segmentation engine uses a default source language abbreviation list when segmenting text. The abbreviation list contains the most common, frequently used abbreviations in the source language, such as max., min. and usd., and all of the abbreviations are followed by a full stop. If the segmentation engine finds a full stop preceded by text, and the text appears in the abbreviation list, the engine interprets the full stop as an abbreviation punctuation mark and keeps reading until the next full stop. The abbreviations list can be manipulated through a Wordlist object.

2. **Ordinal followers**: The segmentation engine uses a default source language ordinal follower list when segmenting text. An ordinal follower, or ordinal noun, is a word that is typically found following a number. The ordinal nouns list allows the segmentation engine to read past instances of the full stop that are preceded by a number and followed by an ordinal noun. This aspect of segmentation is not relevant for all languages, but it is particularly useful for the German language. A dot is not interpreted as a segment boundary marker if it is preceded by a number and followed by an ordinal noun. The ordinal followers list can be manipulated through a Wordlist object.

3. **Segmentation rules**: Segmentation rules are rules that govern how text is broken up into segments. Segments can be paragraph or sentence length. As a general rule, in Englsih, a full stop, exclamation mark, question mark, colon, tab character or end of paragrah mark indicate the end of a sentence when they are followed by a space. A quotation mark (" or '") or a closing parenthesis ()) may follow the closing punctuation mark and precede the space. A semicolon (;) does not end a sentence. The segmentation rules can be customized via the SegmentationRules object.

4. **Variables**: This is a customized list of variables in the translation memory setup. Each item on the list is treated as a non-translatable (placeable) element. For example, you may not want to translate a company name. Adding it to the variable list identifies this as untranslatable content. The properties of the variable list are as follows:
   * Variables must appear exactly as they do in your documents.
   * Each item in the list must be on a line of its own.
   * Punctuation inside variables, such as hyphens or commas, is not supported.
   * As well as the variables themselves, the list can include comments or headings. Comments or headings must be directly preceded by the hash symbol (#), for example, #comment.
   
   Similar to the abbreviations and ordinal followers, variables are as well manipulated through a Wordlist object.

5. **Date recognizers**: (see below)

6. **Time recognizers**: The formats of both dates and times can differ from one language to another, so the date and time recognizers offer the possibility to customise these formats to best suit the needs of the current source and target languages. Both of them are represented as lists of strings. 

7. **Numbers recognizers**: By default, there is a list of number formats for each source and target language. This ensures that the number formats in the source language are recognized and localized correctly in the target language. The numbers recognizers are represented as lists of SeparatorCombination objects.

8. **Measurements recognizers**: Apart from the default measurements for source and target languages, new or customised ones can also be added which will be identified as placeables in the process of translation. The measurements recognizers are represented as dictionaries of string keys and CustomUnitDefinition values.

9. **Currency recognizers**: Similar to all the recognizer types above, <Var:ProductName> uses a default list of currency formats (symbols and symbol placements) for each source and target language. Expanding this list ensures that the currency formats of the source language are recognized and localized correctly in the target language. The currency recognizers can be manipulated through a list of the CurrencyFormat objects.

<img style="display:block; " src="images/Cd-LanguageResources.png"/>

In order to define custom language resources for a given language, you have to create a [LanguageResourceBundle](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.LanguageResourceBundle.yml) object for that language. Subsequently for instance, create a new Wordlist object, populated using a custom list of variables and assign it to the [Abbreviations](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.LanguageResourceBundle.yml#Sdl_LanguagePlatform_TranslationMemoryApi_LanguageResourceBundle_Abbreviations) property. Finally add it to the language resource bundles collection and save the translation memory or language resources template that own the collection in order to save the changes.

Often, when defining custom language resources, the most common scenario is making changes to the default set of language resources. The default language resources can be retrieved from the [DefaultLanguageResourceProvider](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.DefaultLanguageResourceProvider.yml), or by calling [GetDefaultLanguageResources](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.DefaultLanguageResourceProvider.yml#Sdl_LanguagePlatform_TranslationMemoryApi_DefaultLanguageResourceProvider_GetDefaultLanguageResources_System_Globalization_CultureInfo_) in a server scenario. Note that this is only really required when accessing the default language resources from Silverlight, since the [DefaultLanguageResourceProvider](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.DefaultLanguageResourceProvider.yml) is not available in Silverlight. Both methods returns the same defaults, using the [DefaultLanguageResourceProvider](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.DefaultLanguageResourceProvider.yml) is more efficient since no server roundtrips are required to retrieve the default language resources.

See Also
--------
[Adding Language Resources](adding_language_resources.md)

[Working with Language Resource Templates](working_with_language_resource_templates.md)