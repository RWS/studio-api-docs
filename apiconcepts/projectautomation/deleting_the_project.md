Deleting the Project
==

After the reports have been generated, the application should 'clean up after itself', i.e. it should remove all project files, so that it can be run multiple times without causing exceptions due to an already existing folder and without cluttering the hard disk. The user can, however, set a command-line parameter to keep the project files if e.g. it can be assumed that the project will go ahead anyway. In this case, the project files will be required for generating project packages to send out to translators and editors.

To do this apply the `Delete` method to the project object. Remember that the project files and folder structure are deleted by default unless the `keepProjectFiles` variable is True.

# [C#](#tab/tabid-1)
```cs
if (!keepProjectFiles)
{
    newProject.Delete();
}
```
***

See Also
--
[Deleting Projects](deleting_projects.md)