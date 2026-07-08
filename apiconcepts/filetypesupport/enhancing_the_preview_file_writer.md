# Enhancing the preview file writer

This article shows how to enhance the preview writer to support the dynamic behavior of the real-time preview.

## Why you should enhance the preview writer

The static preview writer only needs to generate simple HTML. See [Implementing the Preview Writer](implementing_the_preview_writer.md). A dynamic real-time preview needs more support. Users can click segments and jump to the corresponding row in the side-by-side editor, so the preview must include active elements through CSS and JavaScript.

## Enhance the HTML header

The real-time preview should highlight the currently selected segment, for example with a silver background. Unselected segments should keep a white background. In this sample, CSS handles that styling. Add the following styles to the preview file header. The `activesegment` style formats the selected segment, and the `normal` style formats unselected segments.

# [HTML](#tab/tabid-1)
```html
<style type="text/css">
        body {color:grey; font-size:10pt; font-family:@Arial Unicode MS;}
        span {color:black; cursor:hand;}

        // real-time preview related styles
        .activesegment {color:red; background-color:silver; cursor:hand;}
        .normal {color:black; background-color:white; cursor:hand;}                
 </style>
```

Add two JavaScript functions to switch between the `activesegment` and `normal` styles. The web browser preview control can call these functions later. See [Adding a Preview UI Control](adding_a_preview_ui_control.md).

# [HTML](#tab/tabid-2)
```html
<script type="text/javascript">
    function setActiveStyle(objDivID)
    {
            document.getElementById(objDivID).className = 'activesegment';
    }

    function setNormalStyle(objDivID)
    {
            document.getElementById(objDivID).className = 'normal';
    }
       </script>
```

Next, add a helper function that generates the HTML header:

# [C#](#tab/tabid-3)
```cs
// write the HTML header, which contains CSS styles
// and JavaScript functions, which can be called from the
// preview viewer control
private string GetHTMLStart()
{
    string header = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">\n";
    header += "<html>\n";
    header += "<head>\n";
    header += "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">\n";
    header += "<title>" + "Preview" + "</title>\n";

    header += "<style type=\"text/css\">\n";
    header += "<!--\n";
    header += "body{color:grey; font-size:10pt;font-family: @Arial Unicode MS;}\n";
    header += "span{color:black;cursor: hand;}\n";

    //real-time preview related styles
    header += ".activesegment{color:red;background-color: silver;cursor: hand;}\n";
    header += ".normal{color:black;background-color: white;cursor: hand;}\n";
    header += "//-->\n";
    header += "</style>\n";

    //real-time preview related JavaScript functions
    header += "<script type=\"text/javascript\">\n";
    header += "<!--\n";
    header += "function setActiveStyle(objDivID)\n";
    header += "{\n";
    header += "document.getElementById(objDivID).className = 'activesegment';\n";
    header += "}\n";
    header += "function setNormalStyle(objDivID)\n";
    header += "{\n";
    header += "document.getElementById(objDivID).className = 'normal';\n";
    header += "}\n";
    header += "//-->\n";
    header += "</script>\n";

    header += "</head>\n";
    header += "<body>\n";
    return header;
}
```

Update the [StartOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_StartOfInput) member to call this helper function:

# [C#](#tab/tabid-4)
```cs
// start the preview output
public void StartOfInput()
{
    _preview = new StreamWriter(OutputProperties.OutputFilePath);
    _preview.WriteLine(GetHTMLStart());
}
```

## Enhance the segment output to allow navigation

To highlight a segment in the real-time preview when the user selects it in the side-by-side editor, you need two parameters: the paragraph unit ID and the segment ID.

First, add the paragraph unit as a string member to the writer class:

# [C#](#tab/tabid-5)
```cs
string _paragraphUnitId = String.Empty;
```

Next, update the [ParagraphUnitStart](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileWriter_ParagraphUnitStart_Sdl_FileTypeSupport_Framework_NativeApi_IParagraphUnitProperties_) method so that `_paragraphUnitId` stores the ID of the current paragraph unit.

# [C#](#tab/tabid-6)
```cs
// each paragraph unit should appear in a new line
// therefore use a DIV element
public override void ParagraphUnitStart(IParagraphUnitProperties properties)
{
    _preview.WriteLine("<div>");
    _paragraphUnitId = properties.ParagraphUnitId.Id;
}
```

Then make the segments in the real-time preview clickable by writing the following HTML through the [SegmentStart](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileWriter_SegmentStart_Sdl_FileTypeSupport_Framework_NativeApi_ISegmentPairProperties_) method:

