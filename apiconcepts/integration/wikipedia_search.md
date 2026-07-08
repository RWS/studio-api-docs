# Wikipedia Search

Wikipedia Search extends this plug-in by demonstrating how a view part integrates into an existing Var:ProductName view. The sample integrates a view part into the editor view and allows users to search Wikipedia for selected text in the editor.

To use this feature, open a document in the editor and navigate to View Tab > Information group, then select Wikipedia. This displays the Wikipedia view part. Select text in the editor and click Search Wikipedia in the Add-Ins tab to populate the Wikipedia view part with search results.

## Wikipedia Search Action

This sample demonstrates how to create a Ribbon Group on the Add-Ins tab and add actions to it. The WikipediaSearchAction class shows how to obtain a reference to the EditorController in the Initialize method and use it in the Execute method.

# [C#](#tab/tabid-1)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.Desktop.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi.Presentation;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using System.Windows.Forms;
using Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations;

namespace StudioIntegrationApiSample
{
    [RibbonGroup("WikipediaRibbonGroup", "Wikipedia", ContextByType = typeof(EditorController))]
    [RibbonGroupLayout(LocationByType = typeof(TranslationStudioDefaultRibbonTabs.EditorAdvancedRibbonTabLocation))]
    class WikipediaRibbonGroup : AbstractRibbonGroup
    {
    }

    [Action("WikipediaSearchAction", typeof(EditorController), Name = "WikipediaSearchAction_Name", Description = "WikipediaSearchAction_Description", Icon = "Wikipedia_Icon")]
    [ActionLayout(typeof(WikipediaRibbonGroup), 1, DisplayType.Large)]
    [ActionLayout(typeof(TranslationStudioDefaultContextMenus.EditorDocumentContextMenuLocation), 1, DisplayType.Large)]
    [Shortcut(Keys.Alt | Keys.F8)]
    public class WikipediaSearchAction : AbstractViewControllerAction<EditorController>
    {
        public override void Initialize()
        {
        }

        protected override void Execute()
        {
            EditorController editorController = SdlTradosStudio.Application.GetController<EditorController>();
            WikipediaResultsViewPartController wikiPediaController = SdlTradosStudio.Application.GetController<WikipediaResultsViewPartController>();

            Document doc = editorController.ActiveDocument;

            if (doc == null)
            {
                return;
            }

            string selectedText = 
                doc.FocusedDocumentContent == FocusedDocumentContent.Source
                    ? doc.Selection.Source.ToString() 
                    : doc.Selection.Target.ToString();

            if (!String.IsNullOrEmpty(selectedText))
            {
                wikiPediaController.Lookup(selectedText);
                wikiPediaController.Show();
            }
        }
    }
}
```
***

## Wikipedia ResultsViewPart Controller

The Var:ProductName Integration API follows the MVC pattern. The ViewPart controller adds a new view part using the ViewPart attribute. This controller prepares the lookup URI that the UI control displays.

# [C#](#tab/tabid-2)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.Desktop.IntegrationApi.Interfaces;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using System.Web;

namespace StudioIntegrationApiSample
{
    [ViewPart(
       Id = "WikipediaResultsViewPart",
       Name = "Wikipedia",
       Description = "Wikipedia Results",
       Icon = "Wikipedia_Icon")]
    [ViewPartLayout(Dock = DockType.Bottom, LocationByType = typeof(EditorController))]
    class WikipediaResultsViewPartController : AbstractViewPartController
    {
        protected override IUIControl GetContentControl()
        {
            return _control.Value;
        }

        private readonly Lazy<WikipediaResultsViewPartControl> _control = new Lazy<WikipediaResultsViewPartControl>(() => new WikipediaResultsViewPartControl());

        public void Lookup(string query)
        {
            string url = String.Format("http://en.wikipedia.org/w/index.php?search={0}", HttpUtility.UrlEncode(query));

            _control.Value.Navigate(url);
        }

        protected override void Initialize()
        {
        }
    }
}
```
***

## Wikipedia Results ViewPart Control

This class renders the UI. The view receives a search URI from the controller and passes it to the web browser to search Wikipedia and display the results.

# [C#](#tab/tabid-3)
```cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi.Interfaces;

namespace StudioIntegrationApiSample
{
    public partial class WikipediaResultsViewPartControl : UserControl, IUIControl
    {

        public WikipediaResultsViewPartControl()
        {
            InitializeComponent();
        }

        public void Navigate(string url)
        {
            _webBrowser.Navigate(url);
        }
    }
}
```
***

## See Also

- [Reference Sample](reference_sample.md)
- [Project Creator](project_creator.md)
- [Content Connector](content_connector.md)
- [Integrating actions](integrating_actions.md)
- [Integrating ribbon groups](integrating_ribbon_groups.md)
- [Integrating viewparts](integrating_viewparts.md)
- [Integrating views](integrating_views.md)
