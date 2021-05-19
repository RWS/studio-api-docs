Locking Specific Strings
==

In this chapter you will learn how to protect text against editing by locking strings.

Locking Particular Strings During Parsing
--

Sometimes you may want to create segments, text selections, or paragraph units that are locked, i.e. whose content users cannot edit. This might be the case, for example, if a paragraph unit contains script code, which should not be altered or accidentally modified by the translator. However, translators should be able to view such strings, as they may contain relevant information, which assists during the translation process.

Let us assume that our text files contain single lines that start with a product code prefix, e.g. *Prd-Code NCC1504*. If these strings must not be localized, you can lock them during parsing of the file.

To do this enhance your file parser by adding a`` WriteLockedContent()`` helper function:

```cs
// protect text from being altered during translation 
// by locking it
private void WriteLockedContent(string LockedContent)
{
    //create opening tag for locked content
    ILockedContentProperties Lockedprops = PropertiesFactory.CreateLockedContentProperties(LockTypeFlags.Manual);
    Output.LockedContentStart(Lockedprops);

    //create text inside of locked content
    ITextProperties textProps = PropertiesFactory.CreateTextProperties(LockedContent);
    Output.Text(textProps);

    //close locked content
    Output.LockedContentEnd();
}
```

Here, the [CreateLockedContent](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IDocumentItemFactory.yml#Sdl_FileTypeSupport_Framework_BilingualApi_IDocumentItemFactory_CreateLockedContent_Sdl_FileTypeSupport_Framework_NativeApi_ILockedContentProperties_) method is used to create a mark-up container that can wrap all the other containers and mark-up of a target paragraph in a paragraph unit. However, you must first create an [ILockedContentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ILockedContentProperties.yml) object by calling [CreateLockedContentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPropertiesFactory.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPropertiesFactory_CreateLockedContentProperties_Sdl_FileTypeSupport_Framework_NativeApi_LockTypeFlags_) with a parameter of [LockTypeFlags](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.LockTypeFlags.yml). Then, the resulting locked content properties can be passed to the [CreateLockedContentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPropertiesFactory.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPropertiesFactory_CreateLockedContentProperties_Sdl_FileTypeSupport_Framework_NativeApi_LockTypeFlags_) method. Once an object derived from [ILockedContentProperties](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ILockedContentProperties.yml) object has been created, it can be used to wrap all the mark-up in the target paragraph of the paragraph unit.

All you need to do in our example is to extend the ```ProcessLine()``` helper function to call ```WriteLockedContent()``` whenever a line that starts with a product code prefix has been found:

```cs
// determines whether a given line is
// translatable or not
// if not, a structure tag is output
// otherwise, the translatable text is exposed
private void ProcessLine(string sLine)
{
    if (sLine.StartsWith("[") && sLine.EndsWith("]"))
    {
        WriteStructureTag(sLine);
        WriteContext(sLine);
    }
    else if (sLine.StartsWith("Prd-Code"))
    {
        WriteLockedContent(sLine);

    } 
    else
    {
        WriteText(ProcessFormatting(sLine));
    }
}
```

After making this enhancement to the file parser, the lines that contain product code strings will be locked as shown below:

![LockedContent](images/LockedContent.jpg)

Note that you may not be able to see the locked units in our example. If you do not see the locked content activate the **Display Filter** toolbar in <Var:ProductName> through the menu command **View** > **Toolbars** > **Display Filter**. Then choose **All content** from the **Display** dropdown list.

Putting it All Together
--

Your enhanced file parser should now look as shown below:


```cs
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.FileTypeSupport.Framework.Formatting;
using Sdl.FileTypeSupport.Framework.Core.Utilities.Formatting;

namespace Sdl.Sdk.Snippets.Native
{
    public class SimpleTextParser3 : AbstractNativeFileParser, INativeContentCycleAware
    {
        #region "global settings"
        IPersistentFileConversionProperties _fileConversionProperties;
        StreamReader _reader = null;

        FormattingGroup _fBold;
        #endregion

        #region "INativeContentCycleAware members"
        // through the properties object you can retrieve important information
        // on the native input file such as the original file name and path
        public void SetFileProperties(IFileProperties properties)
        {
            _fileConversionProperties = properties.FileConversionProperties;
        }

        public void StartOfInput()
        {

        }


        public void EndOfInput()
        {

        }
        #endregion

        #region "members of AbstractNativeFileParser"

        #region "before parsing"
        protected override void BeforeParsing()
        {
            // set progress reporter to the beginning
            OnProgress(0);

            // open the native input file for reading
            _reader = new StreamReader(_fileConversionProperties.OriginalFilePath);
        }
        #endregion

        #region "during parsing"
        protected override bool DuringParsing()
        {
            // iterate through all lines in the input file
            while (!_reader.EndOfStream)
            {
                ProcessLine(_reader.ReadLine());
            }
            return false;
        }
        #endregion

        #region "after parsing"
        protected override void AfterParsing()
        {
            //close original file
            _reader.Close();
            _reader.Dispose();
            _reader = null;
            //set progres report to 100%
            OnProgress(100);
        }
        #endregion

        #endregion

        #region "process line"
        // determines whether a given line is
        // translatable or not
        // if not, a structure tag is output
        // otherwise, the translatable text is exposed
        private void ProcessLine(string sLine)
        {
            if (sLine.StartsWith("[") && sLine.EndsWith("]"))
            {
                WriteStructureTag(sLine);
                WriteContext(sLine);
            }
            else if (sLine.StartsWith("Prd-Code"))
            {
                WriteLockedContent(sLine);

            } 
            else
            {
                WriteText(ProcessFormatting(sLine));
            }
        }
        #endregion

        #region "text"
        // output translatable text
        private void WriteText(string TextContent)
        {
            ITextProperties textProperties = PropertiesFactory.CreateTextProperties(TextContent);
            Output.Text(textProperties);
        }
        #endregion

        #region "lock"
        // protect text from being altered during translation 
        // by locking it
        private void WriteLockedContent(string LockedContent)
        {
            //create opening tag for locked content
            ILockedContentProperties Lockedprops = PropertiesFactory.CreateLockedContentProperties(LockTypeFlags.Manual);
            Output.LockedContentStart(Lockedprops);

            //create text inside of locked content
            ITextProperties textProps = PropertiesFactory.CreateTextProperties(LockedContent);
            Output.Text(textProps);

            //close locked content
            Output.LockedContentEnd();
        }
        #endregion

        #region "structure"
        // output non-translatable text as structure tag
        private void WriteStructureTag(string TagContent)
        {
            IStructureTagProperties structureTagProperties = PropertiesFactory.CreateStructureTagProperties(TagContent);
            structureTagProperties.DisplayText = TagContent;
            Output.StructureTag(structureTagProperties);
        }
        #endregion

        #region "context"
        // output context information, not required, but useful
        // information for the translator
        private void WriteContext(string ContextContent)
        {
            IContextProperties contextProperties = PropertiesFactory.CreateContextProperties();
            IContextInfo contextInfo = PropertiesFactory.CreateContextInfo(ContextContent);
            contextInfo.DisplayCode = "EL";
            contextInfo.DisplayName = "Element";
            contextInfo.Description = ContextContent;
            contextInfo.DisplayColor = Color.Beige;
            contextProperties.Contexts.Add(contextInfo);
            Output.ChangeContext(contextProperties);
        }
        #endregion

        #region "process formatting"
        // this function uses regular expressions to identify
        // what is 'normal' translatable content and which strings
        // need to be marked up as inline tags, e.g. <b>
        private string ProcessFormatting(string sLine)
        {
            int LastPosition = 0;
            // search for opening and closing <b> tags
            Regex rx = new Regex(@"\<.*?\>", RegexOptions.Compiled);
            MatchCollection rxMatches = rx.Matches(sLine);

            foreach (Match rxMatch in rxMatches)
            {
                if (LastPosition != rxMatch.Index)
                {
                    WriteText(sLine.Substring(LastPosition, rxMatch.Index - LastPosition));
                }

                bool IsOpening = rxMatch.Value.Contains("/") ? false : true;
                WriteInlineTag(rxMatch.Value, IsOpening);

                LastPosition = rxMatch.Index + rxMatch.Length;
            }
            return sLine.Substring(LastPosition, sLine.Length - LastPosition);
        }
        #endregion

        #region "write inline tag"
        // this function outputs an opening or a closing <b> tag
        // and applies bold character formatting to the strings
        // that the tags enclose
        private void WriteInlineTag(string tagContent, bool isStart)
        {
            _fBold = new FormattingGroup();
            _fBold.Add(new Bold(true));

            if (isStart)
            {
                IStartTagProperties startTag = PropertiesFactory.CreateStartTagProperties(tagContent);
                startTag.DisplayText = "b";
                startTag.TagContent = tagContent;
                startTag.Formatting = _fBold;
                startTag.CanHide = true;
                Output.InlineStartTag(startTag);
            }
            else
            {
                IEndTagProperties endTag = PropertiesFactory.CreateEndTagProperties(tagContent);
                endTag.DisplayText = "b";
                endTag.TagContent = tagContent;
                endTag.CanHide = true;
                Output.InlineEndTag(endTag);
            }
        }
        #endregion
    }
}
```


>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.