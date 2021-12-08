Create a New Project
===

In this chapter you will learn how to properly set up a project for developing a verification plug-in that works on the bilingual (SDLXliff) file format.

Create the Project
--

After launching <Var:VisualStudioEdition> choose to create a new <Var:ProductName> Plug-in Project, and give it an appropriate name, e.g. Sdl.Sdk.FileTypeSupport.Samples.WordArtVerifier. The instructions for creating a <Var:ProductName> Plug-in Project are described in the [**Building a Plug-in**](\..\\..\articles\gettingstarted\building_a_plugin.md) article under the **Getting Started** section.

A <Var:ProductName> Plug-in Project produces a Plug-in Package (* *.sdlplugin*). This needs to be manually deployed or copied to the <Var:ProductName> Plug-in Packages directory so that <Var:ProductName> can use the plug-in. See the [**Plug-in Deployment**](\..\\..\articles\gettingstarted\plugin_deployment.md) article under the **Getting Started** section.

Add the Required References
--

Next add the references from the File Type Support Framework APIs. These are contained in the following assemblies:

* **Sdl.FileTypeSupport.Framework.Core.dll**: This is the main reference to the File Type Support Framework API
* **Sdl.FileTypeSupport.Framework.Core.Settings.dll**

Then add the references from the Core APIs.
* **Sdl.Core.Settings.dll**
* **Sdl.Core.PluginFramework.dll**
By default you will find these files in the <Var:ProductName> installation folder, e.g. *<var:InstallationFolder>*. The **Copy Local** property for these references should be set to True.

![BilingualVerifierReferences](images/BilingualVerifierReferences.jpg)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.