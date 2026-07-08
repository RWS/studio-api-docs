# Scheduled TMX Imports

Use a scheduled TMX import when you import **.tmx** content into a server TM. A WAN connection can interrupt a direct import, so `Var:ProductName` uploads the file to the server and schedules the import to run locally after upload completes.

## Create the importer class

Add a `ServerImporter` class to your project and implement a public `ImportTmx` method. The method should accept the TM server object, the organization name, the TM name, and the import file path.

## Open the TM

Open the server TM that will receive the import. Call [GetTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetTranslationMemory_System_Guid_Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryProperties_) on the TM server object and pass the organization name and TM name. The server TM path must include the organization, for example `/Organization Name/TM Name`.

# [C#](#tab/tabid-1)
```cs
ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(
    orgName + tmName, TranslationMemoryProperties.All);
```

## Create the importer object and set the language direction

Next, create the scheduled import object. The TMX file must use the same language direction as the target TM. For example, an English-French TMX file cannot import into a TM that does not support that language pair.

# [C#](#tab/tabid-2)
```cs
var importer = new ScheduledServerTranslationMemoryImport(
    this.GetLanguageDirection(tm, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE")));
```

The sample uses a helper method named `GetLanguageDirection`. It takes the TM object and the source and target cultures. The helper checks whether the TM supports the requested direction before the import starts.

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

## Configure the import settings

Set import properties such as [ChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_ChunkSize), which controls how many units the importer processes at a time. A smaller chunk size can help on slower connections. The minimum value is 25. The default is 50 ([DefaultTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_DefaultTranslationUnitChunkSize)), and the maximum is 200 ([MaxTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_MaxTranslationUnitChunkSize)). Set `ContinueOnError` to `true` if you want the import to continue after it encounters invalid units. The import file path is provided as a `FileInfo` object.

# [C#](#tab/tabid-4)
```cs
importer.ChunkSize = 25;
importer.ContinueOnError = true;
importer.Source = new FileInfo(importFilePath);
this.GetImportSettings(importer.ImportSettings);
```

The sample also uses a `GetImportSettings` helper method to configure how the import handles translation units and fields.

# [C#](#tab/tabid-5)
```cs
private void GetImportSettings(Sdl.LanguagePlatform.TranslationMemory.ImportSettings importSettings)
{
    if (importSettings == null)
    {
        importSettings = new ImportSettings();
    }

    importSettings.CheckMatchingSublanguages = true;
    importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite;
}
```

You can use these settings to control sublanguage handling, TU overwrite behavior, and field updates. For example, `CheckMatchingSublanguages` rejects English (UK) units when the TM uses English (US). `ExistingTUsUpdateMode` controls whether existing units are overwritten, and `ExistingFieldsUpdateMode` controls whether matching field values are overwritten, preserved, or merged.

## Queue the import

Call `Queue` on the importer object to create the server task and schedule the import.

# [C#](#tab/tabid-6)
```cs
importer.Queue();
```

## Wait for the import to finish

Use [ScheduledOperationStatus](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) to track the import. Stop when the operation reaches a terminal state such as [Completed](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), [Error](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), [Aborted](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), or [Cancelled](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml). Refresh the importer while the operation remains [Queued](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) or [Recovering](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml).

# [C#](#tab/tabid-8)
```cs
bool continueWaiting = true;
while (continueWaiting)
{
    switch (importer.Status)
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
            importer.Refresh();
            break;
        default:
            continueWaiting = false;
            break;
    }
}
```

## Handle the result

After the import finishes, check [ScheduledOperationStatus](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) again. If the status is [Completed](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), show a success message. If the status is [Error](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), show the error message.
# [C#](#tab/tabid-9)
```cs
if (importer.Status == ScheduledOperationStatus.Completed)
{
    MessageBox.Show("Import successfully finished.");
}
else if (importer.Status == ScheduledOperationStatus.Error)
{
    MessageBox.Show(importer.ErrorMessage);
}
else
{
    MessageBox.Show("Import didn't finish.");
}
```

## Complete the sample

The full class looks like this:
# [C#](#tab/tabid-10)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerImporter
    {
        public void ImportTmx(TranslationProviderServer tmServer, string orgName, string tmName, string importFilePath)
        {
            if (!orgName.StartsWith("/")) orgName = "/" + orgName;
            if (!orgName.EndsWith("/")) orgName += "/";

            ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(
                orgName + tmName, TranslationMemoryProperties.All);

            var importer = new ScheduledServerTranslationMemoryImport(
                this.GetLanguageDirection(tm, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE")));

            importer.ChunkSize = 25;
            importer.ContinueOnError = true;
            importer.Source = new FileInfo(importFilePath);
            this.GetImportSettings(importer.ImportSettings);

            importer.Queue();

            bool continueWaiting = true;
            while (continueWaiting)
            {
                switch (importer.Status)
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
                        importer.Refresh(); 
                        break;
                    default:
                        continueWaiting = false;
                        break;
                }
            }

            if (importer.Status == ScheduledOperationStatus.Completed)
            {
                MessageBox.Show("Import successfully finished.");
            }
            else if (importer.Status == ScheduledOperationStatus.Error)
            {
                MessageBox.Show(importer.ErrorMessage);
            }
            else
            {
                MessageBox.Show("Import did not finish.");
            }
        }

        private void GetImportSettings(Sdl.LanguagePlatform.TranslationMemory.ImportSettings importSettings)
        {
            if (importSettings == null)
            {
                importSettings = new ImportSettings();
            }

            importSettings.CheckMatchingSublanguages = true;
            importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite;
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

[Scheduled TMX Exports](scheduled_tmx_exports.md)

[Importing a TMX File](importing_a_tmx_file.md)

[Exporting to a TMX File](exporting_to_a_tmx_file.md)
