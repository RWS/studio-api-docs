Implementing the Search Functionality
======
This page outlines how to implement the actual search functionality and how to output the results in the rich text element using a simple string output.

Implement the Search Function
-------
Start by adding a new class called Search to your project. Implement a public function called DoConcordanceSearch, which takes the search text as string parameter and a boolean parameter that indicates whether the search should be done in the target index. In addition, we pass the SelectedIndex property of the comboLanguagePairs control as another parameter. The index of this list corresponds to the index of the server TM language pair. The function returns the search result as a string.

The function is called by clicking the search button on the main lookup form, which then fills the rich text control with the search result:

In the DoConcordanceSearch function you first execute the search in the selected TM (file or server TM). The boolean server property from the Connector (see Adding the Connector Class) class flags whether the search should be applied to a file or server TM object.

The SearchText method takes the search string as well as the search settings as parameters. The search settings (as set by the user on the corresponding form, see Adding the Search Settings Form) are returned by a separate function:

Next, you start building up the hit result string by determining the hit count:

In the following step, you need to traverse the search results:

The loop continues to build up the hitlist string by calling a separate GetTuInformation helper function, which returns TU information such as the source/target segments, the creation date, any field names and values, as well as the match value:

At the end the DoConcordanceSearch function returns the full hit result string, which is then used to fill the rich text control:

Apply the Search Settings
------
Add the following private function called GetSearchSettings, which configures the search settings according to the settings configured on the search settings form (see Adding the Search Settings Form).

See also Doing Translation Memory Lookups for more information on the available search settings.

Putting it All Together
-----
The complete search class should look as shown below: