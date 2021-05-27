Exporting to a TMX File
==

Sometimes you may want to export the content of a TM to an external TMX file, for example, for generating backups, importing the TMX file into another TM, thereby merging different TMs, etc.

Add a New Class
--

Add a new class called ```TmExporter``` to your project. Then, implement a function called ```ExportTMXFile```. This function can be called as shown below:

```cs
TMExporter objTmExport = new TMExporter();
objTmExport.ExportTMXFile(_translationMemoryFilePath, _exportFilePath);
objTmExport.RunFilteredExport(_translationMemoryFilePath, _exportFilePath);
```

The function requires the file name and path of the TM and the TMX export file as parameters.

Start by opening the TM and by creating an exporter object:

```cs
FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);

TranslationMemoryExporter exporter = new TranslationMemoryExporter(tm.LanguageDirection);
```

Note that the [TranslationMemoryExporter](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml) object requires the TM language direction as parameter. Like a TM a TMX file, too, is bilingual with a dedicated language direction, which will match the TM language direction. That is why you need to provide the TM language direction when creating a new [TranslationMemoryExporter](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryExporter.yml) object.

In the next step, you may specify the [ChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_ChunkSize). With the chunk size you determine the maximum amount of units that should be read from the TM. As long as the TM file resides on a local disk, the chunk size can be large, as the whole TM could be read in one go. If the export is going over e.g. an Internet line, then the chunk size should be small enough for the packages to be sent over the WAN. In our example, the chunk size is set to 20, which corresponds roughly to the size of our small sample TM:

```cs
exporter.ChunkSize = 20;
```

Note that the default chunk size is 50 ([DefaultTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_DefaultTranslationUnitChunkSize)), the maximum chunk size is 200 ([MaxTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_MaxTranslationUnitChunkSize)).

Before executing the export we fire an event after each batch (as specified by the chunk size) has been performed:

```cs
exporter.BatchExported += new EventHandler<BatchExportedEventArgs>(this.exporter_BatchExported);
```

Add the following member to your class, which is triggered from the ```ExportTMXFile``` function:

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

After each batch has been processed, the total number of units processed and the total number of units that has actually been exported is determined and output.

Finally, you apply the ```Export``` method to carry out the actual export operation. This method requires the full name and path of the TMX import file. With the second boolean parameter you can determine whether any existing export file should be overwritten or not.

```cs
exporter.Export(exportFilePath, true);
```

In the above code example the overwrite parameter is set to True. Note that when you set this parameter to False, and an export file with the same name already exists, an error will be thrown, which has to be caught by your implementation.

The full ```ExportTMXFile``` function looks as shown below:

```cs
public void ExportTMXFile(string tmPath, string exportFilePath)
{
    #region "open"
    FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);

    TranslationMemoryExporter exporter = new TranslationMemoryExporter(tm.LanguageDirection);
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


Run a Filtered Export
--

If you do not want to export the entire TM database content into a TMX file, but only a small part of it, you can run a filtered export. This allows you to export only a subset of the TM content, e.g. all translation units that have a *Customer* field with the value *Microsoft*, or all TUs that were created after 1st January 2010, etc. Running such an export is basically the same as the procedure described above. You just have to add a filter before the export is executed. Start by implementing another function called ```RunFilteredExport```, which also takes the TM file name and path as well as the export file name and path as string parameters. First, open the TM and create a TM exporter object as shown below:

```cs
FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);
TranslationMemoryExporter exporter = new TranslationMemoryExporter(tm.LanguageDirection);
```

The main difference to the full export from the example above is that you set the **FilterExpression** property of the TM exporter object. The actual filter expression is generated by a separate helper function, which we will implement in a later step.


```cs
exporter.FilterExpression = this.GetFilterSimple();
```

Like before, we fire the event that outputs the number of exported units in a batch and apply the Export method to carry out the actual export. The complete ```RunFilteredExport``` function looks as shown below:

```cs
public void RunFilteredExport(string tmPath, string exportFilePath)
{
    #region "OpenForFilter"
    FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);
    TranslationMemoryExporter exporter = new TranslationMemoryExporter(tm.LanguageDirection);
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

Define the Filter for the Export (Simple)
--

In this step, you will see how to implement the helper function that creates the filter for the export. Implement a new helper function called ```GetFilterSimple```, which returns a **FilterExpression** object, i.e.:

```cs
private FilterExpression GetFilterSimple()
```

