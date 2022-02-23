Adding custom steps to wizards
====

<Var:ProductName> Integration API provides support for third-party developers to integrate custom WPF views inside certain wizards within the <Var:ProductName> desktop applications.

* Note: Currently, support is limited to the Open Package wizard, but we head to extending it in the future.

Creating pages to use as wizard steps
----
In order to create a view that can be used as a wizard step, a third-party developer needs to:

* Create a WPF view file, e.g. `UserControl`, with custom XAML and/or code behind
* Also create a view model counterpart that inherits [StudioWizardPageViewModel](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPageViewModel.yml)
    * Its `ViewType` property of the page definition should indicate the actual type of the WPF view file. It will be used to eventually inject it into the wizard's user interface by <Var:ProductName>'s infrastructure
  
Integrating pages as steps into <Var:ProductName> wizards
----
To inject the custom pages as steps into a <Var:ProductName> wizard, the third-party developer will require the following steps:

* Determine or define a place in your code where the wizard (with custom steps) would need to be initiated; for example, under an [Action](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractAction.yml)'s `Execute` method.
* Determine the appropriate event (e.g. [OpenProjectPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.OpenProjectPackageEvent.yml)) to be raised in order to start the wizard
* Set up a `List` of [StudioWizardPageViewModel](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractAction.yml) instances indicating the pages to be loaded as initial steps within the wizard, i.e. the `firstPages`
* Create and raise an event instance passing `firstPages` list as an argument

See also: [full sample application with source code](https://github.com/RWS/trados-studio-api-samples/tree/master/TranslationStudioAutomation/Sdl.CustomWizardSteps.Sample) (on GitHub).
