# Implement the User Interface

Implement the user interface of your verification plug-in.

## Add a User Control

Implement the graphical user interface by adding a user control, for example **SettingsUI.cs**. This is the interface users see when configuring the file type plug-in in `Var:ProductName` through **File** > **Options** > **File Types**. Our simple native verifier implements one setting: whether to enable or disable the verification function. Add one check box to the user control named `cb_Enabled`.

The user interface should look as follows:

![length_checker_settings_page](images/length_checker_settings_page.jpg)

Switch to the code view of your user control. This class requires this namespace:

- `Sdl.FileTypeSupport.Framework.Core.Settings`

## The Settings Bundle

Each plug-in uses a settings bundle to store and retrieve settings. A separate class called `VerifierSettings` handles this mechanism (see [Loading and Saving the Settings](loading_and_saving_the_settings_native.md)). Create an object based on the `VerifierSettings` class within the controller UI component:

# [C#](#tab/tabid-1)
```cs
VerifierSettings _settings;
```

## Initialize the Plug-in User Interface

When the user opens the plug-in user interface, set the control element according to what is stored in the settings bundle. Use the `_settings` object (declared previously) to handle this:

# [C#](#tab/tabid-2)
```cs
public VerifierSettings Settings
{
    get
    {
        return _settings;
    }
    set
    {
        _settings = value;
        UpdateControl();
    }
}
```

During initialization, the `UpdateControl` method is invoked. It sets the check box state (checked or unchecked) to the value of the `Enabled` member in the `VerifierSettings` class:

# [C#](#tab/tabid-3)
```cs
public void UpdateControl()
{
    cb_Enable.Checked = _settings.Enable;
}
```

## Save the Settings to the Settings Bundle

When the user opens the plug-in UI, changes the check box setting, and clicks **OK**, save the setting to the settings bundle:

# [C#](#tab/tabid-4)
```cs
private void cb_Enable_CheckedChanged(object sender, EventArgs e)
{
    _settings.Enable = cb_Enable.Checked;
}
```

## Putting It All Together

Your `SettingsUI` class should look as follows:

# [C#](#tab/tabid-5)
```cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sdl.FileTypeSupport.Framework.Core.Settings;

namespace Sdk.FileTypeSupport.Samples.XMLChecker
{
    /// <summary>
    /// Implements the user interface through which the verification plug-in can be enabled or disabled.
    /// </summary>
    public partial class SettingsUI : UserControl, IFileTypeSettingsAware<VerifierSettings>
    {
        /// <summary>
        /// Create a settings object based on the VerifierSettings class. 
        /// </summary>
        VerifierSettings _settings;

        /// <summary>
        /// Initialize the user interface control by setting it to the value
        /// stored in the settings bundle.
        /// </summary>
        public SettingsUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Reset the user interface control to its default value: checked,
        /// which enables verification functionality by default.
        /// </summary>
        public void UpdateControl()
        {
            cb_Enable.Checked = _settings.Enable;
        }

        /// <summary>
        /// Save the settings based on the check box value. The setting is saved through
        /// the VerifierSettings class, which handles the plug-in settings bundle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_Enable_CheckedChanged(object sender, EventArgs e)
        {
            _settings.Enable = cb_Enable.Checked;
        }

        /// <summary>
        /// Implementation of IFileTypeSettingsAware allowing the Filter Framework
        /// to pass through the user settings so that we can initialize the UI.
        /// </summary>
        public VerifierSettings Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
                UpdateControl();
            }
        }
    }
}
```

## See Also

- [Implement the UI Controller Class](implement_the_ui_controller_class_native.md)
- [Loading and Saving the Settings](loading_and_saving_the_settings_native.md)

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
