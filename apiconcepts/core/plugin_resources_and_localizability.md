Plug-in Resources and Localizability
======
This section explains how to make extension meta-data localizable.

Plug-in Resources and Localizability
-----
All the string metadata values used for the properties of the extension attributes shown in the example above are hard-coded strings. In reality these strings need to be localized.

For that reason, the framework allows adding a specific plug-in resource file to a plug-in project. Instead of hard-coding the strings in code, you can specify the key of a resource string defined in the plug-in resources file. The framework resolves these resource strings at runtime based on the current UI culture and automatically populates the string properties of extension attributes with the corresponding localized values.

To indicate that the value of a certain property needs to be retrieved from the plug-in resources file, the definition of that property within the extension attribute definition must be annotated with the `PluginResource` attribute. For instance, a plug-in button extension point, with a localizable `ToolTipText` property becomes:

# [C#](#tab/tabid-1)
[!code-csharp[AdvancedPluginFramework](code_samples/AdvancedPluginFramework.cs#L103-L113)]
***

An extension targeting this extension point, `MyPluginButton`:

# [C#](#tab/tabid-2)
[!code-csharp[AdvancedPluginFramework](code_samples/AdvancedPluginFramework.cs#L46-L56)]
***

, where `MyPluginButton_Name` and `MyPluginButton_ToolTipText` are keys of strings defined in the plugin resource file, `PluginResources.resx`.
As mentioned before, the plug-in manifest generator requires that you pass the path to the plug-in resx file as the second parameter. To avoid having to load the plug-in assembly to get access to the embedded resource strings when the plug-in meta data is accessed, the plug-in manifest generator compiles an external .Net plug-in resource file. This compiled resource file is copied alongside the plug-in manifest file, so that it can be loaded separately from the plug-in assembly itself. For instance, for the plug-in assembly, PluginLibrary, this becomes:

* `PluginLibrary.dll`: the plug-in assembly
* `PluginLibrary.plugin.xml`: the plug-in manifest file
* `PluginLibrary.plugin.resources`: the plug-in resources file containing strings for the neutral culture.
* `PluginLibrary.plugin.*.resources`: any number of resource files containing the localized strings
  
The use of the `PluginResource` attribute is supported in the following cases:

* Extension attribute string properties
* Auxiliary extension attribute string properties
* Assembly-level `PluginAttribute` string attributes
For non-string properties, like for instance an icon, the plug-in resource file can also be used, however, because of the type difference, the value of these kinds of properties cannot be automatically resolved. Define the actual property as a string property, to which uses must assign a resource key. Then, when you need the actual icon, get it from the plug-in resource file using the `GetPluginResource<T>` method:

# [C#](#tab/tabid-3)
[!code-csharp[AdvancedPluginFramework](code_samples/AdvancedPluginFramework.cs#L16-L17)]
***

> [!NOTE]
> 
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.