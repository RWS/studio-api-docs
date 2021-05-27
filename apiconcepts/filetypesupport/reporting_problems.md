Reporting Problems
==

This topic covers different mechanisms that allows information to be communicated from the componentes of a filter to the application that hosts the SDL File Type Support Framework during content processing.

Overview
---
Since filters may be used in server-based scenarios, they cannot display information to users like normal Windows-based applications do, e.g. by using a ```MessageBox``` or directly updating the user interface of an application. Such behavior could cause the server process to hang, as there is no user to interact with the filter.

If a file processing component comes across a show-stopper, it should throw an exception to abort further processing. The exception should have an explanatory error message that clearly points users to the problem and helps them fix it. It is recommended that you derive exceptions thrown for events that occur during content processing from [FileTypeSupportException](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.FileTypeSupportException.yml).

A lot of events do not need to be treated as show-stoppers, but may still indicate that something is not correct. Such situations should not abort the processing, but may still be of interest to the user. The SDL File Type Support Framework provides a message reporting mechanism that can be used to communicate with the user. This can be used by filter components to report non-fatal errors, warnings and notes. Such messages can also be associated with specific content locations. For exampe, in the **Messages** window of <Var:ProductName> you can double-click a message, which leads you directly to the location in the document in which a problem was found (e.g. a missing tag).

