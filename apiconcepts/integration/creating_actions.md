# Creating Actions

The Var:ProductName Integration API allows developers to implement custom actions in Var:ProductName desktop applications.

An action is a piece of code that can have a UI representation and executes over a UI controller. Actions allow users to trigger application-specific functionality.

## Action Types

Var:ProductName supports two types of actions:

* **Global actions** execute in the application context (see [StudioContext](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractApplication.yml))
* **Controller actions** execute in the context of a view or view part controller (see [Creating views](creating_views.md), [Creating viewparts](creating_viewparts.md))

See [Integrating actions](integrating_actions.md) for an example of a global action integrated into a custom ribbon group.

## Creating an Action

To create an action, complete the following steps:

* Create a class that inherits from one of the following base classes:
  * [AbstractAction](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractAction.yml) â€” for global application context actions
  * [AbstractViewControllerAction&lt;TController&gt;](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewControllerAction-1.yml) â€” for context-specific view or view part actions
* Apply the [ActionAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ActionAttribute.yml) attribute to specify action metadata
* Apply one or more [ActionLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ActionLayoutAttribute.yml) attributes to specify layout settings and location

### Action Locations

Specify the action location using one of the following default ribbon tabs or context menus:

**Studio Ribbon Tabs:**
* [StudioDefaultRibbonTabs.AddinsRibbonTabLocation](../../api/integration/Sdl.Desktop.IntegrationApi.DefaultLocations.StudioDefaultRibbonTabs.AddinsRibbonTabLocation.yml)
* [StudioDefaultRibbonTabs.HelpRibbonTabLocation](../../api/integration/Sdl.Desktop.IntegrationApi.DefaultLocations.StudioDefaultRibbonTabs.HelpRibbonTabLocation.yml)
* [StudioDefaultRibbonTabs.ViewRibbonTabLocation](../../api/integration/Sdl.Desktop.IntegrationApi.DefaultLocations.StudioDefaultRibbonTabs.ViewRibbonTabLocation.yml)

**Translation Studio Ribbon Tabs:**
* [TranslationStudioDefaultRibbonTabs.EditorAdvancedRibbonTabLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultRibbonTabs.EditorAdvancedRibbonTabLocation.yml)
* [TranslationStudioDefaultRibbonTabs.EditorReviewRibbonTabLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultRibbonTabs.EditorReviewRibbonTabLocation.yml)
* [TranslationStudioDefaultRibbonTabs.HomeRibbonTabLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultRibbonTabs.HomeRibbonTabLocation.yml)

**Translation Studio Context Menus:**
* [TranslationStudioDefaultContextMenus.EditorDocumentContextMenuLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultContextMenus.EditorDocumentContextMenuLocation.yml)
* [TranslationStudioDefaultContextMenus.FilesContextMenuLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultContextMenus.FilesContextMenuLocation.yml)
* [TranslationStudioDefaultContextMenus.ProjectsContextMenuLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultContextMenus.ProjectsContextMenuLocation.yml)

You can also target a custom ribbon group (see [Integrating ribbon groups](integrating_ribbon_groups.md)).

See [Integrating actions](integrating_actions.md) for an example.


> 
> [!TIP]
> 
> On how to obtain an already created action, see: [GetAction< TAction>](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractApplication.yml#Sdl_Desktop_IntegrationApi_AbstractApplication_GetAction__1) 

## Keyboard Shortcuts

The Var:ProductName Integration API allows developers to add keyboard shortcuts to actions.

To create a keyboard shortcut for an action, apply the [ShortcutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ShortcutAttribute.yml) to your action class and specify the keyboard key combination that triggers the action.

> [!NOTE]
> 
> Shortcuts that conflict with existing shortcuts have lower priority and are ignored.

