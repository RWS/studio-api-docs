Adding the Text Extractor Class
==

In this chapter you will learn how to add a separate class that helps you access segment content such as text and tag pairs.

Add the Text Extractor Helper Class
--

A segment is like a container that can include (apart from 'normal' text) various types of elements such as tag pairs, placeholders, comments, locked content, etc. In our simple implementation we would like to focus on how to retrieve text and tag pairs only. These are the elements that are most commonly found in a segment.

Add a new class to your project, called e.g. **BilTextExtractor.cs**. Since this class is also used to handle bilingual content, it needs to use the namespace S```dl.FileTypeSupport.Framework.BilingualApi```. The class must implement the interface [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml), which provides access to the markup data elements found in a segment.

Generate the Plain Text
--

Now add the following members to your class:

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

The public ```GetPlainText()``` helper function will be called later from the file writer. The writer will pass the current segment object to this method, which will then generate and return text as well as tag pairs. The ```GetPlainText()``` function calls the ```VisitChildren()``` helper function, which loops through all items in a segment markup container such as text and tags:

```cs
private void VisitChildren(IAbstractMarkupDataContainer container)
{
    foreach (var item in container)
    {
        item.AcceptVisitor(this);
    }
}
```

Collect the Markup Container Items
--

For each type of element that you can encounter in a segment (markup) container you need to add a separate member as required by the [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) interface. To keep this example as simple as possible, we will process only text and tag pairs. In one of the following chapters we will enhance this class to also return any comments that were added during the translation process.

We will leave all other members for retrieving e.g. locked content, placeholders, etc. empty.

The ```VistText()``` method adds a text object to the ```PlainText``` streambuilder, which is then also returned to the file writer class:

```cs
public void VisitText(IText text)
{
    PlainText.Append(text.Properties.Text);
}
```

The [VisitTagPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitTagPair_Sdl_FileTypeSupport_Framework_BilingualApi_ITagPair_) method, which is a member of the [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) interface, adds a tag pair object to the ```PlainText``` streambuilder, which is then returned to the file writer class:

```cs
public void VisitTagPair(ITagPair tagPair)
{
    PlainText.Append("<" + tagPair.StartTagProperties.TagContent + ">");
    VisitChildren(tagPair);
    PlainText.Append("</" + tagPair.EndTagProperties.TagContent + ">");
}
```

We leave the [VisitCommentMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitCommentMarker_Sdl_FileTypeSupport_Framework_BilingualApi_ICommentMarker_) method empty for the moment, and will come back to it later:

```cs
public void VisitCommentMarker(ICommentMarker commentMarker)
{

}
```

All other required interface members will be left empty in our sample implementation:


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

Putting it All Together
--

The complete text extractor class should now look as shown below:

```cs
using System.Text;
using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Sdl.Sdk.Snippets.Bilingual
{
    class BilTextExtractor : IMarkupDataVisitor
    {
        #region "plain text"
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
        #endregion

        // loops through all sub items of the container (IMarkupDataContainer)
        #region "loop"
        private void VisitChildren(IAbstractMarkupDataContainer container)
        {
            foreach (var item in container)
            {
                item.AcceptVisitor(this);
            }
        }
        #endregion

        #region IMarkupDataVisitor Members

        #region "text"
        public void VisitText(IText text)
        {
            PlainText.Append(text.Properties.Text);
        }
        #endregion

        #region "tagpairs"
        public void VisitTagPair(ITagPair tagPair)
        {
            PlainText.Append("<" + tagPair.StartTagProperties.TagContent + ">");
            VisitChildren(tagPair);
            PlainText.Append("</" + tagPair.EndTagProperties.TagContent + ">");
        }
        #endregion


        #region "comments"
        public void VisitCommentMarker(ICommentMarker commentMarker)
        {

        }
        #endregion

        #region "left empty"
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
        #endregion

        #endregion
    }
}
```

See Also
--

**Other Resources**

[Adding the File Writer Class](adding_the_file_writer_class.md)

[Generating the Paragraph Units](generating_the_paragraph_units.md)

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.