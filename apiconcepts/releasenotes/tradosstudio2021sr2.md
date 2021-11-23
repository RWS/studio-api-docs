Release Notes for <Var:ProductNameWithEdition> SR2
===================


## Project Automation API


* You can now check-in newly added files using the  [UploadAndCheckinFiles()]() method in the new [SeverBasedProjectOperations]() class.  \
 Example:
    ```cs
        
    var _sdlProject = new FileBasedProject(sdlProjectPath, gsUseWindowsAuthentication, gsUser, gsPassword);
    var tmp = _sdlProject.AddFiles(new[] {"d:\\temp2.txt"})[0];

    var serverBasedProjectOperations = new ServerBasedProjectOperations( _sdlProject);
    serverBasedProjectOperations .UploadAndCheckinFiles(new [] {tmp.Id}, "Temp", (sender, args) => { });
    ```


* Added CRUD operations for project reports in the [ProjectReportsOperations]() class:  
  - Added new [AddReport]() method that allows you to add a custom report to a file based project. \
  Example:
    ```cs    
    var addedReport = new ProjectReportsOperations(fileBasedProject)
    .AddReport("Sdl.ProjectApi.AutomaticTasks.Translate", "Name", "Description", "de-De", "<xml></xml>");
    ```

  - Added new [GetProjectReports]() method that exposes the list of reports for a file based project. \
    Example: 
    ```cs
    var reports = new ProjectReportOperations(fileBasedProject).GetProjectReports();
    ```
  - Added new [UpdateReport]() method that allows you to change the name, description and content for a file based project. \
  Example:
    ```cs
    new ProjectReportsOperations(fileBasedProject).UpdateReport("aa84193b-fd88-439c-8293-4ad0f9cfa8ec", "Name", "Description", "<xml></xml>");
    ```
  - Added new [RemoveReports]() method that allows you to delete the list of reports for a file based project. \
  Example:
    ```cs
    new ReportProjectOperations(fileBasedProject).RemoveReports(reports.Select(r => Guid.Parse(r.Id)).ToList());
    ```


* You can now use the [Save()]() method added to the [IProject](../../api/projectautomation/Sdl.ProjectAutomation.Core.IProject.yml) interface to avoid casting to [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.yml#filebasedproject)  
Example: \
    You can use 
    ```cs
    SelectedProject.Save()
    ```
    instead of 

    ```cs
    if (SelectedProject is FileBasedProject fileBasedProject)
    {
    fileBasedProject.Save();
    }
    ```


## Translation Memory API

* Added a new [ISupportPlaceables]() interface to the existing [ITranslationMemory]() and [ILanguageResourceTemplates]()  interfaces. \
 `ISupportPlaceables` contains two properties, [LanguageResourceBundleCollection]() and [BuiltInRecognizers]() and is implemented by probiders that support placeables. \
 Example:
 ```cs
 public class CustomProvider : ISupportPlaceables

var provider = new CustomProvider();

BuiltinRecognizers recognizers = provider.Recognizers;

LanguageResourceBundleCollection bundles = provider.LanguageResourceBundles
 ```

## Licensing App Support

* The [ProductLicenseManager.Initialize()]() method has a new overload which takes an [ILoggerFactory]() as an extra parameter. This new parameter allows you to inject logging mechanisms into the API.

## Files Controller

* Added a new property [AreAllSelectedTaskFilesAssignedToCurrentUser]() which returns true if all the selected files are assigned to the current user.

## IStudioDocument

* Added new [IsDirty]() parameter that returns true if the document has unsaved changes.

## Desktop.Extensions 

* Added new extension points. For more information see [Trados Studio Command line processor]()