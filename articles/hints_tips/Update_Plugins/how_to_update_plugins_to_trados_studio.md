# How to Update Classic Plugins to Var:ProductNameWithEdition

This guide provides step-by-step instructions for updating classic plugins to ensure compatibility with Var:ProductNameWithEdition.

> [!NOTE]
>Var:ProductNameWithEdition will also support a new plugin system. Check back for details when it becomes available.

The sections below outline key changes and known issues to consider when updating your plugin.

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
> If the project is not SDK-style, `<PlatformTarget>x64</PlatformTarget>` must be set for all relevant configurations (e.g., Debug and Release). Otherwise, some builds may still compile for a different platform.

## Update Plugin Framework Packages
Ensure you are using the latest plugin framework NuGet packages:
- **Sdl.Core.PluginFramework:** v2.1.0
- **Sdl.Core.PluginFramework.Build:** v18.0.1

**How to update:**
- In Solution Explorer, right-click **References > Manage NuGet Packages.**
- Set http://nuget.org as your package source.
- Search for, select, and install/update the above packages.
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

## Project References and Deployment Path
Update references and deployment settings in your .csproj:

### Production

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

### Beta

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
> To update settings directly in the project file from Visual Studio:
> - Right-click on the project node in the **Solution Explorer** and select **Unload Project**.
> - Then right-click on the project and choose **Edit** <projectname>.
> - After applying your changes in the project file, reload the project.
> - In the **Solution Explorer**, select the projects you want to load (press **Ctrl** while clicking to select more than one project).
> - Then right-click on the project and choose **Reload Project**.
> 
> If the project is SDK-style, then unloading/reloading is unnecessary.

## Known Issues and Dependency Updates
The following are a list of known issues and solutions that you might encounter depending on your settings and configuration:
### Dependency Version Changes
Standalone integrations may require binding redirects. Example for `App.config`:
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <startup useLegacyV2RuntimeActivationPolicy="true">
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
  </startup>

  <runtime>
       <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.Logging.Abstractions" publicKeyToken="adb9793829ddae60" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-65535.65535.65535.65535" newVersion="10.0.0.5" />
      </dependentAssembly>
    </assemblyBinding>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Bcl.AsyncInterfaces" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-10.0.0.8" newVersion="10.0.0.8" />
      </dependentAssembly>
    </assemblyBinding>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.DependencyInjection.Abstractions" publicKeyToken="adb9793829ddae60" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-10.0.0.5" newVersion="10.0.0.5" />
      </dependentAssembly>
    </assemblyBinding>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.Logging" publicKeyToken="adb9793829ddae60" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-10.0.0.5" newVersion="10.0.0.5" />
      </dependentAssembly>
    </assemblyBinding>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Microsoft.Extensions.Primitives" publicKeyToken="adb9793829ddae60" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-10.0.0.5" newVersion="10.0.0.5" />
      </dependentAssembly>
    </assemblyBinding>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.Reflection.Metadata" publicKeyToken="b03f5f7f11d50a3a" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-10.0.0.8" newVersion="10.0.0.8" />
      </dependentAssembly>
    </assemblyBinding>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="System.Threading.Tasks.Extensions" publicKeyToken="cc7b13ffcd2ddd51" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-4.2.4.0" newVersion="4.2.4.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>
