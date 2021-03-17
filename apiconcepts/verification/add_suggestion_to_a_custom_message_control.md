Add Suggestion to a Custom Message Control
======
In this section, the IdenticalCheck custom message control described in Create a Custom MessageControl shall be adapted to make a suggestion.

A suggestion is a change to the document that will fix the verification problem described by the verification message.

This project can be found in the samples directory under Sdl.Verification.Sdk.IdenticalCheck.Extended.

Overview
------
The IdenticalCheck custom message control shall be adapted to make a suggestion to replace the contents of the target segment with the source segment. Since the IdenticalCheck custom message control can make a suggestion then the Change button will be enabled.

<img style="display:block; " src="images/CustomMessageControlSuggestion1.png" />


When the user clicks on the Change button, the suggestion is applied and that will make the target segment identical to the source segment. Since the target segment will be identical to the source segment then the verification problem will be fixed and the verification message will be removed.

<img style="display:block; " src="images/CustomMessageControlSuggestion2.png" />

Changing the custom user control to be a suggestion provider
=====
The custom user control is responsible for providing a suggestion because the custom user control may allow the user to select a suggestion. Any custom user control that provides a suggestion should implement ISuggestionProvider.

The IdenticalVerifierMessageUI custom user control should implement ISuggestionProvider to be able to suggest replacing the contents of the target segment with the source segment.

1. Add a using reference to Sdl.Verification.Api.
2. Make the class implement ISuggestionProvider - use Visual Studio to add empty implementations.
3. Make HasSuggestion always return true because there is always a suggestion - replacing the contents of the target segment with the source segment.

The IdenticalVerifierMessageUI custom user control needs the source segment to be able to suggest replacing the contents of the target segment with the source segment.

1. Add a using reference to Sdl.FileTypeSupport.Framework.BilingualApi.
2. Use property ReplaceDocumentSegment of IdenticalVerifierMessageData class to create new Suggestion.

A suggestion is always a replacement where the target contents from one location upto another location is replaced by the new markup. If the new markup is null then the suggestion effectively deletes the contents from one location upto another location. Always set the new markup to null to delete content and do not use empty text markup.

The IdenticalVerifierMessageUI constructor now has the from and upto locations from the verification message (messageEventArgs) and the new markup from the source segment (messageData.ReplacementSuggestion). All that remains is for IdenticalVerifierMessageUI constructor to create the Suggestion and store it in a member variable for the GetSuggestion method.

1. Add a private member variable called _suggestion of type Suggestion.
2. Create the _suggestion from the messageEventArgsFromLocation and UptoLocation and from the messageData.ReplacementSuggestion.
3. Change the GetSuggestion method to return this suggestion.

Summary
====
That completes the work necessary to allow the IdenticalVerifierMessageUI custom message control to provide a suggestion for verification messages produced by the IdenticalCheck global verifier. If the user views a verification message produced by the IdenticalCheck global verifier then the Change button will be enabled and clicking on the Change button will apply the suggestion where the contents of the target segment is replaced with the contents of the source segment as described in the Overview.

In this simple example, the **SuggestionChanged** event was not used. In complex custom message controls, there may be more than one reasonable suggestion that the user could choose from. These suggestions could be represented in a control like a combo-box or a list box where the user can select a suggestion. As the user selects a suggestion, selects a different suggestion, or "unselects" a suggestion the **SuggestionChanged** event should be fired. This event is listened to by the verification form that updates whether the Change button is enabled or not.