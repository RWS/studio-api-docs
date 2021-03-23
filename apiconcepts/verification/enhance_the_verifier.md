Enhance the Verifier
====

In this chapter you will learn learn how to enhance your plug-in to take inline tag differences into account or to ignore inline tags.

Enhance the Verifier Plug-in
---

Let us assume the following: the verifier plug-in needs to be able to 'decide' whether a target segment is identical to the corresponding source segment, even when there is a difference in tags. This means that the user should be able to configure whether tag differences should be taken into account when applying the verification, or whether only text changes should be considered to be differences. If tags are ignored, then the following segments would be regarded as being identical:

```cs
This is a headline.
```

```cs
This is a <u>headline</u>.
```

Add a TextGenerator Class
-----
To fulfil this requirement you need to extend the plug-in, so that either the plain text (without tags) is be compared or the text with tag content. Start by adding a new a class called e.g. `sTextGenerator.cs` to your project.

This class will be responsible for generating the plain text content from the segment pairs. In this class, reference the namespace **Sdl.FileTypeSupport.Framework.BilingualApi** and have it implement the **IMarkupDataVisitor** interface. The skeleton class should look as shown below:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Sdl.Verification.Sdk.IdenticalCheck.Extended
{
    class TextGenerator : IMarkupDataVisitor
    {
    }
}
```

Continue by adding two new members to the class: a string builder for retrieving the plain text and a boolean setting to determine whether to include tag text or not:

```cs
internal StringBuilder PlainText
{
    get;
    set;
}

internal bool IncludeTagText
{
    get;
    set;
}
```

Now add a string method for retrieving the plain segment text. This method takes a segment object and a boolean parameter that determines whether tag text (if any) should be included when building the string or not.

```cs
// Returns the plain text representation of a segment. 
// If the includeTagText parameter is true, the returned string will 
// also contain the tag content for each tag.
public string GetPlainText(ISegment segment, bool includeTagText)
{
    PlainText = new StringBuilder("");
    IncludeTagText = includeTagText;
    VisitChildren(segment);
    return PlainText.ToString();
}
```
Note that a segment is basically a container that can have different types of sub-items (e.g. tag pairs, standalone tags, etc.). Add a method for iterating through the sub-items of the segment object:

```cs
// Iterates all sub items of segment container (IMarkupDataContainer)
private void VisitChildren(IAbstractMarkupDataContainer container)
{
    foreach (var item in container)
    {
        item.AcceptVisitor(this);
    }
}
```

Last, you need to add a number of class members, each of which processes a particular type of sub-item. Not all of these members might be required for the functionality of your plug-in. For example, in our simple implementation we do not need to consider revision markers, comments, etc. However, all of these members need to be present according to the **IMarkupDataVisitor** interface. Actually, we are mainly interested in opening and closing tags, as well as standalone placeholder tags. Depending on whether the value of the `IncludeTagText` boolean property is True or False, we append the corresponding tag text to the segment text (which should be checked) or not.

```cs
public void VisitCommentMarker(ICommentMarker commentMarker)
{
    VisitChildren(commentMarker);
}

public void VisitLocationMarker(ILocationMarker location)
{
    // Not required for this implementation.
}

public void VisitLockedContent(ILockedContent lockedContent)
{
    PlainText.Append(lockedContent.Content);
}

public void VisitOtherMarker(IOtherMarker marker)
{
    VisitChildren(marker);
}

public void VisitPlaceholderTag(IPlaceholderTag tag)
{
    if (tag.Properties.HasTextEquivalent && !IncludeTagText)
    {
        PlainText.Append(tag.Properties.TextEquivalent);
    }

    if (IncludeTagText)
    {
        PlainText.Append(tag.TagProperties.TagContent);
    }
}

public void VisitRevisionMarker(IRevisionMarker revisionMarker)
{
    // Not required for this implementation.
}

public void VisitSegment(ISegment segment)
{
    VisitChildren(segment);
}

public void VisitTagPair(ITagPair tagPair)
{
    if (IncludeTagText)
    {
        PlainText.Append(tagPair.StartTagProperties.TagContent);
    }

    VisitChildren(tagPair);

    if (IncludeTagText)
    {
        PlainText.Append(tagPair.EndTagProperties.TagContent);
    }
}

public void VisitText(IText text)
{
    PlainText.Append(text.Properties.Text);
}
```

The Complete Text Extractor Class
----
The complete TextGenerator class should now look as shown below:

```cs
using System.Text;

