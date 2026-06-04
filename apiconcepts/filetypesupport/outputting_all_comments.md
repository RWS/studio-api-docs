# Outputting All Comments

Finalize the writer class to write comments attached to units in the original BIL file and comments added to target segments during translation in Var:ProductName.

## About Comments

According to the BIL format specification (see [About the Example BIL Format](about_the_example_bil_format.md)), unit elements include one or more ```comment``` elements, each with its own id.

In Var:ProductName, translators add comments to target segments. The commented strings highlight with a specific background color. Comment text appears when users move the mouse pointer over the commented string (see illustration below).

![Comment](images/Comment.jpg)

BIL file comments attach to ```unit``` elements. When generating the native target file, add any comments found in a segment pair to these units.

## Extend the Text Extractor Class

When implementing the ```BilTextExtractor``` class (see [Adding the Text Extractor Class](adding_the_text_extractor_class.md)), comment processing was not included. Add a global string list object and a ```GetSegmentComment()``` helper function. This function clears the comments list, then calls visitor members to collect information from a given segment. The writer class calls this function later.

The [VisitCommentMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitCommentMarker_Sdl_FileTypeSupport_Framework_BilingualApi_ICommentMarker_) member visits segments and adds any found comments to the comments list:

# [C#](#tab/tabid-1)
```cs
public void VisitCommentMarker(ICommentMarker commentMarker)
{
    for (int i = 0; i < commentMarker.Comments.Count; i++)
    {
        _Comments.Add(commentMarker.Comments.GetItem(i).Text);
    }
    VisitChildren(commentMarker);
}
```

## Enhance the Writer Class

Add a helper function called ```UpdateComments()``` to the writer class. Invoke this function from ```CreateParagraphUnit()```, passing the current paragraph unit and XML ```unit``` node:

# [C#](#tab/tabid-2)
```cs
UpdateComments(paragraphUnit, xmlUnit);
```

The ```UpdateComments()``` helper function first removes any existing comments from the original BIL file:

# [C#](#tab/tabid-3)
```cs
XmlNodeList comments = unitNode.SelectNodes("comment");
foreach (XmlNode item in comments)
{
    ((XmlElement)unitNode).RemoveChild(item);
}
```

Create a consolidated list of all comments from the original BIL file and comments added by the translator. Each ```comment``` element in the BIL file requires an id, assigned through a ```commentID``` integer variable.

Retrieve comments added from the original BIL file and attached to the paragraph units:

# [C#](#tab/tabid-4)
```cs
int commentID = 1;
if (paragraphUnit.Properties.Comments != null)
{
    for (int i = 0; i < paragraphUnit.Properties.Comments.Count; i++)
    {
        IComment actualComment = paragraphUnit.Properties.Comments.GetItem(i);
        AddComment(unitNode, actualComment.Text, commentID);
        commentID++;
    }
}
```

Next, collect comments the translator added in the intermediate SDLXliff file to target segments. Invoke the corresponding function from the ```BilTextExtractor``` class, which visits markup elements within a segment:

# [C#](#tab/tabid-5)
```cs
foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
{
    foreach (string comment in _textExtractor.GetSegmentComment(segmentPair.Target))
    {
        AddComment(unitNode, comment, commentID);
        commentID++;
    }
}
```

The complete ```UpdateComments()``` helper function consolidates all comments:

# [C#](#tab/tabid-6)
```cs
private void UpdateComments(IParagraphUnit paragraphUnit, XmlNode unitNode)
{
    XmlNodeList comments = unitNode.SelectNodes("comment");
    foreach (XmlNode item in comments)
    {
        ((XmlElement)unitNode).RemoveChild(item);
    }

    int commentID = 1;
    if (paragraphUnit.Properties.Comments != null)
    {
        for (int i = 0; i < paragraphUnit.Properties.Comments.Count; i++)
        {
            IComment actualComment = paragraphUnit.Properties.Comments.GetItem(i);
            AddComment(unitNode, actualComment.Text, commentID);
            commentID++;
        }
    }

    foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
    {
        foreach (string comment in _textExtractor.GetSegmentComment(segmentPair.Target))
        {
            AddComment(unitNode, comment, commentID);
            commentID++;
        }
    }
}
```

The ```UpdateComments()``` function calls the ```AddComment()``` helper function, which creates a comment element based on the comment text and comment id, then adds it to the current ```unit``` element of the target BIL file:

# [C#](#tab/tabid-7)
```cs
private void AddComment(XmlNode xmlUnit, string commentText, int commentID)
{
    XmlElement commentElement = _targetFile.CreateElement("comment");
    XmlAttribute commentIdAttrib = _targetFile.CreateAttribute("id");
    commentIdAttrib.Value = commentID.ToString();
    commentElement.Attributes.Append(commentIdAttrib);
    commentElement.InnerText = commentText;
    xmlUnit.AppendChild(commentElement);
}
```

## Putting It All Together

Your complete and finished writer class now includes all required functionality:
# [C#](#tab/tabid-8)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.Core.Globalization;

namespace Sdk.FileTypeSupport.Samples.Bil
{
    class BilWriter : AbstractBilingualFileTypeComponent, IBilingualWriter, INativeOutputSettingsAware
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
            UpdateComments(paragraphUnit, xmlUnit);
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

        private void UpdateComments(IParagraphUnit paragraphUnit, XmlNode unitNode)
        {
            XmlNodeList comments = unitNode.SelectNodes("comment");
            foreach (XmlNode item in comments)
            {
                ((XmlElement)unitNode).RemoveChild(item);
            }

            int commentID = 1;
            if (paragraphUnit.Properties.Comments != null)
            {
                for (int i = 0; i < paragraphUnit.Properties.Comments.Count; i++)
                {
                    IComment actualComment = paragraphUnit.Properties.Comments.GetItem(i);
                    AddComment(unitNode, actualComment.Text, commentID);
                    commentID++;
                }
            }

            foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
            {
                foreach (string comment in _textExtractor.GetSegmentComment(segmentPair.Target))
                {
                    AddComment(unitNode, comment, commentID);
                    commentID++;
                }
            }
        }

        private void AddComment(XmlNode xmlUnit, string commentText, int commentID)
        {
            XmlElement commentElement = _targetFile.CreateElement("comment");
            XmlAttribute commentIdAttrib = _targetFile.CreateAttribute("id");
            commentIdAttrib.Value = commentID.ToString();
            commentElement.Attributes.Append(commentIdAttrib);
            commentElement.InnerText = commentText;
            xmlUnit.AppendChild(commentElement);
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
        }
    }
}
```

Your complete and finished text extractor class now includes all required functionality:
# [C#](#tab/tabid-9)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Sdk.FileTypeSupport.Samples.Bil
{
    class BilTextExtractor : IMarkupDataVisitor
    {
        private List<string> _Comments = new List<string>();

        public List<string> GetSegmentComment(ISegment segment)
        {
            _Comments.Clear();
            VisitChildren(segment);
            return _Comments;
        }

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
            for (int i = 0; i < commentMarker.Comments.Count; i++)
            {
                _Comments.Add(commentMarker.Comments.GetItem(i).Text);
            }
            VisitChildren(commentMarker);
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

- [Extracting Comments](extracting_comments.md)
- [Adding the File Writer Class](adding_the_file_writer_class.md)
- [Adding the Text Extractor Class](adding_the_text_extractor_class.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.

