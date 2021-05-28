Adding the TM to the Project
===

In this step you will learn how to add the translation memory to our project that should be used for the batch analysis process.

Implement a separate helper function that we use to add the translation memory to the project. This function takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object and the full TM file name and path string as parameters:

# [C#](#tab/tabid-1)
```cs
private void AddTm(FileBasedProject project, string tmFilePath)
```
***

A project can be associated with a number of translation memories, which are contained in the translation provider configuration. The [TranslationProviderConfiguration](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetTranslationProviderConfiguration) acts as a container for the different kinds of translation providers that can be added to a project, e.g. TMs and Web-based translation services such as Google Translate. In our simple implementation, we will only add one TM to the configuration. Start by creating a translation provider configuration object as shown below:

# [C#](#tab/tabid-2)
```cs
TranslationProviderConfiguration config = project.GetTranslationProviderConfiguration();
```
***

The configuration can contain a cascade of providers. Create an object based on the [TranslationProviderCascadeEntry](../../api/projectautomation/Sdl.ProjectAutomation.Core.TranslationProviderCascadeEntry.yml). The parameters you need to provide here are as follows:
* The TM file name and path (string).
* A boolean parameter to indicate whether the TM should be used for segment lookups (True in our implementation).
* A boolean parameter to indicate whether the TM should be used for concordance searches (False, as concordance searching is not required in our implementation).
* A boolean parameter to indicate whether the TM should be updated (False in our implementation, as we use the TM only for analysis, not for adding new translation units or for updating TUs).

After creating the translation provider cascade entry, we add it to the configuration as shown below:

# [C#](#tab/tabid-3)
```cs
TranslationProviderCascadeEntry tm = new TranslationProviderCascadeEntry(
    tmFilePath,
    true,
    true,
    false);
config.Entries.Add(tm);

project.UpdateTranslationProviderConfiguration(config);
```
***

The complete helper function for adding the TM to the project should now look as follows:

# [C#](#tab/tabid-4)
```cs
/// <summary>
/// Adds the TM that should be used for the analysis. The project languages are
/// set according to the TM.
/// </summary> 
#region "AddTmFunction"
private void AddTm(FileBasedProject project, string tmFilePath)
#endregion
{
    #region "TranslationProviderConfiguration"
    TranslationProviderConfiguration config = project.GetTranslationProviderConfiguration();
    #endregion

    #region "TranslationProviderCascadeEntry"
    TranslationProviderCascadeEntry tm = new TranslationProviderCascadeEntry(
        tmFilePath,
        true,
        true,
        false);
    config.Entries.Add(tm);

    project.UpdateTranslationProviderConfiguration(config);
    #endregion
}
```
***

If you are publishing to a project server it is recommended that you create a server based TM so that the TM can be shared between all the project users. Implement a separate helper function that we use to add the server translation memory to the project. This function takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object, the server address, the organization path, TM name and security details. These will be provided by your TM server administrator:

# [C#](#tab/tabid-5)
```cs
/// <summary>
/// Add a server TM to be used for anaysis. The project languages are
/// set according to the TM.
/// </summary>
private void AddServerTm(FileBasedProject project, string serverAddress, string organizationPath, string tmName, bool useWindowsSecurity, string username, string password)
```
***

A project can be associated with a number of translation memories, which are contained in the translation provider configuration. The [TranslationProviderConfiguration](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetTranslationProviderConfiguration) acts as a container for the different kinds of translation providers that can be added to a project, e.g. TMs and Web-based translation services such as Google Translate. In our simple implementation, we will only add one TM to the configuration. Start by creating a translation provider configuration object as shown below:

# [C#](#tab/tabid-6)
```cs
TranslationProviderConfiguration config = project.GetTranslationProviderConfiguration();
```
***

The configuration can contain a cascade of providers. Create an object based on the [TranslationProviderCascadeEntry](../../api/projectautomation/Sdl.ProjectAutomation.Core.TranslationProviderCascadeEntry.yml). The parameters you need to provide here are as follows:
* A TranslationProviderReference which points server tm
* A boolean parameter to indicate whether the TM should be used for segment lookups (True in our implementation).
* A boolean parameter to indicate whether the TM should be used for concordance searches (False, as concordance searching is not required in our implementation).
* A boolean parameter to indicate whether the TM should be updated (False in our implementation, as we use the TM only for analysis, not for adding new translation units or for updating TUs).

After creating the translation provider cascade entry, we add it to the configuration as shown below:

# [C#](#tab/tabid-7)
```cs
TranslationProviderCascadeEntry tm = new TranslationProviderCascadeEntry(
    tmFilePath,
    true,
    true,
    false);
config.Entries.Add(tm);

project.UpdateTranslationProviderConfiguration(config);
```
***

In addition to setting up the provider you will need to set up the credentials required to access the provider as follows:

# [C#](#tab/tabid-8)
```cs
project.Credentials.AddCredential(new Uri(serverAddress), String.Format("user={0};password={1};type=CustomUser", username, password, useWindowsSecurity ? "WindowsUser" : "CustomUser"));
project.UpdateTranslationProviderConfiguration(config);
```
***

The complete helper function for adding the TM to the project should now look as follows:

# [C#](#tab/tabid-9)
```cs
#region "AddServerTmFunction"
/// <summary>
/// Add a server TM to be used for anaysis. The project languages are
/// set according to the TM.
/// </summary>
private void AddServerTm(FileBasedProject project, string serverAddress, string organizationPath, string tmName, bool useWindowsSecurity, string username, string password)
#endregion
{
    #region "TranslationProviderConfiguration"
    Uri tmAddress = new Uri(String.Format("sdltm.{1}{2}/{3}", serverAddress, organizationPath, tmName));
    TranslationProviderConfiguration config = project.GetTranslationProviderConfiguration();
    #endregion

    #region "TranslationProviderCascadeEntryForServerTM"
    TranslationProviderCascadeEntry tm = new TranslationProviderCascadeEntry
    (
          new TranslationProviderReference(tmAddress),
          true,
          false,
          false
    );
    config.Entries.Add(tm);
    project.UpdateTranslationProviderConfiguration(config);
    #endregion

    #region "CredentialsForServerTm"
    project.Credentials.AddCredential(new Uri(serverAddress), String.Format("user={0};password={1};type=CustomUser", username, password, useWindowsSecurity ? "WindowsUser" : "CustomUser"));
    project.UpdateTranslationProviderConfiguration(config);
    #endregion
}
```
***

See Also
--

[Adding Translation Memories](adding_translation_memories.md)

[Translation Memory Settings](translation_memory_settings.md)

[Translation Memory Search Settings](translation_memory_search_settings.md)

[Setting TM Penalties](setting_tm_penalties.md)

[Translation Memory Fields Update](translation_memory_field_update.md)

[Translation Memory Filter Settings](translation_memory_filter_settings.md)

[Auto-Substitution Settings](auto_substitution_settings.md)

[Project Configuration](project_configuration.md)