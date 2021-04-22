Importing into the Master Translation Memories
======
The application is required to merge multiple *.tmx files of the same language direction into one master TM. If the master TM with the required language direction does not exist, the application needs to create one before it starts the import.

Add a New Class
-----
Start by adding a class called `TmImport`. The main purpose of this class is to import the *.tmx files into master TMs and to trigger a separate function for creating the master TMs with the corresponding language direction if they are not yet available.

Implement a public function called `Import`, which takes the *.tmx file name and path as parameter. Like the export functionality, this function, too, is called from the `ProcessDir` function of the `TmIterator` class (see [Looping through the Folder(s)](looping_through_the_folders.md)).

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

Determine the Language Direction
-------
When generating the *.tmx files we appended the language direction to the TM file names, so that we can retrieve this information for creating and naming the master TMs accordingly. Start by parsing the file name string to retrieve the language direction information. To determine the language direction we use the XML DOM API. Below you see an example of a translation unit in TMX format:

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

The source/target language locales can be retrieved from the lang attributes of the tuv elements inside the tu nodes. For our implementation we make the following assumptions about the *.tmx files :
* the files are all bilingual
* each file contains at least one TU
* all TUs in one *.tmx file have the same source/target language locales
  
We therefore proceed as follows: We select the first tu element, and then retrieve the values of the `lang` attributes of the first (i.e. the source) `tuv`, and then the second (i.e. the target) `tuv` element:

# [C#](#tab/tabid-3)
```cs
// Retrieve the language direction of the current TM
// by reading the appropriate lang values from the first
// tu element.
string srcLang = string.Empty;
string trgLang = string.Empty;
XmlDocument xmlDoc = new XmlDocument();
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

Create the Master TMs
------
Next, check whether a master TM for the language direction of the current *.tmx file already exists. If this is not the case, a separate function should be called to create the TM (see [Creating the Master Translation Memories](creating_the_master_translation_memories.md).
# [C#](#tab/tabid-4)
```cs
// Check whether a master TM with the language direction found in the 
// current TMX file already exists. If not, then create the TM.
string translationMemoryPath = string.Format("{0}MasterTM_{1}_{2}.sdltm", masterPath, srcLang, trgLang);
if (!System.IO.File.Exists(translationMemoryPath))
{
    TMCreator create = new TMCreator();
    create.CreateMasterTm(srcLang, trgLang, masterPath);
}
```
****

Process the Import
------
Now import the given *.tmx file into the master TM:
# [C#](#tab/tabid-5)
```cs
// Open the appropriate master TM and do the import.
FileBasedTranslationMemory tm = new FileBasedTranslationMemory(translationMemoryPath);
TranslationMemoryImporter importer = new TranslationMemoryImporter(tm.LanguageDirection);
this.GetImportSettings(importer.ImportSettings);
importer.Import(tmxPath);
```
****

Note that at this point we call a separate function to configure the import settings.
# [C#](#tab/tabid-6)
```cs
/// <summary>
///  Configure the import settings.
/// </summary>
/// <param name="importSettings">Group of settings that control way of import.</param>
private void GetImportSettings(ImportSettings importSettings)
{
    importSettings.CheckMatchingSublanguages = true;
    importSettings.ExistingFieldsUpdateMode = ImportSettings.FieldUpdateMode.Merge;
    importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite;
}
```
***

For more information on the import functionality and possible settings, please refer to [Importing a TMX File]().

Putting it All Together
-----

The complete class should look as shown below:
# [C#](#tab/tabid-7)
```cs
namespace Sdl.SDK.LanguagePlatform.Samples.BatchImporter
{
    using System;
    using System.Xml;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    /// <summary>
    /// Represents functionality for emporting *.tmx files into translation memory.
    /// </summary>
    public class TMImporter
    {
        #region "ImportTmx"

        /// <summary>
        /// This function performs the actual TMX import into the appropriate master TM.
        /// </summary>
        /// <param name="tmxPath">Path to *.tmx file.</param>
        public void Import(string tmxPath)
        {
            #region "CreatePath"
            // Create the path in which the master TMs should be
            // stored (if the path does not exist yet).
            string masterPath = @"c:\MasterTMs\";
            if (!System.IO.Directory.Exists(masterPath))
            {
                System.IO.Directory.CreateDirectory(masterPath);
            }
            #endregion

            #region "GetLanguageDirection"
            // Retrieve the language direction of the current TM
            // by reading the appropriate lang values from the first
            // tu element.
            string srcLang = string.Empty;
            string trgLang = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
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
            #endregion

            #region "CheckMasterTmExists"
            // Check whether a master TM with the language direction found in the 
            // current TMX file already exists. If not, then create the TM.
            string translationMemoryPath = string.Format("{0}MasterTM_{1}_{2}.sdltm", masterPath, srcLang, trgLang);
            if (!System.IO.File.Exists(translationMemoryPath))
            {
                TMCreator create = new TMCreator();
                create.CreateMasterTm(srcLang, trgLang, masterPath);
            }
            #endregion

            #region "DoImport"
            // Open the appropriate master TM and do the import.
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(translationMemoryPath);
            TranslationMemoryImporter importer = new TranslationMemoryImporter(tm.LanguageDirection);
            this.GetImportSettings(importer.ImportSettings);
            importer.Import(tmxPath);
            #endregion
        }
        #endregion

        #region "settings"

        /// <summary>
        ///  Configure the import settings.
        /// </summary>
        /// <param name="importSettings">Group of settings that control way of import.</param>
        private void GetImportSettings(ImportSettings importSettings)
        {
            importSettings.CheckMatchingSublanguages = true;
            importSettings.ExistingFieldsUpdateMode = ImportSettings.FieldUpdateMode.Merge;
            importSettings.ExistingTUsUpdateMode = ImportSettings.TUUpdateMode.Overwrite;
        }
        #endregion
    }
}
```
***

See Also
--------
[Creating the Master Translation Memories](creating_the_master_translation_memories.md)

[Creating the Log File](creating_a_log_file.md)
