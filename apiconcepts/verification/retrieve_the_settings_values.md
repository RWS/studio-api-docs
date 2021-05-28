Retrieve the Settings Values
======
In this chapter you will learn how to retrieve the settings that have been configured through the plug-in user interface (see [Implement the User Interface](implement_the_user_interface.md)).

Add a Class for Retrieving the Settings Values
------

After implementing the user interface, you need to add a separate class for retrieving the plug-in settings values. Add a new class to your project, and call it e.g. `IdenticalVerifierSettings.cs`. The class needs to reference the **Sdl.Core.Settings** namespace. Your class needs to be derived from the [SettingsGroup](../../api/core/Sdl.Core.Settings.SettingsGroup.yml) class. Below you see what the skeleton of your new class looks like:

# [C#](#tab/tabid-1)
[!code-csharp[Settings](code_samples/Settings.aml#L24-L30)]
***

Our sample application only has one setting, i.e. a (display code) string value that defines which context should be relevant for the verification, e.g. **H** for **Heading**. This setting will be implemented as a string property, which we will call, for example `CheckContext`:

# [C#](#tab/tabid-1)
```cs
// Define the setting constant.
private const string CheckContext_Setting = "CheckContext";

// Return the value of the setting.
public Setting<string> CheckContext
{
    get { return GetSetting<string>(CheckContext_Setting); }
}
```
***


We will also implement a method for setting the default value. Let's assume that headings are likely to stay unchanged in the target language. Therefore it makes sense to apply the verification by default to segment pairs whose context has the context display code **H**:

# [C#](#tab/tabid-2)
```cs
protected override object GetDefaultValue(string settingId)
{
    switch (settingId)
    {
        case "CheckContext":
            return (string)"H";
        default:
            return base.GetDefaultValue(settingId);
    }
}
```
***

The plug-in settings are physically stored in the project files (* .*sdlproj*) or project template files (* .*sdltpl*). The settings group in the (XML-compliant) project (template) file can look as shown below. As you can see, the setting id corresponds to the name of the property that we have implemented in this class.

# [Xml](#tab/tabid-3)
```xml
<SettingsGroup Id="Identical Verifier">
    <Setting Id="Enabled">True</Setting>
  </SettingsGroup>
  <SettingsGroup Id="IdenticalVerifierSettings">
    <Setting Id="CheckContext">H</Setting>
  </SettingsGroup>
```
***

Note that the **Enabled** property does not need to be implemented by your plug-in. The plug-in framework provides the mechanism for enabling/disabling global verifier plug-ins through the user interface of <Var:ProductName>.

Putting it All Together
----
The complete class that is used for retrieving the settings value should look as shown below:

# [C#](#tab/tabid-4)
```cs
using Sdl.Core.Settings;

namespace Verification.Sdk.IdenticalCheck
{
    /// <summary>
    /// This class is used for reading and writing the plug-in setting(s) value(s).
    /// The settings are physically stored in an (XML-compliant) *.sdlproj file, which
    /// is generated for each project that is created in <Var:ProductName> or for 
    /// each file that is opened and translated.
    /// </summary>
    class IdenticalVerifierSettings : SettingsGroup
    {
        #region "setting"
        // Define the setting constant.
        private const string CheckContext_Setting = "CheckContext";

        // Return the value of the setting.
        public Setting<string> CheckContext
        {
            get { return GetSetting<string>(CheckContext_Setting); }
        }
        #endregion

        /// <summary>
        /// Return the default value of the setting property, i.e the context display code,
        /// which by default is H (i.e. headline).
        /// </summary>
        /// <param name="settingId"></param>
        /// <returns></returns>
        #region "default"
        protected override object GetDefaultValue(string settingId)
        {
            switch (settingId)
            {
                case "CheckContext":
                    return (string)"H";
                default:
                    return base.GetDefaultValue(settingId);
            }
        }
        #endregion
    }
}
```
***