# Using Var:ProductName ProjectsController

The TranslationStudioAutomation API enables you to access the Var:ProductName projects view and perform project operations.

## Create a custom project list in a viewpart

This sample demonstrates how to create a custom project list using the [ProjectsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ProjectsController.yml) and add it to the projects view as a custom viewpart.

The custom list includes:

* Two columns: the project name and the number of target language files in the project
* Selection synchronization between the custom list and the projects view
* The ability to open a project by activating a selected project

### Step 1: Implement the user control

Create a Windows Forms user control to display the project list:

# [C#](#tab/tabid-1)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.ProjectAutomation.Core;
using Sdl.ProjectAutomation.FileBased;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi.Actions;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Interfaces;

namespace ProjectsOperations.Sample
{
    public partial class MyProjectsViewPartControl : UserControl, IUIControl
    {
        public MyProjectsViewPartControl()
        {
            InitializeComponent();
            ProjectsController projectsController = GetProjectsController();
            projectsController.ProjectsChanged += (sender, args) => RepopulateProjectsList();
            projectsController.SelectedProjectsChanged += OnSelectedProjectsChanged;
            projectsController.CurrentProjectChanging2 += OnCurrentProjectChanging2;
        }

        private ProjectsController GetProjectsController()
        {
            return SdlTradosStudio.Application.GetController<ProjectsController>();
        }

        private bool preventSelectionChanged;
        private FileBasedProject currentProject;

        private void RepopulateProjectsList()
        {
            preventSelectionChanged = true;
            ProjectsController projectsController = GetProjectsController();            
            ProjectsListView.Items.Clear();            
            foreach (var project in projectsController.GetProjects())
            {
                var item = new ListViewItem(project.GetProjectInfo().Name)
                               {
                                   Tag = project,
                                   Selected = projectsController.SelectedProjects.Contains(project),                                   
                                   Font = (project == currentProject ? new System.Drawing.Font(Font.FontFamily, Font.Size, System.Drawing.FontStyle.Bold): new System.Drawing.Font(Font.FontFamily, Font.Size))
                               };

                item.SubItems.Add(project.GetTargetLanguageFiles().Count().ToString());
                ProjectsListView.Items.Add(item);
            }
            preventSelectionChanged = false;
        }       

        private void ProjectsListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (preventSelectionChanged)
                return;

            var selectedProjects = new List<FileBasedProject>();
            for (int i=0; i < ProjectsListView.SelectedItems.Count; i++)
            {
                selectedProjects.Add(ProjectsListView.SelectedItems[i].Tag as FileBasedProject);
            }

            GetProjectsController().SelectedProjects = selectedProjects;
        }

        private void OnSelectedProjectsChanged(object sender, EventArgs eventArgs)
        {
            preventSelectionChanged = true;
            ProjectsListView.SelectedItems.Clear();

            foreach (var project in GetProjectsController().SelectedProjects)
            {
                ListViewItem projectItem = FindItem(project);
                if (projectItem != null)
                {
                    projectItem.Selected = true;
                }
            }

            preventSelectionChanged = false;
        }

        private void OnCurrentProjectChanging2(object sender, CurrentProjectCancelEventArgs eventArgs)
        {
            preventSelectionChanged = true;

            ListViewItem projectItem = FindItem(eventArgs.NewCurrentProject);
            if (projectItem != null)
            {                
                currentProject = eventArgs.NewCurrentProject;
            }

            preventSelectionChanged = false;
        }

        private void ProjectsListView_ItemActivate(object sender, EventArgs e)
        {
            var selectedItem = ProjectsListView.SelectedItems[0];
            GetProjectsController().Open(selectedItem.Tag as FileBasedProject);
        }     

        private void AddProjectButton_Click(object sender, EventArgs e)
        {
            SdlTradosStudio.Application.ExecuteAction<AddProjectAction>();
        }

        private void SetDueDateButton_Click(object sender, EventArgs e)
        {
            SetDueDateForSelectedProjects(dueDateCalendar.SelectionRange.Start);            
        }

        private void ClearDueDateButton_Click(object sender, EventArgs e)
        {
            SetDueDateForSelectedProjects(null);
        }

        private void SetDueDateForSelectedProjects(DateTime? dueDate)
        {
            bool hasChangedProjects = false;
            for (int i = 0; i < ProjectsListView.SelectedItems.Count; i++)
            {
                var project = (FileBasedProject)ProjectsListView.SelectedItems[i].Tag;
                ProjectInfo projectInfo = project.GetProjectInfo();
                projectInfo.DueDate = dueDate;
                project.UpdateProject(projectInfo);
                project.Save();                
                hasChangedProjects = true;
            }

            if (hasChangedProjects)
            {
                GetProjectsController().RefreshProjects();
            }                        
        }

        private ListViewItem FindItem(FileBasedProject project)
        {
            for (var i = 0; i < ProjectsListView.Items.Count; i++)
            {
                if (ProjectsListView.Items[i].Tag == project)
                {
                    return ProjectsListView.Items[i];
                }
            }

            return null;
        }

        private void MarkAsCompleted_Click(object sender, EventArgs e)
        {
            bool hasChangedProjects = false;
            for (int i = 0; i < ProjectsListView.SelectedItems.Count; i++)
            {
                var project = (FileBasedProject)ProjectsListView.SelectedItems[i].Tag;
                project.Complete();
                project.Save();
                hasChangedProjects = true;
            }

            if (hasChangedProjects)
            {
                GetProjectsController().RefreshProjects();
            }
        }
    }
}
```

### Step 2: Integrate the viewpart into the projects view

Register the viewpart with the projects view:

# [C#](#tab/tabid-1)
```cs
using System;
using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.Desktop.IntegrationApi.Interfaces;
using Sdl.TranslationStudioAutomation.IntegrationApi;

namespace ProjectsOperations.Sample
{
    [ViewPart(
        Id = "MyProjectsViewPartSample", 
        Name = "My Projects View Part", 
        Description = "Integrating a view part inside the projects view"        
        )]
    [ViewPartLayout(typeof(ProjectsController), Dock = DockType.Bottom)]
    class MyEditorViewPart : AbstractViewPartController
    {
        protected override IUIControl GetContentControl()
        {
            return _control.Value;
        }

        protected override void Initialize()
        {            
        }

        private static readonly Lazy<MyProjectsViewPartControl> _control = new Lazy<MyProjectsViewPartControl>(() => new MyProjectsViewPartControl());                        
    }
}
```

## See also

- [Integrating viewparts](integrating_viewparts.md)
