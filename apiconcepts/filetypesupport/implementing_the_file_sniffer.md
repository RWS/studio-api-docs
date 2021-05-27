Implementing the File Sniffer
===

In this chapter you will learn how to implement the functionality required for determining whether a given file is valid and can therefore be processed by our sample file type plug-in.

Determine the Validation Criteria
--

For determining whether a given file can be processed by our file type plug-in or not, you can, of course, use the file extension, which is specified in the File Type Component Builder. However, this is not a very reliable criterion. Even if the file extension does match, the file might not be valid for some reason, and can therefore not be processed by the file type plug-in. Also, you need to consider that different file types might share the same extension.

It is therefore important to identify one or more reliable criteria that clearly indicate whether a given file is valid or not. For this simple text file type plug-in, let us assume that a *.text file is valid if the first line starts with the string ```[Version=n]```.

If this is the case, the file should be considered valid. Otherwise, the file type plug-in should tell the user that the given file is not supported. It is recommended that you implement this validation functionality in a distinct file sniffer component.

Implement the File Sniffer
--

Add a new class called e.g. **SimpleTextSniffer.cs** to your project. This class needs to implement the [INativeFileSniffer](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml) interface. Your file sniffer class needs to contain the [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method, which is part of the native file sniffer interface. This method returns a [SniffInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml) object, which you can set to True or to False, depending on whether the file should be considered valid or not according to specific validation criteria.

Remember, that if you have specific settings which need to be applied to the file sniffer, these can be populated using the ```ISettingsGroup``` which is passed in via the [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method. Please see [The File Sniffer](the_file_sniffer.md) for more details.

The minimum amount of code required to build a file sniffer component looks as shown below:

# [C#](#tab/tabid-1)
```cs
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.Core.Globalization;

namespace Sdl.Sdk.Snippets.Native
{
    class SimpleTextSniffer1 : INativeFileSniffer
    {

       public SniffInfo Sniff(string nativeFilePath, Language language, Codepage suggestedCodepage,
           INativeTextLocationMessageReporter messageReporter, ISettingsGroup settingsGroup)
        {
            SniffInfo fileInfo = new SniffInfo();    
            return fileInfo;
        }
    }
}
```
***

Now you can add the actual logic required to determine whether a file is supported or not. If the file is supported, set the [IsSupported](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_SniffInfo_IsSupported) property to True, otherwise to False. The complete file sniffer class looks as shown below:

# [C#](#tab/tabid-2)
```cs
using System.IO;
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.Core.Globalization;

namespace Sdl.Sdk.Snippets.Native
{
    // the file sniffer component determines whether a given file
    // can be processed by the filter or not
    class SimpleTextSniffer : INativeFileSniffer
    {
        public SniffInfo Sniff(string nativeFilePath, Language language, Codepage suggestedCodepage, INativeTextLocationMessageReporter messageReporter, ISettingsGroup settingsGroup)
        {
            SniffInfo fileInfo = new SniffInfo();

            StreamReader _reader = new StreamReader(nativeFilePath);

            if (_reader.ReadLine().StartsWith("[Version="))
                fileInfo.IsSupported=true;
            else
                fileInfo.IsSupported=false;

            return fileInfo;
        }
    }
}
```
***

Add the Component Reference to the File Type Definition
--

Do not forget to reference the file sniffer component to the File Type Component Builder by implementing the BuildFileSniffer method in your implementation of [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml). Remember that failure to do so will mean that this component will never be used by the file type plug-in, even if the sniffer component has been added to the assembly.

See Also
--



[User Communication Through Messaging](user_communication_through_messaging.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
