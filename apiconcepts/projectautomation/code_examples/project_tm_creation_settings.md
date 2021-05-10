Project TM Creation Settings
==

A project translation memory is a subset of a main TM, which contains only TUs that according to the file analysis were found to be relevant for the source documents of a project. Project TMs are not required for standalone scenarios, but are usually created for projects that involve multiple players, e.g. a project manager, external translators, editors, proofreaders, etc. Project TMs are then useful for two main reasons:

* Project TMs are smaller than the main TMs and thus easier to send e.g. in a package via e-mail
* Through project TMs you can make sure that external translators only receive the content that is relevant for the project that they are working on, and no other content, which may be confidential.

Configuring the Task Settings
--

As illustrated in the following screenshot the following settings can be configured for project TMs.

![SettingsProjectTms](images/SettingsProjectTms.jpg)


Project TMs can be either file-based (default) or server-based. Even if you select a server master TM, <Var:ProductName> will still create a file-based project TM by default. Project TMs are named after the project and the main TM. For example, if your main TM is called *Master* and the project is called *My first project*, the resulting project TM name will be *My first project_Master.sdltm*. If a project TM is created for e.g. the target language German, it will be stored in a *de-DE* sub-folder of the project folder structure, e.g. *Project_Name\TM\de-DE*.

If you want to create a server-based project TM (so that geographically distributed translators can work together more effectively), you need to provide the server URI and the database container name. The database container is the physical database that contains the TMs, which are stored as tables.

To access the project TM creation task settings programmatically, implement a helper function called ```GetProjectTmTaskSettings```, which takes a [FileBasedProject](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object as parameter. The settings for a particular task are saved within the project. First, retrieve a ```ISettingsBundle``` object by applying the GetSettings method to the project object. Then apply the GetSettingsGroup method to generate a settings object based on the [ProjectTranslationMemoryTaskSettings](../../../api/projectautomation/Sdl.ProjectAutomation.Settings.ProjectTranslationMemoryTaskSettings.yml) class:

```CS
ISettingsBundle settings = project.GetSettings();
ProjectTranslationMemoryTaskSettings projectTmSettings = settings.GetSettingsGroup<ProjectTranslationMemoryTaskSettings>();
```

The sample code below illustrates the properties that you need to set in order to create a server-based project TM:

* [CreateServerBasedProjectTranslationMemories](../../../api/projectautomation/Sdl.ProjectAutomation.Settings.ProjectTranslationMemoryTaskSettings.yml#Sdl_ProjectAutomation_Settings_ProjectTranslationMemoryTaskSettings_CreateServerBasedProjectTranslationMemories): Setting this property to True will cause the creation of a server project TM rather than a file-based TM, which is the default option.
* [ServerConnectionUri](../../../api/projectautomation/Sdl.ProjectAutomation.Settings.ProjectTranslationMemoryTaskSettings.yml#Sdl_ProjectAutomation_Settings_ProjectTranslationMemoryTaskSettings_ServerConnectionUri): The URI required to connect to the TM Server on which the project TM should be created. This will usually be the server that also hosts the main TMs.
* [TranslationMemoryContainerName](../../../api/projectautomation/Sdl.ProjectAutomation.Settings.ProjectTranslationMemoryTaskSettings.yml#Sdl_ProjectAutomation_Settings_ProjectTranslationMemoryTaskSettings_TranslationMemoryContainerName): The name of the physical database in which the project TM should be created. Note that the TMs are stored as tables within a database.

```CS
projectTmSettings.CreateServerBasedProjectTranslationMemories.Value = true;
projectTmSettings.ServerConnectionUri.Value = string.Empty;
projectTmSettings.TranslationMemoryContainerName.Value = "TMCont";
```

After configuring all task settings you need to update the project by applying the [UpdateSettings](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_UpdateSettings_Sdl_Core_Globalization_Language_Sdl_Core_Settings_ISettingsBundle_) method:

```CS
project.UpdateSettings(settings);
```

>**Note**
>
>File TMs are created in a Tm sub-folder of the project folder strcture. A German project TM would, for example, be stord in a sub-folder called *Tm/de-DE*. The project TM name is a combination of the project name and the name of the main TM from which the project TM was generated.

Putting it All Together
--

```CS
public void GetProjectTmTaskSettings(FileBasedProject project)
{
    #region "TaskSettings"
    ISettingsBundle settings = project.GetSettings();
    ProjectTranslationMemoryTaskSettings projectTmSettings = settings.GetSettingsGroup<ProjectTranslationMemoryTaskSettings>();            
    #endregion

    #region "ProjectTmSettings"
    projectTmSettings.CreateServerBasedProjectTranslationMemories.Value = true;
    projectTmSettings.ServerConnectionUri.Value = string.Empty;
    projectTmSettings.TranslationMemoryContainerName.Value = "TMCont";
    #endregion

    #region "UpdateTaskSettings"
    project.UpdateSettings(settings);
    #endregion
}
```

See Also
--

**Other Resources**

[Analyze Files Settings](analyze_files_settings.md)

[Pre-translate Settings](project_tm_creation_settings.md)

[Perfect Match](perfect_match.md)

[Update Translation Memory Settings](update_translation_memory_settings.md)

[Generating and Exporting Target Files](generating_and_exporting_target_files.md)

[Translation Count](translation_count.md)

[About Project Translation Memories](..\about_project_translation_memories.md)