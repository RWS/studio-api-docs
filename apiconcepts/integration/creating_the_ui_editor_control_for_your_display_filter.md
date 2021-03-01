Creating the UI Editor Control for your Display Filter
====
To apply your implementation of the *IDisplayFilter*, get a reference to the active document and from there, apply the filter through the *ApplyFilterOnSegments* method. This will invoke a filter action on the document and while iterating over each of the segments, the API will call back into your implementation where you can assert whether or not the segment should be visible in the Trados Studio Editor.

For a more complete solution, it would be useful to make this type of functionality available from the Trados Studio Editor, so that the end-user can choose which criteria they would like to include in the filter.

Add two files to the project that permit the user to view the control in the Studio Editor and then select and apply the filter criteria on the document:

* User Control - The user control will contain the code related to the filter criteria that will be applied on the document
* Controller - The controller is a simple class that is decorated with the extensions that are required by the API to add the user control to the studio editor; for this example, there is very little work involved with the controller.
Add a new user control to the project and name it *DisplayFilterControl*, as shown in the example below:

Include UI design elements to the user control that permit the user to choose the appropriate criteria settings for the filter that will be applied on the document. Make reference to the following images an example of how this might look in your control. For the purpose of this example we will be concentrating more on how to apply the display filter as opposed to how the UI design elements are presented in the control.

> [!NOTE]
> The code related to the design controls is available in the SDK so that you can eventually make reference to it.

Once you have added the UI design elements, open the user control in code view. In order to apply your implementation of the *IDisplayFilter* on the active document, you will first need to get a reference to the *EditorController*, and then subscribe to the *ActiveDocumentChanged* event to ensure that you are always working with the active document from the editor. Making reference to the example code underneath, you can see that we subscribe to this event in the constructor to ensure that we have a handle on this when the control is instantiated in the Editor.

Once you have a reference to the active document, you only need to call the method *ApplyFilterOnSegments* with your implementation of the *IDisplayFilter*, as follows: *ActiveDocument.ApplyFilterOnSegments(new DisplayFilter(filterSettings););* This will then invoke a filter action on the document and while iterating over each of the segments, the API will call back into to the method *EvaluateRow* of your implementation.

Subscribing to the *DocumentFilterChanged* event is quite useful as it permits you to understand when a filter has been applied on the document and then further deduct whether or not that filter is derived from you implementation of the interface.

To load the user control in the Trados Studio Editor, you will need to add a controller and decorate it with the appropriate extensions that will provide the API with enough information to identify and then load it.

Create a new class and name it *DisplayFilterController.*

Add the following code to the *DisplayFilterController.cs* class; it will provide the API with the required information to identify the control and load it in the Editor, docking it at the bottom of the view.