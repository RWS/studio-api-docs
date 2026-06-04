# Implement the Verification Logic

Implement the actual verification logic of the native verifier plug-in.

## Add the Verification Class

Implement the verification functionality by adding a class called, for example, **XMLCheckerMain.cs**. This class requires the following namespaces:

- `Sdl.FileTypeSupport.Framework.NativeApi` — Provides access to functionality for native file types
- `Sdl.Core.Settings` — Provides functionality for initializing and using plug-in settings
- `System.Xml` — Required for XML node processing since the native file type is XML

Your class must implement these interfaces:

- [INativeFileVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml) — Provides methods for implementing native verification
- [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml) — Provides functionality for initializing plug-in settings

## Provide Access to the Native File to Verify

Add the following member to the class, which provides access to important information about the native file such as the file name, which you will later load into an XML DOM object:

# [C#](#tab/tabid-1)
```cs
private INativeOutputFileProperties _outputFileProperties;
```

## Provide Access to the Verification Settings

Add the following boolean property, which provides programmatic access to the setting from the plug-in settings page that determines whether verification logic should be applied:

# [C#](#tab/tabid-2)
```cs
public bool Enabled
{
    get;
    set;
}
```

Implement the `InitializeSettings` method of the `ISettingsAware` interface. Call the `VerifierSettings` class and use the `PopulateFromSettingsBundle` method to retrieve the setting from the stored settings bundle. Provide the settings bundle object and the file type ID (here *Length Check XML v 1.0.0.0*) as parameters. The `Enabled` property is then set according to the value retrieved from the settings bundle:

# [C#](#tab/tabid-3)
```cs
public void InitializeSettings(ISettingsBundle settingsBundle, string configurationId)
{
    VerifierSettings _settings = new VerifierSettings();
    _settings.PopulateFromSettingsBundle(settingsBundle, "Length Check XML v 1.0.0.0");
    Enabled = _settings.Enable;
}
```

## Provide Access to the Verification Message Reporter

When your verifier finds errors, notify the user. Add the following message reporter member to your class:

# [C#](#tab/tabid-4)
```cs
public INativeTextLocationMessageReporter MessageReporter
{
    get;
    set;
}
```

Through the [MessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileVerifier_MessageReporter) you output error messages to the **Messages** window of `Var:ProductName`. Users can view these messages and resolve the problem.

## Implement the Actual Verification Logic

