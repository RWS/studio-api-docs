# Processing custom project package formats

The Var:ProductName Integration API enables you to transform third-party packages into a format that Var:ProductName can work with. This page describes the steps required to create a custom package converter.

## Declare a custom package converter

Creating a custom package converter starts with the declaration:

- Implement the [IExternalPackageConverter](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IExternalPackageConverter.yml) interface with the following methods:

    | Method Name | Description |
    | -------- | ----------- |
    | `ConvertPackage` | Transforms a third-party package format into a format Var:ProductName recognizes. |
    | `ConvertReturnPackage` | Converts a Var:ProductName return package into the third-party package format. |

- Decorate the class with the [ExternalPackageConvertorExtension](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Extensions.ExternalPackageConvertorExtensionAttribute.yml) attribute:

    | Property | Description |
    | -------- | ----------- |
    | `Id` | Unique identifier for the converter. We recommend using a combination of the project namespace and class name to avoid conflicts. |
    | `Name` | Display name for the converter |
    | `Description` | Description of the converter |
    | `PackageFileFilter` | File filter for the Windows file dialog (e.g., "Sample Packages (*.zip)\|*.zip"). This lets users see only compatible files. |
    | `PackageFileExtension` | File extension of the package to import (e.g., ".zip") |
    | `ReturnPackageFileExtension` | File extension of the return package (e.g., ".zip") |

The following code sample shows the basic structure:

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
> The **Open Package** and **Create Return Package** wizards' `Data` object is available in the custom package converter through the [ExternalPackageConversionInfo.CustomData](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.ExternalPackageConversionInfo.yml#Sdl_TranslationStudioAutomation_IntegrationApi_Packaging_ExternalPackageConversionInfo_CustomData) property. This allows third-party developers to use data captured in their custom wizard pages inside the converter.

## Import a custom package

Importing a custom package involves implementing the `ConvertPackage(IConversionContext context, ExternalPackageConversionInfo externalPackageConversionInfo)` method. Typical implementation steps include:

- Unzip the package
- Create an [IPackage](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml) object by calling [IConversionContext.CreatePackage](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IConversionContext.yml#Sdl_TranslationStudioAutomation_IntegrationApi_Packaging_IConversionContext_CreatePackage_System_String_Sdl_Core_Globalization_Language_System_DateTime_System_String_System_Guid_) with these parameters:

    | Parameter Name | Description |
    | -------------- | ----------- |
    | `projectName` | The name of the final project to import. |
    | `sourceLanguage` | The source language of the project to import. |
    | `createdAt` | The `DateTime` creation details. |
    | `createdBy` | The user who created the package. |
    | `originalProjectGuid` | Pass `Guid.Empty` if this is the first import into Var:ProductName. Otherwise, pass the original project ID. Use [IConversionContext.CheckForExistingProject()](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IConversionContext.yml#Sdl_TranslationStudioAutomation_IntegrationApi_Packaging_IConversionContext_CheckForExistingProject_System_Guid_) to verify if the project exists in Var:ProductName. |

- Add resources to the [IPackage](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml) object: files, translation memories, termbases, and settings
- Package the converted data into Var:ProductName format by calling [IPackage.Pack()](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml#Sdl_ProjectAutomation_Core_IPackage_Pack_System_String_) with the target location
- Clean up temporary resources and files

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

## Export a return package to a custom format

Exporting a return package to a custom format involves implementing the `ConvertReturnPackage(IConversionContext context, ExternalPackageConversionInfo externalPackageConversionInfo)` method. Typical implementation steps include:

- Open an [IPackage](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml) object from the return package file by calling [IConversionContext.OpenPackage](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.IConversionContext.yml#Sdl_TranslationStudioAutomation_IntegrationApi_Packaging_IConversionContext_OpenPackage_System_String_)
- Extract the required resources from the [IPackage](../../api/projectautomation/Sdl.ProjectAutomation.Core.IPackage.yml) object and create the custom return package
- Clean up temporary resources and files

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

