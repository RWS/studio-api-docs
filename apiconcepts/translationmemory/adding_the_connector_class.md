Adding the Connector Class
======
This page explains how to implement the class that establishes a connection to the TM. Depending on the choice made on the form for selecting the TM you will either have to open a file TM, or connect to a server TM.

File or Server TM?
-----
This class makes the basic determination of whether a file or server TM should be used when carrying out the searches. The choice is represented by a public static boolean member, which can be set and queried later from the main lookup form:
# [C#](#tab/tabid-1)
```cs
/// <summary>
/// Property to flag whether a file or server TM should be used for the search
/// </summary> 
public static bool server
{
    get;
    set;
}
```
***


Select the File TM
------
On the corresponding form (see [Adding the Server TM Selection Form](adding_the_server_tm_selection_form.md)) the user raises a **File Open** dialog for selecting the required **.sdltm* file. The following class member is used to create the file TM object, to which the searches are applied later (see [Implementing the Search Functionality](implementing_the_search_functionality.md)).
# [C#](#tab/tabid-2)
```cs
/// <summary>
/// Select the file TM using the file name and path chosen by the user through the GUI.
/// </summary>
public void SelectFileTm(string tmPath)
{
    fileTm = new FileBasedTranslationMemory(tmPath);
    server = false;
}
```
***

The file TM object is declared as a public static member:
# [C#](#tab/tabid-3)
```cs
/// <summary>
/// The file TM object
/// </summary> 
public static FileBasedTranslationMemory fileTm
{
    get;
    set;           
}
```
***

Select the Server TM
------
Selecting a server TM is somewhat more demanding, as it requires you to establish a successful connection to the TM Server. First, declare the server TM object as a public static class member, which can later be used by the search functionality (see [Implementing the Search Functionality](implementing_the_search_functionality.md)).
# [C#](#tab/tabid-4)
```cs
/// <summary>
/// The server TM object
/// </summary>
public static ServerBasedTranslationMemory serverTM
{
    get;
    set;
}
```
***

Also, declare the translation provider server object as a private class member:
# [C#](#tab/tabid-5)
```cs
private TranslationProviderServer tmServer;
```
***

Next, add the following function, which establishes the server connection. The function does the following:
* Establish the server connection by creating a new translation server provider object. This requires the user credentials and the server URI, which are returned by separate members.
* After successfully connecting to the server, the combo list is filled with the server TM names by applying the [GetTranslationMemories](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetTranslationMemories_Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryProperties_) method to the translation provider server object, and the list element gets enabled. Note that the full TM path including the organization name is required for selecting a TM (e.g. *Root Organization/TM Name*). In this example we make sure to populate the dropdown list for the TM selection with the full TM path including the organization name.
* Also, the name of the first server TM is retrieved, and the list is set by default to the name of the first server TM.

> [!NOTE]
> 
> Since a number of things may go wrong when establishing the server connection (e.g. server not available, incorrect password, etc.), you should properly catch any exceptions. (For this example we assume that we are not using Windows logins, but SDL logins, so the useWindowsAuthentication flag is set to False.)

# [C#](#tab/tabid-6)
```cs
/// <summary>
/// Establishing a connection to the TM Server.
/// This connection is primarily needed for populating the 
/// TM dropdown list with the TM names.
/// </summary>
public void Connect(string serverUri, string userName, string password, ComboBox tmList)
{
    try
    {
        tmServer = new TranslationProviderServer(GetUri(serverUri),  false, userName, password);
        foreach (ServerBasedTranslationMemory item in tmServer.GetTranslationMemories(TranslationMemoryProperties.All))
        {
            //Resolve path to the TM inclusive name of the organization(s)
            string tmPath = item.ParentResourceGroupPath == "/" ? "" : item.ParentResourceGroupPath;
            tmList.Items.Add(tmPath + "/" + item.Name);
        }

        if (tmList.Items.Count > 0)
        {
            tmList.Enabled = true;
            tmList.SelectedIndex = 0;
        }
    }
    catch(Exception ex)
    {
        MessageBox.Show(ex.Message);
    }

}
```
***

