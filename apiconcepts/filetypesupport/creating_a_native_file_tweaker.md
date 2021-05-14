Creating a Native File Tweaker
==

In this chapter, you will learn how to create a native file tweaker. A native file tweaker can either tweak - or perform a minor change to - the original file before being extracted (a pre-tweaker) or tweak the output file after being written (a post-tweaker). See [Native File Tweakers](native_file_tweakers.md) for more information.

Native file tweakers make use of the following namespaces:

* [Sdl.FileTypeSupport.Framework.NativeApi](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.yml)
* [Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.yml)

These namespaces should be used and the associated assembly references added when developing native file tweakers.

```cs
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi;
```

Creating a Native File Pre-Tweaker
--

A simple native file pre-tweaker will be developed to remove the byte order mark from the original file before being extracted.

Native file pre-tweakers should inherit from [AbstractFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml).

```cs
public class SimpleFilePreTweaker : AbstractFilePreTweaker
```


The **properties** parameter contains two important properties:

* [OriginalFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalFilePath) is the file path of the original native file.
* [InputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_InputFilePath) is the file path of the tweaked native file.

The [Tweak](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePreTweaker_Tweak_Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_) method is where the native file is tweaked. An implementation of this method should read the original native file, perform some processing, and create and write the tweaked native file to [InputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_InputFilePath). The tweaker is only responsible for creating the tweaked native file; it is not responsible for deleting the tweaked native file after processing.

In this example, the implementation of the Tweak method that removes the byte order mark is quite straightforward and consists of the following steps:

1. Open the original file using [OriginalFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalFilePath) as the file path.
2. Create the tweaked file with UTF-8 encoding without the byte order mark using [InputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_InputFilePath) as the file path.
3. Copy the lines from the original file to the tweaked file.

Add the following code to the [Tweak](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePreTweaker_Tweak_Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_) method.

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

[AbstractFilePreTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml) has a [WillFileBeTweaked](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePreTweaker_WillFileBeTweaked_System_String_) method that may be overridden.

```cs
protected override bool WillFileBeTweaked(string originalFilePath)
```

There may be some circumstances where the original native file does not need to be tweaked and in these circumstances the [WillFileBeTweaked](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePreTweaker_WillFileBeTweaked_System_String_) method should return **false** to prevent the original native file from being tweaked.
Here the original native file only needs to be tweaked if the original native file has a byte order.

Add the following code to the [WillFileBeTweaked](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePreTweaker_WillFileBeTweaked_System_String_) method.

```cs
// if original file has a byte order mark then the original file needs to be tweaked
return HasByteOrderMark(originalFilePath);
```


Add the following method that determines whether a given file has a byte order mark to the **SimpleFilePreTweaker** class.

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


That completes this simple native file pre-tweaker which removes the byte order mark from the original native file.

The way native file pre-tweakers work affects the way parsers should be implemented. Any native file pre-tweaker works by reading the original native file from [OriginalFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalFilePath) and writing a tweaked native file to [InputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_InputFilePath). This has an important consequence for any parsers. Any parser should read from [InputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_InputFilePath) rather than [OriginalFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalFilePath) because this allows the parser to read from tweaked files. If a file has not been tweaked either because a tweaker does not exist or the tweaker reported that the file did not need tweaking then [InputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_InputFilePath) will be set to [OriginalFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalFilePath).


Creating a Native File Post-Tweaker
--

A simple native file post-tweaker will be developed to add the byte order mark to the output file after being written. A native file post-tweaker is similiar to a native file pre-tweaker. Native file post-tweakers should inherit from [AbstractFilePostTweaker](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePostTweaker.yml) which has a [Tweak](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePostTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePostTweaker_Tweak_Sdl_FileTypeSupport_Framework_NativeApi_INativeOutputFileProperties_) method that must be overridden.

The [Tweak](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePostTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePostTweaker_Tweak_Sdl_FileTypeSupport_Framework_NativeApi_INativeOutputFileProperties_) method has a **properties** parameter that contains one property that concerns us here:

* [OutputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputFileProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeOutputFileProperties_OutputFilePath) is the file path of the output native file.

One important difference between a native file pre-tweaker and a native file post-tweaker is that whilst the pre-tweaker reads the original file from [OriginalFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_OriginalFilePath) and writes the tweaked file to [InputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IPersistentFileConversionProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_IPersistentFileConversionProperties_InputFilePath), the post-tweaker reads the original file and writes the tweaked file to [OutputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputFileProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeOutputFileProperties_OutputFilePath) . Another important difference is that the native file post-tweaker does not have the [WillFileBeTweaked](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePreTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePreTweaker_WillFileBeTweaked_System_String_) method.

In this example, the implementation of the [Tweak](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi.AbstractFilePostTweaker.yml#Sdl_FileTypeSupport_Framework_Core_Utilities_NativeApi_AbstractFilePostTweaker_Tweak_Sdl_FileTypeSupport_Framework_NativeApi_INativeOutputFileProperties_) method that adds the byte order mark is quite straightforward and consists of the following steps:

1. Open the original file using the original output file path; [OutputFilePath](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeOutputFileProperties.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeOutputFileProperties_OutputFilePath) .
2. Create the tweaked file with UTF-8 encoding with the byte order mark using a temporary file path.
3. Copy the lines from the original file to the tweaked file.
4. Replace the original file with the tweaked file.

Here is the complete simple native file post-tweaker code which adds the byte order mark to the original native file.

```cs
using System.IO;
using System.Text;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.FileTypeSupport.Framework.Core.Utilities.NativeApi;

namespace Sdl.Sdk.FileTypeSupport.Samples.SimpleText
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

See Also
--

**Other Resources**

[Native File Tweakers](native_file_tweakers.md)

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
