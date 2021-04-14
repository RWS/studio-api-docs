Create a <Var:ProductName> application initializer
====

<Var:ProductName> Integration API provides support for third-party developers to add specific initializations or functionalities at <Var:ProductName> application startup.

Creating a <Var:ProductName> application initializer
----
In order to create a <Var:ProductName> application initializer, a third-party developer will require the following steps:

* Create an initialization class which implements the [IApplicationInitializer](../../api/integration/Sdl.Desktop.IntegrationApi.IApplicationInitializer.yml) interface.
* Decorate the initialization class with the [ApplicationInitializerAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ApplicationInitializerAttribute.yml) attribute


[Creating a <Var:ProductName> application initializer Sample](trados_studio_application_initializers.md)