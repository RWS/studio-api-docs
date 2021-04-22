Looping through the Folder(s)
====
The user of your sample application is required to enter a main path. The application then loops through the files in that path and any files contained in sub-folders of the main path. The application logic for traversing the folders and sub-folders will be implemented in a dedicated iterator class. This class will be used both for looping through all *.sdltm, which then need to be exported to *.tmx.

Add a New Class
-----
Add a class called `TmIterator` to your project. At the beginning of the class, declare two constants, one to specify how deep into the sub-folder structure the recursion should go. For example, we will hard-code the depth to 10, so that the application will go down to 10 sub-folder levels. The second constant sets the recursion level, which should be 1:
# [C#](#tab/tabid-1)
```cs
/// <summary>
/// Determines how deep in the sub-folder structure
/// the application should go.
/// </summary>
public const int Depth = 10;

/// <summary>
/// Determines current level of recursion when iterating thru subfolders.
/// </summary>
public const int RecursionLevel = 1;
```
******

Then add a public function called ProcessDir:
# [C#](#tab/tabid-2)
```cs
/// <summary>
/// This function is used to iterate through the main folder and
/// (if applicable) the subfolders to look for *.sdltm files to export.
/// </summary>
/// <param name="sourceDirectory">Directory to iterate thru.</param>
/// <param name="processSubFolders">True if recursive teration thru subfolders is required.</param>
public void ProcessDirectory(string sourceDirectory, bool processSubFolders)
```
****

This function takes the main folder entered by the user in the command line interface as string parameter, and a boolean parameter that indicates whether sub-folders should be processed through self-recursion.

Implement the Recursion
----

Within the function implement an `if`, which makes the function loop through all sub-folders until the recursion level has reached the maximum depth.
# [C#](#tab/tabid-3)
```cs
// Loop until the recursion level has reached the
// maximum folder depth.
if (RecursionLevel <= Depth)
```
****

Next, you iterate through the files found in a given directory. However, the files should only be processed if they match the provided extension, i.e. *.sdltm. If the extension sdltm is encountered, an export will be triggered, which we will implement in a separate class in a later step (see [Importing into the Master Translation Memories](importing_into_the_master_translation_memories.md)).
# [C#](#tab/tabid-4)
```cs
// Retrieve the names of the files found in the given folder.
string[] fileEntries = Directory.GetFiles(sourceDirectory);
foreach (string fileName in fileEntries)
{
    if (fileName.ToLower().EndsWith(".sdltm"))
    {
        // Only process file if it is a TMX import file.
        Console.WriteLine("Exporting " + fileName);
        TMExporter exportTm = new TMExporter();
        exportTm.Export(fileName);
    }
}
```
****

After all files of the given folder have been processed accordingly, the function needs to re-trigger itself, so that any files in a sub-folder (if applicable) can be processed. The boolean `processSubFolders` parameter determines whether the recursion should be triggered or not. If this parameter is `False`, then only the main path will be processed.
# [C#](#tab/tabid-5)
```cs
// Self-recursion to loop through the folder structure until
// the folder depth has reached the recursion level value.
if (processSubFolders)
{
    string[] subdirEntries = Directory.GetDirectories(sourceDirectory);
    foreach (string subdir in subdirEntries)
    {
        if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
        {
            this.ProcessDirectory(subdir, processSubFolders);
        }
    }
}
```
****


Trigger the Function
-----
The `ProcessDir` function needs to be called from `Main` in the `Program` class as shown below. The user provides two parameters:

* The main translation memory path
* the `/y` parameter to indicate that sub-folders (if any) should be processed
  
Example:
# [](#tab/tabid-6)
```
Sdl.SDK.LanguagePlatform.Samples.BatchExport.exe c:\tm /y
```
****


