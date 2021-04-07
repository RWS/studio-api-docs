Plug-in Framework Overview
====
This help system describes the Plug-in Framework. This framework offers plug-in functionality to applications and components. Application developers can define extension points and dynamically load any plug-ins providing extensions targeting these extension points using the plug-in framework.


Version
---
The version of the Plug-in Framework documented here is the one that was released with <Var:ProductName>.

Design Goals
---
The following are the design goals of the plug-in framework:

* Provide plug-in functionality to support large application design consisting of many loosely connected pieces of functionality.
* Performance: Functionality should only be loaded or created as needed.
* Componentization: Plug-ins can be deactivated and combined as needed through deployment to form different applications.
* Extensibility: Plug-ins can define new extension points and therefore also host other plug-ins.
* Easy development: Configuration is checked by the framework at compile-time. No manual editing of config files is needed.
* Easy deployment: No central configuration should be updated when installing or uninstalling a plug-in.
* Localizability: All plug-in meta-data should be localizable.


Main Concepts and Definitions
----
This section introduces a number of concepts and definitions that are central to the plug-in framework.

* **Hosting Application**
  
    This is the application (or component) which provides plug-in capabilities. The hosting application will define certain extension points and will use the plug-in framework to discover and load plug-ins and extensions for these extension points.

* **Extension Point**
  
    An extension point is a point in the application or component that allows adding extensions to it. An extension point is defined by a .Net attribute, called an extension attribute. All extension attributes should derive from the [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) base class.

* **Extension**
  
    An extension is a single piece of functionality that adds functionality to an application or component through a specific extension point. An extension is defined by a .Net class, defined in a plug-in assembly, annotated with the extension attribute that identifies the extension point it targets. An extension class typically implements an interface that is required by the extension point it is targeting.

* **Plug-in**
  
    A plug-in can contain one or more extensions. A plug-in consist is a .Net assembly which is annotated with the assembly-level PluginAttribute, which has a Name property that contains the friendly name of the plug-in. The presence of this attribute tells the framework that this is a plug-in assembly.

* **Plug-in Registry**
  
    The central object in the plug-in framework is the plug-in registry ([IPluginRegistry](../../api/core/Sdl.Core.PluginFramework.IPluginRegistry.yml)). The plug-in registry provides functionality to the host application to detect which plug-ins and extensions are available and create instances of the extensions provided by these plug-ins.

* **Plug-in Manifest**
  
    A plug-in manifest is an XML file, which contains serialized metadata about a plug-in assembly, it contains:

    * The plug-in name, plus the plug-in assembly's name and version.
    * A list of all the extensions defined within the plug-in assembly, together with the meta data defined in the extension attributes.
  
The purpose of the plug-in manifest is to avoid loading all the plug-in assemblies at runtime and reflecting over them to discover the available extensions, which would affect startup performance, an important aspect of desktop applications.

At compile time, the plug-in manifest generator creates a plug-in manifest for every plug-in assembly. The plug-in manifest generator is implemented as an MSBuild task, which runs as part of the standard <var:VisualStudioEdition> build. The plug-in manifest generator reflects over the plug-in assembly and creates the plug-in manifest plus any additional resource files containing resources needed by the manifest (see [Plug-in Resources and Localizability](plugin_resources_and_localizability.md)). The plug-in registry loads plug-in information from one or more plug-in manifest files.

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.