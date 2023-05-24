Release Notes for <Var:ProductNameWithEdition> (SR1 ?)
===================

# Retargeted assemblies
Some of the available APIs provided along with <Var:ProductNameWithEdition> have been retargeted from .NET Framework 4.8 to .NET Standard for better compatibility options and richer overall support.

At the time of this release, the retargeted assemblies are as follows:
| Parent API                           | Assembly                                             |
|--------------------------------------|------------------------------------------------------|
| Integration API                      | `Sdl.TranslationStudioAutomation.IntegrationApi.dll` |
| LanguageCloud Identity API           | `Sdl.LanguageCloud.IdentityAPI.dll`                  |
| Terminology.TerminologyProvider.Core | `Sdl.Terminology.TerminologyProvider.Core.dll`       |

[ProjectsController](../..//api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml)

# ProjectAutomation API
These changes are included in the `Sdl.ProjectAutomation.Settings` assembly

* The Sdl.ProjectAutomation.Settings assembly now includes a [BatchProcessingGeneralSettings](../../api/projectautomation/Sdl.ProjectAutomation.Settings.BatchProcessingGeneralSettings.yml) class which determines whether the Pre-Translate and Analyze batch tasks should use subsegment optimization.
* Completely removed the following properties marked as obsolete in previous versions:
    - ProjectsController.CurrentProjectChanging
    - TranslationMemoryUpdateTaskSettings.AlwaysAddNewTranslation
    - TranslationMemoryUpdateTaskSettings.OverwriteExistingTranslation
    - TranslationMemoryUpdateTaskSettings.LeaveUnchangedTranslation 
    - TranslationMemoryUpdateTaskSettings.KeepMostRecentTranslation
# LanguageCloud Identity API 
These changes are included in the `Sdl.LanguageCloud.IdentityAPI` assembly

Completely removed the following property marked as obsolete in previous versions:

* LanguageCloudIdentityApi.ApiKey
# TranslationMemory API 
These changes are included in the `Sdl.LanguagePlatfrom.TranslationMemoryAPI` assembly

The following unused classes were removed: 
* ImportExportResponse
* ScheduledFieldApplyOperation
* ScheduledLanguageResourcesApplyOperation
* TranslationMemoryQueryFilters

The following unused enums were removed: 
* LanguageDirectionProperties
* DatabaseServerProperties
* FieldsTemplateProperties
* LanguageResourcesTemplateProperties
* ContainerProperties
* TranslationSequenceProperties

The following unused properties were removed:
* ScheduledServerTranslationMemoryImport.FileName 
* ServerBasedFieldsTemplate.TranslationMemories
* ServerBasedFieldsTemplate.CurrentFieldApplyOperation
* ServerBasedLanguageResourcesTemplate.CurrentLangResApplyOperation 

The following methods were marked obsolete and will be removed in a future release:
* FieldDefinition.ctor(Field field, bool isReadOnly);
* TranslationProviderServer.GetDatabaseServer(Guid id, DatabaseServerProperties additionalProperites)
* TranslationProviderServer.GetDatabaseServer(string path, DatabaseServerProperties additionalProperties)
* TranslationProviderServer.GetContainer(Guid id, ContainerProperties additionalProperties) 
* TranslationProviderServer.GetContainer(string path, ContainerProperties additionalProperties) 
* TranslationProviderServer.GetContainers(ContainerProperties additionalProperties)
* TranslationProviderServer.GetFieldsTemplate(Guid id, FieldsTemplateProperties additionalProperties)
* TranslationProviderServer.GetFieldsTemplate(string path, FieldsTemplateProperties additionalProperties)
* TranslationProviderServer.GetLanguageResourcesTemplate(Guid id, LanguageResourcesTemplateProperties additionalProperties)
* TranslationProviderServer.GetLanguageResourcesTemplate(string path, LanguageResourcesTemplateProperties additionalProperties)
* TranslationProviderServer.GetContainer(string path, ContainerProperties additionalProperties)
* TranslationProviderServer.GetTranslationMemories(TranslationMemoryProperties additionalProperties) 
* TranslationProviderServer.GetDatabaseServers(DatabaseServerProperties additionalProperties)
* TranslationProviderServer.GetFieldsTemplates(FieldsTemplateProperties additionalProperties, bool includeTmSpecific = true)
* TranslationProviderServer.GetLanguageResourcesTemplates(LanguageResourcesTemplateProperties additionalProperties,bool includeTmSpecific = true)
* TranslationProviderServer.DeleteBackgroundTask(Guid taskId)
* TranslationProviderServer.DeleteBackgroundTasks(ICollection<Guid> tasksIdentities)

For more information please see [FieldDefinition](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemory.FieldDefinitions.yml) and [TranslationProviderServer]() **TODO: does not have API entry**

The following obsolete methods were removed:
* TranslationProviderServer.GetDefaultLanguageResources(CultureInfo language)  
* TranslationProviderServer.GetDefaultLanguageResources(string languageCode)
* TranslationProviderServer.GetTranslationMemoriesQueryFilters()

Recommended replacements for deprecated API. Please refer to the following table:

| Removed API                                                                                         | Recommended replacement                              |
|-----------------------------------------------------------------------------------------------------|------------------------------------------------------|
| GetDatabaseServer(Guid id, DatabaseServerProperties additionalProperites)                           | GetDatabaseServer(Guid id)                           |
| GetDatabaseServer(string path, DatabaseServerProperties additionalProperties)                       | GetDatabaseServer(string path)                       |
| GetContainer(Guid id, ContainerProperties additionalProperties)                                     | GetContainer(Guid id)                                |
| GetContainer(string path, ContainerProperties additionalProperties)                                 | GetContainer(string path)                            |
| GetContainers(ContainerProperties additionalProperties)                                             | GetContainers()                                      |
| GetFieldsTemplate(Guid id, FieldsTemplateProperties additionalProperties)                           | GetFieldsTemplate(Guid id)                           |
| GetFieldsTemplate(string path, FieldsTemplateProperties additionalProperties)                       | GetFieldsTemplate(string path)                       |
| GetLanguageResourcesTemplate(Guid id, LanguageResourcesTemplateProperties additionalProperties)     | GetLanguageResourcesTemplate(Guid id)                |
| GetLanguageResourcesTemplate(string path, LanguageResourcesTemplateProperties additionalProperties) | GetLanguageResourcesTemplate(string path)            |
| GetTranslationMemories(TranslationMemoryProperties additionalProperties)                            | GetTranslationMemories()                             |
| GetDatabaseServers(DatabaseServerProperties additionalProperties)                                   | GetDatabaseServers()                                 |
| GetFieldsTemplates(FieldsTemplateProperties additionalProperties, bool includeTmSpecific = true)    | GetFieldsTemplates(bool includeTmSpecific = true)    |
| GetLanguageResourcesTemplates(LanguageResourcesTemplateProperties additionalProperties,bool includeTmSpecific = true)| GetLanguageResourcesTemplates(bool includeTmSpecific = true)|