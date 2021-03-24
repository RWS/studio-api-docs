Introduction
====
The  Trados Studio Integration API enables third-party developers to extend, customize and integrate their own functionalities inside Trados Studio UI application and also gain control and perform operations.

This topic covers specific Trados Studio UI integrations and operations.

Based on MVC and MVVM patterns, the Trados Studio Integration API provides view and viewpart controllers to enable third-party developers to integrate their custom UI.

For information on how to create and integrate views and viewparts, see the topics:

[Creating views](integration/creating_views.md)

[Creating viewparts](integration/creating_viewparts.md)

Below is a sample on how you can obtain the view or viewpart controllers of the Trados Studio application.

```
private EditorController GetEditorController()
{
    return SdlTradosStudio.Application.GetController<EditorController>();
}
```

The next topics are discussing in-depth about each of the Trados Studio controllers.