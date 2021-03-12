Adding the Connector Class
======
This page explains how to implement the class that establishes a connection to the TM. Depending on the choice made on the form for selecting the TM you will either have to open a file TM, or connect to a server TM.

File or Server TM?
-----
This class makes the basic determination of whether a file or server TM should be used when carrying out the searches. The choice is represented by a public static boolean member, which can be set and queried later from the main lookup form:


Select the File TM
------
On the corresponding form (see Adding the Server TM Selection Form) the user raises a File Open dialog for selecting the required *.sdltm file. The following class member is used to create the file TM object, to which the searches are applied later (see Implementing the Search Functionality).

The file TM object is declared as a public static member:

Select the Server TM
------
Selecting a server TM is somewhat more demanding, as it requires you to establish a successful connection to the TM Server. First, declare the server TM object as a public static class member, which can later be used by the search functionality (see Implementing the Search Functionality).

Also, declare the translation provider server object as a private class member:

Next, add the following function, which establishes the server connection. The function does the following:
* Establish the server connection by creating a new translation server provider object. This requires the user credentials and the server URI, which are returned by separate members.
* After successfully connecting to the server, the combo list is filled with the server TM names by applying the GetTranslationMemories method to the translation provider server object, and the list element gets enabled. Note that the full TM path including the organization name is required for selecting a TM (e.g. Root Organization/TM Name). In this example we make sure to populate the dropdown list for the TM selection with the full TM path including the organization name.
* Also, the name of the first server TM is retrieved, and the list is set by default to the name of the first server TM.


Note that since a number of things may go wrong when establishing the server connection (e.g. server not available, incorrect password, etc.), you should properly catch any exceptions. (For this example we assume that we are not using Windows logins, but SDL logins, so the useWindowsAuthentication flag is set to False.)

As mentioned above, the server URI is returned by a separate member:

Last, when the user clicks the **OK** button on the TM selection form (see Adding the Server TM Selection Form), the server TM that was chosen by the user through the dropdown list actually gets selected for all subsequent concordance searches. The **OK** button triggers the following function for selecting the TM. Here, another translation provider server connection is made using the URI and the user credentials.

Putting it All Together
-----
The complete class should look as shown below: