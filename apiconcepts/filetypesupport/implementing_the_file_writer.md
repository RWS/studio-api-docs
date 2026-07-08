# Implementing the File Writer

This section explains how to implement the component that generates the target-language file in its native format.

## Add a file writer class

The file parser converts the native source file into a bilingual intermediary SDLXliff file. The file writer performs the reverse operation and generates the target file in its native format. This happens, for example, when users save the current file as target or finalize a project.

Start by adding a **SimpleTextWriter.cs** class to your project. The skeleton writer class closely resembles the file parser class. Add the `System.IO` namespace because the writer creates a target output file. Then add `Sdl.FileTypeSupport.Framework.NativeApi`.

Derive your writer class from [AbstractNativeFileWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml) and implement [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml). The writer class implements the same members as the parser class. The following example shows the minimum code required to build a file type plug-in with a file writer:

# [C#](#tab/tabid-1)
```cs
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.Snippets.Native
{
    class SimpleTextWriter : AbstractNativeFileWriter, INativeContentCycleAware
    {
        private IPersistentFileConversionProperties _conversionProperties;
        private StreamWriter _targetFile = null;

        public void SetFileProperties(IFileProperties properties)
        {
            _conversionProperties = properties.FileConversionProperties;
        }

        public void EndOfInput()
        {

        }

        public void StartOfInput()
        {

        }
    }
}
```
***

If your writer uses custom settings, also implement [InitializeSettings](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_ISettingsAware_InitializeSettings_Sdl_Core_Settings_ISettingsBundle_System_String_) from [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml).

## Create the output text file

Use [StartOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_StartOfInput) to create the native output text file:

# [C#](#tab/tabid-2)
```cs
// Create the output text file.
public void StartOfInput()
{
    _targetFile = new StreamWriter(OutputProperties.OutputFilePath);
}
```
***

## Output text and tags

The text writer traverses the intermediary SDLXliff file and writes the elements it encounters, such as text and inline tags, to the target output file. Override the methods that [AbstractNativeFileWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml) provides to output these elements. The following methods output translatable text, structure tags, and inline start and end tags.

Use [SegmentEnd](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileWriter_SegmentEnd) to define what happens after the writer outputs a segment to the native file. This simple text format only requires a line break after each segment, so call `WriteLine()`.

# [C#](#tab/tabid-3)
```cs
// Iterate through the bilingual file and write translatable text,
// structure tags, and inline tags to the target output file.
public override void StructureTag(IStructureTagProperties tagInfo)
{
    _targetFile.WriteLine(tagInfo.TagContent);
}

public override void Text(ITextProperties textInfo)
{
    _targetFile.Write(textInfo.Text);
}
public override void InlineStartTag(IStartTagProperties tagInfo)
{
    _targetFile.Write(tagInfo.TagContent);
}

public override void InlineEndTag(IEndTagProperties tagInfo)
{
    _targetFile.Write(tagInfo.TagContent);
}

// Insert a line break after each segment.
public override void SegmentEnd()
{
    _targetFile.WriteLine();
}
```
***

At the end of processing, close and dispose the output file object in [EndOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_EndOfInput) from [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml):

# [C#](#tab/tabid-4)
```cs
public void EndOfInput()
{
    _targetFile.Close();
    _targetFile.Dispose();
    _targetFile = null;
}
```
***

## Put everything together

The complete file writer class should now look like this:

# [C#](#tab/tabid-5)
```cs
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.FileTypeSupport.Samples.SimpleText
{
    class SimpleTextWriter : AbstractNativeFileWriter, INativeContentCycleAware
    {
        private IPersistentFileConversionProperties _conversionProperties;
        private StreamWriter _targetFile = null;

        public void SetFileProperties(IFileProperties properties)
        {
            _conversionProperties = properties.FileConversionProperties;
        }

        // Create the output text file.
        public void StartOfInput()
        {
            _targetFile = new StreamWriter(OutputProperties.OutputFilePath);
        }

        public void EndOfInput()
        {
            _targetFile.Close();
            _targetFile.Dispose();
            _targetFile = null;
        }

        // Iterate through the bilingual file and write translatable text,
        // structure tags, and inline tags to the target output file.
        public override void StructureTag(IStructureTagProperties tagInfo)
        {
            _targetFile.WriteLine(tagInfo.TagContent);
        }

        public override void Text(ITextProperties textInfo)
        {
            _targetFile.Write(textInfo.Text);
        }
        public override void InlineStartTag(IStartTagProperties tagInfo)
        {
            _targetFile.Write(tagInfo.TagContent);
        }

        public override void InlineEndTag(IEndTagProperties tagInfo)
        {
            _targetFile.Write(tagInfo.TagContent);
        }

        // Insert a line break after each segment.
        public override void SegmentEnd()
        {
            _targetFile.WriteLine();
        }
    }
}
```
***

## Reference the component in the File Type Component Builder

Reference the file writer class by adding the following code to your [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) implementation:

# [C#](#tab/tabid-6)
```cs
/// <summary>
/// Gets the file generator for this component.
/// </summary>
/// <param name="name">Not used here.</param>
/// <returns>A file generator for the writer.</returns>
public virtual IFileGenerator BuildFileGenerator(string name)
{
    var writer = new SimpleTextWriter();
    var generator = FileTypeManager.BuildFileGenerator(FileTypeManager.BuildNativeGenerator(writer));
    generator.AddFileTweaker(new SimpleFilePostTweaker());
    return generator;
}
```
***

## See also

- [Implementing the Preview Writer](implementing_the_preview_writer.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.