Add the [Verify](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileVerifier_Verify) method, which is triggered when the user starts verification through the menu command **Tools** > **Verify** or by pressing **F8**. This method uses the standard XML API to perform verification. It loops through all `displaytext` elements and checks for a `maxlength` attribute. If this attribute exists, the element content is compared against the maximum length value. If the text exceeds the maximum length, the [ReportMessage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IBasicMessageReporter.yml#Sdl_FileTypeSupport_Framework_NativeApi_IBasicMessageReporter_ReportMessage_System_Object_System_String_Sdl_FileTypeSupport_Framework_NativeApi_ErrorLevel_System_String_System_String_) method adds an error message to the **Messages** window of `Var:ProductName`:

# [C#](#tab/tabid-5)
```cs
public void Verify()
{
    if (!Enabled)
    {
        return;
    }

    XmlDocument doc = new XmlDocument();
    doc.Load(_outputFileProperties.OutputFilePath);
    foreach (XmlNode item in doc.SelectNodes("//displaytext"))
    {
        // if has a max length attribute
        XmlAttribute maxlengthAttribute = item.Attributes["maxlength"];
        if ((maxlengthAttribute != null) && (!string.IsNullOrEmpty(maxlengthAttribute.Value)))
        {
            // if can parse max length and display text length greater than max length
            int lengthLimit;
            if (int.TryParse(maxlengthAttribute.Value, out lengthLimit) && (item.InnerText.Length > lengthLimit))
            {
                // report problem
                MessageReporter.ReportMessage(this, StringResources.VerifierName, ErrorLevel.Error,
                    String.Format(StringResources.ErrorText, item.InnerText.Length.ToString(), lengthLimit.ToString()),
                    String.Format(StringResources.LocationDescription, item.InnerText));
            }
        }
    }
}
```

The [ReportMessage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IBasicMessageReporter.yml#Sdl_FileTypeSupport_Framework_NativeApi_IBasicMessageReporter_ReportMessage_System_Object_System_String_Sdl_FileTypeSupport_Framework_NativeApi_ErrorLevel_System_String_System_String_) method takes these parameters:

- The name of the verifier plug-in that reports the message
- The [ErrorLevel](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ErrorLevel.yml), set to [Error](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ErrorLevel.yml#fields), the highest severity level
- A detailed description of the problem to help users understand what went wrong and take corrective action
- The location (the target segment that exceeded the specified length limit). Double-clicking the message in the **Messages** window of `Var:ProductName` displays the segment string in a message box

![Error_Message_Maxlength_XML_Exceeded](images/Error_Message_Maxlength_XML_Exceeded.jpg)

## Provide Access to the Native Output File

Add the following members, which provide programmatic access to the native output file and information such as file output path, name, encoding, creation date, and file type info. These members are not required for this sample plug-in because the simple implementation only checks for adherence to the length limit without processing the native output file. However, the [INativeFileVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml) interface requires them:

# [C#](#tab/tabid-6)
```cs
public void GetProposedOutputFileInfo(IPersistentFileConversionProperties fileProperties, IOutputFileInfo proposedFileInfo)
{
    // Not required for this implementation
}

/// <summary>
/// Provides information on output file.
/// </summary>
/// <param name="properties"></param>
public void SetOutputProperties(INativeOutputFileProperties properties)
{
    _outputFileProperties = properties;
}
```

## Putting It All Together

Your main verification class should look as follows:

# [C#](#tab/tabid-7)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.FileTypeSupport.Framework.IntegrationApi;

namespace Sdk.FileTypeSupport.Samples.XMLChecker
{
    /// <summary>
    /// This class implements the verification logic. Depending on whether the 
    /// verification plug-in is enabled or not, verification is performed when the user
    /// of Trados Studio presses F8 or invokes the menu command Tools > Verify.
    /// This class is referenced in the file type definition.
    /// </summary>
    class XMLCheckerMain : INativeFileVerifier, ISettingsAware
    {
        private INativeOutputFileProperties _outputFileProperties;

        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the plug-in settings, so that they can be used during the actual verification.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="configurationId"></param>
        public void InitializeSettings(ISettingsBundle settingsBundle, string configurationId)
        {
            VerifierSettings _settings = new VerifierSettings();
            _settings.PopulateFromSettingsBundle(settingsBundle, "Length Check XML v 1.0.0.0");
            Enabled = _settings.Enable;
        }

        public INativeTextLocationMessageReporter MessageReporter
        {
            get;
            set;
        }

        /// <summary>
        /// This method implements the main verification logic.
        /// First, load the XML document into a DOM object. Then, loop through all 'displaytext'
        /// elements and check for a 'maxlength' attribute, which indicates the maximum length
        /// in characters. If the target segment text exceeds the length limit specified by the
        /// attribute, an error message is reported. Any length limit violations are reported
        /// through the message reporter, which fills the Messages window of Trados Studio with
        /// error messages displayed to the end user.
        /// </summary>
        public void Verify()
        {
            if (!Enabled)
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(_outputFileProperties.OutputFilePath);
            foreach (XmlNode item in doc.SelectNodes("//displaytext"))
            {
                // if has a max length attribute
                XmlAttribute maxlengthAttribute = item.Attributes["maxlength"];
                if ((maxlengthAttribute != null) && (!string.IsNullOrEmpty(maxlengthAttribute.Value)))
                {
                    // if can parse max length and display text length greater than max length
                    int lengthLimit;
                    if (int.TryParse(maxlengthAttribute.Value, out lengthLimit) && (item.InnerText.Length > lengthLimit))
                    {
                        // report problem
                        MessageReporter.ReportMessage(this, StringResources.VerifierName, ErrorLevel.Error,
                            String.Format(StringResources.ErrorText, item.InnerText.Length.ToString(), lengthLimit.ToString()),
                            String.Format(StringResources.LocationDescription, item.InnerText));
                    }
                }
            }
        }

        public void GetProposedOutputFileInfo(IPersistentFileConversionProperties fileProperties, IOutputFileInfo proposedFileInfo)
        {
            // Not required for this implementation
        }

        /// <summary>
        /// Provides information on output file.
        /// </summary>
        /// <param name="properties"></param>
        public void SetOutputProperties(INativeOutputFileProperties properties)
        {
            _outputFileProperties = properties;
        }
    }
}
```

## Using the Main Verifier Class

Create a new file type definition based on the XML file type definition to use this verifier (see [Extending existing File Type Component Builder](extending_existing_file_type_component_builder.md)).

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
