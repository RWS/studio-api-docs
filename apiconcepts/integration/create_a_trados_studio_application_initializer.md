# Create a Var:ProductName Application Initializer

The Var:ProductName Integration API enables third-party developers to add custom initializations or functionalities when Var:ProductName starts.

## Creating an Application Initializer

To create a Var:ProductName application initializer, follow these steps:

- Implement the [IApplicationInitializer](../../api/integration/Sdl.Desktop.IntegrationApi.IApplicationInitializer.yml) interface in an initialization class.
- Decorate the initialization class with the [ApplicationInitializerAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ApplicationInitializerAttribute.yml) attribute.

If you fail to implement [IApplicationInitializer](../../api/integration/Sdl.Desktop.IntegrationApi.IApplicationInitializer.yml), an exception will be thrown. If you omit the [ApplicationInitializerAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ApplicationInitializerAttribute.yml), your plugin will simply be ignored when Var:ProductName starts.

## Example

The following example demonstrates a simple plugin data initialization. This plugin measures the time since Var:ProductName has been opened and if you worked less than required, it displays a warning MessageBox.

This example also demonstrates how to cleanup when Var:ProductName exits. All you have to do is subscribe to `Application.Closing` event. You can use `CancelEventArgs` to prevent Var:ProductName from closing.

### Simple Plugin Initializer

```cs
using System;
using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.TranslationStudioAutomation.IntegrationApi;

namespace StudioInitializer.Sample
{    
    /// <summary>
    /// Implements an application initializer which will keep track of the application startup and closing time.    
    /// </summary>
    [ApplicationInitializer]
    class PluginInitializer : IApplicationInitializer
    {
        private const int MinimumWorkTime = 4;

        /// <summary>
        /// This method is executed when the application is starting.
        /// </summary>
        public void Execute()
        {
            StudioTracking.Start();

            // Setting up a check at the application closure verifying if the user has worked less then the minimum amount of time 
            // If the time passed since Studio opening and Studio closing is less than the MinimumWorkTime(4h) than
            // request a confirmation from the user for the application closure otherwise just exit.
            SdlTradosStudio.Application.Closing += (s, e) =>
                {
                    TimeSpan elapsedTime = StudioTracking.Elapsed;
                    if (elapsedTime.Hours < MinimumWorkTime)
                    {
                        DialogResult dialogResult =
                            MessageBox.Show(
                                string.Format("You have been working for {0:dd\.hh\:mm\:ss} and spending less than {1} hours. Are you sure you want to quit ?", StudioTracking.Elapsed, MinimumWorkTime)
                                , string.Format("Total work time is {0} minutes", elapsedTime.Minutes)
                                , MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.No)
                        {
                            //Cancel the the application closing
                            e.Cancel = true;
                            return;
                        }
                    }
                    StudioTracking.Stop();
                };
        }
    }    
}
```
