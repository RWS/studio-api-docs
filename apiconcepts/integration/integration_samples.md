Studio application initializers
====
Integration API makes easy to execute code when <Var:ProductName> starts so you can initialize your plugin. All you need to do is implement [IApplicationInitializer](../../api/integration/Sdl.Desktop.IntegrationApi.IApplicationInitializer.yml) interface, from `Sdl.Desktop.IntegrationApi` namespace and decorate your class with ApplicationInitializerAttribute. If you fail to implement [IApplicationInitializer](../../api/integration/Sdl.Desktop.IntegrationApi.IApplicationInitializer.yml) an exception will be thrown, but if you omit the [ApplicationInitializerAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ApplicationInitializerAttribute.yml) your plugin will simply be ignored when <Var:ProductName> Starts.

Example
----
The following example demonstrates a simple plugin data initialization. This plugin measures the time since <Var:ProductName> has been opened and if you worked less than required displays a warning MessageBox.

This example also demonstrates how to cleanup when <Var:ProductName> exits. All you have to do is subscribe to `Application.Closing` event. You can use `CancelEventArgs` to prevent <Var:ProductName> from closing.