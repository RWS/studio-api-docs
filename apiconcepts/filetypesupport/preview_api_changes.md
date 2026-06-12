
# Preview API changes in Var:ProductName 2026
 
Var:ProductName 2026 changes the way file type plugins integrate
with the preview subsystem. This page summarises what has changed, why, and
what you need to do if you maintain an existing file type plugin.
 
## What changed
 
Previously, each file type was responsible for supplying its own preview UI.
Plugins did this by implementing `BuildPreviewControl` or
`BuildPreviewApplication` on the File Type Component Builder and returning a
custom `UserControl` or a `GenericExternalPreviewApplication` instance.
 
Starting with Var:ProductName 2026, **Var:ProductName ships a set of built-in preview UIs**.
File types declare which built-in UI they want by placing a recognised
Preview ID in their `PreviewSet` definition. Var:ProductName maps that ID to the
corresponding control at runtime. Plugins no longer build, own, or register
preview UI components.
 
## Deprecated API surface
 
The following methods are **deprecated as of Var:ProductName 2026** and are no longer
called by the host application:
 
| Deprecated member | Previously used for |
|---|---|
| `IFileTypeDefinition.BuildPreviewControl` | Returning a custom internal preview `UserControl` |
| `IFileTypeDefinition.BuildPreviewApplication` | Returning a custom external preview application |
| `IFileTypeComponentBuilder.BuildPreviewControl` | Same as above, on the component builder |
| `IFileTypeComponentBuilder.BuildPreviewApplication` | Same as above, on the component builder |
 
Implementing these methods in a plugin targeting Var:ProductName 2026 is harmless —
they are ignored at runtime — but they should be removed to keep the
codebase clean.
 
## Built-in Preview IDs
 
Your `PreviewSet` must reference one of the following IDs. Var:ProductName maps each
ID to a built-in control:
 
| Preview ID | Description |
|---|---|
| `HtmlSingleFilePreview` | Renders an HTML file. Source or target individually. |
| `HtmlSideBySidePreview` | Renders source and target side by side. Content must be HTML. |
| `ExternalPreview` | Opens in an external application based on the OS file-extension registration. |
| `ExternalHtml` | Opens in the default browser. |
 
> [!NOTE]
> Using a Preview ID that is not in this list is not yet supported. Custom
> third-party preview UIs cannot be registered in Var:ProductName 2026.
 
## Migration checklist
 
Use this checklist when updating an existing file type plugin to target
Var:ProductName 2026.
 
### Internal (control-based) previews
 
- Delete your custom `UserControl` class (for example,
  `InternalPreviewControl.cs`).
- Delete your preview controller class (for example,
  `InternalPreviewController.cs`).
- Remove the `BuildPreviewControl` method from your File Type Component
  Builder.
- In your `PreviewSet` definition, replace any custom `PreviewControlId`
  value (for example, `"InternalNavigablePreview"`) with the appropriate
  built-in ID (`"HtmlSingleFilePreview"` or `"HtmlSideBySidePreview"`).
### External (application-based) previews
 
- Remove the `BuildPreviewApplication` method from your File Type
  Component Builder.
- Delete any `GenericExternalPreviewApplication` instantiation and its
  `ApplicationPath` assignment.
- Remove the assembly reference to
  `Sdl.FileTypeSupport.Framework.PreviewControls` if it is no longer used
  elsewhere.
- Confirm your `PreviewSet` uses `new PreviewApplicationId("ExternalPreview")`.
  The OS will determine which application opens the file; no executable path
  is required.
### What to keep
 
- Your **preview generator** (`IFileGenerator` implementation) is unchanged.
  The way preview *content* is produced has not changed — only the way it is
  *displayed*.
- The `PreviewSet` creation and registration pattern itself is unchanged.
  Only the IDs used inside it need to be updated.
## Further reading
 
- [Adding a Preview UI Control](adding_a_preview_ui_control.md)
- [Implementing an External File Preview](implementing_an_external_file_preview.md)
- [Studio Preview UI](https://rws-dev.atlassian.net/wiki/spaces/LTSTUDIO/pages/1989869850)
  (internal Confluence reference — authoritative technical detail on
  built-in preview UI components)
