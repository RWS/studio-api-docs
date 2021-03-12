Implementing the Search Logic
=======
The actual search functionality is implemented in a separate component. The plug-in template contains a class called **MyTranslationProviderLanguageDirection**, which - in our sample implementation - we will re-name to **ListTranslationProviderLanguageDirection**. This class implements the ITranslationProviderLanguageDirection interface, which contains numerous members. In this chapter we will focus on the members that are actually used for implementing the functionality of our plug-in. Members used for updating and adding translation units are not required for our implementation.

Implement the Private Class Members
-------
The class that hosts the logic for the actual search functionality requires access to, for example, the plug-in options, as it needs to 'know' the name and path of the list file as well as the delimiter. This is done by creating an object based on the **ListTranslationOptions** class (see Storing and Retrieving the Plug-in Settings).

Also, the **ListTranslationProviderElementVisitor** helper class (which we will implement in the next chapter, see Implementing the Element Visitor) provides functionality necessary for making sure that we retrieve the required plain segment text information for carrying out the search in the delimited text lists.

Also, we declare a _listOfTranslations object based on the Dictionary class. We will later load the content of the delimited list file into this object. At the beginning of our class we therefore declare the following private members:

These members are then instantiated in the ListTranslationProviderLanguageDirection constructor method, which also creates the _listOfTranslations object to hold the segment pair collection:

In the next step we open the list file. Note that the file name and path is provided through the plug-in options, as is the delimiter character. The first line, which holds the language direction information should not be put into the collection, which is why before looping through the segment pairs we apply the ReadLine method, so that this line is skipped.
Within the loop we split the following lines into segment pairs. If the split is successful and generates a source and a target segment, we add the pair to the collection.
The complete method should now look as shown below:


Search the Current Segment
-----
A segment lookup is (by default) triggered in Trados Studio 2017 when the user moves the cursor into a particular target cell. This will search for a match for the current source segment in the selected translation provider source. The screenshot below shows two source segments as they are displayed in the editor of Trados Studio 2017 and the empty target cells next to them:

<img style="display:block; " src="images/Segments.jpg"/>

When moving into a target cell the SearchSegment method of the ITranslationProviderLanguageDirection interface is invoked. This method takes the current segment object and the search settings as parameters. Search settings include, for example, the maximum number of matches that a search should return. These settings can be configured by the user of Trados Studio 2017 at runtime. The screenshot below shows the search settings that users can configure. If a user, for example, limits the number of concordance matches that a translation provider should return to 5, then the this is the maximum number of concordance hits that your provider will return. There is no need for you to implement the logic for applying the settings yourself.
<img style="display:block; " src="images/SearchSettings.jpg"/>
> [!NOTE]
> Depending on your translation provider, some of the search settings that Trados Studio 2017 offers may or may not be relevant for your implementation. For example, since our provider will only support exact matches, the **Minimum match value** setting will not be relevant for the delimited list translation provider, as our logic will only implement searches for 100% matches.

Also, our implementation only support plain text searches. The segment pairs in the delimited list files are plain text, i.e. they do not contain any text., it is likely that a document that is opened in SDL Trados Studio 2017 contains inline tags, e.g. for character formatting as shown in the example below:

<img style="display:block; " src="images/NotPlainText.jpg"/>

We therefore loop through all elements in the current segment and make sure that only plain text is returned. Note that the functionality for returning only the plain source segment text is implemented in a separate component, which we will add later (see Implementing the Element Visitor).

Create the Search Results Object
------
In the next step we create an object based on the **SearchResults** class. This object holds the results of our search, which we will fill with the information found in our list translation provider (if any). A search result usually consists of the source and the target segments that were found in the translation provider (and potentially further information data such as context information, TM fields - e.g. Client, Project -, which, however will not be the case for our simplified implementation).

Therefore **SourceSegment** property, which we set to the current segment as shown below:

Note that we apply the Duplicate method, which creates an instance that is a deep copy of the segment object instance.

The Search Types Supported by the Plug-in
------
Remember that our plug-in needs to implement the following search types:

* (source) segment lookup
* source concordance search
* target concordance search
A concordance search can be triggered by, for example, selecting a source or target string in the editor and by pressing **F3**. The type of search that was called by the user from the editor can be determined using the **SearchMode** property, whose value can be retrieved through the search settings object.

Implement the Segment Lookup
-------
The following code snippet shows how we handle the normal segment lookup. If the search mode equals **NormalSearch**, and if a source segment match is found for the plain text version of the current source segment from the editor, then a target segment object should be created using the target segment string (i.e. the key value) from our segment pair collection. The current source segment and the target segment will be used to construct a search result, which will be displayed in the **Translation Results** window of Trados Studio 2017. The result will be generated through a separate CreateSearchResult helper function, which we will implement later.

Implement the Concordance Search
-------
In the same manner we implement the source/target concordance search: If the search mode equals **ConcordanceSearch**, we loop through all the items in our segment pair collection. If items contain the search string, then a search result shall be generated for each matching segment. Note that for concordance searches we can have multiple hits.

