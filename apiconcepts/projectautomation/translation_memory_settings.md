Translation Memory Settings
==

Through the API you can also fine-tune the translation memory settings for the entire project or for each language pair. Example: The default minimum match value for TM searches is 70%. This means that by default <Var:ProductName> only offers fuzzy matches if they have a score of 70% or above. Depending on the project (or target language) requirements, however, you may use a lower or a higher minimum fuzzy value.

Implement a function called ```ConfigureTmSettings```, which takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object as parameter. Within this function apply the ```GetSettings``` method to the project to create a settings bundle object (```ISettingsBundle```) for the project. In the next step create an object derived from [TranslationMemorySettings](../../api/projectautomation/Sdl.ProjectAutomation.Settings.TranslationMemorySettings.yml) through which you can configure the various TM settings as shown in the code example below:

# [C#](#tab/tabid-1)
```CS
ISettingsBundle settings = project.GetSettings();
TranslationMemorySettings tmSettings = settings.GetSettingsGroup<TranslationMemorySettings>();
```
***

After that, you can apply the various properties available for fine-tuning the TM settings. The following chapters provide examples of the TM settings that can be configured for a project.
As mentioned above, you can also create translation memory settings specifically for a particular language direction. In that case you need to provide a [Language](../../api/core/Sdl.Core.Globalization.Language.yml) object as parameter when creating the target language-specific settings bundle as shown in the example below:

# [C#](#tab/tabid-2)
```CS
ISettingsBundle deSettings = project.GetSettings(new Language(CultureInfo.GetCultureInfo("de-DE")));
```
***

After applying the various TM settings you need to make sure to update the project settings by applying the [UpdateSettings](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_UpdateSettings_Sdl_Core_Globalization_Language_Sdl_Core_Settings_ISettingsBundle_) method to the project as shown below:

# [C#](#tab/tabid-3)
```CS
project.UpdateSettings(settings);
```
***

Note that there is a large number of TM settings available. Some of these settings (e.g. filters) cannot be handled by the Project Automation API alone, but require the Translation Memory API.

See Also
--
[Translation Memory Search Settings](translation_memory_search_settings.md)

[Setting TM Penalties](setting_tm_penalties.md)

[Translation Memory Fields Update](translation_memory_field_update.md)

[Translation Memory Filter Settings](translation_memory_filter_settings.md)

[Project Configuration](project_configuration.md)