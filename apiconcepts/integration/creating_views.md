# Creating Views

The Var:ProductName Integration API allows developers to integrate custom UI views into Studio desktop applications.

## Basic View

To create a basic view, complete the following steps:

* Create a Windows Forms user control to define the view's content
* Create a controller class that implements the [AbstractViewController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewController.yml) class
* Apply the [ViewAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewAttribute.yml) attribute to the controller class to specify view metadata

See [Integrating view sample](integrating_views.md) for an example.

## View with View Parts

To create a view that supports view parts, complete the following steps:

* Create a controller class that implements the [AbstractViewController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewController.yml) class
* Apply the [ViewAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewAttribute.yml) attribute to the controller class to specify view metadata
* Set the `AllowViewParts` property to `true`

See [Integrating viewparts](integrating_viewparts.md) for more information.
