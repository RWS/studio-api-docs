Translation Count
==

The translation count task is used to calculate the translation progress of a given current project. It determines the status (confirmation level) of the segments and words contained in the project files, i.e. it calculates how many segments, words, and characters have reached the confirmation levels e.g. translated, reviewed, signed-off, etc. This task is quite likely to be run several times during a project lifecycle in order to determine the current progress of a project. Note that for this task there are no specific settings.

The screenshot below illustrates how SDL Trados Studio 2017 visualizes the progress of a project by showing the number of translated, reviewed, etc. words using bar charts:

![Statistics](images/Statistics.jpg)

These bar charts are updated when running the translation count task, which happens, for example when the user triggers this task automatically or when a return package is imported. As return packages contain fully (or partly )translated, reviewed, and signed-off content, importing the package will trigger an update of the translation count statistics.

To run a translation count task for a particular target language programmatically implement a function that takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object and the target language locale string as parameters. The function below illustrates how to run the automatic task by applying the [RunAutomaticTask](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_RunAutomaticTask_System_Guid___System_String_) to your project. The method requires the target file ids and the task template id as parameters. The target language file ids are retrieved by applying the [GetTargetLanguageFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetTargetLanguageFiles) method to the project as shown in the example below:

```CS
public void RunTranslationCount(FileBasedProject project, string trgLocale)
{
    ProjectFile[] deFiles = project.GetTargetLanguageFiles(new Language(CultureInfo.GetCultureInfo(trgLocale)));

    AutomaticTask translationCountTask = project.RunAutomaticTask(
        deFiles.GetIds(),
        AutomaticTaskTemplateIds.TranslationCount);
}
```

See Also
--

**Other Resources**

[Analyze Files Settings](analyze_files_settings.md)

[Project TM Creation Settings](project_tm_creation_settings.md)

[Pre-translate Settings](pre_translate_settings.md)

[Perfect Match](perfect_match.md)

[Update Translation Memory Settings](update_translation_memory_settings.md)

[Generating and Exporting Target Files](generating_and_exporting_target_files.md)

[Pre-translate Settings](project_tm_creation_settings.md)

[Retrieving the Project Statistics](retrieving_the_project_statistics.md)