Integrating actions
=====

Desktop Integration API provides support for third-party developers to integrate actions inside the <Var:ProductName> desktop applications.

Integrating general actions
-----

The following example demonstrates how to create an action into the <Var:ProductName> application which has a general purpose and integrate it into a custom ribbon group (see: [Integrating ribbon groups](integrating_ribbon_groups.md)).

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

Integrating controller actions
-----
The following example demonstrates how to create an action specific to a controller and integrate it into a custom ribbon group (see: [Integrating ribbon groups](integrating_ribbon_groups.md).

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