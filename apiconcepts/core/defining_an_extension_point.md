Defining an Extension Point
====
This section explains how to create an extension point.

Defining an Extension Point
----
An extension point is defined by creating an attribute class, which extends the extension attribute base class,  [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml). The properties of this attribute class define the metadata extension developers can provide with their extensions.

Extension developers can now create an extension .Net class, and mark this up with the extension attribute to indicate which extension point it targets.

Going back to the example, in order to allow message transmitters to be plugged in, we need to define an extension point. This is done by defining an extension attribute class, `MessageTransmitterAttribute`, which extends the extension attribute base class,  [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml):

# [C#](#tab/tabid-1)
[!code-csharp[MessageTransmitterAttribute](code_samples/MessageTransmitterAttribute.cs#L11-L52)]
***

The purpose of the message transmitter attribute is to allow plug-in developers to use it to annotate their message transmitter extension classes, to make them known as message transmitter implementations to the extension point.

In order to make the attribute an extension point, we have to annotate with the [ExtensionPointInfoAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionPointInfoAttribute.yml) attribute, which specifies the name and type of the extension point. The name can be used by a plug-in manager UI to represent the extension point. The type can be either static or dynamic, referring to whether this extension point allows enabling or disabling of one or more of it’s extensions without having to restart the application. More on that later.

The  [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) class has the following properties, providing meta data about the extension:

* *Id*: A unique id for the extension
* *Name*: A friendly name for the extension.
* *Description*: A description of the extension.
* *Icon*: An optional icon representing the extension.
Note that we have defined an extra property, CostPerCharacter, which indicates the cost in dollar for each character sent in a message.

Since all these properties will be extracted to the plug-in manifest file by the plug-in manifest generator, the host application will be able to access their values without having to create an instance of the actual transmitters or even load the plug-in assembly. Since the plug-in manifest generator uses XML serialization to save the attribute information, the attribute has have a default, parameterless constructor.

We still have to define which functionality is required from an extension class to be accepted as a valid message transmitter by the host application. This is done by defining an interface:

[!code-csharp[IMessageTransmitter](code_samples/IMessageTransmitter.cs#L1-L15)]
***

This simple interface contains one method, SendMessage, to be called by the host application to send the message.

Extension points can also define validation functionality which is used by the framework at compile-time to validate its extensions. In the example, tye extension point checks whether the extension implements the IMessageTransmitter interface. If an extension ndoes not implement this interface, an error will be generated at compile time. For more information on extension validation see Compile-time Validation.

We define both the `MessageTransmitterAttribute` and the interface in an assembly, called `ExtensionPointDefinitions`, so it can be references by the host applications and by plug-in assemblies.

Next, we can start creating an extension that targets our new extension point in [Creating Extensions](creating_extensions.md).

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.