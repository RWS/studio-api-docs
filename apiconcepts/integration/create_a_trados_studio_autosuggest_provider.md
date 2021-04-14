Create a <Var:ProductName> AutoSuggest provider
====
<Var:ProductName> Integration API provides support for third-party developers to add specific functionalities for a custom AutoSuggest provider.

Creating a <Var:ProductName> AutoSuggest provider
----
In order to create a <Var:ProductName> AutoSuggest provider, a third-party developer will require the following steps:

* Create an initialization class which implements the [AbstractAutoSuggestProvider](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.AutoSuggest.AbstractAutoSuggestProvider.yml) abstract class.
* Overrite the the abstract methods of the  [AbstractAutoSuggestProvider](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.AutoSuggest.AbstractAutoSuggestProvider.yml) base class to implement your custom provider.
* Decorate the initialization class with the [AutoSuggestProviderAttribute](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Extensions.AutoSuggestProviderAttribute.yml) attribute

[Creating a <Var:ProductName> AutoSuggest provider Sample](trados_studio_autosuggest_provider.md)