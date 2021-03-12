Adding the Search Settings Form
This page shows how to implement the user interface for configuring the search settings. The sample application should allow users to set the maximum number of search results as well as the minimum fuzziness value.

Add the Form Control Elements
The form configuring the search options needs to have the following control elements:

* **txtMaxHits**: text box in which the maximum number of hits can be entered; set the *MaxLength* property to 2, so that only values up to 99 can be entered (note that in this simplified application we will not implement any functionality that checks whether the input in this field is numerical)
* **trackFuzzy**: slider to set the fuzziness value anywhere between 30 and 100; the minimum value should therefore be 30, the maximum 100, the default value 70
* **lblFuzzyValue**: label that shows the fuzziness value set by the slider
* **btnDefaults**: restores the settings back to their 8 default values
* **btnOK**: closes the form and applies the settings changes
* **btnCancel**: closes the form without applying settings changes


<img style="display:block; " src="images/frmSearchSettings.jpg"/>

Implement the GUI Functionality
-----
**Setting the Fuzziness Value**

Through the slider users can set the minimum fuzziness value between 30 and 100. When changing the slider position, the value should be updated in the label element:

**Resetting to Default Values**

The default settings should be restored as follows by clicking the corresponding button:

**Applying the Settings**

Add two public properties that represent the search settings:

Clicking the **OK** button should hide the form and apply the settings.