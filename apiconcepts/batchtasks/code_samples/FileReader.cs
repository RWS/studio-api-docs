using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.Core.Globalization;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;
using System.Windows.Forms;

namespace SDL_Sample_Custom_Batch_Task
{
    #region "Class"
    // This class performs the actual work, it needs to inherit from the 
    // AbstractBilingualContentProcessor class to process bilingual SDL XLIFF files
    public class FileReader : AbstractBilingualContentProcessor
    #endregion
    {
        #region "Variables"
        // Variables to retrieve the task settings
        // as well as the SDL XLIFF file patch to process and
        // the path of the TXT file that contains the exported content
        private readonly MyCustomBatchTaskSettings _taskSettings;
        private readonly string _inputFilePath;
        private StreamWriter _outFile;
        #endregion

        #region "FileReader"
        // Constructor that is set to the task settings and the path and name of the 
        // bilingual file to process
        public FileReader(MyCustomBatchTaskSettings settings, string filePath)
        {
            _taskSettings = settings;
            _inputFilePath = filePath;
        }
        #endregion

        #region "SetFileProperties"
        // We use this member to create the TXT export file
        public override void SetFileProperties(IFileProperties fileInfo)
        {
            _outFile = new StreamWriter(_inputFilePath + ".txt");
        }
        #endregion

        #region "ProcessParagraphUnit"
        // This member loops through all the segments, determines the segment status,
        // and then outputs the content to the text file (if applicable)
        public override void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
        {
            // Check if this paragraph actually contains segments 
            // If not, it is just a structure tag content, which is not processed
            if (paragraphUnit.IsStructure)
            {
                return;
            }

            // If the paragraph contains segment pairs, we loop through them,
            // determine their confirmation status, and depending on the status
            // output the text content to a TXT file
            foreach (ISegmentPair item in paragraphUnit.SegmentPairs)
            {
                int segmentStatus = _taskSettings.ConfirmationLevelSetting;
                if (item.Properties.ConfirmationLevel == (ConfirmationLevel)segmentStatus)
                    _outFile.WriteLine(item.Source + ";" + item.Target);
            }
        }
        #endregion

        #region "FileComplete"
        // Here we close the TXT file
        public override void FileComplete()
        {           
            base.FileComplete();
            _outFile.Close();
        }
        #endregion

        #region "Complete"
        // Not really used in this implementation.
        // Users can merge files and process them as one.
        // If a single file is processed, the FileComlete member is invoked
        // If all single files in a merged file are completed, then the Complete member is invoked
        public override void Complete()
        {
            base.Complete();
        }
        #endregion
    }
}