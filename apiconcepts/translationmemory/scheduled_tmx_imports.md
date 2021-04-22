Scheduled TMX Imports
===
Like file-based TMs, you can import **.tmx* documents into server-based TMs (see [Importing a TMX File]()). However, in contrast to file TMs, you need to consider the following: If a large *.tmx* file needs to be imported over a WAN connection, there is a risk that the Internet connection breaks down, thus failing the import. It is therefore preferable to upload the import file to the server, and then schedule the import to take place once the file has been fully uploaded, so that the import can take place locally on the server, and not through an Internet connection. In this chapter you will learn how to upload a **.tmx* file for a scheduled import.

Add a New Class
----
Start by adding a class called `ServerImporter` to your project. Then implement a public function called `ImportTmx`, which takes a TM Server object, the organization and TM name, and the file name and path of the import file as parameters.

Open the TM
---
First, open the server TM into which the TMX content should be imported by creating a server TM object. You do this applying the [GetTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetTranslationMemory_System_Guid_Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryProperties_) method to the TM Server and provide the organization name and the TM name as string parameters. Note that selecting a server TM requires the full TM path name including the organization, e.g. */Organization Name/TM Name*.

# [C#](#tab/tabid-1)
```cs
ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(
    orgName + tmName, TranslationMemoryProperties.All);
```
***

Create an Importer Object and Set the Language Direction
---

Next, create an object for the scheduled TMX import. The language direction of the TMX import file must, of course, match the language direction of the target TM. If you try to import, for example, an English-French TMX file into a server TM that does not offer this language pair, the import cannot work. When creating the object for the scheduled import you need to provide the language direction object.

# [C#](#tab/tabid-2)
```cs
ScheduledTranslationMemoryImportOperation importer = new ScheduledTranslationMemoryImportOperation(
    this.GetLanguageDirection(tm, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE")));
```
****

In our example, we create the language direction object through a separate function called `GetLanguageDirection`, which takes the TM object as well as the source and target culture info (i.e. the locales) as parameters. To make your application robust you should check whether the TM actually supports the language direction for the import. To do this we loop through the language directions available in the TM. In the next step, we determine whether TM actually offers the required language direction. If this is not the case, an error will be thrown.

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
***

Specify the Import Settings
---

In the next step, specify the settings to apply to the import. There are various parameters that you can configure such as the [ChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_ChunkSize), which specifies the maximum amount of units that should be imported in one go. This can help to achieve a more steady, uninterrupted import process, as you limit the number of units that are read into the memory at a given time. The minimum value here is 25. Note that the default value is 50 ([DefaultTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_DefaultTranslationUnitChunkSize)), the maximum value is 200 ([MaxTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_MaxTranslationUnitChunkSize)). You can also specify whether the import should continue if an error has occurred. You can set the `ContinueOnError` property to True if you want to prevent one faulty unit to stop the entire import. If, for example, among 10,000 units an invalid unit is encountered, this particular unit will not be imported, but the remaining (valid) units will. The most important piece of information is, of course, the import file and path, which is provided as a `FileInfo` object.

# [C#](#tab/tabid-4)
```cs
importer.ChunkSize = 25;
importer.ContinueOnError = true;
importer.Source = new FileInfo(importFilePath);
this.GetImportSettings(importer.ImportSettings);
```
****
There are various other import settings that you may configure related to the way TUs should be treated during import. In our example we use a separate `GetImportSettings` helper function to configure these settings:

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
*****

Here, you can, for example, specify whether sub-languages should be checked or not. If you set, for example, `CheckMatchingSublanguages` to True, any English (UK) TUs in an import file will not be accepted by a TM that uses English (US). If sub-languages are not checked, then, in our example, UK TUs would be treated as if they had a US locale.
Through the settings, you may also specify whether any existing TUs should be overwritten. For example, if the TMX file contains a TU with the same source segment, but with a different translation, the TU in the TM can be overwritten by the import TU. You may also specify what should happen to any TM fields associated with a TU. Example: A TU in a TM has the field value *Customer: Microsoft*. The same TU exists in the TMX import file, but there the same field has the value *Apple*. Through the `ExistingFieldsUpdateMode` property you can set whether existing field values should be overwritten, left unchanged, or merged.

