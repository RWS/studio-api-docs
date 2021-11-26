Release Notes for <Var:ProductNameWithEdition> SR2
===================


## Project Automation API


* You can now check-in newly added files using the  [UploadAndCheckinFiles()](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.ServerOperations.ServerBasedProjectOperations.yml#Sdl_ProjectAutomation_FileBased_ServerOperations_ServerBasedProjectOperations_UploadAndCheckinFiles_System_Guid___System_String_System_EventHandler_Sdl_ProjectAutomation_Core_ProgressEventArgs__) method in the new [SeverBasedProjectOperations](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.ServerOperations.ServerBasedProjectOperations.yml) class.  \
 Example:
    ```cs
        
    var _sdlProject = new FileBasedProject(sdlProjectPath, gsUseWindowsAuthentication, gsUser, gsPassword);
    var tmp = _sdlProject.AddFiles(new[] {"d:\\temp2.txt"})[0];

    var serverBasedProjectOperations = new ServerBasedProjectOperations( _sdlProject);
    serverBasedProjectOperations .UploadAndCheckinFiles(new [] {tmp.Id}, "Temp", (sender, args) => { });
    ```


* Added CRUD operations for project reports in the [ProjectReportsOperations](api/projectautomation/Sdl.ProjectAutomation.FileBased.Reports.Operations.ProjectReportsOperations.yml) class:  
  - Added new [AddReport()](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.Reports.Operations.ProjectReportsOperations.yml#Sdl_ProjectAutomation_FileBased_Reports_Operations_ProjectReportsOperations_AddReport_System_String_System_String_System_String_System_String_System_String_) method that allows you to add a custom report to a file based project. \
  Example:
    ```cs    
    var addedReport = new ProjectReportsOperations(fileBasedProject)
    .AddReport("Sdl.ProjectApi.AutomaticTasks.Translate", "Name", "Description", "de-De", "<xml></xml>");
    ```

  - Added new [GetProjectReports()](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.Reports.Operations.ProjectReportsOperations.yml#Sdl_ProjectAutomation_FileBased_Reports_Operations_ProjectReportsOperations_AddReport_System_String_System_String_System_String_System_String_System_String_) method that exposes the list of reports for a file based project. \
    Example: 
    ```cs
    var reports = new ProjectReportOperations(fileBasedProject).GetProjectReports();
    ```
  - Added new [UpdateReport()](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.Reports.Operations.ProjectReportsOperations.yml#Sdl_ProjectAutomation_FileBased_Reports_Operations_ProjectReportsOperations_UpdateReport_System_Guid_System_String_System_String_System_String_) method that allows you to change the name, description and content for a file based project. \
  Example:
    ```cs
    new ProjectReportsOperations(fileBasedProject).UpdateReport("aa84193b-fd88-439c-8293-4ad0f9cfa8ec", "Name", "Description", "<xml></xml>");
    ```
  - Added new [RemoveReports()](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.Reports.Operations.ProjectReportsOperations.yml#Sdl_ProjectAutomation_FileBased_Reports_Operations_ProjectReportsOperations_RemoveReports_System_Collections_Generic_List_System_Guid__) method that allows you to delete the list of reports for a file based project. \
  Example:
    ```cs
    new ReportProjectOperations(fileBasedProject).RemoveReports(reports.Select(r => Guid.Parse(r.Id)).ToList());
    ```


* You can now use the [Save()](../../api/projectautomation/Sdl.ProjectAutomation.Core.IProject.yml#Sdl_ProjectAutomation_Core_IProject_Save) method added to the [IProject](../../api/projectautomation/Sdl.ProjectAutomation.Core.IProject.yml) interface to avoid casting to [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.yml#filebasedproject)  
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

* Added a new [ISupportPlaceables](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ISupportPlaceables.yml) interface to the existing [ITranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ISupportPlaceables.yml) and [ILanguageResourceTemplates](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ILanguageResourcesTemplate.yml)  interfaces. \
 `ISupportPlaceables` contains two properties, [LanguageResourceBundleCollection](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ISupportPlaceables.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ISupportPlaceables_LanguageResourceBundles) and [BuiltInRecognizers](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ISupportPlaceables.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ISupportPlaceables_Recognizers) and is implemented by providers that support placeables. \
 Example:
 ```cs
 public class CustomProvider : ISupportPlaceables

var provider = new CustomProvider();

BuiltinRecognizers recognizers = provider.Recognizers;

LanguageResourceBundleCollection bundles = provider.LanguageResourceBundles
 ```

## Licensing App Support

* The [ProductLicenseManager.Initialize()](../../api/integration/Sdl.Common.Licensing.AppSupport.ProductLicenseManager.yml#Sdl_Common_Licensing_AppSupport_ProductLicenseManager_Initialize_Sdl_Common_Licensing_AppSupport_Product_Microsoft_Extensions_Logging_ILoggerFactory_) method has a new overload which takes an [ILoggerFactory](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory?view=dotnet-plat-ext-5.0) as an extra parameter. This new parameter allows you to inject logging mechanisms into the API. \
  
  For more information see the [Microsoft.Extensions.Logging.Abstraction](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.abstractions?view=dotnet-plat-ext-5.0) namespace. \

  You can get the NuGet package [here](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions/) or get clients already available as NuGet packages [here](https://www.nuget.org/packages/NLog.Extensions.Logging/5.0.0-preview.1).

## Files Controller

* Added a new property [AreAllSelectedTaskFilesAssignedToCurrentUser](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.FilesController.yml#Sdl_TranslationStudioAutomation_IntegrationApi_FilesController_AreAllSelectedTaskFilesAssignedToCurrentUser) which returns true if all the selected files are assigned to the current user.

## IStudioDocument

* Added new [IsDirty](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml#Sdl_TranslationStudioAutomation_IntegrationApi_IStudioDocument_IsDirty) parameter that returns true if the document has unsaved changes.

# WPF Styles API

* Trados Studio now exposes its own custom progress ring with an RWS look and feel. As a result, the `Sdl.ProgressRing.Asterisk` style has been removed for the MahApps Metro ProgressRing control. 

## Desktop.Extensions 

* Added new extension points. For more information see [Trados Studio Command line processor](../../apiconcepts/integration/create_a_trados_studio_commandLineProcessor.md).
  
## LanguageCloud.IdentityApi

* The `APIKey` has been marked obsolete since it is no longer used when logging in to Language Cloud from Trados Studio.