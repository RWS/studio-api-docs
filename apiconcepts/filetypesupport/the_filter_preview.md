The Filter Preview
==

The preview functionality in the <Var:ProductName> allows you to display the currently opened document (which is available in the bilingual SDLXliff format) in native format. For example, if user has Microsoft Word document opened in <Var:ProductName>, he/she can use the preview feature to see what the target document will look like when opened in Microsoft Word proper.

**<Var:ProductName> offers the following types of preview:**
--
In <Var:ProductName> two main preview types are available:

* External Studio preview
* Internal Studio preview
Both preview types offer the three following optiosn:

* Source: the preview displays the source content in its native format
* Target: the preview displays the target content in its native format
* Bilingual: both source and target content is displayed in the native format

>[!NOTE]
>
>The bilingual mode may not be practical for all supported file types and in some file filters this mode is not implemented.

External Preview
--

The external preview is reponsible for generating of native file format. containing the source or the target content (depends on type of the selected preview) and opening this file in the native or other (configurable option) application.

For each mode of the external preview we can set different generator which will generate expected format of the native file. All preview configuration settings are save in the file type configuration (*.sdlfiletype) file. For more details about preview configuration see [Implementing an External File Preview](implementing_an_external_file_preview.md) topic.

Internal preview
-- 
Internal preview is responsible for generating of the native file in the format which can be displayed inside the control hosted in <Var:ProductName>. As hosted control, one of predefined controls in ```Sdl.FileTypeSupport.Framework.PreviewControls``` or the custom control which derives from [ISingleFilePreviewControl](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISingleFilePreviewControl.yml) and [IPreviewUpdatedViaRefresh](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IPreviewUpdatedViaRefresh.yml) or [INavigablePreview](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.INavigablePreview.yml) can be used.

For more details about the internal preview implementation see [Implement an Internal Preview](internal_preview_introduction.md) chapter.

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
