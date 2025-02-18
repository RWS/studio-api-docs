Creating viewparts
====

Var:ProductName Integration API provides support for third-party developers to integrate UI viewparts inside the Var:ProductName desktop applications.

Creating viewparts for a custom view
----
In order to create view parts, a third-party developer will require the following steps:

* Create a view which allows viewparts
* Create one or more viewparts to define the main view content area
    * Create a windows form user control to define the content of the viewpart
    * Create a controller class that will define the main content area of the view which must implement the [AbstractViewPartController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewPartController.yml)
* Decorate the controller class with the following attributes to provide metadata information regarding the viewparts: 
    * [ViewPartAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartAttribute.yml)
    * [ViewPartLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartLayoutAttribute.yml)
* Target the viewpart location by setting the `LocationByType` property with the view controller type.
* Set the `Dock` property to `Fill` 
* Create the other viewparts as the content viewpart with the exception that the docking will not be of type `Fill`.
  
[ntegrating viewpart sample](integrating_viewparts.md)

Creating viewparts for Var:ProductName views
-----
In order to create viewparts and integrate them into the Var:ProductName views, the third-party developer will require the following steps:

* Create a windows form user control to define the content of the viewpart
* Create a viewpart controller class which must implement the `AbstractViewPartController`
* Decorate the controller class with the following attributes to provide metadata information regarding the viewparts: 
    * [ViewPartAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartAttribute.yml)
    * [ViewPartLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartLayoutAttribute.yml)
* Target the viewpart location by setting the `LocationByType` property with the one of the view controllers defined:
    * [ProjectsController](projects_controller.md)
    * [FilesController](files_controller.md)
    * [EditorController](editor_controller.md)
  
[Integrating viewpart sample](integrating_viewparts.md)
