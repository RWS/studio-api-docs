Loading and Saving the Settings
==

In this chapter we will learn how to implement the class that is actually responsible for physically storing the settings and for loading the plug-in settings into the plug-in UI. The physical storing occurs in an **.sdlproj* or in an **.sdltpl* file, which are both XML compliant.

**.sdlproj* files are automatically created for each document that is opened for translation/editing in <Var:ProductName> or for each project that is created in the application. These files contain project-specific information such as the translation memories/termbases used for a project as well as any project-specific settings, which can also include plug-in settings. An **.sdltpl* file is a project template, which users can create to streamline project creation.

The settings bundle stored in an **.sdlproj* file as used by our implementation would look, for example, as shown below:

```xml
<SettingsGroup Id="Word 2007 v 2.0.0.0">
      <Setting Id="Enable">True</Setting>
    </SettingsGroup>
```

Click or drag to resize
Loading and Saving the Settings
In this chapter we will learn how to implement the class that is actually responsible for physically storing the settings and for loading the plug-in settings into the plug-in UI. The physical storing occurs in an *.sdlproj or in an *.sdltpl file, which are both XML compliant.

*.sdlproj files are automatically created for each document that is opened for translation/editing in SDL Trados Studio 2017 or for each project that is created in the application. These files contain project-specific information such as the translation memories/termbases used for a project as well as any project-specific settings, which can also include plug-in settings. An *.sdltpl file is a project template, which users can create to streamline project creation.

The settings bundle stored in an *.sdlproj file as used by our implementation would look, for example, as shown below:

```xml
<SettingsGroup Id="Word 2007 v 2.0.0.0">
      <Setting Id="Enable">True</Setting>
    </SettingsGroup>
```

Each **Setting** element contains an **Id** attribute that denotes the setting (property) name. The **Setting** element encloses the property value, which is changed when the user changes the settings in the UI.

Add the VerifierSettings Class
--

Start by adding a new class to your project called ```VerifierSettings```. Make sure that your class uses the following namespaces:

* **Sdl.Core.Settings**
* **Sdl.FileTypeSupport.Framework.Core.Settings**


Moreover, the component needs to be derived from the ```FileTypeSettingsBase``` class, which provides the methods required for storing and saving settings to the bundle.

Implement the Setting Properties
--

In this step, add the property that represents our plug-in setting, in this case the boolean property ```CheckWordArt```, which determines whether the verification functionality should be active or not. The ```MaxWordCount``` property is used to set or retrieve the maximum number of words a particular segment is allowed to have.

