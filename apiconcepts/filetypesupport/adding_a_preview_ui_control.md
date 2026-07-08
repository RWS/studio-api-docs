
# Adding a Preview UI Control
 
> [!WARNING]
> **Breaking change in Var:ProductName 2026**
>
> File types no longer provide their own preview UI controls. The methods
> `IFileTypeDefinition.BuildPreviewControl` and
> `IFileTypeComponentBuilder.BuildPreviewControl` are **deprecated and no
> longer called by Var:ProductName**. Custom `UserControl` classes (such as
> `InternalPreviewControl.cs`) and their companion controller classes (such
> as `InternalPreviewController.cs`) are not needed for file types targeting
> Var:ProductName 2026 or later.
>
> If you maintain an existing file type that implements these methods, they
> will be silently ignored at runtime. See [Preview API changes in Trados
> Var:ProductName 2026](preview_api_changes_quantum.md) for the migration checklist.
 
## Overview
 
Starting with Var:ProductName 2026, Var:ProductName ships a set of
**built-in preview UIs**. Your file type no longer builds or registers a
custom control. Instead, it declares which built-in UI it wants by
referencing a recognised **Preview ID** in its `PreviewSet` definition, and
Var:ProductName handles the rest.
 
Your filter is still responsible for generating the preview *content* — the
output file written to the temporary folder. How that content is displayed
is now entirely managed by Var:ProductName.
 
## Built-in Preview IDs
 
Use one of the following IDs when configuring your `PreviewSet`:
 
| Preview ID | Description |
|---|---|
| `HtmlSingleFilePreview` | Renders an HTML file. Can preview source or target individually. |
| `HtmlSideBySidePreview` | Renders source and target side by side. Content must be HTML. |
| `ExternalPreview` | Opens the preview file in the application registered in the OS for that file extension. |
| `ExternalHtml` | Opens the preview file in the default browser. |
 
> [!NOTE]
> Using a preview UI that is not in this list is not yet supported in Var:ProductName
> 2026. Third-party preview controls cannot be registered.
 
## Register a built-in preview in the File Type Component Builder
 
The following example registers `HtmlSingleFilePreview` for both source and
target:
 
```csharp
IPreviewSet internalPreviewSet = previewFactory.CreatePreviewSet();
internalPreviewSet.Id   = new PreviewSetId("InternalPreview");
internalPreviewSet.Name = new LocalizableString(Resources.InternalPreview_Name);
 
// Source preview
IControlPreviewType sourcePreviewType =
    previewFactory.CreatePreviewType<IControlPreviewType>() as IControlPreviewType;
if (sourcePreviewType != null)
{
    sourcePreviewType.SourceGeneratorId          = new GeneratorId("PreviewGenerator");
    sourcePreviewType.SingleFilePreviewControlId =
        new PreviewControlId("HtmlSingleFilePreview");
    internalPreviewSet.Source = sourcePreviewType;
}
 
// Target preview
IControlPreviewType targetPreviewType =
    previewFactory.CreatePreviewType<IControlPreviewType>() as IControlPreviewType;
if (targetPreviewType != null)
{
    targetPreviewType.TargetGeneratorId          = new GeneratorId("PreviewGenerator");
    targetPreviewType.SingleFilePreviewControlId =
        new PreviewControlId("HtmlSingleFilePreview");
    internalPreviewSet.Target = targetPreviewType;
}
 
previewFactory.GetPreviewSets(null).Add(internalPreviewSet);
```
 
Replace `"HtmlSingleFilePreview"` with whichever built-in ID best matches
the preview content your generator produces.

## See also
 
- [Preview API changes in Var:ProductName 2026](preview_api_changes.md)
- [Implementing an External File Preview](implementing_an_external_file_preview.md)
- [Modifying the File Type Component Builder](static_modifying_the_file_type_component_builder.md)
- [Studio Preview UI](https://rws-dev.atlassian.net/wiki/spaces/LTSTUDIO/pages/1989869850)
  (internal Confluence reference for built-in preview UI components)

 
