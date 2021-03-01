Enhance the Verifier
====

In this chapter you will learn learn how to enhance your plug-in to take inline tag differences into account or to ignore inline tags.

Enhance the Verifier Plug-in
---

Let us assume the following: the verifier plug-in needs to be able to 'decide' whether a target segment is identical to the corresponding source segment, even when there is a difference in tags. This means that the user should be able to configure whether tag differences should be taken into account when applying the verification, or whether only text changes should be considered to be differences. If tags are ignored, then the following segments would be regarded as being identical:

Add a TextGenerator Class
-----
To fulfil this requirement you need to extend the plug-in, so that either the plain text (without tags) is be compared or the text with tag content. Start by adding a new a class called e.g. `sTextGenerator.cs` to your project.

This class will be responsible for generating the plain text content from the segment pairs. In this class, reference the namespace **Sdl.FileTypeSupport.Framework.BilingualApi** and have it implement the **IMarkupDataVisitor** interface. The skeleton class should look as shown below:

Continue by adding two new members to the class: a string builder for retrieving the plain text and a boolean setting to determine whether to include tag text or not:

Now add a string method for retrieving the plain segment text. This method takes a segment object and a boolean parameter that determines whether tag text (if any) should be included when building the string or not.

Last, you need to add a number of class members, each of which processes a particular type of sub-item. Not all of these members might be required for the functionality of your plug-in. For example, in our simple implementation we do not need to consider revision markers, comments, etc. However, all of these members need to be present according to the **IMarkupDataVisitor** interface. Actually, we are mainly interested in opening and closing tags, as well as standalone placeholder tags. Depending on whether the value of the `IncludeTagText` boolean property is True or False, we append the corresponding tag text to the segment text (which should be checked) or not.

The Complete Text Extractor Class
----
The complete TextGenerator class should now look as shown below:

Add a New Control to the Plug-in User Interface
------
Of course, the new setting needs to be reflected on the plug-in settings page. For this reason, add a check box control named **cb_ConsiderTags**. Then switch to the code view of the control, and add the following boolean `ConsiderTags` property as the programmatic representation of the control element:

Enhance the Settings Class
------
In the next step you need to make certain that your `IdenticalVerifierSettings` class includes the boolean `ConsiderTags` setting:

Enhance the Settings Page Controller Class
-----
The following members of the `IdenticalVerifierUIPage` class also need to include the new setting for loading, saving, and resetting the values:

Modify the Main Verifier Class
-----
After implementing the `TextGenerator` class, we need to make a small change to the main verifier class, so that it either retrieves the segment text only or the segment text plus tag text for verification.

First, add the following members to the `IdenticalVerifierMain` class, which is derived from the `TextGenerator`:

We will call on this member from the `CheckParagraphUnit()` method to extract the segment text with or without tag text.
Now change the verification logic in the `CheckParagraphUnit()` method by making the following addition:

As you can see, the `GetPlainText()` helper function of the TextGenerator is called to retrieve the plain text without tags. However, by setting the boolean parameter (`includeTagText`) to True, you can retrieve the text with tag text. Of course, it makes sense to implement a setting on the user interface (e.g. a checkbox), which allows the users to make that decision at runtime. We will not cover this in this programming guide. However, the **Sdl.Verification.Sdk.IdenticalCheck.Extended** project, which is included in this SDK, contains the full code for the enhancement that is covered in this chapter (i.e. including the enhancements to the user interface).

The fully enhanced and modified `CheckParagraphUnit()` function should look as shown below: