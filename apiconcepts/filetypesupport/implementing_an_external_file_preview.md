
# Implementing an External File Preview
 
> [!WARNING]
> **Breaking change in Var:ProductName 2026**
>
> The `BuildPreviewApplication` method on `IFileTypeComponentBuilder` is
> **deprecated and no longer called by Var:ProductName**. The
> `GenericExternalPreviewApplication` class and the
> `Sdl.FileTypeSupport.Framework.PreviewControls` assembly reference are no
> longer needed. Remove them from any file type that targets Var:ProductName 2026 or
> later.
>
> The `PreviewSet` registration pattern shown below still applies, but the
> Preview ID must now be one of the **built-in IDs** recognised by Var:ProductName.
> See [Preview API changes in Trados Var:ProductName 2026](preview_api_changes.md)
> for the full list of built-in IDs and a migration checklist.
 
## Overview
 
Var:ProductName can open a preview of the translated file in an external
application. The application that is launched is determined automatically by
the file extension registered in the operating system — the file type plugin
no longer specifies a path to an executable.
 
The application logic needed to generate the preview file is already
provided by your file writer class. All you need to do is register the
external preview in the File Type Component Builder, using the built-in
`ExternalPreview` ID.
 
## Register the external preview
 
Add the following code to your File Type Component Builder to register an
external preview for both source and target:
 
```csharp
IPreviewSet externalPreviewSet = previewFactory.CreatePreviewSet();
externalPreviewSet.Id   = new PreviewSetId("ExternalPreview");
externalPreviewSet.Name = new LocalizableString(Resources.ExternalPreview_Name);
 
IApplicationPreviewType sourceAppPreviewType =
    previewFactory.CreatePreviewType<IApplicationPreviewType>() as IApplicationPreviewType;
if (sourceAppPreviewType != null)
{
    sourceAppPreviewType.SourceGeneratorId = new GeneratorId("DefaultPreview");
    sourceAppPreviewType.SingleFilePreviewApplicationId =
        new PreviewApplicationId("ExternalPreview");
    externalPreviewSet.Source = sourceAppPreviewType;
}
 
IApplicationPreviewType targetAppPreviewType =
    previewFactory.CreatePreviewType<IApplicationPreviewType>() as IApplicationPreviewType;
if (targetAppPreviewType != null)
{
    targetAppPreviewType.TargetGeneratorId = new GeneratorId("DefaultPreview");
    targetAppPreviewType.SingleFilePreviewApplicationId =
        new PreviewApplicationId("ExternalPreview");
    externalPreviewSet.Target = targetAppPreviewType;
}
 
previewFactory.GetPreviewSets(null).Add(externalPreviewSet);
```
 
`ExternalPreview` is a built-in Var:ProductName preview ID. When a user triggers the
preview, Var:ProductName opens the generated file in the application registered in
the OS for that file extension — the same behaviour that the old
`ApplicationPath = ""` shortcut produced, now as the only and default
behaviour.
 
> [!NOTE]
> You do not need to add a reference to
> `Sdl.FileTypeSupport.Framework.PreviewControls`, and you do not need to
> implement `BuildPreviewApplication`. Both are deprecated in Var:ProductName 2026.
 
## See also
 
- [Preview API changes in Trados Studio 2026](preview_api_changes.md)
- [Adding a Preview UI Control](adding_a_preview_ui_control.md)
- [Implementing the File Writer](implementing_the_file_writer.md)
