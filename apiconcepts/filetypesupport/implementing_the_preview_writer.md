Implementing the Preview Writer
==

In this chapter you will learn how to implement the writer component that generates the HTML code that is going to be displayed in the internal Web browser preview control.

Add the Preview Writer Class
--

Add a class called e.g. **InternalPreviewWriter.cs** to your project. The class needs to implement the same interfaces and members as the file writer class, which we use for generating the target files in their native format (see [Implementing the File Writer](implementing_the_file_writer.md)). The minimum amount of code required to build a file type plug-in with the internal preview writer looks as shown below:

```cs
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Sdk.Snippets.Native
{
    class InternalPreviewWriter : AbstractNativeFileWriter, INativeContentCycleAware
    {
        StreamWriter _preview = null;

        public void SetFileProperties(IFileProperties properties)
        {

        }

        public void StartOfInput()
        {

        }

        public void EndOfInput()
        {

        }
    }
}
```

Start the Preview Output
--

As preview output we can generate a highly simplified HTML document. First, write the start of the HTML file:

```cs
// start the preview output
public void StartOfInput()
{
    _preview = new StreamWriter(OutputProperties.OutputFilePath);
    _preview.WriteLine("<html><body>");
}
```

Output Text and Tags
--

Override the ```Text()``` method to output the translatable strings:

```cs
// output the translatable strings
public override void Text(ITextProperties textInfo)
{
    _preview.Write(textInfo.Text);
}
```

Let us assume that you want to insert a line break after each paragraph unit. To do this use the two following override methods to enclose each paragraph in a **DIV** tag pair:

```cs
// each paragraph unit should appear in a new line
// therefore use a DIV element
public override void ParagraphUnitStart(IParagraphUnitProperties properties)
{
    _preview.WriteLine("<div>");
}


public override void ParagraphUnitEnd()
{
    _preview.Write("</div>");
}
```

For good measure, enclose each segment in a **SPAN** tag pair. This is not actually required for the moment, but structuring the output through **SPAN** container tags will become important later, when we implement the real-time preview (see [Enhancing the Preview File Writer](enhancing_the_preview_file_writer.md)).

```cs
// enclose each segment in a SPAN tag pair
public override void SegmentStart(ISegmentPairProperties properties)
{
    _preview.Write("<span>");
}

public override void SegmentEnd()
{
    _preview.Write("</span>");
}
```

Now use the two following override methods to output the content of the inline tags. Note that by emitting the HTML tags we will also apply the corresponding character formatting in the Web browser preview control.

```cs
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
```

Note that we do not output any structure tag content in this preview.

Close the Preview Output
--

Last, close the preview as shown below:

```cs
// end the preview output
public void EndOfInput()
{
    _preview.WriteLine("</body></html>");
    _preview.Close();
}
```

The static HTML preview looks as shown in the following example:

![StaticHTMLPreview](images/StaticHTMLPreview.jpg)

Putting it All Together
--

If you put everything together, the preview writer class looks as shown below:

```cs
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Sdk.Snippets.Native
{
    class InternalPreviewWriter1 : AbstractNativeFileWriter, INativeContentCycleAware
    {
        StreamWriter _preview = null;

        public void SetFileProperties(IFileProperties properties)
        {
            // not used in this implementation
        }

        #region "start output"
        // start the preview output
        public void StartOfInput()
        {
            _preview = new StreamWriter(OutputProperties.OutputFilePath);
            _preview.WriteLine("<html><body>");
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
        // each paragraph unit should appear in a new line
        // therefore use a DIV element
        public override void ParagraphUnitStart(IParagraphUnitProperties properties)
        {
            _preview.WriteLine("<div>");
        }


        public override void ParagraphUnitEnd()
        {
            _preview.Write("</div>");
        }
        #endregion


        #region "segment"
        // enclose each segment in a SPAN tag pair
        public override void SegmentStart(ISegmentPairProperties properties)
        {
            _preview.Write("<span>");
        }

        public override void SegmentEnd()
        {
            _preview.Write("</span>");
        }
        #endregion

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
    }
}
```

See Also
--

**Other Resources**

[Enhancing the Preview File Writer](enhancing_the_preview_file_writer.md)

>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.