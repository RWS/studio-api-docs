using Sdl.Core.Settings;

namespace Sample_Custom_Batch_Task
{
    #region "Class"
    // The settings class needs to implement the SettingsGroup interface
    public class MyCustomBatchTaskSettings : SettingsGroup
    #endregion
    {
        #region "DefaultValue"
        // We set the default confirmation leve to 2, which corresponds to the 'Translated' status
        private readonly int _confirmationLevel = 2;
        #endregion


        #region "ConfirmationLevelSetting"
        // Retrieves and sets the segment status value
        public int ConfirmationLevelSetting
        {
            get { return GetSetting<int>(nameof(ConfirmationLevelSetting)); }
            set { GetSetting<int>(nameof(ConfirmationLevelSetting)).Value = value; }
        }
        #endregion

        #region "ResetToDefaults"
        // When the user clicks the Restore defaults button
        // The default segment status, i.e. 'Translated' (2) should be set
        public void ResetToDefaults()
        {
            ConfirmationLevelSetting = _confirmationLevel;
        }
        #endregion

        #region "GetDefaultValue"
        // Gets the the default segment status value
        protected override object GetDefaultValue(string settingId)
        {
            switch (settingId)
            {
                case nameof(ConfirmationLevelSetting):
                    return _confirmationLevel;
            }
            return base.GetDefaultValue(settingId);
        }
        #endregion
    }
}
