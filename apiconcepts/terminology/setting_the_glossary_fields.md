Setting the Glossary Fields
====
Apart from the actual source and target terms, a terminology source can also contain additional information such as a definition, a note or an example. Such additional information is referred to as 'descriptive fields'.

How to declare the Definition Field
-----
Our glossary text file foresees a definition text in the forth column:

*1;photo printer;Fotodrucker;Peripheral device for creating hardcopies of pictures.*

Such information can also be shown when looking up terminology in Var:ProductName. When the descriptive field is declared, it can be selected in the Var:ProductName UI:

<img style="display:block; " src="images/select_fields.jpg" />

If you select to display the field, the content (the definition) is shown alongside the search results:

<img style="display:block; " src="images/search_results_with_definition.jpg" />

Go to the **MyTerminologyProvider.cs** class and add the following member. It creates a descriptive field labelled 'Definition', which - in turn - can contain a text string. The example field has the field level 'Entry', i.e. it is not specific to a particular term or language. Instead, in the entry structure, it applies to the whole entry.

# [Setting the Glossary Descriptive Field](#tab/tabid-1)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L43-L59)]
***

This function then needs to be called from the following property, which also calls the **GetLanguages()** method to create the full terminology source definition. The terminology source definition then becomes exposed in the Var:ProductName UI, where you can select what fields to display (see screenshot above).

# [Retrieving the Glossary Languages](#tab/tabid-2)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L31-L39)]
***
