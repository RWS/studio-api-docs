Release Notes for Var:ProductNameWithEdition SR1
===================

# MultiTerm TMO Interop
`Sdl.Multiterm.TMO.Interop.dll` is no longer present in the install folder after the separation from MultiTerm.

# Terminology API
These changes are included in the `Sdl.Terminology.TerminologyProvider.Core` assembly.

[TerminologyUserManagerTokenType](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyUserManagerTokenType.yml) enum is marked as obsolete.

The following classes were marked as obsolete:
* [GenericCredentials](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.GenericCredentials.yml)
* [TerminologyProviderCredential](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyProviderCredential.yml)
* [TerminologyProviderCredentialStore](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyProviderCredentialStore.yml)

The following interfaces were marked as obsolete:
* [ITerminologyProviderCredentialStore](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderCredentialStore.yml)
* [IConnectionAwareTerminologyProvider](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.IConnectionAwareTerminologyProvider.yml)
* [IFileBasedTerminologyProviderDefaultPath](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.IFileBasedTerminologyProviderDefaultPath.yml)
* [IMultipleTerminologyProvider](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.IMultipleTerminologyProvider.yml)
* [ITermbaseMetrics](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITermbaseMetrics.yml)
* [ITerminologyMarkupProvider](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyMarkupProvider.yml)
* [ITerminologyProviderReadOnly](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderReadOnly.yml)

## [ITerminologyProvider](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProvider.yml)

* Method `GetEntry(id, IEnumerable<ILanguage> languages)` was marked as obsolete.
* Method `ITerminologyProvider.Initialize(TerminologyProviderCredential credential)` was marked as obsolete.

## [ITerminologyProviderFactory](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderFactory.yml)

* Method `CreateTerminologyProvider(Uri terminologyProviderUri, ITerminologyProviderCredentialStore credentials)` was marked as obsolete.

## [ITerminologyProviderManager](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderManager.yml)

* Method `GetTerminologyProvider(Uri uri, ITerminologyProviderCredentialStore credentials)` was marked as obsolete.
* Added method `GetTerminologyProvider(Uri uri)` to be used instead of `GetTerminologyProviderManager(Uri uri, ITerminologyProviderCredentialStore credentials)`.

## [ITerminologyProviderWinFormsUI](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.ITerminologyProviderWinFormsUI.yml)

* Method `Browse(IWin32Window owner, ITerminologyProviderCredentialsStore credentialStore)` was marked as obsolete.

## [TerminologyProviderManager](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyProviderManager.yml)

* Property `DefaultTerminologyCredentialStore` was marked as obsolete and will return null.
* Method `GetTerminologyProvider(Uri uri, ITerminologyProviderCredentialStore credentials)` was marked as obsolete.
* Added method `Use GetTerminologyProvider(Uri uri)` to be used instead of `GetTerminologyProvider(Uri uri, ITerminologyProviderCredentialStore credentials)`.

# TranslationMemory API
These changes are included in the `Sdl.LanguagePlatfrom.TranslationMemoryApi` assembly.

The Sdl.LanguagePlatfrom.TranslationMemoryApi assembly now includes an [IAIAssistantProvider](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.AIAssistant.IAIAssistantProvider.yml) interface for plugging AI providers to be used as AI Assistant providers in Studio.

Added [LiteDocument](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.LiteBCM.LiteDocument.yml) and [LiteFragment](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.LiteBCM.LiteFragment.yml) classes used to structure and exchange translation content with AI providers.

## [ISubsegmentTranslationMemoryLanguageDirection](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ISubsegmentTranslationMemoryLanguageDirection.yml)

The method `SupportedSubsegmentMatchTypes()` was marked as obsolete.

# Verification API
These changes are included in the `Sdl.Verification.Api` assembly.

The following classes where marked as obsolete:
* [SerializeProfileVerifierAttribute](../../api/verification/Sdl.Verification.Api.SerializeProfileVerifierAttribute.yml)
* [TermPicklistField](../../api/verification/Sdl.Verification.Api.TermPicklistField.yml)

The following interfaces where marked as obsolete:
* [ISerializeVerifierProfile](../../api/verification/Sdl.Verification.Api.ISerializeVerifierProfile.yml)
* [ITermVerifierContext](../../api/verification/Sdl.Verification.Api.ITermVerifierContext.yml)
* [ITermVerifierSettingsContext](../../api/verification/Sdl.Verification.Api.ITermVerifierSettingsContext.yml)
* [ITermVerifierSettingsContextAware](../../api/verification/Sdl.Verification.Api.ITermVerifierSettingsContextAware.yml)