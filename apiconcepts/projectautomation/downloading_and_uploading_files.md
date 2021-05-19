Downloading and Uploading Files
==

Files must be copied to the local copy to be edited and copied back to the server so they are available to other users

Get the latest version of a file from the project server
--

Use the ```DownloadLatestVersion``` method in a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) to get the latest version of a file from the server and copy it to your local copy.

```cs
project.DownloadLatestServerVersion(fileId, (obj, evt) =>
    {
        Console.WriteLine("{0}, {1} of {2} bytes transfered",evt.Filename, evt.FileBytesTransferred, evt.FileTotalBytes );

        if (cancelledByUser)
        {
            evt.Cancel = true;
        }
    }, 
    false);
```

>**Note**
>
>This method will only download the file to your local copy. It does not automatically check out the file for editing. You should check out the file before performing any editing task including batch processing.

>**Caution**
>
>This method does not automatically create the directory structure for the file. If the file location is not already created, an exception will occur

The following example shows how to get all the latest files from the server only downloading files that are missing or out of date in the local workspace. There is also the option to overwrite local files that have been modified in the local workspace with the server version.

```cs
void DownloadAllLatest(FileBasedProject project, Guid[] fileIds, bool overrideWorkspaceVersion)
{
    //Make sure we are syncronized with the server
    project.SynchronizeServerProjectData();

    foreach (Guid fileId in fileIds)
    {
        ProjectFile file = project.GetFile(fileId);

        // We only need to download files that are missing from the workspace, out of date and 
        //modified files if we have chosen to override the file with the server version
        if (file.LocalFileState == LocalFileState.Missing
            || (file.LocalFileState == LocalFileState.Modified && overrideWorkspaceVersion)
            || (file.LocalFileState == LocalFileState.OutOfDate)
            )
        {
            //If the file directory does not exist in the local workspace then create it
            string fileLocation = Path.GetDirectoryName(file.LocalFilePath);
            if (!Directory.Exists(fileLocation))
            {
                Directory.CreateDirectory(fileLocation);
            }

            //Download the file passing in a null as the event handler therefore ignoring progress and cancelation.
            project.DownloadLatestServerVersion(file.Id, null, false);
        }
    }
}
```

List all versions of a file on the project server
--

When a file is uploaded to the server a new version of the file is created. The server stores all these versions allowing you to examine and download previous versions.

The following example writes all the versions of a file to the console.

```cs
private void ShowServerFileHistory(FileBasedProject project, Guid fileId)
{
    ProjectFileVersion[] fileVersions = project.GetProjectFileVersionHistory(fileId);

    foreach (ProjectFileVersion fileVersion in fileVersions)
    {
        Console.WriteLine("Version {0}: {1}, Created By: {2}, Created At: {3}", 
                          fileVersion.VersionNumber, 
                          fileVersion.FileName , 
                          fileVersion.CreatedBy,
                          fileVersion.CreatedAt
            );
    }
}
```

Get a previous version of a file from the project server
--

Downloading a specific version of a file allows you to save a specific file version locally. You may want to download a specific version of a file so that you can compare it to a current version.

When downloading a specific version you will be asked to provide the directory location to which you wish to download the file. This will usually be a different location to your current project. If you wish to replace your current workspace version with a previous version from the server you should check out the file from the server first so that conflicts do not occur.

The following example shows how an anonymous delegate can be used to handle the progress and cancelation events.

```cs
project.DownloadSpecificServerVersion(fileId, 1, @"c:\files", (obj, evt) =>
 {
     Console.WriteLine("{0}, {1} of {2} bytes transfered", evt.Filename, evt.FileBytesTransferred, evt.FileTotalBytes);

     if (cancelledByUser)
     {
         evt.Cancel = true;
     }
 });
```

Uploading a file to the server
--

When you use the [CheckinFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_CheckinFiles_System_Guid___System_String_System_EventHandler_Sdl_ProjectAutomation_Core_ProgressEventArgs__) method this will automatically upload the files to the server. This method provides an event delegate so that you can report on the progress of the upload and cancel the upload if required.

The following example shows how to upload files using a lambda expression to handle the progress and cancelation events.

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

See Also
--

**Other Resources**

[About Server Based Projects](about_server_based_projects.md)

[Connecting a Project to a Project Server](connecting_a_project_to_a_project_server.md)

[Viewing and Deleting Published Projects](viewing_and_deleting_published_projects.md)

[Checking Files In and Out](checking_files_in_and_out.md)

[Putting it All Together](putting_it_all_together.md)