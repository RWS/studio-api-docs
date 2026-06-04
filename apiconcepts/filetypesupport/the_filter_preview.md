# The filter preview

Var:ProductName can display the currently opened document in its native format, even though the editor works with the bilingual SDLXliff file. For example, when users open a Microsoft Word document in Var:ProductName, they can use preview to see how the source or target document will appear in Word.

## Preview types

Var:ProductName provides two main preview types:

- External Studio preview
- Internal Studio preview

Both preview types support the following modes:

- **Source**: displays the source content in its native format.
- **Target**: displays the target content in its native format.
- **Bilingual**: displays both source and target content in the native format.

>[!NOTE]
>
> The bilingual mode may not suit every supported file type. Some file filters do not implement it.


## External preview

External preview generates a native-format file that contains the source or target content, depending on the selected preview mode. It then opens that file in the native application or in another configured application.

You can configure a different generator for each external preview mode. The file type configuration file (`*.sdlfiletype`) stores all preview settings. For more information, see [Implementing an External File Preview](implementing_an_external_file_preview.md).

## Internal preview

Internal preview generates a native-format file that a control hosted inside Var:ProductName can display. You can use one of the predefined controls in `Sdl.FileTypeSupport.Framework.PreviewControls`, or you can provide a custom control that derives from [ISingleFilePreviewControl](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISingleFilePreviewControl.yml) together with [IPreviewUpdatedViaRefresh](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IPreviewUpdatedViaRefresh.yml) or [INavigablePreview](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.INavigablePreview.yml).

For more information about internal preview implementation, see [Implement an Internal Preview](internal_preview_introduction.md).

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
