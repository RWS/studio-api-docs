# Implementing the Search Functionality

This page explains how to implement the search functionality and display the results in the rich text control by using a simple string output.

## Implement the Search Function

Start by adding a class named `Search` to your project. Implement a public method named `DoConcordanceSearch` that takes the search text as a string parameter and a Boolean parameter that indicates whether the search should run in the target language. Also pass the `SelectedIndex` property of the `comboLanguagePairs` control as another parameter. The index in this list corresponds to the index of the server TM language pair. The method returns the search result as a string.

The method is called when the user clicks the Search button on the main lookup form, which then fills the rich text control with the search result:
# [C#](#tab/tabid-1)
```cs
private void btnSearch_Click(object sender, EventArgs e)
{
    try
    {
        Search search = new Search();

        // Determine whether to do the concordance search in the
        // source or in the target language;
        bool searchTarget;
        if (this.comboIndex.Text == "Target")
            searchTarget = true;
        else
            searchTarget = false;

        // Fill the search result into the rich text box.
        this.lblHitCount.Text = search.DoConcordanceSearch(this.txtSearch.Text, searchTarget, 
            comboLanguagePairs.SelectedIndex);
    }
    catch(Exception ex)
    {
        MessageBox.Show("No TM selected." + ex.Message);
    }
}
```
***

In the `DoConcordanceSearch` method, first run the search in the selected TM, either file-based or server-based. The Boolean `server` property from the `Connector` class (see [Adding the Connector Class](adding_the_connector_class.md)) indicates whether the search should run against a file TM or a server TM.
# [C#](#tab/tabid-2)
```cs
/// Do the search in either a file- or server-based TM.
SearchResults results;
if (Connector.server) 
{
    results = Connector.serverTM.LanguageDirections[langDirIndex].SearchText(GetSearchSettings(target), searchText);
}
else 
{
    results = Connector.fileTm.LanguageDirection.SearchText(GetSearchSettings(target), searchText);
}
```
***

The `SearchText` method takes the search string and search settings as parameters. Return the search settings from a separate method. The settings come from the search settings form (see [Adding the Search Settings Form](adding_the_search_settings_form.md)).
# [C#](#tab/tabid-3)
```cs
/// <summary>
/// Configure the settings that should be applied for the search, i.e.
/// minimum match value, maximum hit count, and whether the concordance
/// search should be done in the source or in the target language.
/// </summary>
private SearchSettings GetSearchSettings(bool target)
{
    SearchSettings settings = new SearchSettings();

    settings.MaxResults = frmSettings.maxHits;
    settings.MinScore = frmSettings.minFuzzy;

    if (target)
        settings.Mode = SearchMode.TargetConcordanceSearch;
    else
        settings.Mode = SearchMode.ConcordanceSearch;

    return settings;
}
```
***

Next, build the hit result string by adding the hit count:
# [C#](#tab/tabid-4)
```cs
/// Build up the string that holds the hitlist result.
string hitlist;
hitlist = "Hit count: " + results.Count.ToString() + "\n\n";
```
***

Next, iterate through the search results:
# [C#](#tab/tabid-5)
```cs
foreach (SearchResult result in results)
{
    hitlist += GetTuInformation(result);
}
```
***

The loop continues to build the hit result string by calling a separate `GetTuInformation` helper method. This method returns TU information such as the source and target segments, the creation date, field names and values, and the match value:
# [C#](#tab/tabid-6)
```cs
/// <summary>
/// This function returns further information on the given translation unit (TU).
/// </summary>
private string GetTuInformation(SearchResult tuResult)
{
    string tuInfo;

    /// The matching score
    tuInfo = "\nScore: " + tuResult.ScoringResult.Match.ToString() + "%\n";

    /// The source and target segments
    tuInfo += "Source: " + tuResult.MemoryTranslationUnit.SourceSegment.ToPlain() + "\n";
    tuInfo += "Target: " + tuResult.MemoryTranslationUnit.TargetSegment.ToPlain() + "\n";

    /// The TU creation date.
    tuInfo += "Creation date: " + tuResult.MemoryTranslationUnit.SystemFields.CreationDate + "\n";

    /// Any field values (e.g. Customer, Project id, etc. associated with the
    /// given TU.
    foreach (FieldValue field in tuResult.MemoryTranslationUnit.FieldValues)
    {
        tuInfo += field.Name + ": " + field.ToString();
    }
    tuInfo += "\n";

    return tuInfo;
}
```
***

