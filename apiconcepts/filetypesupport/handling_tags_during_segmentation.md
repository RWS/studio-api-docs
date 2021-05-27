Handling Tags During Segmentation
===

In this chapter you will learn how to process tags that appear at the beginning or end of a segment.

Change the Way the Segmentation Treats Tags
--

A tag such as **IMG** may appear right in front or after a segment, i.e. the parser might have to deal with leading or trailing inline tags. You should always try to display as few tags as possible in order not no clutter the editor view unnecessarily. This is why leading and trailing tags are not displayed in the editor of <Var:ProductName> by default.

For example, the following leading **IMG** tag would not be shown in the editor view, however, the sub-segment content would be displayed:

# [HTML](#tab/tabid-1)
```html
<img src="button.jpg" alt="Open dialog box" /> Click the Open Dialog box button to open the dialog box.
```

Output in the editor <Var:ProductName>.

![SubSegmentWithoutTag](images/SubSegmentWithoutTag.jpg)


The user could set the display filter to show all content (by selecting **All content** from the **Display** dropdown list). In this case the leading tag is shown in a separate row in the editor:

![TagShownSeperately](images/TagShownSeperately.jpg)


However, you can change this behavior as required through the [SegmentationHint](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IStartTagProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IStartTagProperties_SegmentationHint) property of the placeable properties object. Unless otherwise defined, the property value is [Undefined](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields). This means that the default segmentation behavior will be applied, which means that leading and trailing tags will not be shown, as <Var:ProductName> tries to minimize the number of tags displayed, which is usually more translator-friendly.

You can, however, set the segmentation hint property to [Include](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields), if you want to make sure that such tags are shown in the same segment as the actual text, e.g.:

# [C#](#tab/tabid-2)
```cs
placeProperties.SegmentationHint = SegmentationHint.Include
```
***

You could also apply the value [IncludeWithText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SegmentationHint.yml#fields), which means that tags are not shown unless they contain localizable text. In our example the **IMG** tag will be shown, if it contains localizable text in the **ALT** property, but it will not be displayed, if the **ALT** attribute is missing.


See Also
--



[Processing Placeholder Tags](processing_placeholder_tags.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.

