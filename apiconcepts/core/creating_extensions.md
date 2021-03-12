Creating Extensions
=====
This section explains how to develop an extension that targets a specific extension point.

Creating Extensions
----
The plug-in framework comes with a C# project template for developing extensions: *SDL Plug-in Project*. It shows up in the Visual Studio .Net 2010 New Project dialog:

<img style="display:block; " src="images/CreateNewPluginProject.png"/>

Create a new plug-in project, called PluginLibrary. A plug-in project consists of a standard C# library project, with a few extra additions:

* The PluginProperties.cs file: this file contains the PluginAttribute attribute declaration, which marks the assembly as a plug-in assembly, and also indicates the friendly name of the plug-in.
* The PluginResources.resx file: this is a resources file, which contains resources (strings and images) )referred to within extension attribute declarations.
* The plug-in manifest creation build step: the project contains an extra build step, which creates a plug-in manifest file, decribing all the extensions contained in the plug-in assembly. This build step uses the standard MSBuild extension mechanism.

<img style="display:block; " src="images/PluginLibraryProject.png"/>

The first thing to do is look at the PluginProperties.cs file in the Properties folder. It contains the following:

The Name property contains the name of the plug-in. This is actually not the name, but the key in the PluginResources.resx file, which contains the plug-in name:

<img style="display:block; " src="images/PluginResources.png"/>

Going back to the example, let's define a message transmitter which transmits messages by email, the EmailMessageTransmitter class:
The EmailMessageTransmitter class implements the IMessageTransmitter interface. On top of that, the class is annotated with the extension attribute, MessageTransmitter, which we defined earlier, providing an id, a name, a description and the cost per character when sending messages using this transmitter.

Similar to the email transmitter, we also define the an SMS message transmitter in exactly the same way. We can do this within the same plug-in project, because a plug-in project can contain multiple extensions, but normally, you would do this in a separate plug-in project.

When building the project a plug-in manifest file is created in a plugins subfolder of the project output folder. It contains all the meta data for extensions defined in the plug-in assembly:

Any errors or warnings related to the manifest generation will be reported in the Visual Studio task list.

Next, we are ready to bring everything together in the host application, which will support sending messages through pluggable message transmitters: Creating the Host Application .