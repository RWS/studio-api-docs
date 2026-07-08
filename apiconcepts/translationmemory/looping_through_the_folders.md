# Looping through the Folder(s)

The user enters a root path. The application scans that path and any subfolders for `.tmx` files and imports them into the master TMs. A dedicated iterator class handles the folder traversal.

## Add a New Class

Add a class named `TmIterator` to your project. At the beginning of the class, declare two constants: one to define how deep the recursion should go and one to define the current recursion level. For this sample, hard-code the depth to 10 so the application scans up to 10 subfolder levels. Set the initial recursion level to 1:

# [C#](#tab/tabid-1)
```cs
/// <summary>
/// Determines how deep in the sub-folder structure the application should go.
/// </summary>
public const int Depth = 10;

/// <summary>
/// Determines the current recursion level.
/// </summary>
private const int RecursionLevel = 1;
```
******


Then add a public method named `ProcessDirectory`:
# [C#](#tab/tabid-2)
```cs
/// <summary>
/// Iterates through the main folder and, if applicable, the subfolders to find .tmx import files.
/// </summary>
/// <param name="sourceDirectory">Directory to search.</param>
/// <param name="processSubFolders">True to process subfolders recursively.</param>
public void ProcessDirectory(string sourceDirectory, bool processSubFolders)
```
****

This method takes the root folder entered by the user in the command-line interface and a Boolean parameter that indicates whether to process subfolders recursively.

## Implement the Recursion

Within the method, add an `if` statement that keeps the recursion within the maximum depth.
# [C#](#tab/tabid-3)
```cs
// Loop until the recursion level has reached the
// maximum folder depth.
if (RecursionLevel <= Depth)
```
*****


Next, iterate through the files found in the current directory. Process only files that match the `.tmx` extension. When the application finds a `.tmx` file, it triggers an import operation that a separate class implements later in the sample (see [Importing into the Master Translation Memories](importing_into_the_master_translation_memories.md)).
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
        var tmImporter = new TmImporter();
        tmImporter.Import(fileName);
    }
}
```
***

After the method processes all files in the current folder, it calls itself again so that any files in subfolders can be processed. The Boolean `processSubFolders` parameter controls whether recursion runs. If this parameter is `False`, only the root path is processed.
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

## Trigger the Method

Call `ProcessDirectory` from `Main` in the `Program` class as shown below. The user provides two parameters:

* The main TMX file path
* The `/y` parameter to indicate that subfolders, if any, should be processed

Example:
# [](#tab/tabid-6)
```
Sdl.SDK.LanguagePlatform.Samples.BatchImport.exe c:\tm /y
```
*****


The following code in `Program.cs` shows how the application reads the command-line arguments and validates the input folder:
# [C#](#tab/tabid-7)
```cs
/// <summary>
    /// Main entry point of the application.
/// </summary>
    /// <param name="args">String arguments passed through the command line.</param>
public static void Main(string[] args)
{
    string mainPath = string.Empty;
    bool processSubFolders = false;

    if (args.Length != 2)
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("Sdl.SDK.LanguagePlatform.Samples.BatchImport.exe source /ps");
        Console.WriteLine("source path to input folder");
            Console.WriteLine("/ps   process subfolders");
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
        var tmIterator = new TmIterator();
        tmIterator.ProcessDirectory(mainPath, processSubFolders);
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
using System;
using System.IO;

namespace SDK.LanguagePlatform.Samples.BatchImporter
{
    /// <summary>
    /// Represents a class that iterates through a disk directory tree.
    /// </summary>
    public class TmIterator
    {       
        /// <summary>
        /// Determines how deep in the sub-folder structure the application should go.
        /// </summary>
        public const int Depth = 10;

        /// <summary>
        /// Determines the current recursion level.
        /// </summary>
        private const int RecursionLevel = 1;
          
        /// <summary>
        /// Iterates through the main folder and, if applicable, the subfolders to find .tmx import files.
        /// </summary>
        /// <param name="sourceDirectory">Directory to search.</param>
        /// <param name="processSubFolders">True to process subfolders recursively.</param>
        public void ProcessDirectory(string sourceDirectory, bool processSubFolders)
        {            
            // Loop until the recursion level has reached the
            // maximum folder depth.
            if (RecursionLevel <= Depth)
            {                
                // Retrieve the names of the files found in the given folder.
                string[] fileEntries = Directory.GetFiles(sourceDirectory);
                foreach (string fileName in fileEntries)
                {
                    // Only process file if it is a TMX import file.
                    if (fileName.ToLower().EndsWith(".tmx"))
                    {
                        Console.WriteLine("Importing " + fileName);
                        var tmImporter = new TmImporter();
                        tmImporter.Import(fileName);
                    }
                }                
                                
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
            }
        }
    }
}
```
***

## See Also

- [Importing into the Master Translation Memories](importing_into_the_master_translation_memories.md)
- [Creating the Master Translation Memories](creating_the_master_translation_memories.md)
- [Creating the Log File](creating_a_log_file.md)

