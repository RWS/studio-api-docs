using System.Linq;
using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi.Presentation;
using System;
using Sdl.Desktop.IntegrationApi.Extensions.Internal;
using System.ComponentModel;
using Sdl.Desktop.IntegrationApi.DefaultLocations;
using Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations;

namespace Actions.Sample
{
    #region General action   

    [Action("MyMainIconAction", Icon = "MyAction_Icon")]
    [ActionLayout(typeof(MySampleRibbonGroup), 10, DisplayType.Large)]
    [Shortcut(Keys.Alt | Keys.F8)]
    public class MyMainIconAction : AbstractAction
    {
        protected override void Execute()
        {
            MessageBox.Show("My icon and shortcut key action sample.");
        }
    }

    #endregion

    #region Controller specific actions

    [Action("MyNormalSizeAction")]
    [ActionLayout(typeof (MySampleRibbonGroup), DisplayType = DisplayType.Normal)]
    public class MyNormalSizeAction : AbstractViewControllerAction<ProjectsController>
    {
        protected override void Execute()
        {
            MessageBox.Show(string.Format("There are(is) {0} visible project(s) in the projects list",
                                          Controller.GetProjects().Count()));
        }
    }

    [Action("MyTopNormalSizeAction")]
    [ActionLayout(typeof (MySampleRibbonGroup), 9, DisplayType.Normal)]
    public class MyTopAction : AbstractViewControllerAction<EditorController>
    {
        protected override void Execute()
        {
            MessageBox.Show(string.Format("There are(is) {0} document(s) opened in the editor",
                                          Controller.GetDocuments().Count()));
        }
    }

    #endregion

}
