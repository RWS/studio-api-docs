# TM Field Templates

Use field templates on server translation memories when you want multiple TMs to share the same fields. A field template lets you define fields once and reuse them across TMs. When you update the template, every TM that uses it can inherit the change.

This article shows how to retrieve existing field templates, find the TMs that use a template, and create a new template.

## Create the sample class

Add a `ServerFieldTemplates` class to your project. Use it to retrieve existing field templates and create new ones.

## Retrieve available field templates

Start by implementing a `GetFieldTemplates` method that takes a translation provider server object. Build a list of the field templates hosted on the server.
# [C#](#tab/tabid-1)
```cs
var templateList = new StringBuilder();

foreach (ServerBasedFieldsTemplate template in tmServer.GetFieldsTemplates(FieldsTemplateProperties.All))
```
The list should include each template name and description:
# [C#](#tab/tabid-2)
```cs
templateList.AppendFormat("Template name: {0}\n", template.Name);
templateList.AppendFormat("Template description: {0}\n", template.Description);
```
Next, list the field names and types for each template:
# [C#](#tab/tabid-3)
```cs
templateList.AppendLine("Fields: ");
foreach (FieldDefinition def in template.FieldDefinitions)
{
    templateList.AppendFormat("{0}({1})", def.Name, def.ValueType.ToString());
}

templateList.AppendLine(string.Empty);
```

The complete method looks like this:
# [C#](#tab/tabid-4)
```cs
public void GetFieldTemplates(TranslationProviderServer tmServer)
{
    var templateList = new StringBuilder();

    foreach (ServerBasedFieldsTemplate template in tmServer.GetFieldsTemplates(FieldsTemplateProperties.All))
    {
        templateList.AppendFormat("Template name: {0}\n", template.Name);
        templateList.AppendFormat("Template description: {0}\n", template.Description);

        templateList.AppendLine("Fields: ");
        foreach (FieldDefinition def in template.FieldDefinitions)
        {
            templateList.AppendFormat("{0}({1})", def.Name, def.ValueType.ToString());
        }

        templateList.AppendLine(string.Empty);
    }

    MessageBox.Show(templateList.ToString(), "Template List");
}
```

## Retrieve the TMs that use a field template

When you create a server TM, you can assign a field template. The TM then inherits the fields defined in that template. To find the TMs that use a specific template, implement a `GetTmsForTemplate` method that takes the translation provider server and template name.
# [C#](#tab/tabid-5)
```cs
ServerBasedFieldsTemplate template = tmServer.GetFieldsTemplate(templateName, FieldsTemplateProperties.All);
```
Next, create a string builder and add the names of the TMs that use the selected template:
# [C#](#tab/tabid-6)
```cs
var tmList = new StringBuilder();

foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
{
    tmList.AppendLine(tm.Name);
}

MessageBox.Show(tmList.ToString());
```

The complete method looks like this:
# [C#](#tab/tabid-7)
```cs
public void GetTmsForTemplate(TranslationProviderServer tmServer, string templateName)
{
    ServerBasedFieldsTemplate template = tmServer.GetFieldsTemplate(templateName, FieldsTemplateProperties.All);

    var tmList = new StringBuilder();

    foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
    {
        tmList.AppendLine(tm.Name);
    }

    MessageBox.Show(tmList.ToString());
}
```

## Create a new field template

Create a `CreateTemplate` method that takes the translation provider server. In this example, the template contains a text field named *Project Id* and a list field named *Client*. Set the template name and optional description when you create the object.
# [C#](#tab/tabid-8)
```cs
var template = new ServerBasedFieldsTemplate(tmServer);
template.Name = "Sample Template";
template.Description = "Fields template created by API";
```

Create the text field definition by specifying the field name and the field type. Use `MultipleString` for a text field:
# [C#](#tab/tabid-9)
```cs
var projectIdField = new FieldDefinition("Project id", FieldValueType.MultipleString);
```

