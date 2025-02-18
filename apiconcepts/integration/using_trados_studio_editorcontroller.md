Using Var:ProductName EditorController
=====
TranslationStudioAutomation provides support for third-party developers to access the Var:ProductName editor view and perform editor operations.

Creating a custom document list inside a viewpart that belongs to the editor view
----
The following sample demonstrates how to create a custom documents list built by using the [EditorController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml) and add it to the editor view as a custom viewpart.

The custom list features:

* Display the following columns:
    * The document file name
    * Number of segments to be translated
    * Source language
    * Target language

* Activating the document list items will active the document in the editor too.
* The active document will always be displayed as selected in the list.

Start by implementing the windows form user control that will fill the content of the viewpart and is implementing the list view.
# [C#](#tab/tabid-1)
```cs
private void InitializeDocumentListTab()
{
    _editorController.Opened += (s, e) => RepopulateDocumentList();
    _editorController.Closed += (s, e) => RepopulateDocumentList();
    _editorController.ActiveDocumentChanged += (s, e) => ActiveDocument = e.Document;
}

private void RepopulateDocumentList()
{
    DocumentsList.Items.Clear();
    foreach (Document document in _editorController.GetDocuments())
    {
        string documentName = document.Files.Count() > 1 ? "Multiple merged files" : document.Files.First().Name;
        ListViewItem item = DocumentsList.Items.Add(documentName);
        item.SubItems.Add(document.SegmentPairs.Count().ToString());
        item.Tag = document;
        item.SubItems.Add(document.Project.GetProjectInfo().SourceLanguage.DisplayName);
        item.SubItems.Add(document.Project.GetProjectInfo().TargetLanguages[0].DisplayName);
    }

    ActiveDocument = _editorController.ActiveDocument;
}

private void OpenUsingStudioActionButton_Click(object sender, EventArgs e)
{
    SdlTradosStudio.Application.ExecuteAction<OpenDocumentAction>();
}

private void DocumentsList_ItemActivate(object sender, EventArgs e)
{
    _editorController.Activate(DocumentsList.SelectedItems[0].Tag as Document);

    //keep the focus on the list items.
    _editorController.Activate();
    DocumentsList.Focus();
}

private Document ActiveDocument
{
    set
    {
        for (var i = 0; i < DocumentsList.Items.Count; i++)
        {
            var item = DocumentsList.Items[i];
            item.Selected = item.Tag as Document == value;
        }
    }
}
```
****

Finally, integrate the new viewpart to the editor view.
# [C#](#tab/tabid-2)
```cs
using System;
using System.Linq;
using System.Windows.Forms;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.TranslationStudioAutomation.IntegrationApi;

namespace EditorOperations.Sample
{
    [ViewPart(
        Id = "MyEditorViewPart", 
        Name = "My Editor View Part", 
        Description = "Integrationg a view part inside the editor view"        
        )]
    [ViewPartLayout(typeof(EditorController), Dock = DockType.Bottom)]
    class MyEditorViewPart : AbstractViewPartController
    {
        protected override Control GetContentControl()
        {
            return _control.Value;
        }

        protected override void Initialize()
        {                        
        }

        private static readonly Lazy<MyEditorViewPartControl> _control = new Lazy<MyEditorViewPartControl>(() => new MyEditorViewPartControl());       
    }
}
```
****

Tracking all editor document events and display them inside a list view.
---------
The following sample demonstrates how to bind to all [EditorController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml) related events and add them into a tab as an events list view.
# [C#](#tab/tabid-3)
```cs
private void InitializeTrackingEventsTab()
{
    _editorController.ActiveDocumentChanged +=
        (sender, args) =>
        AddListViewEvent("Active document changed", args.Document != null ? args.Document.Files.First().Name : string.Empty);

    _editorController.Saving += (sender, args) => AddListViewEvent("Document saving", args.Document.Files.First().Name);
    _editorController.Saved += (sender, args) => AddListViewEvent("Document saved", args.Document.Files.First().Name);
    _editorController.SaveFailed += (sender, args) => AddListViewEvent("Document save failed", args.Document.Files.First().Name);

    _editorController.Closing += (sender, args) =>
        AddListViewEvent("Document closing", args.Document != null ? args.Document.Files.First().Name : string.Empty);
    _editorController.Closed += (sender, args) =>
        AddListViewEvent("Document closed", args.Document != null ? args.Document.Files.First().Name : string.Empty);
    _editorController.Opening += (sender, args) =>
        AddListViewEvent("Document opening", args.Document != null ? args.Document.Files.First().Name : string.Empty);
    _editorController.Opened += (sender, args) =>
    {
        //bind document events.
        InitializeDocumentTrackingEvents(args.Document);

        AddListViewEvent("Document opened",
                         args.Document != null
                             ? args.Document.Files.First().Name
                             : string.Empty);
    };
}

private void InitializeDocumentTrackingEvents(Document doc)
{
    doc.ActiveSegmentChanged += (o, eventArgs) => AddListViewEvent("ActiveSegmentChanged");
    doc.ContentChanged +=
        (sender, args) => AddListViewEvent("Document changed", args.Segments.First().ToString());

    doc.ActiveFileChanged += (o, eventArgs) => AddListViewEvent("Active file changed");
    doc.ActiveFilePropertiesChanged += (o, eventArgs) => AddListViewEvent("Active file properties changed");


    doc.Selection.Changed +=
        (o, eventArgs) => AddListViewEvent("Document selection changed", doc.Selection.Current.ToString());
    doc.Selection.Source.Changed +=
        (o, eventArgs) =>
        AddListViewEvent("Source selection changed.", doc.Selection.Source.ToString());
    doc.Selection.Target.Changed +=
        (o, eventArgs) =>
        AddListViewEvent("Target selection changed.", doc.Selection.Target.ToString());



}

private void AddListViewEvent(string eventName, string eventMetadata = "")
{
    var item = new ListViewItem(EventsListView.Items.Count.ToString());
    item.SubItems.Add(eventName);
    item.SubItems.Add(eventMetadata);
    EventsListView.Items.Insert(0, item);
}
```
****

Performing operations over document selections
----
The following sample demonstrates how to retrieve and replace the current document selection.
# [C#](#tab/tabid-4)
```cs
private void InitializeSelectionsTab()
{
    _editorController.Opened += (sender, args) =>
    {
        args.Document.Selection.Changed +=
            (o, eventArgs) =>
            CurrentSelectionTextBox.Text = args.Document.Selection.Current.ToString();
    };
}

private void ReplaceSelectionButton_Click(object sender, EventArgs e)
{
    Document doc = _editorController.ActiveDocument;
    if (doc.Selection.Current is SourceSelection)
    {
        MessageBox.Show("The replace of a source selection is not available.");
        return;
    }

    doc.Selection.Target.Replace(ReplaceSelectionTextBox.Text, "Manual selection replacement");
    doc.Selection.Target.Collapse();
}
```
***
