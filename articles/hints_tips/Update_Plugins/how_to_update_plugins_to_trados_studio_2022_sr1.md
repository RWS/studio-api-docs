# How to update plugins to <var:ProductNameWithEdition> SR1

In an effort to achieve consistent behaviour across a range of platforms and applications, some changes have been made to control the way in which language metadata is used in our software.  These changes may affect your plugin or application integration with the public APIs, especially if [Sdl.Core.Globalization.Language](../../../api/core/Sdl.Core.Globalization.Language.yml) referenced in the project.

The following are a list of changes and known issues to consider when updating your plugin to be compatible with  <var:ProductNameWithEdition> SR1

- Plugin Manifest
- Project References
- Language Registry API

## Plugin Manifest

The manifest file **pluginpackage.manifest.xml** is located at the root of your project solution.  The values of the **RequiredProduct** should be updated to align with the latest release.

### RequiredProduct

- Min version should be set to: 17.1. If your plugin has been updated to support this latest release of <var:ProductName>, then you should reflect this by setting the minimum supported version to 17.1
- Max version should be set to 17.9. It is recommended to also set this value, as it will provide the AppStore with sufficient information in correctly identifying plugins that are compatible with the version of <var:ProductName> that is launched.
- Name should be set to: *TradosStudio*

Example

```xml
<?xml version="1.0" encoding="utf-8"?>
<PluginPackage xmlns="http://www.sdl.com/Plugins/PluginPackage/1.0">
  <PlugInName>My plugin name</PlugInName>
  <Version>1.1.0.0</Version>
  <Description>My plugin description</Description>
  <Author>Trados AppStore Team</Author>
  <RequiredProduct name="TradosStudio" minversion="17.1" maxversion="17.9" />
</PluginPackage>
```

## Project References
The following changes should be applied in the project file (.csproj)

- Add a reference to Sdl.Core.Globalization.Async, if your code is making reference to [Sdl.Core.Globalization.Language](../../../api/core/Sdl.Core.Globalization.Language.yml)

``` xml
<Reference Include="Sdl.Core.Globalization.Async">
    <HintPath>$(MSBuildProgramFiles32)\Trados\Trados Studio\Studio17\Sdl.Core.Globalization.Async.dll</HintPath>
</Reference>
```
> [!NOTE]
>
> It is recommended that you use the reserved and well-known MSBuild property **$(MSBuildProgramFiles32) ** as opposed to *$(ProgramFiles)* to define the location of the 32-bit program folder *C:\Program Files (x86)\*

- To update settings directly in the project file from Visual Studio
    - Right-click on the project node in the Solution Explorer and select Unload Project.
    - Then, right-click on the project and choose Edit <projectname>
- Once you have applied your changes in the project file, then reload project
    - In the Solution Explorer, select the projects you want to load (press Ctrl while clicking to select more than one project)
    - Then right-click on the project and choose Reload Project.

## Language Registry API
Third party developers now have access to Trados Studio's custom language registry, which offers finer control over language management than the language registry provided by Microsoft. Following this change, [CultureCode] is now the recommended alternative to the standard CultureInfo.

To ensure compatibility with Studio and other RWS system interfacing with Studio please recover the language info using our internal language registry

```cs
private CultureCode GetCultureCode(string cultureIsoCode)
{
    try
    {
        // Language registry contains all the languages that are supported in Studio               
        var language = LanguageRegistryApi.Instance.GetLanguage(cultureIsoCode);
        return new CultureCode(language.CultureInfo);
    }
    catch (UnsupportedLanguageException)
    {
        // In case the language is not supported an exception is thrown
        return null;
    }
}
```

Use **LanguageRegistryApi.Instance** to recover all langauges

```cs
private IList<Language> GetAllLanguages()
{  
    return LanguageRegistryApi.Instance.GetAllLanguages()
}
```

## Known Issues
The following are a list of known issues and solutions that you might encounter depending on your settings and configuration:

### Trados.Community.Toolkit (formally SDL.Community.Toolkit)
A new version of the Trados Community Toolkit, version 4.1.0, has been released to support the latest version of Trados Studio 2022 SR1.  This includes the following assemblies:

- [Trados.Community.Toolkit.Core](https://www.nuget.org/packages/Trados.Community.Toolkit.Core)
- [Trados.Community.Toolkit.LanguagePlatform](https://www.nuget.org/packages/Trados.Community.Toolkit.LanguagePlatform)
- [Trados.Community.Toolkit.Integration](https://www.nuget.org/packages/Trados.Community.Toolkit.Integration)
- [Trados.Community.Toolkit.FileType](https://www.nuget.org/packages/Trados.Community.Toolkit.FileType)
- [Trados.Community.Toolkit.ProjectAutomation](https://www.nuget.org/packages/Trados.Community.Toolkit.ProjectAutomation)

### Dependency version changes
There is a list of known dependency version changes that may influence your integration with the latest Trados Studio 2022 APIs; this is typically seen from standalone applications that are running outside of the Trados Studio context.  To resolve these references, include the following binding redirects in the configuration file of the project.

``` xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <startup useLegacyV2RuntimeActivationPolicy="true">
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
  </startup>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.Memory" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.1.1" newVersion="4.0.1.1" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="icu.net" publicKeyToken="416fdd914afa6b66" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-2.7.0.0" newVersion="2.7.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Data.SqlClient" publicKeyToken="23ec7fc2d6eaa4a5" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-5.0.0.0" newVersion="5.0.0.0" />
      </dependentAssembly>
    </assemblyBinding>
```