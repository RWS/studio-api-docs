Displaying Entry Content
====
The full content of an entry can be displayed in the **Termbase Viewer** window. Learn how to show the content of a line of your glossary file in Trados Studio.

When you right-click a term in the **Term Recognition** or in the **Termbase Search** window, you can use the command **View term details** to show the full entry content in the **Termbase Viewer** window.

We need to add an Internet Explorer control in which we can display the entry content in HTML format. To do this, add a user control to your project and call it **TermProviderControl.cs**, for example. Add an Internet Explorer control to it:

<img style="display:block; " src="images/Control.jpg">


Go to the **MyTerminologyProviderViewerWinFormsUI.cs** class and declare the following term controller object:
# [The Term Controller Object](#tab/tabid-1)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L17)]
***

Modify the following **TermProviderControl** property (which is implemented by the **ITerminologyProviderViewerWinFormsUI** interface) as shown below to create and return the control element:

# [Returning the Controller Element](#tab/tabid-2)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L21-L30)]
***

When the you right-click a term and call up the command **View term details**, the corresponding entry content should be shown in the newly-created control.

You need to modify the **JumpToTerm()** method as shown below. Trados Studio passes the ID of the selected entry, which you can use to:

* Retrieve the corresponding line from the glossary text file.
* Parse the line and generate the HTML output for the Internet Explorer controller object.

# [Jumping to a Term](#tab/tabid-3)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L112-L150)]
***