Creating views
===
Var:ProductName Integration API provides support for the third-party developers to integrate UI views inside the Studio desktop applications.

Creating a new view
---

In order to create a new view, a third-party developer will require the following steps:

* Create a windows form user control to define the content of the view
* Create a controller class for the view, which must implement the [AbstractViewController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewController.yml) class
* Decorate the controller class with the [ViewAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewAttribute.yml) attribute to provide metadata informations regarding the view

[Integrating view sample](integrating_views.md)

Creating a new view which allows view parts
---
In order to create a new view which allows viewparts, a third-party developer will require the following steps:

* Create a controller class for the view, which must implement the [AbstractViewController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewController.yml) class
* Decorate the controller class with the [ViewAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewAttribute.yml) attribute to provide metadata informations regarding the view
* Set the boolean property `AllowViewParts` to `true`


For more detailed information on how to create a view which contains viewparts, see the topic [Integrating viewparts](integrating_viewparts.md).
