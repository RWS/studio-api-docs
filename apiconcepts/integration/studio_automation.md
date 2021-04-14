Introduction
====
The <Var:ProductName> Integration API enables third-party developers to extend, customize and integrate their own functionalities inside the <Var:ProductName> application.

Make sure the following references are added to your project, you will find them in the installation folder <Var:ProductName>.

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
> 
> As build output path for your implementations please choose the *<var:PluginPackedPath>*.
> 
> Also check that your library references are pointing to the <Var:ProductName> folder. e.g. *<var:InstallationFolder>*.
>
> For more information on how to build and deploy a Studio plug-in, see (Building a plug-in and Plug-in deployment)
>
> Sign and use Strong-Named Assemblies to enable the loading of your plug-ins inside the <Var:ProductName>.
> 
> [How to: Sign an Assembly with a Strong Name](https://docs.microsoft.com/en-us/dotnet/standard/assembly/sign-strong-name?redirectedfrom=MSDN)
>
> Choosing a different build output path or not signing your assembly will prevent your plugin from loading.