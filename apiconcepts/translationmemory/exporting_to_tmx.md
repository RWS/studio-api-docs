# Exporting to TMX

In this step, you implement the functionality that exports the content of file-based TMs (`.sdltm` files) to `.tmx` files.

Add a class named `TmExporter` to your project. Then implement a method named `Export` that takes the path to the TM as a string parameter. In the method, open the file-based TM by using the `translationMemoryPath` parameter, create an exporter for the TM and its language direction, and call `Export` with the target file path.

Store the `.tmx` file in the same folder as the source TM. Use the same base name as the TM, plus the language direction and the `.tmx` extension, for example `sample.sdltm_en-US_de-DE.tmx`. Including the language direction in the file name makes it easier to identify the master TM that should import the file later and helps avoid invalid import operations, such as importing a French-English TMX file into a German-Spanish master TM.

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
    var tm = new FileBasedTranslationMemory(translationMemoryPath);
    var exporter = new TranslationMemoryExporter(tm.LanguageDirection);

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

# Putting it All Together

The complete class should look like this:
# [C#](#tab/tabid-2)
```cs
using System;
using System.Globalization;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace SDK.LanguagePlatform.Samples.BatchExport
{
    /// <summary>
    /// Represents functionality for exporting *.tmx files.
    /// </summary>
    public class TmExporter
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
            var tm = new FileBasedTranslationMemory(translationMemoryPath);
            var exporter = new TranslationMemoryExporter(tm.LanguageDirection);

            // Set event handler.
            exporter.BatchExported += new EventHandler<BatchExportedEventArgs>(this.Exporter_BatchExported);

            // The .tmx export file uses the same base name as the source
            // .sdltm file, including the language direction, for example
            // "en-US_de-DE".
            string exportString = string.Format(
                CultureInfo.CurrentCulture,
                "{0}_{1}_{2}.tmx",
                translationMemoryPath,
                tm.LanguageDirection.SourceLanguage,
                tm.LanguageDirection.TargetLanguage);
            exporter.Export(exportString, true);

            Console.Write("Translation units exported: {0}\n", this.exportProgress);
        }

        /// <summary>
        /// Updates the console with the current export progress.
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

## See Also

- [Exporting to a TMX File](exporting_to_a_tmx_file.md)
