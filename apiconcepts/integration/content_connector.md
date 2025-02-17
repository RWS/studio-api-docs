Content Connector
==

The Content Connector sits at the heart of this extension. Its main classes are ContentConnector, ContentConnectorViewRibbonGroup, ContentConncetorViewController and ContentConnectorViewControl.

Content Connector
--

The ContentConnector class simply reads the file names in the IncomingRequests folder and wraps them in a ProjectRequest object.

# [C#](#tab/tabid-1)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StudioIntegrationApiSample
{
    class ContentConnector
    {
        public static readonly ContentConnector Instance = new ContentConnector();

        private ContentConnector()
        {
        }

        public List<ProjectRequest> ProjectRequests
        {
            get; private set;
        }

        public void Refresh()
        {
            ProjectRequests = new List<ProjectRequest>();

            string dropFolder = GetIncomingRequestsFolder();

            foreach (string directory in Directory.GetDirectories(dropFolder))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(directory);
                ProjectRequests.Add(new ProjectRequest
                {
                    Name = dirInfo.Name,
                    Files = Directory.GetFiles(directory)
                });
            }
        }

        private static string GetIncomingRequestsFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $@"{Versioning.Versions.StudioDocumentsFolderName}\IncomingRequests");
        }

        private static string GetAcceptedRequestsFolder()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $@"{Versioning.Versions.StudioDocumentsFolderName}\AcceptedRequests");
        }

        internal void RequestAccepted(ProjectRequest request)
        {
            Directory.CreateDirectory(GetAcceptedRequestsFolder());
            Directory.Move(Path.Combine(GetIncomingRequestsFolder(), request.Name), Path.Combine(GetAcceptedRequestsFolder(), request.Name));
        }
    }
}
```
***

Content Connector View Ribbon Group
--

This class illustrates how a CreateProjectAction is defined and how a custom RibbonGroup on the Add-Ins ribbon tab is created and also how the action is added to it.

# [C#](#tab/tabid-2)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.Desktop.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations;

namespace StudioIntegrationApiSample
{
    [RibbonGroup("ConnectorViewRibbonGroup", "ConnectorViewRibbonGroup_Name")]
    [RibbonGroupLayout(LocationByType = typeof(TranslationStudioDefaultRibbonTabs.HomeRibbonTabLocation))]    
    class ContentConnectorViewRibbonGroup : AbstractRibbonGroup
    {
    }

    [Action("CheckForProjectsAction", typeof(ContentConnectorViewController), Name = "CheckForProjects_Name", Description = "CheckForProjects_Description", Icon = "CheckForProjects_Icon")]
    [ActionLayout(typeof(ContentConnectorViewRibbonGroup), 2, DisplayType.Large)]
    public class CheckForProjectsAction : AbstractViewControllerAction<ContentConnectorViewController>
    {
        protected override void Execute()
        {
            Controller.CheckForProjects();   
        }
    }

    [Action("CreateProjectsAction", typeof(ContentConnectorViewController), Name = "CreateProjects_Name", Description = "CreateProjects_Description", Icon = "CreateProjects_Icon")]
    [ActionLayout(typeof(ContentConnectorViewRibbonGroup), 1, DisplayType.Large)]
    public class CreateProjectsAction : AbstractViewControllerAction<ContentConnectorViewController>
    {
        public override void Initialize()
        {
            Controller.ProjectRequestsChanged += OnProjectRequestsChanged;
        }

        void OnProjectRequestsChanged(object sender, EventArgs e)
        {
            Enabled = Controller.ProjectRequests.Count > 0;
        }

        protected override void Execute()
        {
            Controller.CreateProjects();
        }
    }

}
```
***

Content Connector ViewController
--

The Var:ProductName> Integration API is built in accordance with the MVC pattern. The View Controller is used to add new views to Var:ProductName> - the code sample below illustrates how this is achieved using the View attribute. The controller is also responsible for providing the View UI element. Additionally this sample has a ```CreateProjects()``` method which the controller uses to create new projects from the files in the IncomingRequests folder.

# [C#](#tab/tabid-3)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.TranslationStudioAutomation.IntegrationApi.Presentation;
using Sdl.Desktop.IntegrationApi;
using System.ComponentModel;
using Sdl.ProjectAutomation.Core;
using Sdl.ProjectAutomation.FileBased;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using System.Windows.Forms;
using Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations;

namespace StudioIntegrationApiSample
{
    [View(
        Id = "ContentConnectorView",
        Name = "Content Connector",
        Description = "Create projects from project request content",
        Icon = "CheckForProjects_Icon",
        LocationByType = typeof(TranslationStudioDefaultViews.TradosStudioViewsLocation))]
    public class ContentConnectorViewController : AbstractViewController, INotifyPropertyChanged
    {
        #region private fields
        private readonly Lazy<ContentConnectorViewControl> _control = new Lazy<ContentConnectorViewControl>(() => new ContentConnectorViewControl());
        private ProjectTemplateInfo _selectedProjectTemplate;
        private List<ProjectRequest> _projectRequests;
        private int _percentComplete;
        #endregion private fields

        public event EventHandler ProjectRequestsChanged;

        public ContentConnectorViewController()
        {
            _projectRequests = new List<ProjectRequest>();
        }

        protected override void Initialize(IViewContext context)
        {
            ProjectsController = SdlTradosStudio.Application.GetController<ProjectsController>();
            _control.Value.Controller = this;
        }

        protected override System.Windows.Forms.Control GetContentControl()
        {
            return _control.Value;
        }

        private ProjectsController ProjectsController
        {
            get;
            set;
        }

        public IEnumerable<ProjectTemplateInfo> ProjectTemplates
        {
            get
            {
                return ProjectsController.GetProjectTemplates();
            }
        }

