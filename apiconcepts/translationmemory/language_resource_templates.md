Language Resource Templates
====
Like file translation memories, server TMs, too, can leverage language resource templates. (For more information on language resources, see [Configuring Translation Memories](configuring_translation_memories.md).) On this page you will learn how to retrieve server language resource templates and how to create a new language resource template.

Add a New Class
----
Start by adding a new class called `ServerLanguageResourceTemplates` to your project. This class should contain the functionality to retrieve existing language resource templates and create new ones.

Retrieve the Available Language Resources Templates
----
The first task will be to generate a list of all language resources templates that are hosted on a specified TM Server. To do this implement a function called `GetTemplates`, which takes the translation provider server as parameter. Use a `foreach` loop to iterate through all language resources templates by applying the [GetLanguageResourcesTemplates](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetLanguageResourcesTemplates_Sdl_LanguagePlatform_TranslationMemoryApi_LanguageResourcesTemplateProperties_System_Boolean_) method:

# [C#](#tab/tabid-1)
```cs
string templateList = string.Empty;
foreach (ServerBasedLanguageResourcesTemplate template in tmServer.GetLanguageResourcesTemplates(LanguageResourcesTemplateProperties.All))
```
****

In the loop you build up a string variable by adding the template names and the (optional) template descriptions:

# [C#](#tab/tabid-2)
```cs
templateList += "Template name: " + template.Name + "\n";
templateList += "Description: " + template.Description + "\n";
```
****

A language resources template contains language resource bundles such as variable and abbreviations lists as well as segmentation rules. Below is a brief example of how to access the resource bundles. Imagine for this simple use case that the first resource bundle is a variable list, and we would like to determine the number of variables that this resource contains. To do this, use the [LanguageResourceBundles](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedLanguageResourcesTemplate_LanguageResourceBundles) property to select the first available resource bundle, then apply the `Variables` property to the resource bundle object to determine the variable count.
# [C#](#tab/tabid-3)
```cs
LanguageResourceBundle bundle = template.LanguageResourceBundles[0];
templateList += "Number of variables in template: " + bundle.Variables.Count.ToString() + "\n\n";
```
***
The complete function should now look as shown below:

# [C#](#tab/tabid-4)
```cs
public void GetTemplates(TranslationProviderServer tmServer)
{
    #region "LoopTemplates"
    string templateList = string.Empty;
    foreach (ServerBasedLanguageResourcesTemplate template in tmServer.GetLanguageResourcesTemplates(LanguageResourcesTemplateProperties.All))
    #endregion
    {
        #region "info"
        templateList += "Template name: " + template.Name + "\n";
        templateList += "Description: " + template.Description + "\n";
        #endregion
        #region "VarCount"
        LanguageResourceBundle bundle = template.LanguageResourceBundles[0];
        templateList += "Number of variables in template: " + bundle.Variables.Count.ToString() + "\n\n";
        #endregion
    }

    MessageBox.Show(templateList, "Available language resources remplates");
}
```
***

Retrieve the TMs that use a given Language Resources Template
----

When creating server TMs you can specify a language resource template. The TM will then 'inherit' the language resources (e.g. variable lists) defined in the template. Imagine that you want to know which server TMs use a particular template. To do this implement a function called `GetTmsForTemplate`, which takes the translation provider server as and the language resources template name as parameters. First, retrieve the template that you want to select. The template can be referenced using the template name:
# [C#](#tab/tabid-5)
```cs
ServerBasedLanguageResourcesTemplate template = tmServer.GetLanguageResourcesTemplate(templateName, LanguageResourcesTemplateProperties.All);
```
****
Next apply the [TranslationMemories](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.LanguageResourcesTemplateProperties.yml) property to the selected template and loop through all TMs associated with the template:
# [C#](#tab/tabid-6)
```cs
string tmList = string.Empty;
foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
{
    tmList += tm.Name + "\n";
}

MessageBox.Show(tmList);
```
*****
The complete function should look as shown below:
# [C#](#tab/tabid-7)
```cs
public void GetTmsForTemplate(TranslationProviderServer tmServer, string templateName)
{
    #region "SelectTemplate"
    ServerBasedLanguageResourcesTemplate template = tmServer.GetLanguageResourcesTemplate(templateName, LanguageResourcesTemplateProperties.All);
    #endregion

    #region "LoopTms"
    string tmList = string.Empty;
    foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
    {
        tmList += tm.Name + "\n";
    }

    MessageBox.Show(tmList);
    #endregion

```
****
Create a New Language Resource Template
---
The next task will be to create a new language resources template on the server. Start by adding a function called `CreateTemplate`, which takes the translation provider server as parameter. Create a template object on the server and assign the template name and an (optional) description by setting the corresponding properties:
# [C#](#tab/tabid-8)
```cs
var template = new ServerBasedLanguageResourcesTemplate(tmServer);
template.Name = "Sample Language Resources Template";
template.Description = "Language Resources template created through API";
```
****
Let us assume that as language resources bundles you would like to add a variables list and an abbreviation list. Note that the language resources apply to the source language, this is why you need to specify the language that the resource bundle applies to. The sample code below creates a variables list for English (US), and then adds two product names as variables:
# [C#](#tab/tabid-9)
```cs
var variables = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
variables.Variables.Add("Trados Studio);
variables.Variables.Add("MultiTerm");
```
****
Similarly, you create the resource bundle object for the abbreviation list. The sample code below creates the abbreviation list object and adds two abbreviations:
# [C#](#tab/tabid-10)
```cs
var abbreviations = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
abbreviations.Abbreviations.Add("hr.");
abbreviations.Abbreviations.Add("cont.");
```
****

