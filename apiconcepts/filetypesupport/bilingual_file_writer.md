# The Bilingual File Writer

The bilingual file writer component creates translated files. Unlike the native file writer, which accesses only target content, the bilingual writer accesses each paragraph-unit object and can retrieve both source and target segments, as well as other bilingual model objects such as context.

## Writing a bilingual writer

Derive your writer class from the [IBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualFileTypeComponent.yml) and [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interfaces. These interfaces couple your writer to the File Type Support Framework and enable it to initialize with the necessary objects for framework interaction. Writers typically also derive from `INativeOutputSettingsAware` and [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml), which provide target file path, target encoding, and writer settings information.

For simplicity, derive from the [IBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualFileTypeComponent.yml) class instead of directly implementing [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml).

The Framework API separates compound or multiple documents into individual files for processing as part of the [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interface. The framework initializes document groups during the [Initialize](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Initialize_Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_) call and finalizes them during the [Complete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Complete) call. Between these calls, the framework initializes multiple sub-documents or files by calling [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) and marks them complete by calling [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete). For each file, the framework breaks processing into manageable units of translatable content (paragraph-units). Each [IParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) object is processed through the [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) call.

## Deriving from AbstractBilingualFileTypeComponent

The [AbstractBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.AbstractBilingualFileTypeComponent.yml) base class implements [IBilingualContentMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentMessageReporter.yml) and [IBilingualFileTypeComponent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualFileTypeComponent.yml), leaving [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) and [INativeOutputSettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputSettingsAware.yml) for your derived class to implement. Using this base class eliminates the need to implement inherited interface properties and methods, and grants access to the [ReportMessage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentMessageReporter.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentMessageReporter_ReportMessage_System_Object_System_String_Sdl_FileTypeSupport_Framework_NativeApi_ErrorLevel_System_String_Sdl_FileTypeSupport_Framework_BilingualApi_TextLocation_Sdl_FileTypeSupport_Framework_BilingualApi_TextLocation_) helper function.

The [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interface used by the Bilingual Writer is the same interface used by the [Output](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentProcessor_Output) property of the [IBilingualParser](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualParser.yml) based Bilingual Parser. This design reuses text, tags, and markup items written during parsing for the translated file writing phase, with markup order and content determined by the translation process. The translated document contains only tags created during parsing, and the Bilingual Writer can rely on tag content to remain identical to what the parser wrote, except for manually added markup in framework-based editors. If your filter allows manually created markup such as **Bold**, *Italic*, or <u>Underlined</u> formatting in translations, this must be defined in the File Type Component Builder file. Another exception occurs when the parser creates a tag with localizable content. In this case, examine the tag properties' `LocalizableContent` property to retrieve text for localizable sub-segments.

Writer class code is typically simpler than parser code. For XML and HTML file types, you can usually use tag content verbatim (if extracted as actual XML or HTML tag content). For other file types, tag content should reference the type or location of the formatting or markup that the tag represents, typically written as a custom XML tag. If needed, encode the original document during the parsing phase and make a copy available to the Bilingual API writer. See [DependencyFiles](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_DependencyFiles) for details on including dependency files during the parsing phase.

## Implementing IBilingualContentProcessor

Derive your Bilingual Writer class from [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) and implement its abstract properties and methods. These properties and methods couple your writer to the File Type Support Framework, mirroring the API methods and properties used during parsing. The same [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) interface is used for Bilingual Content Processors that participate in the extraction or generation processing chain to modify or enhance filter or editor content.

Your [IBilingualContentProcessor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentProcessor.yml) implementation processes streaming data efficiently without keeping the entire document in memory. The framework feeds paragraph-units one at a time through [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) calls. The framework provides document and file properties by calling [Initialize](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Initialize_Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_) and setting [IDocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml) before processing paragraph-units in each document or file.

### Initialize()

The framework calls [Initialize](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Initialize_Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentProperties_) to pass a reference to the document properties for the document being processed. Most implementations store the [IDocumentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentProperties.yml) parameter in a private member variable. This method is always called first, before any other calls on this interface. Content processor implementations may modify document properties during processing. Access the final document properties in the [Complete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Complete) method implementation.

### Complete()

The framework calls [Complete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_Complete) after all content has been processed. Document properties are now finalized and will not change further. Perform class-level cleanup here, in contrast to the [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete) method, which handles individual file cleanup.

### SetFileProperties()

The framework calls [SetFileProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_SetFileProperties_Sdl_FileTypeSupport_Framework_BilingualApi_IFileProperties_) to pass a reference to the properties for each file in the document, before the individual paragraph-units are processed. Your Bilingual Writer or other content processor implementations may modify properties during file processing. Access the final file properties in the [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete) method implementation.

### FileComplete()

The framework calls [FileComplete](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_FileComplete) after all paragraph-units in a file have been processed by all components. File properties are now finalized and will not change further. Perform individual file cleanup in this method implementation.

### ProcessParagraphUnit()

The framework calls [ProcessParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentHandler.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IBilingualContentHandler_ProcessParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_IParagraphUnit_) for each paragraph-unit in each individual file. Your implementation determines how to process the passed [IParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) object. The visitor pattern can be used with paragraph-unit objects to simplify processing.

# [C#](#tab/tabid-1)
```cs
public void InitializeSettings(Sdl.Core.Settings.ISettingsBundle settingsBundle, string configurationId)
{
    UserSettings _userSettings = new UserSettings();
    _userSettings.PopulateFromSettingsBundle(settingsBundle, configurationId);
    WriteUtf8Bom = _userSettings.WriteUtf8Bom;
}
```
***

## See Also

- [The File Writer](the_file_writer.md)
- [IMarkupDataVisitor](imarkupdatavisitor.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
