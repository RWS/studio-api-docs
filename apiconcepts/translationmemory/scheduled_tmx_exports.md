# Scheduled TMX Exports

Use a scheduled TMX export when you export content from a server TM. Network latency can interrupt a direct export over WAN, so `Var:ProductName` runs the export as a scheduled task. After the task finishes, you download the **.tmx** file.

## Create the exporter class

Add a `ServerExporter` class to your project and implement a public `ExportToTmx` method. The method should accept the TM server object and the TM name.

## Open the TM and create the exporter

Open the TM that you want to export. To do that, call [GetTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetTranslationMemory_System_Guid_Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryProperties_) on the TM server object. Pass the full TM path, including the organization name. For example, use `/Organization Name/TM Name`.

# [C#](#tab/tabid-1)
```cs
if (!orgName.StartsWith("/")) orgName = "/" + orgName;
if (!orgName.EndsWith("/")) orgName += "/";
ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(
    orgName + tmName, TranslationMemoryProperties.All);
```

Next, create the exporter object:
# [C#](#tab/tabid-2)
```cs
var exporter = new ScheduledServerTranslationMemoryExport(
    this.GetLanguageDirection(tm, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE")));
```

The TMX export file contains one language direction, for example `en-US -> de-DE`. Use the following helper function to verify that the selected TM supports that direction:
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

## Configure the export settings

Set export properties such as [ChunkSize](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryExporter_ChunkSize), which controls how many TUs the exporter processes at a time. A smaller chunk size can help on slower connections. The default chunk size is 50 ([DefaultTranslationUnitChunkSize](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryExporter_DefaultTranslationUnitChunkSize)), and the maximum is 200 ([MaxTranslationUnitChunkSize](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryExporter_MaxTranslationUnitChunkSize)). You can also set `ContinueOnError`. When you set it to `true`, the export continues after it encounters invalid TUs.
# [C#](#tab/tabid-4)
```cs
exporter.ChunkSize = 25;
exporter.ContinueOnError = true;
```

## Wait for the export to finish

Use [ScheduledOperationStatus](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) to track the export. Stop when the operation reaches a terminal state such as [Completed](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), [Error](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), [Aborted](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), or [Cancelled](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml). Refresh the exporter while the operation remains [Queued](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) or [Recovering](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml).
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

## Handle the result

After the export finishes, check [ScheduledOperationStatus](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) again. If the status is [Completed](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), download the file and show a success message. If the status is [Error](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), show the error message.
# [C#](#tab/tabid-6)
```cs
if (exporter.Status == ScheduledOperationStatus.Completed)
{
    using (Stream outputStream = new FileStream(exportFilePath, FileMode.Create))
    {
        exporter.DownloadExport(outputStream);
    }
    MessageBox.Show("Export successfully finished.");
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

## Complete the sample

The full class looks like this:
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
            if (!orgName.StartsWith("/")) orgName = "/" + orgName;
            if (!orgName.EndsWith("/")) orgName += "/";

            ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(
                orgName + tmName, TranslationMemoryProperties.All);

            var exporter = new ScheduledServerTranslationMemoryExport(
                this.GetLanguageDirection(tm, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE")));

            exporter.ChunkSize = 25;
            exporter.ContinueOnError = true;

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

            if (exporter.Status == ScheduledOperationStatus.Completed)
            {
                using (Stream outputStream = new FileStream(exportFilePath, FileMode.Create))
                {
                    exporter.DownloadExport(outputStream);
                }

                MessageBox.Show("Export successfully finished.");
            }
            else if (exporter.Status == ScheduledOperationStatus.Error)
            {
                MessageBox.Show(exporter.ErrorMessage);
            }
            else
            {
                MessageBox.Show("Export did not finish.");
            }
        }

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
    }
}
```

## See also

[Scheduled TMX Imports](scheduled_tmx_imports.md)

[Exporting to a TMX File](exporting_to_a_tmx_file.md)

[Importing a TMX File](importing_a_tmx_file.md)
