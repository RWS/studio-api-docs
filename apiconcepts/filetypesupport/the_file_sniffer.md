The File Sniffer
==

File sniffers are small classes that are implemented to quickly determine whether a given file is supported by the parser that they are acting for. When a file sniffer is specified in the corresponding File Type Component Builder, it will be invoked by the framework **before** the actual file parsing begins. When the file sniffer determines that a given file cannot be processed by the filter, it will, for example, throw a corresponding message for the user, and the file will not be sent to the file parser, which will save processing time.

Usually a sniffer examines the beginning of a document or the document header in order to determine whether the given file is likely to be supported by the filter or not. This may be done, for example, by looking for certain XML element names or by examining the first few bytes of a file. If a whole file needs woud need to be read and processed to determine if the doucment is valid or not, then the file sniffer's value is significantly diminished, because it will add more processing time to the filter, and an exception could be raised in the parser anyway if the source document was found to be invalid. However, there is some merit in having XML documents being validated against a schema within a file sniffer especially if this extra validation step can be made configurable.

The file sniffer class needs to contain one member, i.e. the ```Sniff()``` method, which takes three parameters: a file path, a language and a suggested codepage. The given file is checked to see if it is a supported file type. The provided language and codepage may be used to help determines if this is the case.

The ```Sniff()``` method returns a [SniffInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml) object, which has four properties:

* [IsSupported](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml) to indicate if a given document is supported.
* [DetectedEncoding](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_SniffInfo_DetectedEncoding) to indicate the encoding of a given document.
* [DetectedSourceLanguage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_SniffInfo_DetectedSourceLanguage) to indicate the source language of a given document.
* [DetectedTargetLanguage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_SniffInfo_DetectedTargetLanguage) to indicate the target language of given document.

If required, you can store extra information for further use in the parser or writer component, for example the type of line break etc., using the [SetMetaData](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IMetaDataContainer.yml#Sdl_FileTypeSupport_Framework_NativeApi_IMetaDataContainer_SetMetaData_System_String_System_String_) and [GetMetaData](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IMetaDataContainer.yml#Sdl_FileTypeSupport_Framework_NativeApi_IMetaDataContainer_GetMetaData_System_String_) methods to store/load any additional information in/from a ```SniffInfo``` object.

These custom values are then accessible through a [IPersistentFileConversionProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml) object.

Any settings that may be required by the file sniffer are passed in via an ISettingsGroup. This allows the file sniffer to set up any required settings in a manner like that shown below:

# [C#](#tab/tabid-1)
```cs
  private void OverrideSettings(ISettingsGroup settingsGroup)
{
    if (settingsGroup == null)
    {
        return;
    }

    _SnifferSettings.PopulateFromSettingsBundle(settingsGroup.SettingsBundle, settingsGroup.Id);           
}
```
***

This can be called from within the ```Sniff()``` method body and will populate any required settings.

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
