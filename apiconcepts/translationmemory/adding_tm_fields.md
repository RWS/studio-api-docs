Adding TM Fields
This page shows how to add fields to a translation memory. For more information on what TM fields are and what their purpose is, please refer to [Configuring Translation Memories](configuring_translation_memories.md).

Add a New Class
--

As we are going to implement the functionality for defining TM fields in a separate class, create a new class called ```TmFieldGenerator```. Let us assume that you need to add a picklist field called Customer and a free text field called *Project id*. Picklist fields are always associated with a pre-defined list of values, which can later be selected as required at runtime. Text fields, on the other hand, can be filled with any text. Apart from picklist and text, fields can also be of the types number and date/time. However, since picklist and text fields constitute the most common types, we fill focus on those two.

The first step is decide whether you want to configure a field as a free text field or a picklist field. Fields that should be associated with only a limited number of values that should be used consistently, will usually be of the type picklist. Common examples are *Subject, Document type, Customer*, etc. Free text fields should be used when at runtime there are virtually limitless possible values. Common examples here are *Project id, Comment*, etc.

Start by adding a public function called ```AddFields```, which takes the file path and name as string parameter. This function can be called as shown below:

```cs
TMFieldGenerator objFieldGenerator = new TMFieldGenerator();
objFieldGenerator.AddFields(_translationMemoryFilePath);
```

In the ```AddFields``` function start by opening the TM to which the fields should be added. Our first field should be called *Customer*, and should be of the type list. Create a list field object through the [FieldDefinition](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemory.FieldDefinitions.yml) class.

Apply the **ValueType** property to define the field as a picklist that can hold multiple values. Finally, add two values, e.g. *Microsoft* and *SDL*:

```cs
FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);

FieldDefinition listField = new FieldDefinition();
listField.Name = "Customer";
listField.ValueType = FieldValueType.MultiplePicklist;
listField.PicklistItems.Add("SDL");
listField.PicklistItems.Add("Microsoft");
```

The second example should be a free text field, which we call *Project id*. Therefore, create a free text field object:

```cs
FieldDefinition textField = new FieldDefinition();
textField.Name = "Project id";
textField.ValueType = FieldValueType.MultipleString;
```

Note that picklist and text fields can allow for only a single value or for multiple values, e.g. if a translation unit can be associated with several customers or project ids. In the above example we define the fields as multiple.

Putting it All Together
--

The complete class should now look as shown below:

```cs
namespace Sdl.SDK.LanguagePlatform.Samples.TmAutomation
{
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TMFieldGenerator
    {
        #region "AddFields"
        public void AddFields(string tmPath)
        {
            #region "listField"
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);

            FieldDefinition listField = new FieldDefinition();
            listField.Name = "Customer";
            listField.ValueType = FieldValueType.MultiplePicklist;
            listField.PicklistItems.Add("SDL");
            listField.PicklistItems.Add("Microsoft");
            #endregion

            #region "textField"
            FieldDefinition textField = new FieldDefinition();
            textField.Name = "Project id";
            textField.ValueType = FieldValueType.MultipleString;
            #endregion

            #region "add"
            tm.FieldDefinitions.Add(listField);
            tm.FieldDefinitions.Add(textField);
            #endregion

            tm.Save();
        }
        #endregion
    }
}
```

See Also
--

**Other Resources**

[Configuring Translation Memories](configuring_translation_memories.md)

[Creating a File-based Translation Memory](creating_a_file_based_translation_memory.md)

[TM Fields Templates](tm_fields_templates.md)

