# Looping through Translation Memories

Iterating through all translation units in a TM is useful for maintenance tasks. For example, you might want to delete TUs for a specific customer or remove outdated TUs. In this chapter, you will see how to delete all TUs that belong to the client *Microsoft*, where the TM field *Customer* has the value *Microsoft*.

## Add a New Class

Start by adding a new class named `TmIterator` to your project. Implement a public method named `Iterate()` that takes the TM path as a string parameter. In this example, the method iterates through a TM and outputs the source segments of each TU where *Customer* equals *Microsoft*.

Start by opening the TM as shown below:

# [C#](#tab/tabid-1)
```cs
var tm = new FileBasedTranslationMemory(tmPath);
```
***

Then create a TM iterator object as follows:
# [C#](#tab/tabid-2)
```cs
var tmIterator = new RegularIterator(1);
```
***

The **RegularIterator** class accepts the maximum number of TUs to return in one round trip. If you do not provide a value, the iterator moves through the TM in increments of 100. You may want to use a lower value for performance reasons, especially when you access TMs over a network. In the next step, retrieve all TUs by calling the [GetTranslationUnits](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.FileBasedTranslationMemoryLanguageDirection.yml#Sdl_LanguagePlatform_TranslationMemoryApi_FileBasedTranslationMemoryLanguageDirection_GetTranslationUnits_Sdl_LanguagePlatform_TranslationMemory_RegularIterator__) method with the iterator object:

# [C#](#tab/tabid-3)
```cs
TranslationUnit[] tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
```
***

Use a `while` loop to continue until no TUs remain:

# [C#](#tab/tabid-4)
```cs
while (tus.Length > 0)
```
***

Because this example uses an iterator increment of 1, the array contains one TU per batch. Next, loop through the TUs:

# [C#](#tab/tabid-5)
```cs
foreach (TranslationUnit tu in tus)
```
***

Then inspect the field values of each TU to determine whether *Customer* equals *Microsoft*.

# [C#](#tab/tabid-6)
```cs
foreach (FieldValue value in tu.FieldValues)
```
***

Use an `if` statement to check the current field name:

# [C#](#tab/tabid-7)
```cs
if (value.Name == "Customer")
```
***

If the current *Customer* field value equals *Microsoft*, output the source segment:

# [C#](#tab/tabid-8)
```cs
if (value.ToString() == "Microsoft")
{
    MessageBox.Show(tu.SourceSegment.ToPlain());
}
```
***

Finally, update the TU collection by using the TM iterator object. This moves the iterator to the next batch of TUs in the translation memory.

# [C#](#tab/tabid-9)
```cs
tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
```
***

The full loop looks like this:

# [C#](#tab/tabid-10)
```cs

while (tus.Length > 0)
{
    #region "LoopTus"
    foreach (TranslationUnit tu in tus)
    #endregion
    {
        #region "LoopValues"
        foreach (FieldValue value in tu.FieldValues)
        #endregion
        {
            #region "DetermineFieldName"
            if (value.Name == "Customer")
            #endregion
            {
                #region "DetermineFieldValue"
                if (value.ToString() == "Microsoft")
                {
                    MessageBox.Show(tu.SourceSegment.ToPlain());
                }
                #endregion
            }
        }
    }
    #region "update"
    tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
    #endregion
}
```
***

## Putting it All Together

Your complete class should now look like this:

# [C#](#tab/tabid-11)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TmIterator
    {
        public void Iterate(string tmPath)
        {
            #region "open"
            var tm = new FileBasedTranslationMemory(tmPath);
            #endregion

            #region "iterator"
            var tmIterator = new RegularIterator(1);
            #endregion

            #region "GetTUs"
            TranslationUnit[] tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
            #endregion

            #region "loop"
            #region "while"
            while (tus.Length > 0)
            #endregion
            {
                #region "LoopTus"
                foreach (TranslationUnit tu in tus)
                #endregion
                {
                    #region "LoopValues"
                    foreach (FieldValue value in tu.FieldValues)
                    #endregion
                    {
                        #region "DetermineFieldName"
                        if (value.Name == "Customer")
                        #endregion
                        {
                            #region "DetermineFieldValue"
                            if (value.ToString() == "Microsoft")
                            {
                                MessageBox.Show(tu.SourceSegment.ToPlain());
                            }
                            #endregion
                        }
                    }
                }
                #region "update"
                tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
                #endregion
            }
            #endregion
        }
    }
}
```
***

## See Also
[Maintaining Translation Memories](maintaining_translation_memories.md)
