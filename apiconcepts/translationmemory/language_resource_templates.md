# Language Resource Templates

Like file translation memories, server TMs can also use language resource templates. For more information on language resources, see [Configuring Translation Memories](configuring_translation_memories.md). This page shows how to retrieve server language resource templates and how to create a new language resource template.

## Add a New Class

Start by adding a class called `ServerLanguageResourceTemplates` to your project. This class contains the logic to retrieve existing language resource templates and create new ones.

## Retrieve the Available Language Resource Templates

First, generate a list of all language resource templates hosted on a specified TM Server. Implement a function called `GetTemplates`, which takes the translation provider server as a parameter. Use a `foreach` loop to iterate through all language resource templates by applying the [GetLanguageResourcesTemplates](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetLanguageResourcesTemplates_System_Boolean_) method:

# [C#](#tab/tabid-1)
```cs
string templateList = string.Empty;
foreach (ServerBasedLanguageResourcesTemplate template in tmServer.GetLanguageResourcesTemplates(LanguageResourcesTemplateProperties.All))
```

In the loop, build a string by adding the template names and optional descriptions:

# [C#](#tab/tabid-2)
```cs
templateList += "Template name: " + template.Name + "\n";
templateList += "Description: " + template.Description + "\n";
```
A language resource template contains resource bundles such as variable and abbreviation lists, as well as segmentation rules. The example below shows how to access the resource bundles. In this use case, the first resource bundle is a variable list, and we want to determine how many variables it contains. Use the [LanguageResourceBundles](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedLanguageResourcesTemplate_LanguageResourceBundles) property to select the first available resource bundle, then use the `Variables` property on that bundle to determine the variable count.
# [C#](#tab/tabid-3)
```cs
LanguageResourceBundle bundle = template.LanguageResourceBundles[0];
templateList += "Number of variables in template: " + bundle.Variables.Count.ToString() + "\n\n";
```
The complete function should now look like this:

# [C#](#tab/tabid-4)
```cs
public void GetTemplates(TranslationProviderServer tmServer)
{    
    string templateList = string.Empty;
    foreach (ServerBasedLanguageResourcesTemplate template in tmServer.GetLanguageResourcesTemplates(LanguageResourcesTemplateProperties.All))
    {
        templateList += "Template name: " + template.Name + "\n";
        templateList += "Description: " + template.Description + "\n";

        LanguageResourceBundle bundle = template.LanguageResourceBundles[0];
        templateList += "Number of variables in template: " + bundle.Variables.Count.ToString() + "\n\n";
    }

    MessageBox.Show(templateList, "Available language resource templates");
}
```
## Retrieve the TMs That Use a Given Language Resource Template

When you create server TMs, you can specify a language resource template. The TM then inherits the language resources, such as variable lists, defined in the template. If you want to know which server TMs use a particular template, implement a function called `GetTmsForTemplate`, which takes the translation provider server and the language resource template name as parameters. First, retrieve the template you want to inspect. Reference the template by name:
# [C#](#tab/tabid-5)
```cs
ServerBasedLanguageResourcesTemplate template = tmServer.GetLanguageResourcesTemplate(templatePath);
```

Next, loop through all TMs associated with the template:
# [C#](#tab/tabid-6)
```cs
string tmList = string.Empty;
foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
{
    tmList += tm.Name + "\n";
}

MessageBox.Show(tmList);
```
The complete function should now look like this:
# [C#](#tab/tabid-7)
```cs
public void GetTmsForTemplate(TranslationProviderServer tmServer, string templatePath)
{   
    ServerBasedLanguageResourcesTemplate template = tmServer.GetLanguageResourcesTemplate(templatePath);
   
    string tmList = string.Empty;
    foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
    {
        tmList += tm.Name + "\n";
    }

    MessageBox.Show(tmList);
```

## Create a New Language Resource Template

Next, create a new language resource template on the server. Start by adding a function called `CreateTemplate`, which takes the translation provider server as a parameter. Create a template object on the server and assign the template name and optional description by setting the corresponding properties:
# [C#](#tab/tabid-8)
```cs
var template = new ServerBasedLanguageResourcesTemplate(tmServer);
template.Name = "Sample Language Resources Template";
template.Description = "Language Resources template created through API";
```
Assume that you want to add a variable list and an abbreviation list as language resource bundles. Language resources apply to the source language, so specify the language that each resource bundle uses. The sample code below creates a variable list for English (US) and then adds two product names as variables:
# [C#](#tab/tabid-9)
```cs
var variables = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
variables.Variables.Add("Trados Studio");
variables.Variables.Add("MultiTerm");
```

