# Creating a Server Translation Memory

You can create server translation memories in Var:ProductName or in the browser-based TM Server Manager, provided that you have the required access rights. This chapter shows how to create server TMs programmatically, which is one of the most common scenarios.

## Add a New Class

The screenshot below shows the information you need to provide when you create a new server TM in Var:ProductName. Some information, such as the description and copyright information, is optional. The required information is as follows:

* TM name
* TM Server name
* Container database
* Organization (default is Root Organization)
* Language direction

<img style="display:block; " src="images/CreateServerTm.jpg"/>

To create a server TM programmatically, start by adding a class called `ServerTmCreator`. Then implement a `Create` method that takes a TM Server object and the name of the TM to create as parameters.

## Check whether the TM already exists

You cannot create a TM with a name that already exists, so check whether the TM you want to create is already present on the server. To keep the code robust, loop through the available TMs on the server and throw an error if you find a TM with the same name.

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
## Assign the TM properties

After you confirm that a TM with that name does not exist, use the [ServerBasedTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedTranslationMemory.yml) class to create a new server TM object. Then assign the TM properties: name, and optionally the description and copyright information.

# [C#](#tab/tabid-2)
```cs
var newTM = new ServerBasedTranslationMemory(tmServer);
newTM.Name = tmName;
newTM.Description = "Programmatically created sample TM";
newTM.Copyright = "(c) 2021 RWS Group";
```
## Select the container

Next, select the container database where the new TM should be created. To do this, retrieve the containers available on the TM Server. Reference the container by using the friendly name and the container properties as parameters.
# [C#](#tab/tabid-3)
```cs
containerPath += containerName;
TranslationMemoryContainer container = tmServer.GetContainer(containerPath, this.GetContainerProperties());
newTM.Container = container;
```
The container properties come from a separate function:
# [C#](#tab/tabid-4)
```cs
private ContainerProperties GetContainerProperties()
{
    return new ContainerProperties();
}
```
## Select the language direction

Now assign the language direction, for example English to German (`en-US` to `de-DE`):
# [C#](#tab/tabid-5)
```cs
this.CreateLanguageDirections(newTM.LanguageDirections);
```
The language direction is created in a separate function. There, you set the locales of the source and target languages, `en-US` and `de-DE`. Then you add the direction to the language-direction collection, which is assigned to the TM.
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
Note that file-based translation memories can only be bilingual, whereas server TMs can be multilingual. You can therefore add multiple language directions to the collection. This example adds only one direction.

## Field and language resources templates

When you create a TM, you can select a field template and a language resources template. Field templates store the fields that can be added to a TU, for example *Customer* and *Project id*. Language resources templates can store custom abbreviation lists, variables, and other resources. For more information, see [Configuring Translation Memories](configuring_translation_memories.md). To select a field template during TM creation, use the [GetFieldsTemplates](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetFieldsTemplates_System_Boolean_) method on the TM Server object. This method requires the template ID or path. You can select language resources templates in the same way by using the [GetLanguageResourcesTemplates](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetLanguageResourcesTemplate_System_Guid_) method, which also takes the template ID or path.

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
> [!NOTE]
>
> Field templates are only available for server TMs. For file-based TMs, fields need to be configured for each TM individually, and cannot be centralized in a field template.

## Select the organization

TMs are assigned to organizations, by default to the *Root Organization*. Organizations make it easier to manage large numbers of TMs. For example, if you want to create TMs for a specific customer, you can first define an organization called, for example, *Microsoft*, and then create all Microsoft-related TMs within that organization. You can also restrict access so that only specific users can work with the organization and its TMs. In this implementation, create the new TM in the *organization* passed in through the organization parameter.

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

## Delete the translation memory

You can also delete server TMs when they are no longer required. This is a dangerous operation, because it physically removes potentially valuable data. Unless you have a backup, you cannot restore the deleted TM data. The sample function below opens a server TM and then applies the [Delete](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedTranslationMemory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedTranslationMemory_Delete) method to remove the TM from the server. Only users with the necessary credentials can delete a TM.

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
## Putting It All Together

The complete class should now look like this:
# [C#](#tab/tabid-9)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Globalization;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerTmCreator
    {        
        public void Create(TranslationProviderServer tmServer, string organizationPath, string containerName, string tmName)
        {
            foreach (ServerBasedTranslationMemory item in tmServer.GetTranslationMemories(TranslationMemoryProperties.None))
            {
                if (item.Name == tmName)
                {
                    throw new Exception("TM with that name already exists.");
                }
            }            
                       
            var newTM = new ServerBasedTranslationMemory(tmServer);
            newTM.Name = tmName;
            newTM.Description = "Programmatically created sample TM";
            newTM.Copyright = "(c) 2024 RWS Group";
           
            string containerPath = organizationPath;
            if (!containerPath.EndsWith("/"))
            {
                containerPath += "/";
            }
                        
            containerPath += containerName;
            TranslationMemoryContainer container = tmServer.GetContainer(containerPath, this.GetContainerProperties());
            newTM.Container = container;
            
            this.CreateLanguageDirections(newTM.LanguageDirections);
          
            newTM.ParentResourceGroupPath = organizationPath;
           
            string templatePath = organizationPath;
            if (!templatePath.EndsWith("/"))
            {
                templatePath += "/";
            }
           
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

            newTM.Save();
        }       

        private ContainerProperties GetContainerProperties()
        {
            return new ContainerProperties();
        }
                
        private void CreateLanguageDirections(ServerBasedTranslationMemoryLanguageDirectionCollection languageDirectionsCollection)
        {
            ServerBasedTranslationMemoryLanguageDirection languageDirection = new ServerBasedTranslationMemoryLanguageDirection();
            languageDirection.SourceLanguage = CultureInfo.GetCultureInfo("en-US");
            languageDirection.TargetLanguage = CultureInfo.GetCultureInfo("de-DE");

            languageDirectionsCollection.Add(languageDirection);
        }
        
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
    }
}
```
***

## See Also

[Retrieving Database Servers](retrieving_database_servers.md)

[Retrieving TM Containers](retrieving_tm_containers.md)

[Creating a Container Database](creating_a_container_database.md)

[Creating a File-based Translation Memory](creating_a_file_based_translation_memory.md)

[Configuring Translation Memories](configuring_translation_memories.md)

[TM Fields Templates](tm_fields_templates.md)

[Language Resource Templates](language_resource_templates.md)

[Adding TM Fields](adding_tm_fields.md)

[Adding Language Resources](adding_language_resources.md)
