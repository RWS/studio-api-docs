# Looping through the Folder(s)

The user enters a root path. The application then scans that folder and any subfolders for `.sdltm` files and exports each file to `.tmx`. The traversal logic lives in a dedicated iterator class.

## Add a New Class

Add a class named `TmIterator` to your project. At the beginning of the class, declare two constants: one to define how deep the recursion should go, and one to define the current recursion level. For this sample, hard-code the depth to 10 so the application can scan up to 10 subfolder levels. Set the initial recursion level to 1:
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

Then add a public method named `ProcessDirectory`:
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

This method takes the root folder entered by the user in the command-line interface as a string parameter and a Boolean parameter that indicates whether to process subfolders recursively.

## Implement the Recursion

Within the method, add an `if` statement that keeps the recursion within the maximum depth.
# [C#](#tab/tabid-3)
```cs
// Loop until the recursion level has reached the
// maximum folder depth.
if (RecursionLevel <= Depth)
```
****

Next, iterate through the files in the current directory. Process only files with the `.sdltm` extension. When the application finds a matching file, it triggers an export that a separate class implements later in the sample.
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
        var tmExporter = new TmExporter();
        tmExporter.Export(fileName);
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
            ProcessDirectory(subdir, processSubFolders);
        }
    }
}
```
****


Trigger the Function
-----
The `ProcessDirectory` function needs to be called from `Main` in the `Program` class as shown below. The user provides two parameters:

* The main translation memory path
* the `/y` parameter to indicate that sub-folders (if any) should be processed
  
Example:
# [](#tab/tabid-6)
```
Sdl.SDK.LanguagePlatform.Samples.BatchExport.exe c:\tm /y
```
****


The following code in `Program.cs` shows how the application reads the command-line arguments and validates the input folder:
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
        var tmIterator = new TmIterator();
        tmIterator.ProcessDirectory(mainPath, processSubFolders);
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

## Putting it All Together

The complete class should now look like this:
# [C#](#tab/tabid-8)
```cs
using System;
using System.IO;

namespace SDK.LanguagePlatform.Samples.BatchExport
{
    /// <summary>
    /// Represents functionality for importing *.tmx files into FileBasedTranslationMemory.
    /// </summary>
    public class TmIterator
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
                        var tmExporter = new TmExporter();
                        tmExporter.Export(fileName);
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
