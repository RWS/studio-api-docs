Segmentation Hints
==

The [SegmentationHint](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml) property is used to specify the required behavior of a tag placeholder or tag pair if it it appears on a segment boundary (leading or trailing). Such tags are moved by the segmentation engine to the content outside the segment boundaries, which is hidden and has to be displayed by in the specific editor view (when setting the display filter to 'all content').

The [SegmentationHint](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml) property setting does not change the behavior of tags that are not located on the segment boundaries and it is used only by the segmentation engine during the initial conversion of the native file to SDLXliff format. [SegmentationHint](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields) may be set to following values:

* [Undefined](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)
* [MayExclude](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)
* [IncludeWithText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)
* [Include](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)
* [Exclude](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.