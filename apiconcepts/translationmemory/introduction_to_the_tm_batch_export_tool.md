# Introduction to the Batch Export Tool

The Translation Memory API can automate repetitive tasks that would otherwise require many manual steps. This sample project shows how to build a command-line application that exports multiple file-based TMs to `.tmx` files.

## Scope of the Batch Export Application

Many users maintain a large number of file-based TMs. To export each TM in Var:ProductName, users must open every TM and export it to a `.tmx` file one by one. That process can take time and can lead to mistakes.

To automate the process, this sample project builds a simple command-line tool that scans a folder structure and exports each TM to a `.tmx` file. The application supports file-based TMs only. After the user starts the application, they enter the root path. The application then scans the folder and subfolders and exports each `.sdltm` file.

This sample demonstrates how to schedule and automate batch tasks that must run regularly.

## See Also

- [Setting up the Project](setting_up_the_project.md)
- [Looping through the Folder(s)](looping_through_the_folder.md)
- [Exporting to TMX](exporting_to_tmx.md)
