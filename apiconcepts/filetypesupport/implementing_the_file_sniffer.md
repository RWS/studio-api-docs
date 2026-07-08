# Implementing the File Sniffer

This section explains how to validate a file before your sample file type plug-in processes it.

## Determine the validation criteria

You can use the file extension that you defined in the File Type Component Builder to identify candidate files. However, the extension alone does not provide a reliable validation rule. A file can use the expected extension and still contain unsupported content. Different file types can also share the same extension.

Define one or more checks that clearly identify a valid file. For this simple text file type plug-in, assume that a `.text` file is valid when its first line starts with `[Version=n]`.

If the file matches this rule, treat it as valid. Otherwise, report that the file type plug-in does not support the file. Implement this validation logic in a dedicated file sniffer component.

## Implement the file sniffer

Add a new class, for example **SimpleTextSniffer.cs**, to your project. Implement the [INativeFileSniffer](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml) interface in that class. The class must include the [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method. This method returns a [SniffInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml) object. Set that object to indicate whether the file matches your validation criteria.

If the file sniffer requires custom settings, use the `ISettingsGroup` parameter that the [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method receives. For more information, see [The File Sniffer](the_file_sniffer.md).

The following example shows the minimum code required to create a file sniffer component:

# [C#](#tab/tabid-1)
```cs
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.Core.Globalization;

namespace Sdk.Snippets.Native
{
    class SimpleTextSniffer1 : INativeFileSniffer
    {
        public SniffInfo Sniff(string nativeFilePath, Language language, Codepage suggestedCodepage,
           INativeTextLocationMessageReporter messageReporter, ISettingsGroup settingsGroup)
        {
            var fileInfo = new SniffInfo();
            return fileInfo;
        }
    }
}
```
***

Now add the validation logic that determines whether the file is supported. If the file is supported, set [IsSupported](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_SniffInfo_IsSupported) to `true`. Otherwise, set it to `false`. The following example shows the complete file sniffer class:

# [C#](#tab/tabid-2)
```cs
using System.IO;
using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.Core.Globalization;

namespace Sdk.Snippets.Native
{
    class SimpleTextSniffer : INativeFileSniffer
    {
        public SniffInfo Sniff(string nativeFilePath, Language language, Codepage suggestedCodepage, INativeTextLocationMessageReporter messageReporter, ISettingsGroup settingsGroup)
        {
            var fileInfo = new SniffInfo();

            using var reader = new StreamReader(nativeFilePath);
            fileInfo.IsSupported = reader.ReadLine().StartsWith("[Version=");

            return fileInfo;
        }
    }
}
```
***

## Add the component reference to the file type definition

Reference the file sniffer component in the File Type Component Builder by implementing the `BuildFileSniffer` method in your implementation of [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml). If you skip this step, Var:ProductName will not use the sniffer component, even though the assembly contains it.

## See also
- [User Communication Through Messaging](user_communication_through_messaging.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
