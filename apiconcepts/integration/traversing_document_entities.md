Traversing document entities
=====

The visitor pattern allows a class implementer to traverse all the child objects in an objects hierarchy without having to know how the hierarchy is implemented or having to be aware of any changes in how the hierarchy is implemented.

Implementing `IMarkupDataVisitor`
----

All that is required is that your class needs to derive from the `IMarkupDataVisitor` interface and to explicitly implement all the interface’s methods. To use the visitor pattern, it is normally helpful to write the similar private method that calls accept visitor for all the markup data child items of the given parent.

This method can then be used to kick off the traversal of all the items in a paragraph-unit so that they can be written to the translated target document. This can be done in the implementation of ProcessParagraphUnit where the passed IParagraphUnit object can have its Source and Target paragraphs passed to AppendItems() and the implementations of the IMarkupDataVisitor interface methods can process the various types of text and mark-up objects that form part of the paragraph data hierarchies.

**VisitTagPair()**

VisitTagPair method is called when a tag pair object of type ITagPair is being visited. This tag pair object is normally processed by writing to the target document the tag pair start tag then processing all the tag pair’s child nodes then writing the tag pair end tag. However, some Bilingual Writers may need to cache this information first. In order to process all the child nodes of a tag pair the AppendItems() private helper method can be called with the tag object as the parameter.

**VisitPlaceholderTag())**

VisitPlaceholderTag method is called when a placeholder tag object of type IPlaceholderTag is being visited. This placeholder object is normally processed by writing to the target document this placeholder tag. However, some Bilingual writers may need to cache this information first.

**VisitText()**

VisitText method is called when a text object of type IText is being visited. This text object is normally processed by writing the content of the text object to the target document. However, when processing text items it may be necessary to consider the value of **ContentRestriction** as the content restriction type may indicate that the source text is to be written instead of the target text. If this is the case then when a translatable paragraph-unit is being processed then AppendItems() should be called for the source paragraph rather than then target paragraph ensuring that subsequent calls to VisitText will be for the source text.

**VisitSegment()**
VisitSegment method is called when a segment object of type ISegment is being visited. This segment object is normally processed by writing any required information from the **ISegmentPairProperties**, such as **MatchPercent** or **TranslationOrigin**.

**VisitLocationMarker()**
VisitLocationMarker method is called when a location marker object of type ILocationMarker is being visited. Location markers are typically used for locating a position within a translated document, such as the scroll position when previewing a document within a framework based editor.

**VisitOtherMarker()**
VisitOtherMarker method is called when an alternate location marker object of type IOtherMarker is being visited.

**VisitRevisionMarker()**
VisitRevisionMarker method is called when an track change location marker object of type IRevisionMarker is being visited.

**VisitLockedContent()**
VisitLockedContent method is called when a locked content container object of type ILockedContent is being visited. This locked content object is normally processed by checking the value of **LockType** in its Properties property. The value of this lock type may indicate how to treat the contents of this container when writing them to the translated target document. Also when processing this ILockedContent container it would normally be the case that AppendItems() would be called for its Content collection of child objects.