TM Fields Templates
===
Server translation memories can use field templates (which are not available for file TMs). Image that you need to use the same fields (e.g. *Customer* and *Project id*) for a large number of TMs. Rather than adding the fields to the setup of each TM, you just define the fields once in a template, which then gets applied to the individual TMs. Moreover, when you change the template to contain a new field (e.g. Domain), this new field will then be instantly made available in all TMs that are based on this particular template.

On this page you will learn how to retrieve available field templates and how to create a template.

Add a New Class
---
Start by adding a new class called `ServerFieldTemplates` to your project. This class should contain the functionality to retrieve existing field templates and create new templates.

Retrieve the Available Fields Templates
---

Our first task will be to generate a list of all field templates that are hosted on the TM Server. To do this implement a function called `GetFieldTemplates`, which takes the translation provider server as parameter. First, add a string variable that will hold the field templates list. Add the templates information to this variable by looping through all field templates that are hosted on the server:
# [C#](#tab/tabid-1)
```cs
StringBuilder templateList = new StringBuilder();

foreach (ServerBasedFieldsTemplate template in tmServer.GetFieldsTemplates(FieldsTemplateProperties.All))
```
****
The list should contain the field template names and descriptions:
# [C#](#tab/tabid-2)
```cs
templateList.AppendFormat("Template name: {0}\n", template.Name);
templateList.AppendFormat("Template description: {0}\n", template.Description);
```
****
Next, retrieve the field names and types (e.g. text or picklist). To do this, loop through the field definitions of each template as follows:
# [C#](#tab/tabid-3)
```cs
templateList.AppendLine("Fields: ");
foreach (FieldDefinition def in template.FieldDefinitions)
{
    templateList.AppendFormat("{0}({1})", def.Name, def.ValueType.ToString());
}

templateList.AppendLine(string.Empty);
```
***

The complete function should look as shown below:
# [C#](#tab/tabid-4)
```cs
public void GetFieldTemplates(TranslationProviderServer tmServer)
{
    #region "LoopTemplates"
    StringBuilder templateList = new StringBuilder();

    foreach (ServerBasedFieldsTemplate template in tmServer.GetFieldsTemplates(FieldsTemplateProperties.All))
    #endregion
    {
        #region "GeneralTemplateInfo"
        templateList.AppendFormat("Template name: {0}\n", template.Name);
        templateList.AppendFormat("Template description: {0}\n", template.Description);
        #endregion

        #region "fields"
        templateList.AppendLine("Fields: ");
        foreach (FieldDefinition def in template.FieldDefinitions)
        {
            templateList.AppendFormat("{0}({1})", def.Name, def.ValueType.ToString());
        }

        templateList.AppendLine(string.Empty);
        #endregion
    }

    MessageBox.Show(templateList.ToString(), "Template List");
}
```
****
Retrieve the TMs that Use a Given Fields Template
----

When creating server TMs you can specify a field template. The TM will then 'inherit' the fields defined in the template. Imagine that you want to know which server TMs use a particular template. To do this, add a function called `GetTmsForTemplate`, which takes the translation provider server and the fields template name as parameters. First, retrieve the fields template that you want to select. The template can be referenced using the template name:
# [C#](#tab/tabid-5)
```cs
ServerBasedFieldsTemplate template = tmServer.GetFieldsTemplate(templateName, FieldsTemplateProperties.All);
```
****
Next, declare a string variable that holds the names of the TMs. Build up the variable by looping through all TMs that are associated with the selected template:
# [C#](#tab/tabid-6)
```cs
StringBuilder tmList = new StringBuilder();

foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
{
    tmList.AppendLine(tm.Name);
}

MessageBox.Show(tmList.ToString());
```
****
The complete function looks as shown below:
# [C#](#tab/tabid-7)
```cs
public void GetTmsForTemplate(TranslationProviderServer tmServer, string templateName)
{
    #region "GetTemplate"
    ServerBasedFieldsTemplate template = tmServer.GetFieldsTemplate(templateName, FieldsTemplateProperties.All);
    #endregion

    #region "TmLoop"
    StringBuilder tmList = new StringBuilder();

    foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
    {
        tmList.AppendLine(tm.Name);
    }

    MessageBox.Show(tmList.ToString());
    #endregion
}
```
****
Create a New Fields Template
---

