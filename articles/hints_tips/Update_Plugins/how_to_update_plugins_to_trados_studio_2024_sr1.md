# How to update plugins to Trados Studio 2024 SR1

The following are a list of changes and known issues to consider when updating your plugin to be compatible with Trados Studio 2024 SR1.

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
  <Version>1.1.0.0</Version>
  <Description>My plugin description</Description>
  <Author>Trados AppStore Team</Author>
  <RequiredProduct name="TradosStudio" minversion="18.1" maxversion="18.1.9" />
</PluginPackage>
```
Ensure **RequiredProduct** reflects `minversion="18.1"` and `maxversion="18.1.9"`.


## Project References & Deployment Path
Update references and deployment settings in your .csproj:

### [Production](#tab/standard)

**References**: Set Trados Studio assemblies to use the Studio 18 path:
~~~xml
<Reference Include="Sdl.Desktop.IntegrationApi.Extensions">
    <HintPath>$(MSBuildProgramFiles32)\Trados\Trados Studio\Studio18\Sdl.Desktop.IntegrationApi.Extensions.dll</HintPath>
</Reference>
~~~

**Deployment Path:**
~~~xml
<PluginDeploymentPath>$(AppData)\Trados\Trados Studio\18\Plugins</PluginDeploymentPath>
~~~

### [BETA](#tab/beta)

**References**: Set Trados Studio assemblies to use the Studio 18 Beta path:
~~~xml
<Reference Include="Sdl.Desktop.IntegrationApi.Extensions">
    <HintPath>$(MSBuildProgramFiles32)\Trados\Trados Studio\Studio18Beta\Sdl.Desktop.IntegrationApi.Extensions.dll</HintPath>
</Reference>
~~~

**Deployment Path:**
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

## Known Issues & Dependency Updates 
### Dependency Version Changes
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
<br/>

## Breaking API Changes

### Assembly Version Change Requires Recompilation
With Trados Studio 2024 SR1, all core Trados assemblies have had their **assembly version increased to 18.1.x.x** (reflecting the semantic versioning minor update). **This assembly version bump introduces a breaking change:** Any plugin or standalone tool that references Trados assemblies must be recompiled against the new release, even if no other code changes are required. Referencing outdated assemblies is not supported and will likely result in runtime failures due to version mismatches.  
<br/>

### Multiterm API Changes
**Migrate from `Sdl.Multiterm.TMO.Interop.dll` to `TerminologyProviderManager`**

As part of the separation of MultiTerm from Trados Studio, the legacy assembly `Sdl.Multiterm.TMO.Interop.dll` **has been removed from the Trados Studio installation folder as of SR1**. Any integration or plugin referencing TMO interop must be updated: **the endorsed approach for terminology-related integrations moving forward is the `TerminologyProviderManager` singleton.**

**How to migrate:** Use `Sdl.Terminology.TerminologyProvider.Core.TerminologyProviderManager.Instance` to access and initialize terminology providers.

**Sample code:**
```csharp
using Sdl.Terminology.TerminologyProvider.Core;

// Example URI for your termbase
string termbaseUriString = "file:///C:/Termbases/MyTermbase.sdltb";
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
var sourceLanguage = new CultureInfo("en-US");
var targetLanguage = new CultureInfo("it-IT");
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
**Note:** Replace any references to `Sdl.Multiterm.TMO.Interop.dll` with the modern `TerminologyProviderManager` API. This ensures compatibility with Trados Studio 2024 SR1 and future releases, and aligns with Trados ongoing architectural updates.

<br/>

## Credential Management Best Practices

When building plugins or integrating third-party terminology and translation providers with Trados Studio, you should always manage credentials independently in your own codebase, especially when working with non-Trados services.

**Key Points:**
- Built-in credential storage in Trados Studio is only required when you need to access native Trados resources (e.g., file-based translation memories, Language Cloud).
- For all other scenarios, including any third-party or custom service integration, you should implement your own secure mechanism for storing and retrieving credentials.
- Managing credentials independently enhances security and gives you greater flexibility and control.

**Best Practice:**
Always use your own secure credential management system, unless your integration specifically requires direct access to Trados resources.  

---

### Example: Managing Credentials Securely
Below is an example showing how you can implement credential management using Windows Credential Manager. This strategy may be adapted to use other secure stores as required by your environment or policies.  

**Credential Store Interface:**
```csharp
public interface ICredentialStore
{
    void SaveCredential(string key, string username, string password);
    (string Username, string Password)? GetCredential(string key);
    void RemoveCredential(string key);
}
```
 
**Sample Windows Credential Manager Implementation**  
*Install the [CredentialManagement](https://www.nuget.org/packages/CredentialManagement/) NuGet package for this sample:*

```csharp
using CredentialManagement;

public class WindowsCredentialStore : ICredentialStore
{
    public void SaveCredential(string key, string username, string password)
    {
        var cred = new Credential
        {
            Target = key,
            Username = username,
            Password = password,
            PersistanceType = PersistanceType.LocalComputer
        };
        cred.Save();
    }

    public (string Username, string Password)? GetCredential(string key)
    {
        var cred = new Credential { Target = key };
        if (cred.Load())
        {
            return (cred.Username, cred.Password);
        }
        return null;
    }

    public void RemoveCredential(string key)
    {
        var cred = new Credential { Target = key };
        cred.Delete();
    }
}
```

**How to Use in Your Integration**
```csharp
// Example usage
var credentialStore = new WindowsCredentialStore();
var credentials = credentialStore.GetCredential("your-provider-key");
if (credentials.HasValue) {
    var username = credentials.Value.Username;
    var password = credentials.Value.Password;
    // Use credentials securely...
} else {
    // Handle missing credentials scenario
}
```
**Note:** Using your own credential store ensures future compatibility, enhances security, and keeps your integration flexible as Trados Studio and its APIs evolve.
  
