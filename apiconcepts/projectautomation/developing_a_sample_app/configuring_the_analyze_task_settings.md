Configuring the Analyze Task Settings
==

The automatic analyze files task can be configured through various parameters. In our implementation we will use two parameters: the reporting of cross-file repetitions and of the internal fuzzy match leverage. For detailed information on the available task settings, please see [Analyze Files Settings](../code_examples/analyze_files_settings.md).

Add a new helper function called GetAnalyzeSettings. Below you see the parameters that need to be provided for this function, i.e. the [FileBasedProject](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml)  object, the target locale string and boolean parameters for the two task reporting settings that are configured through the command-line interface:

```cs
/// <summary>
/// Configures the analyze task file settings, i.e. in our implementation
/// report cross-file repetitions and report the internal fuzzy match leverage.
/// </summary> 
private void GetAnalyzeSettings(
    FileBasedProject project,
    string trgLocale,
    bool reportCrossFileRepetitions,
    bool reportInternalFuzzyMatchLeverage)
```

The settings can be configured for each target language. As our implementation uses only a single target language, we actually do not need to provide the target language as parameter, however, for good measure we are going to demonstrate how you would work when you want to differentiate the settings per target language. First, create a ```Language``` object based on the target locale string:

```cs
Language trgLanguage = new Language(CultureInfo.GetCultureInfo(trgLocale));
```

All available settings (i.e. the settings for all project tasks as well as TM settings) are stored in a ```ISettingsBundle``` object, which acts as a container for all project settings. By applying the ```GetSettingsGroup``` method to the settings bundle, we create a specific ```ISettingsBundle``` object as shown below:

```cs
Language trgLanguage = new Language(CultureInfo.GetCultureInfo(trgLocale));
```

Then you configure the two settings that we use in our implementation by setting the values of the following properties:

```cs
analyzeSettings.ReportCrossFileRepetitions.Value = reportCrossFileRepetitions;
analyzeSettings.ReportInternalFuzzyMatchLeverage.Value = reportInternalFuzzyMatchLeverage;
```

In the last step you need to make sure to apply the [UpdateSettings](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_UpdateSettings_Sdl_Core_Globalization_Language_Sdl_Core_Settings_ISettingsBundle_) method to update the project settings accordingly:

```cs
project.UpdateSettings(trgLanguage, settings);
```

Note that if the settings should be updated for the entire project, then you only need to provide the settings bundle object as parameter. If the settings are target language-specific you also need to provide the target language object.

The complete helper function should now look as shown below:

```cs
#region "GetAnalyzeSettingsFunction"
/// <summary>
/// Configures the analyze task file settings, i.e. in our implementation
/// report cross-file repetitions and report the internal fuzzy match leverage.
/// </summary> 
private void GetAnalyzeSettings(
    FileBasedProject project,
    string trgLocale,
    bool reportCrossFileRepetitions,
    bool reportInternalFuzzyMatchLeverage)
#endregion
{
    #region "trgLanguage"
    Language trgLanguage = new Language(CultureInfo.GetCultureInfo(trgLocale));
    #endregion

    #region "ISettingsBundle"
    ISettingsBundle settings = project.GetSettings(trgLanguage);
    AnalysisTaskSettings analyzeSettings = settings.GetSettingsGroup<AnalysisTaskSettings>();
    #endregion

    #region "ConfigureSettings"
    analyzeSettings.ReportCrossFileRepetitions.Value = reportCrossFileRepetitions;
    analyzeSettings.ReportInternalFuzzyMatchLeverage.Value = reportInternalFuzzyMatchLeverage;
    #endregion

    #region "UpdateSettings"
    project.UpdateSettings(trgLanguage, settings);
    #endregion
}
```

See Also
--

**Other Resources**

[Analyze Files Settings](..\code_examples\analyze_files_settings.md)