The Main Parser
==
An example of a simplified Main Parser which publishes sub-content. 

>**Note**
>
>Usually, you should not add dependency files to the FileProperties unless they are actually used by the sub-content writer - note the implementation of [ISubContentPublisher](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ISubContentPublisher.yml)

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.FileTypeSupport.Framework.NativeApi;
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;

namespace Sdl.Sdk.Snippets.Native
{
    public class MainParser : AbstractNativeFileParser, INativeContentCycleAware, ISubContentPublisher
    {
        public void EndOfInput()
        {

        }

        public void SetFileProperties(IFileProperties properties)
        {

        }

        public void StartOfInput()
        {

        }

        protected override bool DuringParsing()
        {
            bool hasMoreContent = true;

            // In this example, we are using a hardcoded string to demonstrate the sub-content
            // However, in a real parser, this would be derived from the parsing implementation
            var subContentText = "Sample Embedded Content Text";
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (TextWriter textWriter = new StreamWriter(memoryStream))
                {
                    textWriter.Write(subContentText);
                    textWriter.Flush();
                    PublishSubContent(memoryStream);
                }
            }

            return hasMoreContent;
        }

        public event EventHandler<ProcessSubContentEventArgs> ProcessSubContent;

        private void PublishSubContent(Stream subContentStream)
        {
            if (ProcessSubContent != null)
            {
                // In this example, we have hardcoded the sub-content processor ID, however, in a real filter
                // we may wish to use the settings UI to define which of the available processors to use.
                // We would store this information in a specific Settings Object.
                ProcessSubContentEventArgs args = new ProcessSubContentEventArgs("Simple Text Embedded Content 1.0.0.0", subContentStream);
                ProcessSubContent(this, args);
            }
        }
    }
}
```
>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.