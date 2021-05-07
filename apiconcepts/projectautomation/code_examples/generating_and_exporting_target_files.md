Generating and Exporting Target Files
==

At the end of a project lifecycle you will usually generate the native file formats from the bilingual (SDL XLIFF) documents. In addition you may want to copy the native target files to a specified location for delivery to the end customer. This is accomplished with two automatic tasks, i.e. the generation of the native document format and exporting the finalized files to a specific folder.

The screenshot below illustrates the options that you can configure in <Var:ProductName> when exporting the finished documents to a specific location.



To programmatically configure the task settings implement a helper function called ```GetExportTaskSettings```, which takes a [FileBasedProject]() object as parameter. The settings for a particular task are saved within the project. The settings can either apply to the entire project or can be target-language specific, e.g. when the German files require different settings than the French files. Each project is associated with a settings bundle, which contains the settings for all tasks (e.g. analyze, pre-translate, etc.). First, create a ```ISettingsBundle``` object by applying the GetSettings method to the project object. Then apply the ```GetSettingsGroup``` method to generate a settings object based on the [ExportFilesSettings]() class:

```CS
ISettingsBundle settings = project.GetSettings();
ExportFilesSettings exportTaskSettings = settings.GetSettingsGroup<ExportFilesSettings>();
```

Note that for the generate target translations task there are no settings to configure, as it simply generates the native documents from the bilingual (SDL XLIFF) files. For the export files task you can configure two settings:

**Configuring the Export File Path**

Through the [ExportLocation]() property you can set the folder to which the files should be exported, for example:

```CS
exportTaskSettings.ExportLocation.Value = @"c:\temp";
```

**Selecting the Export File Version**

Through the [ExportFileVersion]() property you can set which version of the file should actually be exported. It is possible to not only export the native target document, but also the most up-to-date bilingual (SDL XLIFF) document. Through [ExportFileVersion]() you can access the three available options:
* [Current](): Exports the file in whatever state it is currently, i.e. native or bilingual.
* [Native](): Exports the native document version (if available), e.g. DOC, PPT, MIF, INX, etc. This means that the export task needs to be run after a generate target documents task has been executed.
* [Bilingual](): Exports the latest bilingual version (i.e. SDL XLIFF).

```CS
exportTaskSettings.ExportFileVersion.Value = ExportFileVersion.Bilingual;
```

Last, you need to apply the settings to the project through the [UpdateSettings]() method, so that the settings are persisted in the project.

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
**Other Resources**

Analyze Files Settings

Project TM Creation Settings

Pre-translate Settings

Perfect Match

Update Translation Memory Settings

Translation Count

Pre-translate Settings