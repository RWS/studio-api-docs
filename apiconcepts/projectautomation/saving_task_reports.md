Saving Task Reports
==

Executing certain tasks automatically creates reports in the background. These reports are saved in a *Reports* sub-folder within the project folder structure. The report files are in XML format (e.g. *Analyze Files en-US_de-DE.xml*). For an analysis task a report contains the number of segments, words, characters, tags, and placeables that were found within the different TM match categories, i.e. context, exact, fuzzy matches, etc.

For task such as scan, convert to translatable format, copy target language files, no reports are generated, as no statistical information is generated through those tasks.

The reports that are associated with a specific task can be saved to a more readable file. Currently the following output formats can be generated through the API, which are useful for printing or for importing into an accounting system (e.g. XML):

* Microsoft Excel
* HTML
* MHT
* XML

After executing a task such as an analysis you can access the reports of this task through the [Reports](../../api/projectautomation/Sdl.ProjectAutomation.Core.AutomaticTask.yml#Sdl_ProjectAutomation_Core_AutomaticTask_Reports) property. A task can have more than one report if it has been executed several times. For example, the analysis task can be run several times to get updated analysis statistics. This is why in a project you may find several analyze files reports, which, however, can be deleted by the user if there are too many of them.

To save a report to a file, you apply the [SaveTaskReportAs](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_SaveTaskReportAs_System_Guid_System_String_Sdl_ProjectAutomation_Core_ReportFormat_) method to the project. This method requires the following parameters:

* The id (guid) of the report to save
* The file name and path
* The report format; the available report formats can be accessed through the [ReportFormat](../../api/projectautomation/Sdl.ProjectAutomation.Core.ReportFormat.yml) class

The example below demonstrates how you can generate a report in Microsoft Excel (e.g. for printing) with just two lines of code. First, we retrieve the id of the first report that is available for an analyze file task, then we apply the [SaveTaskReportAs](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_SaveTaskReportAs_System_Guid_System_String_Sdl_ProjectAutomation_Core_ReportFormat_) method to generate the Excel file

# [C#](#tab/tabid-1)
```CS
Guid reportId = analyzeTask.Reports[0].Id;
project.SaveTaskReportAs(reportId, @"C:\ProjectFiles\Analysis_report.xls", ReportFormat.Excel);
```
***

See Also
--
[Running Tasks on the Project Files](running_tasks_on_project_files.md)

[Generating the Task Report](generating_the_task_report.md)