For concordance searches, the search segment returned from the editor will usually not be an entire segment, but part of a segment, i.e. the string that the user selected for the concordance search. Note that in our implementation we apply ToLower to the search string and the hit from our segment pair collection, which makes the concordance search case-insensitive:

The target concordance search works almost the same way. The only difference is that we check for the search mode **TargetConcordanceSearch** and whether the current search string is contained in any of the target segments from the collection:

At the end of the loop we return the search result:

The complete function should look as shown below:

Generate the Search Result Translation Unit
------
In the next step, implement the CreateSearchResult helper function, which generates the search result if a match has been found in the translation list provider.

The search result that we construct is basically a translation unit with the following information:

* The source/target segment from the translation provider
* The match value
* The confirmation status, i.e. **Draft** or **Translated**
* Potentially a formatting/tag penalty

It takes the source segment (searchSegment), the target segment (translation) as segment parameters. In addition it takes the source segment as it was found in the list file as string parameter. The reason for using this additional parameter in our implementation: When finding a match during a normal source segment lookup, the source segment from the list file will be identical to the source segment from the document that was looked up. In this case, there would be no need for this additional string parameter, as the source segment could be already retrieved from the searchSegment parameter.
However, for a concordance search, the search segment would not be the whole segment, but only part of it. But since we want to display the full segment in the **Concordance** window, and not only the concordance search string, we provide the full string as it was found in the list file as an additional string parameter.

Another parameter that we use is the formattingPenalty, which is a boolean value. This parameter communicates to the function whether the original segment contained any inline tags (segment.HasTags). It may happen that an original source segment inline character formatting or any other tags. Our simple list provider only provides plain text, i.e. it does not insert any kind of tags or formatting information. This could lead to the fact that a segment with tags is translated by using a plain text target segment. Applying a slightly lower than 100% match value and a draft confirmation status alerts the translator to the fact that the suggested translation might require some editing. The translator then has to decide whether to apply the tags/formatting manually. Based on this parameter we lower match the match value for the TU to 99%. Also, we apply the confirmation level Draft instead of Translated.

> [!NOTE]
> For concordance searches this formatting penalty parameter is always set to False as in our implementation formatting is not relevant for concordance searches.

Below are some examples that illustrate how search results will be displayed in Trados Studio 2017:

The following result would be displayed in the **Translation Results** window when an exact (100%) match has been found in the delimited list:

<img style="display:block; " src="images/SearchResultNormalSearch.jpg"/>

The following result would be displayed in the **Concordance** window for the search string photo printer. Note that when doing a concordance search in a translation memory, the corresponding search string is highlighted in the matching segments. This is not done in our simplified implementation.

<img style="display:block; " src="images/SearchResultConcordance.jpg"/>

The above result is achieved by creating a translation unit based on the TranslationUnit class. To this tu object we apply the source and target segment as shown below. Remember that in order to make sure that the TU always shows the full source segment for concordance searches (instead of just the search string), we create a new Segment object based on the full source segment string as it is provided from the list file:

Next, we set some properties for the TU:

* The match value, which should be 100, but which might be reduced through a penalty to 99% in order to alert the user to the fact that tags/inline formatting information is missing.
* The **TranslationUnitOrigin**, which indicates that the translation came from. TU origins could be, for example, translation memories, machine translation providers, Context TM, or an alignment of existing documents. A delimited list is similar in nature to a very rudimentary translation memory, therefore we chose **TM** as the TU origin value. Note that information such as the name of the provider and the TU origin is stored in the SDL XLIFF document as illustrated in the screenshot below. The user can make such information visible in a tooltip.


<img style="display:block; " src="images/TuInfoTooltip.jpg"/>

* The **ConfirmationLevel**, which could be, for example, draft, signed-off, translated, etc. As an exact match from a delimited list file can be assumed to be quite reliable, we will give it the status **Translated**, or **Draft** if the suggested translation is missing tags. The confirmation status is displayed in the editor as shown in the above screenshot. The code snippet below shows how the draft status is applied to the TU:

* If the boolean formattingPenalty parameter equals True, the TU should also receive a formatting penalty of 1%. To do this you first have to create a penalty object based on the **Penalty** class. This penalty is then applied to the scoring result by using the **ApplyPenalty** method. This method takes the **PenaltyType** as parameter. Penalties can be applied for various reasons. In our example the cause of the penalty is the lacking formatting tags. Therefore, the applicable penalty type is **TagMismatch**. Another parameter is the 'malus', i.e. the percentage point(s) to subtract from the base score, which we set to 1 as shown in the code snippet below:

* When a formatting (i.e. tag) penalty is applied to a TU, it will be shown in Trados Studio 2017 as illustrated in the screenshot below:

<img style="display:block; " src="images/FormattingPenalty.jpg"/>

The complete function should now look as shown below: