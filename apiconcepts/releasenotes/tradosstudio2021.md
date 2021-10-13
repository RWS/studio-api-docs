Release Notes for <Var:ProductNameWithEdition>
===================

Overall Change
---
<Var:ProductNameWithEdition> now targets <Var:DotNetVersion>. Please make sure to upgrade your plugins to <Var:DotNetVersion>.


Integration API
----

### Introduced enhancements that enables developers to add items in the spellcheck context menu from the editor.

There are two types of context menus that appear in the <Var:ProductName> editor when the user right clicks on content in the target segment:
* a general one, that contains the integrated in context actions from <Var:ProductName>. This one has an API that developers can add their own context menu actions.
* Then, there is the spellcheck context menu that contains the spellcheck suggestions, but also a handful of the integrated actions from <Var:ProductName>, such as TQA Accept/Reject, Concordance Search…

Added an new [AbstractLocation](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.Internal.AbstractLocation.yml) called [EditorDocumentSpellcheckContextMenuLocation](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Presentation.DefaultLocations.TranslationStudioDefaultContextMenus.EditorDocumentSpellcheckContextMenuLocation.yml) that can be used by the thirdparty developer when decorating the context menu action class. This abstract location class is basically used to identify the spellcheck context menu path that is eventually loaded/displayed in the editor view.

The following are examples of how the developer can now use the new `LocationByType` when decorating the context menu class with the [ActionLayout](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ActionLayoutAttribute.yml) attribute, so that the actions are loaded in either or both of the context menus in the <Var:ProductName> editor.

# [Declare a single action that is executed from both menus](#tab/tabid-1)
```cs
[Action(Id = "DemoEditorBothContextMenus_Id", Name = "DemoEditorBothContextMenus_Name",
   Icon = "DemoPlugin_Icon", ContextByType = typeof(EditorController))]
[ActionLayout(typeof(TranslationStudioDefaultContextMenus.EditorDocumentContextMenuLocation),
   0, DisplayType.Default, "", true)]
[ActionLayout(typeof(TranslationStudioDefaultContextMenus.EditorDocumentSpellcheckContextMenuLocation),
   0, DisplayType.Default, "", true)]
public class PluginEditorBothContextMenus : AbstractAction
{
    protected override void Execute() { }
}
```
***

# [Declare the actions individually](#tab/tabid-2)
```cs
[Action(Id = "DemoEditorSpellcheckContextMenu_Id", Name = "DemoEditorSpellcheckContextMenu_Name",
    Icon = "DemoPlugin_Icon", ContextByType = typeof(EditorController))]
[ActionLayout(typeof(TranslationStudioDefaultContextMenus.EditorDocumentSpellcheckContextMenuLocation),
    0, DisplayType.Default, "", true)]
public class PluginEditorSpellcheckContextMenu : AbstractAction
{
    protected override void Execute(){}
}
   
[Action(Id = "DemoEditorGeneralContextMenu_Id", Name = "DemoEditorGeneralContextMenu_Name",
    Icon = "DemoPlugin_Icon", ContextByType = typeof(EditorController))]
[ActionLayout(typeof(TranslationStudioDefaultContextMenus.EditorDocumentContextMenuLocation),
    0, DisplayType.Default, "", true)]
public class DemoEditorGeneralContextMenu : AbstractAction
{
    protected override void Execute() { }
}
```
***

### Exposed two new interfaces in the Integration API  that promotes a testable environment for developers integrating with the Document object.

