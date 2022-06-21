Release Notes for <Var:ProductNameWithEdition>
===================

# Groupshare 2015 Integration
As of <Var:ProductNameWithEdition> the integration with Groupshare 2015 is EOL. Therefore the our public APIs no longer offer the support and any code references have been removed.

# Project Automation API

### New properties on [ProjectInfo](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml)
We expanded the [ProjectInfo](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml) class to include information regarding the current user associated to the project.
The `CurrentUserID` Property is identical to the user under which the API is running locally. For server-based projects this represents the ID of the user that is currently logged on to the server.

    Example: 
    ```cs
    var projectInfo = filesController.CurrentProject.GetProjectInfo();

    Console.WriteLine(projectInfo.CurrentUserId);
    ```
### Studio supported extensions    
Added the [CommonFileExtensions](../../api/projectautomation/Sdl.ProjectAutomation.Core.CommonFileExtensions.yml) class which returns constants for Studio's most common file extensions.


# Integration API

### Extended [FilesController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.FilesController.yml)
The [FilesController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.FilesController.yml) class now exposes a property called `SelectedTasks` which returns the list of tasks currently selected by the user on the UI.

This allows the developers to interact with the selected tasks and not just the selected files on the Files View.

Example:

    ```cs
    var filesController = SdlTradosStudio.Application.GetController<FilesController>();
    var noTasksSelected = filesController.SelectedTasks.Count() == 0;
    ```
### Package conversion support
The Integration API introduces the concept of external package converter. This allows you to create package converters for integrations between different systems, for example WorldServer and TMS. 
    
Along with the external package extension, the `IProject` interface now can now return the [ProjectFileTypeConfiguration](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectFileTypeConfiguration.yml) of that project.
    
For more information see [Extend default packaging functionality](../integration/extend_standard_packaging_support.md).

### Better control through [IExternalJob](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJob.yml)
The `Execute()` method of the [IExternalJob](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJob.yml) interface now receives an `IexternalJobExecutionContext` which allows you to see if the job was cancelled by the user or report the progress.
    
    Example:
    ```cs
    public class SampleJob : IExternalJobWithProgress
    {
       /// <summary>
       ///
       /// </summary>
       /// <param name="jobData"></param>
       public SampleJob(){ }
       
       /// <summary>
       ///
       /// </summary>
       public string JobName { get; set; }
       IDictionary<string, object> IExternalJob.JobData { get; set; }
 
       public event EventHandler<JobProgressArgs> ProgressReported;
 
       public void Execute(IExternalJobExecutionContext externalExecutionContext)
       {
           externalExecutionContext.ReportProgress(0, "Sample Job Started for package at location: ");
 
           System.Threading.Thread.Sleep(5000);
           if (externalExecutionContext.CancelRequested)
           {
               externalExecutionContext.ReportProgress(0, "Job cancelled by user " );
               return;
           }
           ProgressReported?.Invoke(this, new JobProgressArgs()
           {
               PercentComplete = 50,
               StatusMessage = "Sample Job Processing for package at location: "
           });
           System.Threading.Thread.Sleep(5000);
           externalExecutionContext.ReportProgress(100, "Sample Job Completed for package at location: " );
       }
 
       public void JobCanceled(object sender, EventArgs e) { }
    }
    ```

### Ability to inject custom wizard pages in the standard *Open Package* and *Create Return Package* wizards
We extended the [OpenProjectPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.OpenProjectPackageEvent.yml) and [CreateReturnPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.CreateReturnPackageEvent.yml) to allow you to inject wizard pages in the **Open Package** or **Create Return Package** wizards. The pages are added either as the first, or the last pages before processing the page.

    Example: 
    ```cs
    var customPages = new List<StudioWizardPage>
        {
            new WsSettingsWizardPage(settingsProvider, pageLogger),
        };
 
        var command = new OpenProjectPackageEvent(packageFilePath: packageFilePath,
                                job: null,
                                iconPath: string.Empty,
                                projectOrigin: WorldServerConstants.ProjectOriginWorldServer,
                                firstPages: customPages,
                                lastPages: null);
 
        var eventAggregatorService = SdlTradosStudio.Application.GetService<IStudioEventAggregator>();
        eventAggregatorService.Publish(command);
    ```
