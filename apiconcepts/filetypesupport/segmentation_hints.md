# Segmentation Hints

The [SegmentationHint](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml) property specifies the required behavior of a tag placeholder or tag pair that appears on a segment boundary (leading or trailing). The segmentation engine moves such tags to content outside the segment boundaries. This content is hidden by default but displays in the editor when you set the display filter to "all content".

The segmentation engine only applies the [SegmentationHint](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml) property during initial conversion of the native file to SDLXliff format. It does not affect tags that are not located on segment boundaries. Set [SegmentationHint](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields) to one of the following values:

* [Undefined](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)
* [MayExclude](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)
* [IncludeWithText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)
* [Include](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)
* [Exclude](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
