# How to update classic plugins to Var:ProductNameWithEdition

⚠️ Note: Var:ProductNameWithEdition will also support a new plugin system, check back for details on the new plugin system when it is made available.

The following are a list of changes and known issues to consider when updating your plugin to be compatible with Var:ProductNameWithEdition.

## Transition to 64-Bit (x64)
Var:ProductNameWithEdition is released as a 64-bit (x64) version. As a result, plug-ins must also be rebuilt and updated to target x64 in order to remain compatible. 

The following element must be added in a global property group of the .csproj file to target x64:


```xml
<PropertyGroup>
	...
	<PlatformTarget>x64</PlatformTarget>
	...
</PropertyGroup>
```
> [!NOTE]
>
> If the project is not SDK-style, <PlatformTarget>x64</PlatformTarget> must be set for all relevant configurations (e.g., Debug and Release), otherwise some builds may still compile for a different platform.

## Update Plugin Framework Packages
Ensure you are using the latest plugin framework NuGet packages:
- **Sdl.Core.PluginFramework:** v2.1.0
- **Sdl.Core.PluginFramework.Build:** v18.0.1

**How to update:**
- In Solution Explorer, right-click **References > Manage NuGet Packages.**
- Set http://nuget.org as your package source.
- Search, select, and install/update the above packages.
- Accept license agreements to complete installation.

## Update Plugin Manifest

Review and update the manifest (`pluginpackage.manifest.xml`) at the project root as in the following example:
```xml
<?xml version="1.0" encoding="utf-8"?>
<PluginPackage xmlns="http://www.sdl.com/Plugins/PluginPackage/1.0">
  <PlugInName>My plugin name</PlugInName>
  <Version>1.0.0.0</Version>
  <Description>My plugin description</Description>
  <Author>Trados AppStore Team</Author>
  <RequiredProduct name="TradosStudio" minversion="19.0" maxversion="19.9" />
</PluginPackage>
```
Ensure **RequiredProduct** reflects `minversion="19.0"` and `maxversion="19.9"`.

## Project References & Deployment Path
Update references and deployment settings in your .csproj:

### [Production](#tab/standard)

**References**: Set Trados Studio assemblies to use the Studio 19 path:
~~~xml
<Reference Include="Sdl.Desktop.IntegrationApi.Extensions">
    <HintPath>$(ProgramFiles)\Trados\Trados Studio\Studio19\Sdl.Desktop.IntegrationApi.Extensions.dll</HintPath>
</Reference>
~~~

**Deployment Path:**
~~~xml
<PluginDeploymentPath>$(AppData)\Trados\Trados Studio\19\Plugins</PluginDeploymentPath>
~~~

### [BETA](#tab/beta)

**References**: Set Trados Studio assemblies to use the Studio 19 Beta path:
~~~xml
<Reference Include="Sdl.Desktop.IntegrationApi.Extensions">
    <HintPath>$(ProgramFiles)\Trados\Trados Studio\Studio19Beta\Sdl.Desktop.IntegrationApi.Extensions.dll</HintPath>
</Reference>
~~~

**Deployment Path:**
~~~xml
<PluginDeploymentPath>$(AppData)\Trados\Trados Studio\19Beta\Plugins</PluginDeploymentPath>
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
> 
> If the project is SDK-style, then unloading/reloading is unnecessary.

## Known Issues & Dependency Updates 
The following are a list of known issues and solutions that you might encounter depending on your settings and configuration:
### Dependency version changes
Standalone integrations may require binding redirects. Example for `App.config`:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="BestMatchServiceUrlsConfig" type="Sdl.BestMatchServiceStudioIntegration.Common.UrlsConfig, Sdl.BestMatchServiceStudioIntegration.Common" />
  </configSections>
  <BestMatchServiceUrlsConfig GlobalApi="https://api.cloud.trados.com" auth0="https://sdl-prod.eu.auth0.com">
    <LanguageConfig>
      <Lang Code="en" />
      <Lang Code="de" />
      <Lang Code="fr" />
      <Lang Code="es" />
      <Lang Code="ja" />
      <Lang Code="it" />
      <Lang Code="zh-CN" />
    </LanguageConfig>
    <HavingTroubleSigningIn url="https://gateway.sdl.com/apex/communityknowledge?articleName=000001399" />
  </BestMatchServiceUrlsConfig>  
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
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
  </startup>  
</configuration>
```
### Breaking API Changes
`ITerminologyProviderCredentialStore` was removed (together with method parameters of this type).

`TerminologyProviderManager.DefaultTerminologyCredentialStore` was removed.

`public TerminologyProviderType Type` property removed from `ITerminologyProvider`
#### Working with BCMs
The BCM-related classes have been moved from Sdl.LanguagePlatform.TranslationMemoryApito TradosStudio.BcmLite:
```xml
<Reference Include="TradosStudio.BcmLite">
	<HintPath>$(ProgramFiles)\Trados\Trados Studio\Studio19\TradosStudio.BcmLite.dll</HintPath>
</Reference>
```
And some classes were renamed:

`LiteDocument` to `Document`

`LiteFragment` to `DocumentFragment`

The `LiteBcmVisitor` abstract class now includes two more methods:

```cs
public abstract void VisitFeedbackContainer(FeedbackContainer feedbackContainer);
public abstract void VisitStructure(StructureTag structureTag);
```

#### Assembly Version Change Requires Recompilation
With Trados Studio 2026 Release, all core Trados assemblies have had their **assembly version increased to 19.x.x.x** (reflecting the semantic versioning minor update). **This assembly version bump introduces a breaking change:** Any plugin or standalone tool that references Trados assemblies must be recompiled against the new release, even if no other code changes are required. Referencing outdated assemblies is not supported and will likely result in runtime failures due to version mismatches.
### Multiterm API Changes
**Migrate from `Sdl.Multiterm.TMO.Interop.dll` to `TerminologyProviderManager`**

As part of the separation of MultiTerm from Trados Studio, the legacy assembly `Sdl.Multiterm.TMO.Interop.dll` **has been removed from the Trados Studio installation folder as of SR1**. Any integration or plugin referencing TMO interop must be updated: **the endorsed approach for terminology-related integrations moving forward is the `TerminologyProviderManager` singleton.**

**How to migrate:** Use `Sdl.Terminology.TerminologyProvider.Core.TerminologyProviderManager.Instance` to access and initialize terminology providers.

**Sample code:**
```csharp
using Sdl.Terminology.TerminologyProvider.Core;

// Example URI for your termbase
string termbaseUriString = "ttb.file:///C:/Termbases/MyTermbase.ttb";
Uri termbaseUri = new Uri(termbaseUriString);

// Get the terminology provider singleton instance
var terminologyProvider = TerminologyProviderManager.Instance.GetTerminologyProvider(termbaseUri);

// Ensure the provider is initialized before searching
if (!terminologyProvider.IsInitialized)
{
    try
    {
        terminologyProvider.Initialize();
    }
    catch (Exception ex)
    {
        // Handle initialization errors appropriately (log or surface meaningful error)
        throw new InvalidOperationException("Failed to initialize terminology provider.", ex);
    }
}

// Set up search parameters
var sourceLanguage = new DefinitionLanguage
{
    Locale = "EN",
    Name = "English"
};
var targetLanguage = new DefinitionLanguage
{
    Locale = "DE",
    Name = "German"
};
string segmentText = "Insert your source segment text here";
int maxResultsCount = 10;
bool targetRequired = true;

// Perform a fuzzy terminology search
var searchResults = terminologyProvider.Search(
    segmentText,
    sourceLanguage,
    targetLanguage,
    maxResultsCount,
    SearchMode.Fuzzy,
    targetRequired
);

// Process or display results as needed...
```
**Note:** Replace any references to `Sdl.Multiterm.TMO.Interop.dll` with the modern `TerminologyProviderManager` API. This ensures compatibility with Trados Studio 2026 Release and future releases, and aligns with Trados ongoing architectural updates.


### Trados.Community.Toolkit (formally SDL.Community.Toolkit)
A new version of the Trados Community Toolkit, version 6.0.2, has been released to support the latest version of Trados Studio 2026 Release.  This includes the following assemblies:

- [Trados.Community.Toolkit.Core](https://www.nuget.org/packages/Trados.Community.Toolkit.Core)
- [Trados.Community.Toolkit.LanguagePlatform](https://www.nuget.org/packages/Trados.Community.Toolkit.LanguagePlatform)
- [Trados.Community.Toolkit.Integration](https://www.nuget.org/packages/Trados.Community.Toolkit.Integration)
- [Trados.Community.Toolkit.FileType](https://www.nuget.org/packages/Trados.Community.Toolkit.FileType)
- [Trados.Community.Toolkit.ProjectAutomation](https://www.nuget.org/packages/Trados.Community.Toolkit.ProjectAutomation)

