# Projects Controller

The Var:ProductName Integration API enables third-party developers to implement project functionalities in the Var:ProductName application.

## ProjectsController Overview

The [ProjectsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml) enables third-party developers to integrate custom UI functionalities into the Var:ProductName projects view and perform project operations.

For more information, see [About projects](../projectautomation/about_projects.md).

For a complete code sample, see [Using Var:ProductName ProjectsController](using_trados_studio_projectscontroller.md).

## Enhancing the Projects View

The [ProjectsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml) supports integrating custom UI into the Var:ProductName projects view:

- Integrating viewparts (see [Integrating viewparts](integrating_viewparts.md))
- Integrating menus
- Integrating context menus

## Project Operations

The [ProjectsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml) supports performing project operations:

- Adding projects to the projects list
- Opening a project
- Closing projects
- Performing operations on the current project

For more information, see [About projects](../projectautomation/about_projects.md).

To perform project file operations on the current open project, see [Files controller](files_controller.md).

> [!NOTE]
> If you modify projects using the Project Automation API, call [Save](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_Save) to save changes and [RefreshProjects](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml#Sdl_TranslationStudioAutomation_IntegrationApi_ProjectsController_RefreshProjects) to update the UI.
