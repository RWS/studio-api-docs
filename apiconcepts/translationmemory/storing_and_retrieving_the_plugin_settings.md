Storing and Retrieving the Plug-in Settings
======
The settings that are configured through the plug-in user interface (see [Implementing the Plug-in User Interface](implementing_the_plugin_user_interface.md)) need to be programmatically set and retrieved. This chapter describes how to implement a separate class for storing the plug-in settings. These settings are physically stored in an **.sdlproj* or in an **.sdltlp* file.

Implement the Class for Handling the Plug-in Settings
------
Add a helper class called `ListTranslationOptions` to the project. This class is not included in the plug-in template.

Define the Translation Method
------
First, we define the [TranslationMethod](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMethod.yml) that best applies to our translation provider source. The translation method could, for example, be translation memory, machine translation, pseudo-translation, etc. A delimited list is basically like a rudimentary TM. Therefore you could, for example, select [TranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMethod.yml). However, to set our plug-in apart from a 'normal' TM we select the option Other:
# [C#](#tab/tabid-1)
```cs
public static readonly TranslationMethod ProviderTranslationMethod = TranslationMethod.Other;
```
***

Create the Plug-in URI Builder
------
Each plug-in uses a URI to store and retrieve settings. Through the following code we create the URI builder object, which is used to store and persist the plug-in settings:
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

At the end we return the URI as shown below:
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

Set and Retrieve the Plug-in Parameters
------
Our plug-in implements two string settings, i.e. the list file name and path and the delimiter character. In the following we declare the two public string properties which are set by the plug-in user form. The properties call on two separate private functions to get and to set the corresponding values:
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
The following function sets the string parameter. It takes the property name and the corresponding value as string parameters:
# [C#](#tab/tabid-6)
```cs
private void SetStringParameter(string p, string value)
{
    _uriBuilder[p] = value;
}
```
***

The following function is used to retrieve the value for a specified URI property:
# [C#](#tab/tabid-7)
```cs
private string GetStringParameter(string p)
{
    string paramString = _uriBuilder[p];
    return paramString;
}
```
***

The following is an example of what a URI string will look like in our implementation:

```
listprovider:///?delimiter=;&listfile=C:\temp\sample_list.txt" Enabled="true"
```


The translation provider plug-in settings are physically stored in the project file (*.sdlproj) or in the project template (*.sdltpl) file. These are XML-compliant files that store project- or project-template specific information. The file excerpt below provides an example of how a translation provider is listed in a project or template file alongside with its respective settings:

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
        #region "TranslationMethod"
        public static readonly TranslationMethod ProviderTranslationMethod = TranslationMethod.Other;
        #endregion

        #region "TranslationProviderUriBuilder"
        TranslationProviderUriBuilder _uriBuilder;        

        public ListTranslationOptions()
        {
            _uriBuilder = new TranslationProviderUriBuilder(ListTranslationProvider.ListTranslationProviderScheme);
        }

        public ListTranslationOptions(Uri uri)
        {
            _uriBuilder = new TranslationProviderUriBuilder(uri);
        }
        #endregion

        /// <summary>
        /// Set and retrieve the name and path of the delimited list file.
        /// </summary>
        #region "ListFileName"
        public string ListFileName
        {
            get { return GetStringParameter("listfile"); }
            set { SetStringParameter("listfile", value); }            
        }
        #endregion

        /// <summary>
        /// Set and retrieve the delimiter character.
        /// </summary>
        #region "Delimiter"
        public string Delimiter
        {
            get { return GetStringParameter("delimiter");}
            set {SetStringParameter("delimiter", value);}
        }
        #endregion

        #region "SetStringParameter"
        private void SetStringParameter(string p, string value)
        {
            _uriBuilder[p] = value;
        }
        #endregion

        #region "GetStringParameter"
        private string GetStringParameter(string p)
        {
            string paramString = _uriBuilder[p];
            return paramString;
        }
        #endregion


        #region "Uri"
        public Uri Uri
        {            
            get
            {
                return _uriBuilder.Uri;                
            }
        }
        #endregion
    }
}
```
***

See Also
---
[Implementing the Plug-in User Interface](implementing_the_plugin_user_interface.md)

[Controlling the Plug-in User Interface](controlling_the_plugin_user_interface.md)
