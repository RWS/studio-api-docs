Enhancing the File Parser to Process the Settings
==

Now that you have implemented the user interface for configuring the file type plug-in settings as well as the functionality to load and save these settings, you need to extend your file parser class to make it 'aware' of the settings, so that they can be taken into consideration when processing a given file.

Add and Initialize the Settings Property
--

First, you need to make sure that your parser class also uses the following, additional namespaces:

* **Sdl.FileTypeSupport.Framework.IntegrationApi**
* **Sdl.Core.Settings**
* **Sdl.FileTypeSupport.Framework.Core.Settings**

Then your parser class needs to implement the ```ISettingsAware``` interface, which makes it 'aware' of the plug-in settings and provides the method that allows it to initialize the settings.

In the next step, add the following boolean property to your parser class:

```cs
public bool LockPrdCodes
{
    get;
    set;
}
```

The property corresponds to the setting that has been retrieved through the settings UI.

Then, use the ```InitializeSettings``` method of the interface to initialize the plug-in setting. Within this method, we call on the UserSettings class and apply the ```PopulateFromSettingsBundle``` method to retrieve the current plug-in configuration and then set the ```LockPrdCodes``` of the main verifier component accordingly.

```cs
public void InitializeSettings(Sdl.Core.Settings.ISettingsBundle settingsBundle, string configurationId)
{
    UserSettings _userSettings = new UserSettings();
    _userSettings.PopulateFromSettingsBundle(settingsBundle, configurationId);
    LockPrdCodes = _userSettings.LockPrdCodes;
}
```

Modify the ```ProcessLine()``` helper function so that it triggers ```WriteLockedContent()``` only if the current line starts with the *Prd-Code* string and if ```LockPrdCodes``` equals True.

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
    else if (sLine.StartsWith("Prd-Code") && LockPrdCodes==true)
    {
        WriteLockedContent(sLine);

    } 
    else
    {
        WriteText(ProcessFormatting(sLine));
    }
}
```

After making these changes your file parser should now respond to settings changes made at runtime.

See Also
--

**Other Resources**

[Creating a New Assembly for the Settings UI](creating_a_new_assembly_for_the_settings_ui.md)

[Implementing the Settings UI](implementing_the_settings_ui.md)

[Implementing the UI Controller Class](implementing_the_ui_controller_class.md)

[Loading and Saving the Settings](loading_and_saving_settings.md)

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.