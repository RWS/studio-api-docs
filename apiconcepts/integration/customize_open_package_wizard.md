# Customize the *Open Package* wizard 

Var:ProductName Integration API provides support for third-party developers to trigger the __Open Project Package__ wizard from their own custom logic or actions and extend it's basic functionality by
- providing the physical location of the package
- providing a custom icon for the final imported project
- specifying the origin of the imported project
- injecting additional wizard pages at the beginning of the wizard
- injecting additional wizard pages before the final processing of the package
- injecting specific processing logic during the package processing phase

All above details are passed into an instance of [OpenProjectPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.OpenProjectPackageEvent.yml) and published via the [IStudioEventAggregator](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IStudioEventAggregator.yml). The exact purpose of each parameter is explained in the table bellow:

| Property        |  Description  |
| -------------  | -----|
| `packageFilePath` | Used to specify a path to the package to be opened.|
| `job` | Used to pass an [IExternalJobWithProgress](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJobWithProgress.yml) object to be executed once the package was loaded in Studio. Should be left `null` if no custom job needs to be executed. Details on how to create a custom execution job can be found [here](implementing_custom_job.md). |
| `iconPath` | Used to set an icon for your project. This allows you to customize the Projects View and have project icons specific to your plugin. |
| `projectOrigin` | Used to set a project type. This allows you to customize the Projects View and have the project type specific to your plugin. |
|`firstpages`| A collection of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) objects to be displayed at the start of the wizard. Details on how to implement a custom wizard page can be found [here](adding_custom_wizard_steps.md)|
|`lastpages`| A collection of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) objects to be displayed at the end of the wizard. Details on how to implement a custom wizard page can be found [here](adding_custom_wizard_steps.md). The pages will be inserted before the processing step in the wizard.|

Bellow you can see a sample of how a custom call to open the __Open Package__ wizard should look like.
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

See also: [Full sample application with source code](https://github.com/RWS/trados-studio-api-samples/tree/master/TranslationStudioAutomation/Sdl.CustomWizardSteps.Sample) (on GitHub).





