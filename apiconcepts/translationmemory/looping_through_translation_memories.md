Looping through Translation Memories
==

Iterating through all the translation units in a TM can be useful to perform various maintenance-related tasks. Suppose you need to batch-delete a number of TUs, e.g. all TUs that do belong to a certain customer, or all TUs that are older than two years, as they can be considered outdated. In this case, you can loop through the TUs and apply a delete action to each TU that fulfills (or does not fulfill) a specific criteria. In this chapter we would like to demonstrate how to programmatically handle such a use case by deleting all TUs that belong to the client *Microsoft*, i.e. where the TM field *Customer* has the value *Microsoft*.

Add a New Class
--

Start by adding new class called ```TmIterator``` to your project. Implement a public function called ```Iterate```, which takes the TM file name and path as string parameter. For this sample scenario, let us assume that the aim is to iterate through a given TM and output the source segments of each translation unit that has the field value *Customer* -> *Microsoft*.

Start by opening the TM as shown below:

```cs
FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);
```

Then create a TM iterator object as follows:

```cs
RegularIterator tmIterator = new RegularIterator(1);
```

The **RegularIterator** class can take the maximum amount of TUs to return in one roundtrip as parameter. If you do not provide a value, the iterator will move through the TM in increments of 100. You may want to specify a lower value for performance reasons, especially when accessing TMs through an Internet connection. In the next step you retrieve all TUs (ans store them in an array) by applying the [GetTranslationUnits](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.FileBasedTranslationMemoryLanguageDirection.yml#Sdl_LanguagePlatform_TranslationMemoryApi_FileBasedTranslationMemoryLanguageDirection_GetTranslationUnits_Sdl_LanguagePlatform_TranslationMemory_RegularIterator__) method, which takes the previously created iterator object as parameter:

```cs
TranslationUnit[] Tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
```

Use a ```while``` loop to go through the TM until the iterator TU count is still greater than zero:

```cs
while (Tus.Count<TranslationUnit>() > 0)
```

Note that since we specified the iterator to move through the TM in increments of 1, the count will be 1. Next, loop through the actual TUs:

```cs
foreach (TranslationUnit Tu in Tus)
```

Then, traverse the field values of each TU in order to ascertain whether there is a *Customer* field value *Microsoft*.

```cs
foreach (FieldValue item in Tu.FieldValues)
```

Use an ```if``` to determine the name of the current field:

```cs
if (item.Name == "Customer")
```
If the current customer field value equals *Microsoft*, output the source segment:

```cs
if (item.ToString() == "Microsoft")
{
    MessageBox.Show(Tu.SourceSegment.ToPlain());
}
```

In the last step, update the TU collection using the TM iterator object. This will cause the iterator to move to the next set of TUs in the translation memory.


```cs
Tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
```

The full loop looks as shown below:


```cs
#region "while"
while (Tus.Count<TranslationUnit>() > 0)
#endregion
{
    #region "LoopTus"
    foreach (TranslationUnit Tu in Tus)
    #endregion
    {
        #region "LoopValues"
        foreach (FieldValue item in Tu.FieldValues)
        #endregion
        {
            #region "DetermineFieldName"
            if (item.Name == "Customer")
            #endregion
            {
                #region "DetermineFieldValue"
                if (item.ToString() == "Microsoft")
                {
                    MessageBox.Show(Tu.SourceSegment.ToPlain());
                }
                #endregion
            }
        }
    }
    #region "update"
    Tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
    #endregion
}
```

Putting it All Together
--

Your complete class should now look as shown below:

```cs
namespace Sdl.SDK.LanguagePlatform.Samples.TmAutomation
{
    using System.Linq;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TMIterator
    {
        public void Iterate(string tmPath)
        {
            #region "open"
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);
            #endregion

            #region "iterator"
            RegularIterator tmIterator = new RegularIterator(1);
            #endregion

            #region "GetTUs"
            TranslationUnit[] Tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
            #endregion

            #region "loop"
            #region "while"
            while (Tus.Count<TranslationUnit>() > 0)
            #endregion
            {
                #region "LoopTus"
                foreach (TranslationUnit Tu in Tus)
                #endregion
                {
                    #region "LoopValues"
                    foreach (FieldValue item in Tu.FieldValues)
                    #endregion
                    {
                        #region "DetermineFieldName"
                        if (item.Name == "Customer")
                        #endregion
                        {
                            #region "DetermineFieldValue"
                            if (item.ToString() == "Microsoft")
                            {
                                MessageBox.Show(Tu.SourceSegment.ToPlain());
                            }
                            #endregion
                        }
                    }
                }
                #region "update"
                Tus = tm.LanguageDirection.GetTranslationUnits(ref tmIterator);
                #endregion
            }
            #endregion
        }
    }
}
```

See Also
--

**Other Resources**

[Maintaining Translation Memories](maintaining_translation_memories.md)
