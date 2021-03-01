Adding Terms
=====
You can configure your custom terminology provider to support adding and editing of terminology entries. Learn how to implement some simplified functionality for adding source and target terms to your delimited text list.

In Trados Studio you can add source and target terms on the fly by marking them in the Editor and by then clicking either the **Add New Term** button or the **Quick Add New Term** button.

<img style="display:block; " src="images/add_terms_buttons.jpg" />

The difference between these two buttons in the standard, MultiTerm-based implementation of Trados Studio is that **Add New Term** does not immediately save the term pair to the termbase, but opens it up an editor control in the **Termbase Viewer** window that allows the user to make changes to the terms before saving them. **Quick Add New Term** adds the term pair directly to the terminology source and displays the newly-created entry in the **Termbase Viewer** window. Our simplified implementation will not offer any editing function. This means that the same thing will happen in our implementation, regardless of whether the users clicks Add New Term or Quick Add New Term.

Open the **MyTerminologyProviderViewerWinFormsUI.cs** class and go to the **AddTerm()** function. Trados Studio passes the currently selected source and target term to this function through the **source** and **target** string parameters. We then implement the following functionality, which works like this:

* Open the glossary text file.
* Loop to the end of the text file, while counting the lines to determine the next entry id.
* Insert the new source and target term (with empty definition).
* Display the newly-created entry in the Internet Explorer control of the **Termbase Viewer** window.


When a new source/target term pair has been added, the following will, for example, be displayed in the **Termbase Viewer** window:

<img style="display:block; " src="images/entry_added.jpg" />

You can also implement your terminology provider to support editing. However, in this simplified example, we will not actually add any editing functionality. This would require us to implement an editing control in the **Termbase Viewer** window. All we are doing for the moment is to output a message in the **AddAndEditTerm()** function of the **MyTerminologyProviderViewerWinFormsUI.cs** class:

When a user tries to add a term that has already been added, SDL Trados Studio throws the following message that prompts the user to:

* Edit the entry by clicking **Yes**.
* Add the term again, thus risking creating a duplicate entry by clicking **No**.
* Abort the entire add term operation with **Cancel**.

<img style="display:block; " src="images/term_exists.jpg" />

If the user clicks **Yes** to edit the entry, the following message box will be displayed in our implementation and the **Termbase Viewer** window remains empty. In a 'real' implementation the entry content will be shown in an edit control where it can still be edited by the user.

<img style="display:block; " src="images/editing_not_implemented.jpg" />