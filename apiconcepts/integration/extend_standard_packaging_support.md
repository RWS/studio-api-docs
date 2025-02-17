# Extend default packaging functionality

Var:ProductName Integration API provides support for third-party developers to extend the default project packaging operations by injecting additional pages and jobs in the project packaging wizards. It also allows direct integration with other systems package formats.

## Extending **Open Package** functionality

In order to extend the standard **Open Package** functionality, a third party developer will require the following steps:
- Create a specific action in the ribbon. (See [Creating Actions](creating_actions.md))
- Implement specific wizard pages 
- Trigger the [OpenProjectPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.OpenProjectPackageEvent.yml) via the [IStudioEventAggregator](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IStudioEventAggregator.yml) and passing on the custom wizard pages (see [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml)) and the custom external job (see [IExternalJobWithProgress](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJobWithProgress.yml))

See [Customizing the Open Package Wizard Sample](customize_open_package_wizard.md)

## Extending **Create Return Package** functionality

In order to extend the standard **Create Return Package** functionality, a third party developer will require the following steps:
- Create a specific action in the ribbon. (See [Creating Actions](creating_actions.md))
- Implement specific wizard pages 
- Trigger the [CreateReturnPackageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.CreateReturnPackageEvent.yml) via the [IStudioEventAggregator](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IStudioEventAggregator.yml) and passing on the custom wizard pages (see [StudioWizardPage](../../api/integration/Sdl.Desktop.IntegrationApi.Wizard.StudioWizardPage.yml)) and the custom external job (see [IExternalJobWithProgress](../../api/integration/Sdl.Desktop.IntegrationApi.Jobs.IExternalJobWithProgress.yml))

See [Customizing the Create Return Package Wizard Sample](customize_create_return_package_wizard.md)

## Importing and Exporting custom project packages

In order to be able to process project packages that are not the proprietary Var:ProductName package formats, a third party developer can implement an [external package convertor](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IExternalPackageConverter.yml)

