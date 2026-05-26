# The Plug-in Manifest

Visual Studio projects based on the templates included with the Var:ProductName SDK automatically create a plug-in package (that is, an **.sdlplugin** file) when you build the project. See also [Setting up the Project](setting_up_the_provider_project.md).

To support this process, the project template includes the plug-in package manifest in **pluginpackage.manifest.xml**. If the manifest is missing, the project package cannot be built. See also [Building the Plug-in](building_the_plugin.md).

The manifest for the sample plug-in looks like this:
# [XML](#tab/tabid-1)
```xml
<?xml version="1.0" encoding="utf-8"?>
<PluginPackage xmlns="http://www.sdl.com/Plugins/PluginPackage/1.0">
  <PlugInName>Sdk.LanguagePlatform.Samples.ListProvider</PlugInName>
  <Version>1.0.0.0</Version>
  <Description>Delimited List Translation Provider</Description>
  <Author>SDK Sample Provider</Author>
  <RequiredProduct name="TradosStudio" minversion="19.0" maxversion="19.9" />
</PluginPackage>
```
***

The manifest includes the following information:

* **PlugInName**: the friendly name of the plug-in. This string can differ from the name defined in **PluginResources.resx**. A plug-in package can contain multiple plug-ins, so each plug-in needs a distinct identifier.
* **Version**: the version of the plug-in package. Var:ProductName uses this information to detect package updates at start-up.
* **Description**: descriptive information about the plug-in package.
* **Author**: the name of the plug-in developer.
* **RequiredProduct**: the product version required to run the plug-in. Specify the minimum version and, optionally, a maximum version.

# See Also
[Building the Plug-in](building_the_plugin.md)
