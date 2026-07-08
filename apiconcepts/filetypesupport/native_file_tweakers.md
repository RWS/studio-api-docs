# Native File Tweakers

Native file tweakers are optional filter components that perform additional processing on the input file before parsing starts or on the target file after generation completes.

Use native file tweakers only in rare cases. For example, you might need one when the parsing mechanism does not provide enough low-level access to preserve all required information during round-tripping. The .NET `XmlReader`, for example, performs its own preprocessing on entities. As a result, you cannot always determine the exact form of an entity in the original file. A file tweaker can encode those entities before parsing so that you can preserve their original format.

## Using a native file pre-tweaker

The following example shows how to add a pre-tweaker to the extractor in a filter File Type Component Builder:
# [C#](#tab/tabid-1)
```cs
/// <summary>
/// Gets the file extractor for this component.
/// </summary>
/// <param name="name">not used here</param>
/// <returns>a FileExtractor containing a Simple Text Parser</returns>
public virtual IFileExtractor BuildFileExtractor(string name)
{
    var parser = new SimpleTextParser();
    parser.LockPrdCodes = true;
    var extractor = this.FileTypeManager.BuildFileExtractor(this.FileTypeManager.BuildNativeExtractor(parser), this);
    extractor.AddFileTweaker(new SimpleFilePreTweaker {RequireValidEncoding = false});
    return extractor;
}
```
***

`SimpleFilePreTweaker` implements [AbstractFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml).

## Using a native file post-tweaker

The following example shows how to add a post-tweaker to the generator in a filter File Type Component Builder:

# [C#](#tab/tabid-2)
```cs
/// <summary>
/// Gets the file generator for this component.
/// </summary>
/// <param name="name">not used here</param>
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

`SimpleFilePostTweaker` implements [AbstractFilePostTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePostTweaker.yml).

### Reference

- [RegExFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.RegExFilePreTweaker.yml)

- [RegExFilePostTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.RegExFilePostTweaker.yml)

## See also

- [Creating a Native File Tweaker](creating_a_native_file_tweaker.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
