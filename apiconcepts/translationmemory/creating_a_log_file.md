Creating the Log File
======
Let us assume that at the end of the import process you would like to create a log file that lists all master TMs that were created alongside the total TU count per TM.

Add a New Class
-----
Add a class called TmLog, which includes the function CreateLogFile. This function, which takes the main path for the *.tmx import files (i.e. the folder specified by the user) as parameter, and is called from Main.

First, create the log text file as follows:

```
TextWriter log = new StreamWriter(translationMemoryPath + @"\log.txt");
```

In the next step, loop through the master TM files and output the master TM names and the total translation unit count:

```
string[] translationMemoryFiles = Directory.GetFiles(@"c:\MasterTMs");
foreach (string file in translationMemoryFiles)
{
    log.WriteLine(file);
    FileBasedTranslationMemory tm = new FileBasedTranslationMemory(file);
    log.WriteLine("TU Count: " + tm.GetTranslationUnitCount().ToString());
    log.WriteLine();
}
```

Finally, close the log file:

```
log.Close();
```

Putting it All Together
-----
The complete class should look as shown below:

```
// ---------------------------------
// <copyright file="TMLog.cs" company="SDL International">
// Copyright  2010 All Right Reserved
// </copyright>
// <author>Patrik Mazanek</author>
// <email>pmazanek@sdl.com</email>
// <date>2010-09-27</date>
// ---------------------------------
namespace Sdl.SDK.LanguagePlatform.Samples.BatchImporter
{
    using System.IO;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    /// <summary>
    /// Represents file logging functionality.
    /// </summary>
    public class TMLog
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
            TextWriter log = new StreamWriter(translationMemoryPath + @"\log.txt");
            #endregion

            #region "LoopMasterTms"
            string[] translationMemoryFiles = Directory.GetFiles(@"c:\MasterTMs");
            foreach (string file in translationMemoryFiles)
            {
                log.WriteLine(file);
                FileBasedTranslationMemory tm = new FileBasedTranslationMemory(file);
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