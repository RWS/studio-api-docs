Compile-time Validation
=====
This section explains how the plug-in manifest generator (see ) validates extensions at compile-time and how to provide custom validation when developing extension points.

Compile-time Validation
------
In order to catch as many developer errors as possible at compile-time, the plug-in framework provides a mechanism for extension point developers to validate extension definitions at compile-time.

Every ExtensionAttribute and AuxiliaryExtensionAttribute has validation methods which are called during the build process. These methods then have the ability to report errors and warnings, which will be displayed in Visual Studio as standard compiler errors.

**Extension Attribute Validation**
The ExtensionAttribute type has a Validate method, which by default validates that the user has specified values for the Id and the Name property. When developing and extension point, this method can be overridden to perform additional validation.

In the message transmitter example, the MessageTransmitterAttribute checks whether the IMessageTransmitter interface is implemented and raises an error if it is not.

**Auxiliary Extension Attribute Validation**
Also the AuxiliaryExtensionAttribute type has a Validate method, which by default doesn't do any special validation. Extension point developers can override this method to perform additional validation for an auxiliary extension attribute.