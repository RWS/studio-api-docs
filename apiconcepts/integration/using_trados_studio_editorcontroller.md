Using Trados Studio EditorController
=====
TranslationStudioAutomation provides support for the third party developers to access the Trados Studio editor view and perform editor operations.

Creating a custom document list inside a viewpart that belongs to the editor view
----
The following sample demonstrates how to create a custom documents list built by using the EditorController and add it to the editor view as a custom viewpart.

The custom list features:

* Display the following columns:
    * The document file name
    * Number of segments to be translated
    * Source language
    * Target language

Activating the document list items will active the document in the editor too.
The active document will always be displayed as selected in the list.

Start by implementing the windows form user control that will fill the content of the viewpart and is implementing the list view.

Finally integrate the new viewpart to the editor view.

Performing operations over document selections
----
The following sample demonstrates how to retrieve and replace the current document selection.