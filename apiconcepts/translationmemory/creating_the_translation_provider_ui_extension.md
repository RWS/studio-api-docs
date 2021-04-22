Creating the Translation Provider UI Extension
=====
This section explains how to create a translation provider UI extension, which allows applications such as <Var:ProductName> to browse for translation providers, edit translation provider settings, display translation providers in a unified way and prompt the user for translation provider credentials.

Overview
----
The translation provider UI component is a plug-in framework extension that allows applications such as <Var:ProductName> to browse for translation providers, edit translation provider settings, display translation providers in a unified way and prompt the user for translation provider credentials.

The translation provider UI component is essentially optional, depending on where the translation provider is meant to be used. A fully functional <Var:ProductName> translation provider implementation needs a translation provider UI component, but for other sceanrios it is perfectly valid to just implement a translation provider factory, for instance when the translation provider will be used in a server scenario only.

The translation provider UI component has to implement the [ITranslationProviderWinFormsUI](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml) interface.


<img style="display:block; " src="images/TranslationProviderWinFormsUI.png"/>

Registering the Extension
---
In order for the translation provider UI component to become available for use by host applications such as <Var:ProductName>, it has to be marked up with a [TranslationProviderWinFormsUiAttribute](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderWinFormsUiAttribute.yml). This is a plug-in framework extension attribute that will be extracted into the plug-in manifest.

The Name, Description and Icon values of the extension attribute are for information only and are not being used in <Var:ProductName> at the moment.

Browsing for Translation Providers
----
One of the responsibilities of the translation provider UI component is to allow the user to select translation providers through a user interface. The [Browse](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderWinFormsUI_Browse_System_Windows_Forms_IWin32Window_Sdl_LanguagePlatform_Core_LanguagePair___Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderCredentialStore_) method is called by <Var:ProductName> when the user selects the corresponding *"Add -> Translation provider X..."* menu item in the translation providers list.

Editing Translation Provider Settings
----
After the user has added a translation provider to the translation providers list in <Var:ProductName>, the UI component can also allow its changing properties or settings. If the [SupportsEditing](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderWinFormsUI_SupportsEditing) property returns True, a *"Settings..."* button will become available when a translation provider supported by the UI component is selected. When clicked, <Var:ProductName> calls the [Edit](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderWinFormsUI_Edit_System_Windows_Forms_IWin32Window_Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProvider_Sdl_LanguagePlatform_Core_LanguagePair___Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderCredentialStore_) method.

Displaying Translation Providers
----
<Var:ProductName> persists the translation provider list by persisting the Uri and state information. In order to display the list of translation providers in the user interface, <Var:ProductName> does not necessarily instantiate all the translation providers. For this reason, the UI component also needs to be able to provide display information, given a certain translation provider Uri and state. This way, the translation provider UI component has the possibility of generating this information in a more lightweight way then by instantiating the translation provider itself, which might for instance involve connecting to a server, etc.

Prompting for User Credentials
-----
The final responsibility of the Translation provider UI component is asking the user for credentials when these are required. <Var:ProductName> does not necessarily persist credentials stored in the credential store ([ITranslationProviderCredentialStore](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderCredentialStore.yml)), which means that there will be situations where <Var:ProductName> needs to prompt the user for credentials. In order to do this,  <Var:ProductName> calls the appropriate translation provider UI component, which in turn can show a log-on user interface that is appropriate for that particular translation provider and subsequently add these credentials to the translation provider credential store so they become available to the translation provider factory to instantiate the translation provider and perform the necessary authentication.

See Also
-------------
[Controlling the Plug-in User Interface](controlling_the_plugin_user_interface.md)