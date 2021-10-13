Creating Projects Based on Previous Projects
==

<Var:Productname> allows you to create new projects based on previous projects. That way you can leverage the settings that were used in the previous project such as the language pairs, the task settings, the TM and termbase selection, etc.

Example: Suppose that last year you have processed a project called "Spelling Checker 1.0". This year, you need to localize the documentation for version 2.0 of the product. In such a case, it makes sense to create the new project based on the previous project, thereby re-using the settings that exist in the previous project. Only the updated source files then need to be selected, the remaining parameters (except the due date) can usually stay as is. However, of course, you can change any of the existing parameters, e.g. add a new language direction, if the updated product needs to be distributed in a new market. The **New Project** wizard of <Var:Productname> allows you to select a previous project from a dropdown list. Your API-based application could be made, for example, to select the **.sdlproj* file of the previous project to accelerate the project creation.

Create an Update Project Programmatically
--

Implement a function called ```CreateBasedOnPreviousProject```. Leverage the [ProjectReference](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectReference.yml) class to create a reference project object, which holds the previous project that our new (update) project should be based upon. When creating the reference project object provide the **.sdlproj* file name and path as string parameter:

# [C#](#tab/tabid-1)
```cs
string refProjFile = @"C:\temp\RefProject.sdlproj";
ProjectReference refProject = new ProjectReference(refProjFile);
```
***

In the next step, use the [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) class to create the object for the update project (i.e. the object for the new project that should be based on the previous project). Here, you need to provide the project information (which we retrieve through a separate helper function) and the reference project object as parameters. Finally, persist the update project by applying the [Save](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_Save) method:

# [C#](#tab/tabid-2)
```cs
FileBasedProject updateProject = new FileBasedProject(this.GetUpdateProjectInfo(), refProject);

updateProject.Save();
```
***

Implement the separate helper function that returns the information for the update project. As most of the settings are going to be taken over from the reference project, we only set the options that are likely to change in an update project such as the:

* Project name
* Project due date
* Project location (i.e. the project folder)

# [C#](#tab/tabid-3)
```cs
ProjectInfo info = new ProjectInfo();

info.Name = "My update project";
info.DueDate = DateTime.Now.AddDays(3);

string localProjectFolder = string.Format(
        CultureInfo.CurrentCulture,
        "{0){1}{2}{1}Projects{1}{3}",
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString(),
        Path.DirectorySeparatorChar, Versioning.Versions.StudioDocumentsFolderName,
        info.Name);
info.LocalProjectFolder = localProjectFolder;

return info;
```
***

Note that the updated source files to translate would also have to be added to the update project, as these will not be taken over from the previous (reference) project. For an example of how to add files to a project see [Adding Files and Folders](adding_files_and_folders.md).

Putting it All Together
--

The complete function should look as shown below:

# [C#](#tab/tabid-4)
```cs
public void CreateBasedOnPreviousProject()
{
    #region "RefProject"
    string refProjFile = @"C:\temp\RefProject.sdlproj";
    ProjectReference refProject = new ProjectReference(refProjFile);
    #endregion

    #region "UpdateProject"
    FileBasedProject updateProject = new FileBasedProject(this.GetUpdateProjectInfo(), refProject);

    updateProject.Save();
    #endregion
}
```
***

# [C#](#tab/tabid-5)
```cs
public ProjectInfo GetUpdateProjectInfo()
{
    #region "UpdateProjectInfo"
    ProjectInfo info = new ProjectInfo();

    info.Name = "My update project";
    info.DueDate = DateTime.Now.AddDays(3);

    string localProjectFolder = string.Format(
        CultureInfo.CurrentCulture,
        "{0){1}{2}{1}Projects{1}{3}",
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString(),
        Path.DirectorySeparatorChar, Versioning.Versions.StudioDocumentsFolderName,
        info.Name);
    info.LocalProjectFolder = localProjectFolder;

    return info;
    #endregion
}
```
***

See Also
--

[Setting the Project Information](setting_the_project_information.md)

[Configuring the Project Properties](configuring_the_project_properties.md)