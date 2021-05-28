Wikipedia Search
==

Wikipedia Search is a useful addition to this plug-in. It illustrates how a view part can be integrated into an existing view in Studio, in this case a view part which integrates into the editor view and allows a Wikipedia search to be carried out on selected text in the editor itself. To activate this feature, open a document in the editor, navigate to View Tab->Information group and select Wikipedia. This will display the Wikipedia view part. Selecting text in the editor and clicking Search Wikipedia in the Add-Ins tab will populate the Wikipedia view part with the results of the search.

Wikipedia Search Action
--

This sample illustrates how a Ribbon Group can be created on the Add-Ins tab and also how actions can be added to it. In the WikipediaSearchAction class the Initialize method shows how a reference to the EditorController can be obtained. The Execute method shows how this reference can be used.

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

Wikipedia ResultsViewPart Controller
--

The <Var:ProductName> Integration API is built in accordance with the MVC pattern. The ViewPart controller is used to add a new view part using the ViewPart attribute. This controller is also responsible for preparing the lookup URI that will be passed to the UI control for display purposes.

# [C#](#tab/tabid-2)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
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
        protected override System.Windows.Forms.Control GetContentControl()
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

Wikipedia Results ViewPart Control
--

This class is responsible for rendering the UI. The view relies on the controller described above to obtain a search URI and passes it to the web browser in order to search Wikipedia and display the results.

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

namespace StudioIntegrationApiSample
{
    public partial class WikipediaResultsViewPartControl : UserControl
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

See Also
--



[Reference Sample](reference_sample.md)

[Project Creator](project_creator.md)

[Content Connector](content_connector.md)

[Integrating actions](integrating_actions.md)

[Integrating ribbon groups](integrating_ribbon_groups.md)

[Integrating viewparts](integrating_viewparts.md)

[Integrating views](integrating_views.md)
