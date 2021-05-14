The Bilingual File Parser
==

The bilingual parser is used for localizable content extraction. While the native parser allows you to extract source related localizable content, the bilingual parser allows you to set source and target related localizable content, group this content into the paragraph units and the segments.

Writing a bilingual parser
--

When writing filters to use the Bilingual API you must first derive your parser class from the [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml) and [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interfaces. These two interfaces form the core of the work that is done by a Bilingual parser.

Some other interaces may be also required such as [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) or [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml).

However, the simplest way to implement a Bilingual parser is to derive from the [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) class and the [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml) and [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml) interface only and use the helper functions and properties of the [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) class. A parser normally derives from the [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml) interface which provides the parser with additional information such as the original file path name, source and target language and encoding and class methods that are called during key phases of the parsing process to enable your filter to manage its initialisation flow-control and clean-up. You should also derive from the [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml) interface if you have any settings associated with this parser. Of course, you will need to create a UI to set the settings as well. Please see [Filter UI Settings](filter_ui_settings.md) for more information.

Deriving from AbstractBilingualFileTypeComponent
--

The [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) base class provides an implementation of the [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) and [IBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualFileTypeComponent.yml) interfaces leaving only the [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml) and [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml) interfaces to be implemented by the derived class. If this class is used as a base class then most of the properties and methods that a required to implement a Bilingual parser will be implemented in this [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) class or its base class. This leaves the following interfaces that still need to have their interface members implemented in your bilingual parser class: [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml), IParser and [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml).

Implementing IBilingualParser
--

The [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml) interface contains two essential properties for the bilingual parser.

The first property ```DocumentProperties``` contains an [IDocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml) interface that is set by the Bilingual API during the parser initialisation. This document properties interface is used to initialise the source and target languages.

The second property [Output](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentProcessor_Output) of type [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) is used to tell the Bilingual API of all major file processing events that are encountered during the parsing of a bilingual document. The [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interface is implemented by content processors within the Bilingual API that work on the bilingual content model. To facilitate processing in a streaming manner without requiring the entire document object in memory at any time the parser will need to feed paragraph-units one by one through calls to ```Output.ProcessParagraphUnit()```. However, document and file properties will need to be provided by the parser to the framework by calls to ```Output.Initialize()``` and ```Output.SetFileProperties()``` before processing any paragraph-units in each document or file. These events are outlined in the next few sections below.

**IBilingualParser.DocumentProperties**

The [DocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualParser_DocumentProperties) interface of type [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml) is set by the SDL File Type Support Framework when calling a bilingual parser but you will need to define this property and a private member to store its value. This document properties interface is then later used for storing the source and target languages and then initialising the output stream of the bilingual content processes.

**IBilingualContentProcessor.Output**

The [Output](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentProcessor_Output) interface of type [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) is also initialised by the SDL File Type Support Framework and provides a coupling between the Bilingual Parser and all Bilingual Content Processors down the processing chain during the extract conversion phase from a bilingual file format to the default bilingual SDL XLIFF (*.xliff*) persistent file format. The [Output](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentProcessor_Output) interface has several methods that are called throughout the file parsing operation.

**Output.Initialize()**

The Bilingual Parser should call [Initialize](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Initialize_Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_) method to forward the reference to the [DocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualParser_DocumentProperties) object for the document being processed. This is normally done after the [SourceLanguage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_SourceLanguage) and [TargetLanguage](/api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_TargetLanguage) have been set using information from the source file being parsed.

>**Note**
>
>This method should always be called, and always before any other calls on the [Output](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentProcessor_Output) interface.


**Output.SetFileProperties()**

The Bilingual Parser should call [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) method to provide the framework with a reference to the properties for each file in the document, before the paragraph-units of the file are processed by calling [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) as outlined below.

The [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) method takes an interface reference of type [IFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IFileProperties.yml) that needs to be created by the Bilingual Parser its self. This can be done using the [CreateFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentItemFactory.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentItemFactory_CreateFileProperties) method. However, a Bilingual parser will normally need to set the [IPersistentFileConversionProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml) property of the [IFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IFileProperties.yml) created object before passing it to [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_). This property however can normally be obtained from the parameter of [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) when your parser class is derived from [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml). You may also need to update other properties of the [IFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IFileProperties.yml) object such as source and target language Tool name and version and creation date.

**Output.ProcessParagraphUnit()**

The Bilingual Parser should call [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) method for each paragraph-unit found in the source file being parsed. The parameter of type [IParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) must be created using a call to [CreateParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentItemFactory.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentItemFactory_CreateParagraphUnit_Sdl_FileTypeSupport_Framework_NativeApi_LockTypeFlags_). When creating a paragraph-unit the source and target language is specified together with a lock type. Normally paragraph-units are created as structure paragraph-units with lock type [Structure](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.LockTypeFlags.yml#fields) or translatable paragraph-unit of lock type [Unlocked](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.LockTypeFlags.yml#fields).

**Output.FileComplete()**

The Bilingual Processor should call [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete) method after all paragraph-units in a file have been processed by calling [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) for each one. After calling [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete) the file properties should not be changed any further.

**Output.Complete()**

The Bilingual Processor should call [Complete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Complete) method after all content in all files have been processed. After calling [Complete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Complete) the document properties should not be changed any further.

Implementing IParser
--

**IParser.OnProgress**

The [IParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IParser.yml) interface contains an event [OnProgress](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_OnProgress_System_Byte_) or type [ProgressEventArgs](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ProgressEventArgs.yml) that must be defined in classes deriving from it. This event can be used to notify the framework and hence the user of the file parsing progress. As well as calling [OnProgress](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileParser_OnProgress_System_Byte_) with a suitable percentage of the file currently being processed it is normally expected to be called with a parameter of 0% before opening a file and with a parameter of 100% when file reading is complete.

**IParser.ParseNext()**

The [IParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IParser.yml) interface contains a [ParseNext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_IParser_ParseNext) method which must also be implemented. This method is called repeatedly by the framework to process the next chunk of input from the source bilingual document.

The implementation should parse a suitable chunk (preferably not large) of the input and return a bool that indicates if there is more work to be done before this file is completely parsed. When there is no more of the source file to process then the [ParseNext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IParser.yml#Sdl_FileTypeSupport_Framework_NativeApi_IParser_ParseNext) method should return false to indicate that there is no more file content to be processed.

Typically it is in this method, or in methods called by this, that the Output property’s methods are called to inform the framework of the entire source files content.

Implementing INativeContentCycleAware
--

If deriving from the [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml) interface you must implement the following three methods.

**SetFileProperties()**

[SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) methods standard implementation is to save its parameter properties of type [IFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IFileProperties.yml) to a class variable. These file properties can then be used by the bilingual parser to supplement the information available from the source file contents where this information may not be known, such as the original file encoding.

**StartOfInput()**

Called by the framework after component initialisation i.e. after [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) , but before any content is parsed and passed to any of the SDL File Type Support Framework components.

**EndOfInput()**

This is called by the framework after processing of the bilingual content has finished.

Implementing ISettingsAware
--

If deriving from the [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml) interface you must implement the following method.

**InitializeSettings()**

[InitializeSettings](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_ISettingsAware_InitializeSettings_Sdl_Core_Settings_ISettingsBundle_System_String_) Passes in an [ISettingsBundle](../../api/core/Sdl.Core.Settings.ISettingsBundle.yml) object and a ```configurationId``` ```FileTypeConfigurationId```. These can be used to populate the required settings object used by the parser:

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

See Also
--

**Other Resources**

[The File Parser](the_file_parser.md)


>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.