The basic message reporting mechanism is the same for all components. It is accessible through the [IBasicMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IBasicMessageReporter.yml) interface, which is implemented by all message reporters provided by the framework. The [ReportMessage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IBasicMessageReporter.yml#Sdl_FileTypeSupport_Framework_NativeApi_IBasicMessageReporter_ReportMessage_System_Object_System_String_Sdl_FileTypeSupport_Framework_NativeApi_ErrorLevel_System_String_System_String_) method takes an optional ```locationDescription``` (string) parameter that allows the component to provide a textual description of the location that the message refers to. Showing the text (e.g. in a window or message box) can help users find the relevant location in the document. This mechanism can be useful when the component (for whatever reason) is unable to provide exact coordinates to the associated content.

As mentioned before, messages may be associated with detailed location information, which can be used by an application like the <Var:ProductName> to allow users to navigate, e.g. to a particular segment in the editor in which a problem has been identified. How to associate a message with precise location information depends on the type of component used. The mechanism to translate locations associated with messages also differs depending on where in the content processing chain the message is was generated (though this is transparent to the filter component and to the host application). Issues related to message reporting for each component type are covered in separate sections below.

Exact location information can represent either a single spot or a range of content. Ranges are reported by specifying separate "from" and an "up to" locations. The "up to" location is optional. If it is omitted, the location is interpreted as a single spot.

All message locations are translated by the framework into a unified location representation, in the form of [IMessageLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IMessageLocation.yml) instances. An application can e.g. pass this to the editor to get a corresponding location inside the document, to provide click-on message functionality.

The locations can also be serialized for use with batch processing (at the time of writing this mechanism was not implemented).

File Sniffer
--

A [INativeFileSniffer](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml) receives an instance of a [INativeTextLocationMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeTextLocationMessageReporter.yml) in the [Sniff](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileSniffer.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileSniffer_Sniff_System_String_Sdl_Core_Globalization_Language_Sdl_Core_Globalization_Codepage_Sdl_FileTypeSupport_Framework_NativeApi_INativeTextLocationMessageReporter_Sdl_Core_Settings_ISettingsGroup_) method. This can be used to communicate relevant information with associated locations to the SDL File Type Support Framework (and ultimately to the user).

The file sniffer's general purpose is to determine if the specified file is supported or not. Under normal circumstances, file sniffers would not report issues related to the file. This can sometimes be confusing to users, since there may be another filter that properly supports the file. However, in some cases it is helpful for the file sniffer to inform users, e.g. if the file is locked, or if the file type is currect, but cannot be supported for some reason (e.g. a DOC file that contains unaccepted changes).

A file sniffer is not expected to throw exceptions. If a file sniffer throws an exception, this normally indicates a bug in the file sniffer. Exceptions thrown by file sniffers are handled by the SDL File Type Support Framework and reported as warnings through the message reporting mechanism in the same way as messages reported by file sniffers through the message reporter passed to the Sniff() method. If a file sniffer throws an exception, the file is considered as not supported by the corresponding File Type Component Builder.

Locations for messages from a file sniffer are reported using [NativeTextLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.NativeTextLocation.yml) instances, which ultimately represent 1-based line and offset numbers.

The locations will only be used if the file has actually been opened. Messages from all invoked file sniffers may be shown to the user, regardless of whether they originate from the file sniffer that is part of the File Type Component Builder that was ultimately used to process the file. Since this can potentially be confusing to users, it is recommended that file sniffers only report issues that are very likely of interest to the user. (In real-life scenarios this is typically not a problem, as for most file types only a single file sniffer is invoked.)

Upon opening a file the locations is automatically translated by the SDL File Type Support Framework, so that they can be used to generate click-on messages in an editor. The SDL File Type Support Framework does this by listening to the output from the native file parser and by inserting location markers into the content stream at the line and offset numbers that correspond to the locations for messages reported by file sniffers. The line and offsets are determined from the literal text and tag content output by the parser to the native API.

From the nature of this mechanism it is obvious that the locations reported by the file sniffer may not correspond exactly to the correct locations in the bilingual content. File tweakers might have altered the native file before it was processed by the native parser. Moreover, the native parser may not emit all literal tag and text content through the native API, or it may choose to emit content in a different order than it appears in the native file. Message locations from file sniffers work best with file types in which all content in the native file is preserved and "marked up" as tags and text by the parser. For other file types it may be more suitable to provide a location description instead of an exact location.

Here is a code example that shows how to report a warning from a file sniffer:

# [C#](#tab/tabid-1)
```cs
void ReportWarning(string message, int lineNumber, int offset, INativeTextLocationMessageReporter messageReporter)
{
    NativeTextLocation from = new NativeTextLocation(lineNumber, offset);
    messageReporter.ReportMessage(this,"SDK Sample", ErrorLevel.Warning, message, from, null);
}
```
***

File Tweaker
--

File tweakers are invoked in order to alter the native file before processing with the native parser, and/or to alter the translated native file after processing by the file writer. As such they do not have access to the native content stream or the bilingual content model. So, similar to the file sniffers the file tweakers must report message locations in the form of line and offset numbers in the native file content.

During initialization the SDL File Type Support Framework sets the [MessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IFileTweaker.yml#Sdl_FileTypeSupport_Framework_NativeApi_IFileTweaker_MessageReporter) property on the file tweaker. The file tweaker can use this to report messages with associated locations to the framework.

For pre-tweaking the SDL File Type Support Framework treats file tweaker message locations similar to locations for messages reported by file sniffers, i.e. location markers are inserted into the native content stream at locations corresponding to the line and offset numbers.

For post-tweaking the SDL File Type Support Framework tracks the line and offset numbers for literal text and tag content passed to the native file writer, and uses this information to translate the location into a corresponding place in the native content stream. This is then translated into bilingual content locations by attempting to determine which paragraph unit and segment the location refers to, and how many characters into the segment the location appears in.

Like with file sniffers, the locations reported by file tweakers may not translate correctly into bilingual content locations, for pretty much the same reasons. Also, the message locations works best with file types in which the entire native content is present in the form of tags and text in the native content stream. If this is not the case for the file type being processed, file tweakers may use location descriptions rather than exact locations.

Here is a code example that shows how to communicate information from a file tweaker:

# [C#](#tab/tabid-2)
```cs
void ReportMessage(string message, ErrorLevel severity, int lineNumber, int offset, INativeTextLocationMessageReporter messageReporter)
{
    NativeTextLocation from = new NativeTextLocation(lineNumber, offset);
    messageReporter.ReportMessage(this,"SDK Sample", severity, message, from, null);
}
```
***

Native File Parser and Native Content Processor for Extraction and Generation
--

Components that generate output through the native API can mark locations in the native content by creating a new [LocationMarkerId](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.LocationMarkerId.yml) and and calling [LocationMark](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IAbstractNativeContentHandler.yml#Sdl_FileTypeSupport_Framework_NativeApi_IAbstractNativeContentHandler_LocationMark_Sdl_FileTypeSupport_Framework_NativeApi_LocationMarkerId_) to output the marker. Locations marked in this way can be associated with messages reported on the [INativeContentStreamMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentStreamMessageReporter.yml) interface by passing the location marker ID as a "from" or "up to" location. The message reporter implementation is provided by the SDL File Type Support Framework to these components through their implementation of the [MessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IFileTweaker.yml#Sdl_FileTypeSupport_Framework_NativeApi_IFileTweaker_MessageReporter) property.

For messages reported during extraction (parsing) the location marks are propagated into the bilingual content model as [ILocationMarker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.ILocationMarker.yml) instances, which are referenced explicitly as the bilingual locations. This provides very reliable locations, which often point to the correct locations even after the user has edited the text exensively.

For messages reported by native content processors during generation (writing) the SDL File Type Support Framework detects the location marks in the native content stream and does its utmost to translate them into corresponding locations in the bilingual content model. In this case the locations in the bilingual content model will typically NOT be location marker objects. The translation from the native generation into the bilingual content that is implemented in the SDL File Type Support Framework is not always straightforward. The native API content may look quite different from the bilingual API content due to automatic merging of tag pairs and cloning of source content into the target for parts that have not yet been translated, etc. The SDL File Type Support Framework attempts to work around the uncertainties as much as possible by "anchoring" the location to the nearest preceding segment or paragraph boundary as detected in the native generation API. The approximate location is determined by counting the number of characters in the native text and tags between the last segment or paragraph boundary and the location marker in the native content stream. This value is then used with the bilingual content model to locate the corresponding paragraph unit and (if possible) segment. The location inside the paragraph or segment is determined by iterating through the segment content and counting characters in the text and tag properties. The bilingual location is restricted to the determined segment, even if it does not contain "enough" characters. That way the location should end up in the correct row in the editor in almost all cases. Therefore, locations reported by native content processors during generation are usually reasonably accurate, but cannot be relied on quite as much as locations reported by parsing. If the user edits the text in the same segment the locations may no longer be correct, however locations in neighboring segments should not be affected.

Here is a code example that shows how a 'suspicious' tag could be reported by a native content processor. The native content processor in this example would be derived from ```AbstractNativeExtractionContentProcessor``` or ```AbstractNativeGenerationContentProcessor``` and the function below overrides the processing of placeholder tags to check for tags that look 'suspicious' and report those to the user.

# [C#](#tab/tabid-3)
```cs
public void PlaceholderTag(IPlaceholderTagProperties tagInfo)
  {
    if (IsSuspiciousTag(tagInfo))
    {
      // insert location marks around the tag
      LocationMarkerId fromId = new LocationMarkerId();
      LocationMarkerId uptoId = new LocationMarkerId();

      // report the message
      MessageReporter.ReportMessage(this, "SDK Sample", ErrorLevel.Warning, "Suspicious tag!", fromId, uptoId);
    }
  }
```
***

Bilingual Content Processor, Bilingual Parser and Bilingual Writer
--

Components that work directly with the bilingual content model can associate message locations directly with either the source or the target content. The [IBilingualContentMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualContentMessageReporter.yml) interface takes [TextLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.TextLocation.yml) instances for the "from" and "up to" locations. The [TextLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.TextLocation.yml) class allows you to directly reference a text position inside the bilingual content model. It has several constructors. The one that you may find most convenient is the one that takes a single ````IMarkupData```` item as the location to reference.

It is important to note that the use of [TextLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.TextLocation.yml) instances means that components that report messages do not need to alter the content of the bilingual object model. This does not require the use of location markers.

The main component of the [TextLocation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.TextLocation.yml) is a [Location](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.Location.yml), which allows you to reference locations before and after items in the content model.

The locations in the bilingual content model is probably the most reliable message location reference method, as it involves much less 'guesswork' for the SDL File Type Support Framework to pinpoint the location.

Here is a code example that shows how a string that contains a problem could be reported by a bilingual content processor.

# [C#](#tab/tabid-4)
```cs
void ReportOffendingText(IText textItem, int startOffset, int numberOfCharacters)
{
    TextLocation from = new TextLocation(textItem, startOffset);
    TextLocation upto = new TextLocation(textItem, startOffset + numberOfCharacters);

    // report the message
    MessageReporter.ReportMessage(this,"SDK Sample", ErrorLevel.Warning, "Inappropriate expression!", from, upto);
}
```
***

Native File Writer
--

A native file writer receives content on the native input stream. However, the output does not occur through an API provided by the SDL File Type Support Framework, it rather writes directly into the output file that it is creating. For this reason native file writers must report message locations through the [INativeTextLocationMessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeTextLocationMessageReporter.yml) instance provided by the framework through the  [MessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IFileTweaker.yml#Sdl_FileTypeSupport_Framework_NativeApi_IFileTweaker_MessageReporter) property.

The message locations are reported as line and offset numbers that refer to the text and tag content that is passed to the writer's implementation of the native content handler interface. These locations are translated by the SDL File Type Support Framework into locations in the bilingual content model by "anchoring" them to the nearest available paragraph and segment boundaries processed through the writer.

If a native file writer encounters a serious error because of which the output file cannot be generated, it should throw an exception. The SDL File Type Support Framework automatically catches exceptions thrown by native file writers and generates a message for the exception with an associated location that points to the content that was being processed by the file writer, and then the exception again. Provided that the exception is thrown at the time that the 'problematic' content is passed to the native file writer, this should yield a valid location associated with the message. So, if that is the case, the writer does not need to explicitly generate a message. On the other hand, it often happens that the 'problematic' content comes up earlier in the content stream, in which case the location associated with the automatically generated message may be misleading. In such a case, it is recommended that file writers report the error with the appropriate message location before throwing the exception.

The SDL File Type Support Framework provides a [INativeLocationTracker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeLocationTracker.yml) implementation to the writer by setting the [LocationTracker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileWriter.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileWriter_LocationTracker) property during initialization of the writer. The location tracker provides a convenient way for the writer to determine the locations of the content that it is currently processing.

Below you find a code example that shows how an unexpected start tag could be reported by a native file writer:

# [C#](#tab/tabid-5)
```cs
public override void InlineStartTag(IStartTagProperties tagInfo)
{
    if (!IsExpectedStartTag(tagInfo))
    {
        // report the tag and ignore it
        NativeTextLocation from = GetLocationBeforeCurrent();
        NativeTextLocation upto = GetLocationAfterCurrent();

        MessageReporter.ReportMessage(this,"SDK Sample", ErrorLevel.Error, "Unexpected tag pair!", from, upto);
        return;
    }

    // process as normal
    ProcessStartTag(tagInfo);
}
```
**

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.