Implementing the File Writer
===

In this chapter you will learn how to implement the component used for generating the target-language file in its native format.

Add a File Writer Class
--

While the file parser component (see [Implementing the File Parser](implementing_the_file_parser.md) is used to convert the native source file into a bilingual intermediary (SDLXliff) format, the file writer does the exact opposite, i.e. it generates the target file in its native format. This is what happens when the user, for example, saves the current file as target or finalizes a project.

Start by adding a **SimpleTextWriter.cs** class to your project. The skeleton writer class looks very similar to the file parser class. Here too, you must reference the namespace ```System.IO```, as you need you can generate a target output file. Then add the namespace ```Sdl.FileTypeSupport.Framework.NativeApi```.

Your writer class needs be derived from [AbstractNativeFileWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml) and must implement the [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml) interface. The writer class implements the same members as the parser class. Therefore, the minimum amount of code required to build a file type plug-in with a file writer looks as shown below:

# [C#](#tab/tabid-1)
```cs
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.Snippets.Native
{
    class SimpleTextWriter : AbstractNativeFileWriter, INativeContentCycleAware
    {
        IPersistentFileConversionProperties _conversionProperties;
        StreamWriter _targetFile = null;

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

Remember, if you have writer specific settings, you also need to implement the [InitializeSettings](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml#Sdl_FileTypeSupport_Framework_IntegrationApi_ISettingsAware_InitializeSettings_Sdl_Core_Settings_ISettingsBundle_System_String_) method of the [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml) interface.

Create the Output Text File
--

First, use the [StartOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_StartOfInput) member to apply the logic that creates the native output text file:

# [C#](#tab/tabid-2)
```cs
// create the output text file
public void StartOfInput()
{
    _targetFile = new StreamWriter(OutputProperties.OutputFilePath);
}
```
***

Output Text and Tags
===

The text writer basically traverses the intermediary (SDLXliff) file and outputs the elements it encounters (e.g. text and inline tags) to the target output file. Override the methods provided by the abstract file writer class ([AbstractNativeFileWriter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml)) to output these elements. The methods shown below output translatable text, structure tags, and inline start/end tags.

Through the method [SegmentEnd](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.AbstractNativeFileWriter.yml#Sdl_FileTypeSupport_Framework_NativeApi_AbstractNativeFileWriter_SegmentEnd) you determine what should be done after you have written a segment to the native output file. Our simple text format just requires a line break after each segment, which we output using ```WriteLine()```.

# [C#](#tab/tabid-3)
```cs
// iterate through the bilingual file
// and add translatable text content and the content of
// any structure and inline tags to the target output file
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

// make sure a line break is inserted after each end of a segment
public override void SegmentEnd()
{
    _targetFile.WriteLine();
}
```
***

At the end we close the output file object and dispose of it in the [EndOfInput](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeContentCycleAware_EndOfInput) member of the [INativeContentCycleAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeContentCycleAware.yml) interface:

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

Putting Everything Together
--

The complete file writer class should now look as shown below:

# [C#](#tab/tabid-5)
```cs
using System.IO;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.FileTypeSupport.Samples.SimpleText
{
    class SimpleTextWriter : AbstractNativeFileWriter, INativeContentCycleAware
    {
        IPersistentFileConversionProperties _conversionProperties;
        StreamWriter _targetFile = null;

        #region "members of INativecontentCycleAware member"
        public void SetFileProperties(IFileProperties properties)
        {
            _conversionProperties = properties.FileConversionProperties;
        }

        #region "output file"
        // create the output text file
        public void StartOfInput()
        {
            _targetFile = new StreamWriter(OutputProperties.OutputFilePath);
        }
        #endregion

        #region "close"
        public void EndOfInput()
        {
            _targetFile.Close();
            _targetFile.Dispose();
            _targetFile = null;
        }
        #endregion
        #endregion

        #region "text and tags"
        // iterate through the bilingual file
        // and add translatable text content and the content of
        // any structure and inline tags to the target output file
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

        // make sure a line break is inserted after each end of a segment
        public override void SegmentEnd()
        {
            _targetFile.WriteLine();
        }
        #endregion
    }
}
```
***

Reference the Component in the File Type Component Builder
--

Do not forget to reference the file writer class by adding the following code to the [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) implementation:

# [C#](#tab/tabid-6)
```cs
/// <summary>
/// Gets the file generator for this component.
/// </summary>
/// <param name="name">not used herer</param>
/// <returns><c>Null</c> if no file generator is defined</returns>
public virtual IFileGenerator BuildFileGenerator(string name)
{
    var writer = new SimpleTextWriter();
    var generator = FileTypeManager.BuildFileGenerator(FileTypeManager.BuildNativeGenerator(writer));
    generator.AddFileTweaker(new SimpleFilePostTweaker());
    return generator;
}
```
***

See Also
--



[Implementing the Preview Writer](implementing_the_preview_writer.md)

