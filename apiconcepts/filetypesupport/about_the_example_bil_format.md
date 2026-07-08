# About the Example BIL Format

To demonstrate the main features of a bilingual file type plug-in, this sample uses a fictitious format with the `.bil` extension. This format does not belong to a third-party application. It exists only for this SDK sample.

## Format Description

BIL is a custom XML-based format, as shown in the following example:

# [XML](#tab/tabid-1)
```xml
<bilingualdocument source-language="en-US" target-language="de-DE">
  <unit id="1" status="exact">
    <source>
      <seg id="1">
        <b>Features</b>
      </seg>
    </source>
    <target>
      <seg id="1">
        <b>Leistungsmerkmale</b>
      </seg>
    </target>
    <comment id="1">This segment was translated using web translator.</comment>
    <type spec="Heading"/>
  </unit>
</bilingualdocument>
```

The BIL format contains the following elements:

- `bilingualdocument`: The root element. Its attributes specify the source and target languages.
- `unit`: The container for the source and target content. Each `unit` has a unique `id` attribute and a `status` attribute that identifies the translation state, such as `new` or `exact`.
- `source`: Contains a `seg` element that stores the source segment content.
- `target`: Can contain a `seg` element that stores the target content. The `target` element can remain empty when no target segment is available.
- `seg`: Encloses the source or target content. Segment content can contain inline tags such as `<b>` and `<i>`.
- `type`: Contains additional information about the unit type, such as heading, paragraph, or footnote.
- `comment`: Contains a comment for a unit, when applicable. For simplicity, this example assumes that each unit has at most one comment.

## What the Sample File Type Plug-in Must Do

The sample file type plug-in must:

- Generate an SDLXLIFF file from a BIL document.
- Traverse the BIL file and extract all units so that each unit appears as a paragraph in the Var:ProductName translation editor.
- Expose all source segments for translation.
- Expose all target segments, when available.
- Mark up inline tags such as `<b>` as tag pairs within segments and apply the correct display formatting.
- Generate untranslatable contexts from `type` attribute values such as `heading`.
- Map BIL `status` values to the appropriate confirmation levels in the intermediary format used by Var:ProductName, such as `translated`, `draft`, and `approved`.
- Support back-conversion by writing content from the intermediary SDLXLIFF document to a target BIL file.

This sample focuses on extraction through the file parser, generation through the file writer, and mapping BIL features to SDLXLIFF features. It does not implement additional functionality such as QuickInsert, user-configurable settings, or document preview, because the chapter on developing a native file type plug-in already covers those topics.

## Example Document

The following example shows a more comprehensive document that you can use to test your BIL file type plug-in:

# [XML](#tab/tabid-2)
```xml
<?xml version="1.0" encoding="utf-8"?>
<bilingualdocument source-language="en-US" target-language="de-DE">
  <unit id="1" status="exact">
    <source>
      <seg>
        <b>Features</b>
      </seg>
    </source>
    <target>
      <seg>
        <b>Leistungsmerkmale</b>
      </seg>
    </target>
    <comment id="1">This segment was translated using web translator.</comment>
    <type spec="Heading"/>
  </unit>
  <unit id="2" status="fuzzy">
    <source>
      <seg>
        <b>The future of translation technology is here!</b>
      </seg>
    </source>
    <target>
      <seg>
        <b>Die Zukunft der Übersetzungstechnologie hat begonnen!</b>
      </seg>
    </target>
    <comment id="1">This segment was translated using web translator.</comment>
  </unit>
  <unit id="3" status="new">
    <source>
      <seg>
        <b>The largest translation supply chain in the world</b>
      </seg>
    </source>
    <target />
    <type spec="Heading"/>
  </unit>
  <unit id="4" status="new">
    <source>
      <seg><i>Trados Studio</i> is the world’s most popular translation software with over 170,000 users.</seg>
    </source>
    <target />
    <type spec="Box"/>
  </unit>
  <unit id="5" status="new">
    <source>
      <seg>
        <b>Dedicated translation environment to handle almost any file type</b>
      </seg>
    </source>
    <target />
    <type spec="Heading"/>
  </unit>
  <unit id="6" status="new">
    <source>
      <seg><b>Easily open</b> and start working on the widest range of file formats, from the latest Microsoft Office files to XML, from HTML to InDesign.</seg>
    </source>
    <target />
    <type spec="Box"/>
    <comment id="1">This is the original comment</comment>
  </unit>
</bilingualdocument>
```

>[!NOTE]
>
> This content may be out of date. To verify the latest information on this topic, inspect the libraries in the Visual Studio Object Browser.
