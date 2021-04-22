Adding the Server TM Selection Form
========
This page explains how to create the page for selecting the translation memory in which the concordance search is to be carried out. Users should be able to select one TM, which can either be file-based or server-based.

Add the Form Control Elements
-----
The form selecting the TM needs to have the following control elements:

* **groupBoxServerTM**: group box that contains the three following elements:
* **txtServerUri**: text field for entering the server URI
* **txtUserName**: text field for entering the user name
* **txtPassword**: text field for entering the password (set the *PasswordChar* property to *)
* **btnConnect**: clicked to establish a connection to the TM Server; when successful, the combo list below will be enabled and filled with the TM names
* **comboServerTMs**: dropdown list that contains the server TM names after a successful server connection has been established; disabled by default, will be enabled and filled with the TM names if the **Connect** button above has been clicked and a server connection is available
* **btnOK**: by pressing this button the user closes the form, establishes a connection to the TM server, the TM gets selected, and the available language pairs are filled into the corresponding combo list of the main TM lookup form. This button is disabled by default, and only enabled, once the names of the available server TMs have been filled into the list above.
* **btnCancel**: closes the form without selecting a TM
All put together your form should look as shown below:

<img style="display:block; " src="images/frmSelectTM.jpg"/>

Implement the GUI Functionality
-----
When the user presses the **Cancel** button, the form should be hidden:
# [C#](#tab/tabid-1)
```cs
private void btnCancel_Click(object sender, EventArgs e)
{
    this.Close();
}
```
***

### Connecting to the TM Server and Retrieving the TM Names

By clicking the **btnConnect** button you establish an initial connection to the server. This connection is required for populating the **comboServerTMs** list with the server TM names and for enabling the list element. Also, the name of the first TM will be chosen by default.
# [C#](#tab/tabid-2)
```cs
// By clicking the Connect button you establish a connectionw with the TM
// Server. This will fill populate the dropdown list with the names of the
// server TMs and enable the list, which is by default disabled.
// Moreover, the OK button gets enabled.
private void btnConnect_Click(object sender, EventArgs e)
{
    Connector connection = new Connector();
    connection.Connect(this.txtServerUri.Text, this.txtUserName.Text, 
        this.txtPassword.Text, this.comboServerTMs);

    this.btnOK.Enabled = true;
}
```
***
> [!NOTE]
>
> In this simple implementation we will not take into consideration cases in which the file name was left empty or is invalid.

### Selecting the TM for the Search

When the users presses the **OK** button, a connection to the TM Server is established, and the form is closed. The TM name will be filled into the corresponding text box of the main application form. Note that the TM name is composed of the server URI and the actual TM name. Note that server TMs can have more than one language direction. We leverage the [LanguageDirections](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ServerBasedTranslationMemory.yml#Sdl_LanguagePlatform_TranslationMemoryApi_ServerBasedTranslationMemory_LanguageDirections) property to traverse the available language directions, and propagate the language pairs into the corresponding list of the main application form. The first available language direction should be selected by default.

# [C#](#tab/tabid-3)
```cs
// By clicking OK the user connects the server-based TM
// through the Connector class.
// The TM language directions are propagated into the corresponding list of the frmLookup form.
private void btnOK_Click(object sender, EventArgs e)
{
    // Establish a connection to the TM Server.
    Connector connect = new Connector();

    connect.SelectServerTm(this.comboServerTMs.Text, this.txtServerUri.Text,
            this.txtUserName.Text, this.txtPassword.Text);

    // Enter the TM URI and TM name into the main application form.
    frmLookup lookupFrm = new frmLookup();
    lookupFrm.txtTmPath.Text = Connector.serverTM.Uri.ToString();            

    // Loop throught the available language directions of the selected TM and fill them
    // into the corresponding list in the main application form.
    lookupFrm.comboLanguagePairs.Items.Clear();
    for (int i = 0; i < Connector.serverTM.LanguageDirections.Count; i++)
    {
        string srcLang = Connector.serverTM.LanguageDirections[i].SourceLanguage.DisplayName;
        string trgLang = Connector.serverTM.LanguageDirections[i].TargetLanguage.DisplayName;
        lookupFrm.comboLanguagePairs.Items.Add(srcLang + " -> " + trgLang);
    }

    // Select the first available language direction.
    string currentSrcLang = Connector.serverTM.LanguageDirections[0].SourceLanguage.DisplayName;
    string currentTrgLang = Connector.serverTM.LanguageDirections[0].TargetLanguage.DisplayName;
    lookupFrm.comboLanguagePairs.Text = currentSrcLang + " -> " + currentTrgLang;

    lookupFrm.Show();
    this.Close();
}
```
***

See Also
-------
[Adding the Search Settings Form](adding_the_search_settings_form.md)

[Adding the Connector Class](adding_the_connector_class.md)

[Implementing the Search Functionality](implementing_the_search_functionality.md)