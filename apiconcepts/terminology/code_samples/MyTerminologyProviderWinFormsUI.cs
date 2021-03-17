using Sdl.Terminology.TerminologyProvider.Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace SDL_Terminology_Provider_Plug_in
{
    [TerminologyProviderWinFormsUI]
    public class MyTerminologyProviderWinFormsUI : ITerminologyProviderWinFormsUI
    {
        #region "EnableEditing"
        public bool SupportsEditing
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region "NameAndDescription"
        //Return terminology provider name and description to show in the Studio UI after
        //selection of the glossary.
        public string TypeDescription
        {
            get
            {
                return PluginResources.My_Terminology_Provider_Description;
            }
        }

        public string TypeName
        {
            get
            {
                return PluginResources.My_Terminology_Provider_Name;
            }
        }
        #endregion

        #region "Browse"
        //Raises the File Open dialog in which the user selects the glossary text file.
        //The file name and path is then passed to the terminology provider object.
        public ITerminologyProvider[] Browse(IWin32Window owner, ITerminologyProviderCredentialStore credentialStore)
        {
            OpenFileDialog dlgOpenFile = new OpenFileDialog
            {
                Title = "Select list file",
                Filter = "Delimited list files (*.txt)|*.txt"
            };
            dlgOpenFile.ShowDialog();

            var result = new List<ITerminologyProvider>();
            var _termProvider = new MyTerminologyProvider(dlgOpenFile.FileName);
            result.Add(_termProvider);
            return result.ToArray();
        }
        #endregion

        public bool Edit(IWin32Window owner, ITerminologyProvider terminologyProvider)
        {
            throw new NotImplementedException();
        }

        public TerminologyProviderDisplayInfo GetDisplayInfo(Uri terminologyProviderUri)
        {
            throw new NotImplementedException();
        }

        public bool SupportsTerminologyProviderUri(Uri terminologyProviderUri)
        {
            throw new NotImplementedException();
        }
    }
}
