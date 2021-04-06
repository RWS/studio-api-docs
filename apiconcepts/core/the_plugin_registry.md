The Plug-in Registry
====
This section explains how to use the plug-in registry and how to customize its initialization.


The Plug-in Registry
----
The [IPluginRegistry](../../api/core/Sdl.Core.PluginFramework.IPluginRegistry.yml) object is the main object that can be used by the host application or component is order to access and instantiate the available plug-ins.

The [PluginManager](../../api/core/Sdl.Core.PluginFramework.PluginManager.yml) static class, which is the entry point to the plug-in framework object model, provides various ways to create instances of the [IPluginRegistry](../../api/core/Sdl.Core.PluginFramework.IPluginRegistry.yml) object. By default, plug-ins are installed in the applications installation directory and plug-in manifest and resource files are installed in a "plugins" subdirectory. In order to access these plugins, you can use the `DefaultPluginRegistry` property, which returns an [IPluginRegistry](../../api/core/Sdl.Core.PluginFramework.IPluginRegistry.yml) instance, that has been configured to load plug-ins from that default location.

If there is a requirement to customize the initialization of the plug-in registry, the user can create an instance of [IPluginRegistry](../../api/core/Sdl.Core.PluginFramework.IPluginRegistry.yml) through one of the factory methods on [PluginManager](../../api/core/Sdl.Core.PluginFramework.PluginManager.yml). In order to make this instance easily accessible to other parts of the application, it is recommended to set the `DefaultPluginRegistry` property to this instance.

> [!NOTE]
> 
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.