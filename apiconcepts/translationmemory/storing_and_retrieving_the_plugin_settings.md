# Storing and Retrieving the Plug-in Settings

The settings configured through the plug-in user interface, see [Implementing the Plug-in User Interface](implementing_the_plugin_user_interface.md), must also be stored and retrieved programmatically. This chapter shows how to implement a separate class for storing the plug-in settings. The settings are stored in an **.sdlproj** or **.sdltlp** file.

## Implement the Class for Handling the Plug-in Settings

Add a helper class named `ListTranslationOptions` to the project. The plug-in template does not include this class.

## Define the Translation Method

First, define the [TranslationMethod](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMethod.yml) that best fits the translation provider. The translation method could be translation memory, machine translation, pseudo-translation, or another type. A delimited list is similar to a basic TM, so you could choose [TranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMethod.yml). However, to distinguish this plug-in from a standard TM, use the **Other** option:
# [C#](#tab/tabid-1)
```cs
public static readonly TranslationMethod ProviderTranslationMethod = TranslationMethod.Other;
```
***

## Create the Plug-in URI Builder

Each plug-in uses a URI to store and retrieve settings. The following code creates the URI builder object used to store and persist the plug-in settings:
# [C#](#tab/tabid-2)
```cs
TranslationProviderUriBuilder _uriBuilder;        

public ListTranslationOptions()
{
    _uriBuilder = new TranslationProviderUriBuilder(ListTranslationProvider.ListTranslationProviderScheme);
}

public ListTranslationOptions(Uri uri)
{
    _uriBuilder = new TranslationProviderUriBuilder(uri);
}
```
***

The URI property returns the resulting URI, as shown below:
# [C#](#tab/tabid-3)
```cs
public Uri Uri
{            
    get
    {
        return _uriBuilder.Uri;                
    }
}
```
***

## Set and Retrieve the Plug-in Parameters

This plug-in implements two string settings: the list file name and path, and the delimiter character. The following public string properties are set by the plug-in user form. Each property uses a private helper method to get or set the corresponding value:
# [C#](#tab/tabid-4)
```cs
public string ListFileName
{
    get { return GetStringParameter("listfile"); }
    set { SetStringParameter("listfile", value); }            
}
```
***
# [C#](#tab/tabid-5)
```cs
public string Delimiter
{
    get { return GetStringParameter("delimiter");}
    set {SetStringParameter("delimiter", value);}
}
```
***
The following method sets a string parameter. It takes the property name and the corresponding value as string parameters:
# [C#](#tab/tabid-6)
```cs
private void SetStringParameter(string p, string value)
{
    _uriBuilder[p] = value;
}
```
***

The following method retrieves the value for a specified URI property:
# [C#](#tab/tabid-7)
```cs
private string GetStringParameter(string p)
{
    string paramString = _uriBuilder[p];
    return paramString;
}
```
***

The following example shows what a URI string looks like in this implementation:

```
listprovider:///?delimiter=;&listfile=C:\temp\sample_list.txt" Enabled="true"
```


The translation provider plug-in settings are stored in the project file (*.sdlproj) or in the project template (*.sdltpl) file. These XML-compliant files store project-specific or project-template-specific information. The excerpt below shows how a translation provider appears in a project or template file together with its settings:

```xml
<CascadeItem OverrideParent="true" StopSearchingWhenResultsFound="false">
  <CascadeEntryItem PerformConcordanceSearch="true" Penalty="0" PerformUpdate="false" PerformNormalSearch="true">
    <MainTranslationProviderItem Uri="listprovider:///?delimiter=;&listfile=C:\temp\sample_list.txt" Enabled="true" />
  </CascadeEntryItem>
</CascadeItem>
```

> [!NOTE]
> 
> For every project that is created in Var:ProductName as well as for each single file that is opened for translation, an **.sdlproj* file will be created, which contains the translation providers used for the given project or document, the document(s) to translate, the termbase(s) used, etc.

Putting it all Together
----
The complete class should look as shown below:
# [C#](#tab/tabid-10)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System.Windows.Forms;

namespace Sdk.LanguagePlatform.Samples.ListProvider
{
    /// <summary>
    /// This class is used to hold the provider plug-in settings. 
    /// All settings are automatically stored in a URI.
    /// </summary>
    public class ListTranslationOptions
    {
        public static readonly TranslationMethod ProviderTranslationMethod = TranslationMethod.Other;
        TranslationProviderUriBuilder _uriBuilder;        

        public ListTranslationOptions()
        {
            _uriBuilder = new TranslationProviderUriBuilder(ListTranslationProvider.ListTranslationProviderScheme);
        }

        public ListTranslationOptions(Uri uri)
        {
            _uriBuilder = new TranslationProviderUriBuilder(uri);
        }
       
        /// <summary>
        /// Set and retrieve the name and path of the delimited list file.
        /// </summary>        
        public string ListFileName
        {
            get { return GetStringParameter("listfile"); }
            set { SetStringParameter("listfile", value); }            
        }        

        /// <summary>
        /// Set and retrieve the delimiter character.
        /// </summary>
        public string Delimiter
        {
            get { return GetStringParameter("delimiter");}
            set {SetStringParameter("delimiter", value);}
        }

        private void SetStringParameter(string p, string value)
        {
            _uriBuilder[p] = value;
        }

        private string GetStringParameter(string p)
        {
            string paramString = _uriBuilder[p];
            return paramString;
        }

        public Uri Uri
        {            
            get
            {
                return _uriBuilder.Uri;                
            }
        }
    }
}
```
***

# See Also
[Implementing the Plug-in User Interface](implementing_the_plugin_user_interface.md)

[Controlling the Plug-in User Interface](controlling_the_plugin_user_interface.md)
