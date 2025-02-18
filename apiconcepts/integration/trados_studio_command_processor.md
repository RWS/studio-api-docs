Var:ProductName Command line processor 
====
Creating a Var:ProductName command line argument processor
---

To create a Var:ProductName command line processor as a third-party developer :

* implement the [IExternalCommandLineProcessor](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.IExternalCommandLineProcessor.yml) or [IExternalWindowAwareCommandLineProcessor](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.IExternalWindowAwareCommandLineProcessor.yml) depending if you want to run the command line processor before or after the main screen is shown. 

* from `Sdl.Desktop.IntegrationApi.Extensions.CommandLine` namespace and decorate your class with [ExternalCommandLineProcessorAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.ExternalCommandLineProcessorAttribute.yml) attribute. 

If you fail to implement [IExternalCommandLineProcessor](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.IExternalCommandLineProcessor.yml) or [IExternalWindowAwareCommandLineProcessor](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.IExternalWindowAwareCommandLineProcessor.yml),  an exception will be thrown, but if you omit the [ExternalCommandLineProcessorAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.ExternalCommandLineProcessorAttribute.yml), your plugin will simply be ignored when Var:ProductName starts.

Example
-----
The following example demonstrates how to implement a custom command line processor

The plugin implements the required interfaces mentioned above.

* **TaskName** returns the name of the command processing unit
* **TaskDescription** describes what that processing will do 
* **SupportedArguments** lists supported arguments

# [C#](#tab/tabid-1)
****
```
using Sdl.Desktop.IntegrationApi.Extensions.CommandLine;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Sdl.CustomPlugin.Packaging.OpenPackageWizard
{
    [ExternalCommandLineProcessor(Id = "OpenPackageCommandLineProcessor_Id", Name = "OpenPackageCommandLineProcessor_Name", Description = "OpenPackageCommandLineProcessor_Description")]
    public class OpenPackageCommandLineProcessor : IExternalWindowAwareCommandLineProcessor
    {
        private readonly IList<ExternalCommandLineArgumentDefinition> _supportedArguments;
        private const string OpenPackageArgument = "openPackage";

        public OpenPackageCommandLineProcessor()
        {
            _supportedArguments = new List<ExternalCommandLineArgumentDefinition>()
            {
                new ExternalCommandLineArgumentDefinition(OpenPackageArgument,
                    StringResources.OpenPackageCommandLineTask_Files)
                {
                    MinValues = 1,
                    MaxValues = 1,
                    SampleValues = new[] { "package" }
                }
            };
        }

        public string TaskName => StringResources.OpenPackageCommandLineTask_Name;

        public string TaskDescription => StringResources.OpenPackageCommandLineTask_Description;

        public IEnumerable<ExternalCommandLineArgumentDefinition> SupportedArguments => _supportedArguments;

        public void ProcessCommandLine(ExternalCommandLineArguments args)
        {
            ExternalCommandLineArgument packagesToOpen = args[OpenPackageArgument];
            if (packagesToOpen == null)
            {
                return;
            }

            var uiPackageOpener = new UIPackageOpener(Form.ActiveFrom);
            uiPackageOpener.OpenPackage(packagesToOpen.Values[0]);
        }
    }
}
```
****
