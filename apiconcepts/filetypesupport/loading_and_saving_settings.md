# Loading and Saving the Settings

Implement the class responsible for physically storing settings and loading plug-in settings into the plug-in UI. Settings are stored in an **.sdlproj** or **.sdltpl** file, both of which are XML compliant.

**.sdlproj** files are automatically created for each document opened for translation/editing in `Var:ProductName` or for each project created in the application. These files contain project-specific information such as translation memories, termbases, and project-specific settings, which can include plug-in settings. An **.sdltpl** file is a project template that users can create to streamline project creation.

The settings bundle used by our implementation looks, for example, as follows:

# [Xml](#tab/tabid-1)
```xml
<SettingsGroup Id="Simple Text Filter 1.0.0.0">
  <Setting Id="LockPrdCodes">True</Setting>
</SettingsGroup>
```

Each **Setting** element contains an **Id** attribute that denotes the setting (property) name. The **Setting** element encloses the property value, which changes when the user modifies the settings in the UI.

## Add the UserSettings Class

Add a new class to your project called **UserSettings.cs** in the project **Sdk.FileTypeSupport.Samples.SimpleText**. Your class requires these namespaces:

- `Sdl.Core.Settings`
- `Sdl.FileTypeSupport.Framework.Core.Settings`

Your class must implement the `FileTypeSettingsBase` class, which provides the methods required for storing and saving settings to the bundle.

## Implement the Setting Property

Add the property that represents the plug-in setting. In this case, add the boolean property `LockPrdCodes`, which determines whether the verification functionality should be active:

# [C#](#tab/tabid-2)
```cs
private const string SettingsLockPrdCodes = "LockPrdCodes";
private const bool DefaultLockPrdCodes = true;
private bool _lockPrdCodes;

public bool LockPrdCodes
{
    get { return _lockPrdCodes;  }
    set
    {
        _lockPrdCodes = value;
        OnPropertyChanged("LockPrdCodes");
    }
}
```

## Implement the Reset to Defaults Method

Override the `ResetToDefaults` method provided by the base class to set the `LockPrdCodes` property to the default value, True:

# [C#](#tab/tabid-3)
```cs
public override sealed void ResetToDefaults()
{
    LockPrdCodes = DefaultLockPrdCodes;
}
```

## Implement the Constructor Method

Implement the constructor method. The constructor calls `ResetToDefaults` to set the `LockPrdCodes` property to its default value, which is True:

# [C#](#tab/tabid-4)
```cs
public UserSettings()
{
    ResetToDefaults();
}
```

## Populate Settings from the Settings Bundle

The base class provides the `PopulateFromSettingsBundle` method to retrieve the current setting from the bundle in the **.sdlproj** file and set the `LockPrdCodes` property accordingly.

In the **.sdlproj** or **.sdltpl** file, a settings bundle looks as follows:

# [Xml](#tab/tabid-5)
```xml
<SettingsBundle Guid="63403813-d91c-446e-8994-83ea138754e8">
  <SettingsGroup Id="Length Check XML v 1.0.0.0">
    <Setting Id="Enable">True</Setting>
  </SettingsGroup>

  <SettingsGroup Id="Simple Text Filter 1.0.0.0">
    <Setting Id="LockPrdCodes">True</Setting>
  </SettingsGroup>
</SettingsBundle>
```

A **SettingsBundle** element encloses a number of **SettingsGroup** nodes. Each settings bundle has a unique ID (GUID) stored in an attribute. Settings bundles are identified by the ID of the plug-in they refer to. A settings group can enclose file type settings, verification plug-in settings, and so on. In the above example, the settings bundle encloses one settings group for another File Type Component Builder and another settings group for the simple text plug-in. This ID is retrieved from the File Type Component Builder (see [Adding the File Type Component Builder](adding_the_file_type_component_builder.md)).

This XML structure is reflected in the API. Within the `SaveToSettingsBundle` method, create a settings group object based on `ISettingsGroup` by calling the `GetSettingsGroup` method on the settings bundle. This method takes the settings bundle ID (for example, *Simple Text Filter 1.0.0.0*) as a string parameter. Then use the `GetSettingFromSettingsGroup` method to set the `LockPrdCodes` property. This method requires the settings group object, the name of the setting (for example, `LockPrdCodes`), and the default value (for example, True) as parameters:

# [C#](#tab/tabid-6)
```cs
public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string filterDefinitionId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(filterDefinitionId);
    LockPrdCodes = GetSettingFromSettingsGroup(settingsGroup, SettingsLockPrdCodes, DefaultLockPrdCodes);
}
```

## Save the Properties to the Settings Bundle

The base class provides the `SaveToSettingsBundle` method, which physically stores the value of the properties in the bundle of the **.sdlproj** file.

Create a settings group object based on `ISettingsGroup` by calling the `GetSettingsGroup` method on the settings bundle.

Then use the `UpdateSettingInSettingsGroup` method to save the settings into the physical representation. This method takes the settings group object, the setting name (string), the setting value, and the default value as parameters:

# [C#](#tab/tabid-7)
```cs
public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string filterDefinitionId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(filterDefinitionId);
    UpdateSettingInSettingsGroup(settingsGroup, SettingsLockPrdCodes, LockPrdCodes, DefaultLockPrdCodes);
}
```

## Putting It All Together

The complete class should look as follows:

# [C#](#tab/tabid-8)
```cs
using Sdl.FileTypeSupport.Framework.Core.Settings;
using Sdl.Core.Settings;

namespace Sdk.FileTypeSupport.Samples.SimpleText
{
    /// <summary>
    /// This class stores the settings to the settings bundle, which is physically saved
    /// in an *.sdlproj or *.sdltpl file.
    /// </summary>
    public class UserSettings: FileTypeSettingsBase
    {
        private const string SettingsLockPrdCodes = "LockPrdCodes";
        private const bool DefaultLockPrdCodes = true;
        private bool _lockPrdCodes;

        public bool LockPrdCodes
        {
            get { return _lockPrdCodes;  }
            set
            {
                _lockPrdCodes = value;
                OnPropertyChanged("LockPrdCodes");
            }
        }

        public UserSettings()
        {
            ResetToDefaults();
        }

        /// <summary>
        /// Define the default value: Enabled, because product code strings should not be
        /// exposed to translation by default.
        /// </summary>
        public override sealed void ResetToDefaults()
        {
            LockPrdCodes = DefaultLockPrdCodes;
        }

        /// <summary>
        /// Load the setting from the settings bundle, which is physically stored in an
        /// XML-compliant *.sdlproj or *.sdltpl file.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="filterDefinitionId"></param>
        public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string filterDefinitionId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(filterDefinitionId);
            LockPrdCodes = GetSettingFromSettingsGroup(settingsGroup, SettingsLockPrdCodes, DefaultLockPrdCodes);
        }

        /// <summary>
        /// Store the settings as configured in the plug-in UI in the settings bundle.
        /// The settings are physically written into the XML-compliant *.sdlproj or
        /// *.sdltpl file.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="filterDefinitionId"></param>
        public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string filterDefinitionId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(filterDefinitionId);
            UpdateSettingInSettingsGroup(settingsGroup, SettingsLockPrdCodes, LockPrdCodes, DefaultLockPrdCodes);
        }
    }
}
```

## See Also

- [Implement the User Interface](implement_the_user_interface_native.md)
- [Implement the UI Controller Class](implement_the_ui_controller_class_native.md)
- [Implement the Verification Logic](implement_the_verification_logic_native.md)

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
