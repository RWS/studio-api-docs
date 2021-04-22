Implementing the Search Functionality
======
This page outlines how to implement the actual search functionality and how to output the results in the rich text element using a simple string output.

Implement the Search Function
-------
Start by adding a new class called `Search` to your project. Implement a public function called `DoConcordanceSearch`, which takes the search text as string parameter and a boolean parameter that indicates whether the search should be done in the target index. In addition, we pass the `SelectedIndex` property of the `comboLanguagePairs` control as another parameter. The index of this list corresponds to the index of the server TM language pair. The function returns the search result as a string.

The function is called by clicking the search button on the main lookup form, which then fills the rich text control with the search result:
# [C#](#tab/tabid-1)
```cs
private void btnSearch_Click(object sender, EventArgs e)
{            
    try
    {
        #region "SearchObject"
        Search search = new Search();
        #endregion

        #region "SourceOrTarget"
        // Determine whether to do the concordance search in the
        // source or in the target language;
        bool searchTarget;
        if (this.comboIndex.Text == "Target")
            searchTarget = true;
        else
            searchTarget = false;
        #endregion

        #region "FillHitlist"
        // Fill the search result into the rich text box.
        this.lblHitCount.Text = search.DoConcordanceSearch(this.txtSearch.Text, searchTarget, 
            comboLanguagePairs.SelectedIndex);
        #endregion
    }
    catch(Exception ex)
    {
        MessageBox.Show("No TM selected." + ex.Message);
    }
}
```
***

In the `DoConcordanceSearch` function you first execute the search in the selected TM (file or server TM). The boolean server property from the Connector (see [Adding the Connector Class](adding_the_connector_class.md)) class flags whether the search should be applied to a file or server TM object.
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

The `SearchText` method takes the search string as well as the search settings as parameters. The search settings (as set by the user on the corresponding form, see [Adding the Search Settings Form](adding_the_search_settings_form.md)) are returned by a separate function:
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

Next, you start building up the hit result string by determining the hit count:
# [C#](#tab/tabid-4)
```cs
/// Build up the string that holds the hitlist result.
string hitlist;
hitlist = "Hit count: " + results.Count.ToString() + "\n\n";
```
***

In the following step, you need to traverse the search results:
# [C#](#tab/tabid-5)
```cs
foreach (SearchResult result in results)
{
    hitlist += GetTuInformation(result);
}
```
***

The loop continues to build up the hitlist string by calling a separate `GetTuInformation` helper function, which returns TU information such as the source/target segments, the creation date, any field names and values, as well as the match value:
# [C#](#tab/tabid-6)
```cs
/// <summary>
/// This function returns further information on the given translation unit (TU).
/// </summary>
private string GetTuInformation(SearchResult tuResult)
{
    string tuInfo;

    #region "score"
    /// The matching score
    tuInfo = "\nScore: " + tuResult.ScoringResult.Match.ToString() + "%\n";
    #endregion

    #region "segments"
    /// The source and target segments
    tuInfo += "Source: " + tuResult.MemoryTranslationUnit.SourceSegment.ToPlain() + "\n";
    tuInfo += "Target: " + tuResult.MemoryTranslationUnit.TargetSegment.ToPlain() + "\n";
    #endregion

    #region "date"
    /// The TU creation date.
    tuInfo += "Creation date: " + tuResult.MemoryTranslationUnit.SystemFields.CreationDate + "\n";
    #endregion

    #region "fields"
    /// Any field values (e.g. Customer, Project id, etc. associated with the
    /// given TU.
    foreach (FieldValue field in tuResult.MemoryTranslationUnit.FieldValues)
    {
        tuInfo += field.Name + ": " + field.ToString();
    }
    tuInfo += "\n";
    #endregion

    return tuInfo;
}
```
***

At the end the `DoConcordanceSearch` function returns the full hit result string, which is then used to fill the rich text control:
# [C#](#tab/tabid-7)
```cs
return hitlist;
```
***

Apply the Search Settings
------
Add the following private function called `GetSearchSettings`, which configures the search settings according to the settings configured on the search settings form (see [Adding the Search Settings Form](adding_the_search_settings_form.md)).
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

See also [Doing Translation Memory Lookups]() for more information on the available search settings.

Putting it All Together
-----
The complete search class should look as shown below:
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


namespace Sdl.SDK.LanguagePlatform.Samples.TmLookup
{
    class Search
    {
        #region "search"
        /// <summary>
        /// This function performs the actual concordance search operation.
        /// </summary>
        public string DoConcordanceSearch(string searchText, bool target, int langDirIndex)
        {
            #region "execute"
            /// Do the search in either a file- or server-based TM.
            SearchResults results;
            if (Connector.server) 
            {
                results = Connector.serverTM.LanguageDirections[langDirIndex].SearchText(GetSearchSettings(target), searchText);
            }
            else {
                results = Connector.fileTm.LanguageDirection.SearchText(GetSearchSettings(target), searchText);
            }            
            #endregion

            #region "StartHitlist"
            /// Build up the string that holds the hitlist result.
            string hitlist;
            hitlist = "Hit count: " + results.Count.ToString() + "\n\n";
            #endregion

            #region "result"
            foreach (SearchResult result in results)
            {
                hitlist += GetTuInformation(result);
            }  
            #endregion

            #region "CloseHitlist"
            return hitlist;
            #endregion
        }
        #endregion

        #region "TuInfo"
        /// <summary>
        /// This function returns further information on the given translation unit (TU).
        /// </summary>
        private string GetTuInformation(SearchResult tuResult)
        {
            string tuInfo;

            #region "score"
            /// The matching score
            tuInfo = "\nScore: " + tuResult.ScoringResult.Match.ToString() + "%\n";
            #endregion

            #region "segments"
            /// The source and target segments
            tuInfo += "Source: " + tuResult.MemoryTranslationUnit.SourceSegment.ToPlain() + "\n";
            tuInfo += "Target: " + tuResult.MemoryTranslationUnit.TargetSegment.ToPlain() + "\n";
            #endregion

            #region "date"
            /// The TU creation date.
            tuInfo += "Creation date: " + tuResult.MemoryTranslationUnit.SystemFields.CreationDate + "\n";
            #endregion

            #region "fields"
            /// Any field values (e.g. Customer, Project id, etc. associated with the
            /// given TU.
            foreach (FieldValue field in tuResult.MemoryTranslationUnit.FieldValues)
            {
                tuInfo += field.Name + ": " + field.ToString();
            }
            tuInfo += "\n";
            #endregion

            return tuInfo;
        }   
        #endregion

        #region "settings"
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
        #endregion
    }
}
```
***