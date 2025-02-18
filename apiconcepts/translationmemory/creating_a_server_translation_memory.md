Creating a Server Translation Memory
====

Server translation memories can either be created in Var:ProductName or in the browser-based TM Server Manager (provided that you have the required access rights). In this chapter you will learn how to create server TMs programmatically, which is probably one of the most common applications.

Add a New Class
----
The screenshot below illustrates what kind of information you need to provide when creating a new server TM from Var:ProductName. Some information is optional such as the description and the copyright information. The mandatory information that you need to provide is a follows:

* TM name
* TM Server name
* Container database
* Organization (default is Root Organization)
* Language direction

<img style="display:block; " src="images/CreateServerTm.jpg"/>

To create a server TM programmatically, start by adding a new class called `ServerTmCreator`. Then implement a function called `Create`, which takes a TM Server object and the name of the TM to create as parameters.

Check if the TM Already Exists
----
As it is not allowed to create a TM with a name that exists already, you should also check whether the TM you are trying to create is already present on the server. To make your script robust, loop through the available TMs on the server, and then throw an error if a TM with the same name already exists.

# [C#](#tab/tabid-1)
```cs
foreach (ServerBasedTranslationMemory item in tmServer.GetTranslationMemories(TranslationMemoryProperties.None))
{
    if (item.Name == tmName)
    {
        throw new Exception("TM with that name already exists.");
    }
}
```
****
Assign the TM Properties
---
After you have made certain that a TM with that name does not exist yet, use the [ServerBasedTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedTranslationMemory.yml) class to create a new server TM object. Then assign the TM properties, i.e. the name and the (optional) description and copyright information:

# [C#](#tab/tabid-2)
```cs
var newTM = new ServerBasedTranslationMemory(tmServer);
newTM.Name = tmName;
newTM.Description = "Programmatically created sample TM";
newTM.Copyright = "(c) 2021 RWS Group";
```
****
Select the Container
----
In the next step, select the container database in which the new TM should be created. To do this, retrieve the containers available on the TM Server. The container is referenced by using the friendly name and the container properties as parameters.
# [C#](#tab/tabid-3)
```cs
containerPath += containerName;
TranslationMemoryContainer container = tmServer.GetContainer(containerPath, this.GetContainerProperties());
newTM.Container = container;
```
****
The container properties are returned by a separate function:
# [C#](#tab/tabid-4)
```cs
private ContainerProperties GetContainerProperties()
{
    return new ContainerProperties();
}
```
****

Select the Language Direction
----
Now assign the language direction, e.g. English -> German *(en-US -> de-DE)*:
# [C#](#tab/tabid-5)
```cs
this.CreateLanguageDirections(newTM.LanguageDirections);
```
*****
The creation of the language direction is done in a separate function. Here, you set the locales of the source and target languages, i.e. *en-US* and *de-DE*. This language direction is then added to the language direction collection, which, in turn, is then assigned to the TM.
# [C#](#tab/tabid-6)
```cs
private void CreateLanguageDirections(ServerBasedTranslationMemoryLanguageDirectionCollection languageDirectionsCollection)
{
    var languageDirection = new ServerBasedTranslationMemoryLanguageDirection();
    languageDirection.SourceLanguage = CultureInfo.GetCultureInfo("en-US");
    languageDirection.TargetLanguage = CultureInfo.GetCultureInfo("de-DE");

    languageDirectionsCollection.Add(languageDirection);
}
```
*****
Note that while file-based translation memories can only be bilingual, server TMs can be multilingual. This means that you can add multiple language directions to the languages directions collection. However, in this example we only add one direction to the language directions collection.

Field and Language Resources Templates
----

When creating a TM you can select a field and language resources template. Field templates store the fields that can be added to a TU (e.g. *Customer, Project id*, etc.). Language resource templates can store custom abbreviation lists, variables, etc. For more information on these kinds of templates, please see [Configuring Translation Memories](configuring_translation_memories.md). In order to select a fields template during creation of a TM, you need to apply the [GetFieldsTemplates](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetFieldsTemplates_System_Boolean_) method to the TM server object. This method requires either the template ID or path as parameters. In the same way you can select the language resources templates by applying the [GetLanguageResourcesTemplates](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetLanguageResourcesTemplate_System_Guid_) method, which also takes the template ID or path as parameters.

# [C#](#tab/tabid-7)
```cs
string sampleFieldTemplateName = "MyFieldTemplate";
foreach (ServerBasedFieldsTemplate template in tmServer.GetFieldsTemplates(FieldsTemplateProperties.All))
{
    if (template.Name == sampleFieldTemplateName)
    {
        newTM.FieldsTemplate = tmServer.GetFieldsTemplate(
            templatePath + sampleFieldTemplateName, FieldsTemplateProperties.Fields);
        break;
    }
}

string sampleLanguageResourcesTemplateName = "MyLanguageResourcesTemplate";
foreach (ServerBasedLanguageResourcesTemplate template in tmServer.GetLanguageResourcesTemplates(
    LanguageResourcesTemplateProperties.LanguageResources))
{
    if (template.Name == sampleLanguageResourcesTemplateName)
    {
        newTM.LanguageResourcesTemplate = tmServer.GetLanguageResourcesTemplate(
            templatePath + sampleLanguageResourcesTemplateName, LanguageResourcesTemplateProperties.None);
        break;
    }
}
```
********

> [!NOTE]
>
> Field templates are only available for server TMs. For file-based TMs, fields need to be configured for each TM individually, and cannot be centralized in a field template.

Select the Organization
---

