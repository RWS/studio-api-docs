Edit Segment and Apply Changes to Document
=====

This section presents the necessary information to be able to implement the code that allows to edit a segment and apply the changes back to the document without exiting the "Verification Message Details" dialog box.

Introduction
----
A user is able to make changes to the content of the segment over and above the suggestions provided by the segment verifier and apply those changes to the document without have to close the segment verifier dialog.

An Edit control is available to make these changes. An Event is fired by the control whenever the segment has been edited. If the user has edited the control then this content will be used instead of the one coming from the suggestion.

A Reset bottom is available to users to revert their changes.

The figure below shows the dialog box for the QA Checker verifier with the Edit control (Target segment) and the Suggestions control. It also shows the Change and Reset bottom.

<img style="display:block; " src="images/EditSegmentAndApplyChanges.jpg" />

In order to change the behaviour of this control, the [ISegmentChangedAware](../../api/verification/Sdl.Verification.Api.ISegmentChangedAware.yml) interface needs to be implemented.

For a complete example using the QA Checker verifier, please refer to the Control 
**CustomMessageControl** in the Project **Sdl.Verification.Sdk.EditAndApplyChanges.MessageUI** that is included with the **SDK samples** for a complete description of how to implement this.
