# Creating View Parts

The Var:ProductName Integration API allows developers to integrate custom UI view parts into Var:ProductName desktop applications.

## View Parts for Custom Views

To create custom view parts for a custom view, complete the following steps:

* Create a view that allows view parts
* Create one or more view parts to define the content area:
  * Create a Windows Forms user control for the view part content
  * Create a controller class that implements [AbstractViewPartController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewPartController.yml)
* Apply the following attributes to specify view part metadata:
  * [ViewPartAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartAttribute.yml)
  * [ViewPartLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartLayoutAttribute.yml)
* Set the `LocationByType` property to target the view controller
* Set the `Dock` property to `Fill` for the main content area
* Create additional view parts with appropriate docking (not `Fill`)

See [Integrating viewpart sample](integrating_viewparts.md) for an example.

## View Parts for Var:ProductName Views

To create view parts and integrate them into Var:ProductName views, complete the following steps:

* Create a Windows Forms user control for the view part content
* Create a view part controller class that implements `AbstractViewPartController`
* Apply the following attributes to specify view part metadata:
  * [ViewPartAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartAttribute.yml)
  * [ViewPartLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartLayoutAttribute.yml)
* Set the `LocationByType` property to target one of the following view controllers:
  * [ProjectsController](projects_controller.md)
  * [FilesController](files_controller.md)
  * [EditorController](editor_controller.md)

See [Integrating viewpart sample](integrating_viewparts.md) for an example.
