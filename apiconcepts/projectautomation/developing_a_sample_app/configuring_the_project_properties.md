Configuring the Project Properties
==

Before the files can be processed we need to create a project, to which we later add the files to analyze as well as the TM file that should be used for the analysis. For the project creation, we proceed on the following assumptions:

* The project language direction will correspond to the source / target language combination of the translation memory. Therefore, the project language direction will be implicitly determined by the TM.
* The project will be created in the <Var:ProductName> projects folder, e.g. C:\Documents and *Settings\UserName\My Documents\ <Var:ProductName> \Projects.*
* The project name will be hard-coded and based on the current date/time to ensure that the project folder names are unique.

Add the Class for the Project Creation
--

Add a new class to your project called ProjectCreator. Make sure that this class uses the namespaces listed in the chapter [Required References and Namespaces](required_references_and_namespaces.md). Moreover, you also need to use the following namespaces from the Translation Memory API:

* Sdl.LanguagePlatform.TranslationMemoryApi
* Sdl.LanguagePlatform.TranslationMemory
* Sdl.LanguagePlatform.Core.Tokenization
* Sdl.LanguagePlatform.Core

Create the Project
--

Start by implementing a public function called Create. This function takes the following parameters:

* The document folder name as (string)
* The full file name and path of the TM file (string)
* A boolean parameter to indicate whether sub-folders should be processed through recursion
* Two boolean parameters to enable/disable cross-file repetition and internal fuzzy match leverage reporting

```cs
/// <summary>
/// Creates the actual project that is used as a container for
/// the files to analyze. Triggers all subsequent helper function
/// in sequence, i.e. adding the source files, the TM, configuring
/// the task settings, and running the required tasks, 
/// if required publishing the result to a project server
/// </summary> 
public void Create(
    string docFolder,
    string tmFile,
    bool recursion,
    bool reportCrossFileRepetitions,
    bool reportInternalFuzzyMatchLeverage,
    bool keepProjectFiles, 
    bool publishToServer)
    
```

Within this function you first open the TM file. This is the only point at which we make use of the Translation Memory API, as we need to retrieve the source/target language from the TM for the project creation as shown below:

```cs
FileBasedTranslationMemory fileTm = new FileBasedTranslationMemory(tmFile);
string srcLocale = fileTm.LanguageDirection.SourceLanguage.ToString();
string trgLocale = fileTm.LanguageDirection.TargetLanguage.ToString();
```

In the next step use the [FileBasedProject](/api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) class to create an object that represents our new project. When creating the project object we need to provide the project properties. Project properties are, for example, the project name, location, the source / target language, etc. For convenience reasons we 'outsource' the project info configuration to a separate helper function.

```cs
FileBasedProject newProject = new FileBasedProject(this.GetProjectInfo(srcLocale, trgLocale));
```

Configure the Project Properties
--

Implement a separate helper function for configuring the project properties called ```GetProjectInfo```. As this function is responsible for setting the project languages, it takes the source and the target locales (e.g.* en-US* and *de-DE*) as string parameters:

```cs
/// <summary>
/// Configures the project information, i.e. the project location (folder), the project name,
/// and the source/target language.
/// </summary> 
private ProjectInfo GetProjectInfo(string srcLocale, string trgLocale)
```

Start by creating a [ProjectInfo](../../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml) object, which holds the properties that we are going to set subsequently:

```cs
ProjectInfo info = new ProjectInfo();
```

To keep things as simple as possible, we are going to hard-code the name of the project as *BatchAnalyzer*. Note that in our implementation, the project files will be stored in a folder that is named after the project. Note that the API will throw an error when you try to create a project with the same name in the same location again, as a folder with that name already exists. As we can assume that this application will be used quite often for running multiple batch analysis processes, we need to make sure that the project (folder) names stay unique. We achieve this by appending a string that is based on the current date/time:

```cs
string nameExt = DateTime.Now.ToString();
nameExt = nameExt.Replace('.', '_');
nameExt = nameExt.Replace(':', '_');
nameExt = nameExt.Replace('/', '_');
nameExt = nameExt.Replace(' ', 'T');
info.Name = "BatchAnalyzer_" + nameExt;
```

In the next step we set the project folder, which stores all the files and sub-folders associated with our project. Here, we select the Projects folder of <Var:ProductName>, e.g.:* C:\Documents and Settings\UserName\My Documents\ <Var:ProductName> \Projects*. To this main folder, we append the name of our project, so that a corresponding project sub-folder is created. Then we set the [LocalProjectFolder](../../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml#Sdl_ProjectAutomation_Core_ProjectInfo_LocalProjectFolder) property to the specified folder string as shown below:

```cs
string localProjectFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() +
    Path.DirectorySeparatorChar + @"Studio 2011\Projects\" + info.Name;
info.LocalProjectFolder = localProjectFolder;
```

Next, we set the project source language. To do this we first create a ```Language``` object based on the given source locale string.

```cs
Language srcLang = new Language(CultureInfo.GetCultureInfo(srcLocale));
info.SourceLanguage = srcLang;
```

Then we determine the project target language. Note that a project can have several target languages. This is why the [TargetLanguages](../../../api/projectautomation/Sdl.ProjectAutomation.Core.ProjectInfo.yml#Sdl_ProjectAutomation_Core_ProjectInfo_TargetLanguages) needs to be set to a language array. However, since our implementation only supports one target language, the array will contain a single language object:

```cs
Language[] trgLang = new Language[] { new Language(CultureInfo.GetCultureInfo(trgLocale)) };
info.TargetLanguages = trgLang;
```

Finally, the function needs to return the project info object:

```cs
return info;
```

Note that other properties such as setting the due date are not required for our implementation.

The complete function for configuring the project information should look as shown below:

```cs
/// <summary>
/// Configures the project information, i.e. the project location (folder), the project name,
/// and the source/target language.
/// </summary> 
private ProjectInfo GetProjectInfo(string srcLocale, string trgLocale)
```

See Also
--

**Other Resources**


[Setting the Project Information](..\code_examples\setting_the_project_information.md)

[Creating Projects Based on Previous Projects](..\code_examples\creating_proj_based_on_templates.md)

[Creating Projects Based on Templates](..\code_examples\creating_proj_based_on_templates.md)

[Project Configuration](../project_configuration.md)