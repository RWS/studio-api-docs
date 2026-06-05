# Integrating View Parts

The Desktop Integration API allows third-party developers to integrate custom UI view parts into Var:ProductName desktop applications.

View parts allow you to build a view from multiple parts instead of a single content area. A view typically has one main content view part (with `Dock` set to `Fill`) and one or more additional view parts.

You can add view parts to a custom view or to existing Var:ProductName views such as [ProjectsController](projects_controller.md), [FilesController](files_controller.md), or [EditorController](editor_controller.md).

## Integrating View Parts in a Custom View

The following example demonstrates how to create a view built from view parts in a Var:ProductName application.

### Step 1: Create a View That Allows View Parts

# [C#](#tab/tabid-1)
[!code-csharp[MyViewWithParts](code_samples/MyViewWithParts.cs#L1-L25)]
***

### Step 2: Create the Main View Part Content

To define the main content view part, set the `Dock` property of the [ViewPartLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartLayoutAttribute.yml) attribute to `Fill`.

# [C#](#tab/tabid-2)
[!code-csharp[MyCustomViewPartContent](code_samples/MyCustomViewPartContent.cs#L1-L29)]
***

### Step 3: Create Additional View Parts

Define other view parts and order them by setting the `ZIndex` property.

> [!NOTE]
> 
> The `ZIndex` ordering applies only to integrated view parts and displays after Var:ProductName's default view parts. Higher `ZIndex` values appear on the left or top; lower values appear on the right or bottom.

# [C#](#tab/tabid-3)
[!code-csharp[MyCustomViewPart1](code_samples/MyCustomViewPart1.cs#L1-L30)]
***


# [C#](#tab/tabid-4)
[!code-csharp[MyCustomViewPart2](code_samples/MyCustomViewPart2.cs#L1-L29)]
***

# [C#](#tab/tabid-4)
[!code-csharp[MyCustomViewPart3](code_samples/MyCustomViewPart3.cs#L1-L29)]
***

## Integrating View Parts in Existing Var:ProductName Views

The following example demonstrates how to create a view part and integrate it into the Var:ProductName Projects View.

# [C#](#tab/tabid-5)
[!code-csharp[MyProjectViewPart](code_samples/MyProjectViewPart.cs#L1-L44)]
***

## Reference

* [AbstractViewPartController](../../api/integration/Sdl.Desktop.IntegrationApi.AbstractViewPartController.yml)
* [ViewPartAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartAttribute.yml)
* [ViewPartLayoutAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.ViewPartLayoutAttribute.yml)
