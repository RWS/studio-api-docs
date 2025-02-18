Scheduled TMX Exports
===

You can also export the content of a server TM to a **.tmx* file. Here, the same considerations apply as for the *.tmx* import, i.e. the export operation, too, could be disrupted by latencies in the WAN connection. This is why exports are run as scheduled tasks. When running an export from a file-based TM, you select the location where the **.tmx* document should be created on your local hard disk. When running an export from a server TM you do not select a file path. Instead, the export is run as a scheduled task, and the **.tmx* file can then be downloaded once the export has finished.

Add a New Class
---
Start by adding a new class to your project called `ServerExporter`. Within this class, you implement a public function called `ExportToTmx`, which takes the TM Server object and the TM name as parameters.

Open the TM and Create an Exporter Object
---

First, open the TM from which the export should take place. Programmatically, this is tantamount to creating a TM object by applying the [GetTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetTranslationMemory_System_Guid_Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryProperties_) method to the TM Server object. This requires the full path to the TM including the organization name, e.g. /Organization Name/TM Name.

# [C#](#tab/tabid-1)
```cs
if (!orgName.StartsWith("/")) orgName = "/" + orgName;
if (!orgName.EndsWith("/")) orgName += "/";
ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(
    orgName + tmName, TranslationMemoryProperties.All);
```
*****
Next, create an exporter object:
# [C#](#tab/tabid-2)
```cs
var exporter = new ScheduledServerTranslationMemoryExport(
    this.GetLanguageDirection(tm, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE")));
```
****
The TMX export file will contain two languages (e.g. en-US -> de-DE). Use the following separate function to verify whether the selected TM supports this language direction and to set the languages for the export:
# [C#](#tab/tabid-3)
```cs
private ServerBasedTranslationMemoryLanguageDirection GetLanguageDirection(ServerBasedTranslationMemory tm, CultureInfo source, CultureInfo target)
{
    foreach (ServerBasedTranslationMemoryLanguageDirection item in tm.LanguageDirections)
    {
        if (item.SourceLanguage == source && item.TargetLanguage == target)
        {
            return item;
        }
    }

    throw new Exception("Requested direction doesn't exist.");
}
```
****


Configure the Export Settings
---------------
In the next step, you may configure the export settings such as the [ChunkSize](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryExporter_ChunkSize), which determines the number of TUs that are processed at a given time. Especially when using slower Internet connections, it may be better to choose a smaller chunk size. Note that the default chunk size is 50 ([DefaultTranslationUnitChunkSize](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryExporter_DefaultTranslationUnitChunkSize)), the maximum chunk size is 200 ([MaxTranslationUnitChunkSize](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryExporter_MaxTranslationUnitChunkSize)). Another (boolean) parameter that you may set is `ContinueOnError`. True means that when errors are encountered during the export (e.g. due to an invalid TU), the process should still continue by exporting the remaining (valid) TUs.
# [C#](#tab/tabid-4)
```cs
exporter.ChunkSize = 25;
exporter.ContinueOnError = true;
```
****

Continue or Wait
---
In the next step you determine whether the export operation should be refreshed or not Through [ScheduledOperationStatus](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) you can determine the current status of the export operation. When the operation is, for example, [Aborted](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) or [Cancelled](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), the process should not continue. However, when the status of the operation is, for example, [Queued](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) or [Recovering](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), the process should be refreshed ([Refresh]()).
# [C#](#tab/tabid-5)
```cs
exporter.Queue();
exporter.Refresh();

bool continueWaiting = true;
while (continueWaiting)
{
    switch (exporter.Status)
    {
        case ScheduledOperationStatus.Abort:
        case ScheduledOperationStatus.Aborted:
        case ScheduledOperationStatus.Cancel:
        case ScheduledOperationStatus.Cancelled:
        case ScheduledOperationStatus.Completed:
        case ScheduledOperationStatus.Error:
            continueWaiting = false;
            break;
        case ScheduledOperationStatus.Aborting:
        case ScheduledOperationStatus.Allocated:
        case ScheduledOperationStatus.Cancelling:
        case ScheduledOperationStatus.NotSet:
        case ScheduledOperationStatus.Queued:
        case ScheduledOperationStatus.Recovered:
        case ScheduledOperationStatus.Recovering:
        case ScheduledOperationStatus.Recovery:
            continueWaiting = true;
            exporter.Refresh();
            break;
        default:
            continueWaiting = false;
            break;
    }
}
```
****

Complete the Process
---
At the end you can use the [ScheduledOperationStatus](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) class to ascertain whether the export operation has finished, or not. When the status of the operation is [Completed](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), the user should be notified on the successful completion of the process. If the status is [Error](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), the user should also be notified accordingly by throwing an exception message.
# [C#](#tab/tabid-6)
```cs
if (exporter.Status == ScheduledOperationStatus.Completed)
{
    using (Stream outputStream  = new FileStream(exportFilePath, FileMode.Create))
    {
        exporter.DownloadExport(outputStream);
    }
    MessageBox.Show("Export successfuly finished.");
}
else if (exporter.Status == ScheduledOperationStatus.Error)
{
    MessageBox.Show(exporter.ErrorMessage);
}
else
{
    MessageBox.Show("Export did not finish.");
}
```
***

Putting it All Together
---
The complete class should now look as shown below:
# [C#](#tab/tabid-7)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.IO;
    using System.Globalization;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerExporter
    {
        public void ExportToTmx(TranslationProviderServer tmServer, string orgName, string tmName, string exportFilePath)
        {
            #region "OpenTm"
            if (!orgName.StartsWith("/")) orgName = "/" + orgName;
            if (!orgName.EndsWith("/")) orgName += "/";
            ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(
                orgName + tmName, TranslationMemoryProperties.All);
            #endregion

            #region "exporter"
            var exporter = new ScheduledServerTranslationMemoryExport(
                this.GetLanguageDirection(tm, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE")));
            #endregion

            #region "settings"
            exporter.ChunkSize = 25;
            exporter.ContinueOnError = true;
            #endregion

            #region "wait"
            exporter.Queue();
            exporter.Refresh();

            bool continueWaiting = true;
            while (continueWaiting)
            {
                switch (exporter.Status)
                {
                    case ScheduledOperationStatus.Abort:
                    case ScheduledOperationStatus.Aborted:
                    case ScheduledOperationStatus.Cancel:
                    case ScheduledOperationStatus.Cancelled:
                    case ScheduledOperationStatus.Completed:
                    case ScheduledOperationStatus.Error:
                        continueWaiting = false;
                        break;
                    case ScheduledOperationStatus.Aborting:
                    case ScheduledOperationStatus.Allocated:
                    case ScheduledOperationStatus.Cancelling:
                    case ScheduledOperationStatus.NotSet:
                    case ScheduledOperationStatus.Queued:
                    case ScheduledOperationStatus.Recovered:
                    case ScheduledOperationStatus.Recovering:
                    case ScheduledOperationStatus.Recovery:
                        continueWaiting = true;
                        exporter.Refresh();
                        break;
                    default:
                        continueWaiting = false;
                        break;
                }
            }
            #endregion

            #region "completed"
            if (exporter.Status == ScheduledOperationStatus.Completed)
            {
                using (Stream outputStream  = new FileStream(exportFilePath, FileMode.Create))
                {
                    exporter.DownloadExport(outputStream);
                }
                MessageBox.Show("Export successfuly finished.");
            }
            else if (exporter.Status == ScheduledOperationStatus.Error)
            {
                MessageBox.Show(exporter.ErrorMessage);
            }
            else
            {
                MessageBox.Show("Export did not finish.");
            }
            #endregion
        }

        #region "LanguageDirection"
        private ServerBasedTranslationMemoryLanguageDirection GetLanguageDirection(ServerBasedTranslationMemory tm, CultureInfo source, CultureInfo target)
        {
            foreach (ServerBasedTranslationMemoryLanguageDirection item in tm.LanguageDirections)
            {
                if (item.SourceLanguage == source && item.TargetLanguage == target)
                {
                    return item;
                }
            }

            throw new Exception("Requested direction doesn't exist.");
        }
        #endregion
    }
}
```

See Also
----
[Scheduled TMX Imports](scheduled_tmx_imports.md)

[Exporting to a TMX File](exporting_to_a_tmx_file.md)

[Importing a TMX File](importing_a_tmx_file.md)
