# IMarkupDataVisitor

The visitor pattern lets you traverse all child objects in a markup hierarchy without depending on the hierarchy's internal implementation. This makes your code easier to maintain when the hierarchy changes.

## Implementing `IMarkupDataVisitor`

To use this pattern, implement the [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) interface and provide all required visitor methods. In most cases, it also helps to add a private helper method that calls `AcceptVisitor()` for each child item in a markup container.

# [C#](#tab/tabid-1)
```cs
private void AppendItems(IAbstractMarkupDataContainer parent)
{
    foreach (var item in parent)
    {
        item.AcceptVisitor(this);
    }
}
```
***

You can use this helper method to start traversal for all items in a paragraph unit so that your writer can output them to the translated target document. A common place to do this is in [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_). There, the [IParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) object can pass its source and target paragraphs to `AppendItems()`, while the `IMarkupDataVisitor` methods process the different text and markup objects in the hierarchy.

## Visitor methods

### `VisitTagPair()`

[VisitTagPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitTagPair_Sdl_FileTypeSupport_Framework_BilingualApi_ITagPair_) is called when the visitor reaches an [ITagPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ITagPair.yml) object. A typical implementation writes the start tag to the target document, processes the child nodes, and then writes the end tag. Some bilingual writers may need to cache this information first. To process the child nodes, call `AppendItems()` with the tag pair as the parameter.

### `VisitPlaceholderTag()`

[VisitPlaceholderTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitPlaceholderTag_Sdl_FileTypeSupport_Framework_BilingualApi_IPlaceholderTag_) is called when the visitor reaches an [IPlaceholderTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IPlaceholderTag.yml) object. A typical implementation writes the placeholder tag to the target document, although some bilingual writers may need to cache the information first.

### `VisitText()`

[VisitText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitText_Sdl_FileTypeSupport_Framework_BilingualApi_IText_) is called when the visitor reaches an [IText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IText.yml) object. A typical implementation writes the text content to the target document. When you process text items, also consider the [ContentRestriction](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ContentRestriction.yml) value. In some cases, the restriction indicates that you should write the source text instead of the target text. When that happens for a translatable paragraph unit, call `AppendItems()` for the source paragraph instead of the target paragraph so that later calls to `VisitText()` use the source text.

### `VisitSegment()`

[VisitSegment](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitSegment_Sdl_FileTypeSupport_Framework_BilingualApi_ISegment_) is called when the visitor reaches an [ISegment](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ISegment.yml) object. A typical implementation writes any required information from [ISegmentPairProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISegmentPairProperties.yml), such as [MatchPercent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ITranslationOrigin.yml#Sdl_FileTypeSupport_Framework_NativeApi_ITranslationOrigin_MatchPercent) or [TranslationOrigin](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISegmentPairProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_ISegmentPairProperties_TranslationOrigin).

### `VisitLocationMarker()`

[VisitLocationMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitLocationMarker_Sdl_FileTypeSupport_Framework_BilingualApi_ILocationMarker_) is called when the visitor reaches an [ILocationMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILocationMarker.yml) object. Location markers usually identify a position in a translated document, such as the scroll position in a framework-based preview editor.

### `VisitOtherMarker()`

[VisitOtherMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitOtherMarker_Sdl_FileTypeSupport_Framework_BilingualApi_IOtherMarker_) is called when the visitor reaches an [IOtherMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IOtherMarker.yml) object.

### `VisitRevisionMarker()`

[VisitRevisionMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitRevisionMarker_Sdl_FileTypeSupport_Framework_BilingualApi_IRevisionMarker_) is called when the visitor reaches an [IRevisionMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IRevisionMarker.yml) object. Revision markers usually represent tracked changes.

### `VisitLockedContent()`

[VisitLockedContent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitLockedContent_Sdl_FileTypeSupport_Framework_BilingualApi_ILockedContent_) is called when the visitor reaches an [ILockedContent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILockedContent.yml) container. A typical implementation checks the [LockType](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ILockedContentProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_ILockedContentProperties_LockType) value in the container's properties to determine how to handle the content when writing the translated target document. In many cases, you then call `AppendItems()` for the [Content](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILockedContent.yml#Sdl_FileTypeSupport_Framework_BilingualApi_ILockedContent_Content) collection.

## See also

- Reference [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml)
- [Visitor pattern](https://en.wikipedia.org/wiki/Visitor_pattern)
- [Design patterns](https://en.wikipedia.org/wiki/Design_Patterns)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
