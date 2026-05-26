Importing into the Master Translation Memories
======
The application is required to merge multiple *.tmx files of the same language direction into one master TM. If the master TM with the required language direction does not exist, the application needs to create one before it starts the import.

Add a New Class
-----
Start by adding a class called `TmImporter`. The main purpose of this class is to import the *.tmx files into master TMs and to trigger a separate function for creating the master TMs with the corresponding language direction if they are not yet available.

Implement a public function called `Import`, which takes the *.tmx file name and path as parameter. Like the export functionality, this function, too, is called from the `ProcessDirectory` function of the `TmIterator` class (see [Looping through the Folder(s)](looping_through_the_folders.md)).

Create the Master TM Path
------
In the next step, set up the path in which the master TMs should be created. To keep this implementation as simple as possible, we will just hard-code the path name. The path should only be created when it does not yet exist:

# [C#](#tab/tabid-1)
```cs
// Create the path in which the master TMs should be
// stored (if the path does not exist yet).
string masterPath = @"c:\MasterTMs\";
if (!System.IO.Directory.Exists(masterPath))
{
    System.IO.Directory.CreateDirectory(masterPath);
}
```
****

## Determine the Language Direction

When the application generates the `.tmx` files, it appends the language direction to the TM file names so that it can later retrieve that information and use it when creating and naming the master TMs. Start by parsing the file name to retrieve the language direction. Use the XML DOM API to read the information from the TMX content. The following example shows a translation unit in TMX format:

# [XML](#tab/tabid-2)
```xml
<tu creationdate="20090504T230613Z" creationid="PEGASUS\Ziad" changedate="20100507T224806Z" changeid="PEGASUS\Administrator" lastusagedate="20100507T224806Z" usagecount="4">
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
***

The source and target language locales can be retrieved from the `lang` attributes of the `tuv` elements inside the `tu` nodes. For this implementation, assume that the `.tmx` files:

- are bilingual
- contain at least one TU
- use the same source and target language locales for all TUs in a single `.tmx` file
  
Proceed as follows: select the first `tu` element and retrieve the values of the `lang` attributes from the first `tuv` element, which contains the source language, and the second `tuv` element, which contains the target language:

# [C#](#tab/tabid-3)
```cs
// Retrieve the language direction of the current TM
// by reading the appropriate lang values from the first
// tu element.
string srcLang = string.Empty;
string trgLang = string.Empty;
var xmlDoc = new XmlDocument();
xmlDoc.Load(tmxPath);

try
{
    XmlNode tu = xmlDoc.SelectNodes("tmx/body/tu")[0];
    srcLang = tu.SelectNodes("tuv")[0].Attributes[0].Value;
    trgLang = tu.SelectNodes("tuv")[1].Attributes[0].Value;
}
catch
{
    // TU count is zero, i.e. no TM content to import
    Console.WriteLine(tmxPath + " is empty, no content to import.");
}
```
****

## Create the Master TMs

Next, check whether a master TM for the language direction of the current `.tmx` file already exists. If it does not, call a separate method to create it (see [Creating the Master Translation Memories](creating_the_master_translation_memories.md)).
# [C#](#tab/tabid-4)
```cs
// Check whether a master TM with the language direction found in the 
// current TMX file already exists. If not, then create the TM.
string translationMemoryPath = string.Format("{0}MasterTM_{1}_{2}.sdltm", masterPath, srcLang, trgLang);
if (!System.IO.File.Exists(translationMemoryPath))
{
    var tmCreator = new TmCreator();
    tmCreator.CreateMasterTm(srcLang, trgLang, masterPath);
}
```
****

## Process the Import

Now import the `.tmx` file into the master TM:
# [C#](#tab/tabid-5)
```cs
// Open the appropriate master TM and do the import.
var tm = new FileBasedTranslationMemory(translationMemoryPath);
var importer = new TranslationMemoryImporter(tm.LanguageDirection);
this.GetImportSettings(importer.ImportSettings);
importer.Import(tmxPath);
```
****

At this point, call a separate method to configure the import settings.
# [C#](#tab/tabid-6)
```cs
/// <summary>
/// Configures the import settings.
/// </summary>
/// <param name="importSettings">Settings that control the import behavior.</param>
private void GetImportSettings(ImportSettings importSettings)
{
    importSettings.CheckMatchingSublanguages = true;
    importSettings.ExistingFieldsUpdateMode = ImportSettings.FieldUpdateMode.Merge;
    importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite;
}
```
***

For more information about import functionality and available settings, see [Importing a TMX File](importing_a_tmx_file.md).

## Putting it All Together

The complete class should look like this:
# [C#](#tab/tabid-7)
```cs
using System;
using System.Xml;
using Sdl.LanguagePlatform.TranslationMemory;
using Sdl.LanguagePlatform.TranslationMemoryApi;

namespace SDK.LanguagePlatform.Samples.BatchImporter
{

    /// <summary>
    /// Represents functionality for importing .tmx files into a translation memory.
    /// </summary>
    public class TmImporter
    {
        /// <summary>
        /// Performs the actual TMX import into the appropriate master TM.
        /// </summary>
        /// <param name="tmxPath">Path to the .tmx file.</param>
        public void Import(string tmxPath)
        {     
            // Create the path in which the master TMs are stored
            // if it does not already exist.
            string masterPath = @"c:\MasterTMs\";
            if (!System.IO.Directory.Exists(masterPath))
            {
                System.IO.Directory.CreateDirectory(masterPath);
            }
         
            // Retrieve the language direction of the current TM by
            // reading the lang values from the first tu element.
            string srcLang = string.Empty;
            string trgLang = string.Empty;
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(tmxPath);

            try
            {
                XmlNode tu = xmlDoc.SelectNodes("tmx/body/tu")[0];
                srcLang = tu.SelectNodes("tuv")[0].Attributes[0].Value;
                trgLang = tu.SelectNodes("tuv")[1].Attributes[0].Value;
            }
            catch
            {
                // TU count is zero, so there is no TM content to import.
                Console.WriteLine(tmxPath + " is empty, no content to import.");
            }

            // Check whether a master TM with the language direction found in the
            // current TMX file already exists. If not, then create the TM.
            string translationMemoryPath = string.Format("{0}MasterTM_{1}_{2}.sdltm", masterPath, srcLang, trgLang);
            if (!System.IO.File.Exists(translationMemoryPath))
            {
                var tmCreator = new TmCreator();
                tmCreator.CreateMasterTm(srcLang, trgLang, masterPath);
            }

            // Open the appropriate master TM and run the import.
            var tm = new FileBasedTranslationMemory(translationMemoryPath);
            var importer = new TranslationMemoryImporter(tm.LanguageDirection);
            this.GetImportSettings(importer.ImportSettings);
            importer.Import(tmxPath);
        }
     
        /// <summary>
        /// Configures the import settings.
        /// </summary>
        /// <param name="importSettings">Settings that control the import behavior.</param>
        private void GetImportSettings(ImportSettings importSettings)
        {
            importSettings.CheckMatchingSublanguages = true;
            importSettings.ExistingFieldsUpdateMode = ImportSettings.FieldUpdateMode.Merge;
            importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite;
        }        
    }
}
```
***

## See Also

- [Creating the Master Translation Memories](creating_the_master_translation_memories.md)
- [Creating the Log File](creating_a_log_file.md)
