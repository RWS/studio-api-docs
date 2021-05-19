Creating a Return Package
==

After translators/editors have fully processed the files contained in a project package (see [Creating a Project Package](creating_a_project_package.md)), they send them back in a so-called return package. Similar to a project package, a return package is technically a ZIP file that have the extension **.sdlrpx*. Return packages are usually a lot smaller than project packages, as they contain only the compressed bilingual (SDL XLIFF) files, i.e. no project TMs, reference files, or any other resources. For this reason, when you create a return package programmatically there are far less options available to configure.

Create a Return Package Programmatically
--
In the sample implementation below we use a function called ```CreatePackage```, which takes a [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) object as parameter. First, select the files that need to be included in the return package. It is possible that the original project package on which the return package is based contained several target languages, e.g. when the project package was sent to a translator or agency who can handle more than one target language. However, let us assume for the following example that there is only one target language involved, e.g. German (*de-DE*) and that all files of that target language should be returned. Use the [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo?redirectedfrom=MSDN&view=net-5.0) class to create a new Language object.

Next, apply the [GetTargetLanguageFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetTargetLanguageFiles) method to the project to retrieve the target files for the selected language and to create a [ProjectFile](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectFile.yml) array. Since the [GetTargetLanguageFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetTargetLanguageFiles) method, which we are going to apply later requires, the file ids as parameter, apply the ```GetIds``` method to create a ```Guid``` array, which contains the project file ids:

```cs
Language targetLang = new Language(CultureInfo.GetCultureInfo("de-DE"));
ProjectFile[] files = project.GetTargetLanguageFiles(targetLang);
Guid[] fileIds = files.GetIds();
```

In the next step, apply the [GetTargetLanguageFiles](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetTargetLanguageFiles) method to the project, thus creating a package object that is based on the [ReturnPackageCreation](../../api/projectautomation/Sdl.ProjectAutomation.Core.ReturnPackageCreation.yml) class. This method takes the following parameters:

* The ids of the project files to include in the package
* The return package name
* An optional comment string, e.g. a note to the project manager if there was a problem during translation that should be looked at

```cs
ReturnPackageCreation returnPackage = project.CreateReturnPackage(fileIds, "Return Package Name", "Comment: Everything went fine");
```

Finally, apply the [SavePackageAs](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_SavePackageAs_System_Guid_System_String_) method to the project to physically save the actual return package file. This method requires the package id (which you can retrieve from the  [ReturnPackageCreation](../../api/projectautomation/Sdl.ProjectAutomation.Core.ReturnPackageCreation.yml) object) and the file name and path as parameters.

```cs
project.SavePackageAs(returnPackage.PackageId, @"c\temp\return_package.sdlrpx");
```

Note that a return package may also contain only some of the files from the original project package. For example, a translator may have to return the first two out of five files earlier, because they have become more urgent. In that case you can programmatically create the package with only some of the target files by specifying the corresponding file ids during project creation.

Check the Return Package Creation Status
--
It may happen that the package creation fails for some reason (e.g. any files to include in the package have accidentally been deleted or have become corrupted). This is why you should check for any events during the project package creation, and output a message to the user if required.

The following sample code leverages the [PackageStatus](../../api/projectautomation/Sdl.ProjectAutomation.Core.PackageStatus.yml) class to check the project package status. It throws an error message if in the end the package status is not [Completed](../../api/projectautomation/Sdl.ProjectAutomation.Core.PackageStatus.yml#fields):

```cs
bool packageIsCreated = false;
while (!packageIsCreated)
{
    switch (returnPackage.Status)
    {
        case PackageStatus.Cancelling:
        case PackageStatus.InProgress:
        case PackageStatus.Scheduled:
        case PackageStatus.NotStarted:
            packageIsCreated = false;
            break;
        case PackageStatus.Cancelled:
        case PackageStatus.Completed:
        case PackageStatus.Failed:
        case PackageStatus.Invalid:
            packageIsCreated = true;
            break;
        default:
            break;
    }
}

if (returnPackage.Status != PackageStatus.Completed)
{
    throw new Exception("A problem occurred during package creation.");
}
```

Putting it All Together
--

The complete function should now look as shown below:

```cs
public void CreatePackage(FileBasedProject project)
{
    #region "FileIds"
    Language targetLang = new Language(CultureInfo.GetCultureInfo("de-DE"));
    ProjectFile[] files = project.GetTargetLanguageFiles(targetLang);
    Guid[] fileIds = files.GetIds();
    #endregion

    #region "ReturnPackage"
    ReturnPackageCreation returnPackage = project.CreateReturnPackage(fileIds, "Return Package Name", "Comment: Everything went fine");
    #endregion

    #region "PackageStatus"
    bool packageIsCreated = false;
    while (!packageIsCreated)
    {
        switch (returnPackage.Status)
        {
            case PackageStatus.Cancelling:
            case PackageStatus.InProgress:
            case PackageStatus.Scheduled:
            case PackageStatus.NotStarted:
                packageIsCreated = false;
                break;
            case PackageStatus.Cancelled:
            case PackageStatus.Completed:
            case PackageStatus.Failed:
            case PackageStatus.Invalid:
                packageIsCreated = true;
                break;
            default:
                break;
        }
    }

    if (returnPackage.Status != PackageStatus.Completed)
    {
        throw new Exception("A problem occurred during package creation.");
    }
    #endregion

    #region "SavePackage"
    project.SavePackageAs(returnPackage.PackageId, @"c\temp\return_package.sdlrpx");
    #endregion
}
```

See Also
-- 

**Other Resources**

[Creating a Project Package](creating_a_project_package.md)

[Importing a Return Package](importing_a_return_package.md)

[About Packages](about_packages.md)