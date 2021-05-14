The File Parser
==

When writing filters that use the Native API (for monolingual file formats) you must derive your parser class from the interfaces [INativeFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileTypeComponent.yml) and [INativeFileParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileParser.yml). These interfaces provide a way of connecting to the SDL File Type Support Framework and allow the class to initialize the parser with the necessary objects it requires to interact with the SDL File Type Support Framework. A parser will also normally derive from the [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml) interface, which provides the parser with additional information such as the original file path name, source language and encoding. These interface class methods are called during key phases of the parsing process to enable your filter to manage its initialization, flow-control and clean-up.

However, the simplest way to implement a native parser is to merely it derive from the [AbstractNativeFileParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml) class and the ```INativeContentCycleAware``` interface and then use the helper functions of the ```AbstractNativeFileParser``` class.

```cs
public class SimpleTextParser : AbstractNativeFileParser, INativeContentCycleAware, ISettingsAware
{
     ...
}
```

Deriving from the ```AbstractNativeFileParser``` base class provides an implementation of the ```INativeFilterComponent``` and ```INativeFileParser``` interfaces, leaving only the ```INativeContentCycleAware``` and ```ISettingsAware``` interfaces to be implemented by the derived class. If this class is used as a base class, then in its implementation of ```INativeFileParser``` it provides some overridable methods than can be used to simplify the implementation of a native parser class. These overridable methods are as follows:

* [BeforeParsing](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_BeforeParsing): This method is called **before** any parsing commences and before the ```DuringParsing()``` method is called. Any initialization of variables, opening of a file, etc. can be done in the overridden implementation of this method after calling the base class implementation.
* [DuringParsing](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_DuringParsing): This method is called after the initial call to the ```BeforeParsing()``` method and subsequently for each chunk of processing that needs to be done for the file to be extracted. The implementation should return True, if there is more processing to be done and False after the file processing has been completed.
* [AfterParsing](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_AfterParsing): This method is called **after** ```DuringParsing() ```returns False to indicate that the file processing is complete. This method can be overridden to implement closing of the file or any tidying up operations.
* [OnProgress](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_OnProgress_System_Byte_): This method can be used to inform the framework - and hence the user - of the file parsing progress. It takes a byte parameter with values between 0 and 100 as a percentage of file processing progress.
* [OutputText](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_OutputText_System_String_) method: The ```OutputText()``` method can be used to output translatable text to the SDL File Type Support Framework API. This process will be explained in more detail in the section: [Implement the File Parser](implementing_the_file_parser.md).
* [Dispose](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_Dispose) method: This method is implemented in ```AbstractNativeFileParser``` and can be overridden by subclasses. This method is explicitly called after an unhandled exception is thrown in the Filter Framework. This is the last chance for the filter to release acquired resources (managed and unmanged). This can happen when: - Acquire resources in ```StartOfInput()``` and release them in ```EndOfInput()``` - Acquire resources in BeforeParsing() and release them in ```AfterParsing()``` - Acquire resources in ```SetFileProperties()``` and release them in ```FileComplete()``` - or other similar situations.

You may have noticed that we have not yet explained how you access essential information such as the actual name of the file to be processed. For this, we refer you back to the ```INativeContentCycleAware``` interface. This interface contains the method ```SetFileProperties()```. Its standard implementation is to save its parameter (**IFileProperties** properties) to a class variable. Probably the most commonly used member is FileConversionProperties, which in turn implements the following:

* [OriginalFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalFilePath): This property contains the full path name to the monolingual source file that the content originates from.
* [OriginalEncoding](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalEncoding): This property contains the original encoding (if known) of the monolingual file that the content originates from.
* [SourceLanguage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_SourceLanguage): This property contains the original source language of the monolingual file’s translatable content.
* [FileSnifferInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_FileSnifferInfo): This property can contain information generated by a [file sniffer](the_file_sniffer.md) component. The most relevant item provided by the ```FileSnifferInfo``` interface will probably be the ```DetectedEncoding``` property.
* [DependencyFiles](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_DependencyFiles): This property is a list of names of dependency files that are used or created while parsing. This may even include the original source document. A copy of these files is then created and encoded as part of the bilingual intermediate file format (SDL XLIFF). All files are then decoded to temporary file names and made available to the Native API writer during the native file genration phase. The set of dependency files can change while parsing, but should not change after the ```INativeContentCycleAware.EndOfInput()``` call has been invoked. Any dependency files that are reported by a parser or filter component while parsing a native file will be decoded to a temporary file and passed through this property when generating the native file. In future releases of the SDL File Type Support Framework API it will also be possible to add a link to dependency file instead of embedding it. The documentation for this ```DependencyFiles``` property will be updated as soon as this functionality becomes available.

The ```INativeContentCycleAware``` interface contains two further methods that need to be implemented in any parser class, which are outlined below. However, if you are using the flow control methods of the ```AbstractNativeFileParser``` base class, your implementation methods may not require any code content.

* **SetFileProperties** method: Returns a ```IPersistentFileConversionProperties``` object.
* [StartOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_StartOfInput): This method is called by the framework after component initialisation (i.e. after ```SetFileProperties()```, but before any content is parsed and passed to any of the filter components.
* [EndOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_EndOfInput): This method is called by the framework after the finishing processing of the native (monolingual) content.

The ```ISettingsAware``` interface contains one method that needs to be implemented in any parser class, which requires access to any settings the parser may need. This is shown below:

* [InitializeSettings](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_ISettingsAware_InitializeSettings_Sdl_Core_Settings_ISettingsBundle_System_String_) method: Passes in an ```ISettingsBundle``` object and a configurationId FileTypeConfigurationId. These can be used to populate the required settings object used by the parser:

```cs
   #region "InitializeSettings"
public void InitializeSettings(Sdl.Core.Settings.ISettingsBundle settingsBundle, string configurationId)
{
    UserSettings _userSettings = new UserSettings();
    _userSettings.PopulateFromSettingsBundle(settingsBundle, configurationId);
    LockPrdCodes = _userSettings.LockPrdCodes;
}

#endregion
```


>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
