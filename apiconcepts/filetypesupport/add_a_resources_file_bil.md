Add a Resources File
==

In this step, you add a resource file to the project, which contains the message strings that will be shown to end user in the UI of <Var:ProductName>.

Our verification plug-in will output error messages whenever a problem has been identified. It is, of course, not required, but recommended to enter the message strings in a resources file. Start by adding a resources file (named e.g. **Resources.resx**) to your project. The resources file should contain the following strings:

* A message stating the maximum word count (as set through the plug-in user interface) and the number of words that were actually found in the target segment.
* The name of the plug-in that has detected the problem. This is important, as a user might run several verifiers at the same time (e.g. the QA Checker, the Generic Tag Verification, and our sample WordArt checker). By including the name of the verification plug-in that generated an error, the user will know from verifier a particular message came. In the **Messages** window of <Var:ProductName> users can also sort error messages by plug-in name.


Below you see an example of a verification message as it might be thrown by our sample plug-in:

![Error_Message_Length_Worksheet_Exceeded](images/Error_Message_Length_Worksheet_Exceeded.jpg)

The resources file will look as shown below:

![WordArtVerifierResources](images/WordArtVerifierResources.jpg)

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.