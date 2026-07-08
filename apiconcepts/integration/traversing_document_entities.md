# Traversing document entities

The visitor pattern enables a class implementer to traverse all child objects in an object hierarchy without needing to know how the hierarchy is implemented or changes to the implementation.

## Implementing `IMarkupDataVisitor`

Your class must derive from the [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitTagPair_Sdl_FileTypeSupport_Framework_BilingualApi_ITagPair_) interface and explicitly implement all its methods. To use the visitor pattern effectively, write a private method that calls accept visitor for all markup data child items of the given parent.

Use this method to initiate traversal of all items in a paragraph-unit and write them to the translated target document. Implement this in the [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) method. Pass the Source and Target paragraphs from the [IParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) object to `AppendItems()`. The [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) interface methods process the various types of text and markup objects in the paragraph data hierarchies.

### VisitTagPair()

The [VisitTagPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitTagPair_Sdl_FileTypeSupport_Framework_BilingualApi_ITagPair_) method executes when visiting a tag pair object of type [ITagPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ITagPair.yml). Typically, process this object by writing the tag pair start tag to the target document, processing all child nodes, and writing the tag pair end tag. Some bilingual writers may prefer to cache this information first. Call the `AppendItems()` helper method with the tag object as a parameter to process all child nodes of a tag pair.

### VisitPlaceholderTag()

The [VisitPlaceholderTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitPlaceholderTag_Sdl_FileTypeSupport_Framework_BilingualApi_IPlaceholderTag_) method executes when visiting a placeholder tag object of type [IPlaceholderTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IPlaceholderTag.yml). Typically, write the placeholder tag to the target document. Some bilingual writers may prefer to cache this information first.

### VisitText()

The [VisitText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitText_Sdl_FileTypeSupport_Framework_BilingualApi_IText_) method executes when visiting a text object of type [IText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IText.yml). Write the content to the target document. When processing text items, consider the `ContentRestriction` value. The content restriction type indicates whether to write the source or target text. For translatable paragraph-units, call `AppendItems()` for the source paragraph when the restriction requires source text. Subsequent [VisitText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitText_Sdl_FileTypeSupport_Framework_BilingualApi_IText_) calls will then process the source text.

### VisitSegment()

The [VisitSegment](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitSegment_Sdl_FileTypeSupport_Framework_BilingualApi_ISegment_) method executes when visiting a segment object of type [ISegment](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ISegment.yml). Write any required information from the [ISegmentPairProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISegmentPairProperties.yml), such as `MatchPercent` or `TranslationOrigin`.

### VisitLocationMarker()

The [VisitLocationMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitLocationMarker_Sdl_FileTypeSupport_Framework_BilingualApi_ILocationMarker_) method executes when visiting a location marker object of type ILocationMarker. Location markers identify positions within a translated document, such as the scroll position when previewing a document in a framework-based editor.

### VisitOtherMarker()

The [VisitOtherMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitOtherMarker_Sdl_FileTypeSupport_Framework_BilingualApi_IOtherMarker_) method executes when visiting an alternate location marker object of type [IOtherMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IOtherMarker.yml).

### VisitRevisionMarker()

The [VisitRevisionMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitRevisionMarker_Sdl_FileTypeSupport_Framework_BilingualApi_IRevisionMarker_) method executes when visiting a track change location marker object of type [IRevisionMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IRevisionMarker.yml).

### VisitLockedContent()

The [VisitLockedContent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitLockedContent_Sdl_FileTypeSupport_Framework_BilingualApi_ILockedContent_) method executes when visiting a locked content container object of type [ILockedContent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILockedContent.yml). Check the `LockType` value in its [Properties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILockedContent.yml#Sdl_FileTypeSupport_Framework_BilingualApi_ILockedContent_Properties) property. The lock type indicates how to treat the container contents when writing them to the translated target document. Call `AppendItems()` for the [Content](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILockedContent.yml#Sdl_FileTypeSupport_Framework_BilingualApi_ILockedContent_Content) collection of child objects.

## See Also

- [Visitor pattern](https://en.wikipedia.org/wiki/Visitor_pattern)
- [Design pattern](https://en.wikipedia.org/wiki/Design_Patterns)