Upload and Queue for Import
---

Now apply the `Create` method to the importer object. This will create an import task on the server, without actually executing the import. It just acts as a placeholder for the import that should happen after the complete file has been uploaded to the server, which is done through the `Upload` method.

# [C#](#tab/tabid-6)
```cs
importer.Create();
importer.Upload(this.importer_Uploaded);
importer.Queue();
```
*****
The `Upload` method takes an event as parameter, through which you can determine the progress of the upload, i.e. how many bytes out of the total have been transferred.
# [C#](#tab/tabid-7)
```cs
private void importer_Uploaded(object sender, FileTransferEventArgs e)
{
    MessageBox.Show("Transferred - " + e.BytesTransferred.ToString() + " out of " + e.TotalBytes.ToString() + " bytes\r\n");
    e.Cancel = false;
}
```
****
Last, you apply the `Queue` method to schedule the actual import operation on the server.

Track the Operation Status
----
The import task can have a number of statuses that should cause the operation to stop such as aborted, canceled, error, or completed. There are also various statuses that should cause the import operation to be refreshed such as recovering, queued, allocated, etc. Through the following `while` loop you can determine based on the current scheduled operation status whether to refresh or to stop the import. For example, if the status is [Recovering](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), you apply the Refresh method to the importer object. Statuses such as [CancelledOn](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml) should not cause a refresh.

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
****
Check Whether the Import Completed Successfully
----

At the end, you should check whether the import completed successfully indeed. For this, too, you can use the [ScheduledOperationStatus](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml). If the value is [Completed](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), you can output a success message. If the value is [Error](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ScheduledOperationStatus.yml), you output an error message.
# [C#](#tab/tabid-9)
```cs
if (importer.Status == ScheduledOperationStatus.Completed)
{
    MessageBox.Show("Import successfuly finished.");
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
*****
Putting it All Together
----
The complete class should look as shown below:
# [C#](#tab/tabid-10)
```cs
namespace Sdl.SDK.LanguagePlatform.Samples.TmAutomation
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
            #region "OpenTm"
            ServerBasedTranslationMemory tm = tmServer.GetTranslationMemory(
                orgName + tmName, TranslationMemoryProperties.All);
            #endregion

            #region "importer"
            ScheduledTranslationMemoryImportOperation importer = new ScheduledTranslationMemoryImportOperation(
                this.GetLanguageDirection(tm, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE")));
            #endregion

            #region "params"
            importer.ChunkSize = 25;
            importer.ContinueOnError = true;
            importer.Source = new FileInfo(importFilePath);
            this.GetImportSettings(importer.ImportSettings);
            #endregion

            #region "upload"
            importer.Create();
            importer.Upload(this.importer_Uploaded);
            importer.Queue();
            #endregion

            #region "wait"
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
            #endregion

            #region "completed"
            if (importer.Status == ScheduledOperationStatus.Completed)
            {
                MessageBox.Show("Import successfuly finished.");
            }
            else if (importer.Status == ScheduledOperationStatus.Error)
            {
                MessageBox.Show(importer.ErrorMessage);
            }
            else
            {
                MessageBox.Show("Import didn't finish.");
            }
            #endregion
        }

        #region "settings"
        private void GetImportSettings(Sdl.LanguagePlatform.TranslationMemory.ImportSettings importSettings)
        {
            if (importSettings == null)
            {
                importSettings = new ImportSettings();
            }

            importSettings.CheckMatchingSublanguages = true;
            importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite;
        }
        #endregion

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

        #region "event"
        private void importer_Uploaded(object sender, FileTransferEventArgs e)
        {
            MessageBox.Show("Transferred - " + e.BytesTransferred.ToString() + " out of " + e.TotalBytes.ToString() + " bytes\r\n");
            e.Cancel = false;
        }
        #endregion
    }
}
```
****

> [!NOTE]
> 
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.

See Also
----
[Scheduled TMX Exports](scheduled_tmx_exports.md)

[Importing a TMX File]()

[Exporting to a TMX File]()