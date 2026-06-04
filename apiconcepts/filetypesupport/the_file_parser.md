# The File Parser

When you build a filter that uses the Native API for monolingual file formats, your parser must connect to the File Type Support Framework through [INativeFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileTypeComponent.yml) and [INativeFileParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileParser.yml). In most cases, the parser also implements [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml), which provides information such as the original file path, source language, and encoding. The framework calls these members at key stages of parsing so that your filter can initialize, control the parsing flow, and clean up correctly.

The simplest way to implement a native parser is to derive from [AbstractNativeFileParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml) and implement `INativeContentCycleAware`. You can then use the helper methods that `AbstractNativeFileParser` provides.

# [C#](#tab/tabid-1)
```cs
public class SimpleTextParser : AbstractNativeFileParser, INativeContentCycleAware, ISettingsAware
{
     ...
}
```
***

## Using `AbstractNativeFileParser`

The `AbstractNativeFileParser` base class already implements `INativeFilterComponent` and `INativeFileParser`. This leaves your derived class to implement `INativeContentCycleAware` and, if needed, `ISettingsAware`. It also exposes overridable methods that simplify the implementation of a native parser.

* [BeforeParsing](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_BeforeParsing): Called **before** parsing begins and before `DuringParsing()` is called. Override this method to initialize variables, open files, or perform similar setup tasks after you call the base implementation.
* [DuringParsing](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_DuringParsing): Called after the initial `BeforeParsing()` call and then repeatedly for each chunk of work that the parser must process. Return `true` while parsing should continue and `false` when parsing is complete.
* [AfterParsing](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_AfterParsing): Called **after** `DuringParsing()` returns `false`. Override this method to close files or perform other cleanup tasks.
* [OnProgress](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_OnProgress_System_Byte_): Reports file parsing progress to the framework and the user. It takes a byte value between `0` and `100`.
* [OutputText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_OutputText_System_String_): Sends translatable text to the File Type Support Framework API. For more information, see [Implement the File Parser](implementing_the_file_parser.md).
* [Dispose](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_Dispose): Can be overridden to release resources if the Filter Framework throws an unhandled exception. This is the last opportunity to release managed or unmanaged resources. Typical patterns include acquiring resources in `StartOfInput()` and releasing them in `EndOfInput()`, or acquiring resources in `BeforeParsing()` and releasing them in `AfterParsing()`.

## Accessing file information

To access essential information such as the name of the file being processed, use `INativeContentCycleAware`. This interface includes `SetFileProperties()`. A typical implementation stores the `IFileProperties` parameter in a class field. One of the most useful members is `FileConversionProperties`, which exposes the following properties:

* [OriginalFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalFilePath): The full path to the monolingual source file.
* [OriginalEncoding](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalEncoding): The original encoding of the source file, if known.
* [SourceLanguage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_SourceLanguage): The original source language of the file's translatable content.
* [FileSnifferInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_FileSnifferInfo): Information produced by a [file sniffer](the_file_sniffer.md) component. In many cases, the most useful value is `DetectedEncoding`.
* [DependencyFiles](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_DependencyFiles): The list of dependency files that the parser uses or creates during parsing. This list can include the original source document. The framework stores these files in the intermediate bilingual format, then decodes them to temporary file names and makes them available to the Native API writer during native file generation. The set of dependency files can change while parsing is in progress, but it should not change after `INativeContentCycleAware.EndOfInput()` is called.

## Implementing `INativeContentCycleAware`

`INativeContentCycleAware` also defines two additional methods that your parser must implement. If you use the flow-control methods in `AbstractNativeFileParser`, these implementations may not need much code.

* `SetFileProperties()`: Stores the file properties that the framework passes to the parser.
* [StartOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_StartOfInput): Called after component initialization, including `SetFileProperties()`, but before the framework parses and passes any content to filter components.
* [EndOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_EndOfInput): Called after the framework finishes processing the native content.

## Implementing `ISettingsAware`

If your parser needs access to user or file type settings, implement `ISettingsAware`.

* [InitializeSettings](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_ISettingsAware_InitializeSettings_Sdl_Core_Settings_ISettingsBundle_System_String_): Passes an `ISettingsBundle` object and a `configurationId` (`FileTypeConfigurationId`). Use these values to populate the settings object that the parser requires.

# [C#](#tab/tabid-2)
```cs
public void InitializeSettings(Sdl.Core.Settings.ISettingsBundle settingsBundle, string configurationId)
{
    UserSettings _userSettings = new UserSettings();
    _userSettings.PopulateFromSettingsBundle(settingsBundle, configurationId);
    LockPrdCodes = _userSettings.LockPrdCodes;
}
```
***

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
