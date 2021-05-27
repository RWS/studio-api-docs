Automatic Tasks and Task Settings
==

he tasks performed in a project can be configured and customized through various settings to suit particular project requirements. For example, the pre-translate task by default only processes exact matches and context matches. However, through a specific setting you can also allow this task to process e.g. matches down to 95%. For another project, a minimum match value of 85% might be suitable.

Such settings can be configured globally for the entire project, thus all language pairs. Alternatively, you can use different task settings for different target languages if, for example, German requires a minimum match value of 95%, while for French you need to use a minimum match value of 99%.

The screenshot below illustrates how the minimum match value for the pre-translation task is specifically set for the language pair English to German in the New Project wizard of <Var:ProductName>.

![BatchTaskSettings](images/BatchTaskSettings.jpg)

This chapter provides an example of how to configure different settings for the pre-translation task for two different language pairs, e.g. English to German and English to French.

Implement a helper function called ```ConfigureBatchTaskSettings```, which takes three parameters:

* A [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object.
* The target language locale string
* The minimum match value


Since the setting that we are going to configure in this example should apply to specific target languages (and not to the project globally), we first create a ```Language``` object using the target locale string that is provided as a parameter:

# [C#](#tab/tabid-1)
```CS
Language trgLanguage = new Language(CultureInfo.GetCultureInfo(trgLocale));
```
***

Each project is associated with a settings bundle, which contains the settings for all tasks (e.g. pre-translate). First, create a ```ISettingsBundle``` object by applying the ```GetSettings``` method to the project. Then apply the ```GetSettingsGroup``` method to generate a settings object based on the [TranslateTaskSettings](../../api/projectautomation/Sdl.ProjectAutomation.Settings.TranslateTaskSettings.yml) class. In this case, we provide the target language as parameter. When you do not provide a parameter, the settings will apply to the entire project globally:

# [C#](#tab/tabid-2)
```CS
ISettingsBundle settings = project.GetSettings(trgLanguage);
TranslateTaskSettings pretranslateSettings = settings.GetSettingsGroup<TranslateTaskSettings>();
```
***

In the next step use the [MinimumMatchScore](../../api/projectautomation/Sdl.ProjectAutomation.Settings.TranslateTaskSettings.yml#Sdl_ProjectAutomation_Settings_TranslateTaskSettings_MinimumMatchScore) property to set the required minimum match value for the specified target language:

# [C#](#tab/tabid-3)
```CS
pretranslateSettings.MinimumMatchScore.Value = minMatchValue;
```
***

Finally, persist the settings changes by applying the [UpdateSettings](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_UpdateSettings_Sdl_Core_Globalization_Language_Sdl_Core_Settings_ISettingsBundle_) method to the project. In this method, provide the ```ISettingsBundle``` and the target language as parameters:

# [C#](#tab/tabid-4)
```CS
project.UpdateSettings(trgLanguage, settings);
```
***

Note that the above is just one example of a task configuration setting. You will find detailed information on available batch tasks and their settings on the bottom of this page under *See Also* . Note that the task settings are physically stored in the project (* *.sdlproj*) file.

Putting it All Together
--

The complete function should look as shown below:

# [C#](#tab/tabid-5)
```CS
public void ConfigureBatchTaskSettings(FileBasedProject project, string trgLocale, int minMatchValue)
{
    #region "SetLanguage"
    Language trgLanguage = new Language(CultureInfo.GetCultureInfo(trgLocale));
    #endregion

    #region "TaskSettings"
    ISettingsBundle settings = project.GetSettings(trgLanguage);
    TranslateTaskSettings pretranslateSettings = settings.GetSettingsGroup<TranslateTaskSettings>();
    #endregion

    #region "MinimumMatchScore"
    pretranslateSettings.MinimumMatchScore.Value = minMatchValue;
    #endregion

    #region "UpdateSettings"
    project.UpdateSettings(trgLanguage, settings);
    #endregion
}
```
***

See Also
--
[Pre-translate Settings](pre_translate_settings.md)

[Analyze Files Settings](analyze_files_settings.md)

[Project TM Creation Settings](project_tm_creation_settings.md)

[Update Translation Memory Settings](update_translation_memory_settings.md)

[Generating and Exporting Target Files](generating_and_exporting_target_files.md)

[Translation Count](translation_count.md)

[Configuring the Analyze Task Settings](configuring_the_project_properties.md)

[Project Configuration](project_configuration.md)