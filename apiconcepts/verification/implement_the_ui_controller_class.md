Implement the UI Controller Class
=====

In this chapter you will learn how to implement a class for controlling the plug-in user interface. This class acts as a kind of intermediary between the plug-in UI and the class for setting and retrieving the settings values (see Retrieve the Settings Values).

Add the Class for Controlling the User Interface
----
Add new class to your project and call it e.g. `IdenticalVerifierUIPage.cs`. This class contains a number of overriding methods that are required for controlling the user interface, which you created previously (see Implement the User Interface). This class needs to reference the namespace **Sdl.Core.Settings** and **Sdl.Verification.Api**. Moreover, the new class needs to be derived from **AbstractSettingsPage**.

This class needs to be preceded by the following annotation, which makes it an extension class, which is referenced in the plug-in manifest (see also Create a New Project).

# [C#](#tab/tabid-1)
[!code-csharp[IdenticalVerifierUiPage](code_samples/IdenticalVerifierUiPage.cs#L7-L11)]
***

Note that there are various strings to provide in the annotation, such as the plug-in page name and description. The plug-in page name corresponds to the link that is clicked in the user interface of Trados Studio 2017 to display the corresponding page.
Then declare the following private members:

* **_Control**: This member is derived from the user interface class (see Implement the User Interface); we use this member to control the actual user interface (e.g. when the user clicks the buttons **OK, Reset to Defaults**, etc. in the UI)
* **_ControlSettings**: This member is derived from our IdenticalVerifierSettings class (see Retrieve the Settings Values); it is used to access the setting values of the verifier plug-in.
The skeleton for your new class should look as shown below:

# [C#](#tab/tabid-2)
[!code-csharp[IdenticalVerifierUiPage](code_samples/IdenticalVerifierUiPage.cs#L1-L17)]
***

Access the Verifier Settings
-----
Start by adding the following private members, which are derived from the actual plug-in UI (see Implement the User Interface) and the verification settings class (see Retrieve the Settings Values):

# [C#](#tab/tabid-3)
[!code-csharp[IdenticalVerifierUiPage](code_samples/IdenticalVerifierUiPage.cs#L14-L15)]
***

In the next step, add the following member, which is used to get a handle on the plug-in user interface (i.e. the control).

Add the Functionality to Control the UI
------
Now you need to add a number of members that control the user interface. These methods are implemented to do the following:

* initialize the user interface
* load the settings that were configured in a previous session
* determine what happens when the user clicks the **OK** button, thereby saving the configured settings
* define what happens when the user clicks **Reset to Defaults**, thus restoring the settings to their default values
* moves away from the settings page, thus cancelling the settings page operation


Below you see examples of the members for controlling the user interface. The following member controls what happens when the user clicks the **Reset to Defaults** button, which is provided by the framework:

The following member controls what happens when the user clicks the **OK** button, thereby committing the settings that were configured on the plug-in settings page:

The member below is used to control what happens when the user clicks **Cancel**, thereby not saving the settings:

Another required member of the abstract settings page base class is used to properly dispose of the user control:

Putting it All Together
-----
The complete class for controlling the settings page should now as shown below: