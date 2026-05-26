# Introduction to the Batch Import Tool

In this chapter, you will build a simple command-line application that scans a folder structure for `.tmx` files and imports them into file-based translation memories. The goal is to consolidate all `.tmx` files for a given language direction into one `.sdltm` file.

## Scope of the Batch Import Application

This sample application does the following:

- Scans a folder and its subfolders for `.tmx` files.
- Creates translation memories, or master TMs, automatically and consolidates `.tmx` files with the same language direction into one TM through multiple import operations.
- Generates a text log file that lists all master TMs and the total translation unit count for each TM.

> [!NOTE]
> 
> This sample project shows how to build an application that can schedule and automate batch tasks that need to run regularly.

## See Also

- [Setting up the Project](setting_up_the_import_project.md)
- [Looping through the Folder(s)](looping_through_the_folders.md)
- [Importing into the Master Translation Memories](importing_into_the_master_translation_memories.md)
- [Creating the Master Translation Memories](creating_the_master_translation_memories.md)
- [Creating the Log File](creating_a_log_file.md)
