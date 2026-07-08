# Introduction

In addition to the classes from the previous chapters, you can add optional components to extend your file type plug-in.

## Implement extended functionality

At a minimum, a file type plug-in must read a file, convert it to a bilingual format for extraction, and convert the target content back to the native file format for generation.

You can also add features that make translation easier. For example, QuickInsert helps translators insert frequently used elements more quickly. Your plug-in may also need settings that support specific native file formats.

For example, Microsoft PowerPoint documents can contain speaker notes that may or may not require translation. For that reason, the standard PPT filter in Var:ProductName includes a setting that lets you decide whether to expose speaker note content for translation. The following chapters explain how to add this type of extended functionality to your file type plug-in.

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
