Plug-in Deployment
=====
This section describes how to deploy an SDL Plug-in Package (*.sdlplugin) for use in Trados Studio. It also covers updating to a new version of the plug-in package and uninstall a plug-in package.

Installing a Plug-in Package
------
Plug-in package installation is really easy. Just copy the plug-in package into C:\Users\[username]\AppData\Local\SDL\SDL Trados Studio\14\Plugins\Packages. Now start Trados Studio. The application will automatically detect the presence of the new plug-in package, verify it, extract its contents into C:\Users\[username]\AppData\Local\SDL\SDL Trados Studio\14\Plugins\Unpackaged and load it.

The following warning message will be shown while Trados Studio starts:

<img style="display:block; " src="images/UnsignedPluginWarning.png"/>


To avoid this message from appearing, you need to submit your plug-in package to SDL for verification. Once verified, your plug-in package will be signed by SDL and no warning message will appear anymore. For more information, see (TODO!).

Once Trados Studio has started up, go to the Tools->Plug-ins dialog and notice that "MyTranslationProvider" is now listed as a plug-in and is ready to be used.

Updating a Plug-in Package
----
Once deployed, you can update your plug-in package by increasing the version in the plug-in package manifest (see Building a Plug-in). Now simply copy the updated plug-in package to C:\Users\[username]\AppData\Local\SDL\SDL Trados Studio\14\Plugins\Packages, overwriting the the plug-in package that was there already. Now start Trados Studio. The application will automatically detect the presence of the updated plug-in package, verify it, extract its contents into C:\Users\[username]\AppData\Local\SDL\SDL Trados Studio\14\Plugins\Unpackaged and load it.

Note that it is essential that you increase the verion of the plug-in package as listed in the plug-in manifest, or the update will not be applied.

Uninstalling a Plug-in Package
----
To uninstall a plug-in, simply delete the plug-in package from the plug-in packages folder (C:\Users\[username]\AppData\Local\SDL\SDL Trados Studio\14\Plugins\Packages). The next time Trados Studio starts, it will detect this deletion and remove the corresponding extracted content.