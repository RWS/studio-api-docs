Sortable Extension Points
=====
This section describes how to develop extension points that give importance to the order of extensions.

Sortable Extension Points
-----
When a hosting application retrieves all extensions for a certain extensions point, through the Extensions collection, the order in which the extensions appear in this collection is essentially random.

It is a fairly common requirement for extensions to have a certain order and for extensions themselves to be able to specify where they want to "appear" relative to the other extensions for that extension point. An example of this are menu item extensions: when creating a new menu item extension, there is clearly the need to specify where that menu item should appear relative to other menu items.

In order to solve this common use case, the plug-in framework provides the SortableExtensionAttribute extension attribute class. This attribute extends the standard ExtensionAttribute by adding two additional properties: InsertBefore and InsertAfter. Extensions for an extension point that derives from SortableExtensionAttribute can use these properties to specify the Id of other extensions they want to appear before or after. In addition to single Ids, these properties also accept multiple comma-seperated Ids.

The hosting application can use the SortedObjectRegistry< TSortableExtensionAttribute, TExtensionType> class to create instances of all extensions for a sortable extension point and sort these according to the values of the InsertBefore and InsertAfter properties.

In case the hosting application wants more control over when the extensions are instantiated, it can use the TopologicalSort< T> class, which implements the sorting algorithm. It then has to provide wrapper object that implements ITopologicalSortable for each extension.

Community