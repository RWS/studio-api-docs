Introduction to the Batch Import Tool
====
In this chapter you will learn how to build a simple command-line application that loops through a folder/sub-folder structure to find *.tmx files that are then imported into file-based translation memories. The aim is to consolidate all *.tmx files of a particular language direction in one *.sdltm file.

Scope of the Batch Import Application
------
This sample application should do the following:

* Loop through a folder/sub-folder structure and look for *.tmx files.
* Automatically create translation memories (master TMs) and consolidate *.tmx files with the same language direction into one TM through multiple import operations
* Generate a text log file that lists all the master TMs and the total translation unit count for each TM


Note that this sample project is a case in point for an application that can be used to schedule and automate batch tasks that need to be performed on a regular basis.