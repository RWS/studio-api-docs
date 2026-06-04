# Enhancing the File Parser to Process the Settings

After implementing the user interface for configuring the file type plug-in settings and the functionality to load and save settings, extend your file parser class to be settings-aware so it considers settings when processing files.

## Add and Initialize the Settings Property

Ensure your parser class uses the following additional namespaces:

- **Sdl.FileTypeSupport.Framework.IntegrationApi**
- **Sdl.Core.Settings**
- **Sdl.FileTypeSupport.Framework.Core.Settings**

Implement the `ISettingsAware` interface in your parser class. This interface makes the parser settings-aware and provides the method to initialize the settings.

Add the following boolean property to your parser class:

# [C#](#tab/tabid-1)
```cs
public bool LockPrdCodes
{
    get;
    set;
}
```

This property corresponds to the setting retrieved through the settings UI.

Use the `InitializeSettings` method to initialize the plug-in setting. Call the `UserSettings` class and apply `PopulateFromSettingsBundle` to retrieve the current plug-in configuration. Set the `LockPrdCodes` property accordingly:

# [C#](#tab/tabid-2)
```cs
public void InitializeSettings(Sdl.Core.Settings.ISettingsBundle settingsBundle, string configurationId)
{
    UserSettings _userSettings = new UserSettings();
    _userSettings.PopulateFromSettingsBundle(settingsBundle, configurationId);
    LockPrdCodes = _userSettings.LockPrdCodes;
}
```

Modify the `ProcessLine()` helper function to call `WriteLockedContent()` only when the current line starts with the *Prd-Code* string and `LockPrdCodes` equals True:

# [C#](#tab/tabid-3)
```cs
// determines whether a given line is
// translatable or not
// if not, a structure tag is output
// otherwise, the translatable text is exposed
private void ProcessLine(string sLine)
{
    if (sLine.StartsWith("[") && sLine.EndsWith("]"))
    {
        WriteStructureTag(sLine);
        WriteContext(sLine);
    }
    else if (sLine.StartsWith("Prd-Code") && LockPrdCodes == true)
    {
        WriteLockedContent(sLine);
    }
    else
    {
        WriteText(ProcessFormatting(sLine));
    }
}
```

Your file parser now responds to settings changes made at runtime.

## See Also

- [Creating a New Assembly for the Settings UI](creating_a_new_assembly_for_the_settings_ui.md)
- [Implementing the Settings UI](implementing_the_settings_ui.md)
- [Implementing the UI Controller Class](implementing_the_ui_controller_class.md)
- [Loading and Saving the Settings](loading_and_saving_settings.md)

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
