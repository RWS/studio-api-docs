Creating Projects Based on Templates
==

The creation of new projects can further be streamlined through the use of project templates. This is somewhat similar to leveraging reference projects (see [Creating Projects Based on Previous Projects](creating_project_based_on_prev_proj.md)). However, in this case you do not use an **.sdlproj* file, but an **.sdltlp* template file, which contains settings such as the selection of TMs (i.e. the translation provider configuration), termbases, task settings, source / target languages, etc.

While reference projects are mainly used when you need to handle update projects based on previous projects, project templates help streamline the creation of multiple similar projects, e.g. when a customer requires a specific project structure.

Create a Project Programmatically Based on a Template
--

Start by implementing a function called ```CreateBasedOnTemplate```. Use the [ProjectTemplateReference](../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectTemplateReference.yml) class to create a project template object. For this, you just need to provide the full name and path of the **.sdltpl* project template file:

# [C#](#tab/tabid-1)
```cs
string templateFile = @"c:\temp\project_template.sdltpl";
ProjectTemplateReference template = new ProjectTemplateReference(templateFile);
```
***

In the next step use the [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) class to create the new project. As parameters, you need to provide the project information (which we will retrieve through a separate helper function) and the project template object. Finally, apply the  [Save](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_Save) method to persist the project in an **.sdlproj* file.
While the project template contains settings such as the source / target languages, translation providers, task settings, etc., some variable options still need to be configured, i.e. the:

* Project name
* Project due date
* Project location (i.e. the project files folder)

Below you see an example of how to configure the above settings:

# [C#](#tab/tabid-2)
```cs
ProjectInfo info = new ProjectInfo();

info.Name = "Project based on Template";
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

Putting it All Together
--

The two complete functions should now look as shown below:

# [C#](#tab/tabid-3)
```cs
public void CreateBasedOnTemplate()
{
    #region "GetTemplate"
    string templateFile = @"c:\temp\project_template.sdltpl";
    ProjectTemplateReference template = new ProjectTemplateReference(templateFile);
    #endregion

    #region "TemplateBasedProject"
    FileBasedProject newProject = new FileBasedProject(this.GetInfoForTemplateProject(), template);
    newProject.Save();
    #endregion
}
```
***

# [C#](#tab/tabid-4)
```cs
public ProjectInfo GetInfoForTemplateProject()
{
    #region "TemplateProjectInfo"
    ProjectInfo info = new ProjectInfo();

    info.Name = "Project based on Template";
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

[Project Configuration](project_configuration.md)