Create the list field in the same way. Add sample picklist values to the field definition:
# [C#](#tab/tabid-10)
```cs
var clientField = new FieldDefinition("Client", FieldValueType.MultiplePicklist);
clientField.PicklistItems.Add("Microsoft");
clientField.PicklistItems.Add("SAP");
```

Add both field definitions to the template:
# [C#](#tab/tabid-11)
```cs
template.FieldDefinitions.Add(projectIdField);
template.FieldDefinitions.Add(clientField);
```

Save the template and optionally check [IsDirty](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedFieldsTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedFieldsTemplate_IsDirty) to confirm that no unsaved changes remain.
# [C#](#tab/tabid-12)
```cs
template.Save();
MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());
```

Delete the template with [Delete](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedFieldsTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedFieldsTemplate_Delete) if you have the required access rights.
# [C#](#tab/tabid-13)
```cs
template.Delete();
```

The complete method looks like this:
# [C#](#tab/tabid-14)
```cs
public void CreateTemplate(TranslationProviderServer tmServer)
{
    var template = new ServerBasedFieldsTemplate(tmServer);
    template.Name = "Sample Template";
    template.Description = "Fields template created by API";

    var projectIdField = new FieldDefinition("Project id", FieldValueType.MultipleString);

    var clientField = new FieldDefinition("Client", FieldValueType.MultiplePicklist);
    clientField.PicklistItems.Add("Microsoft");
    clientField.PicklistItems.Add("SAP");

    template.FieldDefinitions.Add(projectIdField);
    template.FieldDefinitions.Add(clientField);

    template.Save();
    MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());

    template.Delete();
}
```

## Complete the sample

The complete class looks like this:
# [C#](#tab/tabid-15)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System.Text;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerFieldTemplates
    {
        public void GetFieldTemplates(TranslationProviderServer tmServer)
        {
            var templateList = new StringBuilder();

            foreach (ServerBasedFieldsTemplate template in tmServer.GetFieldsTemplates(FieldsTemplateProperties.All))
            {
                templateList.AppendFormat("Template name: {0}\n", template.Name);
                templateList.AppendFormat("Template description: {0}\n", template.Description);

                templateList.AppendLine("Fields: ");
                foreach (FieldDefinition def in template.FieldDefinitions)
                {
                    templateList.AppendFormat("{0}({1})", def.Name, def.ValueType.ToString());
                }

                templateList.AppendLine(string.Empty);
            }

            MessageBox.Show(templateList.ToString(), "Template List");
        }

        public void GetTmsForTemplate(TranslationProviderServer tmServer, string templateName)
        {
            ServerBasedFieldsTemplate template = tmServer.GetFieldsTemplate(templateName, FieldsTemplateProperties.All);

            var tmList = new StringBuilder();

            foreach (ServerBasedTranslationMemory tm in template.TranslationMemories)
            {
                tmList.AppendLine(tm.Name);
            }

            MessageBox.Show(tmList.ToString());
        }

        public void CreateTemplate(TranslationProviderServer tmServer)
        {
            var template = new ServerBasedFieldsTemplate(tmServer);
            template.Name = "Sample Template";
            template.Description = "Fields template created by API";

            var projectIdField = new FieldDefinition("Project id", FieldValueType.MultipleString);

            var clientField = new FieldDefinition("Client", FieldValueType.MultiplePicklist);
            clientField.PicklistItems.Add("Microsoft");
            clientField.PicklistItems.Add("SAP");

            template.FieldDefinitions.Add(projectIdField);
            template.FieldDefinitions.Add(clientField);

            template.Save();
            MessageBox.Show("Unsaved changes? " + template.IsDirty.ToString());

            template.Delete();
        }
    }
}
```

## See also

[Configuring Translation Memories](configuring_translation_memories.md)

[Adding TM Fields](adding_tm_fields.md)
