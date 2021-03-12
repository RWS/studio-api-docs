The Plug-in Manifest
======
All Visual Studio projects that are based on the templates that come with the SDL Trados Studio 2017 SDK are configured to automatically create a plug-in package (i.e. an *.sdlplugin file) when the project is built. (See also Setting up the Project). One essential piece of information required in order to do this is the plug-in package manifest, which is defined in the file pluginpackage.manifest.xml, which is part of the project template. If this manifest is missing, the project package cannot be built (see also Building the Plug-in).

Below you see what the manifest of our sample plug-in looks like:

The manifest contains the following information:
* **PlugInName**: indicates the friendly name of the plugin. This string be different from the name of the plug-in that is defined in **PluginResources.resx**. The reason for this is that a plug-in package can - in theory - contain multiple plug-ins, which need to be distinguished.
* **Version**: the version of the plug-in package. This information used to detect any package updates upon start-up of Trados Studio 2017, and is therefore important.
* **Description**: descriptive information of the plug-in package.
* **Author**: the name of the plug-in developer.
* **RequiredProduct**: this string indicates which SDL product version is required to run the given plug-in. The information that you indicate here must include the minimum version, and can optionally include a maximum version.