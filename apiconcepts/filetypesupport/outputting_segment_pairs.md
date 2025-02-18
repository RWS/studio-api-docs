Outputting Segment Pairs
===

In this chapter you will learn how to extract and expose the source and target segments of a given BIL input file in the intermediary (SDLXliff) file, and thus in the editing environment of Var:ProductName.

Extend the Parsing Method
--

Now we need to extend the parsing logic to extract the source and target segments and expose them for translation in the editor of Var:ProductName. Basically, our parser should work like this: the content of a unit element defines a paragraph unit in the intermediary (SDLXliff) file. A unit element in a BIL file always contains one source segment and one or no target segment. These segments will be added to the paragraph unit as segment pairs. A segment pair must contain a source segment, but the target segment may be empty, i.e. in this case the target of the BIL unit element is empty.

Note that in this first step we are not going to process inline tags, formatting or any of the other elements a unit may contain (e.g. comments).

Add the following ```foreach``` loop to the [ParseNext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualParser.yml#Sdl_FileTypeSupport_Framework_BilingualApi_AbstractBilingualParser_ParseNext) method:

# [C#](#tab/tabid-1)
```cs
foreach (XmlNode item in _document.SelectNodes("//unit"))
{
        Output.ProcessParagraphUnit(CreateParagraphUnit(item));
}
```
***

This loop iterates through all ```unit``` elements of the input BIL file and outputs a paragraph unit in the intermediary (SDLXliff) document by calling a separate ```CreateParagraphUnit()``` helper function. This helper function takes the sub-nodes of the ```unit``` element as parameter. The sub-nodes in the current basic implementation will be, the ```source``` and ```target``` nodes.

Add a Helper Function for Generating Paragraph Units
--

In the next step add the helper function that is used to generate a paragraph unit from the current unit node. In this function you generate a paragraph unit object through the item factory as shown below:

# [C#](#tab/tabid-2)
```cs
IParagraphUnit paragraphUnit = ItemFactory.CreateParagraphUnit(LockTypeFlags.Unlocked);
```
***


Through the [LockTypeFlags](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.LockTypeFlags.yml) parameter of the ```CreateParagraphUnit()``` helper function you can determine whether a paragraph unit should be locked for editing or not. Normally, the paragraph units will not be locked, which means that they can be accessed and edited by the translator.

Now you use the item factory to create a segment pair object as shown below:

# [C#](#tab/tabid-3)
```cs
ISegmentPairProperties segmentPairProperties = ItemFactory.CreateSegmentPairProperties();
```
***

Next, you create a source segment object, which is then attached to the paragraph unit. If a target segment is available, you also need to create a target segment object and attach it to the paragraph unit. Below you see the full helper function for generating the paragraph unit object to output in the document. Note that the actual source and target segment generation occurs in a separate helper function, which we will create in the next step.

# [C#](#tab/tabid-4)
```cs
// helper function for creating paragraph units
private IParagraphUnit CreateParagraphUnit(XmlNode xmlUnit)
{
    // create paragraph unit object
    IParagraphUnit paragraphUnit = ItemFactory.CreateParagraphUnit(LockTypeFlags.Unlocked);


    // create segment pair object
    ISegmentPairProperties segmentPairProperties = ItemFactory.CreateSegmentPairProperties();  
    // assign the appropriate confirmation level to the segment pair            
    segmentPairProperties.ConfirmationLevel=CreateConfirmationLevel(xmlUnit.Attributes["status"].Value);

    // add source segment to paragraph unit
    ISegment srcSegment = CreateSegment(xmlUnit.SelectSingleNode("source/seg"), segmentPairProperties);            
    paragraphUnit.Source.Add(srcSegment);

    // add target segment to paragraph unit if available
    if(xmlUnit.SelectSingleNode("target/seg") != null)            
    {
        ISegment trgSegment = CreateSegment(xmlUnit.SelectSingleNode("target/seg"), segmentPairProperties);
        paragraphUnit.Target.Add(trgSegment);
    }

    #region "context"
    // create paragraph unit context
    string id = xmlUnit.SelectSingleNode("./@id").InnerText;
    if(xmlUnit.SelectSingleNode("type/@spec")!=null)
    {
        string spec = xmlUnit.SelectSingleNode("type/@spec").InnerText;

        paragraphUnit.Properties.Contexts=CreateContext(spec, id);
    } else {
        paragraphUnit.Properties.Contexts = CreateContext("Paragraph", id);
    }
    #endregion

    #region "comments"
    // extract comment (if applicable)
    if(xmlUnit.SelectSingleNode("comment")!=null)
    {
        paragraphUnit.Properties.Comments = CreateComment(xmlUnit.SelectSingleNode("comment").InnerText);
    }
    #endregion

    return paragraphUnit;
}
```
***

Add a Helper Function for Generating the Source and Target Segments
--

The helper function that creates the source and target segments requires the segment node and the segment pair properties as parameters. Passing the segment pair properties makes certain that the source and target segments are assigned to the correct segment pair. This helper function uses the properties factory to generate the text properties from the text content of the seg node. Through the item factory you create the actual text object from the text properties. Last, the text is added to the segment object, which is then returned to the helper function.

# [C#](#tab/tabid-5)
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

If you build your project at this stage, your file type plug-in should yield the following result when opening the sample file:

![BilText](images/BilText.jpg)


Update the Progress Count
--

At this point it is a good idea to implement the logic required for updating the progress report by making the following additions to the [ParseNext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualParser.yml#Sdl_FileTypeSupport_Framework_BilingualApi_AbstractBilingualParser_ParseNext) method:

# [C#](#tab/tabid-6)
```cs
public bool ParseNext()
{
    // variables for the progress report
    int totalUnitCount = _document.SelectNodes("//unit").Count;
    int currentUnitCount = 0;
    foreach (XmlNode item in _document.SelectNodes("//unit"))
    {
        Output.ProcessParagraphUnit(CreateParagraphUnit(item));

        // update the progress report   
        currentUnitCount++;
        OnProgress(Convert.ToByte(Math.Round(100 * ((decimal)currentUnitCount / totalUnitCount), 0)));
    }

    return false;
}
```
***

See Also
--



[Processing Inline Tags](processing_inline_tags.md)

[Applying Character Formatting](applying_character_formatting.md)

[Applying the Segment Pair Confirmation Levels](applying_the_segment_pair_confirmation_levels.md)

[Adding Context Information](adding_context_information.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
