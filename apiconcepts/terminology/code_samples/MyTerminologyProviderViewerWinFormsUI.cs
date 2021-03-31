using Sdl.Terminology.TerminologyProvider.Core;
using System;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace SDL_Terminology_Provider_Plug_in
{
    [TerminologyProviderViewerWinFormsUI]
    public class MyTerminologyProviderViewerWinFormsUI : ITerminologyProviderViewerWinFormsUI
    {
        
        private MyTerminologyProvider _terminologyProvider;
       

        #region "ControlObject"
        private TermProviderControl termControl;
        #endregion

        #region "Control"
        // Creates the control for the Termbase Viewer window, in this case a window
        // with an Internet Explorer control that displays the entry content in HTML format
        public Control Control
        {
            get
            {
                termControl = new TermProviderControl();
                return termControl;
            }
        }
        #endregion

        #region "Initialized"
        public bool Initialized
        {
            get
            {
                return true;
            }
        }
        #endregion

        public IEntry SelectedTerm
        {
            get
            {
                return SelectedTerm;
            }

            set
            {
                // do nothing
            }
        }

        public event EventHandler<EntryEventArgs> SelectedTermChanged;
        public event EventHandler TermChanged;

        #region "AddEditTerm"
        public void AddAndEditTerm(IEntry term, string source, string target)
        {
            MessageBox.Show("Sorry, editing terms is currently not implemented :-(", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region "AddTerms"
        //Triggered when the Studio user selects a source (and target) term, and then clicks the
        //corresponding button for adding the term or term pair to the glossary.
        //We loop through all existing term pairs to retrieve the next possible entry id, 
        //then we append the term or term pair to the text file.
        public void AddTerm(string source, string target)
        {
            string fileName = _terminologyProvider.fileName.Replace("file:///", "");
            StreamReader glossaryFile = new StreamReader(fileName);
            int i = 0;
            while (!glossaryFile.EndOfStream)
            {
                i++;
                glossaryFile.ReadLine();
            }
            glossaryFile.Close();
            int newEntryId = i + 1;
            StreamWriter outFile = new StreamWriter(fileName, true);
            outFile.WriteLine((newEntryId.ToString() + ";" + source + ";" + target + ";"));
            outFile.Close();

            //Show added entry in web browser control of the Termbase Viewer window
            string tmpFile = System.IO.Path.GetTempPath() + "simple_list_entry.htm";
            StreamWriter previewFile = new StreamWriter(tmpFile);
            previewFile.Write("<html><body><b>Entry id:</b> " + newEntryId.ToString() + 
                "<br/><b>Source term:</b> " + source + 
                "<br/><b>Target term:</b> " + target + 
                "</body></html>");
            previewFile.Close();
            termControl.webBrowser.Navigate(tmpFile);
        }
        #endregion

        public void EditTerm(IEntry term)
        {
            // Not used in this implementation.
        }

        #region "InitializeProvider"
        public void Initialize(ITerminologyProvider terminologyProvider, CultureInfo source, CultureInfo target)
        {
            _terminologyProvider = (MyTerminologyProvider)terminologyProvider;
        }
        #endregion

        #region "JumpToTerm"
        // this function outputs the full entry content in the Internet Explorer control
        // of the Termbase Viewer window
        public void JumpToTerm(IEntry entry)
        {
            // Load the glossary file
            string fileName = _terminologyProvider.fileName.Replace("file:///", "");
            StreamReader glossaryFile = new StreamReader(fileName);
            string[] chunks;
            string entryContent = String.Empty;
            glossaryFile.ReadLine();
            // Loop through all lines of the file and find the line that has the current entry id
            while (!glossaryFile.EndOfStream)
            {
                string thisLine = glossaryFile.ReadLine();
                chunks = thisLine.Split(';');
                string thisId = chunks[0];
                if(thisId==entry.Id.ToString())
                {
                    entryContent = thisLine;
                    break;
                }                    
            }

            // Parse the line alongside the semi-colon
            chunks = entryContent.Split(';');

            // Generate a small HTML file to display in the Termbase Viewer control
            string tmpFile = System.IO.Path.GetTempPath() + "simple_list_entry.htm";
            StreamWriter previewFile = new StreamWriter(tmpFile);
            previewFile.Write("<html><body><b>Entry id:</b> " + chunks[0] +
                "<br/><b>Source term:</b> " + chunks[1] +
                "<br/><b>Target term:</b> " + chunks[2] +
                "<br/><b>Definition:</b> " + chunks[3] +
                "</body></html>");
            previewFile.Close();
            termControl.webBrowser.Navigate(tmpFile);

            glossaryFile.Close();
        }
        #endregion

        #region "Release"
        public void Release()
        {
            _terminologyProvider = null;
        }
        #endregion

        public bool SupportsTerminologyProviderUri(Uri terminologyProviderUri)
        {
            return true;
        }
    }
}
