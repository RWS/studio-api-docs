Serialization Performance
=====
This section describes how to make sure that extensions can be loaded in the most optimal way.

Serialization Performance
-----
The automatic serialization mechanism that comes with .Net is quite handy, since it let’s you serialize and deserialize objects to and from xml in a virtually a single line of code.

However, as it is often the case, there is a downside to this ease of use: the first time you serialize an object of a certain type, the .Net runtime uses reflection to get the list of properties of the type, then automatically generates code that can serialize and deserialize these properties, compiles that code to an assembly, and finally loads it.

Especially in the desktop application scenario, this will impact performance dramatically, since this process will be repeated for every extension or auxiliary extension attribute type that is used at start-up.

We can avoid this automatic serialization overhead, by explicitly implementing the `System.Xml.Serialization.IXmlSerializable` interface. The extension attribute base class already defines the methods required by this interface: GetSchema, ReadXml and WriteXml. All you need to do is mark your attribute class to implement IXmlSerializable and override ReadXml and WriteXml to read and write additional properties, making sure to call the base class method to read and write the base class properties:

The plug-in framework calls the ReadXml and WriteXml methods directly, avoiding the overhead of automatic xml serialization.