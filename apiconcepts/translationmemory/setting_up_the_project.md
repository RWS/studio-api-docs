Setting up the Project
======
In this chapter you will learn how to develop a simple command-line application for batch exporting numerous *.sdltm files in <var:VisualStudioEdition> using C#.

Start by creating a new Console Application called Sdl.**SDK.LanguagePlatform.Samples.BatchExport**.


<img style="display:block; " src="images/BatchExportProject.jpg"/>

The project requires the following references:

* Sdl.Core.Api
* Sdl.LanguagePlatform.Core
* Sdl.LanguagePlatform.TranslationMemory
* Sdl.LanguagePlatform.TranslationMemoryApi


<img style="display:block; " src="images/BatchExportProjectReferences.jpg"/>

The classes in your project should use the following namespaces:

* System.IO
* System.Globalization
* Sdl.LanguagePlatform.Core
* Sdl.LanguagePlatform.Core.Tokenization
* Sdl.LanguagePlatform.TranslationMemory
* Sdl.LanguagePlatform.TranslationMemoryApi

> [!NOTE]
> 
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.

See Also
--
[Looping through the Folder(s)](looping_through_the_folder.md)

[Exporting to TMX](exporting_to_tmx.md)