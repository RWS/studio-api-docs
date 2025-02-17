Searching Terms
====
Searching the terminology source through your term provider is arguably the most important function of your implementation. A term provider can still be useful without the editing function, but is of no use if no term look-up and term recognition functionality is available.

How to search and recognize terms
----

When you click inside a segment in Var:ProductName, the segment is scanned for any terminology contained in the term resource. If terms are recognized, they are highlighted with a red line and displayed in the **Term Recognition** window:

<img style="display:block; " src="images/term_recognition.jpg" />

You can also manually look up terms in the **Termbase Search** window:

<img style="display:block; " src="images/term_search.jpg" />

How to implement the search functionality
-----
Open the **MyTerminologyProvider.cs** class and go to the **Search()** function (which is implemented by the **AbstractTerminologyProvider** interface). This function is called when you click inside a segment or launch a search in the Termbase Search window. Note that Var:ProductName passes a number of useful parameters:

* **text**: the search string. When the user selects a segment in Var:ProductName, this will be the source segment text. When you launch a search manually, this will be the search string entered by you.
* **maxResultsCount**: the maximum number of hits to show, i.e. the search depth.
* **mode**: the search mode, i.e. fuzzy or normal. When selecting a segment in Var:ProductName, the mode is 'fuzzy' by default. When you do a manual lookup, the search mode is 'normal' by default (unless you clicked the **Fuzzy Search** button in the **Termbase Search**) window.
* **targetRequired**: boolean parameter that decides whether source terms without any target term should be shown or not. This parameter is controlled by you through the Var:ProductName UI. Depending on the value returned, your implementation needs to decide whether an entry without a target term should be displayed or not.

Below is the page of the Var:ProductName UI where the user can, for example, determine the search depth or whether to display entries without a target term:

<img style="display:block; " src="images/search_settings.jpg" />

The **Search** function returns a list of results to be displayed in Var:ProductName. In our simplified implementation, we loop through the glossary text file and look for any matching source terms. We assume that when the search mode is 'normal', we just check whether there are any source terms in the glossary that start with the search string. If the search string is fuzzy, we check whether there are any source terms in the glossary that are contained in the search string (i.e. in the segment). Of course, your 'real-life' implementation might have a much more sophisticated way of performing fuzzy searches than the **StartsWith()** or **Contains()** methods. For each search result object, you set the following properties:

* **Text**: the source term, this will also be the term that is going to be highlighted with a red line in Var:ProductName.
* **Score**: the fuzzy score. In our simple implementation, we will simply set it to 100%. Depending on how you implement your fuzzy logic, you may assign different values.
* **Id**: the id of the entry that the search result needs to be associated with. Note that in Var:ProductName, you do not output the search result list, but rather the entries that you need to construct in a different function (see below). The search result objects are linked to the entries using the unique ID. This is also the reason why, in our implementation, every line in the glossary file is preceded with a unique number.
  
The **Search()** function of our implementation would look as shown below:

# [The Search Function](#tab/tabid-1)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L142-L197)]
***

As mentioned above, the results list needs to be associated with the corresponding entry that you construct in the following helper function:

# [Constructing the Entry Content](#tab/tabid-2)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L201-L237)]
***

Make sure that you add the following entry list object to the terminology provider class:

# [The Entry List Object](#tab/tabid-3)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L13-L15)]
***

Besides the **Search()** method, the term provider interface also calls the **GetEntry()** method, which then outputs the entry with the corresponding entry based on the id parameter:

# [Getting the Entries](#tab/tabid-4)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L90-L95)]
***
