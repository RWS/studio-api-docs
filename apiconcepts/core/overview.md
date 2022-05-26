Plug-in Framework Overview
====
This help system describes the Plug-in Framework. This framework offers plug-in functionality to applications and components. Application developers can define extension points and dynamically load any plug-ins providing extensions targeting these extension points using the plug-in framework.

Version
---
The version of the Plug-in Framework documented here is the one that was released with <Var:ProductNameWithEdition>.

Main Concepts and Definitions
----
This section introduces a number of concepts and definitions that are central to the plug-in framework.

* **Hosting Application** <br>
  This is the application (or component) which provides plug-in capabilities. The hosting application will define certain extension points and will use the plug-in framework to discover and load plug-ins and extensions for these extension points.<br><br>
* **Extension Point** <br>
  An extension point is a point in the application or component that allows adding extensions to it. An extension point is defined by a .Net attribute, called an extension attribute. All extension attributes should derive from the [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) base class.<br><br>
* **Extension** <br>
  An extension is an individual unit of logic that adds functionality to an application or component through a specific extension point. An extension is defined by a .Net class, defined in a plug-in assembly, annotated with the extension attribute that identifies the extension point it targets. An extension class typically implements an interface that is required by the extension point it is targeting.<br><br>
* **Plug-in** <br>
  A plug-in can contain one or more extensions. A plug-in consist is a .Net assembly which is annotated with the assembly-level [PluginAttribute](../../api/core/Sdl.Core.PluginFramework.PluginAttribute.yml), which has a Name property that contains the friendly name of the plug-in. The presence of this attribute tells the framework that this is a plug-in assembly.<br><br>
* **Plug-in Registry** <br>
  The central object in the plug-in framework is the plug-in registry ([IPluginRegistry](../../api/core/Sdl.Core.PluginFramework.IPluginRegistry.yml)). The plug-in registry provides functionality to the host application to detect which plug-ins and extensions are available and create instances of the extensions provided by these plug-ins.<br><br>
* **Plug-in Manifest** <br>
  A plug-in manifest is an XML file, which contains serialized metadata about a plug-in assembly, it contains:
    * The plug-in name, plus the plug-in assembly's name and version.
    * A list of all the extensions defined within the plug-in assembly, together with the meta data defined in the extension attributes.