        public ProjectTemplateInfo SelectedProjectTemplate
        {
            get
            {
                return _selectedProjectTemplate;
            }
            set
            {
                _selectedProjectTemplate = value;
                OnPropertyChanged("SelectedProjectTemplate");
            }
        }

        public List<ProjectRequest> ProjectRequests
        {
            get
            {
                return _projectRequests;
            }
            set
            {
                _projectRequests = value;
                OnPropertyChanged("ProjectRequests");

                OnProjectRequestsChanged();
            }
        }

        public int PercentComplete 
        {
            get
            {
                return _percentComplete;
            }
            set
            {
                _percentComplete = value;
                OnPropertyChanged("PercentComplete");
            }
        }

        public List<FileBasedProject> Projects
        {
            get;
            set;
        }

        public void CheckForProjects()
        {
            ContentConnector.Instance.Refresh();
            ProjectRequests = ContentConnector.Instance.ProjectRequests;
        }

        public void CreateProjects()
        {
            _control.Value.ClearMessages();

            ProjectCreator creator = null;
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += (sender, e) =>
                {
                    creator = new ProjectCreator(ProjectRequests, SelectedProjectTemplate);
                    creator.ProgressChanged += (sender2, e2) => { worker.ReportProgress(e2.ProgressPercentage); };
                    creator.MessageReported += (sender2, e2) => { ReportMessage(e2.Project, e2.Message); };
                    creator.Execute();
                };
            worker.ProgressChanged += (sender, e) =>
                {
                    PercentComplete = e.ProgressPercentage;
                };
            worker.RunWorkerCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        MessageBox.Show(e.Error.ToString());
                    }
                    else
                    {
                        foreach (Tuple<ProjectRequest, FileBasedProject> request in creator.SuccessfulRequests)
                        {
                            // accept the request
                            ContentConnector.Instance.RequestAccepted(request.Item1);

                            // remove the request from the list of requests
                            ProjectRequests.Remove(request.Item1);

                            OnProjectRequestsChanged();
                        }
                    }
                };
            worker.RunWorkerAsync();
        }

        private void ReportMessage(FileBasedProject fileBasedProject, string message)
        {
            _control.Value.BeginInvoke(new Action(() => _control.Value.ReportMessage(fileBasedProject, message)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnProjectRequestsChanged()
        {
            if (ProjectRequestsChanged != null)
            {
                ProjectRequestsChanged(this, EventArgs.Empty);
            }
        }
    }
}
```
***

Content Connector ViewControl
--

This class is responsible for rendering the UI and interacts with the controller described above for data retrieval.

# [C#](#tab/tabid-4)
```cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Sdl.ProjectAutomation.Core;
using Sdl.ProjectAutomation.FileBased;

namespace StudioIntegrationApiSample
{
    public partial class ContentConnectorViewControl : UserControl
    {
        ContentConnectorViewController _controller;

        public ContentConnectorViewControl()
        {
            InitializeComponent();

            _projectsListBox.SelectedIndexChanged += new EventHandler(_projectsListBox_SelectedIndexChanged);
        }

        internal ContentConnectorViewController Controller
        {
            get
            {
                return _controller;
            }
            set
            {
                _controller = value;

                if (_controller != null)
                {
                    _controller.ProjectRequestsChanged += new EventHandler(_controller_ProjectRequestsChanged);
                }

                _progressBar.DataBindings.Add("Value", _controller, "PercentComplete");

                LoadProjectTemplates();
                LoadProjectRequests();
            }
        }

        public void ClearMessages()
        {
            _resultsTextBox.Text = "";
        }

        public void ReportMessage(FileBasedProject fileBasedProject, string message)
        {

            _resultsTextBox.AppendText("\r\n" + message);
        }

        private void LoadProjectTemplates()
        {
            _projectTemplatesComboBox.Items.Clear();

            if (_controller.ProjectRequests != null)
            {
                foreach (ProjectTemplateInfo projectTemplate in _controller.ProjectTemplates)
                {
                    _projectTemplatesComboBox.Items.Add(projectTemplate);
                }

                if (_projectTemplatesComboBox.Items.Count > 0)
                {
                    _projectTemplatesComboBox.SelectedIndex = 0;
                }
            }
        }



        private void LoadProjectRequests()
        {
            _projectsListBox.Items.Clear();

            if (_controller.ProjectRequests != null)
            {
                foreach (ProjectRequest projectRequest in _controller.ProjectRequests)
                {
                    _projectsListBox.Items.Add(projectRequest);
                }

                if (_projectsListBox.Items.Count > 0)
                {
                    _projectsListBox.SelectedIndex = 0;
                }
            }

            LoadFileList();
        }

        private void LoadFileList()
        {
            _filesListView.Items.Clear();

            ProjectRequest selectedProject = _projectsListBox.SelectedItem as ProjectRequest;
            if (selectedProject != null)
            {
                foreach (string file in selectedProject.Files)
                {
                    string fileName = Path.GetFileName(file);
                    _filesListView.Items.Add(fileName);
                }
            }
        }

        void _controller_ProjectRequestsChanged(object sender, EventArgs e)
        {
            LoadProjectRequests();
        }

        void _projectsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFileList();
        }



        private void _projectTemplatesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.SelectedProjectTemplate = _projectTemplatesComboBox.SelectedItem as ProjectTemplateInfo;
        }


    }
}
```
***

See Also
--



[Reference Sample](reference_sample.md)

[Project Creator](project_creator.md)

[Wikipedia Search](wikipedia_search.md)

[Integrating actions](integrating_actions.md)

[Integrating ribbon groups](integrating_ribbon_groups.md)

[Integrating viewparts](integrating_viewparts.md)

[Integrating views](integrating_views.md)
