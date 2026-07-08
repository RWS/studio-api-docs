# Integrating Actions

The Desktop Integration API allows third-party developers to integrate custom actions into Var:ProductName desktop applications.

## Global Actions

The following example demonstrates how to create an action with general purpose and integrate it into a custom ribbon group (see [Integrating ribbon groups](integrating_ribbon_groups.md)).
# [C#](#tab/tabid-1)
```cs
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
```
***

## Controller-Specific Actions

The following example demonstrates how to create an action specific to a controller and integrate it into a custom ribbon group (see [Integrating ribbon groups](integrating_ribbon_groups.md)).

# [C#](#tab/tabid-2)
```cs
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
```
***
