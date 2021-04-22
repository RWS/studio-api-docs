Controlling the Plug-in User Interface
======
The translation provider plug-in template contains a component called `MyTranslationProviderWinFormsUI`, which we re-name to `ListTranslationProviderWinFormsUI` for our implementation. This component is used to:

* Control the appearance of the plug-in when it is displayed in <Var:ProductName>, e.g. the plug-in name, icon, tooltip
* Raise the plug-in user interface
* Obtain credentials from the user, which is not required for our sample implementation


The class is preceded by the following declaration, which defines it as an extension class, which will be referenced and declared to the <Var:ProductName> application through the plug-in manifest *.xml file (see also [Building the Plug-in](building_the_plugin.md)).
# [C#](#tab/tabid-1)
```cs
[TranslationProviderWinFormsUi(
    Id = "ListTranslationProviderWinFormsUI",
    Name = "ListTranslationProviderWinFormsUI",
    Description = "ListTranslationProviderWinFormsUI")]
```
***

This class implements the [ITranslationProviderWinFormsUI](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml) interface. Below you see how we use the members of this interface in our implementation.

Browse for the Plug-in
-------
The UI extension allows users to browse for and select translation providers in a unified way. This is done by the user when opening a document for translation or when adding translation providers to a project through the **Add** button in the UI of <Var:ProductName> as illustrated in the screenshot below:

<img style="display:block; " src="images/BrowsePlugin.jpg"/>

Some translation providers do not require a user interface for configuring settings. In our implementation, however, a form is used to enter the required settings, which we raise through the [Browse](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderWinFormsUI_Browse_System_Windows_Forms_IWin32Window_Sdl_LanguagePlatform_Core_LanguagePair___Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderCredentialStore_) method of the interface:

# [C#](#tab/tabid-2)
```cs
public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
{
    ListProviderConfDialog dialog = new ListProviderConfDialog(new ListTranslationOptions());
    if (dialog.ShowDialog(owner) == DialogResult.OK)
    {
        ListTranslationProvider testProvider = new ListTranslationProvider(dialog.Options);
        return new ITranslationProvider[] { testProvider };   
    }
    return null;
}
```
***

Translation provider plug-ins that implement a user interface for configuring settings will usually allow users to change those settings later. For example, users might want to change the plug-in settings while translating, as they discover that a different list file is more suitable for the text that they have selected. For this, the UI of <Var:ProductName> implements a **Settings** button, which can be enabled or disabled through your plug-in (see screenshot below).

<img style="display:block; " src="images/ProviderSettingsButton.jpg"/>

For our sample plug-in it makes sense to enable this button and allow for any settings to be changed. To do this, we set the [SupportsEditing](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderWinFormsUI_SupportsEditing) property to return `True`:
# [C#](#tab/tabid-3)
```cs
public bool SupportsEditing
{
    get { return true; }
}
```
***
Consequently, the [Edit](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderWinFormsUI_Edit_System_Windows_Forms_IWin32Window_Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProvider_Sdl_LanguagePlatform_Core_LanguagePair___Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderCredentialStore_) method is then used for the same purpose as the [Browse](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderWinFormsUI_Browse_System_Windows_Forms_IWin32Window_Sdl_LanguagePlatform_Core_LanguagePair___Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderCredentialStore_) method, i.e. to raise the plug-in user interface:
# [C#](#tab/tabid-4)
```cs
public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
{
    ListTranslationProvider editProvider = translationProvider as ListTranslationProvider;
    if (editProvider == null)
    {
        return false;
    }

    ListProviderConfDialog dialog = new ListProviderConfDialog(editProvider.Options);
    if (dialog.ShowDialog(owner) == DialogResult.OK)
    {
        editProvider.Options = dialog.Options;
        return true;
    }

    return false;
}
```
***

The plug-in user interface of our implementation will look as shown below:

<img style="display:block; " src="images/PluginGui.jpg"/>

User Authentication
-----

Through the [GetCredentialsFromUser](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderWinFormsUI_GetCredentialsFromUser_System_Windows_Forms_IWin32Window_System_Uri_System_String_Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderCredentialStore_) method of the interface we can retrieve user credentials that may be required for connecting to the translation provider. For example, when you connect to a server TM or a Web-based translation service, a user name and login may be required. Very often, you may want to store the login information, so that users are not required have to enter the name and password every time that they connect to the provider.

In our simple implementation, however, we only access a text file, which means that no user authentication is necessary. We can therefore have this method return always True:
# [C#](#tab/tabid-5)
```cs
public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
{            
    return true;
}
```
***


Display the Plug-in Info
-----
The [GetDisplayInfo](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderWinFormsUI.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderWinFormsUI_GetDisplayInfo_System_Uri_System_String_) is used to display information such as the plug-in name, icon, etc. in the user interface of <Var:ProductName>. This information is stored in the resources file (see [The Resources File](the_resources_file.md)). Within this method we first create a [TranslationProviderDisplayInfo](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderDisplayInfo.yml) object, and then set the various properties, i.e. plug-in display name, the tooltip text, and the translation provider icon. This information will then be shown in the application as illustrated below:


<img style="display:block; " src="images/PluginResourcesInAction.jpg"/>

