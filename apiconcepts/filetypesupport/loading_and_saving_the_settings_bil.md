# Loading and Saving the Settings

Implement the class responsible for physically storing settings and loading plug-in settings into the plug-in UI. Settings are stored in an **.sdlproj** or **.sdltpl** file, both of which are XML compliant.

**.sdlproj** files are automatically created for each document opened for translation/editing in `Var:ProductName` or for each project created in the application. These files contain project-specific information such as translation memories, termbases, and project-specific settings, which can include plug-in settings. An **.sdltpl** file is a project template that users can create to streamline project creation.

The settings bundle stored in an **.sdlproj** file looks, for example, as follows:

# [Xml](#tab/tabid-1)
```xml
<SettingsGroup Id="Word 2007 v 2.0.0.0">
  <Setting Id="Enable">True</Setting>
</SettingsGroup>
```

Each **Setting** element contains an **Id** attribute that denotes the setting (property) name. The **Setting** element encloses the property value, which changes when the user modifies the settings in the UI.

## Add the VerifierSettings Class

Add a new class to your project called **VerifierSettings.cs**. Your class requires these namespaces:

- `Sdl.Core.Settings`
- `Sdl.FileTypeSupport.Framework.Core.Settings`

Your class must be derived from the `FileTypeSettingsBase` class, which provides the methods required for storing and saving settings to the bundle.

## Implement the Setting Properties

Add the properties that represent the plug-in settings. In this case, add the boolean property `CheckWordArt`, which determines whether the verification functionality should be active. The `MaxWordCount` property stores the maximum number of words a segment is allowed to have.

# [C#](#tab/tabid-2)
```cs
private bool _checkWordArt;
private int _maxWordCount;

public bool CheckWordArt
{
    get { return _checkWordArt; }
    set
    {
        _checkWordArt = value;
        OnPropertyChanged("CheckWordArt");
    }
}

public int MaxWordCount
{
    get { return _maxWordCount; }
    set
    {
        _maxWordCount = value;
        OnPropertyChanged("MaxWordCount");
    }
}
```

## Implement the Reset to Defaults Method

Override the `ResetToDefaults` method provided by the base class to set the `CheckWordArt` and `MaxWordCount` properties to their default values:

# [C#](#tab/tabid-3)
```cs
public override void ResetToDefaults()
{
    CheckWordArt = true;
    MaxWordCount = 3;
}
```

## Implement the Constructor Method

Implement the constructor method. The constructor calls `ResetToDefaults` to set the `CheckWordArt` property to its default value (True) and set the maximum word count to 3:

# [C#](#tab/tabid-4)
```cs
public VerifierSettings()
{
    ResetToDefaults();
}
```

## Populate Settings from the Settings Bundle

The base class provides the `PopulateFromSettingsBundle` method to retrieve the current settings from the bundle in the **.sdlproj** file and set the `CheckWordArt` and `MaxWordCount` properties accordingly.

In the **.sdlproj** or **.sdltpl** file, a settings bundle looks as follows:

# [C#](#tab/tabid-5)
```xml
<SettingsBundle Guid="e0873e01-9da4-4e37-80e5-a35fc844f03d">
  <SettingsGroup Id="Word 2007 v 2.0.0.0">
    <Setting Id="MaxWordCount">3</Setting>
    <Setting Id="CheckWordArt">True</Setting>
  </SettingsGroup>

  <SettingsGroup Id="TermVerifierSettings">
    <Setting Id="Enabled">True</Setting>
  </SettingsGroup>
</SettingsBundle>
```

A **SettingsBundle** element encloses a number of **SettingsGroup** nodes. Each settings bundle has a unique ID (GUID) stored in an attribute. Settings bundles are identified by the ID of the plug-in they refer to. A settings group can enclose file type settings, verification plug-in settings, and so on. In the above example, the settings bundle encloses one settings group for the term verifier plug-in and another settings group for the WordArt verifier plug-in. This ID is retrieved from the File Type Component Builder (see [Create a New File Type Component Builder](create_new_file_type_component_builder.md)).

This XML structure is reflected in the API. Within the `PopulateFromSettingsBundle` method, create a settings group object based on `ISettingsGroup` by calling the `GetSettingsGroup` method on the settings bundle. This method takes the settings bundle ID (for example, *Word 2007 v 2.0.0.0*) as a string parameter. Then use the `GetSettingFromSettingsGroup` method to retrieve the `CheckWordArt` and `MaxWordCount` properties. This method requires the settings group object, the name of the setting (for example, *CheckWordArt*), and the default value as parameters:

# [C#](#tab/tabid-6)
```cs
public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
    ResetToDefaults();
    CheckWordArt = GetSettingFromSettingsGroup(settingsGroup, "CheckWordArt", CheckWordArt);
    MaxWordCount = GetSettingFromSettingsGroup(settingsGroup, "MaxWordCount", MaxWordCount);
}
```

## Save the Properties to the Settings Bundle

The base class provides the `SaveToSettingsBundle` method, which physically stores the value of the properties in the bundle of the **.sdlproj** file.

Create a settings group object based on `ISettingsGroup` by calling the `GetSettingsGroup` method on the settings bundle.

Then use the `UpdateSettingInSettingsGroup` method to save the settings into the physical representation. This method takes the settings group object, the setting name (string), the setting value, and the default value as parameters:

# [C#](#tab/tabid-7)
```cs
public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
    var defaults = new VerifierSettings();
    UpdateSettingInSettingsGroup(settingsGroup, "CheckWordArt", CheckWordArt, defaults.CheckWordArt);
    UpdateSettingInSettingsGroup(settingsGroup, "MaxWordCount", MaxWordCount, defaults.MaxWordCount);
}
```

## Putting It All Together

The complete class should look as follows:

# [C#](#tab/tabid-8)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.Core.Settings;
using System.Windows.Forms;

namespace Sdk.FileTypeSupport.Samples.WordArtVerifier
{
    /// <summary>
    /// This class stores the settings to the settings bundle, which
    /// is physically saved in an *.sdlproj or *.sdltpl file.
    /// </summary>
    public class VerifierSettings : FileTypeSettingsBase
    {
        private bool _checkWordArt;
        private int _maxWordCount;

        public bool CheckWordArt
        {
            get { return _checkWordArt; }
            set
            {
                _checkWordArt = value;
                OnPropertyChanged("CheckWordArt");
            }
        }

        public int MaxWordCount
        {
            get { return _maxWordCount; }
            set
            {
                _maxWordCount = value;
                OnPropertyChanged("MaxWordCount");
            }
        }

        public VerifierSettings()
        {
            ResetToDefaults();
        }

        /// <summary>
        /// Define the default value: Enabled (True), as the verification function should
        /// be active by default, and 3 for the default maximum word count.
        /// </summary>
        public override void ResetToDefaults()
        {
            CheckWordArt = true;
            MaxWordCount = 3;
        }

        /// <summary>
        /// Load the setting from the settings bundle,
        /// which is physically stored in an XML-compliant *.sdlproj or *.sdltpl file.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="configurationId"></param>
        public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
            ResetToDefaults();
            CheckWordArt = GetSettingFromSettingsGroup(settingsGroup, "CheckWordArt", CheckWordArt);
            MaxWordCount = GetSettingFromSettingsGroup(settingsGroup, "MaxWordCount", MaxWordCount);
        }

        /// <summary>
        /// Store the settings as configured in the plug-in UI
        /// in the settings bundle. The settings are physically written
        /// into the XML-compliant *.sdlproj or *.sdltpl file.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="configurationId"></param>
        public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
            var defaults = new VerifierSettings();
            UpdateSettingInSettingsGroup(settingsGroup, "CheckWordArt", CheckWordArt, defaults.CheckWordArt);
            UpdateSettingInSettingsGroup(settingsGroup, "MaxWordCount", MaxWordCount, defaults.MaxWordCount);
        }
    }
}
```

## See Also

- [Implement the User Interface](implement_the_user_interface_bil.md)
- [Implement the UI Controller Class](implement_the_ui_controller_class_bil.md)
- [Implement the Verification Logic](implement_the_verification_logic_bil.md)

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
