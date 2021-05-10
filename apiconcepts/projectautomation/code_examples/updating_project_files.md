Updating Project Files
==

t is a rather common scenario for files to be updated in the middle of a project. For example, after you have finished creating your project, the customer might send an additional file to the project manager, which needs to be incorporated into the project. Another common use case is that the customer finds out that he/she has sent an outdated source file to the project manager, and therefore provides a more recent version. In this case, the updated file needs to replace the document that is already in the project. Any new or updated files should undergo the same task sequence as the files that are already in the project, e.g. conversion to a translatable format (SDL XLIFF), file analysis, pre-translation, etc.

Therefore, the project manager will first add the new or updated files and apply the same task sequence to those files that were used on the files that are already in the project.

If you want your application to allow users to add new files or update existing ones with a newer version, you can add a helper function called, e.g. ```UpdateFile```. In our sample implementation this helper function takes the following parameters: a [FileBasedProject](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object, the unique id of the file to be replaced, and the name and path of the updated file to be added to the project:

```cs
private void UpdateFile(FileBasedProject project, Guid fileId, string newFileName)
```

Before we actually replace the existing project file, it is worth taking a look at the various properties that you can retrieve on a file. Each project file is uniquely identified by an id (a guid). To get a handle on a file, you can apply the [GetFile](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetFile_System_Guid_) method to the project. This method takes the file guid as parameter. The sample code below illustrates how to create an object based on the [ProjectFile](../../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectFile.yml) class and how various properties can be applied to retrieve file information such as the name, the usage, the folder it is stored in, etc.:

```cs
string fileInfo;
ProjectFile thisFile = project.GetFile(fileId);
fileInfo = "File name: " + thisFile.Name + "\n";
fileInfo += "File usage: " + thisFile.Role.ToString() + "\n";
fileInfo += "Belongs to project with the id: " + thisFile.ProjectId.ToString() + "\n";
fileInfo += "Language: " + thisFile.Language.DisplayName + "\n";
fileInfo += "Stored in folder: " + thisFile.LocalFilePath + "\n";
fileInfo += "Unique file id: " + thisFile.Id.ToString() + "\n";
MessageBox.Show(fileInfo);
```

The actual file replacement is carried out by applying the [AddNewFileVersion](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetFile_System_Guid_) method to the project. This method requires the unique file id (guid) and the name and path of the new file (string) as parameters:

```cs
project.AddNewFileVersion(fileId, newFileName);
```

Note that when applying this method the original file will be overwritten with the updated file within the project folder structure, in particular in the source language sub-folder of the project. However, it will not be automatically processed, e.g. it will not be converted to SDL XLIFF. Depending on when the updated file is added, you might have to re-run tasks such as convert to translatable format, copy to target languages, etc. again:

```cs
Guid[] fileIds = { fileId };

project.RunAutomaticTask(fileIds, AutomaticTaskTemplateIds.ConvertToTranslatableFormat);
project.RunAutomaticTask(fileIds, AutomaticTaskTemplateIds.CopyToTargetLanguages);
```

Note that when you provide a different file name in the method, the file that was originally added, will not be replaced. The update file will rather be added as a completely new file to the project, and it will obtain a new id. This file will also have to be processed by (re-)running the various tasks.

```cs
project.AddNewFileVersion(fileId, newFileName);
```

See Also
--
**Other Resources**

[Adding Files and Folders](adding_files_and_folders.md)

[Adding Files in the Folder to the Project](../\developing_a_sample_app\adding_file_in_the_folder_to_the_project.md)