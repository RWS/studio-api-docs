Creating the Log File
======
Let us assume that at the end of the import process you would like to create a log file that lists all master TMs that were created alongside the total TU count per TM.

Add a New Class
-----
Add a class called `TmLog`, which includes the function `CreateLogFile`. This function, which takes the main path for the *.tmx import files (i.e. the folder specified by the user) as parameter, and is called from `Main`.

First, create the log text file as follows:
# [C#](#tab/tabid-1)
```cs
var log = new StreamWriter(translationMemoryPath + @"\log.txt");
```
****

In the next step, loop through the master TM files and output the master TM names and the total translation unit count:
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

Putting it All Together
-----
The complete class should look as shown below:
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
        /// This function is used to create a log file after
        /// the import operation has finished. The log file
        /// shows which master TMs have been created in the process
        /// as well as the total TU count for each master TM.
        /// </summary>
        /// <param name="translationMemoryPath">Path to translation memory.</param>
        public void CreateLogFile(string translationMemoryPath)
        {
            #region "CreateLog"
            var log = new StreamWriter(translationMemoryPath + @"\log.txt");
            #endregion

            #region "LoopMasterTms"
            string[] translationMemoryFiles = Directory.GetFiles(@"c:\MasterTMs");
            foreach (string file in translationMemoryFiles)
            {
                log.WriteLine(file);
                var tm = new FileBasedTranslationMemory(file);
                log.WriteLine("TU Count: " + tm.GetTranslationUnitCount().ToString());
                log.WriteLine();
            }
            #endregion

            #region "close"
            log.Close();
            #endregion
        }
    }
}
```
****

See Also
---------
[Setting and Retrieving TM Properties](setting_and_retrieving_tm_properties.md)