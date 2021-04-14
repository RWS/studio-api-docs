Editor controller
===
<Var:ProductName> Integration API provides support for third-party developers to implement editor functionalities for the <Var:ProductName> application.

Editor controller
----
The [EditorController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml) enables the third-party developer to integrate custom UI functionalities inside <Var:ProductName> editor view and perform operations on translatable documents.

For a sample on how to use it, see the following sample: [Using <Var:ProductName> EditorController](using_trados_studio_editorcontroller.md)

Enhance <Var:ProductName> editor view using EditorController
---
The [EditorController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml) provide support for integrating custom UI inside the <Var:ProductName> editor view.

* Integrating viewparts (see [Integrating viewparts](integrating_viewparts.md))
* Integrating menus
* Integrating context menus

Operations on translatable documents
----
Based on MVVM pattern, the [EditorController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml) provides support for translatable document operations.

* Open, save, close, activate document operations using [EditorController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml)
* Operations on [Document](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Document.yml) (see also [Bilingual API - Overview](bilingual_api.md))
* Operations on **DocumentView**