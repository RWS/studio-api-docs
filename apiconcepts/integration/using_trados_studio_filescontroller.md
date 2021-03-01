Using Trados Studio FilesController
=====
TranslationStudioAutomation provides support for the third party developers to access the SDL Trados Studio files view and perform project files operations.

Creating a custom project files list inside a viewpart that belongs to the files view
-----
The following sample demonstrates how to create a custom project files list built by using the FilesController and add it to the files view as a custom viewpart.

The custom list features:

* Display two columns: the project file name and the number of total words from the file.
* The projects files selected in the project files list will be in sync with the one from the custom list.


Start by implementing the windows form user control that will fill the content of the viewpart and is implementing the list view.

Finally integrate the new viewpart to the files view.