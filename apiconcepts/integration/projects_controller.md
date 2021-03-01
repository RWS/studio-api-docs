Projects controller
====
Trados Studio Integration API provides support for the third party developers to implement project functionalities for Trados Studio application.

Projects controller
----
The ProjectsController enables the third party developer to integrate custom UI functionalities inside SDL Trados Studio projects view and perform project operations.

For more information: About projects

For a sample on how to use it see the following sample: Using Trados Studio ProjectsController

Enhance SDL Trados Studio projects view using ProjectsController
----
The ProjectsController provide support for integrating custom UI inside the SDL Trados Studio projects view.

Integrating viewparts (see Integrating viewparts)
Integrating menus
Integrating context menus

Operations on the projects
---
The ProjectsController provide support for performing project operations.

Adding projects to the projects list
Opening a project
Closing projects
Operations on the current project
For more informations: About projects

To perform project files operations on the current open project, see the Files controller

> [!NOTE]
> If you make changes to the projects by using the Project Automation API than you may need to call Save for saving the changes and RefreshProjects to reflect the changes in the UI.**