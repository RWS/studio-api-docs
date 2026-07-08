# Adding the Text Extractor Class

This section explains how to add a helper class that gives you access to segment content such as text and tag pairs.

## Add the Text Extractor Helper Class

A segment acts as a container for several element types, including text, tag pairs, placeholders, comments, and locked content. This simple implementation focuses only on text and tag pairs because they are the most common elements in a segment.

Add a new class to your project, for example **BilTextExtractor.cs**. Because this class handles bilingual content, add the `Sdl.FileTypeSupport.Framework.BilingualApi` namespace. The class must implement the [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) interface, which provides access to the markup data elements in a segment.

## Generate the Plain Text

Add the following members to your class:

# [C#](#tab/tabid-1)
```cs
internal StringBuilder PlainText
{
    get;
    set;
}

public string GetPlainText(ISegment segment)
{
    PlainText = new StringBuilder("");
    VisitChildren(segment);
    return PlainText.ToString();
}
```

The public `GetPlainText()` helper function is called later from the file writer. The writer passes the current segment object to this method, which returns the segment text together with any tag pairs. `GetPlainText()` calls the `VisitChildren()` helper function, which loops through all items in a segment markup container such as text and tags:

# [C#](#tab/tabid-2)
```cs
private void VisitChildren(IAbstractMarkupDataContainer container)
{
    foreach (var item in container)
    {
        item.AcceptVisitor(this);
    }
}
```
## Collect the Markup Container Items

For each element type that can appear in a segment markup container, add a separate member as required by the [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) interface. To keep this example simple, process only text and tag pairs. In a later section, you will extend this class to return comments that were added during translation.

Leave the remaining members for locked content, placeholders, and other element types empty.

The `VisitText()` method adds a text object to the `PlainText` string builder, which the file writer later receives:

# [C#](#tab/tabid-3)
```cs
public void VisitText(IText text)
{
    PlainText.Append(text.Properties.Text);
}
```

The [VisitTagPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitTagPair_Sdl_FileTypeSupport_Framework_BilingualApi_ITagPair_) method adds a tag pair to the `PlainText` string builder, which the file writer then uses:

# [C#](#tab/tabid-4)
```cs
public void VisitTagPair(ITagPair tagPair)
{
    PlainText.Append("<" + tagPair.StartTagProperties.TagContent + ">");
    VisitChildren(tagPair);
    PlainText.Append("</" + tagPair.EndTagProperties.TagContent + ">");
}
```

Leave the [VisitCommentMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitCommentMarker_Sdl_FileTypeSupport_Framework_BilingualApi_ICommentMarker_) method empty for now. You will return to it later:

# [C#](#tab/tabid-5)
```cs
public void VisitCommentMarker(ICommentMarker commentMarker)
{

}
```

Leave the other required interface members empty in this sample implementation:

# [C#](#tab/tabid-6)
```cs
public void VisitSegment(ISegment segment)
{
    VisitChildren(segment);
}

public void VisitLocationMarker(ILocationMarker location)
{

}

public void VisitLockedContent(ILockedContent lockedContent)
{

}

public void VisitOtherMarker(IOtherMarker marker)
{

}

public void VisitPlaceholderTag(IPlaceholderTag tag)
{

}

public void VisitRevisionMarker(IRevisionMarker revisionMarker)
{

}
```

## Putting It All Together

The complete text extractor class should now look like this:

# [C#](#tab/tabid-7)
```cs
using System.Text;
using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Sdk.Snippets.Bilingual
{
    class BilTextExtractor : IMarkupDataVisitor
    {
        internal StringBuilder PlainText
        {
            get;
            set;
        }

        public string GetPlainText(ISegment segment)
        {
            PlainText = new StringBuilder("");
            VisitChildren(segment);
            return PlainText.ToString();
        }

        // loops through all sub items of the container (IMarkupDataContainer)
        private void VisitChildren(IAbstractMarkupDataContainer container)
        {
            foreach (var item in container)
            {
                item.AcceptVisitor(this);
            }
        }

        public void VisitText(IText text)
        {
            PlainText.Append(text.Properties.Text);
        }

        public void VisitTagPair(ITagPair tagPair)
        {
            PlainText.Append("<" + tagPair.StartTagProperties.TagContent + ">");
            VisitChildren(tagPair);
            PlainText.Append("</" + tagPair.EndTagProperties.TagContent + ">");
        }


        public void VisitCommentMarker(ICommentMarker commentMarker)
        {

        }

        public void VisitSegment(ISegment segment)
        {
            VisitChildren(segment);
        }

        public void VisitLocationMarker(ILocationMarker location)
        {

        }

        public void VisitLockedContent(ILockedContent lockedContent)
        {

        }

        public void VisitOtherMarker(IOtherMarker marker)
        {

        }

        public void VisitPlaceholderTag(IPlaceholderTag tag)
        {

        }

        public void VisitRevisionMarker(IRevisionMarker revisionMarker)
        {

        }
    }
}
```

## See Also

- [Adding the File Writer Class](adding_the_file_writer_class.md)
- [Generating the Paragraph Units](generating_the_paragraph_units.md)

>[!NOTE]
>
> This content may be out of date. To verify the latest information on this topic, inspect the libraries in the Visual Studio Object Browser.
