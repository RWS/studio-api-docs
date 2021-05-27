Generating and Exporting Target Files
==

At the end of a project lifecycle you will usually generate the native file formats from the bilingual (SDL XLIFF) documents. In addition you may want to copy the native target files to a specified location for delivery to the end customer. This is accomplished with two automatic tasks, i.e. the generation of the native document format and exporting the finalized files to a specific folder.

The screenshot below illustrates the options that you can configure in <Var:ProductName> when exporting the finished documents to a specific location.



To programmatically configure the task settings implement a helper function called ```GetExportTaskSettings```, which takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object as parameter. The settings for a particular task are saved within the project. The settings can either apply to the entire project or can be target-language specific, e.g. when the German files require different settings than the French files. Each project is associated with a settings bundle, which contains the settings for all tasks (e.g. analyze, pre-translate, etc.). First, create a ```ISettingsBundle``` object by applying the GetSettings method to the project object. Then apply the ```GetSettingsGroup``` method to generate a settings object based on the [ExportFilesSettings](../../api/projectautomation/Sdl.ProjectAutomation.Settings.ExportFilesSettings.yml) class:

```CS
ISettingsBundle settings = project.GetSettings();
ExportFilesSettings exportTaskSettings = settings.GetSettingsGroup<ExportFilesSettings>();
```

Note that for the generate target translations task there are no settings to configure, as it simply generates the native documents from the bilingual (SDL XLIFF) files. For the export files task you can configure two settings:

**Configuring the Export File Path**

Through the [ExportLocation](../../api/projectautomation/Sdl.ProjectAutomation.Settings.ExportFilesSettings.yml#Sdl_ProjectAutomation_Settings_ExportFilesSettings_ExportLocation) property you can set the folder to which the files should be exported, for example:

```CS
exportTaskSettings.ExportLocation.Value = @"c:\temp";
```

**Selecting the Export File Version**

Through the [ExportFileVersion](../../api/projectautomation/Sdl.ProjectAutomation.Settings.ExportFileVersion.yml) property you can set which version of the file should actually be exported. It is possible to not only export the native target document, but also the most up-to-date bilingual (SDL XLIFF) document. Through [ExportFileVersion](../../api/projectautomation/Sdl.ProjectAutomation.Settings.ExportFileVersion.yml) you can access the three available options:
* [Current](../../api/projectautomation/Sdl.ProjectAutomation.Settings.ExportFileVersion.yml#fields): Exports the file in whatever state it is currently, i.e. native or bilingual.
* [Native](../../api/projectautomation/Sdl.ProjectAutomation.Settings.ExportFileVersion.yml#fields): Exports the native document version (if available), e.g. DOC, PPT, MIF, INX, etc. This means that the export task needs to be run after a generate target documents task has been executed.
* [Bilingual](../../api/projectautomation/Sdl.ProjectAutomation.Settings.ExportFileVersion.yml#fields): Exports the latest bilingual version (i.e. SDL XLIFF).

```CS
exportTaskSettings.ExportFileVersion.Value = ExportFileVersion.Bilingual;
```

Last, you need to apply the settings to the project through the [UpdateSettings](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_UpdateSettings_Sdl_Core_Globalization_Language_Sdl_Core_Settings_ISettingsBundle_) method, so that the settings are persisted in the project.

```CS
project.UpdateSettings(settings);
```

Putting it All Together
--

The complete function should look as shown below:

```CS
public void GetExportTaskSettings(FileBasedProject project)
{
    #region "ExportTaskSettings"
    ISettingsBundle settings = project.GetSettings();
    ExportFilesSettings exportTaskSettings = settings.GetSettingsGroup<ExportFilesSettings>();
    #endregion

    #region "ExportLocation"
    exportTaskSettings.ExportLocation.Value = @"c:\temp";
    #endregion

    #region "ExportFileVersion"
    exportTaskSettings.ExportFileVersion.Value = ExportFileVersion.Bilingual;
    #endregion

    #region "UpdateSettings"
    project.UpdateSettings(settings);
    #endregion
}
```

See Also
--


[Analyze Files Settings](analyze_files_settings.md)

[Project TM Creation Settings](project_tm_creation_settings.md)

[Pre-translate Settings](pre_translate_settings.md)

[Perfect Match](perfect_match.md)

[Update Translation Memory Settings](update_translation_memory_settings.md)

[Translation Count](translation_count.md)