Improved the Integration API by exposing two new interfaces ([IStudioDocument](../..//api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml) and [IDisplayFilterRowInfo](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.IDisplayFilterRowInfo.yml)) that permits developers to create integration tests or unit test with mocks against the following objects:
* [Document](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Document.yml)
* [DisplayFilterRowInfo](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.DisplayFilterRowInfo.yml)

> [!NOTE]
> 
> These changes will force developers to update existing plugins that have integrated with the Integration API (`Document` or `DisplayFilterRowInfo`).  They will need to either substitute the class references with that of the new interface or cast the concrete class references to the relevant interface.


### Increased code testability.

Substituted the 'CommentUtil' static utility class with a testable [ICommuntUtilityService](../../api/integration/Sdl.TranslationStudio.Api.Editor.ICommentUtilityService.yml) that can eventually be injected in the [Document](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Document.yml) class.

### Exposed methods for recovering the segment pair by providing the row number or Paragraph + Segment Id combination.

Allows recovery of the segment pair by providing the RowNumber as a parameter.

Currently there is no way for external developers to get the segment pair (or pairs) from the information provided in the active selection via API.

> [!NOTE]
> 
> The row number is made available through the [ContentSelectionInfo](../..//api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.ContentSelectionInfo.yml) from the selected content in the source or target segments.

# [Get Segment Pair Examples](#tab/tabid-3)
```cs
public void GetSegmentPairExample()
{
    var editorController = SdlTradosStudio.Application?.GetController<EditorController>();
    var document = editorController?.ActiveDocument;
 
    if (document == null)
    {
        return;
    }
 
    var segmentPair01 = GetSegmentPairFromRowNumber(document);
    var segmentPair02 = GetSegmentPairFromSegmentProperties(document);
}
 
// Get segment pair from the row number
private static ISegmentPair GetSegmentPairFromRowNumber(IStudioDocument document)
{
    var rowNumber = document.Selection.Target.From.RowNumber;
 
    return document.GetSegmentPair(rowNumber);
}
 
// Get the segment pair from the paragraph id and segment id
private static ISegmentPair GetSegmentPairFromSegmentProperties(IStudioDocument document)
{
    var activeSegmentPair = document.ActiveSegmentPair;
    var paragraphId = activeSegmentPair.GetParagraphUnitProperties().ParagraphUnitId.Id;
    var segmentId = activeSegmentPair.Properties.Id.Id;
 
    return document.GetSegmentPair(paragraphId, segmentId);    
}
```
***

### IUIControl interface for automatic detection of UI controls (WPF or WinForms)

<Var:ProductName> can automatically detects WPF controls and inject them into ViewParts without an explicit creation of an `ElementHost` control.
This is a breaking change as [AbstractViewController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewController.yml) and [AbstractViewPartController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewPartController.yml) expect for `GetControl()` methods to return [IUIControl](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IUIControl.yml)

Going forward all plugins will need to implement [IUIControl](../../api/integration/Sdl.Desktop.IntegrationApi.Interfaces.IUIControl.yml) interface on their specific user controls.

# [Sample user control implementing IUIControl](#tab/tabid-4)
```cs
public partial class DefaultExplorerBar : UserControl, IUIControl
{
       public DefaultExplorerBar()
       {
           InitializeComponent();
       }
}
```
***


# [Returning the control from an IViewController implementation](#tab/tabid-5)
```cs
IUIControl IViewController.GetExplorerBarControl()
{
        return GetExplorerBarControl();
}
```
***

> [!NOTE]
> 
> This change was necessary so we can move forward with the product development without a big impact on the plugins.

### Comments changes notifications
Exposed new event [CommentsChangedEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.CommentsChangedEvent.yml) for the user to be notified when comments on a document have changed

# [Subscribing to the event](#tab/tabid-6)
```cs
_eventAggregator = SdlTradosStudio.Application.GetService<IStudioEventAggregator>();
_eventAggregator.GetEvent<CommentsChangedEvent>().Subscribe(CommentsChanged);
 
private void CommentsChanged(CommentsChangedEvent e)
{
 
}
```
***

### Ability to open the Files View for a specific target language
Added a new event, [OpenProjectForSelectedLanguageEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.OpenProjectForSelectedLanguageEvent.yml) which allows users to open the Files View for a specific target language.

# [Publishing the event](#tab/tabid-7)
```cs
var eventAggregator = SdlTradosStudio.Application.GetService<IStudioEventAggregator>();
 
eventAggregator.Publish(new OpenProjectForSelectedLanguageEvent(activeProject,targetLanguage));
eventAggregator.Publish(new OpenProjectForSelectedLanguageEvent(activeProject));
```
***

### Ability to change the source content settings
Added new event, [ChangeSourceContentSettingsEvent](../..//api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.ChangeSourceContentSettingsEvent.yml) which allows user to change the source content settings for a specific project.

# [Publishing the event](#tab/tabid-8)
```cs
var eventAggregator = SdlTradosStudio.Application.GetService<IStudioEventAggregator>();
 
eventAggregator.Publish(new ChangeSourceContentSettingsEvent(activeProject,true,true));
```
***

### Segment split notification
Added new event, [SegmentSplitEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.SegmentSplitEvent.yml) which notifies the user when a segment was split.

# [Subscribing to the event](#tab/tabid-9)
```cs
var eventAggregator = SdlTradosStudio.Application.GetService<IStudioEventAggregator>();
 
eventAggregator.GetEvent<SegmentSplitEvent>().Subscribe(SegmentSplit);
 
private void SegmentSplit(SegmentSplitEvent e)
{
var document = e.Document;
var originalSegId = e.OriginalSegmentId;
var firstNewSegmentId = e.FirstNewSegmentId;
var secondNewSegmentId = e.SecondNewSegmentId;
var paragraphUnit = e.ParagraphUnitId;
}
```
***

### Segments merge notification
Added new event, [SegmentsMergedEvent](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Events.SegmentsMergedEvent.yml) which notifies the user when multiple segments are merged.

# [Subscribing to the event](#tab/tabid-10)
```cs
var eventAggregator = SdlTradosStudio.Application.GetService<IStudioEventAggregator>();
 
eventAggregator.GetEvent<SegmentsMergedEvent>().Subscribe(SegmentsMerged);
 
private void SegmentsMerged(SegmentsMergedEvent e)
{
var document = e.Document;
var paragraphUnit = e.ParagraphUnitId;
var newSegmentId = e.NewSegmentId;
var oldSegmentIds = e.OldSegmentIds;
}
```
***

### Added ability to hide a view part
Exposed [Hide()](../..//api/integration/Sdl.Desktop.IntegrationApi.AbstractViewPartController.yml#Sdl_Desktop_IntegrationApi_AbstractViewPartController_Hide) method on [AbstractViewPartController](../..//api/integration/Sdl.Desktop.IntegrationApi.AbstractViewPartController.yml).

### Exposed <Var:ProductName> Versioning Details

The API now offers the third party developers a quick way to learn more about the product version information, along with certain specific folder locations that depend on that version.

# [Sdl.Versioning public API Code Sample](#tab/tabid-11)
```cs
private StudioVersionService _studioVersionService;
 
public MainWindow()
{
    _studioVersionService = new StudioVersionService();
 
    InitializeComponent();
}
 
// Set different versions
private void Button_Click(object sender, RoutedEventArgs e)
{
    var studioVersion = _studioVersionService.GetStudioVersion();
    if (studioVersion != null)
    {
        Edition_TxtBox.Text = studioVersion.Edition;
        ExecutableVersion_TxtBox.Text = studioVersion.ExecutableVersion?.ToString();
        InstalledPath_TxtBox.Text = studioVersion.InstallPath;
        PublicVersion_TxtBox.Text = studioVersion.PublicVersion;
        StudioShortVersion_TxtBox.Text = studioVersion.ShortVersion;
        StudioDocumentsFolderName_TxtBox.Text = studioVersion.StudioDocumentsFolderName;
        Version_TxtBox.Text = studioVersion.Version;
    }
}
 
// See all the installed Studio versions
private void Button_Click_1(object sender, RoutedEventArgs e)
{
    var installedVersions = _studioVersionService.GetInstalledStudioVersions();
    var installedVersionsList = new List<string>();
    if (installedVersions != null)
    {
        foreach (var installedVersion in installedVersions)
        {
            installedVersionsList.Add(installedVersion.PublicVersion);
        }
    }
 
    InstalledStudioVersions_ListBox.ItemsSource = installedVersionsList;
}
```
***

### Exposed additional UI controllers
Exposed controllers for Welcome View, Translation Memories View and Reports View that will allow 3rd party developers to inject actions in the ribbon.

# [Adding actions through the new controllers](#tab/tabid-12)
```cs
[RibbonGroup("Legacy.WelcomeRibbonGroup", Name = "Ribbon_GroupName",
ContextByType = typeof(WelcomeViewController))]
[RibbonGroupLayout(LocationByType = typeof(TranslationStudioDefaultRibbonTabs.HomeRibbonTabLocation))]
public class LegacyWelcomeViewRibbonGroup : AbstractRibbonGroup
{
}

[RibbonGroup("Legacy.TranslationMemoriesRibbonGroup", Name = "Ribbon_GroupName",
ContextByType = typeof(TranslationMemoriesViewController))]
[RibbonGroupLayout(LocationByType = typeof(TranslationStudioDefaultRibbonTabs.HomeRibbonTabLocation))]
public class LegacyTranslationMemoriesViewRibbonGroup : AbstractRibbonGroup
{
}
```
***

Project Automation API
----

### Ability to declare project origin and change icon in Projects View
[ProjectInfo](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml) now has two new properties that allows controlling the project origin and the icon show in Projects View.

# [Add ProjectOrigin and IconPath on new project](#tab/tabid-13)
```cs
public void Execute()
{
    var iconPath = GetIconPath();
 
    var projectInfo = new ProjectInfo
    {
        Name = "ProjectOriginIconSample",
        SourceLanguage = new Language(CultureInfo.GetCultureInfo("en-US")), //import Sdl.core.globalization.dll
        TargetLanguages = new[]
        {
            new Language(CultureInfo.GetCultureInfo("de-DE")),
            new Language(CultureInfo.GetCultureInfo("fr-FR"))
        },
        // file path where you want to save the project
        LocalProjectFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $@"{Versioning.Versions.StudioDocumentsFolderName}\Projects\ProjectOriginIconSample"),
         
        // update with the needed ProjectOrigin which will be displayed at Project Type column, in Projects controller
        ProjectOrigin = "Star Transit",
        // update with the needed icon path, the phisical path is needed, so Studio will know from where to load the icon
        IconPath = iconPath
        //IconPath = @"C:\Repo_GitHubSdlCommunity\Code samples\AddProjectOrigin_IconPath\Resources\icon.ico"
    };
 
    //import Sdl.ProjectAutomation.FileBased
    var fileBasedProject = new FileBasedProject(projectInfo);
 
    // HERE YOU NEED TO ADD THE PATH FOR FILES YOU WANT TO INCLUDE IN YOUT PROJECT
    var filesPath = new[] { @"C:\TradosStudio_Plugins\TestDocuments\Test.docx" };
 
    //add files to project
    var projectFiles = fileBasedProject.AddFiles(filesPath);
    //we need to run automatic task to create the project in studio
    fileBasedProject.RunAutomaticTask(projectFiles.GetIds(), AutomaticTaskTemplateIds.Scan);
    var taskSequence = fileBasedProject.RunDefaultTaskSequence(projectFiles.GetIds());
    if (taskSequence.Status == TaskStatus.Completed)
    {
        //project was created succesfully
    }
    else
    {
        //here we'll see the erors
    }
    fileBasedProject.Save();
}

// Write the icon to the Plugins\Unpacked folder and use the icon's path from that folder in order to set the project's IconPath
public string GetIconPath()
{
    var assemblyPath = Assembly.GetExecutingAssembly().Location;
    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    var targetPath = assemblyPath.Remove(assemblyPath.Length - assemblyName.Length - 4);
    targetPath = targetPath + "icon.ico";
 
    using (var fs = new FileStream(targetPath, FileMode.Create))
    {
        PluginResources.icon.Save(fs);
    }
    return targetPath;
}
```
***

### Additional information is made available on [ProjectInfo](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml)

Exposed the `Status` and `ProjectType` properties within the [ProjectInfo](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml) class.

# [Sample code to access project status and type](#tab/tabid-14)
```cs
[ApplicationInitializer]
class MyCustomTradosStudio : IApplicationInitializer
{
    public void Execute()
    {
        var projectsController = GetProjectController();
        var allProjects = projectsController?.GetAllProjects();
        if (allProjects != null)
        {
            foreach (var proj in allProjects)
            {
                // Retrieve the needed information from the project
                var projectInfo = proj.GetProjectInfo();
                var status = projectInfo?.Status; // the current status of the project
                var publicationStatus = projectInfo?.PublicationStatus; // the publication status of the project
                var projectType = projectInfo?.ProjectType; // the type of the project
            }
        }
    }
 
    private ProjectsController GetProjectController()
    {
        return SdlTradosStudio.Application.GetController<ProjectsController>();
    }
}
```
***

Other Changes
-----

### Removed legacy assembly from the API
The following assembly used for SLDX TMs have been removed from the Public API: <PublicAssembly name="Sdl.Sdlx.S42tmc" version="2010.0" />.

