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
  <RequiredProduct name="TradosStudio" minversion="19.0" maxversion="19.0.9" />
</PluginPackage>
```
Ensure **RequiredProduct** reflects `minversion="19.0"` and `maxversion="19.0.9"`.

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
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>
  <startup useLegacyV2RuntimeActivationPolicy="true">
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
  </startup>

  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.Memory"
                                publicKeyToken="cc7b13ffcd2ddd51"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.0.1.2"
                               newVersion="4.0.1.2" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Runtime.CompilerServices.Unsafe"
                                publicKeyToken="b03f5f7f11d50a3a"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535"
                               newVersion="6.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.Logging"
                                publicKeyToken="adb9793829ddae60"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535"
                               newVersion="8.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.Logging.Abstractions"
                                publicKeyToken="adb9793829ddae60"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535"
                               newVersion="8.0.0.2" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.Options"
                                publicKeyToken="adb9793829ddae60"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535"
                               newVersion="8.0.0.2" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Collections.Immutable"
                                publicKeyToken="b03f5f7f11d50a3a"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535"
                               newVersion="8.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Text.Encoding.CodePages"
                                publicKeyToken="b03f5f7f11d50a3a"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535"
                               newVersion="8.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Threading.Tasks.Extensions"
                                publicKeyToken="cc7b13ffcd2ddd51"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.2.0.1"
                               newVersion="4.2.0.1" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Text.Json"
                                publicKeyToken="cc7b13ffcd2ddd51"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-8.0.0.6"
                               newVersion="8.0.0.6" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Bcl.AsyncInterfaces"
                                publicKeyToken="cc7b13ffcd2ddd51"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-8.0.0.0"
                               newVersion="8.0.0.0" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="System.Diagnostics.DiagnosticSource"
                                publicKeyToken="cc7b13ffcd2ddd51"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-8.0.0.1"
                               newVersion="8.0.0.1" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.DependencyInjection.Abstractions"
                                publicKeyToken="adb9793829ddae60"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-8.0.0.2"
                               newVersion="8.0.0.2" />
      </dependentAssembly>
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.DependencyInjection"
                                publicKeyToken="adb9793829ddae60"
                                culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-8.0.0.0"
                               newVersion="8.0.0.1" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>

  <log4net>
    <!-- Set levels to DEBUG for extended logging information -->
    <appender name="RollingFile" type="Sdl.Desktop.Logger.LocalUserAppDataFileAppender, Sdl.Desktop.Logger">
    </appender>
    <!-- output to debug string -->
    <appender name="OutputDebug" type="log4net.Appender.OutputDebugStringAppender">
      <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date [%thread] %-5level %logger [%property{NDC}] - %message%newline" />
      </layout>
    </appender>
    <root>
      <level value="INFO" />
      <appender-ref ref="RollingFile" />
      <appender-ref ref="OutputDebug" />
    </root>
    <logger name="Sdl.TranslationStudio">
      <level value="INFO" />
    </logger>
    <logger name="Sdl.Desktop">
      <level value="INFO" />
    </logger>
    <logger name="Sdl.ProjectApi">
      <level value="INFO" />
    </logger>
    <logger name="Licensing">
      <level value="INFO" />
    </logger>
    <!--<logger name="Sdl.MultiTerm">
      <level value="DEBUG"/>
    </logger>-->
  </log4net>
</configuration>
```
In most cases, this will be sufficient to enable proper interaction with the Studio APIs.

If the application still fails to work as expected, an alternative approach is to copy the entire configuration from the Trados Studio executable configuration file (`SdlTradosStudio.exe.config`) into your application's `app.config`. This ensures that all necessary runtime and binding settings are aligned with those expected by Studio.
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
**⚠️** Notice the change of the URI scheme from SDLTB to TTB (ttb.file:///).

**Note:** Replace any references to `Sdl.Multiterm.TMO.Interop.dll` with the modern `TerminologyProviderManager` API. This ensures compatibility with Trados Studio 2026 Release and future releases, and aligns with Trados ongoing architectural updates.


### Trados.Community.Toolkit (formally SDL.Community.Toolkit)
A new version of the Trados Community Toolkit, version 6.0.2, has been released to support the latest version of Trados Studio 2026 Release.  This includes the following assemblies:

- [Trados.Community.Toolkit.Core](https://www.nuget.org/packages/Trados.Community.Toolkit.Core)
- [Trados.Community.Toolkit.LanguagePlatform](https://www.nuget.org/packages/Trados.Community.Toolkit.LanguagePlatform)
- [Trados.Community.Toolkit.Integration](https://www.nuget.org/packages/Trados.Community.Toolkit.Integration)
- [Trados.Community.Toolkit.FileType](https://www.nuget.org/packages/Trados.Community.Toolkit.FileType)
- [Trados.Community.Toolkit.ProjectAutomation](https://www.nuget.org/packages/Trados.Community.Toolkit.ProjectAutomation)

