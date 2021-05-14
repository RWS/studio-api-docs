Outputting all Comments
==

In this chapter we will finalize the writer class by making it write any comments attached to the units of the original BIL file as well as any comments that were added to the target segments during translation in <Var:ProductName>.

About Comments
--

According to the specifications of our fictitious BIL format (see [About the Example BIL Format](about_the_example_bil_format.md)) a unit element may include one or more ```comment``` elements, each of which has its own id.

In <Var:ProductName> translators can add comments to target segments. The commented strings are then highlighted with a specific background colour. The comment text is visible, for example, when the user moves the mouse pointer over the commented string (see illustration below).

![Comment](images/Comment.jpg)

In a BIL file comments are attached to the ```unit``` elements. This is where we will add any comments found in a segment pair when generating the native target file.

Extend the Text Extractor Class
--

When we implemented the ```BilTextExtractor``` class (see [Adding the Text Extractor Class](adding_the_text_extractor_class.md)) we did not process any comments attached to the segments. We want to change this behavior by making the following additions to this class. As a unit might contain more than one comment, you should create a global string list object. Then add a ```GetSegmentComment()``` helper function. This function first clears the comments list, and then calls the 'visitor' members to collect the required information from a given segment. Note that this function will later be called through the writer class.

The [VisitCommentMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IMarkupDataVisitor_VisitCommentMarker_Sdl_FileTypeSupport_Framework_BilingualApi_ICommentMarker_) member, which we originally left empty, should look as shown below:

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

This member actually 'visits' the segments and adds any comments found to our comments string list.


Enhance the Writer Class
--

Now we need to make some additions to our writer class. Start by adding yet another helper function called, e.g. ```UpdateComments()```. This helper function needs to be invoked from ```CreateParagraphUnit()``` and takes the current paragraph unit and the ```unit``` XML node as parameters, i.e.:

```cs
UpdateComments(paragraphUnit, xmlUnit);
```

The ```UpdateComments()``` helper function first 'wipes the slate clean' by removing any existing comments from the original BIL file:

```cs
// clear the original comments
XmlNodeList comments = unitNode.SelectNodes("comment");
foreach (XmlNode item in comments)
{
    ((XmlElement)unitNode).RemoveChild(item);
}
```


Now we need to compile a consolidated list of all comments (from the original BIL file) and of those comments that were added by the translator. Remember that each ```comment``` element in the BIL file requires an id, which we assign through a ```commentID``` integer variable.

First, we retrieve the comments that we added from the original BIL file and that are attached to the paragraph units:

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


Next, we collect the comments that the translator added in the intermediary (SDL XLIFF) file to the target segments. Note that we invoke the corresponding function from our ```BilTextExtractor``` class, which 'visits' the markup elements found within a segment.

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

The complete helper function looks as shown below:

```cs
private void UpdateComments(IParagraphUnit paragraphUnit, XmlNode unitNode)
{
    #region "clear"
    // clear the original comments
    XmlNodeList comments = unitNode.SelectNodes("comment");
    foreach (XmlNode item in comments)
    {
        ((XmlElement)unitNode).RemoveChild(item);
    }
    #endregion


    // loop through the comments in the SDLXLIFF paragraph units (if available)
    // these comments were added from the original BIL file 
    // during parsing (i.e. forward conversion)
    #region "original comments"
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
    #endregion


    // generate the consolidated list of comments from the original 
    // BIL file and any comments added by the translator in the SDLXLIFF file
    #region "tranlator comments"
    foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
    {
        foreach (string comment in _textExtractor.GetSegmentComment(segmentPair.Target))
        {
            AddComment(unitNode, comment, commentID);
            commentID++;
        }
    }
    #endregion
}
```

Note that the above ```UpdateComments()``` function calls a separate ```AddComment()``` helper function, which generates the actual comment based on the comment text and the comment id, which is then added to the current ```unit``` element of the target BIL file. This helper function looks as shown below:

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

Putting it All Together
--

The complete and finished writer class should now look as shown below:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.Core.Globalization;

namespace Sdl.Sdk.FileTypeSupport.Samples.Bil
{
    class BilWriter : AbstractBilingualFileTypeComponent, IBilingualWriter, INativeOutputSettingsAware
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
            //old: string unitId = paragraphUnit.Properties.Contexts.Contexts[1].MetaData["UnitID"];
            string unitId = paragraphUnit.Properties.Contexts.Contexts[1].GetMetaData("UnitID");
            XmlNode xmlUnit = _targetFile.SelectSingleNode("//unit[@id='" + unitId + "']");

            // call helper function to generate the paragraph unit
            CreateParagraphUnit(paragraphUnit, xmlUnit);
            // call helper function to consolidate all comments
            UpdateComments(paragraphUnit, xmlUnit);
        }
        #endregion

        #region "helper functions"

        #region "create paragraph"
        private void CreateParagraphUnit(IParagraphUnit paragraphUnit, XmlNode xmlUnit)
        {
            XmlNode source = xmlUnit.SelectSingleNode("source");
            XmlNode target = xmlUnit.SelectSingleNode("target");

            // iterate all segment pairs
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

        #region "update comments"
        private void UpdateComments(IParagraphUnit paragraphUnit, XmlNode unitNode)
        {
            #region "clear"
            // clear the original comments
            XmlNodeList comments = unitNode.SelectNodes("comment");
            foreach (XmlNode item in comments)
            {
                ((XmlElement)unitNode).RemoveChild(item);
            }
            #endregion


            // loop through the comments in the SDLXLIFF paragraph units (if available)
            // these comments were added from the original BIL file 
            // during parsing (i.e. forward conversion)
            #region "original comments"
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
            #endregion


            // generate the consolidated list of comments from the original 
            // BIL file and any comments added by the translator in the SDLXLIFF file
            #region "tranlator comments"
            foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
            {
                foreach (string comment in _textExtractor.GetSegmentComment(segmentPair.Target))
                {
                    AddComment(unitNode, comment, commentID);
                    commentID++;
                }
            }
            #endregion
        }
        #endregion

        #region "add comment"
        private void AddComment(XmlNode xmlUnit, string commentText, int commentID)
        {
            XmlElement commentElement = _targetFile.CreateElement("comment");
            XmlAttribute commentIdAttrib = _targetFile.CreateAttribute("id");
            commentIdAttrib.Value = commentID.ToString();
            commentElement.Attributes.Append(commentIdAttrib);
            commentElement.InnerText = commentText;
            xmlUnit.AppendChild(commentElement);
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

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            //don't need to dispose of anthing
        }

        #endregion
    }
}
```

Below you also see the complete and finished text extractor class:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Sdl.Sdk.FileTypeSupport.Samples.Bil
{
    class BilTextExtractor : IMarkupDataVisitor
    {
        #region "comment list"
        private List<string> _Comments = new List<string>();

        public List<string> GetSegmentComment(ISegment segment)
        {
            _Comments.Clear();
            VisitChildren(segment);
            return _Comments;
        }
        #endregion

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
            for (int i = 0; i < commentMarker.Comments.Count; i++)
            {
                _Comments.Add(commentMarker.Comments.GetItem(i).Text);
            }
            VisitChildren(commentMarker);
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

[Extracting Comments](extracting_comments.md)

[Adding the File Writer Class](adding_the_file_writer_class.md)

[Adding the Text Extractor Class](adding_the_text_extractor_class.md)

>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.