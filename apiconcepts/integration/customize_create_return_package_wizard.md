# Customize the *Create Return Package* wizard 

<Var:ProductName> Integration API provides support for third-party developers to trigger the __Create Return Package__ wizard from their own custom logic or actions and extend it's basic functionality by:
- specifying the project Guid for which a return package needs to be created
- specifying the project and the files that should be contained in the resulting return package
- specifying the project and all files associated to the given task that should be contained in the resulting return package
- injecting additional wizard pages at the beginning of the wizard
- injecting additional wizard pages before the final processing of the package
- injecting specific processing logic during the package processing phase

All above details are passed into an instance of [CreateReturnPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.CreateReturnPackageEvent.yml) and published via the [IStudioEventAggregator](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IStudioEventAggregator.yml). The exact purpose of each parameter is explained in the table bellow for all constructor options available. 

| Property        |  Description  |
| -------------  | -----|
| `projectId` | The project `Guid` which you can obtain from the [ProjectInfo](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml) class.|
| `job` | Used to pass an [IExternalJobWithProgress](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJobWithProgress.yml) object to be executed once the package was loaded in Studio. Should be left `null` if no custom job needs to be executed. Details on how to create a custom execution job can be found [here](implementing_custom_job.md). |
| `files` | Used to specify the language files that need to be included in the return package as a collection of `Guid`s. |
| `taskId` |Used to specify the task `Guid` that needs to be included in the return package. |
|`firstpages`| A collection of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) objects to be displayed at the start of the wizard. Details on how to implement a custom wizard page can be found [here](adding_custom_wizard_steps.md)|
|`lastpages`| A collection of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) objects to be displayed at the end of the wizard. Details on how to implement a custom wizard page can be found [here](adding_custom_wizard_steps.md). The pages will be inserted before the processing step in the wizard.|

Bellow you can see a sample of how a custom call to open the __Create Return Package__ wizard should look like.

```cs
var initialWizardSteps = new List<StudioWizardPage>
  {
    new FirstPage(),
    new SecondPage()
  };

//create the Job object
var publishJob = new SampleJob() { JobName = "Sample Job" };

//obtain the project Guid
var currentProject = SdlTradosStudio.Application.GetController<ProjectsController>().CurrentProject;
if (currentProject != null)
{
    var selectedProject = currentProject.GetProjectInfo().Id.ToString();
     _eventAggregator.Publish(
                    new CreateReturnPackageEvent(
                            projectId: selectedProject,
                            job: publishJob,
                            firstPages: initialWizardSteps,
                            lastPages: null));
}
```
