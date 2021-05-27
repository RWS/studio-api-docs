Putting it All Together
==
The following example shows the following steps
* Opening a server project and creating a new local copy.
* Overriding files checked out by other users.
* Checking out and downloading files to process.
* Adding a server-based TM.
* Running batch tasks.
* Uploading the results back to the server and checking them in.

```cs
void ServerPuttingItAllTogether()
{
    const string tmServerPrefix ="sdltm.";

    string localcopylocation = @"C:\Projects\";
    string serverName = "http://myServerAddress:80";
    Uri serverAddress = new Uri(serverName);
    string organizationPath = "/MyOrganizationPath";

    ProjectServer server = new ProjectServer(serverAddress, false, "MyUser", "MyPassword");
    ServerProjectInfo projectInfo = server.GetServerProject("/MyOrganization/MyProject");

    FileBasedProject project;

    if (projectInfo != null)
    {

        project = server.OpenProject(projectInfo.ProjectId, localcopylocation + projectInfo.Name);

        ProjectFile[] sourceFiles = project.GetSourceLanguageFiles();
        project.UndoCheckoutFiles(sourceFiles.GetIds());

        //Add a server based translation provider
        Uri tmAddress = new Uri(String.Format("{0}{1}{2}/{3}", tmServerPrefix, serverAddress, organizationPath, "UnitTestTm"));
        TranslationProviderConfiguration tmConfig = project.GetTranslationProviderConfiguration();
        tmConfig.Entries.Add
        (
            new TranslationProviderCascadeEntry
            (
                      new TranslationProviderReference(tmAddress),
                      true,
                      true,
                      true,
                      0
            )
        );

        //The credentials for a server based TM are keyed to the server address without path or tm name
        project.Credentials.AddCredential(serverAddress, "user=sa;password=sa;type=CustomUser");
        project.UpdateTranslationProviderConfiguration(tmConfig);

        List<TaskStatusEventArgs> taskStatusEventArgsList = new List<TaskStatusEventArgs>();
        List<MessageEventArgs> messageEventArgsList = new List<MessageEventArgs>();

        // Run tasks for source files to create target files
        AutomaticTask scanTask = project.RunAutomaticTask(
            sourceFiles.GetIds(),
            AutomaticTaskTemplateIds.Scan,
            (sender, taskStatusArgs) => taskStatusEventArgsList.Add(taskStatusArgs),
            (sender, messageArgs) => messageEventArgsList.Add(messageArgs));

        AutomaticTask convertTask = project.RunAutomaticTask(
            sourceFiles.GetIds(),
            AutomaticTaskTemplateIds.ConvertToTranslatableFormat,
            (sender, taskStatusArgs) => taskStatusEventArgsList.Add(taskStatusArgs),
            (sender, messageArgs) => messageEventArgsList.Add(messageArgs));

        AutomaticTask copyToTargetTask = project.RunAutomaticTask(
            sourceFiles.GetIds(),
            AutomaticTaskTemplateIds.CopyToTargetLanguages,
            (sender, taskStatusArgs) => taskStatusEventArgsList.Add(taskStatusArgs),
            (sender, messageArgs) => messageEventArgsList.Add(messageArgs));

        // Run tasks for target files
        ProjectFile[] targetFiles = project.GetTargetLanguageFiles();

        AutomaticTask perfectMatchTask = project.RunAutomaticTask(
           targetFiles.GetIds(),
           AutomaticTaskTemplateIds.PerfectMatch,
           (sender, taskStatusArgs) => taskStatusEventArgsList.Add(taskStatusArgs),
           (sender, messageArgs) => messageEventArgsList.Add(messageArgs));

        AutomaticTask analyseTask = project.RunAutomaticTask(
           targetFiles.GetIds(),
           AutomaticTaskTemplateIds.AnalyzeFiles,
           (sender, taskStatusArgs) => taskStatusEventArgsList.Add(taskStatusArgs),
           (sender, messageArgs) => messageEventArgsList.Add(messageArgs));

        AutomaticTask preTranslateTask = project.RunAutomaticTask(
          targetFiles.GetIds(),
          AutomaticTaskTemplateIds.PreTranslateFiles,
          (sender, taskStatusArgs) => taskStatusEventArgsList.Add(taskStatusArgs),
          (sender, messageArgs) => messageEventArgsList.Add(messageArgs));

        AutomaticTask populateProjTmTask = project.RunAutomaticTask(
           targetFiles.GetIds(),
           AutomaticTaskTemplateIds.PopulateProjectTranslationMemories,
           (sender, taskStatusArgs) => taskStatusEventArgsList.Add(taskStatusArgs),
           (sender, messageArgs) => messageEventArgsList.Add(messageArgs));

        //Check in the new target files
        targetFiles = project.GetTargetLanguageFiles();
        project.CheckinFiles(targetFiles.GetIds(), "Target Files Created and Pre-translated", null);

        //Save the project back to disk
        project.Save();
    }
}
```

See Also
--


[About Server Based Projects](about_server_based_projects.md)

[Connecting a Project to a Project Server](connecting_a_project_to_a_project_server.md)

[Viewing and Deleting Published Projects](viewing_and_deleting_published_projects.md)

[Checking Files In and Out](checking_files_in_and_out.md)

[Downloading and Uploading Files](downloading_and_uploading_files.md)