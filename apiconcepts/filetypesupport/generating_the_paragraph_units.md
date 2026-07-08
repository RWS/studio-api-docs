# Generating the Paragraph Units

Extend the writer class to output the actual paragraph units from the intermediary (SDLXliff) file to the target BIL document.

## Modify the Function for Processing Paragraph Units

Add the required application logic to the `ProcessParagraphUnit()` method, which loops through the paragraph units in the intermediary document. The function works like this:

1. Retrieve the id of the current paragraph unit from the context metadata (we added the unit id as hidden meta information; see [Adding Context Information](adding_context_information.md))
2. Use the id to locate the corresponding `unit` in the original BIL document
3. Call a separate helper function that replaces the segments from the original BIL document with the paragraph unit content from the intermediary document

The method should look as follows:

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

For this simple implementation, we assume every paragraph unit in the intermediary document corresponds to a unit id from the original BIL file. In a production implementation, you should handle cases where a given id doesn't exist.

## Add the Helper Function to Generate Target Paragraph Units

Implement the helper function that generates the new BIL unit elements from the source and target segments in the intermediary file. This function uses the `BilTextExtractor` class created previously (see [Adding the Text Extractor Class](adding_the_text_extractor_class.md)).

Declare a new global text extractor member derived from this class:

# [C#](#tab/tabid-2)
```cs
private BilTextExtractor _textExtractor;
```

Use the `Initialize()` method to create a text extractor object:

# [C#](#tab/tabid-3)
```cs
public void Initialize(IDocumentProperties documentInfo)
{
    _textExtractor = new BilTextExtractor();
}
```

Add the following helper function. It works as follows:

1. Retrieve the `source` and `target` elements from the current BIL `unit` element
2. Loop through the segment pairs of the current paragraph unit from the intermediary (SDLXliff) document
3. Use the text extractor object to retrieve the plain text and tag pairs for each segment pair
4. Set the `source` and `target` elements to the strings retrieved from the text extractor object

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

This implementation loops through all segment pairs, though our simplified format assumes each unit contains only one segment pair. If the BIL format allowed multiple `seg` elements within a `source` or `target` node, a paragraph unit would contain several segment pairs.

## Putting It All Together

The enhanced writer class should now look as follows:

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
        private IPersistentFileConversionProperties _originalFileProperties;
        private INativeOutputFileProperties _nativeFileProperties;
        private XmlDocument _targetFile;
        private BilTextExtractor _textExtractor;

        public void GetProposedOutputFileInfo(IPersistentFileConversionProperties fileProperties, IOutputFileInfo proposedFileInfo)
        {
            _originalFileProperties = fileProperties;
        }

        public void SetOutputProperties(INativeOutputFileProperties properties)
        {
            _nativeFileProperties = properties;
        }

        public void SetFileProperties(IFileProperties fileInfo)
        {
            _targetFile = new XmlDocument();
            _targetFile.Load(_originalFileProperties.OriginalFilePath);
        }

        public void Initialize(IDocumentProperties documentInfo)
        {
            _textExtractor = new BilTextExtractor();
        }

        public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
        {
            string unitId = paragraphUnit.Properties.Contexts.Contexts[1].GetMetaData("UnitID");
            XmlNode xmlUnit = _targetFile.SelectSingleNode("//unit[@id='" + unitId + "']");

            // call helper function to generate the paragraph unit
            CreateParagraphUnit(paragraphUnit, xmlUnit);
        }

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

        public void FileComplete()
        {
            _targetFile.Save(_nativeFileProperties.OutputFilePath);
            _targetFile = null;
        }

        public void Complete()
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
```

## See Also

- [Adding the File Writer Class](adding_the_file_writer_class.md)
- [Adding the Text Extractor Class](adding_the_text_extractor_class.md)

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
