Implementing filtering capabilities to your Display Filter
======
Add the basic filtering functionality to your custom filter.

To work with the Display Filter API, you need to create a class that implements the [IDisplayFilter](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.IDisplayFilter.yml) interface from the [Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.yml) namespace, which exposes one method called [EvaluateRow](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.IDisplayFilter.yml#Sdl_TranslationStudioAutomation_IntegrationApi_DisplayFilters_IDisplayFilter_EvaluateRow_Sdl_TranslationStudioAutomation_IntegrationApi_DisplayFilters_IDisplayFilterRowInfo_). This method will then be called by the API for each row after the filter is applied on the document, passing in the [IDisplayFilterRowInfo](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.IDisplayFilterRowInfo.yml) model (which includes the [ISegmentPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ISegmentPair.yml) and other relevant properties) that will permit the developer to assert whether or not the segments are visible in the editor.

Create a new class called `FilterSettings`. This class will manage some basic settings that will persist on the document once the filter has been applied. Make reference to the following example:


To recover the source and target content, we will need to create a class that implements the [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml). This interface is designed in such a way that you decide what properties from the [ISegment](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ISegment.yml) are relevant and then process only those.

For the purpose of this example, we will recover the content as plain text only, including tags and revisions. This should be sufficient in allowing us to apply a filter to the content.

Create a new class called `ContentProcessor` with reference to the following code:
# [C#](#tab/tabid-1)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace TranslationStudio.Plugins.AdvancedDisplayFilter.Content
{
    public class ContentProcessor : IMarkupDataVisitor
    {

        public ContentProcessor()
        {
            Initialize();
        }

        private void Initialize()
        {
            PlainText = new StringBuilder("");
            Comments  = new List<IComment>();
        }
        public List<IComment> Comments { get; set; }
        public StringBuilder PlainText { get; set; }

        public bool IncludeTagContent { get; set; }

        public bool IgnoreComments { get; set; }

        public string GetPlainText(ISegment segment, bool includeTagContent)
        {
            IgnoreComments = true;
            IncludeTagContent = includeTagContent;
            Initialize(); 
            VisitChildren(segment);
            return PlainText.ToString();
        }

        public List<IComment> GetSegmentComments(ISegment segment)
        {
            IgnoreComments = false;
            Initialize();
            VisitChildren(segment);
            return Comments;
        }

        public void ProcessSegment(ISegment segment)
        {
            Initialize();
            VisitChildren(segment);
        }

        private void VisitChildren(IAbstractMarkupDataContainer container)
        {
            if (container == null)
                return;
            foreach (var item in container)
            {
                item.AcceptVisitor(this);
            }
        }

        public void VisitCommentMarker(ICommentMarker commentMarker)
        {
            if (!IgnoreComments)
            {
                foreach (var commentProperty in commentMarker.Comments.Comments)
                {
                    Comments.Add(commentProperty);
                }
            }
            VisitChildren(commentMarker);
        }

        public void VisitLocationMarker(ILocationMarker location)
        {
            // Not required for this implementation
        }

        public void VisitLockedContent(ILockedContent lockedContent)
        {            
            PlainText.Append(lockedContent.Content.ToString());
        }

        public void VisitOtherMarker(IOtherMarker marker)
        {
            VisitChildren(marker);
        }

        public void VisitPlaceholderTag(IPlaceholderTag tag)
        {
            if (IncludeTagContent)
                PlainText.Append("<" + tag.Properties.TagContent + "/>");
        }

        public void VisitRevisionMarker(IRevisionMarker revisionMarker)
        {
            VisitChildren(revisionMarker);
        }

        public void VisitSegment(ISegment segment)
        {
            VisitChildren(segment);
        }

        public void VisitTagPair(ITagPair tagPair)
        {
            if (IncludeTagContent)
                PlainText.Append("<" + tagPair.StartTagProperties.TagContent + ">");
            VisitChildren(tagPair);
            if (IncludeTagContent)
                PlainText.Append("</" + tagPair.EndTagProperties.TagContent + ">");
        }

        public void VisitText(IText text)
        {
            PlainText.Append(text.Properties.Text);
        }
    }
}
```
****

Next, create a new class called `DisplayFilter`. This will implement the [IDisplayFilter](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.IDisplayFilter.yml) interface so that we can evaluate each of the segments and decide whether or not they should be displayed in the Var:ProductName Editor.

It is good design to include a reference to `IFilterSetting` interface in this class as it will be persisted on the document once the filter has been applied. This is useful to understand the type of filter that is applied (if any), especially in the case when the user is moving between documents in the editor. It permits the developer to differentiate between the internal system filter provider and their own implementation or multiple implementations and then take the appropriate action based on that.

This example demonstrates how to implement a few filters that are evaluated as the API is iterating over each of the segments after the filter has been applied on the document. Further on in the walkthrough we will discuss how to apply this implementation of the [IDisplayFilter](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters.IDisplayFilter.yml) on the document itself.

You will notice from the filter examples that we are using the `ContentProcessor` functionality that we created earlier to recover the plain text from both the source and target segments and then apply either a regular expression or normal search as criteria for the filter.

# [C#](#tab/tabid-2)
```cs
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.TranslationStudio.Plugins.AdvancedDisplayFilter.Content;
using Sdl.TranslationStudio.Plugins.AdvancedDisplayFilter.Controls;
using Sdl.TranslationStudio.Plugins.AdvancedDisplayFilter.Models;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters;

namespace TranslationStudio.Plugins.AdvancedDisplayFilter.DisplayFilters
{
    public class DisplayFilter : IDisplayFilter
    {
        #region  |  Public  |

        /// <summary>
        /// Display filter settings
        /// </summary>
        public IDisplayFilterSettings Settings { get; private set; }


        #endregion
        #region  |  Private  |

        private ContentProcessor ContentProcessor { get; set; }

        private Document ActiveDocument { get; set; }

        #endregion
        #region  |  Constructor  |
        public DisplayFilter(IDisplayFilterSettings settings, Document document)
        {
            ContentProcessor = new ContentProcessor();
            ActiveDocument = document;
            Settings = settings;
        }

        #endregion


        public bool EvaluateRow(DisplayFilterRowInfo rowInfo)
        {
            var success = !(!Settings.ShowAllContent && !rowInfo.IsSegment);

            if (rowInfo.IsSegment)
            {
                if (success && Settings.SegmentReviewTypes != null && Settings.SegmentReviewTypes.Any())
                    success = IsSegmentReviewTypes(rowInfo);


                if (success && Settings.ConfirmationLevels != null && Settings.ConfirmationLevels.Any())
                    success = IsConfirmationLevelFound(rowInfo);


                if (success && Settings.OriginTypes != null && Settings.OriginTypes.Any())
                    success = IsOriginTypeFound(rowInfo);


                if (success && Settings.PreviousOriginTypes != null && Settings.PreviousOriginTypes.Any())
                    success = IsPreviousOriginTypeFound(rowInfo);


                if (success && Settings.RepetitionTypes != null && Settings.RepetitionTypes.Any())
                    success = IsRepetitionTypes(rowInfo);


                if (success && Settings.SegmentLockingTypes != null && Settings.SegmentLockingTypes.Any())
                    success = IsSegmentLockingTypes(rowInfo);


                if (success && Settings.SegmentContentTypes != null && Settings.SegmentContentTypes.Any())
                    success = IsSegmentContentTypes(rowInfo);


                if (success && Settings.SourceText.Trim() != string.Empty)
                    success = IsTextFoundInSource(rowInfo);


                if (success && Settings.TargetText.Trim() != string.Empty)
                    success = IsTextFoundInTarget(rowInfo);


                if (success && Settings.CommentText.Trim() != string.Empty)
                    success = IsTextFoundInComment(rowInfo);


                if (success && Settings.CommentAuthor.Trim() != string.Empty)
                    success = IsAuthorFoundInComment(rowInfo);


                if (success && Settings.CommentSeverity > 0)
                    success = IsSeverityFoundInComment(rowInfo);


                if (success && Settings.ContextInfoTypes.Any())
                    success = IsContextInfoTypes(rowInfo);
            }

            return success;
        }


        #region  |  Helpers  |

        private bool IsSegmentReviewTypes(DisplayFilterRowInfo rowInfo)
        {
            var success = SegmentWithTQAs(rowInfo)
                || SegmentWithTrackedChanges(rowInfo)
                || SegmentWithComments(rowInfo)
                || SegmentWithMessages(rowInfo);

            return success;
        }
        private bool SegmentWithTQAs(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.SegmentReviewTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.SegmentReviewType.WithTQA.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);

            if (success && !rowInfo.ContainsTQAs)
                success = false;

            return success;
        }
        private bool SegmentWithTrackedChanges(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.SegmentReviewTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.SegmentReviewType.WithTrackedChanges.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);

            if (success && !rowInfo.ContainsTrackChanges)
                success = false;

            return success;
        }
        private bool SegmentWithComments(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.SegmentReviewTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.SegmentReviewType.WithComments.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);

            // check if comments exist in the target segment
            if (success && !ContentProcessor.GetSegmentComments(rowInfo.SegmentPair.Target).Any())
            {
                // check if comments exit in the source segment
                if (!ContentProcessor.GetSegmentComments(rowInfo.SegmentPair.Source).Any())
                    success = false;
            }

            return success;
        }
        private bool SegmentWithMessages(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.SegmentReviewTypes.ToList()
               .Any(status => string.Compare(status, DisplayFilterSettings.SegmentReviewType.WithFeedbackMessages.ToString()
                   , StringComparison.OrdinalIgnoreCase) == 0);

            if (success && !rowInfo.ContainsFeedbackMessages)
                success = false;

            return success;
        }

        private bool IsSeverityFoundInComment(DisplayFilterRowInfo rowInfo)
        {
            var success = ContentProcessor.GetSegmentComments(rowInfo.SegmentPair.Target)
                .Any(comment => Settings.CommentSeverity == (int)comment.Severity);
            if (!success)
                success = ContentProcessor.GetSegmentComments(rowInfo.SegmentPair.Source)
                .Any(comment => Settings.CommentSeverity == (int)comment.Severity);

            return success;
        }
        private bool IsAuthorFoundInComment(DisplayFilterRowInfo rowInfo)
        {
            var success = ContentProcessor.GetSegmentComments(rowInfo.SegmentPair.Target)
                .Any(comment => StringMatch(Settings.CommentAuthor, comment.Author, false));
            if (!success)
                success = ContentProcessor.GetSegmentComments(rowInfo.SegmentPair.Source)
                .Any(comment => StringMatch(Settings.CommentAuthor, comment.Author, false));

            return success;
        }
        private bool IsTextFoundInComment(DisplayFilterRowInfo rowInfo)
        {
            var success = ContentProcessor.GetSegmentComments(rowInfo.SegmentPair.Target)
                .Any(comment => StringMatch(Settings.CommentText, comment.Text, false));
            if (!success)
                success = ContentProcessor.GetSegmentComments(rowInfo.SegmentPair.Source)
                .Any(comment => StringMatch(Settings.CommentText, comment.Text, false));

            return success;
        }

        private bool IsConfirmationLevelFound(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.ConfirmationLevels.ToList().Any(status => string.Compare(status
                , rowInfo.SegmentPair.Properties.ConfirmationLevel.ToString()
                , StringComparison.OrdinalIgnoreCase) == 0);

            return success;
        }

        private bool IsOriginTypeFound(DisplayFilterRowInfo rowInfo)
        {
            var success = false;

            var translationType =
                Helper.GetOriginType(rowInfo.SegmentPair.Properties.TranslationOrigin);

            success = Settings.OriginTypes.ToList()
                .Any(status => string.Compare(status, translationType.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);


            return success;
        }
        private bool IsPreviousOriginTypeFound(DisplayFilterRowInfo rowInfo)
        {
            var success = false;

            if (rowInfo.SegmentPair.Properties.TranslationOrigin.OriginBeforeAdaptation != null)
            {
                var previousTranslationType =
                    Helper.GetOriginType(
                        rowInfo.SegmentPair.Properties.TranslationOrigin.OriginBeforeAdaptation);
                if (
                    Settings.PreviousOriginTypes.ToList()
                        .Any(status => string.Compare(status,
                            previousTranslationType.ToString()
                            , StringComparison.OrdinalIgnoreCase) == 0))
                    success = true;
            }

            return success;
        }


        private bool IsRepetitionTypes(DisplayFilterRowInfo rowInfo)
        {
            var success = IsRepetitionsAll(rowInfo)
                || IsRepetitionsFirstOccurrences(rowInfo)
                || IsRepetitionsExcludingFirstOccurrences(rowInfo);

            return success;
        }
        private bool IsRepetitionsAll(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.RepetitionTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.RepetitionType.All.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);

            if (success)
                success = rowInfo.SegmentPair.Properties.TranslationOrigin.IsRepeated;

            return success;
        }
        private bool IsRepetitionsFirstOccurrences(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.RepetitionTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.RepetitionType.FirstOccurrences.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);

            if (success)
                success = rowInfo.RepetitionFirstOccurrence;

            return success;
        }
        private bool IsRepetitionsExcludingFirstOccurrences(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.RepetitionTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.RepetitionType.ExcludeFirstOccurrences.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);

            if (success)
                success = rowInfo.RepetitionExcludeFirstOccurrence;

            return success;
        }


        private bool IsSegmentLockingTypes(DisplayFilterRowInfo rowInfo)
        {
            var success = IsSegmentLockingTypeLocked(rowInfo)
                || IsSegmentLockingTypeUnLocked(rowInfo);

            return success;
        }
        private bool IsSegmentLockingTypeLocked(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.SegmentLockingTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.SegmentLockingType.Locked.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);

            if (success)
                success = rowInfo.SegmentPair.Properties.IsLocked;

            return success;
        }
        private bool IsSegmentLockingTypeUnLocked(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.SegmentLockingTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.SegmentLockingType.Unlocked.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);


            if (success)
                success = !rowInfo.SegmentPair.Properties.IsLocked;

            return success;
        }


        private bool IsSegmentContentTypes(DisplayFilterRowInfo rowInfo)
        {
            var success = IsSegmentContentTypeNumbersOnly(rowInfo)
                || IsSegmentContentTypeExcludingNumberOnly(rowInfo);

            return success;
        }
        private bool IsSegmentContentTypeNumbersOnly(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.SegmentContentTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.SegmentContentType.NumbersOnly.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);

            if (success)
            {
                success = NumericValidationUtil.IsValidFloatingNumber(ContentProcessor
                    .GetPlainText(rowInfo.SegmentPair.Source, false));
                if (!success)
                {
                    var text = ContentProcessor.GetPlainText(rowInfo.SegmentPair.Target, false);
                    success = NumericValidationUtil.IsValidFloatingNumber(text);
                }
            }

            return success;
        }
        private bool IsSegmentContentTypeExcludingNumberOnly(DisplayFilterRowInfo rowInfo)
        {
            var success = Settings.SegmentContentTypes.ToList()
                .Any(status => string.Compare(status, DisplayFilterSettings.SegmentContentType.ExcludeNumberOnly.ToString()
                    , StringComparison.OrdinalIgnoreCase) == 0);

            if (success)
            {
                success = NumericValidationUtil.IsValidFloatingNumber(ContentProcessor
                    .GetPlainText(rowInfo.SegmentPair.Source, false));
                if (success)
                {
                    var text = ContentProcessor.GetPlainText(rowInfo.SegmentPair.Target, false);
                    success = string.IsNullOrEmpty(text) || NumericValidationUtil.IsValidFloatingNumber(text);
                }
                success = !success;
            }

            return success;
        }




        private bool IsTextFoundInSource(DisplayFilterRowInfo rowInfo)
        {

            var text = ContentProcessor.GetPlainText(rowInfo.SegmentPair.Source, true);

            var success = Settings.IsRegularExpression
                ? RegularExpressionMatch(Settings.SourceText, text, Settings.IsCaseSensitive)
                : StringMatch(Settings.SourceText, text, Settings.IsCaseSensitive);

            return success;
        }
        private bool IsTextFoundInTarget(DisplayFilterRowInfo rowInfo)
        {
            var text = ContentProcessor.GetPlainText(rowInfo.SegmentPair.Target, true);

            var success = Settings.IsRegularExpression
                ? RegularExpressionMatch(Settings.TargetText, text, Settings.IsCaseSensitive)
                : StringMatch(Settings.TargetText, text, Settings.IsCaseSensitive);

            return success;
        }
        private static bool RegularExpressionMatch(string searchFor, string searchIn, bool isCaseSensitive)
        {
            var regex = new Regex(searchFor,
                RegexOptions.Singleline | (!isCaseSensitive ? RegexOptions.IgnoreCase : RegexOptions.None));
            var match = regex.Match(searchIn);

            return match.Success;
        }
        private static bool StringMatch(string searchFor, string searchIn, bool isCaseSensitive)
        {
            if (isCaseSensitive)
                return searchIn.IndexOf(searchFor, StringComparison.Ordinal) > -1 ? true : false;

            return searchIn.IndexOf(searchFor, StringComparison.OrdinalIgnoreCase) > -1 ? true : false;
        }




        private bool IsContextInfoTypes(DisplayFilterRowInfo rowInfo)
        {
            var success = false;

            if (rowInfo.ContextInfo.Count <= 0) return success;
            if (rowInfo.ContextInfo.Any(contextInfo => Settings.ContextInfoTypes.Contains(contextInfo.ContextType)))
            {
                success = true;
            }


            return success;
        }


        #endregion

    }
}
```
***
