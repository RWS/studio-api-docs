Create a Studio AutoSuggest provider
====
Trados Studio Integration API provides support for third-party developers to add specific functionalities for a custom AutoSuggest provider.

Creating a Studio AutoSuggest provider
----
In order to create a Trados Studio AutoSuggest provider, a third-party developer will require the following steps:

* Create an initialization class which implements the `AbstractAutoSuggestProvider` abstract class.
* Overrite the the abstract methods of the `AbstractAutoSuggestProvider` base class to implement your custom provider.
* Decorate the initialization class with the `AutoSuggestProviderAttribute` attribute

Creating a Studio AutoSuggest provider Sample