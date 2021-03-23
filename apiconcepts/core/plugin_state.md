Plug-in State
====
This section explains how to remember the state (enabled or disabled) of plug-ins.

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.


Plug-in State
-----
Plug-ins can be enabled and disabled, for instance through a plug-in management UI. The state of the plug-ins is saved in the plug-in cache, represented by the `IPluginCache` interface. There is a default implementation of this interface, which saves plug-in state to an xml file (`XmlPluginCache`).

When an extension is disabled or enabled, the corresponding extension point is notified through an event (`IExtensionPoint.EnabledExtensionsChanged`). Depending on the implementation of the extension point, it can react to this event and dynamically deactivate or activate the corresponding extension implementation.

The implementer of the extension point needs to indicate whether the extension point implementation has this ability using the Type property of the `ExtensionPointInfo` attribute. This property can be set to two possible values:

* Dynamic: extensions for this extension point can be enabled and disabled dynamically.
* Static: the extension point requires that the application is restarted before the changes take effect.