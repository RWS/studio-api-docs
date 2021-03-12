Adding the Server TM Selection Form
========
This page explains how to create the page for selecting the translation memory in which the concordance search is to be carried out. Users should be able to select one TM, which can either be file-based or server-based.

Add the Form Control Elements
-----
The form selecting the TM needs to have the following control elements:

* **groupBoxServerTM**: group box that contains the three following elements:
* **txtServerUri**: text field for entering the server URI
* **txtUserName**: text field for entering the user name
* **txtPassword**: text field for entering the password (set the PasswordChar property to *)
* **btnConnect**: clicked to establish a connection to the TM Server; when successful, the combo list below will be enabled and filled with the TM names
* **comboServerTMs**: dropdown list that contains the server TM names after a successful server connection has been established; disabled by default, will be enabled and filled with the TM names if the Connect button above has been clicked and a server connection is available
* **btnOK**: by pressing this button the user closes the form, establishes a connection to the TM server, the TM gets selected, and the available language pairs are filled into the corresponding combo list of the main TM lookup form. This button is disabled by default, and only enabled, once the names of the available server TMs have been filled into the list above.
* **btnCancel**: closes the form without selecting a TM
All put together your form should look as shown below:

<img style="display:block; " src="images/frmSelectTM.jpg"/>

Implement the GUI Functionality
-----
When the user presses the **Cancel** button, the form should be hidden:

**Connecting to the TM Server and Retrieving the TM Names**

By clicking the **btnConnect** button you establish an initial connection to the server. This connection is required for populating the **comboServerTMs** list with the server TM names and for enabling the list element. Also, the name of the first TM will be chosen by default.

Note that in this simple implementation we will not take into consideration cases in which the file name was left empty or is invalid.
Selecting the TM for the Search

When the users presses the **OK** button, a connection to the TM Server is established, and the form is closed. The TM name will be filled into the corresponding text box of the main application form. Note that the TM name is composed of the server URI and the actual TM name. Note that server TMs can have more than one language direction. We leverage the LanguageDirections property to traverse the available language directions, and propagate the language pairs into the corresponding list of the main application form. The first available language direction should be selected by default.