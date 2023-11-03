Release Notes for <Var:ProductNameWithEdition> SR2
===================

# Added API licensing support for subscriptions
When creating standalone apps, you can now use Project automation API calls that need a license even if <Var:ProductName> is licensed through a subscription. This expands the list of supported scenarios where <Var:ProductName> is licensed through a perpetual license, or through a network server license.

> [!NOTE]
>
> Starting with <Var:ProductNameWithEdition> SR2, when developing a standalone application using Project Automation APIs that require a license, make sure to call `LicenseManager.ReleaseLicense()` before the application exits. See [Project Automation Overview](../projectautomation/overview.md) for more details.

# Terminology Provider
The changes are included in `Sdl.Terminology.TerminologyProvider.Core`.

## Classes
### Removed clsses
* AbstractTerminologyProvider

### Updated classes

#### [Definition](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Definition.yml) 
* No longer implements interface `IDefinition`.
* Constructor definition changed. The new constructor definition: `public Definition(IEnumerable<DescriptiveField> fields, IEnumerable<DefinitionLanguage> languages)`  

#### [DefinitionLanguage](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.DefinitionLanguage.yml)
* No longer implements interface `IDefinitionLanguage`.
* The type of property `Local` was changed to [CultureCode](../../api/core/Sdl.Core.Globalization.CultureCode.yml)

#### [DescriptiveField](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.DescriptiveField.yml)
* No longer implements interface `IDescriptiveField`.

#### [Entry](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Entry.yml)
* No longer implements interface `IEntry`.
* The type of property `Languages` was changed to *IList<[EntryLanguage](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryLanguage.yml)>*.
* The type of property `Transactions` was changed to *IList<[EntryTransaction](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryTransaction.yml)>*.

#### [EntryEventArgs](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryEventArgs.yml)
* Constructor definition changed to `EntryEventArgs(Entry entry)`
* The type of property `Entry` changed to [Entry](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Entry.yml)

#### [EntryField](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryField.yml)
* No longer implements interface `IEntryField`.
* Type of property `Fields` was changed to *IList<[EntryField](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryField.yml)>*. 

#### [EntryLanguage](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryLanguage.yml)
* No longer implements interface `IEntryLanguage`.
* Type of property `Fields` was changed to *IList<[EntryField](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryField.yml)>*
* Type of property `Locale` was changed to [CultureCode](../../api/core/Sdl.Core.Globalization.CultureCode.yml).
* Type of property `ParentEntry` was changed to [Entry](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Entry.yml).
* Type of property `Terms` was changed to *IList<[EntryTerm](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryTerm.yml)>*

#### [EntryTerm](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryTerm.yml)
* No longer implements interface `IEntryTerm`.
* Type of property `Fields` was changed to *IList<[EntryField](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryField.yml)>*. 
* Type of property `ParentLanguage` was changed to [EntryLanguage](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryLanguage.yml)
* Type of property `Transactions` was changed to *IList<[EntryTransaction](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryTransaction.yml)>*

#### [EntryTransaction](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.EntryTransaction.yml)
* No longer implements interface `IEntryTransaction`.

#### [MarkupPosition](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.MarkupPosition.yml)
* No longer implements interface `IMarkupPosition`.

#### [SearchMarkupResult](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.SearchMarkupResult.yml)
* No longer implements interface `ISearchMarkupResult`.
* No longer implements interface `ISearchResult`.
* Type of property `Positions` was changed to *IList<[MarkupPosition](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.MarkupPosition.yml)>*

#### [SearchMarkupResultWithTermbaseName](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.SearchMarkupResultWithTermbaseName.yml)
* No longer implements interface `ISearchResultWithTermbaseName`.
* No longer implements interface `ISearchResult`. 

#### [SearchResult](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.SearchResult.yml)
* No longer implements interface `ISearchResult`.

#### [SearchResultWithTermbaseName](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.SearchResultWithTermbaseName.yml)
* No longer implements interface `ISearchResultWithTermbaseName`.
* No longer implements interface `ISearchResult`.

