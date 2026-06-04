# The File Sniffer

A file sniffer is a small class that quickly determines whether a parser supports a file. If you specify a file sniffer in the corresponding File Type Component Builder, the framework calls it **before** parsing starts. If the sniffer determines that the filter cannot process the file, the framework can report that result to the user and skip the parser. This reduces unnecessary processing.

## How a file sniffer works

A file sniffer usually examines the start of a document or its header to determine whether the filter is likely to support the file. For example, it can look for specific XML element names or inspect the first few bytes of a file.

If the sniffer must read and process the entire file to determine whether the document is valid, it provides less value. In that case, it adds processing time and the parser could raise an exception anyway when it detects an invalid source document. XML schema validation can still be useful in a file sniffer, especially when you make that extra validation step configurable.

## The `Sniff()` method

The file sniffer class must implement one member: the `Sniff()` method. This method takes three parameters: a file path, a language, and a suggested code page. It checks whether the file is a supported file type. The provided language and code page can help make that determination.

The `Sniff()` method returns a [SniffInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml) object with the following four properties:

* [IsSupported](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml) to indicate if a given document is supported.
* [DetectedEncoding](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_SniffInfo_DetectedEncoding) to indicate the encoding of a given document.
* [DetectedSourceLanguage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_SniffInfo_DetectedSourceLanguage) to indicate the source language of a given document.
* [DetectedTargetLanguage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.SniffInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_SniffInfo_DetectedTargetLanguage) to indicate the target language of the given document.

## Passing metadata

If necessary, you can store additional information for later use in the parser or writer component. For example, you can store the line break type. Use [SetMetaData](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IMetaDataContainer.yml#Sdl_FileTypeSupport_Framework_NativeApi_IMetaDataContainer_SetMetaData_System_String_System_String_) and [GetMetaData](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IMetaDataContainer.yml#Sdl_FileTypeSupport_Framework_NativeApi_IMetaDataContainer_GetMetaData_System_String_) to save and load metadata in a `SniffInfo` object.

These custom values are then available through an [IPersistentFileConversionProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml) object.

## Reading settings

Any settings that the file sniffer requires are passed in through an `ISettingsGroup`. This allows the file sniffer to initialize its settings, as shown in the following example:

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

Call this method from within `Sniff()` to populate the required settings.

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
