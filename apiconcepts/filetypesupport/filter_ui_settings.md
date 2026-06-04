# Filter UI settings

If your filter exposes configurable behavior, provide a user interface that lets users set those parameters easily.

## File type settings pages

Define file type settings pages in the File Type Component Builder when you build the file type information. The following example shows how to register settings pages in the `FilterComponentBuilder.BuildFileTypeInformation` method:

```cs
public IFileTypeInformation BuildFileTypeInformation(string name)
{
    var info = FileTypeManager.BuildFileTypeInformation();

    info.FileTypeDefinitionId = new FileTypeDefinitionId("Simple Text Filter 1.0.0.0");
    info.FileTypeName = new LocalizableString("Simple text files");
    info.FileTypeDocumentName = new LocalizableString("Test text files");
    info.FileTypeDocumentsName = new LocalizableString("Simple text files");
    info.Description = new LocalizableString("This sample filter processes simple text files.");
    info.FileDialogWildcardExpression = "*.text";
    info.DefaultFileExtension = "text";
    info.Icon = new IconDescriptor("assembly://Sdl.Sdk.FileTypeSupport.Samples.SimpleText/Sdl.Sdk.FileTypeSupport.Samples.SimpleText.SimpleText.ico");
    info.WinFormSettingsPageIds = new string[]
    {
        "SimpleText_Settings",
        "QuickInserts_Settings",
    };

    return info;
}
```

Each settings page defines its own ID in the settings page class. The **Plugin Framework** uses that ID to locate the plug-in and display the page in the host application at run time.

## See also

- [Implementing the UI Controller Class](implementing_the_ui_controller_class.md)
- [Implementing the Settings UI](implementing_the_settings_ui.md)

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
