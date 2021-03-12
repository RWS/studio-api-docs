Auxiliary Extension Attributes
======
This section explains how to allow an extension to provide additional meta-data, on top of the properties provided with the extension attribute.

Auxiliary Extension Attributes
----
In some cases, you might need to add some extra meta data to a certain extension implementation, on top of what is defined in the extension attribute and it might be impractical to add these properties to the extension attribute itself.

An example of this is for instance a plug-in user action. The extension attribute for that can define name, icon, tooltip etc…, but you also need to specify on which menus, toolbars and context menus this user action will be available. For cases like this, the plug-in framework provides auxiliary extension attributes.

You can only apply one extension attribute to an extension implementation: this is the attribute that uniquely identifies the extension point the extension implementation targets. On top of this, you can annotate the implementation class with as many auxiliary extension attributes as you like.

An auxiliary extension attribute needs to derive from the AuxiliaryExtensionAttribute base class. For instance, we can define a ToolBarLocation auxiliary attribute, which has a ToolBarId property that can be used to specify on which tool bar the action should appear. For menus, we can define a similar MenuLocation attribute:


, and the plug-in action definition becomes:


The collection of all auxiliary attributes for an extension can be retrieved using the AuxiliaryExtensionAttributes property.