# Converting the Project Files

This step generates translatable, bilingual documents (SDLXliff files) from all translatable source documents added to the project. It then creates target copies for further processing, such as file analysis.

Implement another helper function called ```ConvertFiles```, which takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object as parameter. First, retrieve the file IDs for conversion and copying to a target-language folder using the [GetSourceLanguageFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetSourceLanguageFiles) method on the project:

# [C#](#tab/tabid-1)
```cs
ProjectFile[] files = project.GetSourceLanguageFiles();
```
***

Next, apply the [RunAutomaticTask](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_RunAutomaticTask_System_Guid___System_String_) method to run the task that generates SDLXliff files from translatable source documents. This method requires the file IDs and the task ID—here [ConvertToTranslatableFormat](../../api/projectautomation/Sdl.ProjectAutomation.Core.AutomaticTaskTemplateIds.yml#Sdl_ProjectAutomation_Core_AutomaticTaskTemplateIds_ConvertToTranslatableFormat) as parameters. Apply the conversion only to translatable files—converting a file without a file type definition (such as a reference file) throws an exception. Loop through the source documents and run the convert task only when the [FileRole](../../api/projectautomation/Sdl.ProjectAutomation.Core.FileRole.yml) equals [Translatable](../../api/projectautomation/Sdl.ProjectAutomation.Core.FileRole.yml#fields):

# [C#](#tab/tabid-2)
```cs
for (int i = 0; i < project.GetSourceLanguageFiles().Length; i++)
{
    if (files[i].Role == FileRole.Translatable)
    {
        Guid[] currentFileId = { files[i].Id };
        AutomaticTask convertTask = project.RunAutomaticTask(
            currentFileId,
            AutomaticTaskTemplateIds.ConvertToTranslatableFormat);
        AutomaticTask copyTask = project.RunAutomaticTask(
            currentFileId,
            AutomaticTaskTemplateIds.CopyToTargetLanguages);
    }
}
```
***

Note that the [RunAutomaticTask](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_RunAutomaticTask_System_Guid___System_String_) method requires an array of file guids, even if we process only one file at a time. For this reason, we create an array of file ids and fill it with just the id of the current file. Within this loop, we also run the [CopyToTargetLanguages](../../api/projectautomation/Sdl.ProjectAutomation.Core.AutomaticTaskTemplateIds.yml#Sdl_ProjectAutomation_Core_AutomaticTaskTemplateIds_CopyToTargetLanguages) task, which means that the SDLXliff files that we created during the conversion step are copied into a target language sub-folder, i.e.:

# [C#](#tab/tabid-3)
```cs
AutomaticTask copyTask = project.RunAutomaticTask(
    currentFileId,
    AutomaticTaskTemplateIds.CopyToTargetLanguages);
```
***

For example, when the target language is German, a sub-folder called *de-DE* will be automatically created by this task. The SDLXliff files will then be copied into this sub-folder. Note that you could run the copy task also on untranslatable files, as this involves only copying the files to a different sub-folder of the project. Apart from that no further processing is performed on the files. So, if you try to copy an untranslatable file, no exception will be thrown. Copying untranslatable files to the target sub-folder is often required, as translators also need any untranslatable files as reference material. However, in our implementation, copying reference files would only create files that are not required for the batch analysis anyway, and would therefore only take hard disk space unnecessarily. Also, the subsequent analyze task will be facilitated by only copying the translatable files, as we then do not need to check on the file usage before running the analyze files task.

The complete function should look as shown below:

# [C#](#tab/tabid-4)
```cs
/// <summary>
/// Runs the two automatic tasks: Convert translatable files to a translatable format (i.e. SDLXliff)
/// and creates target file copies.
/// </summary> 
private void ConvertFiles(FileBasedProject project)
{
    ProjectFile[] files = project.GetSourceLanguageFiles();
    for (int i = 0; i < project.GetSourceLanguageFiles().Length; i++)
    {
        if (files[i].Role == FileRole.Translatable)
        {
            Guid[] currentFileId = { files[i].Id };
            AutomaticTask convertTask = project.RunAutomaticTask(
                currentFileId,
                AutomaticTaskTemplateIds.ConvertToTranslatableFormat);
            AutomaticTask copyTask = project.RunAutomaticTask(
                currentFileId,
                AutomaticTaskTemplateIds.CopyToTargetLanguages);
        }
    }
}
```
***

## See Also

[Running Tasks on the Project Files](running_tasks_on_project_files.md)

[Automatic Tasks and Task Settings](automatic_tasks_and_task_settings.md)
