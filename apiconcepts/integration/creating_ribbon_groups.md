Creating ribbon groups
====
Trados Studio Integration API provides support for third-party developers to integrate UI ribbon groups inside the Trados Studio desktop applications.

Creating ribbon groups
---
In order to create ribbon groups, a third-party developer will require the following steps:

* Create a class inherited from `AbstractRibbonGroup`
* Decorate the newly created class using the `RibbonGroupAttribute` attribute to define the ribbon group.
* Decorate single or multiple times the newly created class using the `RibbonGroupLayoutAttribute` attribute to define the ribbon group layout settings.
For a sample on how to create a ribbon group, see: Integrating ribbon groups

> [!NOTE]
> If there are no UI elements added inside the ribbon group, it will not be visible.