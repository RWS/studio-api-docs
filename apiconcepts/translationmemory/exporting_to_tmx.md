Exporting to TMX
-----
In this step you will learn how to implement the functionality for exporting the content of file TMs (*.sdltm files) to *.tmx.

Add a class called `TmExporter` to your project. Then implement a function called Export, which takes the name and path of the TM file as a string parameter. Within this function you first open the file TM using the `tmPath` parameter. Then create an exporter object based on the TM and its language direction. When applying the `Export` method to the exporter object, you specify the export file name and path. Let us assume that the TMX file should be stored in the same path as the file TM. Also, the name should be the same as the file TM plus the language direction and the extension TMX, e.g. *sample.sdltm_en-US_de-DE.tmx*. By including the language direction in the export file name, we can determine into which master TM the *.tmx file should be imported later. That way we can avoid futile import operations (e.g. importing a French-English TMX file into a German-Spanish master TM).

# [C#](#tab/tabid-1)
```cs
/// <summary>
/// This function performs the actual export operation.
/// *.sdltm files are exported to *.tmx format.
/// </summary>
/// <param name="translationMemoryPath">FileBasedTranslationMemory to export into.</param>
public void Export(string translationMemoryPath)
{
    this.exportProgress = 0;
    FileBasedTranslationMemory tm = new FileBasedTranslationMemory(translationMemoryPath);
    TranslationMemoryExporter exporter = new TranslationMemoryExporter(tm.LanguageDirection);

    // set event handler
    exporter.BatchExported += new EventHandler<BatchExportedEventArgs>(this.Exporter_BatchExported);

    // The *.tmx export files have the same name as the original
    // *.sdltm files, including the language direction retrieved
    // from the file TM, e.g. "en-US_de-DE".
    string exportString = string.Format(
        CultureInfo.CurrentCulture,
        "{0}_{1}_{2}.tmx",
        translationMemoryPath,
        tm.LanguageDirection.SourceLanguage,
        tm.LanguageDirection.TargetLanguage);
    exporter.Export(exportString, true);

    Console.Write("Translation Units exported: {0} \n", this.exportProgress);
}

/// <summary>
/// Updates the console with current export progress
/// </summary>
/// <param name="sender">Event sender.</param>
/// <param name="e">Object containing current progress value and related info.</param>
private void Exporter_BatchExported(object sender, BatchExportedEventArgs e)
{
    this.exportProgress = e.TotalExported;
}

```
****

Putting it All Together
----
The complete class should look as shown below:
# [C#](#tab/tabid-2)
```cs
namespace SDK.LanguagePlatform.Samples.BatchExport
{
    using System;
    using System.Globalization;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    /// <summary>
    /// Represents functionality for exporting *.tmx files.
    /// </summary>
    public class TMExporter
    {
        /// <summary>
        /// Represents current export progress in persents.
        /// </summary>
        private int exportProgress = 0;

        #region "export"

        /// <summary>
        /// This function performs the actual export operation.
        /// *.sdltm files are exported to *.tmx format.
        /// </summary>
        /// <param name="translationMemoryPath">FileBasedTranslationMemory to export into.</param>
        public void Export(string translationMemoryPath)
        {
            this.exportProgress = 0;
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(translationMemoryPath);
            TranslationMemoryExporter exporter = new TranslationMemoryExporter(tm.LanguageDirection);

            // set event handler
            exporter.BatchExported += new EventHandler<BatchExportedEventArgs>(this.Exporter_BatchExported);

            // The *.tmx export files have the same name as the original
            // *.sdltm files, including the language direction retrieved
            // from the file TM, e.g. "en-US_de-DE".
            string exportString = string.Format(
                CultureInfo.CurrentCulture,
                "{0}_{1}_{2}.tmx",
                translationMemoryPath,
                tm.LanguageDirection.SourceLanguage,
                tm.LanguageDirection.TargetLanguage);
            exporter.Export(exportString, true);

            Console.Write("Translation Units exported: {0} \n", this.exportProgress);
        }

        /// <summary>
        /// Updates the console with current export progress
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Object containing current progress value and related info.</param>
        private void Exporter_BatchExported(object sender, BatchExportedEventArgs e)
        {
            this.exportProgress = e.TotalExported;
        }

        #endregion
    }
}
```
****

See Also
----------
[Exporting to a TMX File](exporting_to_a_tmx_file.md)