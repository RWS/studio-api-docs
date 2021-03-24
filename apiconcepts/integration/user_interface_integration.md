Introduction
====
The Trados Studio Integration API enables third-party developers to extend and integrate their own UI functionalities inside the Trados Studio application.

Make sure the following references are added to your project, you will find them in the installation folder of Trados Studio.

* Sdl.Desktop.IntegrationApi.dll
* Sdl.Desktop.IntegrationApi.Extensions.dll
* Sdl.TranslationStudioAutomation.IntegrationApi.dll
* Sdl.TranslationStudioAutomation.IntegrationApi.Extensions.dll

> [!NOTE]
> As build output path for your implementations please choose the *C:\Users\[username]\AppData\Roaming\SDL\SDL Trados Studio\[StudioVersionNumber]\Plugins\Packages*.

Also check that your library references are pointing to the Trados Studio folder. e.g. *C:\Program Files\SDL\SDL Trados Studio\Studio[StudioVersionNumber]*.

For more information on building an deploying a Trados Studio plug-in, see (Building a plug-in and Plug-in deployment).

Sign and use Strong-Named Assemblies to enable the loading of your plug-ins inside the Trados Studio.

[How to: Sign an Assembly with a Strong Name](https://docs.microsoft.com/en-us/dotnet/standard/assembly/sign-strong-name?redirectedfrom=MSDN)

Choosing a different build output path or not signing your assembly will prevent your plugin from loading.

Sdl.Desktop.IntegrationApi
-----
Sdl.Desktop.IntegrationApi is the main namespace which enables the integration of new Studio UI functionalities.

Sdl.TranslationStudioAutomation.IntegrationApi
------

Sdl.TranslationStudioAutomation.IntegrationApi is the main namespace which enables the integration inside the Trados Studio application.