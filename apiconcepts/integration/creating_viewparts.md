Creating viewparts
====

Trados Studio Integration API provides support for third-party developers to integrate UI viewparts inside the Trados Studio desktop applications.

Creating viewparts for a custom view
----
In order to create view parts, a third-party developer will require the following steps:

* Create a view which allows viewparts
* Create one or more viewparts to define the main view content area
    * Create a windows form user control to define the content of the viewpart
    * Create a controller class that will define the main content area of the view which must implement the `AbstractViewPartController`
* Decorate the controller class with the following attributes to provide metadata information regarding the viewparts: 
  
      * ViewPartAttribute
      * ViewPartLayoutAttribute
    * Target the viewpart location by setting the `LocationByType` property with the view controller type.
    * Set the `Dock` property to `Fill`
  
* Create the other viewparts as the content viewpart with the exception that the docking will not be of type `Fill`.
  
Integrating viewpart sample

Creating viewparts for Trados Studio views
-----
In order to create viewparts and integrate them into the Trados Studio views, the third-party developer will require the following steps:

* Create a windows form user control to define the content of the viewpart
* Create a viewpart controller class which must implement the `AbstractViewPartController`
* Decorate the controller class with the following attributes to provide metadata information regarding the viewparts: 
    * `ViewPartAttribute`
    * `ViewPartLayoutAttribute`
* Target the viewpart location by setting the `LocationByType` property with the one of the view controllers defined:
    * ProjectsController
    * FilesController
    * EditorController
  
Integrating viewpart sample