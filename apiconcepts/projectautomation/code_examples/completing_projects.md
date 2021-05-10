Completing Projects
==

Projects can change their states during the translation process. By using the complete functionality you can change for a project the state to completed.

All you need to do is to apply the [Complete](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_GetProjectStatistics) method to a specific file-based project object:

```cs
public void CompleteProject()
{
    FileBasedProject project = new FileBasedProject(GetProjectInfo());
    project.Complete();
}
```

You could use this method, for example, when you finished the translation for a project and you want to change the status in completed.

See Also
--
**Other Resources**

[Setting the Project Information](se9tting_the_project_information.md)