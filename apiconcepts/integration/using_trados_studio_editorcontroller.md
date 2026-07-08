# Using Var:ProductName EditorController

The TranslationStudioAutomation API enables you to access the Var:ProductName editor view and perform editor operations.

## Create a custom document list in a viewpart

This sample demonstrates how to create a custom document list using the [EditorController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml) and add it to the editor view as a custom viewpart.

The custom list includes:

* Four columns:
    * Document file name
    * Number of segments to translate
    * Source language
    * Target language

* Selection synchronization: activating items in the custom list activates the document in the editor
* Active document display: the currently active document appears selected in the list

### Step 1: Implement the user control

Create a Windows Forms user control to display the document list:
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

### Step 2: Integrate the viewpart into the editor view

Register the viewpart with the editor view:
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

## Track editor document events

This sample demonstrates how to subscribe to all [EditorController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml) events and add them to a list view for tracking.
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

## Perform operations on document selections

This sample demonstrates how to retrieve and update the current document selection.
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