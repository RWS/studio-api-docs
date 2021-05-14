Implement the Verification Logic
==

In this chapter you will learn how to implement the actual verification logic of our native verifier plug-in.

Add the Verification Class
--

In this chapter, we implement the verification functionality. Start by adding a class called e.g. **XMLCheckerMain.cs**. This class needs to use the following namespaces:

* **Sdl.FileTypeSupport.Framework.NativeApi**: Provides access the functionality to be applied to native file types:
* **Sdl.Core.Settings**: Provides the functionality for initializing and using the plug-in settings.
* **System.Xml**: As the native file type is XML, this namespace is required for accessing the methods used for XML node processing.

Moreover, the class needs to use the following interfaces:
* [INativeFileVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml): Provides the methods required for implementing a native verification.
* [ISettingsAware](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISettingsAware.yml): Provides the functionality for initializing the plug-in settings.

Provide Access to the Native File to Verify
--

Add the following member to the class, which provides access to important information on the native file such as the name of the file, which you will later load into an XML DOM object.

```cs
private INativeOutputFileProperties _outputFileProperties;
```

Provide Access to the Verification Settings
--

Now add the following boolean property, which provides programmatic access to the setting of the plug-in settings page according to which the verification logic should be applied or not.

```cs
public bool Enabled
{
    get;
    set;
}
```


Next, implement the ```InitializeSettings``` method of the **ISettingsAware** interface. Here, we call on the ```VerifierSettings``` class and use the ```PopulateFromSettingsBundle``` method to retrieve the setting from the physically stored settings bundle. To do this, we need to provide the settings bundle object and the file type id (here *Length Check XML v 1.0.0.0*) as parameters. The ```Enabled``` property used in our main verification logic will then be set according to the value retrieved from the settings bundle.

```cs
public void InitializeSettings(ISettingsBundle settingsBundle, string configurationId)
{
    VerifierSettings _settings = new VerifierSettings();
    _settings.PopulateFromSettingsBundle(settingsBundle, "Length Check XML v 1.0.0.0");
    Enabled = _settings.Enable;
}
```

Provide Access to the Verification Message Reporter
--

If your verifier finds any errors in the file, the user should be notified. For this, add the following message reporter member to your class:

```cs
public INativeTextLocationMessageReporter MessageReporter
{
    get;
    set;
}
```

Through the [MessageReporter](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileVerifier_MessageReporter) you can output error messages (if any) to the **Messages** window of <Var:ProductName>. Users can view these messages and resolve the problem.

Implement the Actual Verification Logic
--

Add the [Verify](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml#Sdl_FileTypeSupport_Framework_NativeApi_INativeFileVerifier_Verify) method, which is triggered when the user starts the plug-in through trough the menu command **Tools** > **Verify** or by pressing **F8**. This function uses the standard XML API (i.e. the API of the native format) to perform the verification. It loops through all ```displaytext``` elements and checks for the presence of a ```maxlength``` attribute. If this attribute is found, the element content is compared against the maximum length value. If the text exceeds the maximum length, the [ReportMessage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IBasicMessageReporter.yml#Sdl_FileTypeSupport_Framework_NativeApi_IBasicMessageReporter_ReportMessage_System_Object_System_String_Sdl_FileTypeSupport_Framework_NativeApi_ErrorLevel_System_String_System_String_) method is used to add an error message (i.e. a message with the highest [ErrorLevel](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ErrorLevel.yml)) to the **Messages** window of <Var:ProductName>.

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

The [ReportMessage](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.IBasicMessageReporter.yml#Sdl_FileTypeSupport_Framework_NativeApi_IBasicMessageReporter_ReportMessage_System_Object_System_String_Sdl_FileTypeSupport_Framework_NativeApi_ErrorLevel_System_String_System_String_) method takes the following parameters:
* The name of the verifier plug-in that has thrown the message
* The [ErrorLevel](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ErrorLevel.yml), which in this case we set to [Error](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.ErrorLevel.yml#fields), which is the highest severity level.
* A detailed description of the problem, which helps the user ascertain what the problem is and take corrective action.
* The location, i.e. the target segment that was found to exceed the specified length limit. Double-clicking the message in the Messages window of <Var:ProductName> will display the segment string in a message box.

![Error_Message_Maxlength_XML_Exceeded](images/Error_Message_Maxlength_XML_Exceeded.jpg)

Provide Access to the Native Output File
--

Last, add the following members, which provide programmatic access to the native output file, and thus to information such as the file output path and name, the file encoding, creation date, file type info, etc. These members are not actually required for our sample plug-in, as our simple implementation just checks for adherence to the length limit, but does not do any processing on the native output file. However, these members need to be present in our class according the the [INativeFileVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml) interface.

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

Putting it all Together
--

All put together your main verification class should now look as shown below:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Sdl.Core.Settings;
using Sdl.FileTypeSupport.Framework.NativeApi;
using Sdl.FileTypeSupport.Framework.IntegrationApi;


namespace Sdl.Sdk.FileTypeSupport.Samples.XMLChecker
{
    /// <summary>
    /// This class implements the verification logic. Depending on whether the 
    /// verification plug-in is enabled or not, a verification will be performed
    /// when the user of SDL Trados Studio presses F8 or invokes the menu command
    /// Tools -> Verify.
    /// This class is referenced in the file type definition.
    /// </summary>
    class XMLCheckerMain : INativeFileVerifier, ISettingsAware
    {
        #region "_outputFileProperties"
        private INativeOutputFileProperties _outputFileProperties;
        #endregion

        #region "UISettingsRepresentation"
        public bool Enabled
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Initializes the plug-in settings, so that they can be used during the actual verification.
        /// </summary>
        /// <param name="settingsBundle"></param>
        /// <param name="configurationId"></param>
        #region "InitializeSettings"
        public void InitializeSettings(ISettingsBundle settingsBundle, string configurationId)
        {
            VerifierSettings _settings = new VerifierSettings();
            _settings.PopulateFromSettingsBundle(settingsBundle, "Length Check XML v 1.0.0.0");
            Enabled = _settings.Enable;
        }
        #endregion



        #region "message reporter"
        public INativeTextLocationMessageReporter MessageReporter
        {
            get;
            set;
        }
        #endregion


        /// <summary>
        /// This method implements the main verification logic.
        /// First, the XML document is loaded into a DOM object.
        /// Then, the method loops through all the 'displaytext' elements, and
        /// checks for the presence of a 'maxlength' attribute, which indicates
        /// the maximum length in characters. If the target segment text exceeds the
        /// length limit specified by the attribute, an error message will be reported.
        /// Any length limit violations will be reported through the message reporter,
        /// which will fill the Messages window of SDL Trados Studio with the error
        /// messages that will be displayed to the end user.
        /// </summary>
        #region "verification logic"
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
        #endregion



        #region "INativeOutputSettingsAware Members"
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
        #endregion
    }
}
```

Using the Main Verifier Class
--

To use this verifier, a new file type definition based upon the XML file type definition needs to be created (see [Extending existing File Type Component Builder](extending_existing_file_type_component_builder.md)).

>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.