TMs are assigned to organizations, by default to the *Root Organization*. Organizations make it easier to manage large numbers of TMs. For example, if you want to create TMs for a specific customer, you can first define an organization called e.g. *Microsoft*, and then create all Microsoft-related TMs within this organization. It is also possible to define that only particular users should have access to this organization, and thus to the TMs created therein. In our implementation we create the new TM within the *organization* passed in through the organization parameter.

> [!NOTE]
> 
> If you have not defined organizations, and want to use the Root Organization, the path would simply be:
>  /
> 
> If you take a *sub-ordinate organization*, the path to the organization would look as follows:
> 
> /*My Company*
>
> If an organization has sub-organizations (e.g. a company further
> sub-divides into departments), the path would look as shown below:
>
> /*My Company/Marketing*

Delete the Translation Memory
----
Of course, it is also possible to delete server TMs when they are no longer required. Note that this is a dangerous operation, as this will physically delete potentially valuable data. Unless a backup is available, the deleted TM data can no longer be restored. The sample function below opens a server TM and then applies the [Delete](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedTranslationMemory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedTranslationMemory_Delete) method to delete the TM from the server. Note that a TM can only be deleted by a user who has the necessary credentials.

# [C#](#tab/tabid-8)
```cs
public void DeleteTm(TranslationProviderServer tmServer, string organizationPath, string tmName)
{
    string tmPath = organizationPath;
    if (!tmPath.EndsWith("/"))
    {
        tmPath += "/";
    }

    ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(tmPath + tmName, TranslationMemoryProperties.All);
    tm.Delete();
}
```
*****
Putting it All Together
---
The complete class should now look as shown below:
# [C#](#tab/tabid-9)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Globalization;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerTmCreator
    {
        #region "create"
        public void Create(TranslationProviderServer tmServer, string organizationPath, string containerName, string tmName)
        {
            #region "CheckExists"
            foreach (ServerBasedTranslationMemory item in tmServer.GetTranslationMemories(TranslationMemoryProperties.None))
            {
                if (item.Name == tmName)
                {
                    throw new Exception("TM with that name already exists.");
                }
            }
            #endregion

            #region "TM"
            var newTM = new ServerBasedTranslationMemory(tmServer);
            newTM.Name = tmName;
            newTM.Description = "Programmatically created sample TM";
            newTM.Copyright = "(c) 2021 RWS Group";
            #endregion

            string containerPath = organizationPath;
            if (!containerPath.EndsWith("/"))
            {
                containerPath += "/";
            }

            #region "container"
            containerPath += containerName;
            TranslationMemoryContainer container = tmServer.GetContainer(containerPath, this.GetContainerProperties());
            newTM.Container = container;
            #endregion

            #region "LanguageDirection"
            this.CreateLanguageDirections(newTM.LanguageDirections);
            #endregion

            #region "org"
            newTM.ParentResourceGroupPath = organizationPath;
            #endregion

            string templatePath = organizationPath;
            if (!templatePath.EndsWith("/"))
            {
                templatePath += "/";
            }
                
            #region "templates"
            string sampleFieldTemplateName = "MyFieldTemplate";
            foreach (ServerBasedFieldsTemplate template in tmServer.GetFieldsTemplates(FieldsTemplateProperties.All))
            {
                if (template.Name == sampleFieldTemplateName)
                {
                    newTM.FieldsTemplate = tmServer.GetFieldsTemplate(
                        templatePath + sampleFieldTemplateName, FieldsTemplateProperties.Fields);
                    break;
                }
            }

            string sampleLanguageResourcesTemplateName = "MyLanguageResourcesTemplate";
            foreach (ServerBasedLanguageResourcesTemplate template in tmServer.GetLanguageResourcesTemplates(
                LanguageResourcesTemplateProperties.LanguageResources))
            {
                if (template.Name == sampleLanguageResourcesTemplateName)
                {
                    newTM.LanguageResourcesTemplate = tmServer.GetLanguageResourcesTemplate(
                        templatePath + sampleLanguageResourcesTemplateName, LanguageResourcesTemplateProperties.None);
                    break;
                }
            }
            #endregion


            newTM.Save();
        }
        #endregion

        #region "ContainerProps"
        private ContainerProperties GetContainerProperties()
        {
            return new ContainerProperties();
        }
        #endregion

        #region "languages"
        private void CreateLanguageDirections(ServerBasedTranslationMemoryLanguageDirectionCollection languageDirectionsCollection)
        {
            ServerBasedTranslationMemoryLanguageDirection languageDirection = new ServerBasedTranslationMemoryLanguageDirection();
            languageDirection.SourceLanguage = CultureInfo.GetCultureInfo("en-US");
            languageDirection.TargetLanguage = CultureInfo.GetCultureInfo("de-DE");

            languageDirectionsCollection.Add(languageDirection);
        }
        #endregion

        #region "DeleteTm"
        public void DeleteTm(TranslationProviderServer tmServer, string organizationPath, string tmName)
        {
            string tmPath = organizationPath;
            if (!tmPath.EndsWith("/"))
            {
                tmPath += "/";
            }

            ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(tmPath + tmName, TranslationMemoryProperties.All);
            tm.Delete();
        }
        #endregion
    }
}
```
***

See Also
------------
[Retrieving Database Servers](retrieving_database_servers.md)

[Retrieving TM Containers](retrieving_tm_containers.md)

[Creating a Container Database](creating_a_container_database.md)

[Creating a File-based Translation Memory](creating_a_file_based_translation_memory.md)

[Configuring Translation Memories](configuring_translation_memories.md)

[TM Fields Templates](tm_fields_templates.md)

[Language Resource Templates](language_resource_templates.md)

[Adding TM Fields](adding_tm_fields.md)

[Adding Language Resources](adding_language_resources.md)