```
In most cases, this will be sufficient to enable proper interaction with the Studio APIs.

If the application still fails to work as expected, an alternative approach is to copy the entire configuration from the Trados Studio executable configuration file (`SdlTradosStudio.exe.config`) into your application's `app.config`. This ensures that all necessary runtime and binding settings are aligned with those expected by Studio.
## Breaking API Changes

As part of our ongoing efforts to simplify and streamline the Trados Studio development experience, we are revisiting the existing Trados Studio APIs. During this process, several classes, interfaces, and methods that were identified as redundant or unnecessarily complex have been removed.

Looking ahead, we plan to gradually phase out portions of the current API set and replace them with APIs aligned with the new Trados Studio architecture. This transition is intended to improve performance, provide a more consistent developer experience, and simplify plugin integration. Our goal is to make this transition as smooth as possible, and we will provide ample notice whenever an API is deprecated and before it is eventually removed.

In addition, direct interaction with Trados GroupShare resources will be limited. For all server-based resource interactions, we strongly recommend using the GroupShare API Toolkit, which is designed specifically for secure and efficient integration with GroupShare services. You can find resources [here](https://developers.rws.com/groupshare-api-docs/apiconcepts/overview.html). 

We value feedback from our developer community. Share your thoughts, implementation experiences, and questions regarding these changes on the [Trados Studio Developers Forum](https://community.rws.com/developers-more/trados-portfolio/trados-studio-developers/f/sdk_qa). Your input will help refine the APIs and ensure a smoother transition to the new architecture.

### Terminology Provider API Changes
`ITerminologyProviderCredentialStore` was removed (together with method parameters of this type).

`TerminologyProviderManager.DefaultTerminologyCredentialStore` was removed.

The following methods were removed from `ITerminologyProvider`:
`TerminologyProviderType Type` 

`GetEntry(int id,  IEnumerable<ILanguage> language)`

`Initialize(TerminologyProviderCredential credential)`

Removed interfaces and classes :

`IConnectionAwareTerminologyProvider`

`IFileBasedTerminologyProviderDefaultPath`

`IMultipleTerminologyProvider`

`ITermbaseMetrics`

`ITerminologyMarkupProvider`

`ITerminologyProviderReadOnly`

`TerminologyProviderStatus`

`GenericCredentials`

`TerminologyProviderCredential`

`TerminologyProviderCredentialStore`

`TerminologyUserCredentials`

`TerminologyUserManagerTokenType`

`InvalidCredentialsException`

`MarkupPosition`

`SearchMarkupResult`

`SearchMarkupResultWithTermbaseName`

`SearchResultWithTermbaseName`

`TerminologyProviderType`

Removed from `ITerminologyProviderManager` the `GetTerminologyProvider` method and from its implementation the following methods :

`ITerminologyProviderWinFormsUI GetTerminologyProviderWinFormsUI(Uri uri)`

`IEnumerable<ITerminologyProviderWinFormsUI> GetTerminologyProviderWinFormsUIs()`

`ITerminologyProviderViewerWinFormsUI GetTerminologyProviderViewerWinFormsUI(Uri uri)`

`IEnumerable<ITerminologyProviderViewerWinFormsUI> GetTerminologyProviderViewerWinFormsUIs()`

`bool RemoveTerminologyProvider(Uri uri)`

Removed from TerminologyProviderDisplayInfo the following properties:
`SearchResultIcon` and `TerminologyProviderIcon`

### Translation Memory API Changes

The following classes, methods and interfaces were removed as part of the ongoing Translation Memory API refactoring and cleanup, and are no longer available for use in plugins:

`TranslationProviderCredentialStore`

`ServerBasedTranslationMemoryFactory`

`FileBasedTranslationMemoryFactory`

`AbstractMachineTranslationProviderLanguageDirection.SupportedSubsegmentMatchTypes`

`FileBasedTranslationMemoryLanguageDirection.SupportedSubsegmentMatchTypes`

`ServerBasedTranslationMemoryLanguageDirection.SupportedSubsegmentMatchTypes`

`IAlignableTranslationMemory`

`ILocalTranslationMemory`

`IPremissionCheck`

`IReindexableTranslationMemory`

Following methods were deleted from FileBasedTranslationMemory :

`MeasureModelFitness`

`ClearAlignmentData`

`DatabaseServerType` enum was removed and all of its uses were removed.

`IFileBasedTranslationMemory` has new methods and properties from the interfaces that were removed.

From `ITranslationProvider` interface, the following members were removed:

`SupportsTaggedInput`

`SupportsScoring`

`SupportsMultipleResults`

`SupportsFilters`

`SupportsPenalties`

`SupportsStructureContext`

`SupportsDocumentSearches`

`SupportsTranslation`

`SupportsFuzzySearch`

`SupportPlaceables`

`SupportWordCount`

Removed the following properties from `TranslationMemoryContainer`

`UserName`

`Password`

`DisplayText`

From `DatabaseServer`, `ServerBasedFieldsTemplate`, `ServerBasedLanguageResourcesTemplate`, `ServerBasedTranslationMemory` and `TranslationMemoryContainer` we removed the following properties :

`ParentResourceGroupName`

`ParentResourceGroupDescription`

`LinkedResourceGroupPaths`

Removed HasPermission methods from the following objects :

`DatabaseServer`

`ServerBasedFieldsTemplate`

`ServerBasedLanguageResource`

`TranslationMemoryContainer`

We removed boolean for older GS versions. Now for all removed members users can consider is default true. The members are : 

`ServerSupportsTranslationAndAnalysisService`

`IsTranslationMemoryLocationSupported`

`IsTranslationAndAnalysisServiceSupported`

We simplified TranslationMemoryQuery by removing the following members:

`Properties`

`Text`

`ContainerNames`

`FieldTemplateIds`

`LanguageResourceIds`

`IsMain`

`OwnerId`

Removed `CanReverseLanguageDirection` property from `ITranslationProviderLanguageDirection`

`FileBasedTranslationMemoryLanguageDirection`

`AbstractMachineTranslationProviderLanguageDirection`

`ServerBasedTranslationMemoryLanguageDirection`

Removed following methods from `Cascade` class :

`SubsegmentSearchResultsCollection[] SubsegmentSearchSegments(SubsegmentSearchSettings subsegmentSearchSettings, Segment[] segments, out IEnumerable<CascadeMessage> cascadeMessages)`

`SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit, out IEnumerable<CascadeMessage> cascadeMessages)`

`SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments, out IEnumerable<CascadeMessage> cascadeMessages)`

`SearchResultsMerged[] SearchSegmentsMasked(SearchSettings settings, Segment[] segments, bool[] mask, out IEnumerable<CascadeMessage> cascadeMessages)`

`SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] tus, out IEnumerable<CascadeMessage> cascadeMessages)`

`IList<CascadeMessage> GetWarningMessage(T cascadeEntry, SearchSettings searchSettings)`

`ImportResult UpdateTranslationUnit(TranslationUnit translationUnit, out IEnumerable<CascadeMessage> cascadeMessages)`

`ImportResult[] AddOrUpdateTranslationUnits(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings, out IEnumerable<CascadeMessage> cascadeMessages)`

`ImportResult[] AddTranslationUnits(TranslationUnit[] translationUnits, ImportSettings settings, out IEnumerable<CascadeMessage> cascadeMessages)`

`ImportResult[] AddTranslationUnitsMasked(TranslationUnit[] translationUnits, ImportSettings settings, bool[] mask, out IEnumerable<CascadeMessage> cascadeMessages)`

`ImportResult[] UpdateTranslationUnits(TranslationUnit[] translationUnits, out IEnumerable<CascadeMessage> cascadeMessages)`

`ImportResult AddTranslationUnit(TranslationUnit translationUnit, ImportSettings settings, out IEnumerable<CascadeMessage> cascadeMessages)`

`ImportResult[] AddOrUpdateTranslationUnits(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings)`

`ImportResult UpdateTranslationUnit(TranslationUnit translationUnit)`

`SearchResults[] SearchSegments(SearchSettings settings, Segment[] segments)`

`SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] tus)`

`ImportResult AddTranslationUnit(TranslationUnit translationUnit, ImportSettings settings)`

`SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)`

`ImportResult[] AddTranslationUnits(TranslationUnit[] translationUnits, ImportSettings settings)`

And from `CascadeMessageCode` :

`TranslationProviderDoesNotSupportDocumentSearch`

Removed unused methods from `ITranslationProvider` , `AbstractMachineTranslationProivderLanguageDirection`, `FileBasedTranslationMemoryLanguageDirection, ServerBasedTranslationMemoryLanguageDirection` :

 `ImportResult[] AddOrUpdateTranslationUnits(TranslationUnit[] translationUnits, int[] previousTranslationHashes, ImportSettings settings)`

 `ImportResult UpdateTranslationUnit(TranslationUnit translationUnit)`

`SearchResults[] SearchSegments(SearchSettings settings, Core.Segment[] segments)`

`SearchResults[] SearchTranslationUnits(SearchSettings settings, TranslationUnit[] tus)`

`AddTranslationUnit(TranslationUnit translationUnit, ImportSettings settings)`

`SearchResults SearchTranslationUnit(SearchSettings settings, TranslationUnit translationUnit)`

`ImportResult[] AddTranslationUnits(TranslationUnit[] translationUnits, ImportSettings settings)`

Removed unused method `ImportResult[] UpdateTranslationUnitsMasked(TranslationUnit[] translationUnits, bool[] mask)` from `ITranslationMemoryLanguageDirection`, `ServerBasedTranslationMemoryLanguageDirection`,`FileBasedTranslationMemoryLanguageDirection`.

Based on the `TranslationMemorySettings.MaximumContextSegments` settings, providers will be able get more than one segment context.

`AISearchParams` - Fragment ContextSegmentPair with Document ContextSegmentPairs so providers can ask for more context

`AISettings` - Added `MaxContextSegments` for the maximum number of context segments

### Trados Cloud API Changes

Removed obsolete `ActiveUserId` property

### Verification API Changes

Removed obsolete interfaces and classes :

`ISerializeVerifierProfile`

`ITermVerifierContext`

`ITermVerifierSettingsContext`

`ITermVerifierSettingsContextAware`

`SerializeProfileVerifierAttribute`

`TermPicklistField`

### Desktop Integration API Changes

Removed obsolete methods and properties:

`AbstractViewControllerAction.ViewId`

`TargetSelection.Replace(IAbstractMarkupData markupData, string operationName)`

### Project API Changes

Added `TermbaseServerUri` property to `ServerTermbase` class and removed it from `TermbaseConfiguration` class.

### Desktop Platform Controls API Changes

Removed obsoleted classes and interfaces related to the embedded web browser control:

`WebBrowserBindableSourceBehaviour`

`EmbeddedBrowserCookieManager`

`ICookieManager`

`INativeBrowserAPI`

`NatvieBrowserAPI`

### Desktop Platform Styles API Changes

We replaced the following styles and resource dictionaries 

`General.xaml`

`ContextMenuResources.xaml`

`ManagerColorResources.xaml`

with 

`./Theming/Default.xaml`

`./Theming/HighContrast.xaml`

> [!NOTE]
>
>Do not manually add Default.xaml or HighContrast.xaml ResourceDictionaries in custom or user controls, doing so will break dynamic theming.

ResourceDictionaries from the Theming folder are loaded automatically by Trados Studio according to the OS theme. The corresponding dictionary is loaded as an application-wide resource.

### Filter Framework API Changes
	
The following methods are deprecated and no longer used:

`IFileTypeDefinition.BuildPreviewControl`

`IFileTypeDefinition.BuildPreviewApplication`

`IFileTypeComponentBuilder.BuildPreviewControl`

`IFileTypeComponentBuilder.BuildPreviewApplication`

Third-party file types need to supply PreviewSets containing IDs from the list described below, corresponding to preview UIs that are defined in Studio. Using preview UIs that are not defined in Studio is not yet supported.

| Preview ID            | UI Description                                                                                                 |
| --------------------- | -------------------------------------------------------------------------------------------------------------- |
| HtmlSingleFilePreview | Renders an HTML file. Can be used to preview the source or the target individually.                            |
| HtmlSideBySidePreview | Renders the source and target in a side-by-side layout. The content must be HTML.                              |
| ExternalPreview       | Opens the preview file in an external application (e.g. Word, Photoshop), based on the preview file extension. |
| ExternalHtml          | Opens the preview file in the default browser.                                                                 |


### Working with BCMs
The BCM-related classes have been moved from Sdl.LanguagePlatform.TranslationMemoryApi to TradosStudio.BcmLite:
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

As part of the separation of MultiTerm from Trados Studio, the legacy assembly `Sdl.Multiterm.TMO.Interop.dll` **has been removed from the Trados Studio installation folder as of Trados Studio 2024 SR1**. Any integration or plugin referencing TMO interop must be updated: **the endorsed approach for terminology-related integrations moving forward is the `TerminologyProviderManager` singleton.**

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
> [!NOTE]
>
> Notice the change of the URI scheme from SDLTB to TTB (ttb.file:///).

> [!NOTE]
>
> Replace any references to `Sdl.Multiterm.TMO.Interop.dll` with the modern `TerminologyProviderManager` API. This ensures compatibility with Trados Studio 2026 Release and future releases, and aligns with Trados ongoing architectural updates.

We added the `IStudioTermbase` interface that extends the file based terminology provider with the following functionality :

- `GetEntriesIDs()` - to get the list of all entry IDs in the termbase
- `AddEntry(Entry entry)` - to add a new entry to the termbase
- `UpdateEntry(Entry entry)` - to update an existing entry in the termbase
- `GetEntries(int lastSeenId, int pageSize)` - to get a list of entries in the termbase, starting from the last seen ID, with a specified page size. This allows for efficient paging through large termbases.) 
- `Create(CreateTermbaseRequest request)` - to create a new termbase based on the provided request, which includes necessary information such as the termbase name and languages and returns an operation result indicating success or failure of the creation process and the error message in case of failure.

### Trados.Community.Toolkit (formerly SDL.Community.Toolkit)
A new version of the Trados Community Toolkit, version 6.0.2, has been released to support the latest version of Trados Studio 2026 Release.  This includes the following assemblies:

- [Trados.Community.Toolkit.Core](https://www.nuget.org/packages/Trados.Community.Toolkit.Core)
- [Trados.Community.Toolkit.LanguagePlatform](https://www.nuget.org/packages/Trados.Community.Toolkit.LanguagePlatform)
- [Trados.Community.Toolkit.Integration](https://www.nuget.org/packages/Trados.Community.Toolkit.Integration)
- [Trados.Community.Toolkit.FileType](https://www.nuget.org/packages/Trados.Community.Toolkit.FileType)
- [Trados.Community.Toolkit.ProjectAutomation](https://www.nuget.org/packages/Trados.Community.Toolkit.ProjectAutomation)

