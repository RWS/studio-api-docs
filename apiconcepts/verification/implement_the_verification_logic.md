Implement the Verification Logic
=====
In this chapter you will learn how to implement the extension class that contains actual verification functionality of your plug-in.

Edit the Main Verifier Class
----
Now it is time to implement the verification logic in the main verification class (i.e. IdenticalVerifierMain.cs), which you added previously (see [Create a New Project](create_a_new_project.md)). Start by adding the following namespaces to this class:

* **Sdl.Verification.Api**
* **Sdl.Desktop.Platform.Settings**
* **Sdl.Desktop.Common**
* **Sdl.FileTypeSupport.Framework.BilingualApi**
* **Sdl.FileTypeSupport.Framework.NativeApi**
* **Sdl.Core.Settings**
  
This class needs to be preceded by the following declaration, which makes it an extension class, which is referenced in the plug-in manifest (see also [Create a New Project](create_a_new_project.md)).

# [C#](#tab/tabid-1)
```cs
[GlobalVerifier("Identical Segments Verifier", "Plugin_Name", "Plugin_Description")]
```
***

This line is what makes the plug-in be listed under Verification in the Options or in the Project (Template) Settings dialog box of <Var:ProductName>.
This class needs to implement the interfaces listed below:

* [IGlobalVerifier](../../api/verification/Sdl.Verification.Api.IGlobalVerifier.yml)
* [IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml)
* [ISharedObjectsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISharedObjectsAware.yml)
  
Moreover, add a private member to the class, which you call e.g. `_verificationSettings`. This object is derived from the `IdenticalVerifierSettings` class (see [Retrieve the Settings Values](retrieve_the_settings_values.md)) and it is used to access the plug-in settings, which influence how the actual verification is executed:

# [C#](#tab/tabid-2)
```cs
private ISharedObjects _sharedObjects;
private IdenticalVerifierSettings _verificationSettings;
```
***

Add the Plug-in Name, Icon and Description
----
Add the following members to implement the plug-in icon, name, and description. Note that these elements are drawn from the resources file (see [The Resources File](the_resources_file.md)). This controls what end users will see in the **Options** dialog box of <Var:ProductName> under **Verification** in terms of strings and icons.

# [C#](#tab/tabid-3)
```cs
public string Description
{
    get { return PluginResources.Verifier_Description; }
}

public System.Drawing.Icon Icon
{
    get { return PluginResources.icon; }
}

public string Name
{
    get { return PluginResources.Plugin_Name; }
}
```
***

Retrieve the Verifier Settings
-----
In the next step, add the following internal member. This member is used to get a handle on the settings bundle used for our implementation:

# [C#](#tab/tabid-4)
```cs
internal IdenticalVerifierSettings VerificationSettings
{
    get
    {
        if (_verificationSettings == null && _sharedObjects != null)
        {
            ISettingsBundle bundle = _sharedObjects.GetSharedObject<ISettingsBundle>("SettingsBundle");
            if (bundle != null)
            {
                _verificationSettings = bundle.GetSettingsGroup<IdenticalVerifierSettings>();
            }
        }
        return _verificationSettings;
    }
}
```
***

Add the Item Factory Member
-----
The item factory allows you to create, for example, tag pairs and placeholders. It is actually *not* required for the functionality of our verifier plug-in, however, it has to be present in our class according to the [IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml) interface.

# [C#](#tab/tabid-5)
```cs
public IDocumentItemFactory ItemFactory
{
    get;
    set;
}
```
***

Add the Message Reporter Member
-----
The message reporter is required by the [IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml) interface to implement the functionality of our verifier. Through this member you output messages (if any) to the **Messages** window of <Var:ProductName>. Therefore, this member is responsible for communicating any problems to the end user, who will then try to fix the reported problems.

# [C#](#tab/tabid-1)
[!code-csharp[TermVerifierMessageService](code_samples/TermVerifierMessageService.cs#L19-L21)]
***

Add Further Members of [IBilingualContentHandler](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml)
-----
The [IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml) interface requires you to add a number of further members, which are actually not required for the functionality of our particular sample plug-in. You can therefore leave these methods empty.

# [C#](#tab/tabid-6)
```cs
public void Complete()
{
    // Not required for this implementation.
}

public void FileComplete()
{
    // Not required for this implementation.
}

public void SetFileProperties(IFileProperties fileInfo)
{
    // Not required for this implementation.
}

public void Initialize(IDocumentProperties documentInfo)
{
    // Not required for this implementation.
}
```
***

> [!NOTE]
> An intermediary (XLIFF) document to verify might contain a number of individual documents, as <Var:ProductName> allows you to merge several native files into one intermediary master document. Through the `FileComplete` method you can determine what should happen after a particular file has been verified. With `Complete` you can determine what should happen after the entire (merged) document has been verified.

Through the object derived from [IFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IFileProperties.yml) you can retrieve various information on an individual file such as the original encoding, the original file path, etc. Through an object that is derived from [IDocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml) you can retrieve various information on the entire bilingual document such as the source and target languages, the source count, repetitions, etc.

Process the Paragraph Units
-----
The plug-in loops through all the paragraph units in a given bilingual document. In our implementation, we should use the **ProcessParagraphUnit** method to determine whether the plug-in is enabled or not in the first place. Remember that users can enable or disable global verifier plugs-ins through a checkbox next to the plug-in name. If the plug-in is not enabled, then nothing happens. If the plug-in is active, a separate helper function (which we will implement later) is called to carry out the actual verification.

# [C#](#tab/tabid-7)
```cs
public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
{
    // Apply the verification logic.
    CheckParagraphUnit(paragraphUnit);
}
```
***

Implement the Actual Verification Logic
------
The verification logic is contained in a separate helper function that works as follows: this function determines the context information (if any) of the current paragraph unit and then loops through the unit's segment pairs. It then determines whether the context fits the context that has been specified through the user interface. If this is the case, the method compares the source and the target segment. If the segments are not identical, a message will be generated.

# [C#](#tab/tabid-8)
```cs
private void CheckParagraphUnit(IParagraphUnit paragraphUnit)
{
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
                MessageReporter.ReportMessage(this, PluginResources.Plugin_Name,
                    ErrorLevel.Warning, PluginResources.Error_NotIdentical,
                    new TextLocation(new Location(segmentPair.Target, true), 0),
                    new TextLocation(new Location(segmentPair.Target, false), segmentPair.Target.ToString().Length - 1));
            }
        }
    }
}
```
***

The **ReportMessage** method has the following parameters:

* The name of the verifier plug-in that has thrown the message
* The error level, which in this case we set to **Warning**.
* A detailed description of the problem, which helps users ascertain what the problem is due to and take corrective action.
* The start and end location of the target string that has caused the problem. By specifying the 'from' and 'up to' location you allow the users to jump to the problematic target segment in the document by double-clicking the error message in the **Messages** window.
  
Putting it All Together
------

The complete main verification class will look as shown below:

# [C#](#tab/tabid-9)
```cs
using System;
using System.Collections.Generic;

using Sdl.Core.Settings;

using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

using Sdl.Verification.Api;

namespace Verification.Sdk.IdenticalCheck
{
    /// <summary>
    /// Required annotation for declaring the extension class.
    /// </summary>
    #region "Declaration"
    [GlobalVerifier("Identical Segments Verifier", "Plugin_Name", "Plugin_Description")]
    #endregion
    public class IdenticalVerifierMain : IGlobalVerifier, IBilingualVerifier, ISharedObjectsAware
    {
        #region "PrivateMembers"
        private ISharedObjects _sharedObjects;
        private IdenticalVerifierSettings _verificationSettings;
        #endregion

        /// <summary>
        /// Initializes the settings bundle object from which to retrieve the setting(s)
        /// to be used in the verification logic, e.g. the context display code to
        /// which the verification should be applied.
        /// </summary>
        #region Settings Bundle
        internal IdenticalVerifierSettings VerificationSettings
        {
            get
            {
                if (_verificationSettings == null && _sharedObjects != null)
                {
                    ISettingsBundle bundle = _sharedObjects.GetSharedObject<ISettingsBundle>("SettingsBundle");
                    if (bundle != null)
                    {
                        _verificationSettings = bundle.GetSettingsGroup<IdenticalVerifierSettings>();
                    }
                }
                return _verificationSettings;
            }
        }
        #endregion


        #region "ISharedObjectsAware Members"
        public void SetSharedObjects(ISharedObjects sharedObjects)
        {
            _sharedObjects = sharedObjects;
        }
        #endregion

        #region Members of IGlobalVerifier
        /// <summary>
        /// The following members set some general properties of the verification plug-in,
        /// e.g. the plug-in name and the icon that are displayed in the user interface of <Var:ProductName>. 
        /// </summary>
        #region "DescriptionNameIcon"
        public string Description
        {
            get { return PluginResources.Verifier_Description; }
        }
        public System.Drawing.Icon Icon
        {
            get { return PluginResources.icon; }
        }

        public string Name
        {
            get { return PluginResources.Plugin_Name; }
        }
        #endregion

        public IList<string> GetSettingsPageExtensionIds()
        {
            IList<string> list = new List<string>();

            list.Add("Identical Settings Definition ID");

            return list;
        }

        public string SettingsId
        {
            get { return "Identical Verifier"; }
        }

        public string HelpTopic
        {
            get { return String.Empty; }
        }

        public Type SettingsType
        {
            get { return typeof(IdenticalVerifierSettings); }
        }
        #endregion


        #region IBilingualFilterComponent Members
        #region "ItemFactory"
        public IDocumentItemFactory ItemFactory
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// This member is used to output any verification messages in the user interface of <Var:ProductName>.
        /// </summary>
        #region "MessageReporter"
        public IBilingualContentMessageReporter MessageReporter
        {
            get;
            set;
        }
        #endregion
        #endregion



        #region IBilingualContentHandler Members
        public void Complete()
        {
            // Not required for this implementation.
        }

        public void FileComplete()
        {
            // Not required for this implementation.
        }

        public void SetFileProperties(IFileProperties fileInfo)
        {
            // Not required for this implementation.
        }

        public void Initialize(IDocumentProperties documentInfo)
        {
            // Not required for this implementation.
        }
        #endregion

        #region "process"
        public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
        {
            // Apply the verification logic.
            CheckParagraphUnit(paragraphUnit);
        }
        #endregion

        /// <summary>
        /// The following member performs the actual verification. It traverses the segment pairs of the current document,
        /// and checks whether a particular segment has any context information (count > 0). It then determines whether
        /// the display code is identical to the display code entered in the plug-in settings.
        /// If this is the case, it determines whether the target segment is actually identical to the source segment.
        /// If not, a warning message will be generated, which is then displayed between the source and target segments,
        /// and in the Messages window of <Var:ProductName>.
        /// </summary>
        /// <param name="paragraphUnit"></param>
        #region "verify"
        private void CheckParagraphUnit(IParagraphUnit paragraphUnit)
        {
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
                        MessageReporter.ReportMessage(this, PluginResources.Plugin_Name,
                            ErrorLevel.Warning, PluginResources.Error_NotIdentical,
                            new TextLocation(new Location(segmentPair.Target, true), 0),
                            new TextLocation(new Location(segmentPair.Target, false), segmentPair.Target.ToString().Length - 1));
                    }
                }
            }
        }
        #endregion


    }
}
```
***

Ignoring Track Changes
-----
Verifiers should ignore any segments that have track changes. If a user has pending track changes on a segment then that means that they have not finished changing the segment and so that segment should not be verified. Furthermore, if a verifier tries to account for track changes then it will make the verifier unnecessarily complex.

The example given above does not ignore track changes but any production verifiers should do so. Usually, a verifier would implement a [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) to extract the relevant information from the segment. If this visitor visits a revision marker then that segment has track changes and should be ignored. Below is an example of a [IMarkupDataVisitor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IMarkupDataVisitor.yml) that determines whether a segment has track changes. If it visits a revision marker then it sets the property **HasRevisions**.

# [C#](#tab/tabid-10)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Verification.QAChecker
{
    class SegmentVisitor : IMarkupDataVisitor
    {
        public bool HasRevisions {get; set;}

        /// <summary>
        /// Iterates all sub items of container IAbstractMarkupDataContainer
        /// </summary>
        /// <param name="container"></param>
        private void VisitChildren(IAbstractMarkupDataContainer container)
        {
            foreach (var item in container)
            {
                item.AcceptVisitor(this);
            }
        }

        public void VisitCommentMarker(ICommentMarker commentMarker)
        {
            VisitChildren(commentMarker);
        }

        public void VisitLocationMarker(ILocationMarker location)
        {
        }

        public void VisitLockedContent(ILockedContent lockedContent)
        {
            VisitChildren(lockedContent.Content);
        }

        public void VisitOtherMarker(IOtherMarker marker)
        {
            VisitChildren(marker);
        }

        public void VisitPlaceholderTag(IPlaceholderTag tag)
        {
        }

        public void VisitRevisionMarker(IRevisionMarker revisionMarker)
        {
            // Note: this is also valid for Feedback Markers
            HasRevisions = true;
            //no need to iterate anymore inside.
        }

        public void VisitSegment(ISegment segment)
        {
            HasRevisions = false; //reset the flag before new iteration
            VisitChildren(segment);
        }

        public void VisitTagPair(ITagPair tagPair)
        {
            VisitChildren(tagPair);
        }

        public void VisitText(IText text)
        {

        }
    }
}
```
***

All verifiers should ignore any segments that have track changes apart from the **Document Verifier**. For each segment with track changes, the **Document Verifier** generates a warning message "Segment X could not be verified because it contains track changes" so that the user is aware of any segments that have not been verified.