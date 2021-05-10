Creating a Merged File
==

By default, when you add a native file (e.g. a Microsoft Word document) to a project, a bilingual (SDL XLIFF) intermediary file will be created from it for further processing, e.g. file analysis, pre-translation, editing, etc. As a general rule, one bilingual file will be created for each native source file per target language.

<Var:Productname>  also allows you to create a merged bilingual file that incorporates the content of several native files. This can be useful, for example, to facilitate processing multiple small HTML or XML files. This is helpful for maintaining consistency between multiple source files with a lot of cross-file repetitions. It is easier to perform QA, find/replace operations, etc. in a single master file rather than in dozens of HTML files.

At the end of the project lifecycle, the individual native target files need to be created from the bilingual merged file.

The screenshot below illustrates how several files are merged into one SDL XLIFF file in <Var:Productname> 

![MergeFiles](images/MergeFiles.jpg)

Note that you can merge files of different types into one bilingual master file, e.g. * *.doc*, * *.ppt*, * *.pdf*, * *.html*.

To merge project source files into one master file, implement a function called ```MergeFiles```, which takes a [FileBasedProject](../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) as parameter. When applying the [CreateMergedProjectFile]((../../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml#Sdl_ProjectAutomation_FileBased_FileBasedProject_CreateMergedProjectFile_System_String_System_String_System_Guid___) method to your project, you need to provide the following parameters:

* The name of the merged (SDL XLIFF) file.
* The project path in which the merged file should be created. In this implementation, we will leave this string parameter empty. This means that the merged bilingual file will be created in the source language sub-folder (e.g. *en-US*), and will later be propagated to the corresponding target language sub-folders (e.g. *de-DE* and *fr-FR*) when the copy to target languages task is executed. You would only provide a different path in project, if you wanted the system to create the merged file in another sub-folder relative to the project path, e.g. *en-US/MergedFileSubPath*. However, in this case you will need to make sure that your implementation creates this physical path, as it will not be automatically crated by the Project Automation API.
* The ids of the project files that should be merged. Note that it is, of course, possible to only merge several, but not all project files.

The following sample function shows how files can be merged programmatically. Note that, for simplicity reasons, we assume that all source files should be merged and that all source files are translatable, i.e. no reference or localizable files, which cannot be merged and would therefore have to be exluded from any kind of merge operation.

```cs
private void MergeFiles(FileBasedProject project)
{
    ProjectFile[] files = project.GetSourceLanguageFiles();
    Guid[] fileIds = files.GetIds();

    string masterFileName = "Master_File.sdlxliff";
    string pathInProject = string.Empty;
    project.CreateMergedProjectFile(masterFileName, pathInProject, fileIds);
}
```

Note that only translatable files can be merged. If you try to merge translatable files with reference files, an exception will be thrown. So, you need to make sure to set the usage (i.e. role) of the files to merge to translatable before you apply the merge operation. The following sample code sets all files in the project to translatable:

```cs
ProjectFile[] files = project.GetSourceLanguageFiles();          
project.SetFileRole(files.GetIds(), FileRole.Translatable);
```

Also note that when you decide to create a merged bilingual file, only this bilingual (SDL XLIFF) file will exist alongside the original native documents. There will be no individual SDL XLIFF files, as is the case if you do not create a merged file.

See Also
--

**Other Resources**

[Adding Files and Folders](adding_files_and_folders.md)

[Adding Files in the Folder to the Project](../\developing_a_sample_app\adding_file_in_the_folder_to_the_project.md)