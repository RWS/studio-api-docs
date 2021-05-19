Deleting Projects
==

Projects can take a lot of hard disk space, as they are comprised of a number of folders, sub-folders, which contain source and target documents, project termbases, etc. By applying a simple delete you can physically remove a project and all of its files.

All you need to do is to apply the [Delete](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_Delete) to a specific file-based project object:

```cs
FileBasedProject project = new FileBasedProject(GetProjectInfo());
project.Delete();
```

Note that this will delete all project files and folders including the **.sdlproj* file, so this is a potentially dangerous operation that you should carry out only if you are sure that no productive data gets lost. You could use this method, for example, when you create projects only temporarily, e.g. for the sake of generating a report. After the report has been generated, you could remove the project files, so that they no longer clutter the hard-disk unnecessarily.

See Also
--
**Other Resources**

[Setting the Project Information](setting_the_project_information.md)

[Deleting the Project](deleting_the_project.md)