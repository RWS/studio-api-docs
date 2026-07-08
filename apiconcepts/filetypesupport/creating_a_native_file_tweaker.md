# Creating a Native File Tweaker

Create native file tweakers that modify the original file before extraction (pre-tweaker) or the output file after generation (post-tweaker). See [Native File Tweakers](native_file_tweakers.md) for more information.

Native file tweakers use the following namespaces:

- [Sdl.FileTypeSupport.Framework.NativeApi](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.yml)
- [Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.yml)

Add references to these assemblies when developing native file tweakers.

# [C#](#tab/tabid-1)
```cs
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi;
```

## Creating a Native File Pre-Tweaker

Develop a simple native file pre-tweaker that removes the byte order mark from the original file before extraction.

Pre-tweakers inherit from [AbstractFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml).

# [C#](#tab/tabid-2)
```cs
public class SimpleFilePreTweaker : AbstractFilePreTweaker
```

### Key Properties

The `properties` parameter contains two important properties:

- [OriginalFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalFilePath) — The file path of the original native file
- [InputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_InputFilePath) — The file path of the tweaked native file

### The Tweak Method

The [Tweak](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePreTweaker_Tweak_Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_) method performs the tweaking. Read the original native file from `OriginalFilePath`, process it, and write the tweaked file to `InputFilePath`. The framework handles deletion of the tweaked file after processing.

This example removes the byte order mark with these steps:

1. Open the original file using `OriginalFilePath`
2. Create the tweaked file with UTF-8 encoding without the byte order mark using `InputFilePath`
3. Copy lines from the original file to the tweaked file

Add the following code to the [Tweak](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePreTweaker_Tweak_Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_) method:

# [C#](#tab/tabid-3)
```cs
// open original file
using (StreamReader reader = new StreamReader(properties.OriginalFilePath))
{
    // create tweaked file without byte order mark
    using (StreamWriter writer = new StreamWriter(properties.InputFilePath, false, new UTF8Encoding(false)))
    {
        // copy lines from original file to tweaked file
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            writer.WriteLine(line);
        }
    }
}
```

### The WillFileBeTweaked Method

Override the [WillFileBeTweaked](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePreTweaker_WillFileBeTweaked_System_String_) method to optimize performance. Return `false` when the file doesn't need tweaking, preventing unnecessary file operations.

# [C#](#tab/tabid-4)
```cs
protected override bool WillFileBeTweaked(string originalFilePath)
```

In this example, the file only needs tweaking if it has a byte order mark. Add this code to the method:

# [C#](#tab/tabid-5)
```cs
// if original file has a byte order mark then the original file needs to be tweaked
return HasByteOrderMark(originalFilePath);
```

Add the helper method that checks for a byte order mark to the `SimpleFilePreTweaker` class:

# [C#](#tab/tabid-6)
```cs
private bool HasByteOrderMark(string filePath)
{
    // simple utf-8 byte order mark detection

    // open file
    using (FileStream stream = new FileStream(filePath, FileMode.Open))
    {
        // check each byte order mark in UTF8
        foreach (byte byteOrderMark in UTF8Encoding.UTF8.GetPreamble())
        {
            // if read byte different from byte order mark
            if (stream.ReadByte() != byteOrderMark)
            {
                // file does not have byte order mark
                return false;
            }
        }

        // file has byte order mark
        return true;
    }
}
```

This completes the native file pre-tweaker that removes the byte order mark from the original file.

### Parser Implementation Implications

Pre-tweakers read from `OriginalFilePath` and write to `InputFilePath`. This affects how parsers should read files: parsers must read from `InputFilePath`, not `OriginalFilePath`. This allows parsers to work with tweaked files. If no tweaker exists or the tweaker determines the file doesn't need tweaking, the framework sets `InputFilePath` to `OriginalFilePath`.

## Creating a Native File Post-Tweaker

Develop a simple native file post-tweaker that adds the byte order mark to the output file after generation. Post-tweakers are similar to pre-tweakers and inherit from [AbstractFilePostTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePostTweaker.yml).

Override the [Tweak](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePostTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePostTweaker_Tweak_Sdl_FileTypeSupport_Framework_NativeApi_INativeOutputFileProperties_) method in your post-tweaker.

### Key Differences from Pre-Tweaker

The `properties` parameter contains:

- [OutputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputFileProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeOutputFileProperties_OutputFilePath) — The file path of the output native file

Key differences from pre-tweakers:

- Pre-tweakers read from `OriginalFilePath` and write to `InputFilePath`; post-tweakers read from and write to `OutputFilePath`
- Post-tweakers don't have the `WillFileBeTweaked` method

### Implementing the Post-Tweaker Tweak Method

This example adds the byte order mark with these steps:

1. Read the output file from `OutputFilePath`
2. Create a tweaked file with UTF-8 encoding with the byte order mark using a temporary file
3. Copy lines from the original file to the tweaked file
4. Replace the original file with the tweaked file

Here's the complete native file post-tweaker that adds the byte order mark:

# [C#](#tab/tabid-7)
```cs
using System.IO;
using System.Text;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi;

namespace Sdk.FileTypeSupport.Samples.SimpleText
{
    public class SimpleFilePostTweaker : AbstractFilePostTweaker
    {
        protected override void Tweak(INativeOutputFileProperties properties)
        {
            string originalOutputFilePath = properties.OutputFilePath;
            string tweakedOutputFilePath = Path.GetTempFileName();

            // open original output file
            using (StreamReader reader = new StreamReader(originalOutputFilePath))
            {
                // create tweaked output file with byte order mark
                using (StreamWriter writer = new StreamWriter(tweakedOutputFilePath, false, new UTF8Encoding(true)))
                {
                    // copy lines from original output file to tweaked output file
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        writer.WriteLine(line);
                    }
                }
            }

            // replace original output file with tweaked output file

            // delete original output file
            File.Delete(originalOutputFilePath);

            // move tweaked output file to original output file path
            File.Move(tweakedOutputFilePath, originalOutputFilePath);
        }
    }
}
```

## See Also

- [Native File Tweakers](native_file_tweakers.md)

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
