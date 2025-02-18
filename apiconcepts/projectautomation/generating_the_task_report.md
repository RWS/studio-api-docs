Generating the Task Report
==

This chapter explains how to retrieve the actual task result, which is the analyze files report. There are two ways of generating a task report, an easy way, and a 'hard' way:

* You can simply apply apply the [SaveTaskReportAs](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_SaveTaskReportAs_System_Guid_System_String_Sdl_ProjectAutomation_Core_ReportFormat_) method to a project in order to generate a standard report in any of the following formats: Excel, HTML, MHT, or XML.
* You can also retrieve the task statistics bit-by-bit through various properties, e.g. the number of segments, words, and characters that fall into the exact match category. This allows you to generate a custom output, e.g. a custom XML file for importing into an accounting and billing system.

In this chapter examples for both approaches will be demonstrated.

Start by implementing a helper function called CreateReports. This function takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object and the report file path string as parameters. Note that the reports should be generated in the document main path. Creating a standard report is easy and can basically be done with a single line of code. All you need to do is apply the [SaveTaskReportAs](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_SaveTaskReportAs_System_Guid_System_String_Sdl_ProjectAutomation_Core_ReportFormat_) method to the project. This method requires the following parameters:

* The id of the report for which the report should be generated.
* The report file and location.
* The report format (i.e. Excel, HTML, MHT or XML).

Like project files all task reports are uniquely referenced through a guid. In order to be able to track the guid of the analyze report that was automatically generated in the backgrond when performing this task, we declare the following private variable in the class, i.e.:

# [C#](#tab/tabid-1)
```cs
private Guid reportId;
```
***

Then add the following line to the ```RunFileAnalysis``` function (see chapter [Analyzing the Files](analyzing_the_files.md)) to set the report id. Note that when executing a task, a report is automatically generated in the background without you having to explicitly generate it programmatically. The report is physically saved as an XML file, which is stored in a *Reports* sub-folder. The XML file itself is called, for example, *Analyze Files en-US_de-DE.xml*. We do this by applying the [Reports](../../api/projectautomation/Sdl.ProjectAutomation.Core.AutomaticTask.yml#Sdl_ProjectAutomation_Core_AutomaticTask_Reports) property to the analysis task object. Since this task has been executed only once, there is only one report available, therefore we select the id of the first report:

# [C#](#tab/tabid-2)
```cs
this.reportId = analyzeTask.Reports[0].Id;
```
***

Now you can provide all parameters required by the [SaveTaskReportAs](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_SaveTaskReportAs_System_Guid_System_String_Sdl_ProjectAutomation_Core_ReportFormat_) method. Note that the available report formats can be accessed through the [ReportFormat](../../api/projectautomation/Sdl.ProjectAutomation.Core.ReportFormat.yml) class:

# [C#](#tab/tabid-3)
```cs
project.SaveTaskReportAs(this.reportId, path + "/AnalyzeTaskReport.xls", ReportFormat.Excel);
```
***

Create the Custom Report Output
--

Creating a custom output is a bit more challenging. It requires us to retrieve the segment, word, character, etc. count through various properties and integrate this information in custom format, which in our implementation is a simple console output.

To do this, we need to take three steps. Start by creating an object based on the [ProjectStatistics](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectStatistics.yml) class, which contains the statistical information on the entire project. You create the project statistics object by applying the [GetProjectStatistics](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetProjectStatistics) method to your project. From the project statistics you derive the [TargetLanguageStatistics](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectStatistics.yml#Sdl_ProjectAutomation_Core_ProjectStatistics_TargetLanguageStatistics), i.e. the statistics for your target language. Note that since a project can have multiple target languages, the target language statistics are represented through an array of [TargetLanguageStatistics](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectStatistics.yml#Sdl_ProjectAutomation_Core_ProjectStatistics_TargetLanguageStatistics) objects. Since our project only involves one target language, we take the first object in the array to derive from it a [AnalysisStatistics](../../api/projectautomation/Sdl.ProjectAutomation.Core.AnalysisStatistics.yml) object as shown in the code example below. This analysis statistics object acts as a container for the segment, word, etc. count information of the first (and in this implementation only) target language in which the analysis task was performed:

# [C#](#tab/tabid-4)
```cs
ProjectStatistics projectStats = project.GetProjectStatistics();
TargetLanguageStatistics[] targetStats = projectStats.TargetLanguageStatistics;
AnalysisStatistics analysisStats = targetStats[0].AnalysisStatistics;
```
***

Now you can proceed to retrieving the count results for the various match categories, e.g.:
* Perfect matches
* Context matches
* Exact matches
* Fuzzy matches
* Repetitions
* New (unknown) segments
* The total segment/word/character count

For each match categories you can retrieve the number of:

* Segments
* Words
* Characters
* Tags
* Placeables (i.e. elements such as numbers, acronyms, dates.)

Below is an example of how to retrieve and output this information for the exact match category:

# [C#](#tab/tabid-5)
```cs
result += "\nExact matches\n";
result += "Segments: " + analysisStats.Exact.Segments.ToString() + "\n";
result += "Words: " + analysisStats.Exact.Words.ToString() + "\n";
result += "Characters: " + analysisStats.Exact.Characters.ToString() + "\n";
result += "Tags: " + analysisStats.Exact.Tags.ToString() + "\n";
result += "Placeables: " + analysisStats.Exact.Placeables.ToString() + "\n";
```
***

Outputting the analysis results for the fuzzy matches is somewhat more complex, as the fuzzy category is further subdivided into fuzzy bands. The default fuzzy band are as follows:
* 99%-95%
* 94%-85%
* 84%-75%
* 74%-50%

Note that Var:ProductName allows you to freely re-define these fuzzy bands, i.e. it is possible to change the range and to increase or decrease the number of fuzzy bands. This is why in the following sample code we loop through the available fuzzy bands and determine the [MaximumMatchValue](../../api/projectautomation/Sdl.ProjectAutomation.Core.AnalysisBand.yml#Sdl_ProjectAutomation_Core_AnalysisBand_MaximumMatchValue) and the [TranslationMinimumMatchValue](../../api/projectautomation/Sdl.ProjectAutomation.Settings.TranslationMemorySettings.yml#Sdl_ProjectAutomation_Settings_TranslationMemorySettings_TranslationMinimumMatchValue). After determining the fuzzy bands with their minimum and maximum match values we output the corresponding number of segments, words, and characters as shown below:

# [C#](#tab/tabid-6)
```cs
for (int i = 0; i < analysisStats.Fuzzy.Length; i++)
{
    string rangeMax = analysisStats.Fuzzy[i].Band.MaximumMatchValue.ToString();
    string rangeMin = analysisStats.Fuzzy[i].Band.MinimumMatchValue.ToString();

    result += "\nFuzzy matches " + rangeMax + "% to " + rangeMin + "%\n";
    result += "Segments: " + analysisStats.Fuzzy[i].Segments.ToString() + "\n";
    result += "Words: " + analysisStats.Fuzzy[i].Words.ToString() + "\n";
    result += "Characters: " + analysisStats.Fuzzy[i].Characters.ToString() + "\n";
    result += "Tags: " + analysisStats.Fuzzy[i].Tags.ToString() + "\n";
    result += "Placeables: " + analysisStats.Fuzzy[i].Placeables.ToString() + "\n";
}
```
***

For the full implementation see the section below:

Putting it All Together
--

The complete function for creating the reports should look as shown below:

# [C#](#tab/tabid-7)
```cs
/// <summary>
/// Retrieves the results of the analyze files tasks and generates a standard
/// report in Microsoft Excel format as well as a report in a custom XML format.
/// </summary> 
private void CreateReports(FileBasedProject project, string path)
{
    #region "CreateStandardReport"
    project.SaveTaskReportAs(this.reportId, path + "/AnalyzeTaskReport.xls", ReportFormat.Excel);
    #endregion

    #region "Statistics"
    ProjectStatistics projectStats = project.GetProjectStatistics();
    TargetLanguageStatistics[] targetStats = projectStats.TargetLanguageStatistics;
    AnalysisStatistics analysisStats = targetStats[0].AnalysisStatistics;
    #endregion

    #region "WriteData"
    string result = "Analysis result for all files:\n\n";

    result += "\nPerfect matches\n";
    result += "Segments: " + analysisStats.Perfect.Segments.ToString() + "\n";
    result += "Words: " + analysisStats.Perfect.Words.ToString() + "\n";
    result += "Characters: " + analysisStats.Perfect.Characters.ToString() + "\n";
    result += "Tags: " + analysisStats.Perfect.Tags.ToString() + "\n";
    result += "Placeables: " + analysisStats.Perfect.Placeables.ToString() + "\n";

    result += "\nContext matches\n";
    result += "Segments: " + analysisStats.InContextExact.Segments.ToString() + "\n";
    result += "Words: " + analysisStats.InContextExact.Words.ToString() + "\n";
    result += "Characters: " + analysisStats.InContextExact.Characters.ToString() + "\n";
    result += "Tags: " + analysisStats.InContextExact.Tags.ToString() + "\n";
    result += "Placeables: " + analysisStats.InContextExact.Placeables.ToString() + "\n";

    #region "Exact"
    result += "\nExact matches\n";
    result += "Segments: " + analysisStats.Exact.Segments.ToString() + "\n";
    result += "Words: " + analysisStats.Exact.Words.ToString() + "\n";
    result += "Characters: " + analysisStats.Exact.Characters.ToString() + "\n";
    result += "Tags: " + analysisStats.Exact.Tags.ToString() + "\n";
    result += "Placeables: " + analysisStats.Exact.Placeables.ToString() + "\n";
    #endregion

    #region "Fuzzy"
    for (int i = 0; i < analysisStats.Fuzzy.Length; i++)
    {
        string rangeMax = analysisStats.Fuzzy[i].Band.MaximumMatchValue.ToString();
        string rangeMin = analysisStats.Fuzzy[i].Band.MinimumMatchValue.ToString();

        result += "\nFuzzy matches " + rangeMax + "% to " + rangeMin + "%\n";
        result += "Segments: " + analysisStats.Fuzzy[i].Segments.ToString() + "\n";
        result += "Words: " + analysisStats.Fuzzy[i].Words.ToString() + "\n";
        result += "Characters: " + analysisStats.Fuzzy[i].Characters.ToString() + "\n";
        result += "Tags: " + analysisStats.Fuzzy[i].Tags.ToString() + "\n";
        result += "Placeables: " + analysisStats.Fuzzy[i].Placeables.ToString() + "\n";
    }
    #endregion

    result += "\nRepetitions\n";
    result += "Segments: " + analysisStats.Repetitions.Segments.ToString() + "\n";
    result += "Words: " + analysisStats.Repetitions.Words.ToString() + "\n";
    result += "Characters: " + analysisStats.Repetitions.Characters.ToString() + "\n";
    result += "Tags: " + analysisStats.Repetitions.Tags.ToString() + "\n";
    result += "Placeables: " + analysisStats.Repetitions.Placeables.ToString() + "\n";

    result += "\nNew segments\n";
    result += "Segments: " + analysisStats.New.Segments.ToString() + "\n";
    result += "Words: " + analysisStats.New.Words.ToString() + "\n";
    result += "Characters: " + analysisStats.New.Characters.ToString() + "\n";
    result += "Tags: " + analysisStats.New.Tags.ToString() + "\n";
    result += "Placeables: " + analysisStats.New.Placeables.ToString() + "\n";

    result += "\nTotal\n";
    result += "Segments: " + analysisStats.Total.Segments.ToString() + "\n";
    result += "Words: " + analysisStats.Total.Words.ToString() + "\n";
    result += "Characters: " + analysisStats.Total.Characters.ToString() + "\n";
    result += "Tags: " + analysisStats.Total.Tags.ToString() + "\n";
    result += "Placeables: " + analysisStats.Total.Placeables.ToString() + "\n";

    Console.Write(result);

    #endregion
}
```
***

See Also
--
[Retrieving the Project Statistics](retrieving_the_project_statistics.md)

[Saving Task Reports](saving_task_reports.md)

[About Project Files](about_project_files.md)
