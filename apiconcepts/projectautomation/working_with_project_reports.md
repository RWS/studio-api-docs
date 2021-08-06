Working with Project Reports
==

In Trados Studio, the project files have associated a set of predefined reports. The built-in reports are visible in the Reports Viewer. Trados Studio Reports API allows the 3rd party developers to extend and customize the existing set of reports and display them within an additional Trados Studio Viewer using a custom developed plugin. 

This chapter contains an example of how to add reports to a project programmatically, modify and delete the existing reports and render the reports using Studio predefined templates or custom templates.

Getting the Reports list
--

To enumerate the reports associated to a project, you need to create a ```ProjectReportsOperation``` object based on the current project and call ```GetProjectReports``` method.

# [C#](#tab/tabid-1)
```CS
var reports = new ProjectReportsOperations(fileBasedProject).GetProjectReports();
```
***

The method returns a list with report objects.
The report object is used to handle the reports. It contains the report Id, the name, the description, the task template Id, the ISO abbreviation for language, the physical path of the report file, the group, the creation date, the ```isCustomReport``` flag specifying if the report is custom defined or built-in. 

Get the Report definition
--

To get the report definition, you need to create a ```ProjectReportsOperation``` object based on the current project and call ```GetReportDefinition``` method. This method takes as input parameters the task template Id.

# [C#](#tab/tabid-2)
```CS
var reportDefinition = new ProjectReportsOperations(fileBasedProject).GetReportDefinition("Sdl.ProjectApi.AutomaticTasks.Translate");
```
***

The method returns the report definition object.
The report definition contains the task template id, the report Uri, the assembly and the report data. 

Adding a Report
--

To add a report to a project, you need to create a ```ProjectReportsOperation``` object based on the current project and call ```AddReport``` method. This method takes as input parameters the task template Id, the report name, description language and content.

# [C#](#tab/tabid-3)
```CS
var addedReport = new ProjectReportsOperations(fileBasedProject)
.AddReport("Sdl.ProjectApi.AutomaticTasks.Translate", "Name", "Description", "de-De", "<xml></xml>");
```
***

The method returns the report object.
The method will throw an exception if the task template Id does not exists or the report content is not in xml format.

Updating a Report
--

To update a report to a project, you need to create a ```ProjectReportsOperation``` object based on the current project and call ```UpdateReport``` method. This method takes as input parameters the task template, the report name, description and content.

# [C#](#tab/tabid-4)
```CS
new ProjectReportsOperations(fileBasedProject).UpdateReport("aa84193b-fd88-439c-8293-4ad0f9cfa8ec", "Name", "Description", "<xml></xml>");
```
***

The method will throw an exception if the report Id does not exists or the content is not in xml format.

Deleting Reports
--

To delete one or more reports from a project, you need to create a ```ProjectReportsOperation``` object based on the current project and call ```RemoveReports``` method. This method takes as input parameters the list with report Ids to be deleted.

# [C#](#tab/tabid-5)
```CS
new ProjectReportsOperations(fileBasedProject).RemoveReports(reports.Select(r => Guid.Parse(r.Id)).ToList());
```
***

Get the Report Rendering Supported File Formats
--

To get the supported file formats for rendering a report, you need to create a ```ProjectReportsOperation``` object based on the current project and call ```GetReportRenderingSupportedFileFormats``` method. This method takes as input parameter the report Id.

# [C#](#tab/tabid-6)
```CS
var reportDefinition = new ProjectReportsOperations(fileBasedProject).GetReportRenderingSupportedFileFormats("aa84193b-fd88-439c-8293-4ad0f9cfa8ec");
```
***

The method returns a list with file extensions supported by the rendering engine, such as “xml”, “html”.
The method will throw an exception if the task template Id does not exists.

Get the Report Rendering
--

To render a report, you need to create a ```ProjectReportsOperation``` object based on the current project and call one of the 2 ```GetReportRendering``` overloaded methods. 
The first method takes as input parameters the report Id and the output file format.

# [C#](#tab/tabid-7)
```CS
var reportDefinition = new ProjectReportsOperations(fileBasedProject).GetReportRendering("aa84193b-fd88-439c-8293-4ad0f9cfa8ec", "html");
```
***

The method returns a byte array with the report content in the specified format.
The method will throw an exception if the report Id does not exists or the output format is not supported.

The second method takes as input parameters the report Id, the path to the custom template and the output file format.

# [C#](#tab/tabid-8)
```CS
var reportDefinition = new ProjectReportsOperations(fileBasedProject).GetReportRendering("aa84193b-fd88-439c-8293-4ad0f9cfa8ec", "C:\ReportTemplates\CustomTemplate.xsl", "html");
```
***

The method returns a byte array with the report content transformed in the specified format using the custom template.
The method will throw an exception if the report Id does not exists or the output format is not supported.
