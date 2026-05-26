# Required References and Namespaces

The following chapters include code snippets that show how to perform common TM tasks, such as creating translation memories and adding or searching translation units. Use these examples to get familiar with the most common workflows.

To test the snippets, create a simple console application project in Var:VisualStudioEdition. Add these references from the Var:ProductName installation folder:

* Sdl.Core.Globalization.dll
* Sdl.Core.Globalization.Async
* Sdl.LanguagePlatform.Core.dll
* Sdl.LanguagePlatform.TranslationMemory.dll
* Sdl.LanguagePlatform.TranslationMemoryApi.dll

The following chapters assume that you implement a dedicated class for each task. Each class should contain one or two methods and use these namespaces:

* Sdl.Core.Globalization
* Sdl.Core.Globalization.Async
* Sdl.LanguagePlatform.Core.Tokenization
* Sdl.LanguagePlatform.Core
* Sdl.LanguagePlatform.TranslationMemory
* Sdl.LanguagePlatform.TranslationMemoryApi
* Sdl.Core.TM.ImportExport

>[!NOTE]
>
>Set the output path to the folder where Var:ProductName is installed. If you choose a different build output path, some API functions might fail.

