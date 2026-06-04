# The Bilingual File Parser

The bilingual parser extracts localizable content from bilingual documents. Unlike a native parser, which extracts source content only, a bilingual parser can set both source and target content and group that content into paragraph units and segments.

## Writing a bilingual parser

When you build a filter with the Bilingual API, the parser must implement [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml). In practice, the parser also participates in the bilingual content-processing pipeline through [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml).

You may also need to implement additional types, such as [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml) and [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml).

The simplest approach is to derive from [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) and implement [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml) and [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml). This base class provides helper properties and methods that reduce the amount of code you need to write.

`INativeContentCycleAware` gives the parser access to information such as the original file path, source and target languages, and encoding. It also provides methods that the framework calls at key stages of parsing so that your filter can initialize and clean up correctly. If the parser uses settings, implement [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml) and provide a UI for those settings. For more information, see [Filter UI Settings](filter_ui_settings.md).

## Using `AbstractBilingualFileTypeComponent`

The [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) base class already implements [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) and [IBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualFileTypeComponent.yml). This leaves your parser to implement the members required by [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml), `IParser`, and [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml).

## Implementing `IBilingualParser`

The [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml) interface defines two essential properties.

### `IBilingualParser.DocumentProperties`

The [DocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualParser_DocumentProperties) property holds an [IDocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml) instance. The Bilingual API sets this property during parser initialization. You store the value and then use it to set the source and target languages and initialize the output stream.

### `IBilingualContentProcessor.Output`

The [Output](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentProcessor_Output) property is of type [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml). The framework initializes it and uses it to connect the bilingual parser to the content processors further down the processing chain during extraction from a bilingual file format to the default bilingual SDLXLIFF (`*.xliff`) persistent format.

To support streaming and avoid loading the entire document into memory, the parser should feed paragraph units one at a time through `Output.ProcessParagraphUnit()`. Before it processes any paragraph units, it must also provide document and file properties through `Output.Initialize()` and `Output.SetFileProperties()`.

### `Output.Initialize()`

Call [Initialize](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Initialize_Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_) to pass the [DocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualParser_DocumentProperties) object for the current document. Do this after you set [SourceLanguage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_SourceLanguage) and [TargetLanguage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_TargetLanguage) from the source file.

>[!NOTE]
>
> This method should always be called before any other call on the [Output](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentProcessor_Output) interface.


### `Output.SetFileProperties()`

Call [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) to provide the framework with the properties for each file in the document. Call it before you process the file's paragraph units through [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_).

`SetFileProperties()` takes an [IFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IFileProperties.yml) instance. The bilingual parser must create this instance, typically by using [CreateFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentItemFactory.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentItemFactory_CreateFileProperties). In most cases, the parser should also set the [IPersistentFileConversionProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml) property before it passes the object to `SetFileProperties()`. When your parser implements [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml), you can usually obtain this information from the file properties that the framework passes into the content cycle. You may also need to set other values on [IFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IFileProperties.yml), such as the source language, target language, tool name, tool version, and creation date.

### `Output.ProcessParagraphUnit()`

Call [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) for each paragraph unit in the source file. Create the [IParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) instance by calling [CreateParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentItemFactory.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentItemFactory_CreateParagraphUnit_Sdl_FileTypeSupport_Framework_NativeApi_LockTypeFlags_). When you create a paragraph unit, specify the source language, target language, and lock type. In most cases, paragraph units are either structure paragraph units with the [Structure](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.LockTypeFlags.yml#fields) lock type or translatable paragraph units with the [Unlocked](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.LockTypeFlags.yml#fields) lock type.

### `Output.FileComplete()`

Call [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete) after you process all paragraph units in a file. After you call `FileComplete()`, do not change the file properties.

### `Output.Complete()`

Call [Complete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Complete) after you process all content in all files. After you call `Complete()`, do not change the document properties.

## Implementing `IParser`

### `IParser.OnProgress`

The [IParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IParser.yml) interface defines an [OnProgress](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_OnProgress_System_Byte_) event of type [ProgressEventArgs](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ProgressEventArgs.yml). Use this event to notify the framework, and therefore the user, about parsing progress. In most cases, you should raise it with `0` before you open a file, update it with the current percentage during parsing, and raise it with `100` when parsing is complete.

### `IParser.ParseNext()`

The [IParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IParser.yml) interface also requires the [ParseNext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_IParser_ParseNext) method. The framework calls this method repeatedly to process the next chunk of input from the source bilingual document.

The implementation should parse a suitable chunk of input, preferably a small one, and return a Boolean value that indicates whether more work remains. Return `false` when the parser has processed all file content.

Typically, this method, or methods that it calls, invoke the `Output` property methods to report the source file content to the framework.

## Implementing `INativeContentCycleAware`

If your parser implements [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml), it must implement the following three methods.

### `SetFileProperties()`

The standard implementation of [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) stores its [IFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IFileProperties.yml) parameter in a class field. The bilingual parser can then use these file properties to supplement information from the source file, such as the original encoding.

### `StartOfInput()`

The framework calls this method after component initialization, including [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_), but before it parses and passes any content to File Type Support Framework components.

### `EndOfInput()`

The framework calls this method after it finishes processing the bilingual content.

## Implementing `ISettingsAware`

If your parser implements [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml), it must implement the following method.

### `InitializeSettings()`

[InitializeSettings](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_ISettingsAware_InitializeSettings_Sdl_Core_Settings_ISettingsBundle_System_String_) passes an [ISettingsBundle](../../api/core/Sdl.Core.Settings.ISettingsBundle.yml) object and a `configurationId` (`FileTypeConfigurationId`). Use these values to populate the settings object that the parser requires:

# [C#](#tab/tabid-1)
```cs
public void InitializeSettings(Sdl.Core.Settings.ISettingsBundle settingsBundle, string configurationId)
{
    UserSettings _userSettings = new UserSettings();
    _userSettings.PopulateFromSettingsBundle(settingsBundle, configurationId);
    LockPrdCodes = _userSettings.LockPrdCodes;
}
```
***

## See also

- [The File Parser](the_file_parser.md)


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
