TODO: Remus delete this page
====
Compile-time Validation
=====
This section explains how the plug-in manifest generator validates extensions at compile-time and how to provide custom validation when developing extension points.

Compile-time Validation
------
In order to catch as many developer errors as possible at compile-time, the plug-in framework provides a mechanism for extension point developers to validate extension definitions at compile-time.

Every [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) and [AuxiliaryExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.AuxiliaryExtensionAttribute.yml) has validation methods which are called during the build process. These methods then have the ability to report errors and warnings, which will be displayed in Visual Studio as standard compiler errors.

**Extension Attribute Validation**

The[ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml) type has a `Validate` method, which by default validates that the user has specified values for the `Id` and the `Name` property. When developing and extension point, this method can be overridden to perform additional validation.

In the message transmitter example, the `MessageTransmitterAttribute` checks whether the `IMessageTransmitter` interface is implemented and raises an error if it is not.

# [C#](#tab/tabid-1)
[!code-csharp[MessageTransmitterAttribute](code_samples/MessageTransmitterAttribute.cs#L11-L55)]
***

**Auxiliary Extension Attribute Validation**

Also the [AuxiliaryExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.AuxiliaryExtensionAttribute.yml type has a `Validate` method, which by default doesn't do any special validation. Extension point developers can override this method to perform additional validation for an auxiliary extension attribute.


> [!NOTE]
> 
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.