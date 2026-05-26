# Creating the Master Translation Memories

All `.tmx` files for the same language direction must be consolidated into a master TM. If the importer class (see [Importing into the Master Translation Memories](importing_into_the_master_translation_memories.md)) determines that the required master TM does not exist, it creates one by using a separate class. This page explains how to implement that class.

## Add a New Class

Start by adding a class named `TmCreator` to your project. Add a public method named `CreateMasterTm` that takes the path where the master TMs are created and the source and target language locales as string parameters.

## Create the Master TM

Next, implement the logic that creates the master TM file. For simplicity, the file name combines a hard-coded string (`MasterTM_`), the language direction string, and the `.sdltm` extension.

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

    var translationMemory = new FileBasedTranslationMemory(
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


The fuzzy indexes are determined through a separate helper method:
# [C#](#tab/tabid-2)
```cs
/// <summary>
/// Configures the fuzzy indexes. For example, this ensures that
/// the master TM supports concordance searching in the target language.
/// The fuzzy indexes for source and target should be word-based only,
/// not character-based, for performance reasons. Master TMs can be
/// assumed to be large and therefore slow on character-based searches.
/// </summary>
/// <returns>Hard-coded SourceWordBased and TargetWordBased FuzzyIndexes value.</returns>
private FuzzyIndexes GetFuzzyIndexes()
{
    return FuzzyIndexes.SourceWordBased | FuzzyIndexes.TargetWordBased;
}
```
****


The recognition settings are set in a separate helper method:

# [C#](#tab/tabid-3)
```cs
/// <summary>
/// Configures the recognition settings for the TM.
/// This sample enables all recognition settings by using
/// RecognizeAll, which includes variables, dates, numbers,
/// acronyms, measurements, and times.
/// </summary>
/// <returns>Hard-coded RecognizeAll BuiltinRecognizers value.</returns>
private BuiltinRecognizers GetRecognizers()
{
    return BuiltinRecognizers.RecognizeAll;
}
```
***

For more information about these settings and TM creation, see [Creating a File-based Translation Memory](creating_a_file_based_translation_memory.md).

## Putting it All Together

The complete class should look like this:
# [C#](#tab/tabid-4)
```cs
using System.Globalization;
using Sdl.LanguagePlatform.Core.Tokenization;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace SDK.LanguagePlatform.Samples.BatchImporter
{
    /// <summary>
    /// Represents functionality for creating translation memories.
    /// </summary>
    public class TmCreator
    {
        #region "createTM"

        /// <summary>
        /// Creates the master TM based on the source and target locales found in the current TMX file.
        /// </summary>
        /// <param name="sourceLanguage">String representation of the translation memory source language.</param>
        /// <param name="targetLanguage">String representation of the translation memory target language.</param>
        /// <param name="masterPath">Path to the translation memory folder.</param>
        public void CreateMasterTm(string sourceLanguage, string targetLanguage, string masterPath)
        {
            #region "Create Translation Memory"

            string path = string.Format(
                CultureInfo.CurrentCulture,
                "{0}MasterTM_{1}_{2}.sdltm",
                masterPath,
                sourceLanguage,
                targetLanguage);

            var translationMemory = new FileBasedTranslationMemory(
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
        /// Configures the fuzzy indexes. For example, this ensures that
        /// the master TM supports concordance searching in the target language.
        /// The fuzzy indexes for source and target should be word-based only,
        /// not character-based, for performance reasons. Master TMs can be
        /// assumed to be large and therefore slow on character-based searches.
        /// </summary>
        /// <returns>Hard-coded SourceWordBased and TargetWordBased FuzzyIndexes value.</returns>
        private FuzzyIndexes GetFuzzyIndexes()
        {
            return FuzzyIndexes.SourceWordBased | FuzzyIndexes.TargetWordBased;
        }

        #endregion

        #region "get recognizers"

        /// <summary>
        /// Configures the recognition settings for the TM.
        /// This sample enables all recognition settings by using
        /// RecognizeAll, which includes variables, dates, numbers,
        /// acronyms, measurements, and times.
        /// </summary>
        /// <returns>Hard-coded RecognizeAll BuiltinRecognizers value.</returns>
        private BuiltinRecognizers GetRecognizers()
        {
            return BuiltinRecognizers.RecognizeAll;
        }

        #endregion
    }
}

```
****
## See Also

- [Creating the Log File](creating_a_log_file.md)
- [Creating a File-based Translation Memory](creating_a_file_based_translation_memory.md)
