Introduction
===

Apart from the classes that you implemented in the previous chapters there are other (optional) components that you may add to your file type plug-in in order to extend its functionality.

Implementing Extended Functionality
--

The minimum requirement for a file type plug-in component is, of course, to read a file and convert it into a bilingual format (extraction) and to convert the target content back to the native file format (generation).

There are a number of features to add to your file type plug-in to either make life easier for translators by allowing them to quickly insert frequently used elements (QuickInsert). However, most importantly, your file type plug-in may require certain settings to meet the requirements of particular native file formats.

For example, Microsoft PowerPoint documents can contain speaker notes, which may be translatable or not. This is why the standard PPT filter that is delivered with Var:ProductName implements a setting that allows you to decide on a case-by-case basis whether to expose speaker note content for translation or not. In the following chapters you will learn how to add such extended functionality to your file type plug-in.

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