> [!NOTE]
> >
> Abbreviations are case-sensitive.

Next, add the variables and the abbreviation lists to the language resources template object:
# [C#](#tab/tabid-11)
```cs
template.LanguageResourceBundles.Add(variables);
template.LanguageResourceBundles.Add(abbreviations);
```
****
Last, save the template, thereby actually creating it on the server. Optionally, you may use the [IsDirty](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedLanguageResourcesTemplate_IsDirty) boolean property to verify whether there are any unsaved changes in your template definition, which should not be the case:
# [C#](#tab/tabid-12)
```cs
template.Save();
MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());
```
*****

Fields templates can be deleted by applying the [Delete](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedLanguageResourcesTemplate_Delete) method:
# [C#](#tab/tabid-13)
```cs
template.Delete();
```
****
The complete function should look as shown below:
# [C#](#tab/tabid-14)
```cs
public void CreateTemplate(TranslationProviderServer tmServer)
{
    #region "TemplateProps"
    var template = new ServerBasedLanguageResourcesTemplate(tmServer);
    template.Name = "Sample Language Resources Template";
    template.Description = "Language Resources template created through API";
    #endregion

    #region "variables"
    var variables = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
    variables.Variables.Add("Trados Studio");
    variables.Variables.Add("MultiTerm");
    #endregion

    #region "abbreviations"
    var abbreviations = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
    abbreviations.Abbreviations.Add("hr.");
    abbreviations.Abbreviations.Add("cont.");
    #endregion

    #region "AddResources"
    template.LanguageResourceBundles.Add(variables);
    template.LanguageResourceBundles.Add(abbreviations);
    #endregion

    #region "save"
    template.Save();
    MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());
    #endregion

    #region "delete"
    template.Delete();
    #endregion
}
```
*****
Putting it All Together
---
The complete class should look as shown below:
# [C#](#tab/tabid-15)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System.Globalization;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerLanguageResourceTemplates
    {
        #region "get"
        public void GetTemplates(TranslationProviderServer tmServer)
        {
            #region "LoopTemplates"
            string templateList = string.Empty;
            foreach (ServerBasedLanguageResourcesTemplate template in tmServer.GetLanguageResourcesTemplates(LanguageResourcesTemplateProperties.All))
            #endregion
            {
                #region "info"
                templateList += "Template name: " + template.Name + "\n";
                templateList += "Description: " + template.Description + "\n";
                #endregion
                #region "VarCount"
                LanguageResourceBundle bundle = template.LanguageResourceBundles[0];
                templateList += "Number of variables in template: " + bundle.Variables.Count.ToString() + "\n\n";
                #endregion
            }

            MessageBox.Show(templateList, "Available language resources remplates");
        }
        #endregion

        #region "GetTmsForTemplate"
        public void GetTmsForTemplate(TranslationProviderServer tmServer, string templateName)
        {
            #region "SelectTemplate"
            ServerBasedLanguageResourcesTemplate template = tmServer.GetLanguageResourcesTemplate(templateName, LanguageResourcesTemplateProperties.All);
            #endregion

            #region "LoopTms"
            string tmList = string.Empty;
            foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
            {
                tmList += tm.Name + "\n";
            }

            MessageBox.Show(tmList);
            #endregion
        }
        #endregion

        #region "CreateTemplate"
        public void CreateTemplate(TranslationProviderServer tmServer)
        {
            #region "TemplateProps"
            var template = new ServerBasedLanguageResourcesTemplate(tmServer);
            template.Name = "Sample Language Resources Template";
            template.Description = "Language Resources template created through API";
            #endregion

            #region "variables"
            var variables = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
            variables.Variables.Add("Trados Studio");
            variables.Variables.Add("MultiTerm");
            #endregion

            #region "abbreviations"
            var abbreviations = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
            abbreviations.Abbreviations.Add("hr.");
            abbreviations.Abbreviations.Add("cont.");
            #endregion

            #region "AddResources"
            template.LanguageResourceBundles.Add(variables);
            template.LanguageResourceBundles.Add(abbreviations);
            #endregion

            #region "save"
            template.Save();
            MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());
            #endregion

            #region "delete"
            template.Delete();
            #endregion
        }
        #endregion
    }
}
```
****

See Also
-----------
[Configuring Translation Memories](configuring_translation_memories.md)

[Adding Language Resources](adding_language_resources.md)

