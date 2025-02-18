Using Var:ProductName FilesController
=====
TranslationStudioAutomation provides support for third-party developers to access the Var:ProductName files view and perform project files operations.

Creating a custom project files list inside a viewpart that belongs to the files view
-----
The following sample demonstrates how to create a custom project files list built by using the [FilesController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.FilesController.yml) and add it to the files view as a custom viewpart.

The custom list features:

* Display two columns: the project file name and the number of total words from the file.
* The projects files selected in the project files list will be in sync with the one from the custom list.


Start by implementing the windows form user control that will fill the content of the viewpart and is implementing the list view.

# [C#](#tab/tabid-1)
```cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi.Interfaces;
using Sdl.ProjectAutomation.Core;
using Sdl.TranslationStudioAutomation.IntegrationApi;

namespace FilesOperations.Sample
{
    public partial class MyFilesViewPartControl : UserControl, IUIControl
    {
        public MyFilesViewPartControl()
        {
            InitializeComponent();
            GetFilesController().SelectedFilesChanged += OnSelectedFilesChanged;
        }    

        private FilesController GetFilesController()
        {
            return SdlTradosStudio.Application.GetController<FilesController>();
        }

        private void OnSelectedFilesChanged(object sender, EventArgs eventArgs)
        {
            RepopulateFilesList();
            OpenFileButton.Enabled = false;
        }

        private void RepopulateFilesList()
        {
            FilesListView.Items.Clear();
            FilesController filesController = GetFilesController();   
            foreach (ProjectFile file in filesController.SelectedFiles)
            {
                var item = new ListViewItem(file.Name)
                               {
                                   Tag = file
                               };
                item.SubItems.Add(file.AnalysisStatistics.Total.Words.ToString());

                FilesListView.Items.Add(item);
            }            
        }

        private void FilesListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            OpenFileButton.Enabled = FilesListView.SelectedItems.Count > 0;
        }

        private void FilesListView_ItemActivate(object sender, EventArgs e)
        {
            if (FilesListView.SelectedItems.Count == 0)
            {
                return;
            }

            SdlTradosStudio.Application.GetController<EditorController>().Open((ProjectFile)FilesListView.SelectedItems[0].Tag, EditingMode.Translation);
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            var selectedProjectFiles = new List<ProjectFile>();
            for (int i = 0; i < FilesListView.SelectedItems.Count; i++)
            {
                selectedProjectFiles.Add((ProjectFile)FilesListView.SelectedItems[i].Tag);
            }

            if (selectedProjectFiles.Count == 0)
            {
                return;
            }

            try
            {
                SdlTradosStudio.Application.GetController<EditorController>()
                               .Open(selectedProjectFiles, EditingMode.Translation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Cannot open the project file: \n{0}", ex.Message));
            }
        }

        private void AddFileButton_Click(object sender, EventArgs e)
        {
            GetFilesController().AddFiles();
        }        
    }
}
```
***

Finally integrate the new viewpart to the files view.

# [C#](#tab/tabid-2)
```cs
using System;
using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.Desktop.IntegrationApi.Interfaces;
using Sdl.TranslationStudioAutomation.IntegrationApi;

namespace FilesOperations.Sample
{
    [ViewPart(
        Id = "MyFilesViewPart", 
        Name = "My Files View Part", 
        Description = "Integrating a view part inside the files view"        
        )]
    [ViewPartLayout(typeof(FilesController), Dock = DockType.Bottom)]
    class MyFilesViewPart : AbstractViewPartController
    {
        protected override IUIControl GetContentControl()
        {
            return _control.Value;
        }

        protected override void Initialize()
        {            
        }

        private FilesController FilesController
        {
            get { return SdlTradosStudio.Application.GetController<FilesController>(); }
        }

        private static readonly Lazy<MyFilesViewPartControl> _control = new Lazy<MyFilesViewPartControl>(() => new MyFilesViewPartControl());                        
    }
}
```
***
