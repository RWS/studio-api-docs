Adding Files in the Folder to the Project
==

In the next step we add the functionality for adding the project files from the specified main folder.

Implement a new helper function that is used to:

* Add files from the specified documents folder to the project.
* Scan the files to ascertain whether they are translatable (e.g. **.doc*, **.ppt*) or whether they cannot be processed by the application (e.g. pixel graphics) and are therefore set as reference, which means that they are not processed any further.

The new helper function takes the following parameters:
* A [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object.
* The documents folder name (string).
* The recursion parameter to indicate whether sub-folders should be taken into account (boolean).

```cs
private void AddFiles(FileBasedProject project, string folder, bool recursion)
```

Apply the [AddFolderWithFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_AddFolderWithFiles_System_String_System_Boolean_) method to the project. This method requires the folder name as string parameter and the boolean recursion parameter. This means that this method can have the API loop through any sub-folders without you having to implement the recursion logic yourself.

```cs
project.AddFolderWithFiles(folder, recursion);
```

As the specified folder (and any sub-folders) may also contain untranslatable files, we need to determine which files are actually translatable, and thus suitable for further processing. To do this we use the automatic scan task, which determines the file usage. We start by applying the [GetSourceLanguageFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetSourceLanguageFiles) method to the project, in order to retrieve the project files. Next, we use the [GetSourceLanguageFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetSourceLanguageFiles) method to the project, which requires two parameters:
* The ids of the files, which we previously retrieved
* The type of task that should be run on the project, i.e. ```Scan```.

After that, all documents from the specified folder should be added to our project, and the usage (i.e. translatable or reference) should be determined.

The complete function for adding and scanning the source files should look as shown below:

# [C#](#tab/tabid-1)
```cs
/// <summary>
/// Adds the files from the specified folder to the project and sets the file use, e.g. translatable or reference.            
/// </summary> 
#region "AddFilesFunction"
private void AddFiles(FileBasedProject project, string folder, bool recursion)
#endregion
{
    #region "AddFolderWithFiles"
    project.AddFolderWithFiles(folder, recursion);
    #endregion

    #region "GetSourceLanguageFiles"
    ProjectFile[] projectFiles = project.GetSourceLanguageFiles();

    AutomaticTask scan = project.RunAutomaticTask(
        projectFiles.GetIds(),
        AutomaticTaskTemplateIds.Scan);
    #endregion
}
```
***

See Also
--

[Adding Files and Folders](adding_files_and_folders.md)

[Updating Project Files](updating_project_files.md)

[Creating a Merged File](creating_a_merged_file.md)

[About Project Files](about_project_files.md)