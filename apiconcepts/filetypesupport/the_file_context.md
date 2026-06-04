# The File Context

Translatable text can appear in one or more contexts. Examples include file names, URLs, domains, resource types, and layout properties such as headings, titles, and table cells. Contexts can contain other contexts, and they can overlap.

## Native File Processing API representation

During native content processing, the filter creates contexts through [ChangeContext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_ChangeContext_Sdl_FileTypeSupport_Framework_NativeApi_IContextProperties_). The [IContextProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextProperties.yml) parameter wraps an array of [IContextInfo](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml) objects.

`IContextInfo` exposes the following properties:

* [ContextType](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_ContextType)
* [DefaultFormatting](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_DefaultFormatting)
* [Description](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_Description)
* [DisplayCode](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_DisplayCode)
* [DisplayColor](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_DisplayColor)
* [DisplayName](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_DisplayName)
* [Purpose](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IContextInfo.yml#Sdl_FileTypeSupport_Framework_NativeApi_IContextInfo_Purpose)

Every call to [ChangeContext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_ChangeContext_Sdl_FileTypeSupport_Framework_NativeApi_IContextProperties_) closes the currently open contexts and opens the contexts specified in the call.

## Conversion from native to bilingual

Filters must identify, create, and maintain contexts. To pass them into the framework, use the [ChangeContext](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_ChangeContext_Sdl_FileTypeSupport_Framework_NativeApi_IContextProperties_) method on [IAbstractNativeContentHandler](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml). The framework does not provide a default context.

Contexts are optional, but they improve translation and match quality. For example, they help translators identify whether a string is a heading, footnote, or another content type that affects translation.

## Conversion from bilingual to native

Contexts are not part of the native file format, so they do not appear in translated output files.

## See also

- [Adding Context Information](adding_context_information.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
