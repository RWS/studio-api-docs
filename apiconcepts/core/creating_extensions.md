Creating Extensions
=====
This section explains how to develop an extension that targets a specific extension point.

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.

Creating Extensions
----
The plug-in framework comes with a C# project template for developing extensions: *SDL Plug-in Project*. It shows up in the Visual Studio .Net 2010 New Project dialog:

<img style="display:block; " src="images/CreateNewPluginProject.png"/>

Create a new plug-in project, called `PluginLibrary`. A plug-in project consists of a standard C# library project, with a few extra additions:

* The `PluginProperties.cs` file: this file contains the `PluginAttribute` attribute declaration, which marks the assembly as a plug-in assembly, and also indicates the friendly name of the plug-in.
* The `PluginResources.resx` file: this is a resources file, which contains resources (strings and images) )referred to within extension attribute declarations.
* The plug-in manifest creation build step: the project contains an extra build step, which creates a plug-in manifest file, describing all the extensions contained in the plug-in assembly. This build step uses the standard MSBuild extension mechanism.

<img style="display:block; " src="images/PluginLibraryProject.png"/>

The first thing to do is look at the `PluginProperties.cs` file in the `Properties` folder. It contains the following:

# [C#](#tab/tabid-1)
[!code-csharp[PluginProperties](code_samples/PluginProperties.cs#L1)]
***

The Name property contains the name of the plug-in. This is actually not the name, but the key in the `PluginResources.resx` file, which contains the plug-in name:

<img style="display:block; " src="images/PluginResources.png"/>

Going back to the example, let's define a message transmitter which transmits messages by email, the `EmailMessageTransmitter` class:

# [C#](#tab/tabid-2)
[!code-csharp[EmailMessageTransmitter](code_samples/EmailMessageTransmitter.cs#L9-L26)]
***

The `EmailMessageTransmitter` class implements the `IMessageTransmitter` interface. On top of that, the class is annotated with the extension attribute, `MessageTransmitter`, which we defined earlier, providing an id, a name, a description and the cost per character when sending messages using this transmitter.

Similar to the email transmitter, we also define the an SMS message transmitter in exactly the same way. We can do this within the same plug-in project, because a plug-in project can contain multiple extensions. Normally, you would do this in a separate plug-in project.

# [C#](#tab/tabid-3)
[!code-csharp[SMSMessageTransmitter](code_samples/SMSMessageTransmitter.cs#L8-L30)]
***
When building the project a plug-in manifest file is created in a plugins subfolder of the project output folder. It contains all the metadata for extensions defined in the plug-in assembly:

```
<?xml version="1.0" encoding="utf-16"?>
<plugin id="pluginlibrary" name="My Plugin Library" version="1.0.0.0">
  <extension 
        type="PluginLibrary.SMSMessageTransmitter, 
              PluginLibrary">
    <extensionAttribute 
            type="ExtensionPointDefinitions.MessageTransmitterAttribute,  
                  ExtensionPointDefinitions">
      <MessageTransmitterAttribute>
        <Id>sms</Id>
        <Name>SMS Transmitter</Name>
        <Description>Send messages via SMS</Description>
        <CostPerCharacter>0.5</CostPerCharacter>
      </MessageTransmitterAttribute>
    </extensionAttribute>
  </extension>
  <extension 
        type="PluginLibrary.EmailMessageTransmitter, 
              PluginLibrary">
    <extensionAttribute 
            type="ExtensionPointDefinitions.MessageTransmitterAttribute, 
                  ExtensionPointDefinitions">
      <MessageTransmitterAttribute>
        <Id>email</Id>
        <Name>E-mail Transmitter</Name>
        <Description>Send messages via e-mail</Description>
        <CostPerCharacter>0.1</CostPerCharacter>
      </MessageTransmitterAttribute>
    </extensionAttribute>
  </extension>
</plugin>
```

Any errors or warnings related to the manifest generation will be reported in the Visual Studio task list.

Next, we are ready to bring everything together in the host application, which will support sending messages through pluggable message transmitters:  [Creating the Host Application](core/creating_the_host_application.md).