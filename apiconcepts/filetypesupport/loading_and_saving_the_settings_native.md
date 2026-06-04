# Loading and Saving the Settings

Implement a class to store and load plug-in settings. Settings store in **.sdlproj* or **.sdltpl* files, both XML-compliant.

**.sdlproj* files automatically generate when a document opens for translation/editing in Var:ProductName or when a user creates a project. These files contain project-specific information, including translation memories, termbases, and plug-in settings. An **.sdltpl* file serves as a project template that streamlines project creation.

Within an **.sdlproj* or **.sdltpl* file, the settings bundle node appears as follows:

# [Xml](#tab/tabid-1)

```xml
<SettingsGroup Id="Length Check XML v 1.0.0.0">
  <Setting Id="Enable">True</Setting>
</SettingsGroup>
```
***

Each **Setting** element contains an **Id** attribute that denotes the setting (property) name. The **Setting** element encloses the property value, which updates whenever the user changes settings in the UI.

## Add the VerifierSettings Class

Add a new class called ```VerifierSettings``` to your project. Ensure your class uses the following namespaces:

* Sdl.Core.Settings
* Sdl.FileTypeSupport.Framework.Core.Settings

The class must implement the ```FileTypeSettingsBase``` class, which provides methods for storing and saving values to the settings bundle.

## Implement the Setting Property

Add the property representing the plug-in setting: the boolean property ```Enabled```, which determines whether verification functionality is active:

# [C#](#tab/tabid-2)
```cs
bool _enable;


public bool Enable
{
    get { return _enable; }
    set
    {
        _enable = value;
        OnPropertyChanged("Enable");
    }
}
```
***

## Implement the Constructor Method

The base class provides the `ResetToDefaults()` method. Override it to set the `Enabled` property to True:

# [C#](#tab/tabid-3)
```cs
public override void ResetToDefaults()
{
    Enable = true;
}
```
***

Next, implement the constructor method. The constructor calls `ResetToDefaults()` to set the `Enabled` property to True:

# [C#](#tab/tabid-4)
```cs
public VerifierSettings()
{
    ResetToDefaults();
}
```
***

## Set the Enabled Property from the Settings Bundle

The base class provides the `PopulateFromSettingsBundle()` method to retrieve the current setting from the bundle in the **.sdlproj* file and set the Enabled property accordingly.

In the **.sdlproj* or **.sdltpl* file, a settings bundle appears as follows:

# [Xml](#tab/tabid-5)
```xml
<SettingsBundle Guid="63403813-d91c-446e-8994-83ea138754e8">

    <SettingsGroup Id="TermVerifierSettings">
        <Setting Id="Enabled">True</Setting>

      <SettingsGroup Id="Length Check XML v 1.0.0.0">
        <Setting Id="Enable">False</Setting>

  </SettingsBundle>
```
***

A **SettingsBundle** element contains multiple **SettingsGroup** nodes. Each settings bundle has a unique identifier (GUID) in an attribute that identifies it by the plug-in id it refers to. A settings group contains file type settings, verification plug-in settings, and so on. In the example above, the settings bundle contains one settings group for the term verifier plug-in and another for the sample length verifier plug-in. The File Type Component Builder provides this identifier (see [Extending existing File Type Component Builder](extending_existing_file_type_component_builder.md)).

This XML structure reflects in the API. Within the `PopulateFromSettingsBundle()` method, create a settings group object based on `ISettingsGroup` by calling `GetSettingsGroup()` on the settings bundle. This method accepts the settings bundle id (for example, *Length Check XML v 1.0.0.0*) as a string parameter. Call `GetSettingFromSettingsGroup()` to set the `Enabled` property. This method requires the settings group object, the setting name (for example, *Enabled*), and the default value (for example, True) as parameters.

# [C#](#tab/tabid-6)
```cs
public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
    ResetToDefaults();
    Enable = GetSettingFromSettingsGroup(settingsGroup, "Enable", Enable);
}
```
***

## Save Properties to the Settings Bundle

The base class provides the `SaveToSettingsBundle()` method to store property values in the bundle of the **.sdlproj* file.

Create a settings group object based on `ISettingsGroup` by calling `GetSettingsGroup()` on the settings bundle. Call `UpdateSettingInSettingsGroup()` to save settings to the physical representation. This method accepts the settings group object, the setting name (string), the setting value, and the default value as parameters.

# [C#](#tab/tabid-7)
```cs
public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
    var defaults = new VerifierSettings();
    UpdateSettingInSettingsGroup(settingsGroup, "Enable", Enable, defaults.Enable);
}
```
***


## Putting It All Together

Your complete class now includes all required functionality:

# [C#](#tab/tabid-8)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.Core.Settings;
using System.Windows.Forms;

namespace Sdk.FileTypeSupport.Samples.XMLChecker
{
    public class VerifierSettings : FileTypeSettingsBase
    {
        bool _enable;

        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                OnPropertyChanged("Enable");
            }
        }

        public VerifierSettings()
        {
            ResetToDefaults();
        }

        public override void ResetToDefaults()
        {
            Enable = true;
        }

        public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
            ResetToDefaults();
            Enable = GetSettingFromSettingsGroup(settingsGroup, "Enable", Enable);
        }

        public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
            var defaults = new VerifierSettings();
            UpdateSettingInSettingsGroup(settingsGroup, "Enable", Enable, defaults.Enable);
        }
    }
}
```
***

## See Also

- [Implement the User Interface](implement_the_user_interface_native.md)
- [Implement the UI Controller Class](implement_the_ui_controller_class_native.md)
- [Implement the Verification Logic](implement_the_verification_logic_native.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
