Looping through the Folder(s)
=====
The user of your sample application is required to enter a main path. The application then loops through the files in that path and any files contained in sub-folders of the main path. The application logic for traversing the folders and sub-folders will be implemented in a dedicated iterator class. This class will be used both for looping through all *.tmx files to import into the master TMs.

Add a New Class
----
Add a class called `TmIterator` to your project. At the beginning of the class, declare two constants, one to specify how deep into the sub-folder structure the recursion should go. For example, we will hard-code the depth to 10, so that the application will go down to 10 sub-folder levels. The second constant sets the recursion level, which should be 1:

# [C#](#tab/tabid-1)
```cs
/// <summary>
/// Determines how deep in the sub-folder structure the application should go.
/// </summary>
public const int Depth = 10;

/// <summary>
/// Determines current recursion level.
/// </summary>
private const int RecursionLevel = 1;
```
******


Then add a public function called `ProcessDir`:
# [C#](#tab/tabid-2)
```cs
/// <summary>
/// This function is used to iterate through the main folder and (if applicable) the subfolders to look for *.tmx import files.
/// </summary>
/// <param name="sourceDirectory">Directory to search in.</param>
/// <param name="processSubFolders">True if subfolder processing required.</param>
public void ProcessDir(string sourceDirectory, bool processSubFolders)
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
*****


Next, you iterate through the files found in a given directory. However, the files should only be processed if they match the provided extension, i.e. *.tmx. If the extension tmx is encountered, an import will be triggered, which we will implement in a separate class in a later step (see [Importing into the Master Translation Memories](importing_into_the_master_translation_memories.md)).
# [C#](#tab/tabid-4)
```cs
// Retrieve the names of the files found in the given folder.
string[] fileEntries = Directory.GetFiles(sourceDirectory);
foreach (string fileName in fileEntries)
{
    // Only process file if it is a TMX import file.
    if (fileName.ToLower().EndsWith(".tmx"))
    {
        Console.WriteLine("Importing " + fileName);
        TMImporter importTmx = new TMImporter();
        importTmx.Import(fileName);
    }
}
```
***

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
            this.ProcessDir(subdir, processSubFolders);
        }
    }
}
```
****

Trigger the Function
------
The `ProcessDir` function needs to be called from `Main` in the `Program` class as shown below. The user provides two parameters:

* The main TMX file path
* the `/y` parameter to indicate that sub-folders (if any) should be processed

Example:
# [](#tab/tabid-6)
```
Sdl.SDK.LanguagePlatform.Samples.BatchImport.exe c:\tm /y
```
*****


The following code in `Program.cs` is used for explaining the application use and retrieves the parameters for the file processing. It also checks the validity of the input folder:
# [C#](#tab/tabid-7)
```cs
/// <summary>
/// Main entrance point of the application.
/// </summary>
/// <param name="args">String arguments passed via command line.</param>
public static void Main(string[] args)
{
    string mainPath = string.Empty;
    bool processSubFolders = false;

    if (args.Length != 2)
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("Sdl.SDK.LanguagePlatform.Samples.BatchImport.exe source /ps");
        Console.WriteLine("source path to input folder");
        Console.WriteLine("/ps   should process subfolders");
        Console.WriteLine("This application uses a hard-coded recursion level of up to 10 sub-folders.");
        Console.WriteLine("The master TMs are created in a hard-coded location, i.e.: c:\\MasterTMs");
        return;
    }

    if (!String.IsNullOrEmpty(args[0]) && !Directory.Exists(args[0]))
    {
        Console.WriteLine("Specify a valid input directory. Press ENTER to exit.");
        return;
    }

    mainPath = args[0];

    if (!String.IsNullOrEmpty(args[1]) && args[1] == "/ps")
    {
        processSubFolders = true;
    }

    try
    {
        TMIterator it = new TMIterator();
        it.ProcessDir(mainPath, processSubFolders);
        Console.WriteLine();
        Console.WriteLine("Batch import finished. Press ENTER to exit.");
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
*****

Putting it All Together
-----
The complete class should now look as shown below:
# [C#](#tab/tabid-8)
```cs
// ---------------------------------
// <copyright file="TMIterator.cs" company="SDL International">
// Copyright  2010 All Right Reserved
// </copyright>
// <author>Patrik Mazanek</author>
// <email>pmazanek@sdl.com</email>
// <date>2010-09-27</date>
// ---------------------------------
namespace Sdl.SDK.LanguagePlatform.Samples.BatchImporter
{
    using System;
    using System.IO;

    /// <summary>
    /// Represents class able to iterate thru disk directory tree.
    /// </summary>
    public class TMIterator
    {
        #region "constants"

        /// <summary>
        /// Determines how deep in the sub-folder structure the application should go.
        /// </summary>
        public const int Depth = 10;

        /// <summary>
        /// Determines current recursion level.
        /// </summary>
        private const int RecursionLevel = 1;
        #endregion

        #region "function"

        /// <summary>
        /// This function is used to iterate through the main folder and (if applicable) the subfolders to look for *.tmx import files.
        /// </summary>
        /// <param name="sourceDirectory">Directory to search in.</param>
        /// <param name="processSubFolders">True if subfolder processing required.</param>
        public void ProcessDir(string sourceDirectory, bool processSubFolders)
        #endregion
        {
            #region "scan"
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
                    // Only process file if it is a TMX import file.
                    if (fileName.ToLower().EndsWith(".tmx"))
                    {
                        Console.WriteLine("Importing " + fileName);
                        TMImporter importTmx = new TMImporter();
                        importTmx.Import(fileName);
                    }
                }
                #endregion

                #region "recursion"
                // Self-recursion to loop through the folder structure until
                // the folder depth has reached the recursion level value.
                if (processSubFolders)
                {
                    string[] subdirEntries = Directory.GetDirectories(sourceDirectory);
                    foreach (string subdir in subdirEntries)
                    {
                        if ((File.GetAttributes(subdir) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
                        {
                            this.ProcessDir(subdir, processSubFolders);
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
---------
[Importing into the Master Translation Memories](importing_into_the_master_translation_memories.md)

[Creating the Master Translation Memories](creating_the_master_translation_memories.md)

[Creating the Log File](creating_a_log_file.md)

