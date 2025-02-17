Projects controller
====
Var:ProductName Integration API provides support for third-party developers to implement project functionalities for the Var:ProductName application.

Projects controller
----
The [ProjectsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml) enables the third-party developer to integrate custom UI functionalities inside Var:ProductName projects view and perform project operations.

For more information: [About projects](../projectautomation/about_projects.md)

For a sample on how to use it, see the following sample: [Using Var:ProductName ProjectsController](using_trados_studio_projectscontroller.md)

Enhance Var:ProductName projects view using ProjectsController
----
The [ProjectsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml) provide support for integrating custom UI inside the Var:ProductName projects view.

Integrating viewparts (see [Integrating viewparts](integrating_viewparts.md))

* Integrating menus
* Integrating context menus

Operations on the projects
---
The [ProjectsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml)  provide support for performing project operations.

* Adding projects to the projects list
* Opening a project
* Closing projects
* Operations on the current project

For more informations: [About projects](../projectautomation/about_projects.md)

To perform project files operations on the current open project, see the [Files controller](files_controller.md)

> [!NOTE]
>
> If you make changes to the projects by using the Project Automation API, you may need to call [Save](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_Save) for saving the changes and [RefreshProjects](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml#Sdl_TranslationStudioAutomation_IntegrationApi_ProjectsController_RefreshProjects) to reflect the changes in the UI.
