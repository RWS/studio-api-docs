Introduction to the Sample Application
==

This chapter will walk you step by step through the creation of a command-line sample application that leverages the Project Automation API.

Imagine that you need to develop a command-line utility that can quickly analyze a large number of files stored in a folder/sub-folder structure. The aim is to provide a cost estimate based on the analyze files report. You therefore need to create an application that loops through the folder/sub-folder structure and performs an analyze task on all the translatable files that are found therein, and then generates a report. In detail, the application should do the following:

* Loop through a specified folder and its sub-folders (if any).
* Identify translatable files (e.g. DOC, PPT, etc.).
* Create a (temporary) project in the background and add the translatable files that were found in this folder to the project. Through a parameter the user should be able to configure whether sub-folders should be taken into account or not.
* Convert all translatable files to a translatable, bilingual format (i.e. SDLXliff) for further processing.
* Apply an analyze files batch task to the translatable documents in the project.
* Generate a file analysis reports in the standard Microsoft Excel (e.g. for printing). Apart from that, a summary of the file analysis results should be displayed in the application console.
* After the reports have been generated, the project folder and files are by default automatically deleted (unless specified otherwise by the user), as the project is not supposed to be used any further. The expected end result is only the report for the cost estimate.

At runtime, the user needs to be able to provide the following parameters:

* The (main) folder name
* The TM(s) to use for the file analysis
* Whether sub-folders should be included or not (recursion)
* Whether cross-file repetitions and the internal fuzzy match leverage should be reported. For detailed information on these settings, see [Analyze Files Settings](analyze_files_settings.md)
* Whether the project files should be deleted after the analysis report has been generated. By default, the project files should be removed from the hard disk to avoid cluttering.

The application to develop within the scope of the sample project constitutes a realistic use case. For comprehensive projects a large number of files usually needs to be processed in some ways. With a command-line application you can effectively automate such tasks, which can also be run as a scheduled process.

See Also
--
[Setting up the Visual Studio Project](setting_up_the_visual_studio_project.md)

[Configuring the Project Properties](configuring_the_project_properties.md)

[Adding Files in the Folder to the Project](adding_file_in_the_folder_to_the_project.md)

[Adding the TM to the Project](adding_tm_to_the_project.md)

[Configuring the Analyze Task Settings](configuring_the_analyze_task_settings.md)

[Analyzing the Files](analyzing_the_files.md)

[Generating the Task Report](generating_the_task_report.md)

[Putting it All Together](putting_it_all_together.md)