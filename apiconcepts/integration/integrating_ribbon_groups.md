Integrating ribbon groups
=====

Desktop Integration API provides support for third-party developers to integrate UI ribbon groups inside the <Var:ProductName> desktop applications.

Integrating ribbon groups
-----
The following example demonstrates how to create a ribbon group into a <Var:ProductName> application.

# [C#](#tab/tabid-1)
[!code-csharp[CreatingRibbonGroups](code_samples/CreatingRibbonGroups.cs#L1-L14)]
***

> [!NOTE]
> 
> <Var:ProductName> Integration API provide a special location for the plug-ins (see: [TranslationStudioDefaultRibbonTabs](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultRibbonTabs.yml)).

For information on how to add UI items inside a ribbon group, read about the actions. (see: [Creating actions](creating_actions.md) or check the sample [Integrating actions](integrating_actions.md))

> [!NOTE]
> 
> If there are no UI elements added inside the ribbon group, it will not be visible.
