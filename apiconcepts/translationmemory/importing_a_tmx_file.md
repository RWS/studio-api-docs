Importing a TMX File
==

You can import bilingual content stored in TMX (Translation Memory Exchange) files into a translation memory. TMX is a standard, XML-based exchange format for translation memories.

Add a new Class
--

Add a new class called ```TmImporter``` to your project.

Continue by implementing a function function called ```ImportTMYFile``` in the class. This function can be called as shown below:

```cs
TMImporter objTmImport = new TMImporter();
objTmImport.ImportTMXFile(_translationMemoryFilePath, _importFilePath);
```

The function requires the file name and path of the TM and the TMX document as string parameters.

First, open the TM into which the TMX content should be imported. Then create an importer object, which takes the TM language direction as parameter. Note that like a TM a TMX file, too, is bilingual with a dedicated language direction, which should match the TM language direction. Therefore you need to provide the TM language direction as parameter to the ```TranslationMemoryImporter``` object as shown in the example below:

```cs
FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);
TranslationMemoryImporter importer = new TranslationMemoryImporter(tm.LanguageDirection);
```

Next, define the import chunk size. With the chunk size you determine the maximum amount of units that should be read from the TMX file in one go. As long as the import file resides on a local disk, the chunk size can be large, as the whole file can potentially be read at once, as there are no Internet connection required, which may cause latency. If the import is going over e.g. an Internet line, then the chunk size ([ChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_ChunkSize)) should be small enough for the packages to be sent over the WAN. In our example we set the chunk size to 20, which corresponds roughly to the number of TUs contained in our small TMX sample file. Since we are running this import from the local hard disk, we can easily afford to take in the entire set of TUs in the TMX file. Note that the default chunk size is 50 ([DefaultTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_DefaultTranslationUnitChunkSize)), the maximum chunk size is 200 ([MaxTranslationUnitChunkSize](../../api/translationmemory/Sdl.Core.TM.ImportExport.Importer.yml#Sdl_Core_TM_ImportExport_Importer_MaxTranslationUnitChunkSize).


```cs
importer.ChunkSize = 20;
```

Configure the Import Settings
--

There are a number of settings that you may configure for an import operation. We configure the settings by calling a separate helper function:

```cs
this.GetImportSettings(importer.ImportSettings);
```

Now add a helper function called ```GetImportSettings```, which takes the importer settings as parameter. Below you find a number of examples of options that you may configure for the import operation:

Through the **CheckMatchingSublanguages** property you can determine whether the import should take sublanguages into account. Example: The TM into which you import has English (US) as source language, but the TMX file contains English (UK) translation units. Setting this property to False will cause the import to treat US and UK English equally, i.e. the UK units will be added to the US memory. However, if you configure the import to check for sublanguages, UK units will *not* be added to the US memory, i.e. UK English will be regarded as being a different language.

```cs
importSettings.CheckMatchingSublanguages = false;
```


With the **ExistingFieldsUpdateMode** property you can determine what should happen to TM fields of already existing TUs. Example: The TM already contains a TU in which the field Customer has the value *Microsoft*. Now, let us assume that the TMX file contains the same TU (i.e. same source and target segment), however, the TU from the import file has the *Customer* value *SAP*. Through the **FieldUpdateMode** enumerator you can determine whether the original value of the TU in the TM should be overwritten, left unchanged, or whether the values should be merged, so that you end up with a TU that contains both field values, i.e.: *Customer*: *Microsoft*, *SAP*. Note that TM fields can also be defined to allow for only one value. If no multiple values are allowed for a particular field, then by default the import value will overwrite the existing value, unless you have set the field update mode to **LeaveUnchanged**.

```cs
importSettings.ExistingFieldsUpdateMode = ImportSettings.FieldUpdateMode.Merge;
```

You may also configure the import to accept only TUs from the TMX file that have specific confirmation status values. You can thereby, for example, exclude TUs from the import that are in draft status, or that have no confirmation status value at all. Below you see an example of what a TU looks like in a TMX file. Note that the confirmation level value in this example is **ApprovedTranslation**.

```xml
<tu creationdate="20100507T161524Z" creationid="Ziad" changedate="20100507T161524Z" changeid="PEGASUS\Administrator" lastusagedate="20100507T161524Z">
  <prop type="x-Origin">TM</prop>
  <prop type="x-ConfirmationLevel">ApprovedTranslation</prop>
  <prop type="x-Customer:MultiplePicklist">Microsoft</prop>
  <tuv xml:lang="en-US">
    <seg>A dialog box will open.</seg>
  </tuv>
  <tuv xml:lang="de-DE">
    <seg>Es öffnet sich ein Dialogfenster.</seg>
  </tuv>
</tu>
```

The code example below shows how to configure the operation to allow only the import of TUs with the confirmation status values [ApprovedTranslation](../../api/core/Sdl.Core.Globalization.ConfirmationLevel.yml#fields) and [Translated](../../api/core/Sdl.Core.Globalization.ConfirmationLevel.yml#fields).

```cs
ConfirmationLevel[] levels = { ConfirmationLevel.ApprovedTranslation, ConfirmationLevel.Translated };
importSettings.ConfirmationLevels = levels;
```

During import it may happen that invalid TUs are encountered in the TMX file. If you want such units to be written to an exclusion file for further analysis and troubleshooting you may specify a path and name for the file that is supposed to contain the invalid TUs, which failed to import as shown in the example below:

```cs
importSettings.InvalidTranslationUnitsExportPath = @"c:\temp\invalid.tmx";
```


Also, you can determine whether the import should be allowed to overwrite TUs that already exist in the TM. Example: The TM contains a particular TU, which is also stored in the TMX import file. However, the TU in the import file has a different target segment. Through the **OverwriteExistingTUs** property you can set whether the import is allowed to overwrite the TU in the TM, thereby replacing the existing target segment with the import target segment (True), or whether the existing translation should be left unchanged (False).

```cs
importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite;
```

You may also import the TMX content in a way that only the plain text will be added to the TM. This means that all tags (e.g. inline formatting information) will be stripped from the TUs. If this is the case, you need to set the PlainText property to True. This is the most effective way to rid the import TUs of formatting information that is no longer required. A possible use case is the import of TMX files that come from third-party systems, that handle inline tags and formatting different from <Var:ProductName>. Adding that kind of formatting to the TM in <Var:ProductName> may degrade the matching quality and might 'clutter' the TM with formatting information that is not required, or that cannot be properly handled in <Var:ProductName>.

Adding only the plain text information gives you the chance to start 'fresh' with existing linguistic content. Another use case: In the past your source documentation was created in Microsoft Word 2000 (RTF), however, now you have switched to XML. The inline tags of those two formats are likely to be very different. You therefore want to strip all the old tags from the RTF-based format to handle current and future XML files more efficiently.

If you decide to import the text with tags you may also set a tag count limit (**TagCountLimit**). By setting this property to e.g. 10, you will prevent any TUs with a higher tag count to end up in your TM. That way you can prevent your TM from being cluttered with TUs that contain a large number of tags (e.g. segments in which different formatting was applied to every single letter). Example:



```cs
importSettings.PlainText = false;
importSettings.TagCountLimit = 10;
```

The screenshot below shows an example of TUs that contain inline tags:


![TUsWithTags](images/TUsWithTags.jpg)

hermore, you can determine whether the usage count of the import TUs should be kept track of after the TUs have been added to the TM. Using a TU means that a translator retrieves the TU from the TM, and then inserts the translation into the document, thereby 'using' the translation, which, in turn, increments the usage counter:

```cs
importSettings.IncrementUsageCount = true;
```


Below is an example of a TU with a usage counter. Note the **usagecount** attribute of the **tu** element.

```xml
<tu creationdate="20090504T230613Z" creationid="Ziad" changedate="20100507T161030Z" changeid="PEGASUS\Ziad" lastusagedate="20090630T172121Z" usagecount="2">
  <prop type="x-Context">0, 0</prop>
  <prop type="x-Origin">TM</prop>
  <prop type="x-Customer:MultiplePicklist">Microsoft</prop>
  <tuv xml:lang="en-US">
    <seg>The Check Spelling Command</seg>
  </tuv>
  <tuv xml:lang="de-DE">
    <seg>Der Befehl Rechtschreibung</seg>
  </tuv>
</tu>
```

Executing the Import
--

Before we execute the actual import we fire an event that should be triggered after the import of each batch (i.e. chunk). The event is triggered as shown in the example below:


```cs
importer.BatchImported += new EventHandler<BatchImportedEventArgs>(this.importer_BatchImported);
```

Add the following member to your class, which outputs the statistics after the import of each batch (which is limited by the chunk size that you specified):

```cs
private void importer_BatchImported(object sender, BatchImportedEventArgs e)
{
    string info;
    ImportStatistics stats = e.Statistics;

    info = "Total read: " + stats.TotalRead + "\n";
    info += "Total imported: " + stats.TotalImported + "\n";
    info += "TUs added: " + stats.AddedTranslationUnits + "\n";
    info += "TUs discarded: " + stats.DiscardedTranslationUnits + "\n";
    info += "TUs merged: " + stats.MergedTranslationUnits + "\n";
    info += "Errors: " + stats.Errors + "\n";

    MessageBox.Show(info, "Import statistics of current chunk");
    e.Cancel = false;
}
```

The statistics include the total number of TUs read, and the number of TUs that were actually imported. Note that very often the number of imported TUs is lower than the total number of TUs that were read from the TMX file. This is because TUs may be invalid, they may be duplicated and thus merged with other TUs, etc.

Finally, you apply the ```Import``` method, which takes the TMX import file name and path as parameter:

```cs
importer.Import(importFilePath);
```

The complete ```ImportTMXFile``` function looks as shown below:

```cs
public void ImportTMXFile(string tmPath, string importFilePath)
{
    #region "CreateImporter"
    FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);
    TranslationMemoryImporter importer = new TranslationMemoryImporter(tm.LanguageDirection);
    #endregion

    #region "chunk"
    importer.ChunkSize = 20;
    #endregion

    #region "GetSettings"
    this.GetImportSettings(importer.ImportSettings);
    #endregion

    #region "FireEvent"
    importer.BatchImported += new EventHandler<BatchImportedEventArgs>(this.importer_BatchImported);
    #endregion

    #region "execute"
    importer.Import(importFilePath);
    #endregion
}
```


Putting it All Together
--

The complete class should now look as shown below:

```cs
// ---------------------------------
// <copyright file="TmImporter.cs" company="SDL International">
// Copyright  2010 All Right Reserved
// </copyright>
// <author>Patrik Mazanek</author>
// <email>pmazanek@sdl.com</email>
// <date>2010-09-27</date>
// ---------------------------------
namespace Sdl.SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Windows.Forms;
    using Sdl.Core.Globalization;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TMImporter
    {
        #region "import"
        public void ImportTMXFile(string tmPath, string importFilePath)
        {
            #region "CreateImporter"
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);
            TranslationMemoryImporter importer = new TranslationMemoryImporter(tm.LanguageDirection);
            #endregion

            #region "chunk"
            importer.ChunkSize = 20;
            #endregion

            #region "GetSettings"
            this.GetImportSettings(importer.ImportSettings);
            #endregion

            #region "FireEvent"
            importer.BatchImported += new EventHandler<BatchImportedEventArgs>(this.importer_BatchImported);
            #endregion

            #region "execute"
            importer.Import(importFilePath);
            #endregion
        }
        #endregion

        #region "event"
        private void importer_BatchImported(object sender, BatchImportedEventArgs e)
        {
            string info;
            ImportStatistics stats = e.Statistics;

            info = "Total read: " + stats.TotalRead + "\n";
            info += "Total imported: " + stats.TotalImported + "\n";
            info += "TUs added: " + stats.AddedTranslationUnits + "\n";
            info += "TUs discarded: " + stats.DiscardedTranslationUnits + "\n";
            info += "TUs merged: " + stats.MergedTranslationUnits + "\n";
            info += "Errors: " + stats.Errors + "\n";

            MessageBox.Show(info, "Import statistics of current chunk");
            e.Cancel = false;
        }
        #endregion

        #region "settings"
        private void GetImportSettings(ImportSettings importSettings)
        {
            #region "sublanguages"
            importSettings.CheckMatchingSublanguages = false;
            #endregion

            #region "update"
            importSettings.ExistingFieldsUpdateMode = ImportSettings.FieldUpdateMode.Merge;
            #endregion

            #region "ConfirmationLevels"
            ConfirmationLevel[] levels = { ConfirmationLevel.ApprovedTranslation, ConfirmationLevel.Translated };
            importSettings.ConfirmationLevels = levels;
            #endregion

            #region "InvalidPath"
            importSettings.InvalidTranslationUnitsExportPath = @"c:\temp\invalid.tmx";
            #endregion

            #region "overwrite"
            importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite;
            #endregion

            #region "PlainText"
            importSettings.PlainText = false;
            importSettings.TagCountLimit = 10;
            #endregion

            #region "UsageCount"
            importSettings.IncrementUsageCount = true;
            #endregion
        }
        #endregion
    }
}
```

See Also
--

**Other Resources**

[Exporting to a TMX File](exporting_to_a_tmx_file.md)

[Scheduled TMX Imports](scheduled_tmx_imports.md)

[Introduction to the Batch Import Tool](introduction_to_the_tm_batch_import_tool.md)

