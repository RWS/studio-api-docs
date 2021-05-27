About the Example BIL Format
===

In order to demonstrate the main features that can be used when developing a bilingual file type plug-in, we have created an example format with the extension **.bil* (from now on BIL). This file type is not used in any 3rd party application. It is a fictitious document format, which was created only for this SDK sample project.

Format Description
--

BIL is custom a XML based format which looks as shown below:

# [Xml](#tab/tabid-1)
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
</bilingualdocument>
```
***


The BIL format contains the following elements:

* ```bilingualdocument```: The root element, in which the source and the target languages are specified within element attributes.
* ```unit```: Acts as the container for the source and the target content. Each unit element is identified by the unique value of the id attribute. Also the unit element contains a status attribute, which defines whether the current unit is, for example, new or an exact match.
* ```source```: Contains one seg element, which stores the actual source segment content.
* ```target```: Can contain a seg element for storing the target content. The target element can be empty if no target segment is available.
* ```seg```: Encloses the actual source or target content. Segment content may be enclosed in inline tags such as *< b >*, *< i >*, etc.
* ```type```: Contains additional information on the type of unit, e.g. heading, paragraph, footnote, etc.
* ```comment```: contains a comment related to a unit (if applicable). To keep this example as simple as possible, let us assume that a unit can only have one comment.

What the Sample File Type Plug-in Must Do
--

Your file type plug-in needs to fulfill the following requirements:

* Generate an SDL XLIFF file from a given BIL document
* Traverse the BIL file and extract all units; each unit shall represent a paragraph in the translation editor of <Var:ProductName>
* Expose all source segments for translation
* Expose all target segments (if available)
* Markup inline tags such as *< b>* as tag pairs within the segments and apply the appropriate display formatting
* Generate (untranslatable) contexts from the type attribute values (e.g. heading)
* Map the BIL unit status values to the appropriate confirmation levels found in the intermediary format used by <Var:ProductName> (i.e. SDL XLIFF), e.g. translated, draft, approved, etc.
* Back conversion, i.e. writing back the content from the intermediary document (SDL XLIFF)into a target BIL file

When developing the sample file type plug-in we fill primarily focus on extraction (file parser) and generation (file writer) as well as ways of mapping BIL features to SDL XLIFF features. We will not implement additional functionality such as QuickInsert, user-configurable settings, or document preview, as this is already covered in the chapter on developing a native file type plug-in.

Example Document
Below you see an example of a more comprehensive document, which you can use for testing your BIL file type plug-in:

# [Xml](#tab/tabid-2)
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
    <target></target>
    <type spec="Heading"/>
  </unit>
  <unit id="4" status="new">
    <source>
      <seg><i>SDL Trados</i> is the world’s most popular translation software with over 170,000 users.</seg>
    </source>
    <target></target>
<type spec="Box"/>
  </unit>
  <unit id="5" status="new">
    <source>
      <seg>
        <b>Dedicated translation environment to handle almost any file type</b>
      </seg>
    </source>
    <target></target>
    <type spec="Heading"/>
  </unit>
  <unit id="6" status="new">
    <source>
      <seg><b>Easily open</b> and start working on the widest range of file formats, from the latest Microsoft Office files to XML, from HTML to InDesign.</seg>
    </source>
    <target></target>
  <type spec="Box"/>
<comment id="1">This is the original comment</comment>
  </unit>
</bilingualdocument>
```
***


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.