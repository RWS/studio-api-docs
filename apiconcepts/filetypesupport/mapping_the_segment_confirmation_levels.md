Mapping the Segment Confirmation Levels
===

In this chapter we will enhance our writer component to map the SDLXliff confirmation level values back to the BIL status values.

Enhance the Writer Class to Update the Status Values
--

The parser component mapped the ```status``` attribute values of the BIL units to the corresponding confirmation levels that are used in SDLXliff (see [Applying the Segment Pair Confirmation Levels](applying_the_segment_pair_confirmation_levels.md)). The writer component needs to do the exact opposite. For example: in the original BIL file the status of a unit was *new*, as it did not have a target segment. In <Var:ProductName> the translation has been added and confirmed by the translator. Thus, the status should be 'upgraded' to exact.

First, modify the ```CreateParagraphUnit()``` helper function to call a separate helper function, which returns the status value that was mapped from the paragraph unit confirmation value used in SDLXliff. Then, the status attribute of the current BIL ```unit``` element is changed to the string value returned by the ```UpdateStatus()``` helper function:

# [C#](#tab/tabid-1)
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
        // update unit status
        xmlUnit.SelectSingleNode("./@status").Value = UpdateStatus(segmentPair.Properties.ConfirmationLevel);
    }
}
```
***

Now add the new ```UpdateStatus()``` helper function:

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

Putting it All Together
--

The enhanced writer class should now look as shown below:

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
                // update unit status
                xmlUnit.SelectSingleNode("./@status").Value = UpdateStatus(segmentPair.Properties.ConfirmationLevel);
            }
        }
        #endregion

        #region "confirmation level"
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



[Applying the Segment Pair Confirmation Levels](applying_the_segment_pair_confirmation_levels.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.