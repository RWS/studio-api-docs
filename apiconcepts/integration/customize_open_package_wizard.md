# Customize the Open Package wizard

The Var:ProductName Integration API enables you to launch the **Open Package** wizard from custom code or actions and extend its functionality by:

- Specifying the physical location of the package
- Providing a custom icon for the imported project
- Specifying the project type/origin
- Injecting custom wizard pages at the beginning of the wizard
- Injecting custom wizard pages before the final processing step
- Adding custom processing logic during package processing

All these options are passed through an [OpenProjectPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.OpenProjectPackageEvent.yml) instance and published via the [IStudioEventAggregator](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IStudioEventAggregator.yml). The following table explains each parameter:

| Property | Description |
| --- | --- |
| `packageFilePath` | Path to the package to open. |
| `job` | An [IExternalJobWithProgress](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJobWithProgress.yml) object to execute after the package loads in Studio. Set to `null` if no custom job is needed. See [implementing custom jobs](implementing_custom_job.md) for details. |
| `iconPath` | Icon path to customize the project appearance in the Projects View. |
| `projectOrigin` | Project type to differentiate projects in the Projects View. |
| `firstPages` | A collection of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) objects to display at the wizard start. See [adding custom wizard steps](adding_custom_wizard_steps.md) for details. |
| `lastPages` | A collection of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) objects to display before the processing step. See [adding custom wizard steps](adding_custom_wizard_steps.md) for details. |

## Example

The following code sample shows how to launch the **Open Package** wizard:
```cs
var initialWizardSteps = new List<StudioWizardPage>
  {
    new FirstPage(),
    new SecondPage()
  };

var finalWizardSteps = new List<StudioWizardPage>
  {
    new LastPage()
  };

_eventAggregator.Publish(
            new OpenProjectPackageEvent(
                    packageFilePath: filePath, 
                    job: null, 
                    iconPath: null, 
                    projectOrigin: null,
                    firstPages: initialWizardSteps,
                    lastPages: finalWizardSteps));
```

## See also

[Full sample application with source code](https://github.com/RWS/trados-studio-api-samples/tree/master/TranslationStudioAutomation/Sdl.CustomWizardSteps.Sample) (on GitHub)





