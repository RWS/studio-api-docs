Adding custom steps to wizards
====

<Var:ProductName> Integration API provides support for third-party developers to add custom pages to certain wizards within the <Var:ProductName> desktop application. Custom pages can be added using WPF views and view-models.

*For the moment, only the Open Package wizard supports this functionality, but we may extend it in the future to include other wizards as well.*

Creating pages to use as wizard steps
----
In order to create a view that can be used as a wizard step, a third-party developer needs to:

* Create a WPF view file, e.g. `UserControl`, with custom XAML and/or code behind
* You can also create a ViewModel for the view 
* Create a new class representing the custom page
    * This class needs to inherit from [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml)
    * Implement the required abstract properties
      * Set the `ViewType` property to the actual type of the WPF view file. It will be used to inject the view into the wizard's user interface by <Var:ProductName>'s infrastructure.
      * Set the `ViewModel` property. The wizard infrastructure will automatically assign it as DataContext to the view, so you can use `Binding` from the view to the ViewModel properties.

Notes:
* For the `ViewModel` you may choose whatever INotifyPropertyChanged implementation suits you best, such as providing your own or using an MVVM framework.
  
Integrating pages as steps into <Var:ProductName> wizards
----
Custom pages are injected into the wizard via the `firstPages` argument of the event object corresponding to the respective wizard. Keep in mind that the `firstPages` arguemnt is available only for some wizard event objects.
 
To inject the custom pages as steps into a <Var:ProductName> wizard, the third-party developer will require the following steps:

* Determine or define a place in your code where the wizard (with custom steps) would need to be launched ; for example, from an [Action](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractAction.yml)'s `Execute` method.
* Determine the appropriate event (e.g. [OpenProjectPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.OpenProjectPackageEvent.yml)) to be raised, that corresponds to the wizard you want to open
* Set up a `List` of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) instances indicating the custom pages to be inserted
* Create the event passing in `firstPages` list as an argument
* Use the event aggregator to raise the event. This will launch the wizard.

Notes:
* The custom pages will appear in the wizard at the beginning, before any other standard pages.
* The custom pages will appear in the wizard in the same order they are defined in the list configured in the event object.

Wizard data
----
You can define custom data for your pages using the `Data` property on the [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) object.

The Data property is a `Dictionary<string,object>` that can be used across pages. Once a dictionary key is set from a page, it will be available to any pages that are displayed after it. This includes pages that are navigated to via the Back button. You can use the Page's `OnShow` method to query for the most recent key values that may have been updated/inserted by previously shown pages.

Open package wizard specifics
----
If you provide your own custom package converter, the wizard `Data` object will be available to the custom converter as well, via the `ExternalPackageConversionInfo.CustomData` property. This way you can use wizard pages to make configurations to your converter if needed.

See also: [full sample application with source code](https://github.com/RWS/trados-studio-api-samples/tree/master/TranslationStudioAutomation/Sdl.CustomWizardSteps.Sample) (on GitHub).

