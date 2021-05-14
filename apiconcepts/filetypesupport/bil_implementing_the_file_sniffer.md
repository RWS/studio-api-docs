Implementing the File Sniffer
In this chapter, we implement the functionality required for determining whether a given BIL file is valid and can be processed by our sample file type plug-in.

Identify the File Validation Criteria
--

For determining whether a given file can be processed by a filter or not, you can, of course, use the file extension, which is also specified in the File Type Component Builder. However, this criterion is not very reliable. Even if the file extension does match, the file might still not be valid for some reason, and can therefore not be processed by the file type plug-in. Moreover, different file types might share the same extension, which is another reason for not relying on file name extensions only.

It is therefore important to determine one more reliable criteria that tell you whether the file is valid or not. The most obvious criterion for an XML file is, of course, the root element, which in this case is ```bilingualdocument```. Let us proceed on the assumption that if a given document has this root element, the file should be considered valid. If not, the file type plug-in should tell the user that the file is not supported. It is recommended that you implement the validation functionality in a distinct file sniffer component.

Add the File Sniffer Class
--

Add a new class called e.g. **BilSniffer.cs** to your project. This class needs to implement the [INativeFileSniffer](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml) interface. Note that even though we are developing a bilingual file type plug-in (and not a native file type plug-in), we require the above-mentioned interface. The sniffer component determines the validity of the file based on file-specific (native) criteria (in our example the root element).