In this step you will learn how to create a new server-based fields template. Imagine that you need to create a template that contains a text field called *Project id* and a list field called *Client*. Implement a function called `CreateTemplate`, which takes the translation provider server as parameter. Next, create a template object for the given server as shown below. Here, you also specify the general template information, i.e. the name and the (optional) description.
# [C#](#tab/tabid-8)
```cs
ServerBasedFieldsTemplate template = new ServerBasedFieldsTemplate(tmServer);
template.Name = "Sample Template";
template.Description = "Fields template created by API";
```
****
In the next step, create the field definition object for the text field *Project id* by providing the field name and the field type (i.e. **MultipleString**):
# [C#](#tab/tabid-9)
```cs
FieldDefinition projField = new FieldDefinition("Project id", FieldValueType.MultipleString);
```
****
Similarly, you create the list field called *Client*, which should allow for multiple picklist values. Since a list field needs to be associated with one or several pre-defined values, add two sample values to the field definition:
# [C#](#tab/tabid-10)
```cs
FieldDefinition clientField = new FieldDefinition("Client", FieldValueType.MultiplePicklist);
clientField.PicklistItems.Add("Microsoft");
clientField.PicklistItems.Add("SAP");
```
****
Now add the two field definitions to the template:
# [C#](#tab/tabid-11)
```cs
template.FieldDefinitions.Add(projField);
template.FieldDefinitions.Add(clientField);
```
***

In the last step you need to save the template. Optionally, you may use the [IsDirty](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedFieldsTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedFieldsTemplate_IsDirty) boolean property to verify whether there are any unsaved changes in your template definition, which - at this point - should not be the case:
# [C#](#tab/tabid-12)
```cs
template.Save();
MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());
```
****
Fields templates can be deleted by applying the [Delete](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedFieldsTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedFieldsTemplate_Delete) method (provided that you have the required access rights):
# [C#](#tab/tabid-13)
```cs
template.Delete();
```
*****
The complete function should look as shown below:
# [C#](#tab/tabid-14)
```cs
public void CreateTemplate(TranslationProviderServer tmServer)
{
    #region "CreateTemplate"
    ServerBasedFieldsTemplate template = new ServerBasedFieldsTemplate(tmServer);
    template.Name = "Sample Template";
    template.Description = "Fields template created by API";
    #endregion

    #region "AddTextField"
    FieldDefinition projField = new FieldDefinition("Project id", FieldValueType.MultipleString);
    #endregion

    #region "AddListField"
    FieldDefinition clientField = new FieldDefinition("Client", FieldValueType.MultiplePicklist);
    clientField.PicklistItems.Add("Microsoft");
    clientField.PicklistItems.Add("SAP");
    #endregion

    #region "AddFieldsToTemplate"
    template.FieldDefinitions.Add(projField);
    template.FieldDefinitions.Add(clientField);
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
****

Putting it All Together
---

The complete class should look as shown below:
# [C#](#tab/tabid-15)
```cs
namespace Sdl.SDK.LanguagePlatform.Samples.TmAutomation
{
    using System.Text;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerFieldTemplates
    {
        #region "get"
        public void GetFieldTemplates(TranslationProviderServer tmServer)
        {
            #region "LoopTemplates"
            StringBuilder templateList = new StringBuilder();

            foreach (ServerBasedFieldsTemplate template in tmServer.GetFieldsTemplates(FieldsTemplateProperties.All))
            #endregion
            {
                #region "GeneralTemplateInfo"
                templateList.AppendFormat("Template name: {0}\n", template.Name);
                templateList.AppendFormat("Template description: {0}\n", template.Description);
                #endregion

                #region "fields"
                templateList.AppendLine("Fields: ");
                foreach (FieldDefinition def in template.FieldDefinitions)
                {
                    templateList.AppendFormat("{0}({1})", def.Name, def.ValueType.ToString());
                }

                templateList.AppendLine(string.Empty);
                #endregion
            }

            MessageBox.Show(templateList.ToString(), "Template List");
        }
        #endregion

        #region "GetTms"
        public void GetTmsForTemplate(TranslationProviderServer tmServer, string templateName)
        {
            #region "GetTemplate"
            ServerBasedFieldsTemplate template = tmServer.GetFieldsTemplate(templateName, FieldsTemplateProperties.All);
            #endregion

            #region "TmLoop"
            StringBuilder tmList = new StringBuilder();

            foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
            {
                tmList.AppendLine(tm.Name);
            }

            MessageBox.Show(tmList.ToString());
            #endregion
        }
        #endregion

        #region "create"
        public void CreateTemplate(TranslationProviderServer tmServer)
        {
            #region "CreateTemplate"
            ServerBasedFieldsTemplate template = new ServerBasedFieldsTemplate(tmServer);
            template.Name = "Sample Template";
            template.Description = "Fields template created by API";
            #endregion

            #region "AddTextField"
            FieldDefinition projField = new FieldDefinition("Project id", FieldValueType.MultipleString);
            #endregion

            #region "AddListField"
            FieldDefinition clientField = new FieldDefinition("Client", FieldValueType.MultiplePicklist);
            clientField.PicklistItems.Add("Microsoft");
            clientField.PicklistItems.Add("SAP");
            #endregion

            #region "AddFieldsToTemplate"
            template.FieldDefinitions.Add(projField);
            template.FieldDefinitions.Add(clientField);
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
*****

See Also
-------
[Configuring Translation Memories](configuring_translation_memories.md)

[Adding TM Fields]()