# [C#](#tab/tabid-7)
```cs
// enclose each segment in a SPAN tag pair
public override void SegmentStart(ISegmentPairProperties properties)
{
        _preview.Write("<span id=\"" + properties.Id.Id + "\" onClick=\"window.external.SelectSegment('" + _paragraphUnitId + "','" + properties.Id.Id + "')\" >");
}
```

The **SPAN** tag now includes an `onClick` event handler that passes the segment ID and the paragraph unit ID.

The following example shows the HTML generated for a paragraph unit that contains two segments:

# [HTML](#tab/tabid-8)
```html
<div> 
<span id="5" onclick="window.external.SelectSegment('5445c60d-445a-48ef-8623-8c63d56a9a51','5')">
Prompt me to re-open previously edited documents.
</span> 
<span id="6" onclick="window.external.SelectSegment('5445c60d-445a-48ef-8623-8c63d56a9a51','6')">
Opens a dialog box on start-up.
</span>
</div>
```

## Put it all together

The enhanced preview writer class should now look like this. You can use the same preview writer for both the internal static preview and the real-time preview. The static preview can include the styles and JavaScript functions even though it never calls them.

# [C#](#tab/tabid-9)
```cs
using System;
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.FileTypeSupport.Samples.SimpleText.Preview
{
    class InternalPreviewWriter : AbstractNativeFileWriter, INativeContentCycleAware
    {
        StreamWriter _preview = null;
        string _paragraphUnitId = String.Empty;

        public void SetFileProperties(IFileProperties properties)
        {
            // not used in this implementation
        }

        // start the preview output
        public void StartOfInput()
        {
            _preview = new StreamWriter(OutputProperties.OutputFilePath);
            _preview.WriteLine(GetHTMLStart());
        }

        // output the translatable strings
        public override void Text(ITextProperties textInfo)
        {            
            _preview.Write(textInfo.Text);
        }

        // each paragraph unit should appear in a new line
        // therefore use a DIV element
        public override void ParagraphUnitStart(IParagraphUnitProperties properties)
        {
            _preview.WriteLine("<div>");
            _paragraphUnitId = properties.ParagraphUnitId.Id;
        }


        public override void ParagraphUnitEnd()
        {
            _preview.Write("</div>");
        }


        // enclose each segment in a SPAN tag pair
        public override void SegmentStart(ISegmentPairProperties properties)
        {
                _preview.Write("<span id=\"" + properties.Id.Id + "\" onClick=\"window.external.SelectSegment('" + _paragraphUnitId + "','" + properties.Id.Id + "')\" >");
        }

        public override void SegmentEnd()
        {
            _preview.Write("</span>");
        }


        // output any inline tags,
        // which will also apply the corresponding character formatting
        public override void InlineStartTag(IStartTagProperties tagInfo)
        {
            _preview.Write(tagInfo.TagContent);
        }

        public override void InlineEndTag(IEndTagProperties tagInfo)
        {
            _preview.Write(tagInfo.TagContent);
        }



        // end the preview output
        public void EndOfInput()
        {
            _preview.WriteLine("</body></html>");
            _preview.Close();
        }

        // write the HTML header, which contains CSS styles
        // and JavaScript functions, which can be called from the
        // preview viewer control
        private string GetHTMLStart()
        {
            string header = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">\n";
            header += "<html>\n";
            header += "<head>\n";
            header += "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">\n";
            header += "<title>" + "Preview" + "</title>\n";

            header += "<style type=\"text/css\">\n";
            header += "<!--\n";
            header += "body{color:grey; font-size:10pt;font-family: @Arial Unicode MS;}\n";
            header += "span{color:black;cursor: hand;}\n";

            //real-time preview related styles
            header += ".activesegment{color:red;background-color: silver;cursor: hand;}\n";
            header += ".normal{color:black;background-color: white;cursor: hand;}\n";
            header += "//-->\n";
            header += "</style>\n";

            //real-time preview related JavaScript functions
            header += "<script type=\"text/javascript\">\n";
            header += "<!--\n";
            header += "function setActiveStyle(objDivID)\n";
            header += "{\n";
            header += "document.getElementById(objDivID).className = 'activesegment';\n";
            header += "}\n";
            header += "function setNormalStyle(objDivID)\n";
            header += "{\n";
            header += "document.getElementById(objDivID).className = 'normal';\n";
            header += "}\n";
            header += "//-->\n";
            header += "</script>\n";

            header += "</head>\n";
            header += "<body>\n";
            return header;
        }
    }
}
```

## See also

- [Implementing the Preview Writer](implementing_the_preview_writer.md)
- [Adding a Preview UI Control](adding_a_preview_ui_control.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
