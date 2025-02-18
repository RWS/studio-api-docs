Checking Files In and Out
==
To modify a file on a server project you will need to check it out from the server so that you have exclusive access. Once you have performed any modifications you will need to check in the file again to make a new version of the file available to other users of the project.

You will need to check in files when you have:

* Modified an existing project file.
* Added a new file to the project

The [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object provides methods to check files in and out.

Local file state
--

As you work on files in a server project the local file state will change do indicate the status of the local copy in relation to the file on the server. The file can be in one of the following states. These are represented in the enumeration [LocalFileState](../../api/projectautomation/Sdl.ProjectAutomation.Core.LocalFileState.yml)

* **Conflict**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;The file on the server is in conflict with the local copy for example you have modified an older version of a file without first downloading the latest version.

* **Missing**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;There is a file on the server but the file is not available in the local copy. To use this file you will need to check it out or download the latest version from the server.

* **Modified**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;The file on the local copy has been modified. To keep the changes you should check the file in to the server. To revert your changes you can download the latest version from the server.

* **New**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;The file is new and not yet copied to the server. To keep the file, you need to check it into the server.

* **None**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This status is for file based projects only.

* **OutOfDate** 

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;A newer version of the file is available on the server. Download the latest version from the server to resolve this.

* **UpToDate**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;The file on the server matches the file on the local copy.

>[!NOTE]
>
>The [LocalFileState](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectFile.yml#Sdl_ProjectAutomation_Core_ProjectFile_LocalFileState) can be accessed from the [ProjectFile](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectFile.yml) object. As Project Automation API works on a snapshot, you must get the file information from the project again to update the status.

Checking out files
--

When you open files for translation or review, run a batch task or create a package, you must check the files out first. Use the [CheckoutFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_CheckoutFiles_System_Guid___System_Boolean_System_EventHandler_Sdl_ProjectAutomation_Core_ProgressEventArgs__) method to check out the file. If the local copy of the file is out of date or missing then it will be automatically downloaded from the server. If the local file has been modified it will not be downloaded unless the override parameter is set to true.

# [C#](#tab/tabid-1)
```cs
project.CheckoutFiles(fileIds, false, 
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
>You only need to check out files once the project has been published. You can process files without check out before that.

Checking in a new version of a file
--

When you open a file for translation or review, run a batch task or create a package, you must first check out the files from the Project Server. Once complete, you need to update the files on the Project Server with your locally modified files by checking them in.

# [C#](#tab/tabid-2)
```cs
project.CheckinFiles(fileIds, "This is where you add a check in comment",
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
>Use the [CheckinFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_CheckinFiles_System_Guid___System_String_System_EventHandler_Sdl_ProjectAutomation_Core_ProgressEventArgs__) method when checking in a new or modified file. This will make sure that a new version of the file is created on the server and that it is available for other users to check out.

Adding new files to the server
--

If you add a new file to a server project, checking in the file uploads it to the Project Server so other users can work with the file. The steps to add a new file are identical to adding a file to a local project but with the one extra step of checking in the files and uploading them to the server.

The following example uploads and checks in all the new source files that have been added to the project.

# [C#](#tab/tabid-3)
```cs
Guid[] sourceFileIds = project.GetSourceLanguageFiles()
    .Where(file => file.LocalFileState == LocalFileState.New)
    .GetIds();

project.CheckinFiles(sourceFileIds, "New Source Files",
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
>Use the [CheckinFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_CheckinFiles_System_Guid___System_String_System_EventHandler_Sdl_ProjectAutomation_Core_ProgressEventArgs__) method when checking in a new or modified file. This will make sure that a new version of the file is created on the server and that it is available for other users to check out.

Undo file check out
--

To cancel the checkout of one or more files use the [UndoCheckoutFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_CheckoutFiles_System_Guid___System_Boolean_System_EventHandler_Sdl_ProjectAutomation_Core_ProgressEventArgs__) method. This unlocks the file on the server without changing the version and leaves the local copy untouched.

# [C#](#tab/tabid-4)
```cs
project.UndoCheckoutFiles(fileIds);
```
***

>[!NOTE]
>
>You can only undo the check out for a file if it was previously checked out. If a file is checked out to another user you must have the "Cancel Checkout of Other Users Project Files" permission on the server.

See Also
--

[About Server Based Projects](about_server_based_projects.md)

[Connecting a Project to a Project Server](connecting_a_project_to_a_project_server.md)

[Viewing and Deleting Published Projects](viewing_and_deleting_published_projects.md)

[Downloading and Uploading Files](downloading_and_uploading_files.md)

[Putting it All Together](putting_it_all_together.md)
