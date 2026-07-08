# Plug-in Framework Overview
The Plug-in Framework enables applications and components to support plug-in functionality. Developers can define extension points and dynamically load plug-ins that provide targeted extensions using this framework.

## Version
This documentation covers the version of the Plug-in Framework released with `Var:ProductNameWithEdition`.

## Main Concepts and Definitions
This section outlines key concepts and definitions central to the Plug-in Framework.

### Hosting Application
The hosting application (or component) provides plug-in capabilities. It defines extension points and uses the Plug-in Framework to discover and load plug-ins and extensions for these points.

### Extension Point
An extension point is a location in the application or component that supports adding extensions. It is defined by a .NET attribute, called an extension attribute. All extension attributes derive from the [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) base class.

### Extension
An extension is a unit of logic that enhances an application or component through a specific extension point. It is defined by a .NET class within a plug-in assembly and annotated with the extension attribute identifying its target extension point. Typically, an extension class implements an interface required by the targeted extension point.

### Plug-in
A plug-in contains one or more extensions. It is a .NET assembly annotated with the assembly-level [PluginAttribute](../../api/core/Sdl.Core.PluginFramework.PluginAttribute.yml), which includes a Name property for the plug-in's friendly name. This attribute indicates to the framework that the assembly is a plug-in.

### Plug-in Registry
The plug-in registry ([IPluginRegistry](../../api/core/Sdl.Core.PluginFramework.IPluginRegistry.yml)) is the central object in the framework. It enables the host application to detect available plug-ins and extensions and create instances of the extensions provided by these plug-ins.

### Plug-in Manifest
A plug-in manifest is an XML file containing serialized metadata about a plug-in assembly. It includes:
- The plug-in name, assembly name, and version.
- A list of all extensions defined within the plug-in assembly, along with metadata specified in the extension attributes.
