Create a New Project
====

In this chapter you will learn how to properly set up a project for developing a global verification plug-in.

Create the Project
-----
Before you start developing plug-ins for <Var:ProductName>, you should make sure that the SDK is installed on your development computer. The SDK installer will add new templates to your Microsoft Visual Studio environment, as illustrated in the screenshot below. For the type of plug-in discussed in this chapter, we require the **<Var:ProductName> (2021)** project template.

<img style="display:block; " src="images/PlugInTemplate.jpg"/>

By default, when you create a project based on this template, the project name will be e.g. **SDL Trados Studio1**. Change the project name to **Sdl.Verification.Sdk.IdenticalCheck** for our sample implementation.

Add the Required References
-----
The plug-in template will come with the **Sdl.Core.PluginFramework.dll** reference. For our global verifier implementation, we also need to reference the Verification API, i.e. **Sdl.Verification.Api.dll**. For implementing the functionality required by our example, you need to add the libraries used for integration with <Var:ProductName>, which are as follows:

* **Sdl.Desktop.Platform.dll**
* **Sdl.FileTypeSupport.Framework.Core**
* **Sdl.Core.Settings.dll**
  
By default, these files can be found in the installation folder of <Var:ProductName>, i.e. *C:\Program Files\SDL\SDL Trados Studio\ <Var:ProductVersion>*. The 'Copy Local' property for these references should be set to True.

<img style="display:block; " src="images/GlobalVerifierRef.jpg"/>

> [!NOTE]
> Do not forget to sign the assembly. Otherwise, your plug-in might not be loaded by <Var:ProductName>.