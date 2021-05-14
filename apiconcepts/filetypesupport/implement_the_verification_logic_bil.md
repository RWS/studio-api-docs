Implement the Verification Logic
==

In this chapter you will learn how to implement the actual verification logic of the bilingual verification plug-in.

Add the Main Verifier Class
--

Start by adding a class called, e.g. **VerifierMain.cs** to your project. The class needs to reference the following namespaces:

* **Sdl.FileTypeSupport.Framework.NativeApi**
* **Sdl.FileTypeSupport.Framework.BilingualApi**: Provides access to the functionality used for processing bilingual documents.
* **Sdl.FileTypeSupport.Framework.IntegrationApi**
* **Sdl.Core.Settings**: Provides programmatic access to settings bundles

Moreover, your class needs to implement the following interfaces:
* [IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml): Provides the methods required for handling bilingual documents.
* [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml): Provides the functionality for initializing the plug-in settings.

Add the following boolean and string properties. These are the programmatic representation of the plug-in settings, which are set by the class that we implemented previously (see [Loading and Saving the Settings](loading_and_saving_the_settings_bil.md)):

```cs
public bool CheckWordArt
{
    get;
    set;
}

public int MaxWordCount
{
    get;
    set;
}
```
Next, implement the ```InitializeSettings``` method of the ```ISettingsAware``` interface. Here, we call on the VerifierSettings class and use the ```PopulateFromSettingsBundle``` method to retrieve the setting from the physically stored settings bundle. To do this, we need to provide the settings bundle object and the file type id (here *Word 2007 v 2.0.0.0 WordArt Verifier*) as parameters. *Word 2007 v 2.0.0.0 WordArt Verifier* is the file type id of the new file type that we will create - see [Create a New File Type Component Builder](create_new_file_type_component_builder.md). The ```CheckWordArt``` and ```MaxWordCount``` properties used in our main verification logic will then be set according to the value retrieved from the settings bundle.

```cs
public void InitializeSettings(ISettingsBundle settingsBundle, string configurationId)
{
    VerifierSettings _settings = new VerifierSettings();
    _settings.PopulateFromSettingsBundle(settingsBundle, "Word 2007 v 2.0.0.0 WordArt Verifier");
    CheckWordArt = _settings.CheckWordArt;
    MaxWordCount = _settings.MaxWordCount;
}
```

Provide Access to the Verification Message Reporting Functionality
--

If your verifier finds any errors in the file, the user should be notified accordingly. To this end, add the following message reporter member to your class.

```cs
public IBilingualContentMessageReporter MessageReporter
{
    get;
    set;
}
```

The [ReportMessage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentMessageReporter.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentMessageReporter_ReportMessage_System_Object_System_String_Sdl_FileTypeSupport_Framework_NativeApi_ErrorLevel_System_String_Sdl_FileTypeSupport_Framework_BilingualApi_TextLocation_Sdl_FileTypeSupport_Framework_BilingualApi_TextLocation_) is required for adding error messages (if any) to the **Messages** window of <Var:ProductName>.

Provide Access to the Item Factory
--

The item factory allows you to create, e.g. structure tags, placeholders, etc. Since our verifier only checks for the number of words in WordArt objects, this functionality is actually not necessary. However, you need to add this member, as it is required by the [IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml) interface.

```cs
public IDocumentItemFactory ItemFactory
{
    get;
    set;
}
```

Add the Initialize Method
--

Add the ```Initialize``` method, through which you can retrieve various information on the verified document such as the source and target language, source count, repetition count, etc. However, our verification logic does not require this information, therefore this member is strictly speaking not necessary for our example, but it must be added according to the [IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml) interface.

```cs
public void Initialize(IDocumentProperties documentInfo)           
{
    // Through the document properties you can access information that is
    // common to ALL bilingual files in a master bilingual document, e.g. the
    // source/target language, the repetition/source count, etc.
    // This is not required for this implementation.
}
```

Add the File and Process Complete Members
--

In a similar manner, the following members need to be added as per the [IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml) interface, although they are not actually required for the functionality of our plug-in. You may wonder why there is a ```FileComplete``` and a ```Complete``` method, which both seem to serve the same purpose. The reason is that <Var:ProductName> allows you to merge several documents into one bilingual (SDL XLIFF) master file (see [Merging files](merging_files.md)). You could use the ```FileComlete``` to carry out an action after the verification of each of the single (merged) files has been completed. You can then call ```Complete``` when the verification process for the entire bilingual document has finished.

```cs
/// <summary>
/// These members of the IBilingualContentHandler interface are not used in this
/// implementation.
/// </summary>
public void Complete()
{
    // Controls what happens when the whole verification process is complete.
}

public void FileComplete()
{
    // Controls what happens when the whole verification process for one single file is complete.
}

public void SetFileProperties(IFileProperties fileInfo)
{
    // A bilingual document can potentially be a master document that contains
    // a number of single (smaller) bilingual documents.
    // The File Info object can be used to access properties of particular bilingual file 
    // in a bilingual document, such as the file type definition id, the creation tool.
    // This information can be different from biligual file to bilingual file, as
    // each single bilingual file might have been created using different
    // file types, e.g. one bilingual file was derived from a PPT document,
    // another one from a DOC file.
    // This is not required for this implementation.

}
```

Traverse the Paragraph Units
--

The ```ProcessParagraphUnit()``` method is used to loop through the paragraph units in the intermediary (SDL XLIFF) file. This is where we determine whether the actual verification should be applied to a paragraph unit or not. Remember that the verification should only be applied to segments from WordArt objects.

If the ```CheckWordArt``` property (which is set by the user through the UI) is not True, the verification should not be carried out. Otherwise, the method traverses the segment pairs of the current paragraph unit.

Through an ``if`` condition we determine whether the current paragraph unit contains any context information, i.e. whether the context count is greater than zero. If that is the case, we determine the display code of the current context. **TAG** is the display code used (among other things) for WordArt objects.

If the display code equals this value and if the context description contains the string wordart, we call the helper function ```CheckWordCount``` (which we will add in the next step).


```cs
public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
{
    if (!CheckWordArt)
    {
        return;
    }

    foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
    {
        // Four conditions have to be met before the word count check is done:
        // 1. The current segment needs to have context information (i.e. context count > 0)
        // 2. The display code of the first context information unit is 'TAG'
        // 3. The context description contains the string 'wordart'
        // 4. The target segment is not empty
        if (paragraphUnit.Properties.Contexts.Contexts.Count > 0 &&
            paragraphUnit.Properties.Contexts.Contexts[0].DisplayCode == "TAG" &&
            paragraphUnit.Properties.Contexts.Contexts[0].Description.Contains("wordart") &&
            segmentPair.Target.ToString()!="")
            {
                {
                    CheckWordCount(segmentPair.Target);
                }
            }
    }
}
```

Carry out the Actual Word Count Verification
--

Finally, add the following helper function, which checks whether the target segment contains more than the maximum allowed number of words. Our simplified implementation will just work as follows:

* loop through the target segment string and count the number of spaces
* the number of words corresponds to the number of spaces + 1

Note that we will not check for non-breaking spaces, hyphens, etc., which is what you might have to do in a 'real', productive implementation. If the number of spaces +1 exceeds the maximum word count, a warning message will be added to the **Messages** window of <Var:ProductName>. Also, when the translator confirms a WordArt translation that exceeds the maximum word count, a yellow warning icon will be displayed next to the segment in question.
The ReportMessage method takes the following parameters:

