# Creating the Log File

At the end of the import process, you may want to create a log file that lists all master TMs created during the run, together with the total TU count for each TM.

## Add a New Class

Add a class named `TmLog` with a method named `CreateLogFile`. The method takes the root path for the `.tmx` import files, that is, the folder specified by the user, and is called from `Main`.

First, create the log text file as follows:
# [C#](#tab/tabid-1)
```cs
var log = new StreamWriter(translationMemoryPath + @"\log.txt");
```
****

Next, loop through the master TM files and write the master TM names and total translation unit counts:
# [C#](#tab/tabid-2)
```cs
string[] translationMemoryFiles = Directory.GetFiles(@"c:\MasterTMs");
foreach (string file in translationMemoryFiles)
{
    log.WriteLine(file);
    var tm = new FileBasedTranslationMemory(file);
    log.WriteLine("TU Count: " + tm.GetTranslationUnitCount().ToString());
    log.WriteLine();
}
```
****

Finally, close the log file:
# [C#](#tab/tabid-3)
```
log.Close();
```
***

# Putting it All Together

The complete class should look like this:
# [C#](#tab/tabid-4)
```cs
using System.IO;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace SDK.LanguagePlatform.Samples.BatchImporter
{
    /// <summary>
    /// Represents file logging functionality.
    /// </summary>
    public class TmLog
    {
        /// <summary>
        /// Creates a log file after the import operation has finished.
        /// The log file lists the master TMs created during the process
        /// together with the total TU count for each master TM.
        /// </summary>
        /// <param name="translationMemoryPath">Path to the translation memory folder.</param>
        public void CreateLogFile(string translationMemoryPath)
        {            
            var log = new StreamWriter(translationMemoryPath + @"\log.txt");
            
            string[] translationMemoryFiles = Directory.GetFiles(@"c:\MasterTMs");
            foreach (string file in translationMemoryFiles)
            {
                log.WriteLine(file);
                var tm = new FileBasedTranslationMemory(file);
                log.WriteLine("TU Count: " + tm.GetTranslationUnitCount().ToString());
                log.WriteLine();
            }            
                       
            log.Close();            
        }
    }
}
```
****

## See Also

- [Setting and Retrieving TM Properties](setting_and_retrieving_tm_properties.md)
