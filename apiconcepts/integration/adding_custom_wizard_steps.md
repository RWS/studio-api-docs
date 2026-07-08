# Adding Custom Steps to Wizards

The Var:ProductName Integration API allows third-party developers to add custom pages to certain wizards in the Var:ProductName desktop application. You can add custom pages using WPF views and view-models.

> [!NOTE]
> 
> Currently, only the **Open Package wizard** supports this functionality. Support for other wizards may be added in the future.

## Creating Pages for Wizard Steps

To create a view that can be used as a wizard step, complete the following:

* Create a WPF view file (for example, a `UserControl`) with custom XAML and/or code behind
* Create a ViewModel for the view, if necessary
* Create a class that inherits from the abstract [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml)
  * Implement the required abstract properties

## StudioWizardPage Base Class

The [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) abstract class provides default implementations for some methods and properties required to define a custom wizard page. You can override these methods and properties to provide custom logic.



<img style="display:block; margin-left: auto; margin-right: auto;" src="images/Wizard public API.png"/>

The following properties display directly on the user interface:

### ViewType

Set the `ViewType` property to the actual type of the WPF view file. Var:ProductName uses the type to inject the view into the wizard's user interface. Use the `typeof` C# keyword (`GetType` in VB.NET) to obtain the view type.

### ViewModel

The wizard infrastructure automatically assigns the `ViewModel` as the DataContext to the view. You can use `Binding` from the view to the ViewModel properties. If you don't need a ViewModel, assign `null` here.

You can use any `INotifyPropertyChanged` implementation, such as your own implementation or an MVVM framework.

### IsAvailable

The wizard framework uses this property to determine whether a page should display when navigating to other pages.

- When the user clicks **Next**, the wizard steps through subsequent pages and displays the first one where `IsAvailable` returns `true`. The **Next** button is disabled when no more pages are available.
- When the user clicks **Back**, the wizard steps through previous pages and displays the first one where `IsAvailable` returns `true`. The **Back** button is disabled when no more pages are available.

### Data

Define custom data for your pages using the `Data` property on the [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) object.

- The `Data` property is a `Dictionary<string,object>` that is shared across all wizard pages
- Once you set a dictionary key from a page, it becomes available to any subsequently displayed pages, including pages accessed via the **Back** button
- Use the page's `OnShow` method to query for the most recent key values that may have been updated by previously displayed pages
- The `Data` property is initialized for all pages before the wizard displays

### Icon

Set this property to display an icon in the top-right area of the page. Set to `null` if you don't want an icon displayed.

### Id

The wizard framework uses this property to uniquely identify pages.
## Integrating Pages as Wizard Steps

Custom pages integrate into wizards via the `firstPages` argument of the wizard's event object. The `firstPages` argument is available only for some wizard event objects.

To inject custom pages into a Var:ProductName wizard, complete the following steps:

* Determine or define a place in your code to launch the wizard (with custom steps), for example, from an [Action](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractAction.yml)'s `Execute` method
* Determine the appropriate event (for example, [OpenProjectPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.OpenProjectPackageEvent.yml)) that corresponds to the wizard you want to open
* Create a `List` of [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml) instances representing the custom pages to insert into the wizard
* Create the event, passing the `firstPages` list as an argument
* Use the event aggregator to raise the event, which launches the wizard

The custom pages appear at the beginning of the wizard, before any standard pages. Custom pages display in the same order they are defined in the list.

## Wizard Execution Flow

The following diagram describes the wizard's execution sequence and shows the order in which `StudioWizardPage` properties and methods execute at runtime.

<img style="display:block; " src="images/Wizard public API flow.png"/>

See also: [Full sample application with source code](https://github.com/RWS/trados-studio-api-samples/tree/master/TranslationStudioAutomation/Sdl.CustomWizardSteps.Sample) on GitHub.

