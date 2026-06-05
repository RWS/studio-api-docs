# Extend Default Packaging Functionality

The Var:ProductName Integration API enables third-party developers to extend default project packaging operations by injecting additional pages and jobs into the project packaging wizards. You can also integrate directly with other systems' package formats.

## Extending Open Package Functionality

To extend the standard Open Package functionality, follow these steps:

- Create a specific action in the ribbon (see [Creating Actions](creating_actions.md))
- Implement specific wizard pages
- Trigger the [OpenProjectPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.OpenProjectPackageEvent.yml) via the [IStudioEventAggregator](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IStudioEventAggregator.yml), passing the custom wizard pages (see [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml)) and custom external job (see [IExternalJobWithProgress](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJobWithProgress.yml))

For a complete code sample, see [Customizing the Open Package Wizard Sample](customize_open_package_wizard.md).

## Extending Create Return Package Functionality

To extend the standard Create Return Package functionality, follow these steps:

- Create a specific action in the ribbon (see [Creating Actions](creating_actions.md))
- Implement specific wizard pages
- Trigger the [CreateReturnPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.CreateReturnPackageEvent.yml) via the [IStudioEventAggregator](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IStudioEventAggregator.yml), passing the custom wizard pages (see [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml)) and custom external job (see [IExternalJobWithProgress](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJobWithProgress.yml))

For a complete code sample, see [Customizing the Create Return Package Wizard Sample](customize_create_return_package_wizard.md).

## Importing and Exporting Custom Project Packages

To process project packages that are not in the proprietary Var:ProductName package format, third-party developers can implement an [external package converter](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IExternalPackageConverter.yml).

