Enabling the Required Translation Provider Features
======
Not all features that are typically supported by translation memories may make sense for a particular translation provider. For example, a machine translation translation provider will usually not offer concordance searching, nor will it be updated during the translation. Our sample plug-in should support segment lookup and concordance searching in the source and target language. However, the delimited list that contains translation solutions should not be updated, i.e. no write access will occur while the list is used.

Implement the Class for Enabling the Provider Features
------
In the project template the class that controls which features the plug-in supports is called `MyTranslationProvider`. For our sample provider, we will rename this class to `ListTranslationProvider`.

This class implements the [ITranslationProvider](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml) interface, which actually forms the main part of a custom translation provider plug-in implementation.

This interface contains numerous members such as [SupportsConcordanceSearch](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProvider_SupportsConcordanceSearch), which you can set to return `True` or `False` depending on whether your implementation is supposed to support e.g. concordance searching or not.

If you set the [SupportsConcordanceSearch](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProvider_SupportsConcordanceSearch) member to `True`, the **Concordance** check box for the selected provider will be enabled in the UI of <Var:ProductName>, which should be the case for our implementation. Users can uncheck these options at runtime if, for some reason, the selected provider should not be taken into account when executing a concordance search. Example: While working, the user discovers that the concordance matches offered by a given translation provider are not helpful for a particular project. In this case, he/she can disable concordance searching for this particular provider, and focus on the matches returned by another provider, for example, a translation memory.

The screenshot below shows an example in which a file TM has been selected for lookup, concordance, and updating. Our sample provider is only enabled for lookup and concordance searching.

<img style="display:block; " src="images/PlugInsSelected.jpg"/>

In this chapter we will not explain the functions of all members (as they are quite numerous), but only the ones that are relevant for our particular implementation.

* **Concordance searching**: As our implementation should support concordance searching, the [SupportsConcordanceSearch](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProvider_SupportsConcordanceSearch) member needs to return `True`:
    # [C#](#tab/tabid-1)
    ```cs
    public bool SupportsConcordanceSearch
    {
        get { return true; }
    }
    ```
    ***

* **Source/target concordance searching**: The concordance search can be sub-divided into support for concordance searching in the source and/or the target language. Since our implementation is supposed to support concordance searching in both languages, the two following members will also be set to return `True`:
    
    ```cs
    public bool SupportsSourceConcordanceSearch
    {
        get { return true; }
    }

    public bool SupportsTargetConcordanceSearch
    {
        get { return true; }
    }
    ```
    
* **Translation unit search**: Our implementation should support matching of translation units to segments in the source document. Therefore, we set [SupportsSearchForTranslationUnits](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProvider_SupportsSearchForTranslationUnits) to return `True`. Note that a translation unit in our implementation corresponds to a delimited line in the list file.
   
    ```cs
    public bool SupportsSearchForTranslationUnits
    {
        get { return true; }
    }
    ```
    
* **Fuzzy searching**: The [SupportsFuzzySearch](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProvider_SupportsFuzzySearch) should be set to return False, as our simplified implementation should only support exact matches:
  
    ```cs
    public bool SupportsFuzzySearch
    {
        get { return false; }
    }
    ```

Examples of Features that the Sample Implementation does not Support
----
Below are a few examples of members in the [ITranslationProvider](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml) interface, which you might have to implement for your particular implementation, but that do not need to be used for our simple delimited list provider.

As our sample plug-in is only supposed to look up and return plain text (i.e. no tagged input), the [SupportsTaggedInput](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProvider_SupportsTaggedInput) property can be set to return `False`:
# [C#](#tab/tabid-2)
```cs
public bool SupportsTaggedInput
{
    get { return false; }
}
```
***

Also, the delimited lists are only supposed to be used for lookup, not for updating the translation provider. For this reason, the [SupportsUpdate`](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProvider_SupportsUpdate)property is also set to return `False`:

# [C#](#tab/tabid-3)
```cs
public bool SupportsUpdate
{
    get { return false; }
}
```
***

Returning `False` has a direct impact on the user interface of <Var:ProductName>, as it will disable the Update check box as shown below:

<img style="display:block; " src="images/UpdateDisabled.jpg"/>

This component of the plug-in is also used to determine whether the selected translation provider list actually supports the selected language combination. This is explained in detail in the next chapter (see [Verifying the Language Pair Support](verifying_the_language_pair_support.md)).