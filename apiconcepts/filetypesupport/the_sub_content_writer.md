# The Sub Content Writer

This example demonstrates a sub-content Writer. Review the [ISubContentWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISubContentWriter.yml) implementation for details.

# [C#](#tab/tabid-1)
```cs
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.Snippets.Native
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
            // This writer ignores the original source content, so we can disregard this input parameter.
        }
    }
}
```
***

>[!NOTE]
>
>This content may be out-of-date. Inspect the libraries in the Visual Studio Object Browser to verify the latest information on this topic.
