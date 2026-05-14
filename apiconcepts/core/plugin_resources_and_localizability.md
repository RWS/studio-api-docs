# Plug-in Resources and Localizability
This section explains how to make extension meta-data localizable.

## Overview
All string metadata values used for the properties of extension attributes in the example above are hard-coded strings. These strings need to be localized.

To achieve this, the framework allows adding a specific plug-in resource file to a plug-in project. Instead of hard-coding strings in code, you can specify the key of a resource string defined in the plug-in resource file. The framework resolves these resource strings at runtime based on the current UI culture and automatically populates the string properties of extension attributes with the corresponding localized values.

### Using the `PluginResource` Attribute
To indicate that a property's value needs to be retrieved from the plug-in resource file, annotate the property within the extension attribute definition with the `PluginResource` attribute. For example, a plug-in button extension point with a localizable `ToolTipText` property becomes:

# [C#](#tab/tabid-1)
[!code-csharp[AdvancedPluginFramework](code_samples/AdvancedPluginFramework.cs#L25-L34)]
***
<br>

An extension targeting this extension point, `MyPluginButton`, is defined as:

# [C#](#tab/tabid-2)
[!code-csharp[AdvancedPluginFramework](code_samples/AdvancedPluginFramework.cs#L40-L50)]
***
<br>

In the above code sample, `MyPluginButton_Name` and `MyPluginButton_ToolTipText` are keys of strings defined in the plug-in resource file, `PluginResources.resx`.
The plug-in manifest generator requires the path to the plug-in `.resx` file as the second parameter. To avoid loading the plug-in assembly to access embedded resource strings when accessing plug-in metadata, the manifest generator compiles an external .NET plug-in resource file. This compiled resource file is copied alongside the plug-in manifest file, allowing it to be loaded separately from the plug-in assembly. For example, for the plug-in assembly `PluginLibrary`, the following files are generated:

* `PluginLibrary.dll`: the plug-in assembly
* `PluginLibrary.plugin.xml`: the plug-in manifest file
* `PluginLibrary.plugin.resources`: the plug-in resources file containing strings for the neutral culture.
* `PluginLibrary.plugin.*.resources`: any number of resource files containing the localized strings
  
### Supported Use Cases
The `PluginResource` attribute can be used in the following cases:

* Extension attribute string properties
* Auxiliary extension attribute string properties
* Assembly-level `PluginAttribute` string attributes

### Non-String Properties
For non-string properties, such as an icon, the plug-in resource file can also be used. However, due to type differences, these properties cannot be automatically resolved. Define the property as a string property, and assign a resource key to it. When the actual icon is needed, retrieve it from the plug-in resource file using the `GetPluginResource<T>` method:

# [C#](#tab/tabid-3)
[!code-csharp[AdvancedPluginFramework](code_samples/AdvancedPluginFramework.cs#L16-L17)]
***
