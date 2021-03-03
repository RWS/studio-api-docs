Trados Studio AutoSuggest provider
====
Integration API makes it easy to to create your own AutoSuggest provider. All you need to do is implement the `AbstractAutoSuggestProvider` abstract class, from `Sdl.TranslationStudioAutomation.IntegrationApi.AutoSuggest` namespace and decorate your class with `AutoSuggestProvider` attribute. If you fail to implement `AbstractAutoSuggestProvider`,  an exception will be thrown, but if you omit the `AutoSuggestProviderAttribute`, your plugin will simply be ignored when Trados Studio Starts.

Example
-----
The following example demonstrates how to implement a custom AutoSuggest provider.

The plugin implements the required abstract methods of the AbstractAutoSuggestProvider base class.

* **GetSuggestions** to return the list of the items for auto suggestion list box
* **OnActiveSegmentChanged** to be able to prepare the list of suggested items for the new selected segment
* **OnSettingsChanged** to react to any change of the document settings
* **OnActiveDocumentChanged** to react to changing of the active document