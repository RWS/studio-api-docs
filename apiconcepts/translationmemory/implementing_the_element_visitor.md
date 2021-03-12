Implementing the Element Visitor
====
In the previous chapter (see Implementing the Search Logic) we generate the search result based on the search segment / string from the document that is currently open in the editor of Trados Studio 2017. To make certain that the search string only consists of plain text, we add a helper class called **ListTranslationProviderElementVisitor**. ( This class is not included in the plug-in template.) This component is used to loop through all the elements of a given segment (e.g. text, inline tags, etc), and returns the plain source segment string that will be used for the search and matching to the segments from the delimited list.

The class needs to implement the ISegmentElementVisitor interface.

The complete class looks as shown below: