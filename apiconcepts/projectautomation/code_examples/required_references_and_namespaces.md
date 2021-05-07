Required References and Namespaces
===
To develop 'against' the Project Automation API your project needs to use the following libraries as references:

Reference Libraries to Add
--

* Sdl.Core.Globalization
* Sdl.Core.Settings
* Sdl.ProjectApi
* Sdl.ProjectAutomation.Core
* Sdl.ProjectAutomation.FileBased
* Sdl.ProjectAutomation.Settings

The API is organized in a way that allows future implementations for other types of projects, like for instance projects hosted on a project server. For this reason, the separate **Sdl.ProjectAutomation.Core** assembly defines a generic IProject interface that represents a generic localization project.

The **Sdl.ProjectAutomation.FileBased** assembly contains the specific implementation of the Project Automation API for file-based SDL Trados Studio 2017 projects, via the FileBasedProject class.

Finally, the **Sdl.ProjectAutomation.Settings** assembly exposes a number of settings groups that provide access to settings which control various aspects of localization project processing.

![References](images/References.jpg)

Namespaces to Use
--
The classes of your project needs to use the following namespaces:

* System.Globalization
* Sdl.Core.Settings
* Sdl.Core.Globalization
* Sdl.ProjectAutomation.Core
* Sdl.ProjectAutomation.FileBased
* Sdl.ProjectAutomation.Settings

Further Requirements
--
In addition to the above, you should observer the following:

* As build output location choose the installation folder of <Var:ProductName> , e.g. *<Var:InstallationFolder>*
* Configure your projects as x84 applications, as otherwise they will not work when running on 64 bit platforms. Therefore, set the platform target of your projects to x84.

References from the Translation Memory API
-- 
For some implementations it may be necessary to leverage the functionality of the Translation Memory API. In fact, this SDK describes the development of a sample application that uses the Project Automation API to run a batch analysis and at the same time requires some of the functionality offered by the TM API to read certain information from a TM setup (see Introduction to the Sample Application).

If you need to use the TM API in your implementation, you require the following references in addition:

* Sdl.LanguagePlatform.Core.dll
* Sdl.LanguagePlatform.TranslationMemory.dll
* Sdl.LanguagePlatform.TranslationMemoryApi.dll

Your class needs to use the following namespaces:

* Sdl.LanguagePlatform.TranslationMemoryApi
* Sdl.LanguagePlatform.TranslationMemory
* Sdl.LanguagePlatform.Core.Tokenization
* Sdl.LanguagePlatform.Core

See Also
--
**Other Resources**

[Setting up a Developer Machine](../../../articles/gettingstarted/setting_up_a_developer_machine.md)

[Platform Support](../../../articles/gettingstarted/platform_support.md) 

[Threading](../threading_support.md)

[Setting up the Visual Studio Project](../../batchtasks/setting_up_the_visual_studio_project.md)
