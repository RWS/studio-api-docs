Converting the Project Files
==

In this step we will generate translatable, bilingual documents (i.e. SDL XLIFF files) from all translatable documents that have previously been added to our project. After that, we will create target copies of the translatable files, which will then be used for further processing, i.e. for the file analysis.

Implement another helper function called ```ConvertFiles```, which takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object as parameter. First, we need to retrieve the ids of the files to which the two automatic tasks (i.e. conversion and copy to a target-language folder) should be applies. To do this we use the [GetSourceLanguageFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetSourceLanguageFiles) method on the project:

```cs
ProjectFile[] files = project.GetSourceLanguageFiles();
```

In the next step we apply the [RunAutomaticTask](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_RunAutomaticTask_System_Guid___System_String_) method to the project in order to run the task that generates the SDL XLIFF files from the translatable source documents. This method requires the ids of the files to process as well as the task id, here [ConvertToTranslatableFormat](../../api/projectautomation/Sdl.ProjectAutomation.Core.AutomaticTaskTemplateIds.yml#Sdl_ProjectAutomation_Core_AutomaticTaskTemplateIds_ConvertToTranslatableFormat) as parameters. Before we actually convert the source files to a translatable format, we need to make sure that this task is only applied to translatable files. If you try to convert a file for which no file type definition exists (i.e. a reference file), an exception will be thrown. For this reason, we loop through the source documents and only apply the convert task if the [FileRole](../../api/projectautomation/Sdl.ProjectAutomation.Core.FileRole.yml) equals [Translatable](../../api/projectautomation/Sdl.ProjectAutomation.Core.FileRole.yml#fields):


```cs
for (int i = 0; i < project.GetSourceLanguageFiles().Length; i++)
{
    if (files[i].Role == FileRole.Translatable)
    {
        Guid[] currentFileId = { files[i].Id };
        AutomaticTask convertTask = project.RunAutomaticTask(
            currentFileId,
            AutomaticTaskTemplateIds.ConvertToTranslatableFormat);

        #region "CopyToTarget"
        AutomaticTask copyTask = project.RunAutomaticTask(
            currentFileId,
            AutomaticTaskTemplateIds.CopyToTargetLanguages);
        #endregion
    }
}
```

Note that the [RunAutomaticTask](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_RunAutomaticTask_System_Guid___System_String_) method requires an array of file guids, even if we process only one file at a time. For this reason, we create an array of file ids and fill it with just the id of the current file. Within this loop, we also run the [CopyToTargetLanguages](../../api/projectautomation/Sdl.ProjectAutomation.Core.AutomaticTaskTemplateIds.yml#Sdl_ProjectAutomation_Core_AutomaticTaskTemplateIds_CopyToTargetLanguages) task, which means that the SDL XLIFF files that we created during the conversion step are copied into a target language sub-folder, i.e.:

```cs
AutomaticTask copyTask = project.RunAutomaticTask(
    currentFileId,
    AutomaticTaskTemplateIds.CopyToTargetLanguages);
```

For example, when the target language is German, a sub-folder called *de-DE* will be automatically created by this task. The SDL XLIFF files will then be copied into this sub-folder. Note that you could run the copy task also on untranslatable files, as this involves only copying the files to a different sub-folder of the project. Apart from that no further processing is performed on the files. So, if you try to copy an untranslatable file, no exception will be thrown. Copying untranslatable files to the target sub-folder is often required, as translators also need any untranslatable files as reference material. However, in our implementation, copying reference files would only create files that are not required for the batch analysis anyway, and would therefore only take hard disk space unnecessarily. Also, the subsequent analyze task will be facilitated by only copying the translatable files, as we then do not need to check on the file usage before running the analyze files task.

The complete function should look as shown below:

```cs
/// <summary>
/// Runs the two automatic tasks: Convert translatable files to a translatable format (i.e. SDL XLIFF)
/// and creates target file copies.
/// </summary> 
private void ConvertFiles(FileBasedProject project)
{
    #region "GetFilesForProcessing"
    ProjectFile[] files = project.GetSourceLanguageFiles();
    #endregion

    #region "RunConversion"
    for (int i = 0; i < project.GetSourceLanguageFiles().Length; i++)
    {
        if (files[i].Role == FileRole.Translatable)
        {
            Guid[] currentFileId = { files[i].Id };
            AutomaticTask convertTask = project.RunAutomaticTask(
                currentFileId,
                AutomaticTaskTemplateIds.ConvertToTranslatableFormat);

            #region "CopyToTarget"
            AutomaticTask copyTask = project.RunAutomaticTask(
                currentFileId,
                AutomaticTaskTemplateIds.CopyToTargetLanguages);
            #endregion
        }
    }
    #endregion
}
```

See Also
--

**Other Resources**


[Running Tasks on the Project Files](running_tasks_on_project_files.md)

[Automatic Tasks and Task Settings](automatic_tasks_and_task_settings.md)