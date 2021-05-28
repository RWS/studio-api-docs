Required References and Namespaces
==

The following chapters contain a number of code snippets, which show you how to perform common TM-related tasks such as creating TMs and adding/searching translation units. These simple code examples will help you quickly get familiar with handling frequent use cases.

To test the code snippets you can create a simple Console Application project in <Var:VisualStudioEdition>. Make sure to add the following references to your project, which you will find in the installation folder of <Var:ProductName>.

* Sdl.Core.Api.dll
* Sdl.Core.Globalization.dll
* Sdl.LanguagePlatform.Core.dll
* Sdl.LanguagePlatform.TranslationMemory.dll
* Sdl.LanguagePlatform.TranslationMemoryApi.dll

Classes that deal with server TM-related tasks such as connecting to a TM Server require the following additional libraries:

* Sdl.Enterprise2.Platform.Client.dll
* Sdl.Enterprise2.Platform.Contracts.dll
* Sdl.LanguagePlatform.ServerBasedTranslationMemory.Contracts.dll

Each of the following chapters assume that you implement a dedicated class for performing particular tasks. Each class will contain one or two functions. Each class needs to use the following namespaces:

* System.Globalization
* Sdl.Core.Globalization
* Sdl.LanguagePlatform.Core.Tokenization
* Sdl.LanguagePlatform.Core
* Sdl.LanguagePlatform.TranslationMemory
* Sdl.LanguagePlatform.TranslationMemoryApi

Classes that deal with server TM-related tasks such as connecting to a TM Server, creating a server-based TM, etc. additionally need to use the following namespaces:

* Sdl.Enterprise2.Platform.Client.IdentityModel
* Sdl.Enterprise2.Platform.Client
* Sdl.Enterprise2.Platform.Contracts

>[!NOTE]
>
>At the moment you need to set the output path to the folder in which <Var:ProductName> is installed. This may change soon, but at the time of writing this was still a requirement. Choosing a different build output path might cause problems with a number of API functions.

