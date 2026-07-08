# Customize the Create Return Package wizard

The Var:ProductName Integration API enables you to launch the **Create Return Package** wizard from custom code or actions and extend its functionality by:

- Specifying the project GUID for creating a return package
- Selecting which project files to include in the return package
- Selecting a task and all associated files to include in the return package
- Injecting custom wizard pages at the beginning of the wizard
- Injecting custom wizard pages before the final processing step
- Adding custom processing logic during package creation

All these options are passed through a [CreateReturnPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.CreateReturnPackageEvent.yml) instance and published via the [IStudioEventAggregator](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IStudioEventAggregator.yml). The following table explains each constructor parameter:

| Property        |  Description  |
| -------------  | -----|
| `projectId` | The project `Guid` from the [ProjectInfo](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml) class. |
| `job` | An [IExternalJobWithProgress](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJobWithProgress.yml) object to execute after the package loads in Studio. Set to `null` if no custom job is needed. See [implementing custom jobs](implementing_custom_job.md) for details. |
| `files` | A collection of `Guid` values specifying which language files to include in the return package. |
| `taskId` | The task `Guid` to include in the return package. |
| `firstPages` | A collection of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) objects to display at the wizard start. See [adding custom wizard steps](adding_custom_wizard_steps.md) for details. |
| `lastPages` | A collection of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) objects to display before the processing step. See [adding custom wizard steps](adding_custom_wizard_steps.md) for details. |

## Example

The following code sample shows how to launch the **Create Return Package** wizard:

```cs
var initialWizardSteps = new List<StudioWizardPage>
  {
    new FirstPage(),
    new SecondPage()
  };

// Create the Job object
var publishJob = new SampleJob() { JobName = "Sample Job" };

// Get the project Guid
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