Similarly, create the resource bundle object for the abbreviation list. The sample code below creates the abbreviation list and adds two abbreviations:
# [C#](#tab/tabid-10)
```cs
var abbreviations = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
abbreviations.Abbreviations.Add("hr.");
abbreviations.Abbreviations.Add("cont.");
```
> [!NOTE]
>
> Abbreviations are case-sensitive.

Next, add the variable and abbreviation lists to the language resource template object:
# [C#](#tab/tabid-11)
```cs
template.LanguageResourceBundles.Add(variables);
template.LanguageResourceBundles.Add(abbreviations);
```
Finally, save the template to create it on the server. Optionally, you can use the [IsDirty](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedLanguageResourcesTemplate_IsDirty) Boolean property to verify whether any unsaved changes remain in the template definition. In this case, the property should return `false`:
# [C#](#tab/tabid-12)
```cs
template.Save();
MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());
```
Language resource templates can be deleted by applying the [Delete](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedLanguageResourcesTemplate_Delete) method:
# [C#](#tab/tabid-13)
```cs
template.Delete();
```
The complete function should now look like this:
# [C#](#tab/tabid-14)
```cs
public void CreateTemplate(TranslationProviderServer tmServer)
{    
    var template = new ServerBasedLanguageResourcesTemplate(tmServer);
    template.Name = "Sample Language Resources Template";
    template.Description = "Language Resources template created through API";
 
    var variables = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
    variables.Variables.Add("Trados Studio");
    variables.Variables.Add("MultiTerm");
    
    var abbreviations = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
    abbreviations.Abbreviations.Add("hr.");
    abbreviations.Abbreviations.Add("cont.");
    
    template.LanguageResourceBundles.Add(variables);
    template.LanguageResourceBundles.Add(abbreviations);
    
    template.Save();
    MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());

    template.Delete();    
}
```

## Putting It All Together

The complete class should now look like this:
# [C#](#tab/tabid-15)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System.Globalization;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerLanguageResourceTemplates
    {        
        public void GetTemplates(TranslationProviderServer tmServer)
        {
            string templateList = string.Empty;
            foreach (ServerBasedLanguageResourcesTemplate template in tmServer.GetLanguageResourcesTemplates(LanguageResourcesTemplateProperties.All))
            {
                templateList += "Template name: " + template.Name + "\n";
                templateList += "Description: " + template.Description + "\n";
                LanguageResourceBundle bundle = template.LanguageResourceBundles[0];
                templateList += "Number of variables in template: " + bundle.Variables.Count.ToString() + "\n\n";
            }

            MessageBox.Show(templateList, "Available language resource templates");
        }

        public void GetTmsForTemplate(TranslationProviderServer tmServer, string templateName)
        {
            ServerBasedLanguageResourcesTemplate template = tmServer.GetLanguageResourcesTemplate(templateName, LanguageResourcesTemplateProperties.All);
                        
            string tmList = string.Empty;
            foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
            {
                tmList += tm.Name + "\n";
            }

            MessageBox.Show(tmList);
        }
       
        public void CreateTemplate(TranslationProviderServer tmServer)
        {
            var template = new ServerBasedLanguageResourcesTemplate(tmServer);
            template.Name = "Sample Language Resources Template";
            template.Description = "Language Resources template created through API";
            
            var variables = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
            variables.Variables.Add("Trados Studio");
            variables.Variables.Add("MultiTerm");
           
            var abbreviations = new LanguageResourceBundle(CultureInfo.GetCultureInfo("en-US"));
            abbreviations.Abbreviations.Add("hr.");
            abbreviations.Abbreviations.Add("cont.");
           
            template.LanguageResourceBundles.Add(variables);
            template.LanguageResourceBundles.Add(abbreviations);
          
            template.Save();
            MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());
          
            template.Delete();           
        }
    }
}
```
## See Also

[Configuring Translation Memories](configuring_translation_memories.md)

[Adding Language Resources](adding_language_resources.md)

