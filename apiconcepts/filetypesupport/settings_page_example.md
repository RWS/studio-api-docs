# Settings Page Example

The following example shows a settings page that sets the sub-content processor [FileTypeConfiguration](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Settings.IFileTypeConfigurationAware.yml) IDs on the control:

# [C#](#tab/tabid-1)
```cs
using Sdl.FileTypeSupport.Framework.Core.Settings;
using Sdl.Sdk.Snippets.Native;

namespace Sdk.FileTypeSupport.Samples.SimpleText.WinUI
{
    /// <summary>
    /// This class controls the plug-in user interface. It handles what happens when the user clicks
    /// the Reset to Defaults button in the user interface. This class is referenced in the file type definition.
    /// Without this reference in the SDLFILETYPE file, the plug-in user interface is not available to the end user.
    /// </summary>
    [FileTypeSettingsPage(Id = "SimpleText_Settings", Name = "Settings_Name", Description = "Settings_Description")]
    class SettingsPage : AbstractFileTypeSettingsPage<SettingsUI, UserSettings>
    {
        /// <summary>
        /// Called when the user clicks the Reset to Defaults button in Trados Studio.
        /// Restores the default checkbox state (product code strings should be locked).
        /// </summary>
        public override void ResetToDefaults()
        {
            base.ResetToDefaults();
            Control.UpdateControl();
            // Set sub-content IDs on Control
            Control.FileTypeConfigurationIds = SubContentFileTypeConfigurationIds;
        }

        /// <summary>
        /// Called when the user opens the plug-in UI. Sets the control values according to the settings
        /// stored in the settings bundle.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            Control.UpdateControl();
            // Set sub-content IDs on Control
            Control.FileTypeConfigurationIds = SubContentFileTypeConfigurationIds;
        }
    }
}
```

>[!NOTE]
>
>The way the settings pages are created and referenced is exactly the same as in a main filter: See [Filter UI Settings](filter_ui_settings.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