using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Sdl.Verification.Sdk.IdenticalCheck.Extended
{
    /// <summary>
    /// This class is used to traverse all elements that can occur inside a segment, e.g.
    /// text, tags, comments, placeholders, etc. In our implementation it is used to
    /// retrieve the plain text information (if required).
    /// </summary>
    public class TextGenerator : IMarkupDataVisitor
    {
        #region settings
        internal StringBuilder PlainText
        {
            get;
            set;
        }

        internal bool IncludeTagText
        {
            get;
            set;
        }
        #endregion

        #region "get plain text"
        // Returns the plain text representation of a segment. 
        // If the includeTagText parameter is true, the returned string will 
        // also contain the tag content for each tag.
        public string GetPlainText(ISegment segment, bool includeTagText)
        {
            PlainText = new StringBuilder("");
            IncludeTagText = includeTagText;
            VisitChildren(segment);
            return PlainText.ToString();
        }
        #endregion

        #region "iterate sub-items"
        // Iterates all sub items of segment container (IMarkupDataContainer)
        private void VisitChildren(IAbstractMarkupDataContainer container)
        {
            foreach (var item in container)
            {
                item.AcceptVisitor(this);
            }
        }
        #endregion



        #region "IMarkupDataVisitor Members"
        public void VisitCommentMarker(ICommentMarker commentMarker)
        {
            VisitChildren(commentMarker);
        }

        public void VisitLocationMarker(ILocationMarker location)
        {
            // Not required for this implementation.
        }

        public void VisitLockedContent(ILockedContent lockedContent)
        {
            PlainText.Append(lockedContent.Content);
        }

        public void VisitOtherMarker(IOtherMarker marker)
        {
            VisitChildren(marker);
        }

        public void VisitPlaceholderTag(IPlaceholderTag tag)
        {
            if (tag.Properties.HasTextEquivalent && !IncludeTagText)
            {
                PlainText.Append(tag.Properties.TextEquivalent);
            }

            if (IncludeTagText)
            {
                PlainText.Append(tag.TagProperties.TagContent);
            }
        }

        public void VisitRevisionMarker(IRevisionMarker revisionMarker)
        {
            // Not required for this implementation.
        }

        public void VisitSegment(ISegment segment)
        {
            VisitChildren(segment);
        }

        public void VisitTagPair(ITagPair tagPair)
        {
            if (IncludeTagText)
            {
                PlainText.Append(tagPair.StartTagProperties.TagContent);
            }

            VisitChildren(tagPair);

            if (IncludeTagText)
            {
                PlainText.Append(tagPair.EndTagProperties.TagContent);
            }
        }

        public void VisitText(IText text)
        {
            PlainText.Append(text.Properties.Text);
        }
        #endregion
    }
 }
 ```

Add a New Control to the Plug-in User Interface
------
Of course, the new setting needs to be reflected on the plug-in settings page. For this reason, add a check box control named **cb_ConsiderTags**. Then switch to the code view of the control, and add the following boolean `ConsiderTags` property as the programmatic representation of the control element:

```cs
public bool ConsiderTags
{
    get
    {
        return cb_ConsiderTags.Checked;
    }
    set
    {
        cb_ConsiderTags.Checked = value;
    }

}
```

Enhance the Settings Class
------
In the next step you need to make certain that your `IdenticalVerifierSettings` class includes the boolean `ConsiderTags` setting:

```cs
// Define the setting constant.
private const string CheckContext_Setting = "CheckContext";
private const string ConsiderTags_Setting = "ConsiderTags";

// Return the value of the setting.
public Setting<string> CheckContext
{
    get { return GetSetting<string>(CheckContext_Setting); }
}