The following code in `Program.cs` is used for explaining the application use and retrieves the parameters for the file processing. It also checks the validity of the input folder:
# [C#](#tab/tabid-7)
```cs
/// <summary>
/// Main entrance point of the program.
/// </summary>
/// <param name="args">Contain string parameters that are passed via the command line.</param>
public static void Main(string[] args)
{
    string mainPath = string.Empty;
    bool processSubFolders = false;

    if (args.Length != 2)
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("Sdl.SDK.LanguagePlatform.Samples.BatchExport.exe source /y");
        Console.WriteLine("source   path to input folder");
        Console.WriteLine("/y   should process subfolders");
        Console.WriteLine("This application uses a hard-coded recursion level of up to 10 sub-folders.");
        return;
    }

    if (!String.IsNullOrEmpty(args[0]) && !Directory.Exists(args[0]))
    {
        Console.WriteLine("Specify valid input directory. Press ENTER to exit.");
        return;
    }

    mainPath = args[0];

    if (!String.IsNullOrEmpty(args[1]) && args[1] == "/y")
    {
        processSubFolders = true;
    }

    try
    {
        TMIterator it = new TMIterator();
        it.ProcessDirectory(mainPath, processSubFolders);
        Console.WriteLine();
        Console.WriteLine("Batch export finished. Press ENTER to exit.");
        Console.ReadLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine("Press ENTER to exit.");
        Console.ReadLine();
    }
}

```
****

Putting it All Together
-----
The complete class should now look as shown below:
# [C#](#tab/tabid-8)
```cs
// ---------------------------------
// <copyright file="TMIterator.cs" company="SDL International">
// Copyright  2010 All Right Reserved
// </copyright>
// <author>Kostiantyn Lukianets</author>
// <email>klukianets@sdl.com</email>
// <date>2010-09-27</date>
// ---------------------------------
namespace Sdl.SDK.LanguagePlatform.Samples.BatchExport
{
    using System;
    using System.IO;

    /// <summary>
    /// Represents functionality for importing *.tmx files into FileBasedTranslationMemory.
    /// </summary>
    public class TMIterator
    {
        #region "Constants"

        /// <summary>
        /// Determines how deep in the sub-folder structure
        /// the application should go.
        /// </summary>
        public const int Depth = 10;

        /// <summary>
        /// Determines current level of recursion when iterating thru subfolders.
        /// </summary>
        public const int RecursionLevel = 1;

        #endregion

        #region "Function"

        /// <summary>
        /// This function is used to iterate through the main folder and
        /// (if applicable) the subfolders to look for *.sdltm files to export.
        /// </summary>
        /// <param name="sourceDirectory">Directory to iterate thru.</param>
        /// <param name="processSubFolders">True if recursive teration thru subfolders is required.</param>
        public void ProcessDirectory(string sourceDirectory, bool processSubFolders)
        #endregion
        {
            #region "Scan"
            // Loop until the recursion level has reached the
            // maximum folder depth.
            if (RecursionLevel <= Depth)
            #endregion
            {
                #region "ProcessFiles"

                // Retrieve the names of the files found in the given folder.
                string[] fileEntries = Directory.GetFiles(sourceDirectory);
                foreach (string fileName in fileEntries)
                {
                    if (fileName.ToLower().EndsWith(".sdltm"))
                    {
                        // Only process file if it is a TMX import file.
                        Console.WriteLine("Exporting " + fileName);
                        TMExporter exportTm = new TMExporter();
                        exportTm.Export(fileName);
                    }
                }

                #endregion

                #region "Recursion"

                // Self-recursion to loop through the folder structure until
                // the folder depth has reached the recursion level value.
                if (processSubFolders)
                {
                    string[] subdirEntries = Directory.GetDirectories(sourceDirectory);
                    foreach (string subdir in subdirEntries)
                    {
                        if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                        {
                            this.ProcessDirectory(subdir, processSubFolders);
                        }
                    }
                }

                #endregion
            }
        }
    }
}
```
***

See Also
--------
[Exporting to TMX](exporting_to_tmx.md)
