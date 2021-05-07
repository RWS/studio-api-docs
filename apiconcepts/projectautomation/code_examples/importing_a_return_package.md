Importing a Return Package
==

After the return package (see [Creating a Return Package]()) has been sent back to the project manager, e.g. via e-mail, he/she opens it in <Var:ProductName>. This basically means that the package content is extracted, and the translated, edited, or proofread bilingual (SDL XLIFF) document overwrite the previous (e.g. untranslated) files, thus updating the project on the project manager's side.

Import a Return Package Programmatically
--

Implementing a function called ```OpenPackage```, which takes the project file name (i.e. the name and path of the **.sdlproj* file) and the name and path of the return package (i.e. the **.sdlrpx* file) as string parameters.

Within the function start by opening the project as shown below:

```cs
FileBasedProject project = new FileBasedProject(projectFile);
```

To import and extract the return package content into your project, apply the [ImportReturnPackage]() method to your project to create a [ReturnPackageImport]() object. This method requires the full return package file name and path as parameter, as shown in the example function below:


```cs
ReturnPackageImport import = project.ImportReturnPackage(returnPackageFile);
```

>**Note**
>
>Note that the system 'knows' to which project a return package belongs, because the XML manifest file contains the unique project id (guid), which is used to identify the project into which the return package content should be imported. If the return package manifest contains a guid that does not match any of the projects of the system, an error will be thrown.

>**Note**
>
>After importing a return package, the return package file will be copied into the folder structure of the project. The sub-folder will be called *Packages/In*. Moreover, the package file name will be marked with the date and time of the import, e.g. *return_package-201071-13h39m34s.sdlrpx*.

>**Note**
>
>After importing the return package, the project statistics will usually change, e.g. more segments should now have the translated, reviewed, etc. state. See also [Retrieving the Project Statistics]().

>**Note**
>
>Once a return package has been imported, it cannot be imported again. If you try to re-import the same return package, an exception will be thrown.

Check the Import Status
-- 

It may happen that the return extraction and import fails for some reason (e.g. a project file has accidentally been set to read-only and thus cannot be overwritten and updated by a file in the return package). This is why you should check for the import status, and output a message to the user if required. The following sample code leverages the [PackageStatus]() class to check the project package status. It throws an error message if in the end the package status is not [Completed]():

```cs
bool packageIsImported = false;
while (!packageIsImported)
{
    switch (import.Status)
    {
        case PackageStatus.Cancelling:
        case PackageStatus.InProgress:
        case PackageStatus.Scheduled:
        case PackageStatus.NotStarted:
            packageIsImported = false;
            break;
        case PackageStatus.Cancelled:
        case PackageStatus.Completed:
        case PackageStatus.Failed:
        case PackageStatus.Invalid:
            packageIsImported = true;
            break;
        default:
            break;
    }
}

if (import.Status != PackageStatus.Completed)
{
    throw new Exception("A problem occurred during package import.");
}
```


Putting it All Together
--

The complete function should now look as shown below:

```cs
public void OpenPackage(string projectFile, string returnPackageFile)
{
    #region "EventArgs"
    List<TaskStatusEventArgs> taskStatusEventArgsList = new List<TaskStatusEventArgs>();
    List<MessageEventArgs> messageEventArgsList = new List<MessageEventArgs>();
    #endregion

    #region "OpenProject"
    FileBasedProject project = new FileBasedProject(projectFile);
    #endregion

    #region "ImportPackage"
    ReturnPackageImport import = project.ImportReturnPackage(returnPackageFile);
    #endregion

    #region "CheckImportStatus"
    bool packageIsImported = false;
    while (!packageIsImported)
    {
        switch (import.Status)
        {
            case PackageStatus.Cancelling:
            case PackageStatus.InProgress:
            case PackageStatus.Scheduled:
            case PackageStatus.NotStarted:
                packageIsImported = false;
                break;
            case PackageStatus.Cancelled:
            case PackageStatus.Completed:
            case PackageStatus.Failed:
            case PackageStatus.Invalid:
                packageIsImported = true;
                break;
            default:
                break;
        }
    }

    if (import.Status != PackageStatus.Completed)
    {
        throw new Exception("A problem occurred during package import.");
    }
    #endregion
}
```

See Also
--
**Other Resources**

Creating a Return Package

About Packages