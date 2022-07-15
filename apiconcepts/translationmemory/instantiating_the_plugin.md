Instantiating the Plug-in
====

A translation provider plug-in has to be properly instantiated, so that it can be used in and displayed in <Var:ProductName>. Instantiation is done by a component that is also provided in the plug-in template, and which is called `MyTranslationProviderFactory`. In our implementation we will rename this component to `ListTranslationProviderFactory`.

The Translation Provider Factory Interface
------
The factory component implements the interface [ITranslationProviderFactory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderFactory.yml). This interface, in turn, consists of three methods. The following sections outline how we implement these methods for our sample plug-in.

The class is preceded by the following declaration, which defines it as an extension class, which will be referenced and declared to the <Var:ProductName> application through the plug-in manifest *.xml file (see also [Building the Plug-in](building_the_plugin.md)).

# [C#](#tab/tabid-1)
```cs
[TranslationProviderFactory(
    Id = "ListTranslationProviderFactory",
    Name = "Delimited list translation provider",
    Description = "Searches in delimited text files.")]
```
***

Create the Translation Provider
------
The [CreateTranslationProvider](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderFactory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderFactory_CreateTranslationProvider_System_Uri_System_String_Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderCredentialStore_): method is used to create the translation provider, which is specified in the translation provider URI. In our implementation we use the string *listprovider*. As this value is used for identification, it needs to be unique. The URI is also used to store any plug-in settings information, which are loaded through the below method. In our implementation this would be the delimiting character and the delimited text file name and path.

# [C#](#tab/tabid-2)
```cs
public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
{
    if (!SupportsTranslationProviderUri(translationProviderUri))
    {
        throw new Exception("Cannot handle URI.");
    }

    ListTranslationProvider tp = new ListTranslationProvider(new ListTranslationOptions(translationProviderUri));

    return tp;
}
```
***

The plug-in URI is stored in an **.sdlproj* or an **.sdltlp* file. These are the file types that contain, among other things, the translation providers used for a translation project and the corresponding settings.
Below you see an example of what a plug-in URI looks like for this sample implementation. The URI is prefixed with *listprovider///* and followed by the plug-in settings parameters, i.e. *delimiter* and *listfile*.

```
listprovider:///?delimiter=;&listfile=C:\temp\sample_list.txt" Enabled="true"
```


Determine Whether the URI is Supported by the Plug-in
------
[SupportsTranslationProviderUri](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderFactory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderFactory_SupportsTranslationProviderUri_System_Uri_) method you should determine whether the provided URI is actually valid and supported by the plug-in as shown in the sample code below:
# [C#](#tab/tabid-4)
```cs
public bool SupportsTranslationProviderUri(Uri translationProviderUri)
{
    if (translationProviderUri == null)
    {
        throw new ArgumentNullException("Translation provider URI not supported.");
    }
    return String.Equals(translationProviderUri.Scheme, ListTranslationProvider.ListTranslationProviderScheme, StringComparison.OrdinalIgnoreCase);
}
```
***

Setting the Plug-in Info
-------

This component is also responsible for setting the plug-in GUI elements, i.e. the name and the plug-in icon. Through the [GetTranslationProviderInfo](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderFactory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderFactory_GetTranslationProviderInfo_System_Uri_System_String_): method you set the plug-in name, which should be shown in the user interface of <Var:ProductName> as well as the translation method.

First, create a [TranslationProviderInfo](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderInfo.yml) object to hold and set the plug-in properties. The string fro the (nice) plug-in name is accessed from the resources file:
# [C#](#tab/tabid-5)
```cs
info.Name = PluginResources.Plugin_NiceName;
```
***

The [TranslationMethod](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMethod.yml) is retrieved from a separate `ListTranslationOptions` class, which we will implement in a later step (see [Storing and Retrieving the Plug-in Settings](storing_and_retrieving_the_plugin_settings.md)). The translation method is used to specify what kind of translations a given plug-in provides, e.g. translation solutions from a translation memory, a machine translation system, etc.
# [C#](#tab/tabid-6)
```cs
info.TranslationMethod = ListTranslationOptions.ProviderTranslationMethod;
```
***

After the specifying these plug-in properties, the info object is returned as shown below:
# [C#](#tab/tabid-7)
```cs
public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
{
    TranslationProviderInfo info = new TranslationProviderInfo();

    #region "TranslationMethod"
    info.TranslationMethod = ListTranslationOptions.ProviderTranslationMethod;
    #endregion

    #region "Name"
    info.Name = PluginResources.Plugin_NiceName;
    #endregion

    return info;
}
```
***

Putting it All Together
------
The complete factory class should look as shown below:
# [C#](#tab/tabid-8)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.Core;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using Sdl.Sdk.LanguagePlatform.Samples.ListProvider;

namespace Sdk.LanguagePlatform.Samples.ListProvider
{
    #region "Declaration"
    [TranslationProviderFactory(
        Id = "ListTranslationProviderFactory",
        Name = "ListTranslationProviderFactory",
        Description = "Searches in delimited text files.")]
    #endregion

    public class ListTranslationProviderFactory : ITranslationProviderFactory
    {
        #region ITranslationProviderFactory Members

        #region "CreateTranslationProvider"
        public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            if (!SupportsTranslationProviderUri(translationProviderUri))
            {
                throw new Exception("Cannot handle URI.");
            }

            ListTranslationProvider tp = new ListTranslationProvider(new ListTranslationOptions(translationProviderUri));

            return tp;
        }
        #endregion

        #region "SupportsTranslationProviderUri"
        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null)
            {
                throw new ArgumentNullException("Translation provider URI not supported.");
            }
            return String.Equals(translationProviderUri.Scheme, ListTranslationProvider.ListTranslationProviderScheme, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region "GetTranslationProviderInfo"
        public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
        {
            TranslationProviderInfo info = new TranslationProviderInfo();

            #region "TranslationMethod"
            info.TranslationMethod = ListTranslationOptions.ProviderTranslationMethod;
            #endregion

            #region "Name"
            info.Name = PluginResources.Plugin_NiceName;
            #endregion

            return info;
        }
        #endregion

        #endregion
    }
}
```
***

See Also
--------
[Creating the Translation Provider](creating_the_translation_provider.md)

[Storing and Retrieving the Plug-in Settings](storing_and_retrieving_the_plugin_settings.md)

[Building the Plug-in](building_the_plugin.md)