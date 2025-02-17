The Main Writer
==

An example of a simplified Main Writer which accepts sub-content from the framework - note the implementation of [ISubContentAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISubContentAware.yml)

# [C#](#tab/tabid-1)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.FileTypeSupport.Framework.NativeApi;
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Sdk.Snippets.Native
{
    public class MainWriter : AbstractNativeFileWriter, INativeContentCycleAware, ISubContentAware
    {
        private StreamWriter _writer;
        private IFileProperties _properties;
        public void EndOfInput()
        {
            _writer.Close();
        }

        public void SetFileProperties(IFileProperties properties)
        {
            _properties = properties;
        }

        public void StartOfInput()
        {
            _writer = new StreamWriter(OutputProperties.OutputFilePath);
        }

        public override void Text(ITextProperties textInfo)
        {
            _writer.Write(textInfo.Text);
        }

        public override void StructureTag(IStructureTagProperties tagInfo)
        {
            _writer.Write(tagInfo.TagContent);
        }

        public override void InlineStartTag(IStartTagProperties tagInfo)
        {
            _writer.Write(tagInfo.TagContent);
        }

        public override void InlineEndTag(IEndTagProperties tagInfo)
        {
            _writer.Write(tagInfo.TagContent);
        }

        public override void InlinePlaceholderTag(IPlaceholderTagProperties tagInfo)
        {
            _writer.Write(tagInfo.TagContent);
        }

        public void AddSubContent(Stream subContentStream)
        {
            _writer.Write(subContentStream);
        }
    }
}
```
***

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
