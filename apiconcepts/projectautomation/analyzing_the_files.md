Analyzing the Files
==

In this chapter we will learn how to implement the functionality for performing the actual file analysis. Note that the analyze files task is applied to the SDL XLIFF files that were previously copied to the target folder. In a project that involves multiple target languages, an analyze files task needs to be applied separately to each set of target documents.

Implement another helper function called RunFileAnalysis, which takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object as parameter as well as the target language locale as string parameter:

# [C#](#tab/tabid-1)
```cs
/// <summary>
/// Runs the actual analyze files task on the SDL XLIFF target documents.
/// </summary> 
private void RunFileAnalysis(FileBasedProject project, string trgLocale)
```
***

First, retrieve the target files by applying the [GetTargetLanguageFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetTargetLanguageFiles) method to the project, which takes the appropriate target ```Language``` object as parameter:

# [C#](#tab/tabid-2)
```cs
ProjectFile[] targetFiles = project.GetTargetLanguageFiles(new Language(CultureInfo.GetCultureInfo(trgLocale)));
```
***

In the next step, we perform the actual analysis task by applying [RunAutomaticTask](../..//api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_RunAutomaticTask_System_Guid___System_String_) to the project, which takes the file ids and the automatic task id (i.e. [AnalyzeFiles](../../api/projectautomation/Sdl.ProjectAutomation.Core.AutomaticTaskTemplateIds.yml#Sdl_ProjectAutomation_Core_AutomaticTaskTemplateIds_AnalyzeFiles)) as parameters:

# [C#](#tab/tabid-3)
```cs
AutomaticTask analyzeTask = project.RunAutomaticTask(
    targetFiles.GetIds(),
    AutomaticTaskTemplateIds.AnalyzeFiles);
```
***

See Also
--

[Configuring the Analyze Task Settings](configuring_the_analyze_task_settings.md)

[Analyze Files Settings](analyze_files_settings.md)