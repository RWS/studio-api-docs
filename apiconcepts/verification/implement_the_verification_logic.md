Implement the Verification Logic
=====
In this chapter you will learn how to implement the extension class that contains actual verification functionality of your plug-in.

Edit the Main Verifier Class
----
Now it is time to implement the verification logic in the main verification class (i.e. IdenticalVerifierMain.cs), which you added previously (see Create a New Project). Start by adding the following namespaces to this class:

* **Sdl.Verification.Api**
* **Sdl.Desktop.Platform.Settings**
* **Sdl.Desktop.Common**
* **Sdl.FileTypeSupport.Framework.BilingualApi**
* **Sdl.FileTypeSupport.Framework.NativeApi**
* **Sdl.Core.Settings**
  
This class needs to be preceded by the following declaration, which makes it an extension class, which is referenced in the plug-in manifest (see also Create a New Project).



This line is what makes the plug-in be listed under Verification in the Options or in the Project (Template) Settings dialog box of SDL Trados Studio 2017.
This class needs to implement the interfaces listed below:

* IGlobalVerifier
* **IBilingualVerifier**
* **ISharedObjectsAware**
  
Moreover, add a private member to the class, which you call e.g. `_verificationSettings`. This object is derived from the `IdenticalVerifierSettings` class (see Retrieve the Settings Values) and it is used to access the plug-in settings, which influence how the actual verification is executed:

Add the Plug-in Name, Icon and Description
----
Add the following members to implement the plug-in icon, name, and description. Note that these elements are drawn from the resources file (see The Resources File). This controls what end users will see in the **Options** dialog box of SDL Trados Studio 2017 under **Verification** in terms of strings and icons.

Retrieve the Verifier Settings
-----
In the next step, add the following internal member. This member is used to get a handle on the settings bundle used for our implementation:

Add the Item Factory Member
-----
The item factory allows you to create, for example, tag pairs and placeholders. It is actually *not* required for the functionality of our verifier plug-in, however, it has to be present in our class according to the **IBilingualVerifier** interface.

Add the Message Reporter Member
-----
The message reporter is required by the **IBilingualVerifier** interface to implement the functionality of our verifier. Through this member you output messages (if any) to the **Messages** window of Trados Studio 2017. Therefore, this member is responsible for communicating any problems to the end user, who will then try to fix the reported problems.

# [C#](#tab/tabid-1)
[!code-csharp[TermVerifierMessageService](code_samples/TermVerifierMessageService.cs#L20)]
***

Add Further Members of IBilingualContentHandler
-----
The **IBilingualVerifier** interface requires you to add a number of further members, which are actually not required for the functionality of our particular sample plug-in. You can therefore leave these methods empty.

> [!NOTE]
> An intermediary (XLIFF) document to verify might contain a number of individual documents, as Trados Studio 2017 allows you to merge several native files into one intermediary master document. Through the `FileComplete` method you can determine what should happen after a particular file has been verified. With `Complete` you can determine what should happen after the entire (merged) document has been verified.

Through the object derived from **IFileProperties** you can retrieve various information on an individual file such as the original encoding, the original file path, etc. Through an object that is derived from **IDocumentProperties** you can retrieve various information on the entire bilingual document such as the source and target languages, the source count, repetitions, etc.

Process the Paragraph Units
-----
The plug-in loops through all the paragraph units in a given bilingual document. In our implementation, we should use the **ProcessParagraphUnit** method to determine whether the plug-in is enabled or not in the first place. Remember that users can enable or disable global verifier plugs-ins through a checkbox next to the plug-in name. If the plug-in is not enabled, then nothing happens. If the plug-in is active, a separate helper function (which we will implement later) is called to carry out the actual verification.

Implement the Actual Verification Logic
------
The verification logic is contained in a separate helper function that works as follows: this function determines the context information (if any) of the current paragraph unit and then loops through the unit's segment pairs. It then determines whether the context fits the context that has been specified through the user interface. If this is the case, the method compares the source and the target segment. If the segments are not identical, a message will be generated.


The **ReportMessage** method has the following parameters:

* The name of the verifier plug-in that has thrown the message
* The error level, which in this case we set to **Warning**.
* A detailed description of the problem, which helps users ascertain what the problem is due to and take corrective action.
* The start and end location of the target string that has caused the problem. By specifying the 'from' and 'up to' location you allow the users to jump to the problematic target segment in the document by double-clicking the error message in the **Messages** window.
  
Putting it All Together
------

The complete main verification class will look as shown below:


Ignoring Track Changes
-----
Verifiers should ignore any segments that have track changes. If a user has pending track changes on a segment then that means that they have not finished changing the segment and so that segment should not be verified. Furthermore, if a verifier tries to account for track changes then it will make the verifier unnecessarily complex.

The example given above does not ignore track changes but any production verifiers should do so. Usually, a verifier would implement a **IMarkupDataVisitor** to extract the relevant information from the segment. If this visitor visits a revision marker then that segment has track changes and should be ignored. Below is an example of a **IMarkupDataVisitor** that determines whether a segment has track changes. If it visits a revision marker then it sets the property **HasRevisions**.

All verifiers should ignore any segments that have track changes apart from the **Document Verifier**. For each segment with track changes, the **Document Verifier** generates a warning message "Segment X could not be verified because it contains track changes" so that the user is aware of any segments that have not been verified.