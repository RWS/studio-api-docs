Release Notes for <Var:ProductNameWithEdition>
===================

# Retargeted assemblies
Some of the available APIs provided along with <Var:ProductNameWithEdition> have been retargeted from .NET Framework 4.8 to .NET Standard for better compatibility options and richer overall support.

At the time of this release, the retargeted assemblies are as follows:
| Parent API                           | Assembly                                             |
|--------------------------------------|------------------------------------------------------|
| Integration API                      | `Sdl.TranslationStudioAutomation.IntegrationApi.dll` |
| LanguageCloud Identity API           | `Sdl.LanguageCloud.IdentityAPI.dll`                  |


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
To move away from OS generated languge registry we crated an internal langauge registry where we can control the languages provided to RWS applciation. 

We removed the usage of CultureInfo from our public API's and replace it with a custom object called [CultureCode](../../api/core/Sdl.Core.Globalization.CultureCode.yml). 

To ensure compatibility with Studio and other RWS system interfacing with Studio please fetch the language info using our internal language registry : 

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
This method will return a [Language](../../api/core/Sdl.Core.Globalization.Language.yml) object. 
In case the language is not found or the language code is incorrect it will throw an [UnsupportedLanguageException](../../api/core/Sdl.Core.Globalization.UnsupportedLanguageException.yml) exception.

In case you need the CultureInfo for that language you can retrieve it like this :

Example:

    ```cs
    var ci = LanguageRegistryApi.Instance.GetLanguage("fr-FR").CultureInfo;        
    ```
> [!NOTE]
> Avoid using the .NET runtime language registry, see below :
>
>Example: 
>
>    ```cs
>    // These lines create CultureInfo from .NET runtime, so results can vary across platforms and Windows OS versions
>    var ci = new CultureInfo("en-US");
>    ci = CultureInfo.GetCultureInfo("en-US");      
>    ```

To create a wrapper arround the lanugage code avoid possible error when comparing codes you can use [CultureCode](../../api/core/Sdl.Core.Globalization.CultureCode.yml)

Example: 

    ```cs
    var cultureCode = new CultureCode("fr-FR");
    ```
