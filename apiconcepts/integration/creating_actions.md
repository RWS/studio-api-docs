Creating actions
=====
Trados Studio Integration API provides support for third-party developers to implement actions inside the Trados Studio desktop applications.

An action is a piece of code which can have a UI representation and is usually executed over a UI controller. Most of the time, it is used to allow users to execute an application-specific functionality.

About actions
----
There are two kind of actions that can be created inside Trados Studio applications:

* Actions which have a global purpose beeing executed in the application context (see: **StudioContext**).
* Actions which are executed over a view or viewpart controller (see: **Creating views, Creating viewparts**).

For a sample on how to create a global context actions integrated into a custom ribbon group, see: Integrating actions

Creating an action
------
In order to create an action, a third-party developer will require the following steps:

* Create a class inherited based on the scope from:
`AbstractAction` if the action requires to be available in a Trados Studio global context and its execution is not specific to a view or viewpart, but to the application.
`AbstractViewControllerAction< TController>` if the action requires to be available in any context, but it is specific to one of the view or viewpart controller.
* Decorate the newly created class using the    `ActionAttribute` attribute to define the action.
* Decorate single or multiple times the newly created class using the `ActionLayoutAttribute` attribute to define the action layout settings. The location specified in the action layout settings can be either a custom ribbon group (see: Integrating ribbon groups), or one of the following default locations:
    * StudioDefaultRibbonTabs.AddinsRibbonTabLocation
    * StudioDefaultRibbonTabs.HelpRibbonTabLocation
    * StudioDefaultRibbonTabs.ViewRibbonTabLocation
    * TranslationStudioDefaultRibbonTabs.EditorAdvancedRibbonTabLocation
    * TranslationStudioDefaultRibbonTabs.EditorReviewRibbonTabLocation
    * TranslationStudioDefaultRibbonTabs.HomeRibbonTabLocation
    * TranslationStudioDefaultContextMenus.EditorDocumentContextMenuLocation
    * TranslationStudioDefaultContextMenus.FilesContextMenuLocation
    * TranslationStudioDefaultContextMenus.ProjectsContextMenuLocation


For a sample on how to create actions, see: Integrating actions


> [!NOTE]
> On how to obtain an already created action, see: GetAction< TAction > 

Creating a keyboard shortcut for an action
------
The Trados Studio Integration API provides support for third-party developers to add keyboard shortcuts to the actions they implement.

In order to create a shortcut action, a third-party developer will require to decorate the action class with the `ShortcutAttribute` and specify the combination of keyboard keys on which the action should be triggered and executed.

You can see in the sample below how ALT+F8 is added as a shortcut key for the action.

> [!NOTE]
> The shortcut keys that are in conflict with shortcuts will have a lower priority and will be ignored.