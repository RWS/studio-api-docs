Introduction
====
The  <Var:ProductName> Integration API enables third-party developers to extend, customize and integrate their own functionalities inside <Var:ProductName> UI application and also gain control and perform operations.

This topic covers specific <Var:ProductName> UI integrations and operations.

Based on MVC and MVVM patterns, the <Var:ProductName> Integration API provides view and viewpart controllers to enable third-party developers to integrate their custom UI.

For information on how to create and integrate views and viewparts, see the topics:

[Creating views](creating_views.md)

[Creating viewparts](creating_viewparts.md)

Below is a sample on how you can obtain the view or viewpart controllers of the <Var:ProductName> application.

```cs
private EditorController GetEditorController()
{
    return SdlTradosStudio.Application.GetController<EditorController>();
}
```

The next topics are discussing in-depth about each of the <Var:ProductName> controllers.