Creating the Master Translation Memories
======
Remember that all *.tmx files of the same language direction need to be consolidated in a master TM. If the importer class (see [Importing into the Master Translation Memories](importing_into_the_master_translation_memories.md)) determines that the corresponding master TM does not yet exist, it should be created through a separate class. On this page you will learn how to implement the class for creating the master TMs.

Add a New Class
-----
Start by adding a new class called TmCreator to your project. Add a public function called `CreateMasterTm`, which takes the path in which the master TMs should be created and the source/target language locales as string parameters.

Create the Master TM
------
In the next step, implement the logic required for creating the master TM file. Note that for simplicity's sake, the file name is a combination of a hard-coded string *(MasterTM_)*, the language direction string, and the extension *.sdltm.

# [C#](#tab/tabid-1)
```cs
/// <summary>
/// Create the master TM based on the source and target locales found in the current TMX file.
/// </summary>
/// <param name="sourceLanguage">String representation of translation memory source language.</param>
/// <param name="targetLanguage">String representation of translation memory target language.</param>
/// <param name="masterPath">Path to translation memory.</param>
public void CreateMasterTm(string sourceLanguage, string targetLanguage, string masterPath)
{
    #region "Create Translation Memory"

    string path = string.Format(
        CultureInfo.CurrentCulture,
        "{0}MasterTm_{1}_{2}.sdltm",
        masterPath,
        sourceLanguage,
        targetLanguage);
    FileBasedTranslationMemory translationMemory = new FileBasedTranslationMemory(
        path,
        "Master TM",
        CultureInfo.GetCultureInfo(sourceLanguage),
        CultureInfo.GetCultureInfo(targetLanguage),
        this.GetFuzzyIndexes(),
        this.GetRecognizers(),
        TokenizerFlags.AllFlags,
        WordCountFlags.AllFlags
        );

    #endregion

    #region "Save"

    translationMemory.LanguageResourceBundles.Clear();
    translationMemory.Save();

    #endregion
}
```
****


The fuzzy indexes are determined through a separate helper function:
# [C#](#tab/tabid-2)
```cs
/// <summary>
/// Configure the fuzzy indexes, to determine, for example,
/// that the master TM should support concordance searching 
/// in the target language.
/// The fuzzy indexes for source and target should be word-based only
/// i.e. not character-based for performance reasons. (Master TMs
/// can be assumed to be big, and therefore slow on character-based searches.)
/// </summary>
/// <returns>Hard-coded SourceWordBased and TargetWordBased FuzzyIndexes type</returns>
private FuzzyIndexes GetFuzzyIndexes()
{
    return FuzzyIndexes.SourceWordBased | FuzzyIndexes.TargetWordBased;
}
```
****


Also, the recognition settings are set in a distinct helper function:

# [C#](#tab/tabid-3)
```cs
/// <summary>
/// Configure the recognition settings for the TM.
/// Here, we simply activate all recognition settings 
/// through RecognizeAll, which includes recognition
/// of variables, dates, numbers, acronyms, measurements, and times.
/// </summary>
/// <returns>Hard-coded RecognizeAll BuiltinRecognizer.</returns>
private BuiltinRecognizers GetRecognizers()
{
    return BuiltinRecognizers.RecognizeAll;
}
```
***

For more information, on these settings and on TM creation see also [Creating a File-based Translation Memory](creating_a_file_based_translation_memory.md).

Putting it All Together
----
The complete class should look as shown below:
# [C#](#tab/tabid-4)
```cs
namespace SDK.LanguagePlatform.Samples.BatchImporter
{
    using System.Globalization;
    using Sdl.LanguagePlatform.Core.Tokenization;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    /// <summary>
    /// Represents functionality for creating Translation Memories.
    /// </summary>
    public class TMCreator
    {
        #region "createTM"

        /// <summary>
        /// Create the master TM based on the source and target locales found in the current TMX file.
        /// </summary>
        /// <param name="sourceLanguage">String representation of translation memory source language.</param>
        /// <param name="targetLanguage">String representation of translation memory target language.</param>
        /// <param name="masterPath">Path to translation memory.</param>
        public void CreateMasterTm(string sourceLanguage, string targetLanguage, string masterPath)
        {
            #region "Create Translation Memory"

            string path = string.Format(
                CultureInfo.CurrentCulture,
                "{0}MasterTm_{1}_{2}.sdltm",
                masterPath,
                sourceLanguage,
                targetLanguage);
            FileBasedTranslationMemory translationMemory = new FileBasedTranslationMemory(
                path,
                "Master TM",
                CultureInfo.GetCultureInfo(sourceLanguage),
                CultureInfo.GetCultureInfo(targetLanguage),
                this.GetFuzzyIndexes(),
                this.GetRecognizers(),
                TokenizerFlags.AllFlags,
                WordCountFlags.AllFlags
                );

            #endregion

            #region "Save"

            translationMemory.LanguageResourceBundles.Clear();
            translationMemory.Save();

            #endregion
        }
        #endregion

        #region "Get fuzzy indexes"

        /// <summary>
        /// Configure the fuzzy indexes, to determine, for example,
        /// that the master TM should support concordance searching 
        /// in the target language.
        /// The fuzzy indexes for source and target should be word-based only
        /// i.e. not character-based for performance reasons. (Master TMs
        /// can be assumed to be big, and therefore slow on character-based searches.)
        /// </summary>
        /// <returns>Hard-coded SourceWordBased and TargetWordBased FuzzyIndexes type</returns>
        private FuzzyIndexes GetFuzzyIndexes()
        {
            return FuzzyIndexes.SourceWordBased | FuzzyIndexes.TargetWordBased;
        }

        #endregion

        #region "get recognizers"

        /// <summary>
        /// Configure the recognition settings for the TM.
        /// Here, we simply activate all recognition settings 
        /// through RecognizeAll, which includes recognition
        /// of variables, dates, numbers, acronyms, measurements, and times.
        /// </summary>
        /// <returns>Hard-coded RecognizeAll BuiltinRecognizer.</returns>
        private BuiltinRecognizers GetRecognizers()
        {
            return BuiltinRecognizers.RecognizeAll;
        }

        #endregion
    }
}

```
****
See Also
-------------
[Creating the Log File](creating_a_log_file.md)

[Creating a File-based Translation Memory](creating_a_file_based_translation_memory.md)