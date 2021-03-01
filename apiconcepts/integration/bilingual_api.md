Overview
=====
This section contains a quick overview of the Bilingual API. For detailed documentation on individual interfaces, properties, methods etc. see the reference documentation.

Bilingual processor components implement the *IBilingualContentProcessor* interface, which the framework calls to let the component process content in a bilingual object model one paragraph unit at a time.

<img style="display:block; " src="images/Bilingual Filter Component.jpg"/>

The content is delivered to the components through calls to ProcessParagraphUnit. The Initialize method is invoked before any content is passed through the component, and it communicates the document properties which are common for all files in the document. For each native file in the bilingual document the framework calls SetFileProperties before the content of the file is processed. When all content in a file has been processed the framework calls FileComplete and when all files in a document have been processed the framework calls Complete.

The file properties passed through the SetFileProperties call can be used to retrieve the persistant file conversion settings, where components can store and retrieve important settings related to the data being processed.

<img style="display:block; " src="images/File Properties.jpg"/>

The `EvenfFiringBilingualProcessor` is an implementation of the IBilingualContentProcessor interface that is convenient to use when you are only interested in processing one or two of these calls. That implementation simply fires events for each of the calls and you can hook up your event handler to the one you are interested in. (This is very handy especially when writing unit tests.)

<img style="display:block; " src="images/Event Driven Bilingual Processing.jpg"/>

Paragraph units can be divided into two types: structure paragraph units and localizable paragraph units. Structure paragraph units contain only structural data (i.e. structure tags), they have no directly localizable content. Localizable paragraph units contain text and tags that are modified during translation. Both types of paragraph units are processed in the ProcessParagraphUnit call, the implementation must check the IsStructure property to determine which type each is.

<img style="display:block; " src="images/Structure Paragraph Unit.jpg"/>

A localizable paragraph unit can have the following main properties:

<img style="display:block; " src="images/Localizable Paragraph Unit.jpg"/>

The source and target language content in a paragraph is comprised of objects that implement IMarkupDataVisitor derived interfaces:

<img style="display:block; " src="images/Paragraph.jpg"/>

Here is a diagram with some more details on the inline tags:

<img style="display:block; " src="images/Inline Tags.jpg"/>

Tags and text inside a paragraph can be annotated with different types of markup, represented with IAbstractMarker as base interface:

<img style="display:block; " src="images/Markers.jpg"/>

The most important type of markup is segments. A segment is uniquely identified within the paragraph unit through its segment ID. As segment in a localized paragraph unit always has a source/target language counterpart. The source and target language segments referemce the same ISegmentPair object, and thus implicitly will always have the same segment ID. The corresponding source/target segment can be easily retrieved through the GetSourceSegment and GetTargetSegment methods, which take the segment ID as parameter and returns the corresponding object.

Navigation and iteration through the bilingual content in a paragraph or other markup data container can be achieved in a number of different ways. Perhaps the most intuitive is to use the Parent, IndexInParent and Items properties to directly access related nodes, or by iterating over all items in an IAbstractMarkupDataContainer either directly or through the AllSubItems property of the container, or by calling the ForEachSubItem method passing in an action object.

The Location class provides another flexible way of iterating through and working with the localizable content in a paragraph:

<img style="display:block; " src="images/Locations.jpg"/>

Localizable content can also be processed through a visitor pattern. This is often useful when dealing with collections of objects (e.g. in markup data containers), as the use of a visitor avoids having to construct awkward and difficult to maintain switch/if statements to test for different object types. Simply call AcceptVisitor on the object, and the object will call back to its corresponding method on the visitor. For more information about the Visitor pattern, see Design Patters by the “Gang of Four” (Gamma, Helm, Johnson, Vlissides), a book that should be on every developer's desk.

<img style="display:block; " src="images/Visitor Pattern.jpg"/>