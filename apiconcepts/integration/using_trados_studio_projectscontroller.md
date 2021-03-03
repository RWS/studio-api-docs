Using Trados Studio ProjectsController
=====
TranslationStudioAutomation provides support for third-party developers to access the Trados Studio projects view and perform project operations.

Creating a custom project list inside a viewpart that belongs to the projects view
----
The following sample demonstrates how to create a custom project list built by using the ProjectsController and add it to the projects view as a custom viewpart.

The custom list features:

* Display two columns: the project name and the number of target language files contained in the project.
* The projects selected in the list will be selected in the projects view list too.
* Activating a selected project will open it.


Start by implementing the windows form user control that will fill the content of the viewpart and is implementing the list view.

Finally integrate the new viewpart to the projects view.