Introduction
===

This guide will walk you through the creation of a C# project in Var:VisualStudioEdition for implementing a simple native verifier that works on XML documents.

The Sample Scenario
--

Native verifiers do not work on the intermediary (SDLXliff) files, but on the native document output. For this sample project let us imagine the following scenario:

You need to localize XML files that looks as shown below:

# [Xml](#tab/tabid-1)
```xml
<?xml version="1.0" encoding="utf-8" ?>
<root>
  <item>
    <text>XML (Extensible Markup Language) is a general-purpose specification for creating custom markup languages.</text>
    <displaytext maxlength="35">XML (Extensible Markup Language)</displaytext>
  </item>
</root>
```
***

Note that the ```displaytext``` element contains the attribute ```maxlength```, which specifies the maximum length of the text that is enclosed in this tag pair. Image that you need to develop a verification plug-in that checks whether the length limit specified in this attribute has been adhered to during translation.

You will find the complete Visual Studio project for this plug-in in the **Sdl.Sdk.FilterFramework.Samples.XMLChecker** sub-directory of the samples folder. This directory also contains a small sample XML file (**XMLCheckerTestFile.xml**) and a new file type definition (**Length Check XML v 1.0.0.0**) that uses this verification plug-in.

See Also
--



[What is the Verification Framework?](what_is_the_verification_framework.md)

[Create a New Project](create_a_new_native_project.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
