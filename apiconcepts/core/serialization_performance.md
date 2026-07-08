## Serialization Performance
This section describes how to make sure that extensions can be loaded in the most optimal way.

### Overview
The automatic serialization mechanism that comes with .Net is quite handy, since it let’s you serialize and deserialize objects to and from xml in a virtually a single line of code.

However, this ease of use comes with a downside: the first time you serialize an object of a certain type, the .NET runtime uses reflection to retrieve the list of properties of the type. It then generates code to serialize and deserialize these properties, compiles the code into an assembly, and loads it.

In desktop application scenarios, this process can significantly impact performance, as it is repeated for every extension or auxiliary extension attribute type used at startup.

### Optimizing Serialization
To avoid the overhead of automatic serialization, explicitly implement the `System.Xml.Serialization.IXmlSerializable` interface. The extension attribute base class already defines the methods required by this interface: `GetSchema`, `ReadXml`, and `WriteXml`. To optimize serialization:

1. Mark your attribute class to implement `IXmlSerializable`.
2. Override `ReadXml` and `WriteXml` to handle additional properties.
3. Ensure you call the base class methods to read and write base class properties.

# [C#](#tab/tabid-1)
[!code-csharp[AdvancedPluginFramework](code_samples/AdvancedPluginFramework.cs#L85-L110)]
***

The plug-in framework directly calls the `ReadXml` and `WriteXml` methods, eliminating the overhead of automatic XML serialization.
