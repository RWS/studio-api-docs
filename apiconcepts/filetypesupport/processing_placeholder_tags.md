Processing Placeholder Tags
==

In one of the previous chapters you learned how to process tag pairs (e.g. <b></b>) during file parsing (see [Processing Inline Formatting](processing_inline_formatting.md)). In this chapter you will learn how to process standalone placeholder tags, such as <img src="button.jpg" />

Enhance your Parser to Process Standalone Placeholder Tags
--

Let us assume that your sample text files sometimes contain tags for referencing images, e.g.:

```html
<img src="button.jpg" alt="Click here" />
```

Below you see an example of a segment that contains a standalone placeholder tag:

```html
Click the button <img src="button.jpg" alt="Open"/> to open 
the dialog box.
```

These tags are closed within themselves and occur as placeholders within the text. To keep this example as simple as possible let us proceed on the assumption that the **IMG** tag is the only placeholder tag that we can expect to find in any given document.

Start by making a small change to the ```ProcessFormatting()``` helper function: if the regular expression pattern found a match that starts on "**<img**", then the ```WritePlaceHolder()``` helper function (which we still need to implement) should be called, otherwise we call ```WriteInlineTag()```.

```cs
if (rxMatch.Value.StartsWith("<img"))
{
    WritePlaceholderTag(rxMatch.Value);
} else {
    bool IsOpening = rxMatch.Value.Contains("/") ? false : true;
    WriteInlineTag(rxMatch.Value, IsOpening);
}
```

The complete function now looks as shown below:

```cs
// this function uses regular expressions to identify
// what is 'normal' translatable content and which strings
// need to be marked up as inline tags, e.g. <b>
private string ProcessFormatting(string sLine)
{
    int LastPosition = 0;
    // search for opening and closing <b> tags
    Regex rx = new Regex(@"\<.*?\>", RegexOptions.Compiled);
    MatchCollection rxMatches = rx.Matches(sLine);

    foreach (Match rxMatch in rxMatches)
    {
        if (LastPosition != rxMatch.Index)
        {
            WriteText(sLine.Substring(LastPosition, rxMatch.Index - LastPosition));
        }
        #region "inline or placeholder"
        if (rxMatch.Value.StartsWith("<img"))
        {
            WritePlaceholderTag(rxMatch.Value);
        } else {
            bool IsOpening = rxMatch.Value.Contains("/") ? false : true;
            WriteInlineTag(rxMatch.Value, IsOpening);
        }
        #endregion

        LastPosition = rxMatch.Index + rxMatch.Length;
    }
    return sLine.Substring(LastPosition, sLine.Length - LastPosition);
}
```


Next, implement the method that outputs the **IMG** placeholder tag. The properties factory implements a [CreatePlaceholderTagProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPropertiesFactory.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPropertiesFactory_CreatePlaceholderTagProperties_System_String_) method that creates properties for a standalone inline tag. When creating the tag property, the tag content is passed to the ```Create()``` method as a parameter. You can define the optional [DisplayText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractBasicTagProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractBasicTagProperties_DisplayText property, which specifies how the tag should be displayed in partial tag text mode. The tag property can be output using the method ```Output.InlinePlaceholderTag()```.

```cs
private void WritePlaceholderTag(string tagContent)
{
    IPlaceholderTagProperties placeProperties = PropertiesFactory.CreatePlaceholderTagProperties(tagContent);
    placeProperties.TagContent = tagContent;
    placeProperties.DisplayText = "img";

    // calculate offset and length
    int offset, length;
    offset = tagContent.IndexOf("alt=\"") + 5;
    length = tagContent.Substring(offset, tagContent.Length - offset - 4).Length;

    ISubSegmentProperties subSeg = PropertiesFactory.CreateSubSegmentProperties(offset, length);
    placeProperties.AddSubSegment(subSeg);
    Output.InlinePlaceholderTag(placeProperties);
}
```

The placeholder tag will appear in partial tag text mode as shown below:

![ImgTag](images/ImgTag.jpg)

And this is what the placeholder tag looks like when the full tag content is displayed (full tag text mode):

![ImgFull](images/ImgFull.jpg)

Expose Localizable Text Within a Tag
As you can see in the above example tags may contain localizable text such as the text defined by the **ALT** attribute. In our current implementation this text is not exposed for translation, as the entire **IMG** tag with all its attributes is treated as a placeable, and thereby as a non-translatable element.

The API allows you to access localizable text that is embedded within a placeable, which is then treated as a so-called sub-segment, i.e. a segment within a segment. To do this you first have to create a sub-segment object through the properties factory. To this end, the properties factory implements the [CreateSubSegmentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPropertiesFactory.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPropertiesFactory_CreateSubSegmentProperties_System_Int32_System_Int32_) method, which takes two parameters: the offset and the length of the localizable string. Take a look at our example:

```html
<img src="button.jpg" alt="img src="button.jgp" alt="Open dialog box" /"/>
```

The offset in this example is 27, the length of the localizable piece of text (*Open dialog box*) is 15. The sub-segment object can therefore be created as shown below:

```cs
ISubSegmentProperties subSeg = PropertiesFactory.CreateSubSegmentProperties(27, 15);
```
After creating the sub-segment object, you can add it to the placeable properties by applying the [AddSubSegment](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractTagProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractTagProperties_AddSubSegment_Sdl_FileTypeSupport_Framework_NativeApi_ISubSegmentProperties_) method:

```cs
placeProperties.AddSubSegment(subSeg);
```

After adding the sub-segment object to the placeable properties you can output the placeable as shown below:



```cs
Output.InlinePlaceholderTag(placeProperties);
```

The SDL File Type Support Framework API will then handle the extraction of these localizable sub-segments into separate translation units that are displayed to the translator. Therefore, the output in the editor of <Var:ProductName> will look as shown below:

![SubSegment](images/SubSegment.jpg)

Of course, you would have to implement the functionality to calculate the offset and length, so that your ```WritePlaceholderTag()``` helper function would look as shown below:


```cs
private void WritePlaceholderTag(string tagContent)
{
    IPlaceholderTagProperties placeProperties = PropertiesFactory.CreatePlaceholderTagProperties(tagContent);
    placeProperties.TagContent = tagContent;
    placeProperties.DisplayText = "img";

    // calculate offset and length
    int offset, length;
    offset = tagContent.IndexOf("alt=\"") + 5;
    length = tagContent.Substring(offset, tagContent.Length - offset - 4).Length;

    ISubSegmentProperties subSeg = PropertiesFactory.CreateSubSegmentProperties(offset, length);
    placeProperties.AddSubSegment(subSeg);
    Output.InlinePlaceholderTag(placeProperties);
}
```

See Also
--

**Other Resources**

[Processing Inline Formatting](processing_inline_formatting.md)

[Handling Tags During Segmentation](handling_tags_during_segmentation.md)

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.