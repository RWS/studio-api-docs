# Creating Translation Provider Plug-ins

This section explains how to create a translation provider plug-in for Var:ProductName. A plug-in lets Var:ProductName connect to machine translation engines, translation memory systems, and other translation services.

## Overview

A translation provider translates segments, such as sentences, from a source language into a target language. A provider can support many source and target language pairs.

Examples include RWS Translation Memory, which looks up segments in a file-based or server-based translation memory database ([FileBasedTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.FileBasedTranslationMemory.yml) or [ServerBasedTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedTranslationMemory.yml)), and the RWS Automated Translation Provider, which uses RWS's automated translation engine.

Var:ProductName uses translation providers in these tasks:

- Project preparation, including the Analyze, Populate Project Translation Memory, and Pre-translate batch tasks.
- Interactive translation, when users look up segments in a document in the Var:ProductName editor.
- Project finalization, during the TM Update task.

Var:ProductName includes built-in translation provider implementations. It also provides a plug-in mechanism that lets third parties create their own translation provider plug-ins. To implement a translation provider plug-in, create these classes:

1. **Translation Provider**: Represents the multilingual provider and returns a translation provider language direction for each source-target language pair. This class must implement the [ITranslationProvider](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProvider.yml) interface.
2. **Translation Provider Language Direction**: Exposes translation functionality for a specific source-target language pair.
3. **Translation Provider Factory Extension**: Creates provider instances from a URI. Var:ProductName uses this class to load providers from URIs stored in translation projects. Each provider should define a URI scheme that uniquely identifies it.
4. **Translation Provider UI Extension**: Supplies the user interface that lets Var:ProductName browse for providers, edit provider settings, and present providers consistently. Var:ProductName selects the appropriate UI extension based on the provider URI.

The following diagram shows how Var:ProductName uses a translation provider plug-in:

<img style="display:block; " src="images/Translation Provider Plug-in.png"/>

The following topics describe the implementation steps in detail:

1. [Creating the Translation Provider](creating_the_translation_provider.md)
2. [Creating the Translation Provider Factory](creating_the_translation_provider_factory.md)
3. [Creating the Translation Provider UI Extension](creating_the_translation_provider_ui_extension.md)
