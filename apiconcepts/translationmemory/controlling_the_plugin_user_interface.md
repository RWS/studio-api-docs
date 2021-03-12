Controlling the Plug-in User Interface
======
The translation provider plug-in template contains a component called MyTranslationProviderWinFormsUI, which we re-name to ListTranslationProviderWinFormsUI for our implementation. This component is used to:

* Control the appearance of the plug-in when it is displayed in Trados Studio 2017, e.g. the plug-in name, icon, tooltip
* Raise the plug-in user interface
* Obtain credentials from the user, which is not required for our sample implementation


The class is preceded by the following declaration, which defines it as an extension class, which will be referenced and declared to the Trados Studio 2017 application through the plug-in manifest *.xml file (see also Building the Plug-in).

This class implements the ITranslationProviderWinFormsUI interface. Below you see how we use the members of this interface in our implementation.

Browse for the Plug-in
-------
The UI extension allows users to browse for and select translation providers in a unified way. This is done by the user when opening a document for translation or when adding translation providers to a project through the **Add** button in the UI of Trados Studio 2017 as illustrated in the screenshot below:

<img style="display:block; " src="images/BrowsePlugin.jpg"/>

Some translation providers do not require a user interface for configuring settings. In our implementation, however, a form is used to enter the required settings, which we raise through the Browse method of the interface:

Translation provider plug-ins that implement a user interface for configuring settings will usually allow users to change those settings later. For example, users might want to change the plug-in settings while translating, as they discover that a different list file is more suitable for the text that they have selected. For this, the UI of Trados Studio 2017 implements a **Settings** button, which can be enabled or disabled through your plug-in (see screenshot below).

<img style="display:block; " src="images/ProviderSettingsButton.jpg"/>

For our sample plug-in it makes sense to enable this button and allow for any settings to be changed. To do this, we set the SupportsEditing property to return True:

Consequently, the Edit method is then used for the same purpose as the Browse method, i.e. to raise the plug-in user interface:

The plug-in user interface of our implementation will look as shown below:

<img style="display:block; " src="images/PluginGui.jpg"/>

User Authentication
-----

Through the GetCredentialsFromUser method of the interface we can retrieve user credentials that may be required for connecting to the translation provider. For example, when you connect to a server TM or a Web-based translation service, a user name and login may be required. Very often, you may want to store the login information, so that users are not required have to enter the name and password every time that they connect to the provider.

In our simple implementation, however, we only access a text file, which means that no user authentication is necessary. We can therefore have this method return always True:


Display the Plug-in Info
-----
The GetDisplayInfo is used to display information such as the plug-in name, icon, etc. in the user interface of SDL Trados Studio 2017. This information is stored in the resources file (see The Resources File). Within this method we first create a TranslationProviderDisplayInfo object, and then set the various properties, i.e. plug-in display name, the tooltip text, and the translation provider icon. This information will then be shown in the application as illustrated below:


<img style="display:block; " src="images/PluginResourcesInAction.jpg"/>

The SearchResultImage property sets the image that is shown in the **Translation Results** or **Concordance** window of Trados Studio 2017, which helps users ascertain more quickly by which provider a translation solution has been suggested (see screenshot below):

<img style="display:block; " src="images/PngForShowingResults.jpg"/>

Putting it All Together
-----
The complete component should look as shown below: