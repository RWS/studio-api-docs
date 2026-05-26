# Instantiating the Plug-in

You must instantiate a translation provider plug-in before Var:ProductName can use or display it. The plug-in template includes the factory component that performs this task. In this implementation, rename `MyTranslationProviderFactory` to `ListTranslationProviderFactory`.

## The Translation Provider Factory Interface

The factory component implements the [ITranslationProviderFactory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderFactory.yml) interface. This interface defines three methods, which the following sections describe for the sample plug-in.

The class begins with the following declaration. It marks the class as an extension class and makes it available to Var:ProductName through the plug-in manifest *.xml file. See also [Building the Plug-in](building_the_plugin.md).

# [C#](#tab/tabid-1)
```cs
[TranslationProviderFactory(
    Id = "ListTranslationProviderFactory",
    Name = "Delimited list translation provider",
    Description = "Searches in delimited text files.")]
```
***

## Create the Translation Provider

Use the [CreateTranslationProvider](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderFactory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderFactory_CreateTranslationProvider_System_Uri_System_String_Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderCredentialStore_) method to create the translation provider specified in the translation provider URI. In this implementation, the scheme uses the string *listprovider*. This value identifies the provider, so it must be unique. The URI also stores the plug-in settings, which the following method reads. In this implementation, those settings include the delimiter character and the delimited text file name and path.

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

The plug-in URI is stored in an **.sdlproj** or **.sdltlp** file. These file types store, among other things, the translation providers used in a translation project and their settings.

The following example shows a plug-in URI for this implementation. The URI starts with *listprovider:///* and includes the *delimiter* and *listfile* settings.

```
listprovider:///?delimiter=;&listfile=C:\temp\sample_list.txt" Enabled="true"
```


## Determine Whether the URI is Supported by the Plug-in

Use the [SupportsTranslationProviderUri](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderFactory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderFactory_SupportsTranslationProviderUri_System_Uri_) method to determine whether the provided URI is valid and supported by the plug-in, as shown in the sample code below:
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

## Setting the Plug-in Info

This component also sets the plug-in UI elements, such as the name and icon. Use the [GetTranslationProviderInfo](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationProviderFactory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ITranslationProviderFactory_GetTranslationProviderInfo_System_Uri_System_String_) method to set the plug-in name and translation method for the Var:ProductName user interface.

First, create a [TranslationProviderInfo](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderInfo.yml) object to hold the plug-in properties. Retrieve the friendly name from the resources file:
# [C#](#tab/tabid-5)
```cs
info.Name = PluginResources.Plugin_NiceName;
```
***

The [TranslationMethod](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMethod.yml) comes from the separate `ListTranslationOptions` class, which we implement in a later step. See [Storing and Retrieving the Plug-in Settings](storing_and_retrieving_the_plugin_settings.md). The translation method identifies the type of translation solution the plug-in provides, such as translation memory or machine translation.
# [C#](#tab/tabid-6)
```cs
info.TranslationMethod = ListTranslationOptions.ProviderTranslationMethod;
```
***

After you set these plug-in properties, return the info object as shown below:
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

## Putting it All Together

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
    [TranslationProviderFactory(
        Id = "ListTranslationProviderFactory",
        Name = "ListTranslationProviderFactory",
        Description = "Searches in delimited text files.")]
 
    public class ListTranslationProviderFactory : ITranslationProviderFactory
    {
        public ITranslationProvider CreateTranslationProvider(Uri translationProviderUri, string translationProviderState, ITranslationProviderCredentialStore credentialStore)
        {
            if (!SupportsTranslationProviderUri(translationProviderUri))
            {
                throw new Exception("Cannot handle URI.");
            }

            ListTranslationProvider tp = new ListTranslationProvider(new ListTranslationOptions(translationProviderUri));

            return tp;
        }
 
        public bool SupportsTranslationProviderUri(Uri translationProviderUri)
        {
            if (translationProviderUri == null)
            {
                throw new ArgumentNullException("Translation provider URI not supported.");
            }
            return String.Equals(translationProviderUri.Scheme, ListTranslationProvider.ListTranslationProviderScheme, StringComparison.OrdinalIgnoreCase);
        }
                
        public TranslationProviderInfo GetTranslationProviderInfo(Uri translationProviderUri, string translationProviderState)
        {
            TranslationProviderInfo info = new TranslationProviderInfo();
                    
            info.TranslationMethod = ListTranslationOptions.ProviderTranslationMethod;
            info.Name = PluginResources.Plugin_NiceName;
            
            return info;
        }     
    }
}
```
***

# See Also
[Creating the Translation Provider](creating_the_translation_provider.md)

[Storing and Retrieving the Plug-in Settings](storing_and_retrieving_the_plugin_settings.md)

[Building the Plug-in](building_the_plugin.md)