* The name of the verifier plug-in that has thrown the message
* The [ErrorLevel](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ErrorLevel.yml), which in this case we set to [Warning](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ErrorLevel.yml#fields). The fact that the word count has been exceeded, is not considered a critical problem, as it does not prevent the translator from generating a valid Microsoft Word target file. The translator is just prompted to re-consider the translation and shorten it.
* A detailed description of the problem, which helps the user ascertain why this segment has been flagged to help him/her take corrective action, i.e. shorten the translation.
* The start and end location of the target string that has caused the problem. By specifying the 'from' and 'up to' location you allow users to jump to the faulty target segment in the document by double-clicking the error message in the **Messages** window of <Var:ProductName>.


```cs
private void CheckWordCount(ISegment targetSegment)
{
    int pos = 0, count = 0;
    char c = ' ';

    while ((pos = targetSegment.ToString().IndexOf(c, pos)) != -1)
    {
        count++;
        pos++;
    }
    count++;

    if (count > MaxWordCount)
    {
        MessageReporter.ReportMessage(this, Resources.Plugin_Name, ErrorLevel.Warning, 
            String.Format(Resources.MsgWordCountExceeded, count, MaxWordCount),
            new TextLocation(new Location(targetSegment, true), 0),
            new TextLocation(new Location(targetSegment,false), targetSegment.ToString().Length-1));
    }
}
```

![Error_Message_Length_Worksheet_Exceeded](images/Error_Message_Length_Worksheet_Exceeded.jpg)

Putting it All Together
--

The complete verification class should now look as shown below:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.IntegrationApi;


namespace Sdl.Sdk.FileTypeSupport.Samples.WordArtVerifier
{
    class VerifierMain : IBilingualVerifier, ISettingsAware
    {
        /// <summary>
        /// These properties provide access to the two plug-in settings.
        /// </summary>
        #region UI settings representation        

        public bool CheckWordArt
        {
            get;
            set;
        }

        public int MaxWordCount
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Initializes the plug-in settings, so that they can be used during the actual verification.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="configurationId"></param>
        #region "InitializeSettings"       
        public void InitializeSettings(ISettingsBundle settingsBundle, string configurationId)
        {
            VerifierSettings _settings = new VerifierSettings();
            _settings.PopulateFromSettingsBundle(settingsBundle, "Word 2007 v 2.0.0.0 WordArt Verifier");
            CheckWordArt = _settings.CheckWordArt;
            MaxWordCount = _settings.MaxWordCount;
        }
        #endregion

        #region "IBilingualFilterComponent Members"
        /// <summary>
        /// Provides access to the message reporter, which is responsible for 
        /// outputting any messages in the user interface of SDL Trados Studio
        /// /// </summary>
        #region "messagereporter"
        public IBilingualContentMessageReporter MessageReporter
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Not required in this implementation. Provides access to elements
        /// such as tags, placeholders, etc.
        /// </summary>
        #region "itemfactory"
        public IDocumentItemFactory ItemFactory
        {
            get;
            set;
        }
        #endregion
        #endregion


        #region "IBilingualContentHandler Members"
        /// <summary>
        /// These members of the IBilingualContentHandler interface are not used in this
        /// implementation.
        /// </summary>
        public void Complete()
        {
            // Controls what happens when the whole verification process is complete.
        }

        public void FileComplete()
        {
            // Controls what happens when the whole verification process for one single file is complete.
        }

        public void SetFileProperties(IFileProperties fileInfo)
        {
            // A bilingual document can potentially be a master document that contains
            // a number of single (smaller) bilingual documents.
            // The File Info object can be used to access properties of particular bilingual file 
            // in a bilingual document, such as the file type definition id, the creation tool.
            // This information can be different from biligual file to bilingual file, as
            // each single bilingual file might have been created using different
            // file types, e.g. one bilingual file was derived from a PPT document,
            // another one from a DOC file.
            // This is not required for this implementation.

        }
        #endregion

        #region "InitializeMethod"
        public void Initialize(IDocumentProperties documentInfo)           
        {
            // Through the document properties you can access information that is
            // common to ALL bilingual files in a master bilingual document, e.g. the
            // source/target language, the repetition/source count, etc.
            // This is not required for this implementation.
        }        
        #endregion        

        /// <summary>
        /// This method implements the actual verification logic.
        /// If CheckWordArt is true, the method loops through all segment pairs, and 
        /// determines whether they have any context information. If true, and if 
        /// the display code equals 'WA' (WordArt), a separate helper function is called
        /// to check the word count.
        /// </summary>
        /// <param name="paragraphUnit"></param>
        #region process paragraph unit
        public void ProcessParagraphUnit(IParagraphUnit paragraphUnit)
        {
            if (!CheckWordArt)
            {
                return;
            }

            foreach (ISegmentPair segmentPair in paragraphUnit.SegmentPairs)
            {
                // Four conditions have to be met before the word count check is done:
                // 1. The current segment needs to have context information (i.e. context count > 0)
                // 2. The display code of the first context information unit is 'TAG'
                // 3. The context description contains the string 'wordart'
                // 4. The target segment is not empty
                if (paragraphUnit.Properties.Contexts.Contexts.Count > 0 &&
                    paragraphUnit.Properties.Contexts.Contexts[0].DisplayCode == "TAG" &&
                    paragraphUnit.Properties.Contexts.Contexts[0].Description.Contains("wordart") &&
                    segmentPair.Target.ToString()!="")
                    {
                        {
                            CheckWordCount(segmentPair.Target);
                        }
                    }
            }
        }
        #endregion

        /// <summary>
        /// Helper function that counts the words in the current target segment.
        /// If the word count (i.e. number of spaces + 1) exceeds the maximum count
        /// that was set through the properties, a message should be added to the 
        /// Messages window of SDL Trados Studio.
        /// </summary>
        /// <param name="targetSegment"></param>
        #region output message
        private void CheckWordCount(ISegment targetSegment)
        {
            int pos = 0, count = 0;
            char c = ' ';

            while ((pos = targetSegment.ToString().IndexOf(c, pos)) != -1)
            {
                count++;
                pos++;
            }
            count++;

            if (count > MaxWordCount)
            {
                MessageReporter.ReportMessage(this, Resources.Plugin_Name, ErrorLevel.Warning, 
                    String.Format(Resources.MsgWordCountExceeded, count, MaxWordCount),
                    new TextLocation(new Location(targetSegment, true), 0),
                    new TextLocation(new Location(targetSegment,false), targetSegment.ToString().Length-1));
            }
        }
        #endregion
    }
}
```

Using the Main Verifier Class
--

To use this verifier, a new file type definition based upon the Microsoft Word 2007 file type definition needs to be created (see [Create a New File Type Component Builder](create_new_file_type_component_builder.md)).

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.