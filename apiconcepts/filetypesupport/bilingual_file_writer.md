The Bilingual File Writer
==

The bilingual file writer component is responsible for translated file creating. While the native file writer has access only to the target content, the bilingual filter has access to each paragraph-unit object and can access both source and target segments as well as other bilingual model objects like context etc.

Writing a bilingual writer
--

When writing a bilingual file writer you must derive your writer class from the interfaces [IBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualFileTypeComponent.yml) and [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml). These interfaces provide a way of coupling to the SDL File Type Support Framework and allowing it to initialise the writer with the necessary objects it needs to interact with the framework. A writer will also normally derive from the INativeOutputSettingsAware and [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml) interfaces which provide the writer with additional information such as the target file path name and preferred target encoding and allow for the population of any associated writer settings objects.

However, the simplest way to implement a Bilingual Writer is to derive from the [IBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualFileTypeComponent.yml) class rather than directly from the [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interface.

When processing files in a Bilingual Processor the Framework API has the facility to separate compound or multiple documents into individual files for processing. This facility forms part of the [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interface and a group of documents is initialised during the [Initialize](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Initialize_Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_) call and finalised during the [Complete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Complete) call. Between these calls multiple sub-documents or files are initialised by a call to [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) and flagged as complete during call to [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete). For each individual file within a compound or multiple file document the file processing is broken into manageable units of translatable content or paragraph-units and each of these [IParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) based objects is processed in the [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) call.

Deriving from AbstractBilingualFileTypeComponent
--

The [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) base class provides an implementation of the [IBilingualContentMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentMessageReporter.yml) and [IBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualFileTypeComponent.yml) interfaces leaving the [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) and [INativeOutputSettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputSettingsAware.yml) interfaces to be implemented by the derived class. If the [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) class is used as a base class then this saves the derived class from having to provide implementations for the properties of the base interfaces that it is derived from. This also enables the use of the helper function [ReportMessage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentMessageReporter.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentMessageReporter_ReportMessage_System_Object_System_String_Sdl_FileTypeSupport_Framework_NativeApi_ErrorLevel_System_String_Sdl_FileTypeSupport_Framework_BilingualApi_TextLocation_Sdl_FileTypeSupport_Framework_BilingualApi_TextLocation_) by its derived classes.

You may have noticed that the [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interface that the Bilingual Writer is derived from is the same interface that is used for the [Output](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentProcessor_Output) property of the [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml) based Bilingual Parser. This is so that all text, tags and other mark-up items that are written during the parsing phase are re-used for the translated file writing phase with the mark-up order and translated text content determined by the translation process, However, the translated document will only contain tags that were created during the parsing phase and the Bilingual Writer can rely on the tag content of each tag to be the same as it was when it was written by the parser with the exception of any manually mark-up in a framework based editor. If a filter does allow for manually created mark-up such as added **Bold**, *Italic* or <u>Underlined</u> formatting in the translation then this will be defined in the File Type Component Builder file. The only other exception to this is when the parser class has created a tag with localizable content in it. If this is the case then the LocalizableContent property of the tag properties must be examined to retrieve the text for the localizable sub-segments.

The coding of the writer class is usually simpler than the parser as the actual tag content can usually be used verbatim when writing file types such as XML and HTML (provided they were extracted as the actual XML or HTML tag content). For other file types the tag content should refer to the type or location of the formatting or mark-up that the tag represents. This tag content string referring to formatting or mark-up in the original document is normally written as a custom XML tag. If required the original document can be encoded during the parsing phase and a copy made available to the Bilingual API writer, please see [DependencyFiles](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_DependencyFiles) property for more details on how to include dependency files during the parsing phase.

Implementing IBilingualContentProcessor
--

The Bilingual Writer class will need to derive from [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) and provide implementations of its inherited abstract properties and methods. These properties and methods provide a way of coupling the Bilingual Writer class to the SDL File Type Support Framework in a way that mirrors the same API methods and properties that were used during the parsing phase. It is also worth noting that the same [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interface is used for Bilingual Content Processors that are able to be included in the chain of processing during the extract or generate phase to modify or enhance the content generated by a filter or editor.

To facilitate processing in a streaming manner without requiring the entire document object in memory at any time the implementation of a [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interface will be fed paragraph-units one by one through calls to [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_). Document and file properties will also be provided by the framework by calling [Initialize](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Initialize_Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_) and setting [IDocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml) before processing paragraph-units in each document or file.

**Initialize()**

The framework will call [Initialize](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Initialize_Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_) method to provide the implementation with a reference to the document properties for the document being processed. Most implementations of this interface will simply store the parameter of type [IDocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml) in a private member variable. This method will always be called, and always before any other calls on this interface. This or other content processor implementations may modify the document properties as part of the document processing. If you need to access the final version of the document properties you should do that in the [Complete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Complete) method implementation.

**Complete()**

[Complete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Complete) method is called by the framework when all content has been processed. It is now pretty safe to assume that document properties will not change any further. Any class tidy-up can be done here as apposed to within the [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete) method below that is normally used for individual file processing tidy-up.

**SetFileProperties()**

The framework will call [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) method to provide the implementation with a reference to the properties for each file in the document, before the individual paragraph-units of the file are processed. This Bilingual Writer or other content processor implementations may modify the properties as part of the file processing. If you need to access the Final version of the file properties you should do that in the [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete) method implementation.

**FileComplete()**

[FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete) method is called by the framework after all paragraph-units in a file have been processed by all components. It is now pretty safe to assume that file properties won't change further. Individual file processing tidy-up can also be done in the implementation of this method.

**ProcessParagraphUnit()**

The framework will call [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) method for each paragraph-unit in each individual file that is processed. It is up to the implementer of the Bilingual filter how the passed [IParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) object is processed but there is an implementation of the visitor pattern that can be used with these paragraph-unit objects.

```cs
   #region "InitializeSettings"
public void InitializeSettings(Sdl.Core.Settings.ISettingsBundle settingsBundle, string configurationId)
{
    UserSettings _userSettings = new UserSettings();
    _userSettings.PopulateFromSettingsBundle(settingsBundle, configurationId);
    WriteUtf8Bom = _userSettings.WriteUtf8Bom;
}

#endregion
```

See Also
--

**Other Resources**

[The File Writer](the_file_writer.md)

[IMarkupDataVisitor](imarkupdatavisitor.md)

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.