Suppose that you want the export to only output TUs where the *Customer* field is equal to the value Microsoft. Note that in this example, *Customer* is a picklist field that allows multiple values. The following sample code shows you how to set the field name and value and how to build a filter criterion:

```cs
PicklistItem fieldName = new PicklistItem("Customer");
MultiplePicklistFieldValue fieldValue = new MultiplePicklistFieldValue("Microsoft");
fieldValue.Add(fieldName);
```
In the next step you use the **AtomicExpression** class to create the filter expression to return to the export function. The parameters required are the field value and the operator. In our case, the filter calls for an **Equal** value. (Other possible values could be **Contains**, **ContainsNot**, **Greater**, **Smaller**, etc.)

```cs
AtomicExpression filter = new AtomicExpression(fieldValue, AtomicExpression.Operator.Equal);
return filter;
```

Define the Filter for the Export (Advanced)
--

Suppose you want to create a more advanced filter that has two (or more) criteria, e.g. export all TUs where the Customer field value equals Microsoft**OR** in which the *Project id* text field contains the string *2010*. Implement another helper function called ```GetFilterAdvanced```, i.e.:

```cs
private FilterExpression GetFilterAdvanced()
```

The first step is almost identical to the previous example, i.e. you define the first filter criterion and create the first filter 
expression using the **AtomicExpression** class.


```cs
PicklistItem fieldName1 = new PicklistItem("Customer");
MultiplePicklistFieldValue fieldValue1 = new MultiplePicklistFieldValue("Microsoft");
fieldValue1.Add(fieldName1);
AtomicExpression expression1 = new AtomicExpression(fieldValue1, AtomicExpression.Operator.Equal);
```

Then, you set up the second filter criterion, i.e. *Project id* contains *2010* and use the **AtomicExpression** class to construct the second filter expression:

```cs
MultipleStringFieldValue fieldName2 = new MultipleStringFieldValue("Project id");
fieldName2.Add("2010");
AtomicExpression expression2 = new AtomicExpression(fieldName2, AtomicExpression.Operator.Contains);
```

The actual filter object that is returned to the export function is based on the **ComposedExpression** class, which combines expression1 and expression2 with an OR operator, and finally returns the complete filter expression.


```cs
ComposedExpression filter = new ComposedExpression(expression1, ComposedExpression.Operator.Or, expression2);
return filter;
```

Putting it All Together
--

The complete class should now look as shown below:

```cs
namespace Sdl.SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TMExporter
    {
        #region "export"
        public void ExportTMXFile(string tmPath, string exportFilePath)
        {
            #region "open"
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);

            TranslationMemoryExporter exporter = new TranslationMemoryExporter(tm.LanguageDirection);
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
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);
            TranslationMemoryExporter exporter = new TranslationMemoryExporter(tm.LanguageDirection);
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
            PicklistItem fieldName = new PicklistItem("Customer");
            MultiplePicklistFieldValue fieldValue = new MultiplePicklistFieldValue("Microsoft");
            fieldValue.Add(fieldName);
            #endregion

            #region "SimpleFilter"
            AtomicExpression filter = new AtomicExpression(fieldValue, AtomicExpression.Operator.Equal);
            return filter;
            #endregion
        }
        #endregion

        #region "GetFilterAdvanced"
        private FilterExpression GetFilterAdvanced()
        {
            #region "AdvancedCriterion1"
            PicklistItem fieldName1 = new PicklistItem("Customer");
            MultiplePicklistFieldValue fieldValue1 = new MultiplePicklistFieldValue("Microsoft");
            fieldValue1.Add(fieldName1);
            AtomicExpression expression1 = new AtomicExpression(fieldValue1, AtomicExpression.Operator.Equal);
            #endregion

            #region "AdvancedCriterion2"
            MultipleStringFieldValue fieldName2 = new MultipleStringFieldValue("Project id");
            fieldName2.Add("2010");
            AtomicExpression expression2 = new AtomicExpression(fieldName2, AtomicExpression.Operator.Contains);
            #endregion

            #region "AdvancedFilter"
            ComposedExpression filter = new ComposedExpression(expression1, ComposedExpression.Operator.Or, expression2);
            return filter;
            #endregion
        }
        #endregion
    }
}
```
See Also
--



[Importing a TMX File](importing_a_tmx_file.md)

[Introduction to the Batch Export Tool](introduction_to_the_tm_batch_export_tool.md)

[Scheduled TMX Exports](scheduled_tmx_exports.md)