As mentioned above, the server URI is returned by a separate member:
# [C#](#tab/tabid-7)
```cs
/// Returns the address of the TM Server.
/// </summary>
private Uri GetUri(string uri)
{
    string address = uri;
    return new Uri(address);
}
```
***

Last, when the user clicks the **OK** button on the TM selection form (see [Adding the Server TM Selection Form](adding_the_server_tm_selection_form.md)), the server TM that was chosen by the user through the dropdown list actually gets selected for all subsequent concordance searches. The **OK** button triggers the following function for selecting the TM. Here, another translation provider server connection is made using the URI and the user credentials.
# [C#](#tab/tabid-8)
```cs
/// <summary>
/// Selects the particular server TM as chosen by the user through the dropdown list.
/// </summary>
public void SelectServerTm(string tmName, string uri, string userName, string password)
{
    tmServer = new TranslationProviderServer(GetUri(uri), false, userName, password);
    serverTM = tmServer.GetTranslationMemory(tmName, 
        TranslationMemoryProperties.None);
    server = true;
}
```
***

Putting it All Together
-----
The complete class should look as shown below:
# [C#](#tab/tabid-9)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.Core.Tokenization;
using Sdl.LanguagePlatform.Core;

namespace Sdl.SDK.LanguagePlatform.Samples.TmLookup
{
    class Connector
    {
        #region "TmServer"
        private TranslationProviderServer tmServer;
        #endregion        

        #region "FileOrServer"
        /// <summary>
        /// Property to flag whether a file or server TM should be used for the search
        /// </summary> 
        public static bool server
        {
            get;
            set;
        }
        #endregion

        #region "fileTM"
        /// <summary>
        /// The file TM object
        /// </summary> 
        public static FileBasedTranslationMemory fileTm
        {
            get;
            set;           
        }
        #endregion



        #region "SelectFileTm"
        /// <summary>
        /// Select the file TM using the file name and path chosen by the user through the GUI.
        /// </summary>
        public void SelectFileTm(string tmPath)
        {
            fileTm = new FileBasedTranslationMemory(tmPath);
            server = false;
        }
        #endregion

        #region "serverTM"
        /// <summary>
        /// The server TM object
        /// </summary>
        public static ServerBasedTranslationMemory serverTM
        {
            get;
            set;
        }
        #endregion

        #region "connect"
        /// <summary>
        /// Establishing a connection to the TM Server.
        /// This connection is primarily needed for populating the 
        /// TM dropdown list with the TM names.
        /// </summary>
        public void Connect(string serverUri, string userName, string password, ComboBox tmList)
        {
            try
            {
                tmServer = new TranslationProviderServer(GetUri(serverUri),  false, userName, password);
                foreach (ServerBasedTranslationMemory item in tmServer.GetTranslationMemories(TranslationMemoryProperties.All))
                {
                    //Resolve path to the TM inclusive name of the organization(s)
                    string tmPath = item.ParentResourceGroupPath == "/" ? "" : item.ParentResourceGroupPath;
                    tmList.Items.Add(tmPath + "/" + item.Name);
                }

                if (tmList.Items.Count > 0)
                {
                    tmList.Enabled = true;
                    tmList.SelectedIndex = 0;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion


        #region "uri"
        /// <summary>
        /// Returns the address of the TM Server.
        /// </summary>
        private Uri GetUri(string uri)
        {
            string address = uri;
            return new Uri(address);
        }
        #endregion


        #region "SelectServerTm"
        /// <summary>
        /// Selects the particular server TM as chosen by the user through the dropdown list.
        /// </summary>
        public void SelectServerTm(string tmName, string uri, string userName, string password)
        {
            tmServer = new TranslationProviderServer(GetUri(uri), false, userName, password);
            serverTM = tmServer.GetTranslationMemory(tmName, 
                TranslationMemoryProperties.None);
            server = true;
        }
        #endregion
    }
}
```
***

See Also
--------
[Implementing the Search Functionality](implementing_the_search_functionality.md)