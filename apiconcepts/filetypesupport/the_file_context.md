The File Context
==

Translatable text can occur in one or more contexts. Examples of contexts are file names, URLs, domains, resource types, layout properties (e.g. heading, title, table cell, etc.), and potentially many more. Obviously, contexts can contain other contexts, and they can overlap with other contexts.

Native File Processing API Representation
--

During native content processing contexts are created by the filter through [ChangeContext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_ChangeContext_Sdl_FileTypeSupport_Framework_NativeApi_IContextProperties_). [IContextProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextProperties.yml), the input parameter type of [ChangeContext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_ChangeContext_Sdl_FileTypeSupport_Framework_NativeApi_IContextProperties_), wraps an [IContextInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml) array. [IContextInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml) has following properties:

* [ContextType](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_ContextType)
* [DefaultFormatting](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_DefaultFormatting)
* [Description](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_Description)
* [DisplayCode](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_DisplayCode)
* [DisplayColor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_DisplayColor)
* [DisplayName](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_DisplayName)
* [Purpose](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_Purpose)

Every call to [ChangeContext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_ChangeContext_Sdl_FileTypeSupport_Framework_NativeApi_IContextProperties_) closes all open contexts and opens all contexts specified in the call parameters.

Conversion from Native to Bilingual
--

Contexts must be identified, created, and maintained by filters. To pass them into the framework the filter must use the [ChangeContext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_ChangeContext_Sdl_FileTypeSupport_Framework_NativeApi_IContextProperties_) method of the [IAbstractNativeContentHandler](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml) interface. There is no default context. Using contects is not mandatory, but it is recommended to improve translation and match quality, as translators can easily identify a translatable string as e.g. a headline, footnote, etc., which may be important information for the translation process.

Conversion from Bilingual to Native
--

Contexts are not part of the native format and will therefore not show up in translated files.

See Also
--

**Other Resources**

[Adding Context Information](adding_context_information.md)


>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.