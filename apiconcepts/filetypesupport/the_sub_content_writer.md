The Sub Content Writer
==

An example of a sub-content Writer is shown below - note the implementation of [ISubContentWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISubContentWriter.yml)

# [C#](#tab/tabid-1)
```cs
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdl.Sdk.Snippets.Native
{
    class SimpleSubContentTextWriter : AbstractNativeFileWriter, INativeContentCycleAware, ISubContentWriter
    {
        IPersistentFileConversionProperties _conversionProperties;
        StreamWriter _targetFile = null;
        private Stream _subContentStream = new MemoryStream();

        public void SetFileProperties(IFileProperties properties)
        {
            _conversionProperties = properties.FileConversionProperties;
        }

        public void EndOfInput()
        {

        }

        public void StartOfInput()
        {
            _targetFile = new StreamWriter(_subContentStream);
        }

        public override void Text(ITextProperties textInfo)
        {
            _targetFile.Write(textInfo.Text);
        }

        public override void StructureTag(IStructureTagProperties tagInfo)
        {
            _targetFile.Write(tagInfo.TagContent);
        }

        public override void InlineStartTag(IStartTagProperties tagInfo)
        {
            _targetFile.Write(tagInfo.TagContent);
        }

        public override void InlineEndTag(IEndTagProperties tagInfo)
        {
            _targetFile.Write(tagInfo.TagContent);
        }

        public Stream GetSubContentStream()
        {
            return _subContentStream;
        }

        public void InitializeSubContentWriter(Stream originalSubContent) 
        {
            // We don't make use of the original source content in this writer, so we can ignore this input parameter here.
        }
    }
}
```
***

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.