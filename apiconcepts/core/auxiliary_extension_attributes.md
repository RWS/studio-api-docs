Auxiliary Extension Attributes
======
This section explains how to allow an extension to provide additional metadata, on top of the properties provided with the extension attribute.

Auxiliary Extension Attributes
----
In some cases, you might need to add some extra metadata to a certain extension implementation, on top of what is defined in the extension attribute and it might be impractical to add these properties to the extension attribute itself.

An example of this is for instance a plug-in user action. The extension attribute for that can define name, icon, tooltip etc, but you also need to specify on which menus, toolbars and context menus this user action will be available. For cases like this, the plug-in framework provides auxiliary extension attributes.

You can only apply one extension attribute to an extension implementation: this is the attribute that uniquely identifies the extension point the extension implementation targets. On top of this, you can annotate the implementation class with as many auxiliary extension attributes as you like.

An auxiliary extension attribute needs to derive from the [AuxiliaryExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.AuxiliaryExtensionAttribute.yml) base class. For instance, we can define a `ToolBarLocation` auxiliary attribute, which has a `ToolBarId` property that can be used to specify on which tool bar the action should appear. For menus, we can define a similar `MenuLocation` attribute:

# [C#](#tab/tabid-1)
[!code-csharp[AdvancedPluginFramework](code_samples/AdvancedPluginFramework.cs#L62-L82)]
***

, and the plug-in action definition becomes:

# [C#](#tab/tabid-1)
[!code-csharp[AdvancedPluginFramework](code_samples/AdvancedPluginFramework.cs#L88-L97)]
***

The collection of all auxiliary attributes for an extension can be retrieved using the `AuxiliaryExtensionAttributes` property.

> [!NOTE]
> 
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.