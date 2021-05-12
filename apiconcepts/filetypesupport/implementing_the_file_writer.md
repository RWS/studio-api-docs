Implementing the File Writer
==

In this chapter you will learn how to implement the component used for generating the target-language file in its native format.

Add a File Writer Class
--

While the file parser component (see [Implementing the File Parser](implementing_the_file_parser.md) is used to convert the native source file into a bilingual intermediary (SDL XLIFF) format, the file writer does the exact opposite, i.e. it generates the target file in its native format. This is what happens when the user, for example, saves the current file as target or finalizes a project.

Start by adding a SimpleTextWriter.cs class to your project. The skeleton writer class looks very similar to the file parser class. Here too, you must reference the namespace System.IO, as you need you can generate a target output file. Then add the namespace Sdl.FileTypeSupport.Framework.NativeApi.

Your writer class needs be derived from AbstractNativeFileWriter and must implement the INativeContentCycleAware interface. The writer class implements the same members as the parser class. Therefore, the minimum amount of code required to build a file type plug-in with a file writer looks as shown below: