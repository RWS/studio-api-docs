Creating the Translation Provider
======
This section explains how to create a class that implements the ITranslationProvider interface, which forms the main part of a custom translation provider plug-in implementation.

Overview
-----
The translation provider component represents a translation engine that provides functionality to translate segments. A translation provider is a class that implements ITranslationProvider. This functionality of this class can be categorized as follows:

* **Translation method**: The TranslationMethod property specifies which translation engine is used for generating translation results. Some search options in Trados Studio 2017 can determine whether a translation provider is called or not. This means that if a TM result already exists Trados Studio 2017 may no longer looks up for suggestions in the MT provider.
* **Supported features**: The translation provider class also has to indicate which functionality it supports. There are a number of "Supports..." properties, which indicate which features the translation provider supports. Again, these values are used by Trados Studio 2017 to determine in which scenarios to use the translation provider and how to use it. See Supported Features.
* **Language directions**: The translation provider can indicate which language directions it supports, and allows retrieving a language direction (ITranslationProviderLanguageDirection) using the GetLanguageDirection method. See Language Directions.
* **Serializing and loading state**: When a translation provider is added to an Trados Studio 2017 project, two pieces of information are serialized: the URI (Uri) and additional state information. See Serializing and Loading State.
A translation provider can support one or more language directions, i.e. source-to-target language combinations. The functionality for a specific language direction is provided by a class that implements ITranslationProviderLanguageDirection.

<img style="display:block; " src="images/TranslationProvider.png"/>

Supported Features
-----
For more information on each of the properties that indicate whether the translation provider supports certain functionality or content, see ITranslationProvider.

Language Directions
-----
A translation provider language direction is defined by a source and target language, both of which should be a region-qualified culture. The interface has two method related to language directions:

* SupportsLanguageDirection: This method checks whether the translation provider supports the specified region-qualified source-target language combination.
* GetLanguageDirection: If the translation provider supports a given language direction, a ITranslationProviderLanguageDirection can be retrieved using this method. The ITranslationProviderLanguageDirection contains language-direction specific methods for performing translation unit lookups and updates. Depending on which features are supported by the translation provider, some or all of these methods can be used.

Serializing and Loading State
-----
When a translation provider is added to an SDL Trados Studio 2017 project, two pieces of information are serialized: the URI and the additional state. The additional state can take the form of additional settings required by the translation provider.

Serialization of state is done using the SerializeState method. The translation provider implementation is free to determine the format that it serializes its state in. SDL Trados Studio 2017 will only store this state information in the project. Also, the state information is included in packages, so it is not recommended to include sensitive information like credentials in there. There is a seperate mechanism for credentials and other user specific settings. For more details on this mechanism, refer to the Authentication section in Creating the Translation Provider Factory.