#### [TerminologyProviderManager](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyProviderManager.yml)
* Implements interface `ITerminologyProviderManager`
* New method added `RemoveTerminologyProvider(Uri)` used to remove a terminology provider based on the provider [Uri](https://learn.microsoft.com/dotnet/api/system.uri)

#### [TerminologyProviderStatus](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyProviderStatus.yml)
* No longer implements interface `ITerminologyProviderStatus`


### Added classes
* [FilterDefinition](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.FilterDefinition.yml) which can be used to define the terminology provider filter

## Interfaces

### Removed interfaces
* IDefinition
* IDefinitionLanguage
* IDescriptiveField
* IEntry
* IEntryField
* IEntryLanguage
* IEntryTerm
* IEntryTransaction
* IMarkupPosition
* ISearchMarkupResult
* ISearchResult
* ISearchResultWithTermbaseName
* IServerBasedTerminologyProvider
* ITerminologyProviderStatus

### Updated interfaces

#### [IConnectionAwareTerminologyProvider](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.IConnectionAwareTerminologyProvider.yml)
* Type of property `Status` was changed to [TerminologyProviderStatus](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyProviderStatus.yml)

#### [ILanguage](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ILanguage.yml)
* Type of property `Locale` was changed to [CultureCode](../../api/core/Sdl.Core.Globalization.CultureCode.yml).

#### [ITerminologyMarkupProvider](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyMarkupProvider.yml)
* Is no longer extended by interfaces `IConnectionAwareTerminologyProvider`, `ITerminologyProvider` and `IDisposable`
* Return type for method `SearchAndMarkup(string text, ILanguage source, ILanguage destination, int maxResultsCount, SearchMode mode, bool targetRequired)` was changed to *IList<[SearchMarkupResult](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.SearchMarkupResult.yml)>*

#### [ITerminologyProvider](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProvider.yml)
* Added property `ActiveFilter { get; set; }` used to manage the active filter definition, it returns [FilterDefinition](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.FilterDefinition.yml).
* Added property `IsInitialized { get; }` used to track where the provider has been initialized, it returns a `boolean` value.
* Added method `GetFilters()` to get all the filters definitions available for the terminology provider, it returns *IList<[FilterDefinition](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.FilterDefinition.yml)>*.
* Added method `Initialize()` to be used for provider initialization, it returns a `boolean` value.
* Added method `Initialize(TerminologyProviderCredential credential)` to be used to initialize providers with credentials, it returns a `boolean` value.
* Added method `IsProviderUpToDate()` to be used to check if the provider information is up-to-date, it returns a `boolean` value.
* Added method `Uninitialize()` to be used to uninitialize the terminology provider, it returns a `boolean` value.
* Type of property `Definition` was changed to [Definition](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Definition.yml). 
* Type returned by method `GetEntry(int)` was changed to [Entry](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Entry.yml).
* Type returned by method `GetEntry(int id, IEnumerable<ILanguage> languages)` was changed to [Entry](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Entry.yml). 
* Type returned by method `Search(string, ILanguage, ILanguage, int, SearchMode, bool)` was changed to *IList<[SearchResult](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.SearchResult.yml)>*. 

#### [ITerminologyProviderViewerWinFormsUI](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderViewerWinFormsUI.yml)
* Added property `CanAddTerm { get; }` that returns a `boolean` value, true if this component supports adding a term; otherwise, false.
* Added property `IsEditing { get; }` used to indicate whether the component TerminologyProviderViewerWinFormsUI is in edit mode, it returns a `boolean` value.
* Added method `CancelTerm()` used to cancel the current term editing operation.
* Added method `SaveTerm()` used to save the current state of a term.
* Type of property `SelectedTerm` was changed to [Entry](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Entry.yml).
* Type of parameter `term` of method `AddAndEditTerm` was changed to [Entry](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Entry.yml). The new method declaration is `void AddAndEditTerm(Entry term, string source, string target)`.
* Type of parameter `term` of method `EditTerm` was changed to [Entry](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Entry.yml). The new method declaration is `void EditTerm(Entry term)`.
* Type of parameters `source` and `target` of method `Initialize` was changed to [CultureCode](../../api/core/Sdl.Core.Globalization.CultureCode.yml). The new method declaration is `void Initialize(ITerminologyProvider terminologyProvider, CultureCode source, CultureCode target)`.
* Type of parameter `entry` of method `JumpToTerm` was changed to [Entry](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.Entry.yml). The new method declaration is `void JumpToTerm(Entry entry)`.

#### [ITerminologyProviderWinFormsUI](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderWinFormsUI.yml)
* Removed property `SupportsEditing`.
* Removed method `bool Edit(IWin32Window owner, ITerminologyProvider terminologyProvider)`.Type

### Added interfaces

#### [ITerminologyProviderManager](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderManager.yml)
* This interface represents a mechanism responsible for creating and getting available terminology providers.
* Interface methods: 
   - [GetTerminologyProvider(Uri)](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderManager.yml#Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderManager_GetTerminologyProvider_System_Uri_): get an instance of a terminology provider, specified by its uri.
   - [GetTerminologyProvider(Uri, ITerminologyProviderCredentialStore)](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderManager.yml##Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderManager_GetTerminologyProvider_System_Uri_Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderCredentialStore_): get an instance of a terminology provider, specified by its uri and the provider credentials.
   - [GetTerminologyProviderViewerWinFormsUI(Uri)](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderManager.yml#Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderManager_GetTerminologyProviderViewerWinFormsUI_System_Uri_): returns the terminology provider viewer win forms UI object specific for the input URI.
   - [GetTerminologyProviderViewerWinFormsUIs()](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderManager.yml#Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderManager_GetTerminologyProviderViewerWinFormsUIs): returns the list of available terminology provider viewer win forms UI.
   - [GetTerminologyProviderWinFormsUI(Uri)](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderManager.yml#Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderManager_GetTerminologyProviderWinFormsUI_System_Uri_): returns the win forms UI for the specified terminology provider.
   - [GetTerminologyProviderWinFormsUIs()](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderManager.yml#Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderManager_GetTerminologyProviderWinFormsUIs): returns the list of all available terminology providers that have implementations for win forms UI.
   - [RemoveTerminologyProvider(Uri)](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderManager.yml#Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderManager_RemoveTerminologyProvider_System_Uri_): Remove a terminology provider based on uri.


#### [ITerminologyProviderWinFormsUIWithCreate](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderWinFormsUIWithCreate.yml)
* Adds the capability to create a `ITerminologyProvider`.
* Extends interface [ITerminologyProviderWinFormsUI](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderWinFormsUI.yml).
* Has method [Create()](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderWinFormsUIWithCreate.yml#Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderWinFormsUIWithCreate_Create) that allows the creation of an `ITerminologyProvider` from the UI. It returns a [ITerminologyProvider](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProvider.yml)

#### [ITerminologyProviderWinFormsUIWithEdit](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderWinFormsUIWithEdit.yml)
* Adds the capability to edit settings.
* Extends interface [ITerminologyProviderWinFormsUI](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderWinFormsUI.yml).
* Has method [Edit(IWin32Window owner, ITerminologyProvider terminologyProvider)](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderWinFormsUIWithEdit.yml#Sdl_Terminology_TerminologyProvider_Core_ITerminologyProviderWinFormsUIWithEdit_Edit_System_Windows_Forms_IWin32Window_Sdl_Terminology_TerminologyProvider_Core_ITerminologyProvider_) used to display a dialog to interactively change any of the terminology provider settings.

## Enums

### Added enums

#### [TerminologyImportType](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyImportType.yml)
* Determines the import format.

[TerminologyImportType](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyImportType.yml) fields

|Name|Description                                                                                      |
|----|-------------------------------------------------------------------------------------------------|
|MTF |Use MFT XML schema: https://developers.rws.com/multiterm-api-docs/apiconcepts/MTF/xsd_schema.html|