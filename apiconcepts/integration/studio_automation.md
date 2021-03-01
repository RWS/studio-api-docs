Introduction
====
The Trados Studio Integration API enables third party developers to extend, customize and integrate their own functionalities inside Studio application.

Make sure the following references are added to your project, you will find them in the installation folder Trados Studio.

* Sdl.Desktop.IntegrationApi.dll
* Sdl.Desktop.IntegrationApi.Extensions.dll
* Sdl.TranslationStudioAutomation.IntegrationApi.dll
* Sdl.TranslationStudioAutomation.IntegrationApi.Extensions.dll
* Sdl.FileTypeSupport.Framework.Core
* Sdl.FileTypeSupport.Framework.Implementation
* Sdl.ProjectAutomation.Core
* Sdl.ProjectAutomation.FileBased
* Sdl.ProjectAutomation.Settings

> [!NOTE]
> As build output path for your implementations please choose the *C:\Users\[username]\AppData\Roaming\SDL\SDL Trados Studio\[StudioVersionNumber]\Plugins\Packages*
> 
Also check that your library references are pointing to the Trados Studio folder. e.g. *C:\Program Files\SDL\SDL Trados Studio\Studio[StudioVersionNumber]*.

For more informations regarding how to build and deploy a Studio plug-in see (Building a plug-in and Plug-in deployment)

Sign and use Strong-Named Assemblies to enable the loading of your plug-ins inside the Trados Studio.

How to: Sign an Assembly with a Strong Name

Choosing a different build output path or not signing your assembly will prevent your plugin to be loaded.