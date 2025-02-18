Processing Inline Tags
===

In this chapter you will learn how to parse inline tags and how to display character formatting in the side-by-side editing environment of Var:ProductName.

Enhance the Helper Function for Creating Segments
--

The segments in our bilingual sample format can contain the following inline tags: *< b>*, *< i>*, and *< u>*. To keep this example simple let us proceed on the assumption that only these three inline tag types can occur in a seg element.

A segment with inline tags may look as follows:

# [HTML](#tab/tabid-1)
```html
Open the <b>dialog box</b>.
```
***

You need to enhance the ```CreateSegment()``` helper function to loop through the segment node and create a text object or a tag depending on whether a sub-node found in the segment is of the type **text** or of the type **element**. If a text node is found, you call another helper function (```CreateText()```) that adds the text to the segment or ```CreateTagPair()``` that creates a tag pair:

# [C#](#tab/tabid-2)
```cs
// helper function for creating segment objects
private ISegment CreateSegment(XmlNode segNode, ISegmentPairProperties pair)
{
    ISegment segment = ItemFactory.CreateSegment(pair);

    foreach (XmlNode item in segNode.ChildNodes)
    {
        if (item.NodeType == XmlNodeType.Text)
        {
            segment.Add(CreateText(item.InnerText));
        }

        if (item.NodeType == XmlNodeType.Element)
        {
            segment.Add(CreateTagPair(item));
        }
    }
    return segment;
}
```
***

Add the Helper Function for Generating Text
--

For convenience reasons, 'outsource' the generation of text items to a separate helper function, which looks as shown below:

# [C#](#tab/tabid-3)
```cs
private IText CreateText(string segText)
{
    ITextProperties textProperties = PropertiesFactory.CreateTextProperties(segText);
    IText textContent = ItemFactory.CreateText(textProperties);

    return textContent;
}
```
***

Add the Helper Function for Generating Tag Pairs
--

Now add the function for generating tag pairs. This function works as follows: The properties factory is leveraged to create the start and the end tag properties. The display text of the tags is the tag text that users see when the (default) partial tag text mode of Var:ProductName is activated. Then we use the item factory to generate the actual tag pair object based on the start and end tag properties. Note that each opening tag requires a closing tag, i.e. the bilingual document needs to be well-formed in an XML sense. If that is not the case, the framework will raise a critical error.

Do not forget to extract also the text that is enclosed in the tag pair. To this end we call the ```CreateText()``` helper function, which generates the text between the opening and the closing tag, which is then appended to the tag pair.

# [C#](#tab/tabid-4)
```cs
private ITagPair CreateTagPair(XmlNode item)
{
    // create the start and the end tag
    IStartTagProperties startTag = PropertiesFactory.CreateStartTagProperties(item.Name);
    #region "formatting"
    // apply character formatting to the start tag
    IFormattingGroup formattingGroup = PropertiesFactory.FormattingItemFactory.CreateFormatting();
    startTag.Formatting = new FormattingGroup();
    switch (item.Name)
    {
        case "b":
            formattingGroup.Add(new Bold(true));
            break;
        case "i":
            formattingGroup.Add(new Italic(true));
            break;
        case "u":
            formattingGroup.Add(new Underline(true));
            break;
        default:
            break;
    }
    startTag.Formatting = formattingGroup;
    #endregion

    startTag.DisplayText=item.Name;
    startTag.CanHide = true;
    IEndTagProperties endTag = PropertiesFactory.CreateEndTagProperties(item.Name);
    endTag.DisplayText=item.Name;
    endTag.CanHide = true;

    // create a tag pair out of the start and the end tag
    ITagPair tagPair = ItemFactory.CreateTagPair(startTag, endTag);

    // add text enclosed in the tag pair
    tagPair.Add(CreateText(item.InnerText));

    return tagPair;
}
```
***

After you have enhanced your file type plug-in as described above, the BIL sample file should look as shown below in Var:ProductName:

![BilWithTags](images/BilWithTags.jpg)

See Also
--



[Applying Character Formatting](applying_character_formatting.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
