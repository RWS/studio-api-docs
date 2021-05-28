using System;
using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi;

namespace Sample_Custom_Batch_Task
{
    #region "ClassDeclaration"
    // The visible UI control needs to implement the following interfaces:
    public partial class MyCustomBatchTaskSettingsControl : UserControl, ISettingsAware<MyCustomBatchTaskSettings>
    #endregion
    {
        #region "Settings"
        // Member that refers to the batch task settings
        public MyCustomBatchTaskSettings Settings { get; set; }
        #endregion

        #region "Initialize"
        // Initializes the UI component
        public MyCustomBatchTaskSettingsControl()
        {
            InitializeComponent();
        }
        #endregion

        #region "SetSettings"
        public void SetSettings(MyCustomBatchTaskSettings taskSettings)
        {
            // sets the UI element, i.e. the status dropdown list to the corresponding segment status value
            Settings = taskSettings;
            SettingsBinder.DataBindSetting<int>(combo_Status, "SelectedIndex", Settings, nameof(Settings.ConfirmationLevelSetting));
            UpdateUI(taskSettings);
        }
        #endregion

        #region "UpdateSettings"
        public void UpdateSettings(MyCustomBatchTaskSettings mySettings)
        {
            Settings = mySettings;
        }
        #endregion

        #region "UpdateUi"
        // Updates the UI elements to the corresponding settings
        public void UpdateUI(MyCustomBatchTaskSettings mySettings)
        {
            Settings = mySettings;
            this.UpdateSettings(Settings);
        }
        #endregion


        #region "OnLoad"
        // The control elements on the UI are configured with the corresponding values
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.SetSettings(Settings);
        }
        #endregion

        #region "ResetToDefault"
        // Set dropbown control to the default value 'Translate' (2) when the user 
        // clicks the Restore Defaults button
        private void btn_Reset_Click(object sender, EventArgs e)
        {
            Settings.ResetToDefaults();
            this.UpdateUI(Settings);
        }
        #endregion
    }
}
