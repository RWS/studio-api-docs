The Command-line Parameters
==

This chapter explains which parameters are used in the command-line application, how to set them in the Program.cs class, and check for the validity of the parameters.

Add the Required Variables
--

Within main declare the three following variables for setting the following parameters:

* The main document path
* The TM (*.sdltm) file or name of the TM on a server if the /p parameter is used)
* A boolean parameter to set whether sub-folders should be processed as well (False by default)
* Boolean parameters to set whether cross-file repetitions and the internal fuzzy match leverage should be reported (False by default)
* A boolean parameter that specifies whether the project files should be kept on the hard disk (False by default)
* A boolean parameter that specifies whether the project files should be published to a server (False by default)

```cs
string mainPath = string.Empty;
string tmFile = string.Empty;
bool processSubFolders = false;
bool reportCrossFileRepetitions = false;
bool reportInternalFuzzyMatchLeverage = false;
bool keepProjectFiles = false;
bool publishToServer = false;
```

If the two parameters, main file path and TM have not been entered, the application should output some explanation on the usage:


```cs
if (args.Length < 2)
{
    Console.WriteLine("Usage:");
    Console.WriteLine("Sdl.SDK.ProjectAutomation.Samples.BatchAnalyze.exe <source> <tm> [/c] [/i] [/k] [/s]");
    Console.WriteLine("<source> Path to a folder that contains the source files to process.");
    Console.WriteLine("<tm> Path to a TM file or server TM name");
    Console.WriteLine("/c   cross-file repetitions should be reported");
    Console.WriteLine("/i   internal fuzzy match leverage should be reported");
    Console.WriteLine("/k   keep project files");
    Console.WriteLine("/s   sub-folders should be processed");
    Console.WriteLine("/p   publish to a server");
    return;
}
```

In the next step, we need to verify whether the main path entered and a TM has been entered (at this point we do not know if the tm is a file or server tm)

```cs
if (!String.IsNullOrEmpty(args[0]) && !Directory.Exists(args[0])
    && !String.IsNullOrEmpty(args[1]) )
{
    Console.WriteLine("Please specify a valid input directory and a valid TM. Press ENTER to exit.");
    return;
}
```

After the validity check set the variables as shown below:

```cs
mainPath = args[0];
tmFile = args[1];

for (int i = 2; i < args.Length; ++i)
{
    switch (args[i])
    {
        case "/c":
            reportCrossFileRepetitions = true;
            break;
        case "/i":
            reportInternalFuzzyMatchLeverage = true;
            break;
        case "/k":
            keepProjectFiles = true;
            break;
        case "/s":
            processSubFolders = true;
            break;
        case "/p":
            publishToServer = true;
            break;
    }
}
```
If you do not wish to publish to a server if must be a file based TM so check it exists

```cs
if (!publishToServer && !File.Exists(args[1]))
{
    Console.WriteLine("Please specify a valid file TM. Press ENTER to exit.");
    return;
}
```

Start by creating the project and output a success message if the task has been executed successfully. Remember that after running the application the analysis result should be presented in the console.

```cs
try
{
    ProjectCreator process = new ProjectCreator();
    process.Create(
        mainPath, 
        tmFile, 
        processSubFolders,
        reportCrossFileRepetitions,
        reportInternalFuzzyMatchLeverage,
        keepProjectFiles, publishToServer);

    Console.WriteLine();
    Console.WriteLine("Analyze files report created successfully. Press ENTER to exit.");
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.ReadLine();
}
```

Putting it All Together
--

The full implementation should look as shown below:

```cs
public static void Main(string[] args)
{
    #region "DeclareVariables"
    string mainPath = string.Empty;
    string tmFile = string.Empty;
    bool processSubFolders = false;
    bool reportCrossFileRepetitions = false;
    bool reportInternalFuzzyMatchLeverage = false;
    bool keepProjectFiles = false;
    bool publishToServer = false;
    #endregion

    #region "Usage"
    if (args.Length < 2)
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("Sdl.SDK.ProjectAutomation.Samples.BatchAnalyze.exe <source> <tm> [/c] [/i] [/k] [/s]");
        Console.WriteLine("<source> Path to a folder that contains the source files to process.");
        Console.WriteLine("<tm> Path to a TM file or server TM name");
        Console.WriteLine("/c   cross-file repetitions should be reported");
        Console.WriteLine("/i   internal fuzzy match leverage should be reported");
        Console.WriteLine("/k   keep project files");
        Console.WriteLine("/s   sub-folders should be processed");
        Console.WriteLine("/p   publish to a server");
        return;
    }
    #endregion

    #region "CheckPath"
    if (!String.IsNullOrEmpty(args[0]) && !Directory.Exists(args[0])
        && !String.IsNullOrEmpty(args[1]) )
    {
        Console.WriteLine("Please specify a valid input directory and a valid TM. Press ENTER to exit.");
        return;
    }
    #endregion

    #region "SetVariables"
    mainPath = args[0];
    tmFile = args[1];

    for (int i = 2; i < args.Length; ++i)
    {
        switch (args[i])
        {
            case "/c":
                reportCrossFileRepetitions = true;
                break;
            case "/i":
                reportInternalFuzzyMatchLeverage = true;
                break;
            case "/k":
                keepProjectFiles = true;
                break;
            case "/s":
                processSubFolders = true;
                break;
            case "/p":
                publishToServer = true;
                break;
        }
    }

    #endregion

    #region "CheckTMPathIfFileTM"

    if (!publishToServer && !File.Exists(args[1]))
    {
        Console.WriteLine("Please specify a valid file TM. Press ENTER to exit.");
        return;
    }
    #endregion

    #region "ProcessTask"
    try
    {
        ProjectCreator process = new ProjectCreator();
        process.Create(
            mainPath, 
            tmFile, 
            processSubFolders,
            reportCrossFileRepetitions,
            reportInternalFuzzyMatchLeverage,
            keepProjectFiles, publishToServer);

        Console.WriteLine();
        Console.WriteLine("Analyze files report created successfully. Press ENTER to exit.");
        Console.ReadLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.ReadLine();
    }
    #endregion
}
```

See Also
--



[Configuring the Project Properties](configuring_the_project_properties.md)