At the end, the `DoConcordanceSearch` method returns the full hit result string, which is then used to populate the rich text control:
# [C#](#tab/tabid-7)
```cs
return hitlist;
```
***

## Apply the Search Settings

Add the following private method named `GetSearchSettings`, which configures the search settings according to the values on the search settings form (see [Adding the Search Settings Form](adding_the_search_settings_form.md)).
# [C#](#tab/tabid-8)
```cs
/// <summary>
/// Configure the settings that should be applied for the search, i.e.
/// minimum match value, maximum hit count, and whether the concordance
/// search should be done in the source or in the target language.
/// </summary>
private SearchSettings GetSearchSettings(bool target)
{
    SearchSettings settings = new SearchSettings();

    settings.MaxResults = frmSettings.maxHits;
    settings.MinScore = frmSettings.minFuzzy;

    if (target)
        settings.Mode = SearchMode.TargetConcordanceSearch;
    else
        settings.Mode = SearchMode.ConcordanceSearch;

    return settings;
}
```
***

See also [Doing Translation Memory Lookups](doing_translation_memory_lookups.md) for more information about the available search settings.

## Putting it All Together

The complete search class should look like this:
# [C#](#tab/tabid-9)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.Core.Tokenization;
using Sdl.LanguagePlatform.Core;


namespace SDK.LanguagePlatform.Samples.TmLookup
{
    class Search
    {
        /// <summary>
        /// This function performs the actual concordance search operation.
        /// </summary>
        public string DoConcordanceSearch(string searchText, bool target, int langDirIndex)
        {
            // Run the search in either a file-based or server-based TM.
            SearchResults results;
            if (Connector.server) 
            {
                results = Connector.serverTM.LanguageDirections[langDirIndex].SearchText(GetSearchSettings(target), searchText);
            }
            else {
                results = Connector.fileTm.LanguageDirection.SearchText(GetSearchSettings(target), searchText);
            }            

            // Build the string that holds the hit result.
            string hitlist;
            hitlist = "Hit count: " + results.Count.ToString() + "\n\n";

            foreach (SearchResult result in results)
            {
                hitlist += GetTuInformation(result);
            }  

            #region "CloseHitlist"
            return hitlist;
        }
              
        /// <summary>
        /// This function returns further information on the given translation unit (TU).
        /// </summary>
        private string GetTuInformation(SearchResult tuResult)
        {
            string tuInfo;

            // The matching score.
            tuInfo = "\nScore: " + tuResult.ScoringResult.Match.ToString() + "%\n";

            // The source and target segments.
            tuInfo += "Source: " + tuResult.MemoryTranslationUnit.SourceSegment.ToPlain() + "\n";
            tuInfo += "Target: " + tuResult.MemoryTranslationUnit.TargetSegment.ToPlain() + "\n";

            // The TU creation date.
            tuInfo += "Creation date: " + tuResult.MemoryTranslationUnit.SystemFields.CreationDate + "\n";

            // Any field values, such as Customer or Project ID, associated with the
            // given TU.
            foreach (FieldValue field in tuResult.MemoryTranslationUnit.FieldValues)
            {
                tuInfo += field.Name + ": " + field.ToString();
            }
            tuInfo += "\n";

            return tuInfo;
        }   
        
        /// <summary>
        /// Configure the settings that should be applied for the search, including
        /// minimum match value, maximum hit count, and whether the concordance
        /// search should run in the source or target language.
        /// </summary>
        private SearchSettings GetSearchSettings(bool target)
        {
            SearchSettings settings = new SearchSettings();

            settings.MaxResults = frmSettings.maxHits;
            settings.MinScore = frmSettings.minFuzzy;

            if (target)
                settings.Mode = SearchMode.TargetConcordanceSearch;
            else
                settings.Mode = SearchMode.ConcordanceSearch;

            return settings;
        }
    }
}
```
***
