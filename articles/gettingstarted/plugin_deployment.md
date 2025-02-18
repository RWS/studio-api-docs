Plug-in deployment
====

This section describes how to deploy a Plug-in Package (*.sdlplugin) for use in Var:ProductName. It also covers updating to a new version of the plug-in package and uninstalling a plug-in package.


Installing a Plug-in Package
------
Double click the plug-in package and follow the instructions.

> [!NOTE]
>
> During development, you can configure the output path of the project to point to *Var:PluginPackedPath*. This is already configured if you created the project with one of the project templates available in the **Var:ProductName SDK** here. For more information on this, see [Setting up a Developer Machine](setting_up_a_developer_machine.md).

The following warning message will be shown while Var:ProductName starts:
<img style="display:block; " src="images/UnsignedWarning.png" />


To avoid this message from appearing, you need to submit your plug-in package to RWS for verification. Once verified, your plug-in package will be signed by RWS and the warning message will not appear anymore. To verify the plugin, send an email to **Var:AppSigningEmail** with a link from where the plugin can be downloaded for verification. Once the verification is done, you will receive an answer email with a download link, where you will be able to obtain the signed version of the plugin.

Once Var:ProductName has started, go to the **Tools > Plug-ins** dialog and notice that "MyPlugin" is now listed as a plug-in and is ready to be used.

Updating a Plug-in Package
----
Once deployed, you can update your plug-in package by increasing the version in the plug-in package manifest (see [Plug-in deployment](plugin_deployment.md)). Double click the plug-in package and follow the instructions. Once the plug-in is deployed, start Var:ProductName. The application will automatically detect the presence of the updated plug-in package, verify it, extract its contents into <em> Var:PluginUnpackedPath </em> and load it.

It is essential that you increase the version of the plug-in package as listed in the plug-in manifest, or the update will not be applied.

Uninstalling a Plug-in Package
-----
To uninstall a plug-in, you have two options:
> - Run the Plugin Manager that is installed with Var:ProductName. 
> - Use RWS AppStore Integration from Var:ProductName.
<img style="display:block; " src="images/uninstall.png" /> </br>

If you have an older Var:ProductName version, install if from [here](https://appstore.sdl.com/language/app/sdl-plugin-installer/462/).
