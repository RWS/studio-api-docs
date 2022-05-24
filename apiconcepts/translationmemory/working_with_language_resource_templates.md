Working with Language Resource Templates
======


This section describes how to work with language resource templates in order to centralize the management of server-based translation memory language resources.

Overview
-----
Just like any other translation memory, server-based translation memories support adding custom language resources. For more information, see [Working with Language Resources](working_with_language_resources.md).

When managing a large number of translation memories it becomes tedious to have to manage the language resources of each translation memory individually. For this reason, instead of defining language resources for every translation memory individually, server-based translation memories can inherit their language resources from a language resources template, which is essentially a named collection of language resources. Any change to the language resources template is automatically propagated to all the translation memories that are linked to the language resources template.

Language resources templates are represented by the [ServerBasedLanguageResourcesTemplate](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml) class. In order to create a language resources template, create a new [ServerBasedLanguageResourcesTemplate](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml) object, subsequently set the [Name](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedLanguageResourcesTemplate_Name) property and add language resource bundles to the [LanguageResourceBundles](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedLanguageResourcesTemplate_LanguageResourceBundles) collection. A language resource bundle is a set of language resources for a specific language. Finally call [Save](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedLanguageResourcesTemplate.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedLanguageResourcesTemplate_Save) in order to save the language resources template.

To associate a server-based translation memory with a language resources template, simply set the [LanguageResourcesTemplate](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedTranslationMemory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedTranslationMemory_LanguageResourcesTemplate) property of the translation memory and call [Save](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedTranslationMemory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedTranslationMemory_Save) to save the changes. You can set the language resources template for a new translation memory or on an existing translation memory.


<img style="display:block; " src="images/cd-LanguageResourceTemplates.jpg"/>