Adding a Settings UI to our Batch Task
===============================
Add a settings page to your batch task so that users can select the status that defines whether the content of a particular segment pair shall be exported to a text file.

The User Control
-------------------------
In your Visual Studio project go to the yet empty       **MyCustomBatchTaskSettingsControl.cs** control and add the following UI elements:
<img style="display:block; " src="images/SettingsPage.jpg" />
Add the following list items to the dropdown list element (which we name **combo_Status**):
* Unspecified
* Draft
* Translated
* Approved
* Rejected
* Signed-off
* Sign-off rejected
  
The Class that Controls the Plug-in Settings
-----------------------------------
Open the **MyCustomBatchTaskSettings.cs** class, which has been automatically added to your Visual Studio project. This is the class that we use to programmatically access the settings configured by the elements on the user control UI.

This class needs to inherit the following class:

# [The Settings Class](#tab/tabid-1)
[!code-csharp[MyCustomBatchTaskSettings](code_samples/MyCustomBatchTaskSettings.cs#L6-L7)]
***

Here we declare the default setting value, which is the integer value '2' and corresponds to the confirmation level 'Translated':
# [The Default Setting Value](#tab/tabid-2)
[!code-csharp[MyCustomBatchTaskSettings](code_samples/MyCustomBatchTaskSettings.cs#L11-L12)]
***
The following member gets or to sets the value used for the plug-in settings. In this implementation there is only one integer value that defines the confirmation level:
# [Getting or Setting the Value](#tab/tabid-3)
[!code-csharp[MyCustomBatchTaskSettings](code_samples/MyCustomBatchTaskSettings.cs#L17-L22)]
***

Then we add the following member, which sets the confirmation property to the default value when the user clicks the corresponding button on the UI.
# [Configuring the Default Setting](#tab/tabid-4)
[!code-csharp[MyCustomBatchTaskSettings](code_samples/MyCustomBatchTaskSettings.cs#L26-L31)]
***	
    
Finally, the following member is added to retrieve the default value for the confirmation level property of our implementation:
# [Getting the Default Value](#tab/tabid-5)
[!code-csharp[MyCustomBatchTaskSettings](code_samples/MyCustomBatchTaskSettings.cs#L35-L44)]
***	

Adding Functionality to the User Control<
-----------------------------------------
Open the code view of the user control **MyCustomBatchTaskSettingsControl.cs**. This control implements the following interfaces:
# [The User Settings Interfaces](#tab/tabid-6)
[!code-csharp[MyCustomBatchTaskSettingsControl](code_samples/MyCustomBatchTaskSettingsControl.cs#L8-L9)]
***	

The following mandatory member needs to be implemented to set and get the settings properties from the **MyCustomBatchTaskSettings** class:
# [Getting and Setting the Properties](#tab/tabid-7)
[!code-csharp[MyCustomBatchTaskSettingsControl](code_samples/MyCustomBatchTaskSettingsControl.cs#L13-L14)]
***

The following member initializes the user control:
# [User Control Initialisation](#tab/tabid-8)
[!code-csharp[MyCustomBatchTaskSettingsControl](code_samples/MyCustomBatchTaskSettingsControl.cs#L18-L22)]
***

The following member sets the settings on the UI control:
# [Setting the Settings](#tab/tabid-9)
[!code-csharp[MyCustomBatchTaskSettingsControl](code_samples/MyCustomBatchTaskSettingsControl.cs#L26-L32)]
***

The following member updates the settings on the UI control:
# [Updating the Settings](#tab/tabid-10)
[!code-csharp[MyCustomBatchTaskSettingsControl](code_samples/MyCustomBatchTaskSettingsControl.cs#L36-L39)]
***

The above member is called by the following function:
# [Updating the UI](#tab/tabid-11)
[!code-csharp[MyCustomBatchTaskSettingsControl](code_samples/MyCustomBatchTaskSettingsControl.cs#L43-L48)]
***

When the UI control is loaded, its control elements are populated with the corresponding values:
# [Loading the UI](#tab/tabid-12)
[!code-csharp[MyCustomBatchTaskSettingsControl](code_samples/MyCustomBatchTaskSettingsControl.cs#L53-L58)]
***

When the user clicks the **Restore Defaults** button, the UI controls are set to their default values:
# [Restoring the Defaults](#tab/tabid-13)
[!code-csharp[MyCustomBatchTaskSettingsControl](code_samples/MyCustomBatchTaskSettingsControl.cs#L62-L68)]
***

The Settings UI Container
-------------------------------------

Note the **MyCustomBatchTaskSettingsPage.cs** class, which has been added to the project by default. This is the class that references the settings UI and the class that controls the UI. Without the settings page class the plug-in would not be aware of the settings UI. It enables the settings UI to be shown when the user reaches the **Settings** page of the plug-in: 
<img style="display:block; " src="images/SampleTaskSettings.jpg" />
> [!NOTE]
> Leave this class as it is, you do not have to change anything here.