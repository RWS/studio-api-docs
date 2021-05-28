Settings Page Example
==

An example of a settings page is shown below which sets the sub-content processor [FileTypeConfiguration](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Settings.IFileTypeConfigurationAware.yml) Ids on the control:

# [C#](#tab/tabid-1)
```cs
using Sdl.FileTypeSupport.Framework.Core.Settings;
using Sdl.Sdk.Snippets.Native;

namespace Sdk.FileTypeSupport.Samples.SimpleText.WinUI
{
    /// <summary>
    /// This class controls the plug-in user interface. It controls what happens, for example,
    /// when the user clicks the button in the user interface for resetting the control elements
    /// to their default values. This class is referenced in the file type definition. Without
    /// this reference in the SDLFILETPYE file, the plug-in user interface would not be available
    /// to the end user.
    /// </summary>
    #region "SettingsPagePlugin"
    [FileTypeSettingsPage(Id = "SimpleText_Settings", Name = "Settings_Name", Description = "Settings_Description")]
    #endregion
    #region "ClassDeclaration"
    class SettingsPage : AbstractFileTypeSettingsPage<SettingsUI, UserSettings>
    #endregion
    {
        /// <summary>
        /// Triggered, when the user clicks the button Reset to Defaults button in 
        /// Trados Studio. Restores the default check box state, which should
        /// be Checked (i.e. product code strings should be locked).
        /// </summary>
        #region "ResetToDefaults"
        public override void ResetToDefaults()
        {
            base.ResetToDefaults();
            Control.UpdateControl();
            // Set sub-content Ids on Control
            Control.FileTypeConfigurationIds = SubContentFileTypeConfigurationIds;
        }   
        #endregion

        /// <summary>
        /// Triggered when the user raises the plug-in UI, whose controls (in this case the check box
        /// for locking product code strings) will then be set according to the values stored in 
        /// the settings bundle.
        /// </summary>
        /// <param name="settingsBundle"></param>
        #region "Refresh"
        public override void Refresh()
        {
            base.Refresh();
            Control.UpdateControl();
            // Set sub-content Ids on Control
            Control.FileTypeConfigurationIds = SubContentFileTypeConfigurationIds;
        }
        #endregion


    }
}
```
***

>[!NOTE]
>
>The way the settings pages are created and referenced is exactly the same as in a main filter: See [Filter UI Settings](filter_ui_settings.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
