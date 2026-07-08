# Mapping the Segment Confirmation Levels

Enhance your writer component to map SDLXliff confirmation level values back to BIL status values.

## Enhance the Writer Class to Update the Status Values

The parser component maps the ```status``` attribute values of BIL units to the corresponding confirmation levels used in SDLXliff (see [Applying the Segment Pair Confirmation Levels](applying_the_segment_pair_confirmation_levels.md)). The writer component reverses this mapping. For example, a unit with status *new* (no target segment) receives a translation in Var:ProductName and the translator confirms it. The status updates to *exact*.

Modify the ```CreateParagraphUnit()``` helper function to call a separate helper function that returns the BIL status value mapped from the paragraph unit confirmation level. Update the status attribute of the XML ```unit``` element using the value returned by ```UpdateStatus()```:

# [C#](#tab/tabid-1)
```cs
private void CreateParagraphUnit(IParagraphUnit paragraphUnit, XmlNode xmlUnit)
{
    XmlNode source = xmlUnit.SelectSingleNode("source");
    XmlNode target = xmlUnit.SelectSingleNode("target");

    foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
    {
        source.InnerXml = _textExtractor.GetPlainText(segmentPair.Source);
        target.InnerXml = _textExtractor.GetPlainText(segmentPair.Target);
        xmlUnit.SelectSingleNode("./@status").Value = UpdateStatus(segmentPair.Properties.ConfirmationLevel);
    }
}
```
***

Add the ```UpdateStatus()``` helper function:

# [C#](#tab/tabid-2)
```cs
private string UpdateStatus(ConfirmationLevel unitLevel)
{
    string status = "";

    switch (unitLevel)
    {
        case ConfirmationLevel.Translated:
            status = "exact";
            break;
        case ConfirmationLevel.Draft:
            status = "fuzzy";
            break;
        case ConfirmationLevel.Unspecified:
            status = "new";
            break;
        default:
            status = "new";
            break;
    }

    return status;
}
```
***

## Putting It All Together

Your enhanced writer class now includes all required functionality:

# [C#](#tab/tabid-3)
```cs
using System;
using System.Xml;
using Sdl.Core.Globalization;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.Snippets.Bilingual
{
    class BilWriter3 : AbstractBilingualFileTypeComponent, IBilingualWriter, INativeOutputSettingsAware
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
            CreateParagraphUnit(paragraphUnit, xmlUnit);
        }

        private void CreateParagraphUnit(IParagraphUnit paragraphUnit, XmlNode xmlUnit)
        {
            XmlNode source = xmlUnit.SelectSingleNode("source");
            XmlNode target = xmlUnit.SelectSingleNode("target");

            foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
            {
                source.InnerXml = _textExtractor.GetPlainText(segmentPair.Source);
                target.InnerXml = _textExtractor.GetPlainText(segmentPair.Target);
                xmlUnit.SelectSingleNode("./@status").Value = UpdateStatus(segmentPair.Properties.ConfirmationLevel);
            }
        }

        private string UpdateStatus(ConfirmationLevel unitLevel)
        {
            string status = "";

            switch (unitLevel)
            {
                case ConfirmationLevel.Translated:
                    status = "exact";
                    break;
                case ConfirmationLevel.Draft:
                    status = "fuzzy";
                    break;
                case ConfirmationLevel.Unspecified:
                    status = "new";
                    break;
                default:
                    status = "new";
                    break;
            }

            return status;
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
***

## See Also

- [Applying the Segment Pair Confirmation Levels](applying_the_segment_pair_confirmation_levels.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
