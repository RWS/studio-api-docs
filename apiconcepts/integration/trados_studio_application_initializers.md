Trados Studio application initializers
=====
Integration API makes it easy to execute code when Trados Studio starts, so you can initialize your plugin. All you need to do is implement `IApplicationInitializer` interface, from `Sdl.Desktop.IntegrationApi` namespace and decorate your class with `ApplicationInitializerAttribute`. If you fail to implement `IApplicationInitializer`, an exception will be thrown, but if you omit the `ApplicationInitializerAttribute`, your plugin will simply be ignored when Trados Studio Starts.

Example
----
The following example demonstrates a simple plugin data initialization. This plugin measures the time since Trados Studio has been opened and if you worked less than required, it displays a warning MessageBox.

This example also demonstrates how to cleanup when Trados Studio exits. All you have to do is subscribe to `Application.Closing` event. You can use `CancelEventArgs` to prevent Trados Studio from closing.