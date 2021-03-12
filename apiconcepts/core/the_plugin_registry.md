The Plug-in Registry
====
This section explains how to use the plug-in registry and how to customize it's initialization.

The Plug-in Registry
----
The IPluginRegistry object is the main object that can be used by the host application or component is order to access and instantiate the available plug-ins.

The PluginManager static class, which is the entry point to the plug-in framework object model, provides various ways to create instances of the IPluginRegistry object. By default, plug-ins are installed in the applications installation directory and plug-in manifest and resource files are installed in a "plugins" subdirectory. In order to access these plugins, you can use the DefaultPluginRegistry property, which returns an IPluginRegistry instance, that has been configured to load plug-ins from that default location.

If there is a requirement to customize the initialization of the plug-in registry, the user can create an instance of IPluginRegistry through one of the factory methods on PluginManager. In order to make this instance easily accessible to other parts of the application, it is recommended to set the DefaultPluginRegistry property to this instance.