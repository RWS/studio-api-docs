# Processing custom project package formats

Var:ProductName Integration API provides support for third-party developers to transform third-party packages into a format that Var:ProductName can work with. 
This page will describe the steps required to create a custom package converter.

## Declaring a custom package converter

The first step in creating a custom package converter is the declaration:
- Create a class that implements [IExternalPackageConverter](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IExternalPackageConverter.yml) interface and provide specific implementations for the following methods:

    |Method Name| Description|
    | --------  | -----------|
    |`ConvertPackage`| This method allows implementing logic that will transform a third party package format into a format recognised by Var:ProductName. |
    |`ConvertReturnPackage`|This method allows conversion from the Var:ProductName return package format into the third party package format.|

- Decorate the class with the [ExternalPackageConvertorExtension](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Extensions.ExternalPackageConvertorExtensionAttribute.yml) attribute. 
 
    | Property | Description |
    | -------- | ----------- |
    | `Id`| Unique identifier for the converter. To avoid confusion with other third party plugins we recommend using a combination of the project namespace and class name. |
    | `Name` | Name of the converter |
    | `Description` | Description for the converter |
    | `PackageFileFilter` | The filter to be used by the Windows OS File Opener dialog. This allows the user to see only files that match the given extension and thus pick a valid file. |
    |`PackageFileExtension`| The file extension of the package to be imported into Var:ProductName.|
    |`ReturnPackageFileExtension`|The file extension of the return package.|

The bellow sample provides a visual to the elements described above:

```cs
[ExternalPackageConvertorExtension(
    Id = "Sdl.ProjectsOperations.Sample.SamplePackageConverter",
    Name = "Sample Package Converter",
    Description = "Sample Package Converter",
    PackageFileFilter = "Sample Packages (*.zip)|*.zip",
    PackageFileExtension = ".zip",
    ReturnPackageFileExtension = ".zip")]
public class SamplePackageConverter : IExternalPackageConverter
{
    public SamplePackageConverter()
    {
    }

    public PackageConversionResult ConversionResult {get; private set;}

    public event EventHandler<PackageConverterMessageEventArgs> PackageMessage;
    public event EventHandler<ConvertExternalPackageEventArgs> PackageConverted;

    public void ConvertPackage(IConversionContext context, ExternalPackageConversionInfo externalPackageConversionInfo)
    {
        // your costum logic here
    }

    public void ConvertReturnPackage(IConversionContext context, ExternalPackageConversionInfo externalPackageConversionInfo)
    {
        // your custom logic here
    }
}
```

> [!NOTE]
> The __Open Package__ and __Create Return Package__ wizard's `Data` object is available in the custom package converter as well, via the [ExternalPackageConversionInfo.CustomData](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.ExternalPackageConversionInfo.yml#Sdl_TranslationStudioAutomation_IntegrationApi_Packaging_ExternalPackageConversionInfo_CustomData) property, allowing third party developers to use inside the converter data that has been captured in their custom wizard pages .

## Importing a custom package
Importing a custom package is done by implementing the `ConvertPackage(IConversionContext context, ExternalPackageConversionInfo externalPackageConversionInfo)` method. The usual steps within the implementation are as follows:
- Unzipping the package
- Creating an [IPackage](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml) object by calling the [IConversionContext.CreatePackage](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IConversionContext.yml#Sdl_TranslationStudioAutomation_IntegrationApi_Packaging_IConversionContext_CreatePackage_System_String_Sdl_Core_Globalization_Language_System_DateTime_System_String_System_Guid_) method. The parameters required here are as follows

    | Parameter Name | Description |
    | -------------- | ----------- |
    |`projectName`| The name of the final project to be imported. |
    |`sourceLanguage`| The source language of the project to be imported. |
    |`createdAt`| The `DateTime` creation details. |
    |`createdBy`| The user who created the package. |
    |`originalProjectGuid`| Pass this as `Guid.Empty` if this is the first time the project is imported into Var:ProductName, otherwise pass the original project id. The presence of the project in Var:ProductName can be determined by calling [IConversionContext.CheckForExistingProject()](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IConversionContext.yml#Sdl_TranslationStudioAutomation_IntegrationApi_Packaging_IConversionContext_CheckForExistingProject_System_Guid_) 
- Adding the resources to the package [IPackage](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml) object such as: files, translation memories, termbases, specific settings
- Physically packaging the package into Var:ProductName valid format by calling [IPackage.Pack()](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml#Sdl_ProjectAutomation_Core_IPackage_Pack_System_String_) and specifying the location where the package will be saved.
- Performing cleanup on temporary resources used, such as files.

```cs
public void ConvertPackage(IConversionContext context, ExternalPackageConversionInfo externalPackageConversionInfo)
{
    OnPackageConverted(0, "Beginning conversion");

    try
    {
        var projectId = Guid.Empty;              
        var package = context.CreatePackage("SampleProject", 
                    new Language("en-us"), DateTime.Now, "sample_api", projectId);

        AddResourcesToPackage(package);

        package.Pack("C:\\SampleLocation\\sample_package.sdlppx");
    }
    finally
    {
        Cleanup();
    }
     
    OnPackageConverted(100, "Conversion Completed");
}
```

## Exporting a return package into a custom format
Exporting a return package into a custom package is done by implementing the `ConvertReturnPackage(IConversionContext context, ExternalPackageConversionInfo externalPackageConversionInfo)` method. The usual steps within the implementation are as follows:
- Creating an [IPackage](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml) object from the return package file by calling the [IConversionContext.OpenPackage](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IConversionContext.yml#Sdl_TranslationStudioAutomation_IntegrationApi_Packaging_IConversionContext_OpenPackage_System_String_) method. 
- Extracting the necessary resources from the [IPackage](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml) object and creating the custom return package.
- Performing cleanup on temporary resources used, such as files.

```cs
public void ConvertReturnPackage(IConversionContext context, ExternalPackageConversionInfo externalPackageConversionInfo)
{
    OnPackageConverted(0, "Beginning conversion");

    try
    {
        var fromPackage = context.OpenPackage(externalPackageConversionInfo.FromPackagePath);

        CreateCustomReturnPackage(fromPackage);
    }
    finally
    {
        Cleanup();
    }

    OnPackageConverted(100, "Conversion Completed");
}
```

