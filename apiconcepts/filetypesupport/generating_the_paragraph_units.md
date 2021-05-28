Generating the Paragraph Units
===

In this chapter you will learn to extend the writer class to output the actual paragraph units from the intermediary (SDLXliff) file to the target BIL document.

Modify the Function for Processing the Paragraph Units
--

First, we will add the required application logic to the method ```CreateParagraphUnit()```, which loops through the paragraph units in the intermediary document. The function is supposed to work like this: the id of the current paragraph unit is retrieved from the context metadata. Remember that we added the unit id as hidden meta information (see [Adding Context Information](adding_context_information.md)). Using the id we will determine the corresponding ```unit``` in the original BIL document. We then call a separate helper function, which replaces the segments from the original BIL document with the paragraph unit content, and thus the segment pairs from the intermediary document.

The method should look as shown below:

# [C#](#tab/tabid-1)
```cs
public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
{
    string unitId = paragraphUnit.Properties.Contexts.Contexts[1].GetMetaData("UnitID");
    XmlNode xmlUnit = _targetFile.SelectSingleNode("//unit[@id='" + unitId + "']");

    // call helper function to generate the paragraph unit
    CreateParagraphUnit(paragraphUnit, xmlUnit);
}
```
***

Note that for this simple implementation we just assume that every paragraph unit in the intermediary document corresponds to a unit id from the original BIL file. In a 'real-life' implementation you should, of course, make sure to catch cases in which a given id does not exist.

Add the Helper Function to Generate the Target Paragraph Units
--

Now it is time to implement the helper function that generates the new BIL unit elements from the source target segments in the intermediary file. This function leverages the ```BilTextExtractor``` class, which we created previously (see [Adding the Text Extractor Class](adding_the_text_extractor_class.md)). Therefore, you should declare a new global text extractor member derived from this class:

# [C#](#tab/tabid-2)
```cs
private BilTextExtractor _textExtractor;
```
***

Then you use the the ```Initialize()``` method to create a text extractor object:

# [C#](#tab/tabid-3)
```cs
public void Initialize(IDocumentProperties documentInfo)
{
    _textExtractor = new BilTextExtractor();
}
```
***

Now you can add the following helper function, which works as follows: the ```source``` and the ```target``` elements are taken from the current BIL ```unit``` element. Then, the function loops through the segment pairs of the current paragraph unit from the intermediary (SDLXliff) document. The text extractor object is used to retrieve the plain text and the tag pairs for each segment pair. The ```source``` and the ```target``` elements are then set to the strings that were retrieved through the text extractor object.

# [C#](#tab/tabid-4)
```cs
private void CreateParagraphUnit(IParagraphUnit paragraphUnit, XmlNode xmlUnit)
{
    XmlNode source = xmlUnit.SelectSingleNode("source");
    XmlNode target = xmlUnit.SelectSingleNode("target");

    //iterate all segment pairs
    foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
    {
        source.InnerXml = _textExtractor.GetPlainText(segmentPair.Source);
        target.InnerXml = _textExtractor.GetPlainText(segmentPair.Target);
    }
}
```
***

Note that we loop through all segment pairs, although in our simplified implementation each unit can be assumed to contain only one segment pair. A paragraph unit in the intermediary (SDLXliff) document would contain several segment pairs if our fictitious BIL format allowed for more than ```one seg``` element within a ```source``` or ```target``` node.

Putting it All Together
--

The enhanced writer class should now look as shown below:

# [C#](#tab/tabid-5)
```cs
using System;
using System.Xml;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.Snippets.Bilingual
{
    class BilWriter2 : AbstractBilingualFileTypeComponent, IBilingualWriter, INativeOutputSettingsAware
    {
        #region "global"
        private IPersistentFileConversionProperties _originalFileProperties;
        private INativeOutputFileProperties _nativeFileProperties;
        private XmlDocument _targetFile;
        private BilTextExtractor _textExtractor;
        #endregion

        #region "INativeOutputSettingsAware members"
        public void GetProposedOutputFileInfo(IPersistentFileConversionProperties fileProperties, IOutputFileInfo proposedFileInfo)
        {
            _originalFileProperties = fileProperties;
        }

        public void SetOutputProperties(INativeOutputFileProperties properties)
        {
            _nativeFileProperties = properties;
        }
        #endregion


        #region "IBilingualWriter members"


        #region "load file"
        public void SetFileProperties(IFileProperties fileInfo)
        {
            _targetFile = new XmlDocument();
            _targetFile.Load(_originalFileProperties.OriginalFilePath);
        }

        #region "initialize"
        public void Initialize(IDocumentProperties documentInfo)
        {
            _textExtractor = new BilTextExtractor();
        }
        #endregion

        #endregion

        #region "paragraphs"
        public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
        {
            string unitId = paragraphUnit.Properties.Contexts.Contexts[1].GetMetaData("UnitID");
            XmlNode xmlUnit = _targetFile.SelectSingleNode("//unit[@id='" + unitId + "']");

            // call helper function to generate the paragraph unit
            CreateParagraphUnit(paragraphUnit, xmlUnit);
        }
        #endregion

        #region "create paragraph"
        private void CreateParagraphUnit(IParagraphUnit paragraphUnit, XmlNode xmlUnit)
        {
            XmlNode source = xmlUnit.SelectSingleNode("source");
            XmlNode target = xmlUnit.SelectSingleNode("target");

            //iterate all segment pairs
            foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
            {
                source.InnerXml = _textExtractor.GetPlainText(segmentPair.Source);
                target.InnerXml = _textExtractor.GetPlainText(segmentPair.Target);
            }
        }
        #endregion

        #region "save file and complete"
        public void FileComplete()
        {
            _targetFile.Save(_nativeFileProperties.OutputFilePath);
            _targetFile = null;
        }

        public void Complete()
        {

        }
        #endregion

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
```
***

See Also
--



[Adding the File Writer Class](adding_the_file_writer_class.md)

[Adding the Text Extractor Class](adding_the_text_extractor_class.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.