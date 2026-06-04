# Introduction

This guide steps you through creating a C# project in Var:VisualStudioEdition to implement a simple native verifier that works on XML documents.

## The Sample Scenario

Native verifiers work on native document output, not on intermediary (SDLXliff) files. For this sample project, consider this scenario:

You're localizing XML files with the following structure:

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

The `displaytext` element contains a `maxlength` attribute that specifies the maximum length of the enclosed text. Develop a verification plug-in that checks whether translations adhere to this length limit.

The complete Visual Studio project for this plug-in is in the **Sdl.Sdk.FilterFramework.Samples.XMLChecker** subdirectory of the samples folder. This directory also contains a sample XML file (**XMLCheckerTestFile.xml**) and a file type definition (**Length Check XML v 1.0.0.0**) that uses this verification plug-in.

## See Also

- [What is the Verification Framework?](what_is_the_verification_framework.md)
- [Create a New Project](create_a_new_native_project.md)

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
