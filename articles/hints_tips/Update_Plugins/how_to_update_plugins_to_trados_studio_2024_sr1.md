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
  
