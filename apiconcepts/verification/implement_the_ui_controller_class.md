Implement the UI Controller Class
=====

In this chapter you will learn how to implement a class for controlling the plug-in user interface. This class acts as a kind of intermediary between the plug-in UI and the class for setting and retrieving the settings values (see [Retrieve the Settings Values](retrieve_the_settings_values.md)).

Add the Class for Controlling the User Interface
----
Add new class to your project and call it e.g. `IdenticalVerifierUIPage.cs`. This class contains a number of overriding methods that are required for controlling the user interface, which you created previously (see Implement the User Interface). This class needs to reference the namespace 'Sdl.Core.Settings** and **Sdl.Verification.Api**. Moreover, the new class needs to be derived from [AbstractSettingsPage](../../api/core/Sdl.Core.Settings.AbstractSettingsPage.yml).

This class needs to be preceded by the following annotation, which makes it an extension class, which is referenced in the plug-in manifest (see also [Create a New Project](create_a_new_project.md)).

# [C#](#tab/tabid-1)
[!code-csharp[IdenticalVerifierUiPage](code_samples/IdenticalVerifierUiPage.cs#L7-L11)]
***

Note that there are various strings to provide in the annotation, such as the plug-in page name and description. The plug-in page name corresponds to the link that is clicked in the user interface of Var:ProductName to display the corresponding page.
Then declare the following private members:

* **_Control**: This member is derived from the user interface class (see [Implement the User Interface](implement_the_user_interface.md)); We use this member to control the actual user interface (e.g. when the user clicks the buttons **OK, Reset to Defaults**, etc. in the UI)
* **_ControlSettings**: This member is derived from our `IdenticalVerifierSettings` class (see [Retrieve the Settings Values](retrieve_the_settings_values.md)); it is used to access the setting values of the verifier plug-in.
The skeleton for your new class should look as shown below:

# [C#](#tab/tabid-2)
[!code-csharp[IdenticalVerifierUiPage](code_samples/IdenticalVerifierUiPage.cs#L1-L17)]
***

Access the Verifier Settings
-----
Start by adding the following private members, which are derived from the actual plug-in UI (see [Implement the User Interface](implement_the_user_interface.md)) and the verification settings class (see [Retrieve the Settings Values](retrieve_the_settings_values.md)):

# [C#](#tab/tabid-3)
[!code-csharp[IdenticalVerifierUiPage](code_samples/IdenticalVerifierUiPage.cs#L14-L15)]
***

In the next step, add the following member, which is used to get a handle on the plug-in user interface (i.e. the control).

# [C#](#tab/tabid-4)
```cs
public override object GetControl()
{
    _ControlSettings = ((ISettingsBundle)DataSource).GetSettingsGroup<IdenticalVerifierSettings>();
    _ControlSettings.BeginEdit();
    if (_Control == null)
    {
        _Control = new IdenticalVerifierUI();
    }

    return _Control;
}
```
***

Add the Functionality to Control the UI
------
Now you need to add a number of members that control the user interface. These methods are implemented to do the following:

* initialize the user interface
* load the settings that were configured in a previous session
* determine what happens when the user clicks the **OK** button, thereby saving the configured settings
* define what happens when the user clicks **Reset to Defaults**, thus restoring the settings to their default values
* moves away from the settings page, thus cancelling the settings page operation


Below you see examples of the members for controlling the user interface. The following member controls what happens when the user clicks the **Reset to Defaults** button, which is provided by the framework:

# [C#](#tab/tabid-5)
```cs
public override void ResetToDefaults()
{
    _ControlSettings.CheckContext.Reset();
    _Control.ContextToCheck = _ControlSettings.CheckContext;
}
```
***

The following member controls what happens when the user clicks the **OK** button, thereby committing the settings that were configured on the plug-in settings page:

# [C#](#tab/tabid-6)
```cs
public override void Save()
{
    _ControlSettings.CheckContext.Value = _Control.ContextToCheck;
}
```
***

The member below is used to control what happens when the user clicks **Cancel**, thereby not saving the settings:

# [C#](#tab/tabid-7)
```cs
public override void Cancel()
{
    _ControlSettings.CancelEdit();
}
```
***

Another required member of the abstract settings page base class is used to properly dispose of the user control:

# [C#](#tab/tabid-8)
```cs
public override void Dispose()
{
    _Control.Dispose();
}
```
***

Putting it All Together
-----
The complete class for controlling the settings page should now as shown below:

# [C#](#tab/tabid-9)
```cs
using Sdl.Core.Settings;

using Sdl.Verification.Api;

namespace Verification.Sdk.IdenticalCheck
{
    /// <summary>
    /// This is the extension class that displays and controls the plug-in user interface,
    /// in which the verification setting(s) can be specified. This class is responsible for
    /// e.g. saving the setting(s) configured in the UI, for resetting the values to their defaults,
    /// and for properly disposing of the UI control.
    /// </summary>
    #region "Declaration"
    [GlobalVerifierSettingsPage(
    Id = "Identical Settings Definition ID",
    Name = "Context to check",
    Description = "The display code of the context for which the length check should be performed.",
    HelpTopic = "")]
    #endregion
    class IdenticalVerifierUIPage : AbstractSettingsPage
    {
        #region "PrivateMembers"
        private IdenticalVerifierUI _Control;
        private IdenticalVerifierSettings _ControlSettings;
        #endregion

        #region "control"
        // Return the UI control.
        #region "GetControl"
        public override object GetControl()
        {
            _ControlSettings = ((ISettingsBundle)DataSource).GetSettingsGroup<IdenticalVerifierSettings>();
            _ControlSettings.BeginEdit();
            if (_Control == null)
            {
                _Control = new IdenticalVerifierUI();
            }

            return _Control;
        }
        #endregion


        // Load data from the settings into the UI control.
        #region "OnActivate"
        public override void OnActivate()
        {
            _Control.ContextToCheck = _ControlSettings.CheckContext;
        }
        #endregion



        // Reset the values on the UI control.
        #region "ResetToDefaults"
        public override void ResetToDefaults()
        {
            _ControlSettings.CheckContext.Reset();
            _Control.ContextToCheck = _ControlSettings.CheckContext;
        }
        #endregion


        public override bool ValidateInput()
        {
            return _Control.ValidateChildren();
        }


        // Save the values from the UI into settings class.
        #region "Save"
        public override void Save()
        {
            _ControlSettings.CheckContext.Value = _Control.ContextToCheck;
        }
        #endregion


        // Call EndEdit after all changes have been saved in the Save() call.
        #region "AfterSave"
        public override void AfterSave()
        {
            _ControlSettings.EndEdit();
        }
        #endregion


        // Cancel any pending changes.
        #region "Cancel"
        public override void Cancel()
        {
            _ControlSettings.CancelEdit();
        }
        #endregion

        // Properly dispose of the control.
        #region "Dispose"
        public override void Dispose()
        {
            _Control.Dispose();
        }
        #endregion

        #endregion
    }
}
```
***
