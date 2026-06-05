# Creating Ribbon Groups

The Var:ProductName Integration API allows developers to integrate custom UI ribbon groups into Var:ProductName desktop applications.

## Basic Ribbon Group

To create ribbon groups, complete the following steps:

* Create a class that inherits from [AbstractRibbonGroup](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractRibbonGroup.yml)
* Apply the [RibbonGroupAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.RibbonGroupAttribute.yml) attribute to specify ribbon group metadata
* Apply one or more [RibbonGroupLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.RibbonGroupLayoutAttribute.yml) attributes to specify layout settings

See [Integrating ribbon groups](integrating_ribbon_groups.md) for an example.

> [!IMPORTANT]
> 
> You must add at least one UI element to the ribbon group. Empty ribbon groups do not appear in the UI.

## Related Topics

* [Creating action](creating_actions.md)
