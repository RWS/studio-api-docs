Retrieving the Project Statistics
==

During a project lifecycle the segments contained in the project files typically go through a set of states:

* Not Translated
* Draft
* Translated
* Reviewed
* Translation rejected
* Signed-off
* Sign-off rejected
* The total number of translated and untranslated segments is used to compute the statistics for the project. That way project managers can ascertain the project status at a glance. Using the number of translated and untranslated words project managers can quickly give an estimate of whether the project is on track or not.

Retrieve the Project Statistics Programmatically
--

Below you see an example of how <Var:ProductName> uses bar charts to visualize the project statistics:

![Statistics](images/Statistics.jpg)

Below you will find an example of how to retrieve basic statistical information on a project. Start by implementing a helper function called ```GetProjectStatistics```, which takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object as parameter. In order to retrieve the project statistics, apply the [GetProjectStatistics](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetProjectStatistics) method to the project:

# [C#](#tab/tabid-1)
```cs
public void GetProjectStatistics(FileBasedProject project)
{
    #region "GetProjectStatistics"
    ProjectStatistics projStats = project.GetProjectStatistics();
    #endregion

    #region "TargetLanguageStatistics"
    TargetLanguageStatistics[] targetStats = projStats.TargetLanguageStatistics;
    #endregion

    #region "trgInfo"
    StringBuilder trgInfo = new StringBuilder();
    for (int i = 0; i < targetStats.Length; i++)
    {
        ConfirmationStatistics confirmationStats = targetStats[i].ConfirmationStatistics;

        Language trgLang = new Language(CultureInfo.GetCultureInfo("de-DE"));
        Guid[] ids = project.GetTargetLanguageFiles(trgLang).GetIds();
        project.RunAutomaticTask(ids, AutomaticTaskTemplateIds.TranslationCount);

        trgInfo.Append("\nConfirmation statistics for: " + targetStats[i].TargetLanguage.DisplayName + "\n");
        trgInfo.Append("Total word count: " + confirmationStats.Total.Words.ToString() + "\n");
        trgInfo.Append("Words with untranslated status: " + confirmationStats[ConfirmationLevel.Unspecified].Words.ToString() + "\n");
        trgInfo.Append("Words with draft status: " + confirmationStats[ConfirmationLevel.Draft].Words.ToString() + "\n");
        trgInfo.Append("Words with translated status: " + confirmationStats[ConfirmationLevel.Translated].Words.ToString() + "\n");
    }

    MessageBox.Show(trgInfo.ToString());
    #endregion
}
```
***

By doing so, you can retrieve the statistics for the whole project. Usually, you will be interested in the statistics for particular target languages in order to asertain the progress for a specific target language, i.e. you will want to find out the percentage of words already processed e.g. through pre-translation or interactive translation by human translators. The first step is to use the [TargetLanguageStatistics](../../api/projectautomation/Sdl.ProjectAutomation.Core.TargetLanguageStatistics.yml) class to derive a target language statistics object from the project statistics as shown below:

# [C#](#tab/tabid-2)
```cs
TargetLanguageStatistics[] targetStats = projStats.TargetLanguageStatistics;
```
***

Note that since we assume that our sample project has multiple target languages, we create an array of target language statistics objects.
In the next step we loop through the target language statistics objects and compile a string that outputs the total number of words as well as the percentage completed for each target language. Within the loop we derive a [ConfirmationStatistics](../../api/projectautomation/Sdl.ProjectAutomation.Core.ConfirmationStatistics.yml) object for each target language. This object holds, among other things, the total number of words that are in e.g. the untranslated, draft, translated, etc. state for the respective target language. You can specify the state for which you want to retrieve the word or segment count through the [CombinedConfirmationLevel](../../api/projectautomation/Sdl.ProjectAutomation.Core.CombinedConfirmationLevel.yml) class. The example below retrieves the following information:

* The total word count per state.
* The number of words in untranslated (unspecified) state.
* The number of words in draft state. Draft segments are usually created during execution of the pre-translate task if a fuzzy match has been found and is inserted into the document.
* The number of words in translated status. During pre-translation exact and context matches are usually inserted as translated (confirmed) segments.

The information on all other states (e.g. reviewed, signed-off) can be retrieved in the same way.

# [C#](#tab/tabid-3)
```cs
StringBuilder trgInfo = new StringBuilder();
for (int i = 0; i < targetStats.Length; i++)
{
    ConfirmationStatistics confirmationStats = targetStats[i].ConfirmationStatistics;

    Language trgLang = new Language(CultureInfo.GetCultureInfo("de-DE"));
    Guid[] ids = project.GetTargetLanguageFiles(trgLang).GetIds();
    project.RunAutomaticTask(ids, AutomaticTaskTemplateIds.TranslationCount);

    trgInfo.Append("\nConfirmation statistics for: " + targetStats[i].TargetLanguage.DisplayName + "\n");
    trgInfo.Append("Total word count: " + confirmationStats.Total.Words.ToString() + "\n");
    trgInfo.Append("Words with untranslated status: " + confirmationStats[ConfirmationLevel.Unspecified].Words.ToString() + "\n");
    trgInfo.Append("Words with draft status: " + confirmationStats[ConfirmationLevel.Draft].Words.ToString() + "\n");
    trgInfo.Append("Words with translated status: " + confirmationStats[ConfirmationLevel.Translated].Words.ToString() + "\n");
}

MessageBox.Show(trgInfo.ToString());
```
***

The above is just an example of what kind of statistical information you can retrieve for a project and its target languages. Another example, which shows how to retrieve detailed analysis information can be found in the chapters [Generating the Task Report](generating_the_task_report.md).

Putting it All Together
--

The complete function should now look as shown below:

# [C#](#tab/tabid-4)
```cs
public void GetProjectStatistics(FileBasedProject project)
{
    #region "GetProjectStatistics"
    ProjectStatistics projStats = project.GetProjectStatistics();
    #endregion

    #region "TargetLanguageStatistics"
    TargetLanguageStatistics[] targetStats = projStats.TargetLanguageStatistics;
    #endregion

    #region "trgInfo"
    StringBuilder trgInfo = new StringBuilder();
    for (int i = 0; i < targetStats.Length; i++)
    {
        ConfirmationStatistics confirmationStats = targetStats[i].ConfirmationStatistics;

        Language trgLang = new Language(CultureInfo.GetCultureInfo("de-DE"));
        Guid[] ids = project.GetTargetLanguageFiles(trgLang).GetIds();
        project.RunAutomaticTask(ids, AutomaticTaskTemplateIds.TranslationCount);

        trgInfo.Append("\nConfirmation statistics for: " + targetStats[i].TargetLanguage.DisplayName + "\n");
        trgInfo.Append("Total word count: " + confirmationStats.Total.Words.ToString() + "\n");
        trgInfo.Append("Words with untranslated status: " + confirmationStats[ConfirmationLevel.Unspecified].Words.ToString() + "\n");
        trgInfo.Append("Words with draft status: " + confirmationStats[ConfirmationLevel.Draft].Words.ToString() + "\n");
        trgInfo.Append("Words with translated status: " + confirmationStats[ConfirmationLevel.Translated].Words.ToString() + "\n");
    }

    MessageBox.Show(trgInfo.ToString());
    #endregion
}
```
***

See Also
--
[Generating the Task Report](generating_the_task_report.md)

[About Project Files](about_project_files.md)