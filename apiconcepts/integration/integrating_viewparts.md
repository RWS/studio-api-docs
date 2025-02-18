Integrating viewparts
=====

Desktop Integration API provides support for third-party developers to integrate UI view parts inside the Var:ProductName desktop applications.

Integrating view parts implies changes, in the way that a view is no longer having one content, but is built by one too many viewparts having at least one main centered viewpart, which is filling its content.

The viewparts can be added to be part of an integrated view (see the sample regarding the integration of a view) or to an existing view (see [ProjectsController](projects_controller.md), [FilesController](files_controller.md), [EditorController](editor_controller.md)).

Integrating view parts inside a custom view
-----
The following example demonstrates how to create a view built by viewparts into a Var:ProductName application.

First, create and integrate a view which allows viewparts:

# [C#](#tab/tabid-1)
[!code-csharp[MyViewWithParts](code_samples/MyViewWithParts.cs#L1-L25)]
***

Create the main viewpart content of the view.

To define a main content viewpart, set the `Dock` property of the [ViewPartLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartLayoutAttribute.yml) attribute to `Fill`. 

# [C#](#tab/tabid-2)
[!code-csharp[MyCustomViewPartContent](code_samples/MyCustomViewPartContent.cs#L1-L29)]
***

Define other viewparts and order their relevance by setting the `ZIndex` property.

> [!NOTE]
> 
> The ZIndex ordering is performed only for the intergrated viewparts and displayed only after Var:ProductName viewparts and acts as importance from left to right or top to bottom. The highest ZIndex value is in the left or top and the lowest value is in the right or bottom.

# [C#](#tab/tabid-3)
[!code-csharp[MyCustomViewPart1](code_samples/MyCustomViewPart1.cs#L1-L30)]
***


# [C#](#tab/tabid-4)
[!code-csharp[MyCustomViewPart2](code_samples/MyCustomViewPart2.cs#L1-L29)]
***

# [C#](#tab/tabid-4)
[!code-csharp[MyCustomViewPart3](code_samples/MyCustomViewPart3.cs#L1-L29)]
***

Integrating viewparts inside an existing Var:ProductName view
------
The following example demonstrates how to create a viewpart and integrate it into the Var:ProductName Projects View.

# [C#](#tab/tabid-5)
[!code-csharp[MyProjectViewPart](code_samples/MyProjectViewPart.cs#L1-L44)]
***

See Also
--

**Reference**

[AbstractViewPartController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewPartController.yml)

[ViewPartAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartAttribute.yml)

[ViewPartLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartLayoutAttribute.yml)
