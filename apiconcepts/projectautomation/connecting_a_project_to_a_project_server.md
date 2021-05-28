Connecting a Project to a Project Server
==

Using Trados GroupShare, projects can be shared between many users within an organization. The Automation API provides methods for publishing and accessing server-based projects.

Server-based projects are not accessed directly but through a local copy. Interaction with files in a project through the local copy is just like interacting with a standard project. However additional steps are required to check in, check out files so they are available to all project users.

Publishing to a Project Server
--

To share a project on Project Server the project must be published to the server. To do this through the Project Automation API requires calling the [PublishProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_PublishProject_System_Uri_System_Boolean_System_String_System_String_System_String_System_EventHandler_Sdl_ProjectAutomation_FileBased_PublishProjectEventArgs__) method on the [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object.

In general you will call this method only after you have performed all the necessary project preparation tasks however it can be done at any point after the project has been created.

The following example shows how to publish the project to a server.

# [C#](#tab/tabid-1)
```cs
project.PublishProject(
    new Uri("http://myServerAddress:80"),
    true,
    "MyUserName",
    "MyPassword",
    "/MyOrganization",
     (obj, evt) =>
     {
         Console.WriteLine(evt.StatusMessage + " " + evt.PercentComplete + "% complete");

         if (cancelledByUser)
         {
             evt.Cancel = true;
         }
     });
```
***

>[!NOTE]
>
>Once a project has been published the local project will become the workspace for this project and you will only need to create a new workspace if you access the project from another machine or you delete the existing workspace.

Opening a new server project (no local copy available).
--

If you do not already have a local copy for a server project you must open the project using the [OpenProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.ProjectServer.yml#Sdl_ProjectAutomation_FileBased_ProjectServer_OpenProject_System_Guid_System_String_) method in the [ProjectServer](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.ProjectServer.yml) class. This will create the workspace and return a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object.

The following example shows how to open a server-based project if no pre-existing local copy exists. To use this method, you need the unique id of the project on the server and the path on your local file system where you wish to create the local copy.

# [C#](#tab/tabid-2)
```cs
FileBasedProject SetupServerProjectLocalCopy(Guid projectId, string locationOfLocalCopy)
{
    Uri serverAddress = new Uri("http://myServerAddress:80");

    ProjectServer server = new ProjectServer(serverAddress, false, "MyUser", "MyPassword");
    FileBasedProject project = server.OpenProject(projectId, locationOfLocalCopy);
    return project;
}
```
***

>[!NOTE]
>
>Opening the project from the server only downloads the necessary files to describe the contents of the project and any project settings. The actual source and target files are not automatically downloaded to the local copy.

>[!NOTE]
>
>If you already have a workspace attached to a server project then you should create a new [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) using the constructor for this purpose

Opening a server project from an existing local copy.
--

If you already have a local copy of a server project you can open the project using the constructor designed for this purpose on the ```FileBasedProject``` class.

The following example shows how to open a server-based project from an existing local copy.

# [C#](#tab/tabid-3)
```cs
FileBasedProject project = new FileBasedProject(@"c:\MyProjectDirectory\MyProjectFile.sdlproj", false, "MyUserName", "MyPassword");

```
***

See Also
--

[About Server Based Projects](about_server_based_projects.md)

[Viewing and Deleting Published Projects](viewing_and_deleting_published_projects.md)

[Checking Files In and Out](checking_files_in_and_out.md)

[Downloading and Uploading Files](downloading_and_uploading_files.md)

[Putting it All Together](putting_it_all_together.md)