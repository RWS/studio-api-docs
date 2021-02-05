using Sdl.ProjectAutomation.AutomaticTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.ProjectAutomation.Core;
using Sdl.Core.Globalization;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using System.Windows.Forms;

namespace SDL_Sample_Custom_Batch_Task
{
    #region "PluginDeclaration"
    // Plug-in is declared to Studio with is, name and description
    // Furthermore, you declare the type of file that is processed with this plug-in
    // This sample plug-in works on bilingual SDL XLIFF files, i.e. not on native source files
    [AutomaticTask("My_Custom_Batch_Task_ID", "My_Custom_Batch_Task_Name", "My_Custom_Batch_Task_Description",
        GeneratedFileType = AutomaticTaskFileType.BilingualSource)]
    #endregion

    #region "SettingsPage"
    // References the settings page, i.e. the UI in which the user can configure plug-in settings
    [AutomaticTaskSupportedFileType(AutomaticTaskFileType.NativeSource)]
    [RequiresSettings(typeof(MyCustomBatchTaskSettings), typeof(MyCustomBatchTaskSettingsPage))]
    #endregion

    #region "ClassDeclaration"
    // A batch task plug-in needs to implement the AbstractFileContentProcessingAutomaticTask class
    public class MyCustomBatchTask : AbstractFileContentProcessingAutomaticTask
    #endregion
    {
        #region "Variables"
        // These variables store the plug-in settings,
        // the report string content
        // and the language direction
        private MyCustomBatchTaskSettings _settings;
        private string _reportContent;
        #endregion

        #region "Initialize"
        // The task is initialized
        // This means that the settings are retrieved,
        // and we start constructing the report content string
        protected override void OnInitializeTask()
        {
            _settings = GetSetting<MyCustomBatchTaskSettings>();
            _reportContent +="<report segmentStatus='" + ((ConfirmationLevel)_settings.ConfirmationLevelSetting).ToString() + "'>";
        }
        #endregion

        #region "Converter"
        // Here we continue constructing the report string.
        // Also we trigger the actual task by creating a FileReader object
        // to which we pass the settings and the SDL XLIFF file name
        protected override void ConfigureConverter(ProjectFile projectFile, IMultiFileConverter multiFileConverter)
        {
            // We output each file name in the report
            // and the date/time at which processing started
            _reportContent += "<file>";
            _reportContent += "<fileName>" + projectFile.Name + "</fileName>";
            _reportContent += "<language>" + projectFile.Language.ToString() + "</language>";
            _reportContent += "<processTime>" + DateTime.Now.ToString() + "</processTime>";
                        
            // We initialize the class that performs the actual work
            FileReader _task = new FileReader(_settings, projectFile.LocalFilePath);
            multiFileConverter.AddBilingualProcessor(_task);
            multiFileConverter.Parse();
            _reportContent += "</file>";
        }
        #endregion

        #region "TaskComplete"
        // At the end of the task, we output the 
        public override void TaskComplete()
        {
            // We close the XML report string
            _reportContent += "</report>";
            // We call the method that actually creates the report in Studio
            // In this method we pass the report name, description, the actual
            // XML report content string and the optional language direction parameter.
            CreateReport("SDK Sample Task Report", "Sample batch task plug-in report", _reportContent);
            
        }
        #endregion
    }
}
