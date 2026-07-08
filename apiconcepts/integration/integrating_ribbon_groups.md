# Integrating Ribbon Groups

The Desktop Integration API allows third-party developers to integrate custom UI ribbon groups into Var:ProductName desktop applications.

## Example

The following example demonstrates how to create a ribbon group in a Var:ProductName application.

# [C#](#tab/tabid-1)
[!code-csharp[CreatingRibbonGroups](code_samples/CreatingRibbonGroups.cs#L1-L14)]
***

> [!NOTE]
> 
> Var:ProductName Integration API provides a special location for plug-ins (see [TranslationStudioDefaultRibbonTabs](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultRibbonTabs.yml)).

To add UI elements to a ribbon group, see [Creating actions](creating_actions.md) or the [Integrating actions](integrating_actions.md) sample.

> [!IMPORTANT]
> 
> You must add at least one UI element to the ribbon group. Empty ribbon groups do not appear.


## Reference

* [AbstractRibbonGroup](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractRibbonGroup.yml)
* [RibbonGroupAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.RibbonGroupAttribute.yml)
* [RibbonGroupLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.RibbonGroupLayoutAttribute.yml)
* [TranslationStudioDefaultRibbonTabs](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultRibbonTabs.yml)
