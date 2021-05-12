Native File Tweakers
==

Native file tweakers are optional filter components, which allow additional processing to be performed on the input file before parsing begins or after the target file has been generated.

Native file tweakers should only be used in the rare cases, for example when the parsing mechanism does not allow for sufficient low-level access to the file details and it is therefore not possible to round-trip all required information. For example, The XML parser uses the .NET XmlReader, which performs it's own pre-processing on entities. This means that you cannot be certain in which form the entity was present in the original file. A file tweaker would then allow you to encode these entities before parsing so you can be certain of their original format.

Using a Native File Pre-Tweaker
--

The following example demonstrates how to include a pre-tweaker in the extractor of a filter File Type Component Builder:
```cs
/// <summary>
/// Gets the file extractor for this component.
/// </summary>
/// <param name="name">not used here</param>
/// <returns>a FileExtractor containing an Simple Text Parser</returns>
public virtual IFileExtractor BuildFileExtractor(string name)
{
    var parser = new SimpleTextParser();
    parser.LockPrdCodes = true;
    var extractor = this.FileTypeManager.BuildFileExtractor(this.FileTypeManager.BuildNativeExtractor(parser), this);
    extractor.AddFileTweaker(new SimpleFilePreTweaker {RequireValidEncoding = false});
    return extractor;
}
```

(SimpleFilePreTweaker is a class that implements [AbstractFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml))

Using a Native File Post-Tweaker
--

The following example demonstrates how to include a post-tweaker in the generator of a filter File Type Component Builder:

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

(SimpleFilePostTweaker is a class that implements [AbstractFilePostTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePostTweaker.yml))

See Also
--

**Reference**

[AbstractFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml)

[AbstractFilePostTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePostTweaker.yml)

[RegExFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.RegExFilePreTweaker.yml)

[RegExFilePostTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.RegExFilePostTweaker.yml)

**Other Resources**

[Creating a Native File Tweaker](creating_a_native_file_tweaker.md)

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.