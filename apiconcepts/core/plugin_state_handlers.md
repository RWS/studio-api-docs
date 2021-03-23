Plug-in State Handlers
====
This section explains how to control whether certain plug-ins can be enabled or disabled.

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.


Plug-in State Handlers
----
The `IPluginRegistry` object allows the user to specify a `PluginStateHandler`. If this handler is set, the plug-in framework uses it to determine whether plug-ins can be enabled or disabled.