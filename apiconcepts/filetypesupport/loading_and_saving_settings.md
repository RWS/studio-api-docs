Loading and Saving the Settings
===

In this chapter you will learn how to implement the class that is actually responsible for physically the settings and for loading the plug-in settings into the plug-in UI. The physical storing occurs in an **.sdlproj* or in an **.sdltpl* file, which are both XML compliant.

**.sdlproj* files are automatically created for each document that is opened for translation/editing in Var:ProductName or for each project that is created in the application. These files contain project-specific information such as the translation memories/termbases used for a project and any project-specific settings, which can also include plug-in settings. An **.sdltpl* file is a project template, which users can create to streamline project creation.

The settings bundle used by our implementation would look, for example, as shown below:

# [Xml](#tab/tabid-1)
```xml
<SettingsGroup Id="Simple Text Filter 1.0.0.0">
  <Setting Id="LockPrdCodes">True</Setting>
</SettingsGroup>
```
***

Each **Setting** element contains an **Id** attribute that denotes the setting (property) name. The **Setting** element encloses the property value, which is changed when the user changes the settings in the UI.

Add the UserSettings Class
--

Start by adding a new class to your project called **UserSettings.cs** to the project **Sdl.Sdk.FileTypeSupport.Samples.SimpleText**. Make sure that your class uses the following namespaces:

* **Sdl.Core.Settings**
* **Sdl.FileTypeSupport.Framework.Core.Settings**
Moreover, the class needs to implement the ```FileTypeSettingsBase``` class, which provides the methods required for storing and saving settings to the bundle.


Implement the Setting Property
--

In this step, add the property that represents our plug-in setting, in this case the boolean property ```LockPrdCodes```. It determines whether the verification functionality should be active or not.

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
***

Implement the Constructor Method
--

The ```ResetToDefaults``` method is provided by the base class. In our implementation, it is just overridden to set the ```LockPrdCodes``` property to our default value, True:

# [C#](#tab/tabid-3)
```cs
public override sealed void ResetToDefaults()
{
    LockPrdCodes = DefaultLockPrdCodes;
}
```
***

Reset to Defaults
--

Next, implement the constructor method. In this implementation, the constructor just calls on the ```ResetToDefaults``` of the class to set the ```LockPrdCodes``` property to its default value, which is True:

# [C#](#tab/tabid-4)
```cs
public UserSettings()
{
    ResetToDefaults();
}
```
***

Set the LockPrdCodes Property to the Value from the Settings Bundle
--

The base class provides the ```PopulateFromSettingsBundle``` method to retrieve the current setting from the bundle in the **.sdlproj* file and set the ```LockPrdCodes``` property of our implementation accordingly.

In the **.sdlproj* or **.sdltpl* file, a settings bundle can look as shown below:

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
***

A **SettingsBundle** element can enclose a number of **SettingsGroup** nodes. Each settings bundle has a unique id (guid) that is stored in an attribute. The settings bundles are identified by the id of the plug-in that they refer to. A settings group can enclose, for example, file type settings, verification plug-in settings, etc. In the above example the settings bundle encloses one settings group for another File Type Component Builder and another settings group for simple text plug-in. This id is retrieved from the File Type Component Builder (see [Adding the File Type Component Builder](adding_the_file_type_component_builder.md)).

This XML structure is also reflected in the API. Within the ```SaveToSettingsBundle``` method we create a settings group object based on ```ISettingsGroup``` by applying the ```GetSettingsGroup``` method to the settings bundle. This method takes the settings bundle id (e.g. *Simple Text Filter 1.0.0.0*) as string parameter. We then use the ```GetSettingFromSettingsGroup``` method to set the ```LockPrdCodes``` property. This method requires the settings group object, the name of the setting (e.g. ```LockPrdCodes```), and the default value as parameters (e.g. True).

# [C#](#tab/tabid-6)
```cs
public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string filterDefinitionId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(filterDefinitionId);
    LockPrdCodes = GetSettingFromSettingsGroup(settingsGroup, SettingsLockPrdCodes, DefaultLockPrdCodes);
}
```
***

Set the Property to the Values from the Settings Bundle
--

The base class also provides the ```SaveToSettingsBundle``` method, which is required to physically store the value of the properties in the bundle of the **.sdlproj* file.

Here too, we create a settings group object based on ```ISettingsGroup``` by applying the ```GetSettingsGroup``` method to the settings bundle.

Then we use the ```UpdateSettingInSettingsGroup``` method to save the settings into the physical representation. This method takes the settings group object, the setting name (string), the setting value, and the default value as parameters.

# [C#](#tab/tabid-7)
```cs
public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string filterDefinitionId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(filterDefinitionId);
    UpdateSettingInSettingsGroup(settingsGroup, SettingsLockPrdCodes, LockPrdCodes, DefaultLockPrdCodes);
}
```
***

Putting it All Together
--

The complete class should now look as shown below:

# [C#](#tab/tabid-8)
```cs
using Sdl.FileTypeSupport.Framework.Core.Settings;
using Sdl.Core.Settings;

namespace Sdk.FileTypeSupport.Samples.SimpleText
{
    /// <summary>
    /// This class is used to actually store the settings to the settings bundle, which
    /// is physically saved in an *.sdlproj or in an *.sdltpl file.
    /// </summary>
    public class UserSettings: FileTypeSettingsBase
    {
        #region "Properties"
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
        #endregion

        #region "Constructor"
        public UserSettings()
        {
            ResetToDefaults();
        }
        #endregion

        /// <summary>
        /// Define the default value, which is Enabled, as the product code strings should
        /// not be exposed to translation by default.
        /// </summary>
        #region "ResetToDefaults"
        public override sealed void ResetToDefaults()
        {
            LockPrdCodes = DefaultLockPrdCodes;
        }
        #endregion

        /// <summary>
        /// This method is used to load the setting from the settings bundle,
        /// which is physically stored in an XML-compliant *.sdlproj or *.sdltpl file.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="configurationId"></param>
        #region "PopulateFromSettingsBundle"
        public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string filterDefinitionId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(filterDefinitionId);
            LockPrdCodes = GetSettingFromSettingsGroup(settingsGroup, SettingsLockPrdCodes, DefaultLockPrdCodes);
        }
        #endregion

        /// <summary>
        /// This method is used to store the settings as configured in the plug-in UI
        /// in the settings bundle, which means that the settings are physically written
        /// into the XML-compliant *.sdlproj or *.sdltpl file.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="configurationId"></param>
        #region "SaveToSettingsBundle"
        public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string filterDefinitionId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(filterDefinitionId);
            UpdateSettingInSettingsGroup(settingsGroup, SettingsLockPrdCodes, LockPrdCodes, DefaultLockPrdCodes);
        }
        #endregion
    }
}
```
***

See Also
--



[Implement the User Interface](implement_the_user_interface_native.md)

[Implement the UI Controller Class](implement_the_ui_controller_class_native.md)

[Implement the Verification Logic](implement_the_verification_logic_native.md)


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
