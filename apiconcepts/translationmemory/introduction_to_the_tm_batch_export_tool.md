Introduction to the Batch Export Tool
=====
The Translation Memory API can be leveraged to fully automate tedious and time-consuming tasks, which usually require the user to perform a lot of mouse clicks. The sample project in this chapter describes how to develop a command line application that automates the export of several file-based TMs to *.tmx documents.

Scope of the Batch Export Application
------
Users sometimes maintain a large number of file TMs. When you want to export the content of each TM using Var:ProductName, you are required to open each TM and export its content to a *.tmx file one by one, which can be time-consuming and error-prone.

To automate this process we want to develop a simple command-line tool that automates the export by looping through a given folder/sub-folder structure and exporting the content of each TM to a *.tmx file (the application is meant to work only for file-based TMs). After starting the application the user enters the main path. The application is then supposed to loop through the main folder and any subfolders and run an export on each *.sdltm file. Note that server TMs are not supposed to be supported by this simplified sample application.

Note that this sample project is a case in point for an application that can be used to schedule and automate batch tasks that need to be performed on a regular basis.

See Also
--------
[Setting up the Project](setting_up_the_project.md)

[Looping through the Folder(s)](looping_through_the_folder.md)

[Exporting to TMX](exporting_to_tmx.md)
