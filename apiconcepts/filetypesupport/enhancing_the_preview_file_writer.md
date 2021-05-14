Enhancing the Preview File Writer
==

In this chapter you will learn how to enhance the preview writer component to support the dynamic functionality of the real-time preview.

Why you Should Enhance the Preview Writer
--

For the static preview writer it was sufficient to output very simple HTML code (see Implementing the Preview Writer). However, to implement a dynamic real-time preview, which allows users to click segments and jump to the corresponding row in the side-by-side editor, the preview needs to implement active elements through the addition of CSS and JavaScript.

Enhance the HTML Header
--

The currently selected (i.e. active) segment in the real-time preview should be highlighted, e.g. with a silver background, while unselected segments retain a white background. In our case, this can be best implemented through CSS stylesheets. Therefore, the header of the preview file will contain the following styles, which are applied to the currently selected segment (```activesegment```) and to the unselected segments (```normal```).

```html
<style type="text/css">
        body {color:grey; font-size:10pt; font-family:@Arial Unicode MS;}
        span {color:black; cursor:hand;}

        // real-time preview related styles
        .activesegment {color:red; background-color:silver; cursor:hand;}
        .normal {color:black; background-color:white; cursor:hand;}                
 </style>
```

Moreover, we will use two JavaScript functions to apply the ```activesegment``` or the normal styles. These JavaScript functions can later be called from the Web browser preview control, which we will implement in one of the following chapters (see [Adding a Preview UI Control](adding_a_preview_ui_control.md)).


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

First, it is best to add a separate helper function that generates the HTML header as shown below:

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
Modify the [StartOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_StartOfInput) member as shown below to call the above helper function, which generates the HTML header:

```cs
// start the preview output
public void StartOfInput()
{
    _preview = new StreamWriter(OutputProperties.OutputFilePath);
    _preview.WriteLine(GetHTMLStart());
}
```

Enhance the Segment Output to Allow for Navigation
--

To determine a particular segment to be highlighted in the real-time preview when the user selects it in the side-by-side editor (or vice versa), you require two parameters: the paragraph unit id and the segment id.

First, we add the paragraph unit as a string member to the writer class:

```cs
string _paragraphUnitId = String.Empty;
```


Then we enhance the [ParagraphUnitStart](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileWriter_ParagraphUnitStart_Sdl_FileTypeSupport_Framework_NativeApi_IParagraphUnitProperties_) method to set the value of ```_paragraphUnitId``` to the id of the currently selected paragraph unit.

```cs
// each paragraph unit should appear in a new line
// therefore use a DIV element
public override void ParagraphUnitStart(IParagraphUnitProperties properties)
{
    _preview.WriteLine("<div>");
    _paragraphUnitId = properties.ParagraphUnitId.Id;
```

Next, we make the segments in the real-time preview 'clickable' by outputting the following HTML through the [SegmentStart](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileWriter_SegmentStart_Sdl_FileTypeSupport_Framework_NativeApi_ISegmentPairProperties_) method:

```cs
// enclose each segment in a SPAN tag pair
public override void SegmentStart(ISegmentPairProperties properties)
{
        _preview.Write("<span id=\"" + properties.Id.Id + "\" onClick=\"window.external.SelectSegment('" + _paragraphUnitId + "','" + properties.Id.Id + "')\" >");
}
```

The **SPAN** tag now includes an onClick event handler, which passes the segment id and the paragraph unit id.
Below you see an example of the HTML code the enhanced preview writer will generate for a paragraph unit that contains two segments:

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

Putting it All Together
--

Your enhanced preview writer class should now look as shown below. Note that the same preview writer can be used both for the internal static and the real-time preview. There is no harm in having the styles and JavaScript functions in the static preview, they will just never be called.

```cs
using System;
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Sdk.FileTypeSupport.Samples.SimpleText.Preview
{
    class InternalPreviewWriter : AbstractNativeFileWriter, INativeContentCycleAware
    {
        StreamWriter _preview = null;
        string _paragraphUnitId = String.Empty;

        public void SetFileProperties(IFileProperties properties)
        {
            // not used in this implementation
        }

        #region "start output"
        // start the preview output
        public void StartOfInput()
        {
            _preview = new StreamWriter(OutputProperties.OutputFilePath);
            _preview.WriteLine(GetHTMLStart());
        }
        #endregion

        #region "text"
        // output the translatable strings
        public override void Text(ITextProperties textInfo)
        {            
            _preview.Write(textInfo.Text);
        }
        #endregion

        #region "para"
        #region "para start"
        // each paragraph unit should appear in a new line
        // therefore use a DIV element
        public override void ParagraphUnitStart(IParagraphUnitProperties properties)
        {
            _preview.WriteLine("<div>");
            _paragraphUnitId = properties.ParagraphUnitId.Id;
        }
        #endregion


        public override void ParagraphUnitEnd()
        {
            _preview.Write("</div>");
        }
        #endregion


        #region "segment"
        // enclose each segment in a SPAN tag pair
        public override void SegmentStart(ISegmentPairProperties properties)
        {
                _preview.Write("<span id=\"" + properties.Id.Id + "\" onClick=\"window.external.SelectSegment('" + _paragraphUnitId + "','" + properties.Id.Id + "')\" >");
        }
        #endregion

        public override void SegmentEnd()
        {
            _preview.Write("</span>");
        }


        #region "inline tags"
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
        #endregion



        #region "end output"
        // end the preview output
        public void EndOfInput()
        {
            _preview.WriteLine("</body></html>");
            _preview.Close();
        }
        #endregion

        #region "html start"
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
        #endregion
    }
}
```
>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
