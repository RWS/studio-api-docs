Release Notes for <Var:ProductNameWithEdition>
===================

# Retargeted assemblies
Some of the available APIs provided along with <Var:ProductNameWithEdition> have been retargeted from .NET Framework 4.8 to .NET Standard for better compatibility options and richer overall support.

At the time of this release, the retargeted assemblies are as follows:
| Parent API                           | Assembly                                             |
|--------------------------------------|------------------------------------------------------|
| Integration API                      | `Sdl.TranslationStudioAutomation.IntegrationApi.dll` |
| LanguageCloud Identity API           | `Sdl.LanguageCloud.IdentityAPI.dll`                  |


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
These changes are included in the `Sdl.LanguageCloud.IdentityAPI` assembly.

Completely removed the following property marked as obsolete in previous versions:

* LanguageCloudIdentityApi.ApiKey


# TranslationMemory API 
These changes are included in the `Sdl.LanguagePlatfrom.TranslationMemoryAPI` assembly.

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

The following obsolete methods were removed:
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
* TranslationProviderServer.GetDefaultLanguageResources(CultureInfo language)  
* TranslationProviderServer.GetDefaultLanguageResources(string languageCode)
* TranslationProviderServer.GetTranslationMemoriesQueryFilters()

For more information please see [FieldDefinition](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemory.FieldDefinitions.yml) and [TranslationProviderServer](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml) 

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

# General API changes 
Third-party developers now have access to Trados Studio's custom language registry, which offers finer control over language management than the language registry provided by Microsoft.

Following this change, [CultureCode](../../api/core/Sdl.Core.Globalization.CultureCode.yml) is now the recommended alternative to the standard CultureInfo. 

To ensure compatibility with Studio and other RWS system interfacing with Studio, fetch the language info using our internal language registry: 

Example:

    ```cs
    try
    {
        var language = LanguageRegistryApi.Instance.GetLanguage("fr-FR");        
    }
    catch(UnsupportedLanguageException ex)
    {
        // language is not suported
    }
    ```
This method returns a [Language](../../api/core/Sdl.Core.Globalization.Language.yml) object. 
If the language is not found or the language code is incorrect [UnsupportedLanguageException](../../api/core/Sdl.Core.Globalization.UnsupportedLanguageException.yml) is thrown.

CultureInfo objects are still accessible via the following call:

Example:

    ```cs
    var ci = LanguageRegistryApi.Instance.GetLanguage("fr-FR").CultureInfo;        
    ```

 Use of the .NET runtime language registry, such as the following example, is not recommended as it may have unexpected results.

Example: 

    ```cs
    // These lines create CultureInfo from .NET runtime, so results can vary across platforms and Windows OS versions
    var ci = new CultureInfo("en-US");
    ci = CultureInfo.GetCultureInfo("en-US");      
    ```
                                                        ??
To ensure consistency across the application when comparing the string representation of the language codes, we recommend using the [CultureCode](../../api/core/Sdl.Core.Globalization.CultureCode.yml) wrapper.

Example: 

    ```cs
    var cultureCode = new CultureCode("fr-FR");
    ```