```cs
bool _checkWordArt;
int _maxWordCount;

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

Implement the Constructor Method
--

The ```ResetToDefaults``` method is provided by the base class. In our implementation, it is just overridden to set the ```MaxWordCount``` and ```CheckWordArtproperties``` to their default values, i.e. 3 and True:

```cs
public override void ResetToDefaults()
{
    CheckWordArt = true;
    MaxWordCount = 3;
}
```
Reset to Defaults
--

Next, implement the constructor method. In this implementation, the constructor just calls on the ```ResetToDefaults``` of the class to set the ```CheckWordArt``` property to its default value, which is True, and 3 as the maximum word count:

```cs
public VerifierSettings()
{
    ResetToDefaults();
}
```

Set the Properties to the Values from the Settings Bundle
--

The base class provides the ```PopulateFromSettingsBundle``` method to retrieve the current setting from the bundle in the **.sdlproj* file and set the ```CheckWordArt``` and the ```MaxWordCount``` properties of our implementation accordingly.

In the **.sdlproj* or **.sdltpl* file a settings bundle can look, for example, as shown below:

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

A **SettingsBundle** element can enclose a number of **SettingsGroup** nodes. Each settings bundle has a unique id (guid) that is stored in an attribute. The settings bundles are identified by the id of the plug-in that they refer to. A settings group can enclose, for example, file type settings, verification plug-in settings, etc. In the above example the settings bundle encloses one settings group for the term verifier plug-in and another settings group for our sample WordArt verifier plug-in. This id is retrieved from the File Type Component Builder (see [Create a New File Type Component Builder](create_new_file_type_component_builder.md)).

This XML structure is also reflected in the API. Within the ```SaveToSettingsBundle``` method we create a settings group object based on ```ISettingsGroup``` by applying the ```GetSettingsGroup``` method to the settings bundle. This method takes the settings bundle id (e.g. *Word 2007 v 2.0.0.0*) as string parameter. We then use the ``GetSettingFromSettingsGroup`` method to set the ```CheckWordArt``` and the ```MaxWordCount``` properties. This method requires the settings group object, the name of the setting (e.g. *CheckWordArt*), and the default value as parameters.

```cs
public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
    ResetToDefaults();
    CheckWordArt = GetSettingFromSettingsGroup(settingsGroup, "CheckWordArt", CheckWordArt);
    MaxWordCount = GetSettingFromSettingsGroup(settingsGroup, "MaxWordCount", MaxWordCount);
}
```

Set the Properties to the Values from the Settings Bundle
--

The base class also provides the ```SaveToSettingsBundle``` method, which is required to physically store the value of the two properties in the bundle of the *.sdlproj file.

Here too, we create a settings group object based on ```ISettingsGroup``` by applying the ```GetSettingsGroup``` method to the settings bundle.

Then we use the ```UpdateSettingInSettingsGroup``` method to save the settings into the physical representation. This method takes the settings group object, the setting name (string), the setting value, and the default value as parameters.

```cs
public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
{
    ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
    var defaults = new VerifierSettings();
    UpdateSettingInSettingsGroup(settingsGroup, "CheckWordArt", CheckWordArt, defaults.CheckWordArt);
    UpdateSettingInSettingsGroup(settingsGroup, "MaxWordCount", MaxWordCount, defaults.MaxWordCount);
}
```

Putting it All Together
--

The complete class should now look as shown below:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.Core.Settings;
using System.Windows.Forms;

namespace Sdl.Sdk.FileTypeSupport.Samples.WordArtVerifier
{
    /// <summary>
    /// This class is used to actually store the settings to the settings bundle, which
    /// is physically saved in an *.sdlproj or in an *.sdltpl file.
    /// </summary>
    public class VerifierSettings: FileTypeSettingsBase
    {
        #region "Properties"
        bool _checkWordArt;
        int _maxWordCount;

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
        #endregion


        #region "Constructor"
        public VerifierSettings()
        {
            ResetToDefaults();
        }
        #endregion    

        #region "OverrideMethods"

        /// <summary>
        /// Define the default value, which is Enabled (i.e. true), as the verification function should
        /// be active by default, and 3 for the default maximum word count.
        /// </summary>
        #region "ResetToDefaults"
        public override void ResetToDefaults()
        {
            CheckWordArt = true;
            MaxWordCount = 3;
        }
        #endregion


        /// <summary>
        /// This method is used to load the setting from the settings bundle,
        /// which is physically stored in an XML-compliant *.sdlproj or *.sdltpl file.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="configurationId"></param>
        #region "PopulateFromSettingsBundle"
        public override void PopulateFromSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
            ResetToDefaults();
            CheckWordArt = GetSettingFromSettingsGroup(settingsGroup, "CheckWordArt", CheckWordArt);
            MaxWordCount = GetSettingFromSettingsGroup(settingsGroup, "MaxWordCount", MaxWordCount);
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
        public override void SaveToSettingsBundle(ISettingsBundle settingsBundle, string configurationId)
        {
            ISettingsGroup settingsGroup = settingsBundle.GetSettingsGroup(configurationId);
            var defaults = new VerifierSettings();
            UpdateSettingInSettingsGroup(settingsGroup, "CheckWordArt", CheckWordArt, defaults.CheckWordArt);
            UpdateSettingInSettingsGroup(settingsGroup, "MaxWordCount", MaxWordCount, defaults.MaxWordCount);
        }
        #endregion

        #endregion
    }
}
```

See Also
--

**Other Resources**

[Implement the User Interface](implement_the_user_interface_bil.md)

[Implement the UI Controller Class](implement_the_ui_controller_class_bil.md)

[Implement the Verification Logic](implement_the_verification_logic_bil.md)

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.