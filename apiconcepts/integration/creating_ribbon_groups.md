Creating ribbon groups
====
<Var:ProductName> Integration API provides support for third-party developers to integrate UI ribbon groups inside the <Var:ProductName> desktop applications.

Creating ribbon groups
---
In order to create ribbon groups, a third-party developer will require the following steps:

* Create a class inherited from [AbstractRibbonGroup](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractRibbonGroup.yml)
* Decorate the newly created class using the [RibbonGroupAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.RibbonGroupAttribute.yml) attribute to define the ribbon group.
* Decorate single or multiple times the newly created class using the [RibbonGroupLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.RibbonGroupLayoutAttribute.yml) attribute to define the ribbon group layout settings.
  
For a sample on how to create a ribbon group, see: [Integrating ribbon groups](integrating_ribbon_groups.md)

> [!NOTE]
> 
> If there are no UI elements added inside the ribbon group, it will not be visible.

See Also
-----
[Creating action](creating_actions.md)