# How to update plugins to Trados Studio 2024

The following are a list of changes and known issues to consider when updating your plugin to be compatible with Trados Studio 2024.

## Plugin Framework
The latest version of the plugin framework packages should be installed. You can add/update the plugin framework nuget packages in your project via the package manager user interface or console.
### Package Manager UI
* In **Solution Explorer**, right-click **References** and choose **Manage NuGet Packages**.
* Select nuget.org as the **Package source**.
* Search for `Sdl.Core.PluginFramework` from the **Browse** tab.
* Select the package from the list and click **Install** or **Update**.
  * `Sdl.Core.PluginFramework`, version _2.1.0_
  * `Sdl.Core.PluginFramework.Build`, version _18.0.1_
* Accept any license prompts to finnish the installation.

<br/>

## Plugin Manifest

The manifest file **pluginpackage.manifest.xml** is located at the root of your project solution.  The values of the **RequiredProduct** should be updated to align with the latest release.

### RequiredProduct

- Min version should be set to: 18.0. 
- Max version should be set to 18.9. It is recommended to also set this value, as it will provide the AppStore with sufficient information in correctly identifying plugins that are compatible with the version of Trados Studio that is launched.
- Name should be set to: *TradosStudio*

Example
```xml
<?xml version="1.0" encoding="utf-8"?>
<PluginPackage xmlns="http://www.sdl.com/Plugins/PluginPackage/1.0">
  <PlugInName>My plugin name</PlugInName>
  <Version>1.1.0.0</Version>
  <Description>My plugin description</Description>
  <Author>Trados AppStore Team</Author>
  <RequiredProduct name="TradosStudio" minversion="18.0" maxversion="18.9" />
</PluginPackage>
```

## Project References & Deployment Path
The references in the project file (.csproj) should be mapped to the new installation path for Trados Studio 2024. Make reference to the following examples for both the production and beta releases.

### [Production](#tab/standard)

Installation path: *$(MSBuildProgramFiles32)\Trados\Trados Studio\Studio18*:
~~~xml
<Reference Include="Sdl.Desktop.IntegrationApi.Extensions">
    <HintPath>$(MSBuildProgramFiles32)\Trados\Trados Studio\Studio18\Sdl.Desktop.IntegrationApi.Extensions.dll</HintPath>
</Reference>
~~~
<br/>

Plugin deployment path: *$(AppData)\Trados\Trados Studio\18\Plugins*:
~~~xml
<PluginDeploymentPath>$(AppData)\Trados\Trados Studio\18\Plugins</PluginDeploymentPath>
~~~

### [BETA](#tab/beta)

Installation path: *$(MSBuildProgramFiles32)\Trados\Trados Studio\Studio18Beta*:
~~~xml
<Reference Include="Sdl.Desktop.IntegrationApi.Extensions">
    <HintPath>$(MSBuildProgramFiles32)\Trados\Trados Studio\Studio18Beta\Sdl.Desktop.IntegrationApi.Extensions.dll</HintPath>
</Reference>
~~~
<br/>

Plugin deployment path: *$(AppData)\Trados\Trados Studio\18Beta\Plugins*:
~~~xml
<PluginDeploymentPath>$(AppData)\Trados\Trados Studio\18Beta\Plugins</PluginDeploymentPath>
~~~

---

> [!NOTE]
>
> To update settings directly in the project file from Visual Studio
> * Right-click on the project node in the **Solution Explorer** and select **Unload Project**.
> * Then, right-click on the project and choose **Edit** <projectname>
> 
> Once you have applied your changes in the project file, then reload project
> * In the **Solution Explorer**, select the projects you want to load (press **Ctrl** while clicking to select more than one project)
> * Then right-click on the project and choose **Reload Project**.

<br/>
<br/>

## Known Issues
The following are a list of known issues and solutions that you might encounter depending on your settings and configuration:

### Trados.Community.Toolkit (formally SDL.Community.Toolkit)
A new version of the Trados Community Toolkit, version 5.0.1, has been released to support the latest version of Trados Studio 2024.  This includes the following assemblies:

- [Trados.Community.Toolkit.Core](https://www.nuget.org/packages/Trados.Community.Toolkit.Core)
- [Trados.Community.Toolkit.LanguagePlatform](https://www.nuget.org/packages/Trados.Community.Toolkit.LanguagePlatform)
- [Trados.Community.Toolkit.Integration](https://www.nuget.org/packages/Trados.Community.Toolkit.Integration)
- [Trados.Community.Toolkit.FileType](https://www.nuget.org/packages/Trados.Community.Toolkit.FileType)
- [Trados.Community.Toolkit.ProjectAutomation](https://www.nuget.org/packages/Trados.Community.Toolkit.ProjectAutomation)

### Dependency version changes
There is a list of known dependency version changes that may influence your integration with the latest Trados Studio 2024 APIs; this is typically seen from standalone applications that are running outside of the Trados Studio context.  To resolve these references, include the following binding redirects in the configuration file of the project.

``` xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
  </startup>
  <runtime>
    <NetFx40_PInvokeStackResilience enabled="1" />
    <legacyCorruptedStateExceptionsPolicy enabled="true" />
    <ThrowUnobservedTaskException enabled="true" />
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.DependencyModel" publicKeyToken="adb9793829ddae60" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-7.0.0.0" newVersion="7.0.0.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>
```
