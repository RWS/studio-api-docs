Extensions and extension points
====
<var:ProductName> offers a predefined set of extension points. Your plug-ins can add custom functionality to <var:ProductName> by defining extensions that target these extension points.

### Extensions
An extension is an individual unit of logic that provides functionality for <var:ProductName> through a specific extension point. 
You can create an extension by following the steps below:
1. Create a class in your plug-in assembly. 
2. Decorate the class with the attribute that identifies the extension point it is targeting. 
3. Implement the interface that is required by the extension point.

### Extension points
An extension point is a point in <var:ProductName> that allows adding extensions to it. 
You can define an extension point by following the steps below:  
1. Create a class in your plug-in assembly and derive it from the [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) base. 
2. Decorate the class with the [ExtensionPointInfoAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionPointInfoAttribute.yml) attribute.
3. Make sure that the class has a default parameterless constructor. This is needed because the plugin manifest generator uses XML serialization to save the attribute information. (TODO Remus: link to manifest generator?)
    
Other plug-in developers can create extensions for the extension points you define, by following the procedure described in Extensions (TODO: link) paragraph above.

The [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) class defines the properties that plug-in developers can provide with their extensions:
* *Id*: A unique id for the extension
* *Name*: A friendly name for the extension.
* *Description*: A description of the extension.
* *Icon*: An optional icon representing the extension.

The [ExtensionPointInfoAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionPointInfoAttribute.yml) class specifies the name and type of the extension point: 
* The name can be used by a plug-in manager UI to represent the extension point. 
* The type can be either static or dynamic, referring to whether this extension point allows enabling or disabling of one or more of it’s extensions without having to restart the application. (OLD: More on that later. | NEW: TODO Remus: can I link some relevant info here instead?)

### Auxiliary Extension Attributes
In some cases, you might need to add some extra metadata to a certain extension implementation, on top of what is defined in the extension attribute and it might be impractical to add these properties to the extension attribute itself.

An example of this is for instance a plug-in user action. The extension attribute for the action can define name, icon, tooltip etc, but you also need to specify on which menus, toolbars and context menus this user action will be available. For cases like this, the plug-in framework provides auxiliary extension attributes.

You can only apply one extension attribute to an extension implementation: this is the attribute that uniquely identifies the extension point the extension implementation is targeting. On top of this, you can decorate the implementation class with as many auxiliary extension attributes as you like.

An auxiliary extension attribute needs to derive from the [AuxiliaryExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.AuxiliaryExtensionAttribute.yml) base class. For instance, we can define a `ToolBarLocation` auxiliary attribute, which has a `ToolBarId` property that can be used to specify on which tool bar the action should appear. For menus, we can define a similar `MenuLocation` attribute:

# [C#](#tab/tabid-1)
[!code-csharp[AdvancedPluginFramework](./code_samples/AdvancedPluginFramework.cs#L62-L82)]
***

, and the plug-in action definition becomes:

# [C#](#tab/tabid-1)
[!code-csharp[AdvancedPluginFramework](./code_samples/AdvancedPluginFramework.cs#L88-L97)]
***

The collection of all auxiliary attributes for an extension can be retrieved using the `AuxiliaryExtensionAttributes` property.

### Compile-time extension validation
In order to catch as many developer errors as possible at compile-time, the plug-in framework provides a mechanism for extension point developers to validate extension definitions at compile-time.

Every [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) and [AuxiliaryExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.AuxiliaryExtensionAttribute.yml) has validation methods which are called during the build process. These methods then have the ability to report errors and warnings, which will be displayed in Visual Studio as standard compiler errors.

**Extension Attribute Validation**

The [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) type has a `Validate` method, which by default validates that the user has specified values for the `Id` and the `Name` property. When developing and extension point, this method can be overridden to perform additional validation.

**Auxiliary Extension Attribute Validation**

The [AuxiliaryExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.AuxiliaryExtensionAttribute.yml) type also has a `Validate` method, which by default doesn't do any special validation. Extension point developers can override this method to perform additional validation for an auxiliary extension attribute.


