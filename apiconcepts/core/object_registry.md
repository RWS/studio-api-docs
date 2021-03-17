Object Registry
=====
This sections explains a coupld of utility classes that can be used by host applications.

Object Registry
----
Often, the host application just needs to create an instance of all the extension implementations registered with a particular extension point.

In the example above, we explicitly went through the plug-in registry object model to illustrate how the various classes interact. However, to facilitate this common use case, there’s a helper class, called ObjectRegistry<TExtensionAttribute, TExtensionType>. This class allows the host application to quickly create a list of extension implementation objects as follows:

ObjectRegistry<TExtensionAttribute, TExtensionType> is a generic class accepting two template parameters:

* TExtensionAttribute: the extension attribute type defining the extension point
* TExtensionType: the common interface, that all extensions for this extension point must implement.
The CreateObjects method simply instantiates all the extension implementations and returns them in a typed array.