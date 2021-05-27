Filter UI Settings
===

To configure the filter functionality you may need a user interface through which any parameters can be set in a convenient way.

File Type Settings Pages
--
File type settings pages can be defined in the File Type Component Builder while building the File Type Information. The example below shows how to do this inside the FilterComponentBuilder class inside the BuildFileTypeInformation method.

# [C#](#tab/tabid-1)
```cs
   public IFileTypeInformation BuildFileTypeInformation(string name)
{
    var info = this.FileTypeManager.BuildFileTypeInformation();

    info.FileTypeDefinitionId = new FileTypeDefinitionId("Simple Text Filter 1.0.0.0");
    info.FileTypeName = new LocalizableString("Simple text files");
    info.FileTypeDocumentName = new LocalizableString("Test text files");
    info.FileTypeDocumentsName = new LocalizableString("Simple text files");
    info.Description = new LocalizableString("This sample filter is used to process simple text files.");
    info.FileDialogWildcardExpression = "*.text";
    info.DefaultFileExtension = "text";
    info.Icon = new IconDescriptor("assembly://Sdl.Sdk.FileTypeSupport.Samples.SimpleText/Sdl.Sdk.FileTypeSupport.Samples.SimpleText.SimpleText.ico");

    // Define setting pages.
    info.WinFormSettingsPageIds = new string[]
    {
        "SimpleText_Settings",
        "QuickInserts_Settings",
    };

    return info;
}
```
***


The ID of the settings page is defined within the settings page class itself. The **SDL Plugin Framework** is used to locate the plug-in with this ID and display it to the user in the host application at runtime.

See Also
--



[Implementing the UI Controller Class](implementing_the_ui_controller_class.md)

[Implementing the Settings UI](implementing_the_settings_ui.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.