public Setting<bool> ConsiderTags
{
    get { return GetSetting<bool>(ConsiderTags_Setting); }
}
```
```cs
protected override object GetDefaultValue(string settingId)
{
    switch (settingId)
    {
        case "CheckContext":
            return (string)"H";
        case "ConsiderTags":
            return (bool)true;
        default:
            return base.GetDefaultValue(settingId);
    }
}
```

Enhance the Settings Page Controller Class
-----
The following members of the `IdenticalVerifierUIPage` class also need to include the new setting for loading, saving, and resetting the values:

```cs
public override void OnActivate()
{
    _Control.ContextToCheck = _ControlSettings.CheckContext;
    _Control.ConsiderTags = _ControlSettings.ConsiderTags;
}
```
```cs
public override void Save()
{
    _ControlSettings.CheckContext.Value = _Control.ContextToCheck;
    _ControlSettings.ConsiderTags.Value = _Control.ConsiderTags;
}
```

```cs
public override void ResetToDefaults()
{
    _ControlSettings.CheckContext.Reset();
    _Control.ContextToCheck = _ControlSettings.CheckContext;
    _Control.ConsiderTags = _ControlSettings.ConsiderTags;
}
```

Modify the Main Verifier Class
-----
After implementing the `TextGenerator` class, we need to make a small change to the main verifier class, so that it either retrieves the segment text only or the segment text plus tag text for verification.

First, add the following members to the `IdenticalVerifierMain` class, which is derived from the `TextGenerator`:

```cs
private TextGenerator _textGeneratorProcessor;

public TextGenerator TextGeneratorProcessor
{
    get
    {
        if (_textGeneratorProcessor == null)
        {
            _textGeneratorProcessor = new TextGenerator();
        }
        return _textGeneratorProcessor;
    }
}
```

We will call on this member from the `CheckParagraphUnit()` method to extract the segment text with or without tag text.
Now change the verification logic in the `CheckParagraphUnit()` method by making the following addition:

```cs
completeTextTarget += TextGeneratorProcessor.GetPlainText(segmentPair.Target, VerificationSettings.ConsiderTags.Value);
```

As you can see, the `GetPlainText()` helper function of the TextGenerator is called to retrieve the plain text without tags. However, by setting the boolean parameter (`includeTagText`) to True, you can retrieve the text with tag text. Of course, it makes sense to implement a setting on the user interface (e.g. a checkbox), which allows the users to make that decision at runtime. We will not cover this in this programming guide. However, the **Sdl.Verification.Sdk.IdenticalCheck.Extended** project, which is included in this SDK, contains the full code for the enhancement that is covered in this chapter (i.e. including the enhancements to the user interface).

The fully enhanced and modified `CheckParagraphUnit()` function should look as shown below:

```cs
private void CheckParagraphUnit(IParagraphUnit paragraphUnit)
{
    // Declare and reset target segment text.
    string completeTextTarget = "";

    // loop through the whole paragraph unit
    foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
    {
        // Determine if context information is available,
        // and if the context equals the one specified in the user interface.
        if (paragraphUnit.Properties.Contexts.Contexts.Count > 0 &&
            paragraphUnit.Properties.Contexts.Contexts[0].DisplayCode == VerificationSettings.CheckContext.Value)
        {
            // Check whether target differs from source.
            // If this is the case, then output a warning message
            if (segmentPair.Source.ToString() != segmentPair.Target.ToString())
            {
                // Generate the plain text information if ConsiderTags is not true.
                #region "GetPlainText"
                completeTextTarget += TextGeneratorProcessor.GetPlainText(segmentPair.Target, VerificationSettings.ConsiderTags.Value);
                #endregion

                #region ReportingMessage
                if (MessageReporter is IBilingualContentMessageReporterWithExtendedData)
                {
                    #region CreateExtendedData
                    string context = paragraphUnit.Properties.Contexts.Contexts[0].DisplayCode;
                    IdenticalVerifierMessageData extendedData = new IdenticalVerifierMessageData(completeTextTarget +
                        " - must be identical to source because the paragraph has context " + context + ".", segmentPair.Source);
                    #endregion

                    #region ReportingMessageWithExtendedData
                    IBilingualContentMessageReporterWithExtendedData extendedMessageReporter = (IBilingualContentMessageReporterWithExtendedData)MessageReporter;
                    extendedMessageReporter.ReportMessage(this, PluginResources.Plugin_Name,
                        ErrorLevel.Warning, PluginResources.Error_NotIdentical,
                        new TextLocation(new Location(segmentPair.Target, true), 0),
                        new TextLocation(new Location(segmentPair.Target, false), segmentPair.Target.ToString().Length - 1),
                        extendedData);
                    #endregion

                }
                else
                {
                    #region ReportingMessageWithoutExtendedData
                    MessageReporter.ReportMessage(this, PluginResources.Plugin_Name,
                        ErrorLevel.Warning, PluginResources.Error_NotIdentical,
                        new TextLocation(new Location(segmentPair.Target, true), 0),
                        new TextLocation(new Location(segmentPair.Target, false), segmentPair.Target.ToString().Length - 1));
                    #endregion
                }
                #endregion
            }

        }
    }
}
```