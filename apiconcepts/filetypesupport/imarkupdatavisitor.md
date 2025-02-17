IMarkupDataVisitor
===

The visitor pattern allows a class implementer to traverse all the child objects in an objects hierarchy without having to know how the hierarchy is implemented or having to be aware of any changes in how the hierarchy is implemented.

Implementing IMarkupDataVisitor
--

All that is required is that your class needs to derive from the [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) interface and to explicitly implement all the interface’s methods. To use the visitor pattern it is normally helpful to write the similar private method that calls accept visitor for all the markup data child items of the given parent.

# [C#](#tab/tabid-1)
```cs
private void AppendItems(IAbstractMarkupDataContainer parent)
{
    // implemented as a visitor
    foreach (var item in parent)
    {
        item.AcceptVisitor(this);
    }
}
```
***

This method can then be used to kick off the traversal of all the items in a paragraph-unit so that they can be written to the translated target document. This can be done in the implementation of [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) where the passed [IParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) object can have its Source and Target paragraphs passed to ```AppendItems()``` and the implementations of the [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) interface methods can process the various types of text and mark-up objects that form part of the paragraph data hierarchies.

**VisitTagPair()**

[VisitTagPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitTagPair_Sdl_FileTypeSupport_Framework_BilingualApi_ITagPair_) method is called when a tag pair object of type [ITagPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ITagPair.yml) is being visited. This tag pair object is normally processed by writing to the target document the tag pair start tag then processing all the tag pair’s child nodes then writing the tag pair end tag. However, some Bilingual Writers may need to cache this information first. In order to process all the child nodes of a tag pair the ```AppendItems()``` private helper method can be called with the tag object as the parameter.

**VisitPlaceholderTag()**

[VisitPlaceholderTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitPlaceholderTag_Sdl_FileTypeSupport_Framework_BilingualApi_IPlaceholderTag_) method is called when a placeholder tag object of type [IPlaceholderTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IPlaceholderTag.yml) is being visited. This placeholder object is normally processed by writing to the target document this placeholder tag. However, some Bilingual writers may need to cache this information first.

**VisitText()**

[VisitText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitText_Sdl_FileTypeSupport_Framework_BilingualApi_IText_) method is called when a text object of type [IText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IText.yml) is being visited. This text object is normally processed by writing the content of the text object to the target document. However, when processing text items it may be necessary to consider the value of [ContentRestriction](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ContentRestriction.yml) as the content restriction type may indicate that the source text is to be written instead of the target text. If this is the case then when a translatable paragraph-unit is being processed then ```AppendItems()``` should be called for the source paragraph rather than then target paragraph ensuring that subsequent calls to [VisitText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitText_Sdl_FileTypeSupport_Framework_BilingualApi_IText_) will be for the source text.

**VisitSegment()**

[VisitSegment](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitSegment_Sdl_FileTypeSupport_Framework_BilingualApi_ISegment_) method is called when a segment object of type ISegment is being visited. This segment object is normally processed by writing any required information from the [ISegmentPairProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISegmentPairProperties.yml) , such as [MatchPercent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ITranslationOrigin.yml#Sdl_FileTypeSupport_Framework_NativeApi_ITranslationOrigin_MatchPercent) or [TranslationOrigin](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISegmentPairProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_ISegmentPairProperties_TranslationOrigin).

**VisitLocationMarker()**

[VisitLocationMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitLocationMarker_Sdl_FileTypeSupport_Framework_BilingualApi_ILocationMarker_) method is called when a location marker object of type [ILocationMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILocationMarker.yml) is being visited. Location markers are typically used for locating a position within a translated document, such as the scroll position when previewing a document within a framework based editor.

**VisitOtherMarker()**

[VisitOtherMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitOtherMarker_Sdl_FileTypeSupport_Framework_BilingualApi_IOtherMarker_) method is called when an alternate location marker object of type [IOtherMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IOtherMarker.yml) is being visited.

**VisitRevisionMarker()**

[VisitRevisionMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitRevisionMarker_Sdl_FileTypeSupport_Framework_BilingualApi_IRevisionMarker_) method is called when an track change location marker object of type [IRevisionMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IRevisionMarker.yml) is being visited.

**VisitLockedContent()**

[VisitLockedContent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitLockedContent_Sdl_FileTypeSupport_Framework_BilingualApi_ILockedContent_) method is called when a locked content container object of type [ILockedContent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILockedContent.yml) is being visited. This locked content object is normally processed by checking the value of [LockType](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ILockedContentProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_ILockedContentProperties_LockType) in its Properties property. The value of this lock type may indicate how to treat the contents of this container when writing them to the translated target document. Also when processing this [ILockedContent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILockedContent.yml) container it would normally be the case that ```AppendItems()``` would be called for its [Content](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILockedContent.yml#Sdl_FileTypeSupport_Framework_BilingualApi_ILockedContent_Content) collection of child objects.

See Also
--

**Reference**

[IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml)



[Visitor pattern](https://en.wikipedia.org/wiki/Visitor_pattern)

[Design patterns](https://en.wikipedia.org/wiki/Design_Patterns)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
