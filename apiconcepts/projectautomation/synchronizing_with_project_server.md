Synchronizing With Project Server
==

When using Var:ProductName, synchronization between the server and the local copy occurs every 5 seconds. This makes sure that changes made by other users are reflected in the local copy. In the Project Automation API it is the responsibility of the developer to make sure that synchronization is performed before you access any files or settings and after you have made changes to any files or settings.

How to Synchronize and what Synchronization Does
--

Synchronization ensures that all the changes made by other users since the last time the project was synchronised are copied to the local copy. Only the status of the files are synchronised. The actual physical files must be explicitly downloaded.

To synchronize a project use the [SynchronizeServerProjectData](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_SynchronizeServerProjectData) method on the [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object.

# [C#](#tab/tabid-1)
```cs
project.SynchronizeServerProjectData();
```
****

>[!NOTE]
>
>Synchronization does not automatically upload or download files only the status of the files and the project settings.

See Also
--
[About Server Based Projects](about_server_based_projects.md)

[Connecting a Project to a Project Server](connecting_a_project_to_a_project_server.md)

[Viewing and Deleting Published Projects](viewing_and_deleting_published_projects.md)

[Checking Files In and Out](checking_files_in_and_out.md)

[Downloading and Uploading Files](downloading_and_uploading_files.md)

[Putting it All Together](putting_it_all_together.md)