The [SearchResultImage](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderDisplayInfo.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderDisplayInfo_SearchResultImage) property sets the image that is shown in the **Translation Results** or **Concordance** window of <Var:ProductName>, which helps users ascertain more quickly by which provider a translation solution has been suggested (see screenshot below):

<img style="display:block; " src="images/PngForShowingResults.jpg"/>

# [C#](#tab/tabid-6)
```cs
public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
{
    TranslationProviderDisplayInfo info = new TranslationProviderDisplayInfo();
    info.Name = PluginResources.Plugin_NiceName;            
    info.TranslationProviderIcon = PluginResources.band_aid_icon;
    info.TooltipText = PluginResources.Plugin_Tooltip;

    info.SearchResultImage = PluginResources.band_aid_symbol;

    return info;
}
```
***

Putting it All Together
-----
The complete component should look as shown below:
# [C#](#tab/tabid-7)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace Sdl.Sdk.LanguagePlatform.Samples.ListProvider
{
    #region "Declaration"
    [TranslationProviderWinFormsUi(
        Id = "ListTranslationProviderWinFormsUI",
        Name = "ListTranslationProviderWinFormsUI",
        Description = "ListTranslationProviderWinFormsUI")]
    #endregion
    public class ListTranslationProviderWinFormsUI : ITranslationProviderWinFormsUI
    {
        #region ITranslationProviderWinFormsUI Members


        /// <summary>
        /// Show the plug-in settings form when the user is adding the translation provider plug-in
        /// through the GUI of SDL Trados Studio
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="languagePairs"></param>
        /// <param name="credentialStore"></param>
        /// <returns></returns>
        #region "Browse"
        public ITranslationProvider[] Browse(IWin32Window owner, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            ListProviderConfDialog dialog = new ListProviderConfDialog(new ListTranslationOptions());
            if (dialog.ShowDialog(owner) == DialogResult.OK)
            {
                ListTranslationProvider testProvider = new ListTranslationProvider(dialog.Options);
                return new ITranslationProvider[] { testProvider };   
            }
            return null;
        }
        #endregion



        /// <summary>
        /// Determines whether the plug-in settings can be changed
        /// by displaying the Settings button in SDL Trados Studio.
        /// </summary>
        #region "SupportsEditing"
        public bool SupportsEditing
        {
            get { return true; }
        }
        #endregion

        /// <summary>
        /// If the plug-in settings can be changed by the user,
        /// SDL Trados Studio will display a Settings button.
        /// By clicking this button, users raise the plug-in user interface,
        /// in which they can modify any applicable settings, in our implementation
        /// the delimiter character and the list file name.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="translationProvider"></param>
        /// <param name="languagePairs"></param>
        /// <param name="credentialStore"></param>
        /// <returns></returns>
        #region "Edit"
        public bool Edit(IWin32Window owner, ITranslationProvider translationProvider, LanguagePair[] languagePairs, ITranslationProviderCredentialStore credentialStore)
        {
            ListTranslationProvider editProvider = translationProvider as ListTranslationProvider;
            if (editProvider == null)
            {
                return false;
            }

            ListProviderConfDialog dialog = new ListProviderConfDialog(editProvider.Options);
            if (dialog.ShowDialog(owner) == DialogResult.OK)
            {
                editProvider.Options = dialog.Options;
                return true;
            }

            return false;
        }
        #endregion

        /// <summary>
        /// Can be used in implementations in which a user login is required, e.g.
        /// for connecting to an online translation provider.
        /// In our implementation, however, this is not required, so we simply set
        /// this member to return always True.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="translationProviderUri"></param>
        /// <param name="translationProviderState"></param>
        /// <param name="credentialStore"></param>
        /// <returns></returns>
        #region "GetCredentialsFromUser"
        public bool GetCredentialsFromUser(IWin32Window owner, Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {            
            return true;
        }
        #endregion

        /// <summary>
        /// Used for displaying the plug-in info such as the plug-in name,
        /// tooltip, and icon.
        /// </summary>
        /// <param name="translationProviderUri"></param>
        /// <param name="translationProviderState"></param>
        /// <returns></returns>
        #region "GetDisplayInfo"
        public TranslationProviderDisplayInfo GetDisplayInfo(Uri translationProviderUri, string translationProviderState)
        {
            TranslationProviderDisplayInfo info = new TranslationProviderDisplayInfo();
            info.Name = PluginResources.Plugin_NiceName;            
            info.TranslationProviderIcon = PluginResources.band_aid_icon;
            info.TooltipText = PluginResources.Plugin_Tooltip;

            info.SearchResultImage = PluginResources.band_aid_symbol;

            return info;
        }
        #endregion



        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null)
            {
                throw new ArgumentNullException("URI not supported by the plug-in.");
            }
            return String.Equals(translationProviderUri.Scheme, ListTranslationProvider.ListTranslationProviderScheme, StringComparison.CurrentCultureIgnoreCase);
        }

        public string TypeDescription
        {
            get { return PluginResources.Plugin_Description; }
        }

        public string TypeName
        {
            get { return PluginResources.Plugin_NiceName; }
        }

        #endregion
    }
}
```
***

See Also
--------
[Creating the Translation Provider UI Extension](creating_the_translation_provider_ui_extension.md)

[Implementing the Plug-in User Interface](implementing_the_plugin_user_interface.md)

[The Resources File](the_resources_file.md)