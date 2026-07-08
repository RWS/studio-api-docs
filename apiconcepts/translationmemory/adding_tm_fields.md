# Adding TM Fields

This page explains how to add fields to a translation memory. For background on TM fields and their purpose, see [Configuring Translation Memories](configuring_translation_memories.md).

## Add a New Class

Implement the field logic in a separate class named `TmFieldGenerator`. This example adds a picklist field named *Customer* and a free-text field named *Project id*. Picklist fields use a predefined list of values that you select at runtime. Text fields accept any value. TM fields can also use number and date/time types, but this example focuses on picklist and text fields because they are the most common.

First decide whether the field should be a free-text field or a picklist field. Use picklist fields when you want to limit values to a consistent set, such as *Subject*, *Document type*, or *Customer*. Use free-text fields when the runtime values can vary widely, such as *Project id* or *Comment*.

Add a public method named `AddFields()` that takes the TM file path as a string parameter. Call it as shown below:

# [C#](#tab/tabid-1)
```cs
var tmFieldGenerator = new TmFieldGenerator();
tmFieldGenerator.AddFields(_translationMemoryFilePath);
```
***

In the `AddFields()` method, open the TM to which you want to add the fields. Create the first field, *Customer*, as a picklist by using the [FieldDefinition](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemory.FieldDefinitions.yml) class.

Set the **ValueType** property to define the field as a multiple picklist, and add two values such as *Microsoft* and *RWS*:

# [C#](#tab/tabid-2)
```cs
var tm = new FileBasedTranslationMemory(tmPath);

var listField = new FieldDefinition();
listField.Name = "Customer";
listField.ValueType = FieldValueType.MultiplePicklist;
listField.PicklistItems.Add("RWS");
listField.PicklistItems.Add("Microsoft");
```
***

Create the second field, *Project id*, as a free-text field:

# [C#](#tab/tabid-3)
```cs
var textField = new FieldDefinition();
textField.Name = "Project id";
textField.ValueType = FieldValueType.MultipleString;
```
***

Picklist and text fields can allow a single value or multiple values. Use multiple values when a translation unit can be associated with several customers or project IDs, as in this example.

## Putting it All Together

The complete class should now look like this:

# [C#](#tab/tabid-4)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TmFieldGenerator
    {
        #region "AddFields"
        public void AddFields(string tmPath)
        {
            #region "listField"
            var tm = new FileBasedTranslationMemory(tmPath);

            var listField = new FieldDefinition();
            listField.Name = "Customer";
            listField.ValueType = FieldValueType.MultiplePicklist;
            listField.PicklistItems.Add("RWS");
            listField.PicklistItems.Add("Microsoft");
            #endregion

            #region "textField"
            var textField = new FieldDefinition();
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
***

## See Also
[Configuring Translation Memories](configuring_translation_memories.md)

[Creating a File-based Translation Memory](creating_a_file_based_translation_memory.md)

[TM Fields Templates](tm_fields_templates.md)

