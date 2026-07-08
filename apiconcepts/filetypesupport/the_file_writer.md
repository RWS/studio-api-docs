# The File Writer

The final step in the localization workflow is to generate the translated target file in its original document format. For example, an SDK sample writes translated segments from a bilingual SDLXLIFF document to a native SubRip file. A Native API writer performs this task.

## Implementing a Native API writer

When you implement a Native API file writer, your class must connect to the File Type Support Framework through [INativeFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileTypeComponent.yml) and [INativeFileWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileWriter.yml). In most cases, the writer also implements [INativeOutputSettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputSettingsAware.yml), [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml), and [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml). These interfaces provide information such as the target file path, target language, preferred encoding, and any required settings.

The simplest way to implement a native writer is to derive from [AbstractNativeFileWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml) and implement [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml). You can then use the helper methods that `AbstractNativeFileWriter` provides.

## Using `AbstractNativeFileWriter`

The `AbstractNativeFileWriter` base class already implements `INativeFilterComponent`, `IAbstractNativeContentHandler`, and `INativeOutputSettingsAware`. This leaves the derived class to implement `INativeContentCycleAware`. The base class also provides default, empty virtual implementations of `IAbstractNativeContentHandler` members, so you only need to override the methods that matter for your document type.

The [IAbstractNativeContentHandler](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml) interface is also used by the `Output` property of [AbstractNativeFileParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml). This shared interface allows the framework to reuse the text, tags, and other markup produced during parsing when it generates the translated output file.

In most cases, the writer can assume that tag content matches the content produced by the parser. Two exceptions apply:

* A framework-based editor can add markup manually, such as **bold**, *italic*, or <u>underline</u>, if the File Type Component Builder allows it.
* A parser can create a tag that contains localizable content. In that case, inspect the [LocalizableContent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractTagProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractTagProperties_LocalizableContent) property to retrieve the localizable subsegment text.

## Overridable writer methods

If you derive from `AbstractNativeFileWriter`, override only the methods that your document type requires. If you do not derive from `AbstractNativeFileWriter`, you must implement all of these methods, even when some implementations remain empty.

* [StructureTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_StructureTag_Sdl_FileTypeSupport_Framework_NativeApi_IStructureTagProperties_): Called when the framework sends structure tag information to the writer. Your implementation interprets `IStructureTagProperties.TagContent`. Simple text-based formats may only need to write that tag content directly to the output file.
* [InlineStartTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_InlineStartTag_Sdl_FileTypeSupport_Framework_NativeApi_IStartTagProperties_): Called when the framework sends a paired start tag to the writer. A simple implementation may only need to write the tag content to the output file.
* [InlineEndTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_InlineEndTag_Sdl_FileTypeSupport_Framework_NativeApi_IEndTagProperties_): Called when the framework sends the end tag of a paired tag. The framework guarantees that tag pairs remain well-formed during translation.
* [InlinePlaceholderTag](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_InlinePlaceholderTag_Sdl_FileTypeSupport_Framework_NativeApi_IPlaceholderTagProperties_): Called when the framework sends a standalone placeholder tag to the writer.
* `Text`: Called when the framework sends human-readable text to the writer. This is usually translated text, but it can also be source text when the framework previews the source document.
* [CustomInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_CustomInfo_Sdl_FileTypeSupport_Framework_NativeApi_ICustomInfoProperties_): Sends temporary custom information between native content-processing components.
* [ChangeContext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_ChangeContext_Sdl_FileTypeSupport_Framework_NativeApi_IContextProperties_): Usually invoked on the parser output rather than on the writer. The writer still defines it because it belongs to the shared `IAbstractNativeContentHandler` interface.
* [Dispose](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileWriter_Dispose): Can be overridden to release resources if the Filter Framework throws an unhandled exception. This is the last opportunity to release managed or unmanaged resources.

## Implementing `INativeContentCycleAware`

Writers that implement `INativeContentCycleAware` must also define the following methods:

* [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_): Receives an `IPersistentFileConversionProperties` object.
* [StartOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_StartOfInput): Called after component initialization, including `SetFileProperties()`, but before any content is parsed and passed to filter components.
* [EndOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_EndOfInput): Called after the framework finishes processing the native content.

## Implementing `ISettingsAware`

If your writer needs access to settings, implement `ISettingsAware`.

* [InitializeSettings](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_ISettingsAware_InitializeSettings_Sdl_Core_Settings_ISettingsBundle_System_String_): Passes an `ISettingsBundle` object and a `configurationId` (`FileTypeConfigurationId`). Use these values to populate the settings object that the writer requires, for example:

# [C#](#tab/tabid-1)
```cs
public void InitializeSettings(Sdl.Core.Settings.ISettingsBundle settingsBundle, string configurationId)
{
    UserSettings _userSettings = new UserSettings();
    _userSettings.PopulateFromSettingsBundle(settingsBundle, configurationId);
    WriteUtf8Bom = _userSettings.WriteUtf8Bom;
}
```

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
