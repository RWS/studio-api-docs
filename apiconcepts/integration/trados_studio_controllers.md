# Var:ProductName Controllers

The Var:ProductName Integration API enables third-party developers to extend, customize, and integrate custom functionalities into the Var:ProductName UI application. You can also gain control and perform various operations.

This topic covers specific Var:ProductName UI integrations and operations.

## MVC and MVVM Patterns

Based on MVC and MVVM patterns, the Var:ProductName Integration API provides view and viewpart controllers to enable third-party developers to integrate custom UI.

For information on how to create and integrate views and viewparts, see:

- [Creating views](creating_views.md)
- [Creating viewparts](creating_viewparts.md)

## Obtaining Controller Instances

The following example shows how to obtain the view or viewpart controllers of the Var:ProductName application:

# [C#](#tab/tabid-1)
```cs
private EditorController GetEditorController()
{
    return SdlTradosStudio.Application.GetController<EditorController>();
}
```
***

The following topics discuss each Var:ProductName controller in detail.
