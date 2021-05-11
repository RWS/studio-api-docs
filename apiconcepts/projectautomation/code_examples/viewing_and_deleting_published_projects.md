Viewing and Deleting Published Projects
==

As well as allowing you to open a project, the [ProjectServer](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.ProjectServer.yml) class provides methods to view existing projects on the server and delete projects from the server.

Viewing Server Projects
--

The following example shows how to view all the projects on the project server starting from a particular organization folder including all sub-folders.

```cs
Uri serverAddress = new Uri("http://myServerAddress:80");
string organizationPath = "/LocationOnServerToStartListFrom";

ProjectServer server = new ProjectServer(serverAddress, false, "MyUser", "MyPassword");
ServerProjectInfo[] projects = server.GetServerProjects(organizationPath, true, false);
```
This next example shows how to search for a particular product by name in a particular organization path. The result from the search is used to open the project and create a new workspace on the local machine.

```cs
string rootLocalProjectLocation = @"C:\Projects\";

 Uri serverAddress = new Uri("http://myServerAddress:80");

 ProjectServer server = new ProjectServer(serverAddress, false, "MyUser", "MyPassword");
 ServerProjectInfo projectInfo = server.GetServerProject("/MyOrganizationName/MyProjectName");

 FileBasedProject project;

 if (projectInfo!=null)
 {
     project = server.OpenProject(projectInfo.ProjectId, rootLocalProjectLocation + projectInfo.Name);
 }
```

Deleting server projects
--

The following example shows how to delete a project from the project server using the unique id for the project.

```cs
Uri serverAddress = new Uri("http://myServerAddress:80");

ProjectServer server = new ProjectServer(serverAddress, false, "MyUser", "MyPassword");
server.DeleteProject(projectId);
```

>**Note**
>
>This only removes the project from the server and marks the local copy as deleted You can still access the local copy as a local project but any attempt to access the previously attached server will generate an exception

See Also
--

**Other Resources**

[About Server Based Projects](..\about_server_based_projects.md)

[Connecting a Project to a Project Server](connecting_a_project_to_a_project_server.md)

[Checking Files In and Out](checking_files_in_and_out.md)

[Downloading and Uploading Files](downloading_and_uploading_files.md)

[Putting it All Together](putting_it_all_together.md)