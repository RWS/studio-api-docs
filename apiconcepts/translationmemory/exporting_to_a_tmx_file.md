# Exporting to a TMX File

Sometimes you may want to export TM content to an external TMX file, for example, to create backups or merge content into another TM.

## Add a New Class

Add a new class named `TmExporter` to your project. Then implement a method named `ExportTMXFile()`. Call it as shown below:

# [C#](#tab/tabid-1)
```cs
var tmExporter = new TmExporter();
tmExporter.ExportTMXFile(_translationMemoryFilePath, _exportFilePath);
tmExporter.RunFilteredExport(_translationMemoryFilePath, _exportFilePath);
```
***

The method requires the TM path and the TMX export file path as parameters.

Start by opening the TM and creating an exporter object:

# [C#](#tab/tabid-2)
```cs
var tm = new FileBasedTranslationMemory(tmPath);

var exporter = new TranslationMemoryExporter(tm.LanguageDirection);
```
***

Note that the [TranslationMemoryExporter](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml) object requires the TM language direction. Like a TM, a TMX file is bilingual and uses a language direction that must match the TM direction. Pass the TM language direction when you create the exporter.

Next, you can set the [ChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_ChunkSize). The chunk size determines the maximum number of units read from the TM at one time. If the TM is on a local disk, a larger chunk size is usually fine. If the export runs over a network, keep the chunk size smaller to reduce latency. In this example, set the chunk size to 20, which roughly matches the size of the sample TM:

# [C#](#tab/tabid-3)
```cs
exporter.ChunkSize = 20;
```
***

The default chunk size is 50 ([DefaultTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_DefaultTranslationUnitChunkSize)), and the maximum is 200 ([MaxTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_MaxTranslationUnitChunkSize)).

Before you execute the export, subscribe to an event that fires after each batch, or chunk, is exported:

# [C#](#tab/tabid-4)
```cs
exporter.BatchExported += new EventHandler<BatchExportedEventArgs>(this.exporter_BatchExported);
```
***

Add the following member to your class. The `ExportTMXFile()` method triggers it:

# [C#](#tab/tabid-5)
```cs
private void exporter_BatchExported(object sender, BatchExportedEventArgs e)
{
    string info;
    info = "Total TUs processed: " + e.TotalProcessed + "\n";
    info += "Total TUs exported: " + e.TotalExported + "\n";

    MessageBox.Show(info, "Export statistics for batch");
    e.Cancel = false;
}
```
***

After each batch is processed, the code reports the total number of units processed and the number actually exported.

Finally, call the `Export()` method to perform the export. This method requires the TMX export file path. The second boolean parameter determines whether an existing export file is overwritten.

# [C#](#tab/tabid-6)
```cs
exporter.Export(exportFilePath, true);
```
***

In the example above, the overwrite parameter is set to `true`. If you set it to `false` and a file with the same name already exists, the export throws an error that your code must handle.

The complete `ExportTMXFile()` method looks like this:

# [C#](#tab/tabid-7)
```cs
public void ExportTMXFile(string tmPath, string exportFilePath)
{
    #region "open"
    var tm = new FileBasedTranslationMemory(tmPath);

    var exporter = new TranslationMemoryExporter(tm.LanguageDirection);
    #endregion

    #region "chunk"
    exporter.ChunkSize = 20;

    #endregion

    #region "FireEvent"
    exporter.BatchExported += new EventHandler<BatchExportedEventArgs>(this.exporter_BatchExported);
    #endregion

    #region "execute"
    exporter.Export(exportFilePath, true);
    #endregion
}
```
***

## Run a Filtered Export

If you do not want to export the entire TM, you can run a filtered export. This lets you export a subset of the TM content, such as all translation units with a *Customer* field value of *Microsoft* or all TUs created after 1 January 2010. The overall workflow is the same as for a full export, but you add a filter before you run the export. Start by implementing another method named `RunFilteredExport()`, which takes the TM path and the TMX export file path as string parameters. First, open the TM and create a TM exporter object:

# [C#](#tab/tabid-8)
```cs
var tm = new FileBasedTranslationMemory(tmPath);
var exporter = new TranslationMemoryExporter(tm.LanguageDirection);
```
***

The main difference from the full export is that you set the **FilterExpression** property on the exporter. A separate helper method builds the filter expression.

# [C#](#tab/tabid-9)
```cs
exporter.FilterExpression = this.GetFilterSimple();
```
***

Like before, subscribe to the batch event and call `Export()` to perform the export. The complete `RunFilteredExport()` method looks like this:

# [C#](#tab/tabid-10)
```cs
public void RunFilteredExport(string tmPath, string exportFilePath)
{
    #region "OpenForFilter"
    var tm = new FileBasedTranslationMemory(tmPath);
    var exporter = new TranslationMemoryExporter(tm.LanguageDirection);
    #endregion

    #region "FilterDefinition"
    exporter.FilterExpression = this.GetFilterSimple();
    #endregion

    #region "DoFilteredExport"
    exporter.BatchExported += new EventHandler<BatchExportedEventArgs>(this.exporter_BatchExported);
    exporter.Export(exportFilePath, true);
    #endregion
}
```
***

## Define the Filter for the Export (Simple)

This section shows how to implement the helper method that creates a simple export filter. Add a method named `GetFilterSimple()` that returns a **FilterExpression** object:

# [C#](#tab/tabid-11)
```cs
private FilterExpression GetFilterSimple()
```
***

Suppose you want the export to include only TUs where the *Customer* field equals *Microsoft*. In this example, *Customer* is a picklist field that allows multiple values. The following sample code shows how to define the field name and value and build the criterion:

# [C#](#tab/tabid-12)
```cs
var fieldName = new PicklistItem("Customer");
var fieldValue = new MultiplePicklistFieldValue("Microsoft");
fieldValue.Add(fieldName);
```
***

Next, use the **AtomicExpression** class to create the filter expression to return to the export method. Pass the field value and the operator. In this case, the filter uses **Equal**. Other possible operators include **Contains**, **ContainsNot**, **Greater**, and **Smaller**.

# [C#](#tab/tabid-13)
```cs
var filter = new AtomicExpression(fieldValue, AtomicExpression.Operator.Equal);
return filter;
```
***

## Define the Filter for the Export (Advanced)

Suppose you want a more advanced filter with two or more criteria, such as exporting all TUs where the *Customer* field equals *Microsoft* **or** the *Project id* text field contains *2010*. Add another helper method named `GetFilterAdvanced()`:

# [C#](#tab/tabid-14)
```cs
private FilterExpression GetFilterAdvanced()
```
***

The first step is almost identical to the previous example: define the first criterion and create the first filter expression with the **AtomicExpression** class.

# [C#](#tab/tabid-15)
```cs
var fieldName1 = new PicklistItem("Customer");
var fieldValue1 = new MultiplePicklistFieldValue("Microsoft");
fieldValue1.Add(fieldName1);
var expression1 = new AtomicExpression(fieldValue1, AtomicExpression.Operator.Equal);
```
***

Then define the second criterion, such as *Project id* containing *2010*, and use the **AtomicExpression** class to construct the second expression:

# [C#](#tab/tabid-16)
```cs
var fieldName2 = new MultipleStringFieldValue("Project id");
fieldName2.Add("2010");
var expression2 = new AtomicExpression(fieldName2, AtomicExpression.Operator.Contains);
```
***

The final filter object uses the **ComposedExpression** class, which combines `expression1` and `expression2` with an OR operator and returns the complete filter expression.

# [C#](#tab/tabid-17)
```cs
var filter = new ComposedExpression(expression1, ComposedExpression.Operator.Or, expression2);
return filter;
```
***

## Putting it All Together

The complete class should now look like this:

# [C#](#tab/tabid-18)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TmExporter
    {
        #region "export"
        public void ExportTMXFile(string tmPath, string exportFilePath)
        {
            #region "open"
            var tm = new FileBasedTranslationMemory(tmPath);

            var exporter = new TranslationMemoryExporter(tm.LanguageDirection);
            #endregion

            #region "chunk"
            exporter.ChunkSize = 20;

            #endregion

            #region "FireEvent"
            exporter.BatchExported += new EventHandler<BatchExportedEventArgs>(this.exporter_BatchExported);
            #endregion

            #region "execute"
            exporter.Export(exportFilePath, true);
            #endregion
        }
        #endregion

        #region "event"
        private void exporter_BatchExported(object sender, BatchExportedEventArgs e)
        {
            string info;
            info = "Total TUs processed: " + e.TotalProcessed + "\n";
            info += "Total TUs exported: " + e.TotalExported + "\n";

            MessageBox.Show(info, "Export statistics for batch");
            e.Cancel = false;
        }
        #endregion

        #region "RunFilteredExport"
        public void RunFilteredExport(string tmPath, string exportFilePath)
        {
            #region "OpenForFilter"
            var tm = new FileBasedTranslationMemory(tmPath);
            var exporter = new TranslationMemoryExporter(tm.LanguageDirection);
            #endregion

            #region "FilterDefinition"
            exporter.FilterExpression = this.GetFilterSimple();
            #endregion

            #region "DoFilteredExport"
            exporter.BatchExported += new EventHandler<BatchExportedEventArgs>(this.exporter_BatchExported);
            exporter.Export(exportFilePath, true);
            #endregion
        }
        #endregion

        #region "GetFilterSimple"
        private FilterExpression GetFilterSimple()
        {
            #region "SimpleCriterion"
            var fieldName = new PicklistItem("Customer");
            var fieldValue = new MultiplePicklistFieldValue("Microsoft");
            fieldValue.Add(fieldName);
            #endregion

            #region "SimpleFilter"
            var filter = new AtomicExpression(fieldValue, AtomicExpression.Operator.Equal);
            return filter;
            #endregion
        }
        #endregion

        #region "GetFilterAdvanced"
        private FilterExpression GetFilterAdvanced()
        {
            #region "AdvancedCriterion1"
            var fieldName1 = new PicklistItem("Customer");
            var fieldValue1 = new MultiplePicklistFieldValue("Microsoft");
            fieldValue1.Add(fieldName1);
            var expression1 = new AtomicExpression(fieldValue1, AtomicExpression.Operator.Equal);
            #endregion

            #region "AdvancedCriterion2"
            var fieldName2 = new MultipleStringFieldValue("Project id");
            fieldName2.Add("2010");
            var expression2 = new AtomicExpression(fieldName2, AtomicExpression.Operator.Contains);
            #endregion

            #region "AdvancedFilter"
            var filter = new ComposedExpression(expression1, ComposedExpression.Operator.Or, expression2);
            return filter;
            #endregion
        }
        #endregion
    }
}
```
***

## See Also
[Importing a TMX File](importing_a_tmx_file.md)

[Introduction to the Batch Export Tool](introduction_to_the_tm_batch_export_tool.md)