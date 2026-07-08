# Reporting problems

This article explains how filter components can report problems to the application that hosts the File Type Support Framework during content processing.

## Overview

Filters can run in server-based scenarios, so they must not display UI such as a `MessageBox` or update an application's user interface directly. That behavior can block the server process because no user can respond.

When a file processing component encounters a fatal error, it should throw an exception and stop processing. Use an error message that clearly explains the problem and helps users resolve it. When possible, derive content-processing exceptions from [FileTypeSupportException](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.FileTypeSupportException.yml).

Some events do not justify aborting processing, but users may still need to see them. The File Type Support Framework provides a message reporting mechanism for non-fatal errors, warnings, and notes. Components can also associate messages with specific content locations. In the **Messages** window of Var:ProductName, for example, users can double-click a message to jump to the location where the framework detected the issue, such as a missing tag.

All framework-provided message reporters implement [IBasicMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IBasicMessageReporter.yml). The [ReportMessage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IBasicMessageReporter.yml#Sdl_FileTypeSupport_Framework_NativeApi_IBasicMessageReporter_ReportMessage_System_Object_System_String_Sdl_FileTypeSupport_Framework_NativeApi_ErrorLevel_System_String_System_String_) method accepts an optional `locationDescription` parameter. Use this parameter when a component cannot provide exact coordinates but can still describe the relevant location in a way that helps users find it.

Components can also attach precise location information. Applications such as Var:ProductName can use that information to navigate to a segment or another relevant location in the editor. The exact mechanism depends on the component type. The framework also translates message locations differently depending on where the component reported the message, although that translation remains transparent to both the filter component and the host application. The following sections describe each component type.

Exact location information can identify a single spot or a range of content. To report a range, provide separate `from` and `up to` locations. The `up to` location is optional. If you omit it, the framework treats the location as a single spot.

The framework translates all message locations into a unified representation through [IMessageLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IMessageLocation.yml). An application can pass that object to the editor to resolve the corresponding document location and enable click-on message behavior.

The framework can also serialize locations for batch processing, although this mechanism was not implemented at the time of writing.

## File sniffer

An [INativeFileSniffer](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml) receives an [INativeTextLocationMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeTextLocationMessageReporter.yml) in the [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method. The sniffer can use this reporter to send relevant information and associated locations to the File Type Support Framework.

The main purpose of a file sniffer is to determine whether it supports a file. Under normal circumstances, a sniffer should not report issues for unsupported files because another filter might support the same file correctly. In some cases, however, a warning helps users. Examples include a locked file or a file that matches the file type but still cannot be processed, such as a DOC file that contains unaccepted changes.

A file sniffer should not throw exceptions. If it does, the framework usually treats that exception as a bug in the sniffer. The File Type Support Framework catches the exception, reports it as a warning through the same message reporting mechanism, and treats the file as unsupported by the corresponding File Type Component Builder.

File sniffers report locations with [NativeTextLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.NativeTextLocation.yml) instances, which use 1-based line and offset numbers.

The framework only uses these locations if it actually opens the file. Users may still see messages from all invoked file sniffers, even if the selected File Type Component Builder came from a different sniffer. To avoid confusion, report only issues that users are very likely to care about. In practice, this usually works well because most file types invoke only one sniffer.

When the framework opens a file, it translates the reported locations automatically so the editor can support click-on messages. The File Type Support Framework listens to the native file parser output and inserts location markers into the content stream at the reported line and offset numbers. It derives those positions from the literal text and tag content that the parser emits through the native API.

These translated locations may not always match the final bilingual content exactly. A file tweaker may change the native file before the parser runs. The parser may also omit some literal text or tag content, or emit content in a different order than the native file. File sniffer locations work best when the parser preserves the native file content and represents it as tags and text. If that does not apply to the file type, provide a location description instead of an exact location.

The following example reports a warning from a file sniffer:

```cs
void ReportWarning(string message, int lineNumber, int offset, INativeTextLocationMessageReporter messageReporter)
{
    NativeTextLocation from = new NativeTextLocation(lineNumber, offset);
    messageReporter.ReportMessage(this, "SDK Sample", ErrorLevel.Warning, message, from, null);
}
```

## File tweaker

File tweakers can modify the native file before the native parser processes it, or modify the translated native file after the file writer finishes. Because they do not access the native content stream or the bilingual content model directly, they must report locations as line and offset numbers in the native file.

During initialization, the File Type Support Framework sets the [MessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IFileTweaker.yml#Sdl_FileTypeSupport_Framework_NativeApi_IFileTweaker_MessageReporter) property on the file tweaker. The tweaker can then use that reporter to send messages and associated locations to the framework.

For pre-tweaking, the File Type Support Framework handles file tweaker locations in the same way that it handles file sniffer locations. It inserts location markers into the native content stream at the reported line and offset numbers.

For post-tweaking, the framework tracks the line and offset numbers for literal text and tag content that the native file writer receives. It uses that information to map the reported location to a position in the native content stream, and then tries to translate that position into a corresponding paragraph unit, segment, and character offset in the bilingual content.

As with file sniffers, file tweaker locations may not always translate correctly into bilingual content locations. They work best when the native content stream contains the full native file content as tags and text. If that does not apply to the file type, use a location description instead of exact coordinates.

The following example reports a message from a file tweaker:

```cs
void ReportMessage(string message, ErrorLevel severity, int lineNumber, int offset, INativeTextLocationMessageReporter messageReporter)
{
    NativeTextLocation from = new NativeTextLocation(lineNumber, offset);
    messageReporter.ReportMessage(this, "SDK Sample", severity, message, from, null);
}
```

## Native file parser and native content processor for extraction and generation

Components that generate output through the native API can mark locations in the native content by creating a [LocationMarkerId](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.LocationMarkerId.yml) and calling [LocationMark](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_LocationMark_Sdl_FileTypeSupport_Framework_NativeApi_LocationMarkerId_). They can then pass those marker IDs as `from` and `up to` locations when they report a message through [INativeContentStreamMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentStreamMessageReporter.yml). The File Type Support Framework provides the message reporter through the component's [MessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IFileTweaker.yml#Sdl_FileTypeSupport_Framework_NativeApi_IFileTweaker_MessageReporter) property.

During extraction, the framework propagates location markers into the bilingual content model as [ILocationMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILocationMarker.yml) instances. This approach provides highly reliable locations and often still points to the correct place even after users edit the text extensively.

During generation, the framework detects location markers in the native content stream and translates them into corresponding locations in the bilingual content model. In this case, the resulting bilingual locations usually are not location marker objects. This translation is less direct because the native API content can differ significantly from the bilingual API content. For example, the framework may merge tag pairs automatically or clone source content into the target for untranslated parts.

To improve accuracy, the File Type Support Framework anchors the reported location to the nearest preceding segment or paragraph boundary detected in the native generation API. It then counts characters in the native text and tags between that boundary and the location marker. Next, it uses that count to find the corresponding paragraph unit and, when possible, the segment in the bilingual content model. Finally, it determines the location inside that paragraph or segment by counting characters in text and tag properties. The framework restricts the final location to the resolved segment, even if the character count does not map perfectly. As a result, generation-time locations are usually accurate enough to identify the correct editor row, but they are less reliable than parsing-time locations. If a user edits the same segment, the location may become inaccurate. Edits in neighboring segments should not affect it.

The following example reports a suspicious tag from a native content processor. This processor derives from `AbstractNativeExtractionContentProcessor` or `AbstractNativeGenerationContentProcessor` and overrides placeholder tag handling:

```cs
public override void PlaceholderTag(IPlaceholderTagProperties tagInfo)
{
    if (IsSuspiciousTag(tagInfo))
    {
        LocationMarkerId fromId = new LocationMarkerId();
        LocationMarkerId uptoId = new LocationMarkerId();

        LocationMark(fromId);
        base.PlaceholderTag(tagInfo);
        LocationMark(uptoId);

        MessageReporter.ReportMessage(this, "SDK Sample", ErrorLevel.Warning, "Suspicious tag!", fromId, uptoId);
        return;
    }

    base.PlaceholderTag(tagInfo);
}
```

## Bilingual content processor, bilingual parser, and bilingual writer

Components that work directly with the bilingual content model can associate message locations with either the source or the target content. The [IBilingualContentMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentMessageReporter.yml) interface accepts [TextLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.TextLocation.yml) instances for the `from` and `up to` locations.

The [TextLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.TextLocation.yml) class lets you reference a text position directly in the bilingual content model. It provides several constructors. One convenient option accepts a single `IMarkupData` item as the referenced location.

Because components can report messages with `TextLocation`, they do not need to alter the bilingual object model. They also do not need location markers.

At its core, [TextLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.TextLocation.yml) wraps a [Location](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.Location.yml), which lets you reference positions before and after items in the content model.

Direct bilingual locations usually provide the most reliable message references because the File Type Support Framework does far less translation work.

The following example reports problematic text from a bilingual content processor:

```cs
void ReportOffendingText(IText textItem, int startOffset, int numberOfCharacters)
{
    TextLocation from = new TextLocation(textItem, startOffset);
    TextLocation upto = new TextLocation(textItem, startOffset + numberOfCharacters);

    MessageReporter.ReportMessage(this, "SDK Sample", ErrorLevel.Warning, "Inappropriate expression!", from, upto);
}
```

## Native file writer

A native file writer receives content on the native input stream, but it writes output directly to the file that it creates instead of writing through an API provided by the File Type Support Framework. For that reason, native file writers must report locations through the [INativeTextLocationMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeTextLocationMessageReporter.yml) instance that the framework provides through the [MessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IFileTweaker.yml#Sdl_FileTypeSupport_Framework_NativeApi_IFileTweaker_MessageReporter) property.

Native file writers report locations as line and offset numbers that refer to the text and tag content passed to the native content handler implementation. The File Type Support Framework translates those locations into bilingual content locations by anchoring them to the nearest available paragraph and segment boundaries that the writer processes.

If a native file writer encounters a fatal error and cannot generate the output file, it should throw an exception. The File Type Support Framework catches exceptions from native file writers and automatically generates a message with an associated location that points to the content being processed when the exception was thrown. If the writer throws the exception exactly when it processes the problematic content, that generated location is usually sufficient. If the problematic content appears earlier in the stream, however, the generated location can mislead users. In that case, report the error explicitly with the correct location before you throw the exception.

During initialization, the File Type Support Framework also sets the [LocationTracker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileWriter.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileWriter_LocationTracker) property with an [INativeLocationTracker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeLocationTracker.yml) implementation. Use that tracker to determine the location of the content currently being processed.

The following example reports an unexpected start tag from a native file writer:

```cs
public override void InlineStartTag(IStartTagProperties tagInfo)
{
    if (!IsExpectedStartTag(tagInfo))
    {
        NativeTextLocation from = GetLocationBeforeCurrent();
        NativeTextLocation upto = GetLocationAfterCurrent();

        MessageReporter.ReportMessage(this, "SDK Sample", ErrorLevel.Error, "Unexpected tag pair!", from, upto);
        return;
    }

    ProcessStartTag(tagInfo);
}
```

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
