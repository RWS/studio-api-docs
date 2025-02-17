Creating actions
=====
Var:ProductName Integration API provides support for third-party developers to implement actions inside the Var:ProductName desktop applications.

An action is a piece of code which can have a UI representation and is usually executed over a UI controller. Most of the time, it is used to allow users to execute an application-specific functionality.

About actions
----
There are two kind of actions that can be created inside Var:ProductName applications:

* Actions which have a global purpose beeing executed in the application context (see: **StudioContext**).
* Actions which are executed over a view or viewpart controller (see: [Creating views](creating_views.md), [Creating viewparts](creating_viewparts.md)).

For a sample on how to create a global context actions integrated into a custom ribbon group, see: Integrating actions

Creating an action
------
In order to create an action, a third-party developer will require the following steps:

* Create a class inherited based on the scope from:
[AbstractAction](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractAction.yml) if the action requires to be available in a Var:ProductName global context and its execution is not specific to a view or viewpart, but to the application.
[AbstractViewControllerAction<TController>](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewControllerAction-1.yml) if the action requires to be available in any context, but it is specific to one of the view or viewpart controller.
* Decorate the newly created class using the [ActionAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ActionAttribute.yml) attribute to define the action.
* Decorate single or multiple times the newly created class using the [ActionLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ActionLayoutAttribute.yml) attribute to define the action layout settings. The location specified in the action layout settings can be either a custom ribbon group (see: [Integrating ribbon groups](integrating_ribbon_groups.md)), or one of the following default locations:
    * [StudioDefaultRibbonTabs.AddinsRibbonTabLocation](../../api/integration/Sdl.Desktop.IntegrationApi.DefaultLocations.StudioDefaultRibbonTabs.AddinsRibbonTabLocation.yml)
    * [StudioDefaultRibbonTabs.HelpRibbonTabLocation](../../api/integration/Sdl.Desktop.IntegrationApi.DefaultLocations.StudioDefaultRibbonTabs.HelpRibbonTabLocation.yml)
    * [StudioDefaultRibbonTabs.ViewRibbonTabLocation](../../api/integration/Sdl.Desktop.IntegrationApi.DefaultLocations.StudioDefaultRibbonTabs.ViewRibbonTabLocation.yml)
    * [TranslationStudioDefaultRibbonTabs.EditorAdvancedRibbonTabLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultRibbonTabs.EditorAdvancedRibbonTabLocation.yml)
    * [TranslationStudioDefaultRibbonTabs.EditorReviewRibbonTabLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultRibbonTabs.EditorReviewRibbonTabLocation.yml)
    * [TranslationStudioDefaultRibbonTabs.HomeRibbonTabLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultRibbonTabs.HomeRibbonTabLocation.yml)
    * [TranslationStudioDefaultContextMenus.EditorDocumentContextMenuLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultContextMenus.EditorDocumentContextMenuLocation.yml)
    * [TranslationStudioDefaultContextMenus.FilesContextMenuLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultContextMenus.FilesContextMenuLocation.yml)
    * [TranslationStudioDefaultContextMenus.ProjectsContextMenuLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultContextMenus.ProjectsContextMenuLocation.yml)


For a sample on how to create actions, see: [Integrating actions](integrating_actions.md)


> [!NOTE]
> 
> On how to obtain an already created action, see: [GetAction< TAction>](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractApplication.yml#Sdl_Desktop_IntegrationApi_AbstractApplication_GetAction__1) 

Creating a keyboard shortcut for an action
------
The Var:ProductName Integration API provides support for third-party developers to add keyboard shortcuts to the actions they implement.

In order to create a shortcut action, a third-party developer will require to decorate the action class with the [ShortcutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ShortcutAttribute.yml) and specify the combination of keyboard keys on which the action should be triggered and executed.

You can see in the sample below how ALT+F8 is added as a shortcut key for the action.

> [!NOTE]
> 
> The shortcut keys that are in conflict with shortcuts will have a lower priority and will be ignored.