The interface implements a [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method, which returns a [SniffInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml) object. Through this object you can, for example, set whether a given file is supported or not. The minimum amount of code required to build a file sniffer component looks as shown below:

```cs
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.Core.Globalization;

namespace Sdl.Sdk.Snippets.Bilingual
{
    class BilSniffer : INativeFileSniffer
    {

        public SniffInfo Sniff(
            string nativeFilePath,
            Language language,
            Codepage suggestedCodepage,     
            INativeTextLocationMessageReporter messageReporter,
             ISettingsGroup settingsGroup)

        {
            SniffInfo fileInfo = new SniffInfo();    
            return fileInfo;
        }
    }
}
```

Implement the Sniffer Logic
--

Now you implement the actual logic required to determine whether a given file is supported or not. Remember, that if you have specific settings which need to be applied to the file sniffer, these can be populated using the ```ISettingsGroup``` which is passed in via the Sniff method. Please see [The File Sniffer](the_file_sniffer.md) for more details. The file sniffer reads the root element name from a given document. In our sample format, the root element also contains the source and target language information, e.g.:

```xml
<bilingualdocument source-language="en-US" target-language="de-DE">
```

As we are going to retrieve these three items from the root element, it makes sense to add the root element name to search for as well as the source/target language attribute names as constants to our sniffer class:

```cs
static string _BilingualDocument = "bilingualdocument";
static string _SourceLanguage = "source-language";
static string _TargetLanguage = "target-language";
```

The [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method then calls two separate helper functions to read the root element name and the source/target language values. Depending on the result returned by the ```IsFileSupported()``` helper function, the [IsSupported](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_SniffInfo_IsSupported) property is set to True or to False:

```cs
public SniffInfo Sniff(string nativeFilePath, Language suggestedSourceLanguage, 
    Codepage suggestedCodepage, INativeTextLocationMessageReporter messageReporter, 
    ISettingsGroup settingsGroup)
{
    SniffInfo info = new SniffInfo();

    if (System.IO.File.Exists(nativeFilePath))
    {
        // call method to check if file is supported
        info.IsSupported = IsFileSupported(nativeFilePath);
        // call method to determine the file language pair
        GetFileLanguages(ref info, nativeFilePath);
    }
    else
    {
        info.IsSupported = false;
    }

    return info;
}
```

The ```IsFileSupported()``` helper function receives the file name and path from the [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method. It then loads the document into an XML DOM object to determine the root element, and returns True or False.

```cs
// determine whether a given file is supported based on the
// root element
private bool IsFileSupported(string nativeFilePath)
{
    bool result = false;
    XmlDocument doc = new XmlDocument();
    doc.Load(nativeFilePath);
    if (doc.DocumentElement.Name == _BilingualDocument)
    {
        result = true;
    }

    return result;
}
```

The ``GetFileLanguages()`` helper function retrieves the file info object as well as the file name and path from the [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method. It then loads the document into an XML DOM object to read the source and target language attribute values. After that, it sets the properties for the detected source and target languages to the values that were retrieved from the root element attributes. In our example the root element attributes clearly state the source and target language. This is why we can set the [DetectionLevel](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.DetectionLevel.yml) property to [Certain](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.DetectionLevel.yml#fields). For document types in which the languages have to be determined, for example, based on heuristics, you may want to set the detection level to one of the other possible values, e.g. [Likely](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.DetectionLevel.yml#fields) or [Guess](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.DetectionLevel.yml#fields).

```cs
// retrieve the source and target language
// from the file header
private void GetFileLanguages(ref SniffInfo info, string nativeFilePath)
{
    XmlDocument doc = new XmlDocument();
    doc.Load(nativeFilePath);
    if (doc.DocumentElement.HasAttributes)
    {
        XmlAttribute source = doc.DocumentElement.Attributes[_SourceLanguage];
        if (source != null)
        {
            info.DetectedSourceLanguage =
                new Sdl.FileTypeSupport.Framework.Pair<Language, DetectionLevel>(new Language(source.Value),
                    DetectionLevel.Certain);
        }

        XmlAttribute target = doc.DocumentElement.Attributes[_TargetLanguage];
        if (source != null)
        {
            info.DetectedTargetLanguage =
                new Sdl.FileTypeSupport.Framework.Pair<Language, DetectionLevel>(new Language(target.Value),
                    DetectionLevel.Certain);
        }
    }
}
```

If the detection level is set to [Certain](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.DetectionLevel.yml#fields), the language dropdown lists in the **Open Document** dialog box will be disabled, and thus cannot be changed by the user. Otherwise, end users can change the proposed language combination at their discretion.

![LanguagesAutoDeteced](images/LanguagesAutoDeteced.jpg)

When the language pair has been programmatically determined by the sniffer, the user does not have to select the source/target language combination manually upon opening the file. The language pair will be displayed in the status bar of <Var:ProductName>, e.g.:

![LanguagePair](images/LanguagePair.jpg)

>**Note**
>
>If the sniffer determines that the file is not supported, you should communicate this to the user. This is done in the same way as in a native file type plug-in. See chapter [User Communication Through Messaging](user_communication_through_messaging.md) for more information and for an example on how to provide users with information on why a given file cannot be processed.

Add the Component Reference to the Component Builder
--

Do not forget to reference the file sniffer component to the File Type Component Builder by inserting the following code to your implementation of the [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) interface. Remember that failure to do so will mean that this component will never be used by the file type plug-in, even if the sniffer has been implemented in the assembly.

```cs
public INativeFileSniffer BuildFileSniffer(string name)
{
    return new BilSniffer();
}
```

Putting it All Together
--

The complete sniffer class should now look as shown below:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.Core.Globalization;
using Sdl.Core.Settings;

namespace Sdl.Sdk.FileTypeSupport.Samples.Bil
{
    class BilSniffer : INativeFileSniffer
    {
        #region "constants"
        static string _BilingualDocument = "bilingualdocument";
        static string _SourceLanguage = "source-language";
        static string _TargetLanguage = "target-language";
        #endregion

        #region "sniff"
        public SniffInfo Sniff(string nativeFilePath, Language suggestedSourceLanguage, 
            Codepage suggestedCodepage, INativeTextLocationMessageReporter messageReporter, 
            ISettingsGroup settingsGroup)
        {
            SniffInfo info = new SniffInfo();

            if (System.IO.File.Exists(nativeFilePath))
            {
                // call method to check if file is supported
                info.IsSupported = IsFileSupported(nativeFilePath);
                // call method to determine the file language pair
                GetFileLanguages(ref info, nativeFilePath);
            }
            else
            {
                info.IsSupported = false;
            }

            return info;
        }
        #endregion

        #region "issupported"
        // determine whether a given file is supported based on the
        // root element
        private bool IsFileSupported(string nativeFilePath)
        {
            bool result = false;
            XmlDocument doc = new XmlDocument();
            doc.Load(nativeFilePath);
            if (doc.DocumentElement.Name == _BilingualDocument)
            {
                result = true;
            }

            return result;
        }
        #endregion

        #region "get languages"
        // retrieve the source and target language
        // from the file header
        private void GetFileLanguages(ref SniffInfo info, string nativeFilePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(nativeFilePath);
            if (doc.DocumentElement.HasAttributes)
            {
                XmlAttribute source = doc.DocumentElement.Attributes[_SourceLanguage];
                if (source != null)
                {
                    info.DetectedSourceLanguage =
                        new Sdl.FileTypeSupport.Framework.Pair<Language, DetectionLevel>(new Language(source.Value),
                            DetectionLevel.Certain);
                }

                XmlAttribute target = doc.DocumentElement.Attributes[_TargetLanguage];
                if (source != null)
                {
                    info.DetectedTargetLanguage =
                        new Sdl.FileTypeSupport.Framework.Pair<Language, DetectionLevel>(new Language(target.Value),
                            DetectionLevel.Certain);
                }
            }
        }
        #endregion
    }
}
```
See Also
--

**Other Resources**

[User Communication Through Messaging](user_communication_through_messaging.md)

>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.