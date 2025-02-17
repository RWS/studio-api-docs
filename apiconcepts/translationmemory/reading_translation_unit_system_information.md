Reading Translation Unit System Information
==

Translation units are associated with a set of system information, which is added automatically to each TU during translation and editing. This means that the system keeps track of such information automatically, no TM fields have to be created or maintained for this kind of information.

About Translation Unit System Information
--

For each TU, a set of information is stored, i.e. the date/time when the TU was created and the date/time the TU was last changed (if applicable). In addition, the name of the user who created/changed the TU is also stored as well as the name of the user who last used the TU and when (if applicable). Furthermore, a usage counter is maintained, i.e. the system tracks how often a TU has been used. Using a TU means that it has been found during a TM lookup operation, and the suggested translation has been inserted into the document, thus being 'used'. This allows you, for example, to filter for TUs that have never been used, and delete them in order to keep the TM lean and efficient. The screenshot below shows an example of the system information that is kept for a particular TU in Var:ProductName:

![SystemInfo](images/SystemInfo.jpg)


Add a New Class
--

In the following you will learn how to retrieve the system information of a TU programmatically. Start by adding a new class called ```TuSystemInfo```. Implement a public function called ```GetInfo``` to your class, which uses the TM file name and path as string parameter. First, you open the TM and retrieve a particular TU by searching a segment as demonstrated below:

# [C#](#tab/tabid-1)
```cs
var tm = new FileBasedTranslationMemory(tmPath);

SearchResults results = tm.LanguageDirection.SearchText(this.GetSearchSettings(), "A dialog box will open.");
```
***

Then you create a TU object based on the search result, from which you derive a system info object through which you compile the TU system information output string:

# [C#](#tab/tabid-2)
```cs
string tuInfo = string.Empty;
foreach (SearchResult item in results)
{
    if (item.ScoringResult.Match == 100)
    {
        TranslationUnit tu = item.MemoryTranslationUnit;
        SystemFields sysFields = tu.SystemFields;

        tuInfo = "Creation date: " + sysFields.CreationUser + "\n";
        tuInfo += "Creation user: " + sysFields.CreationUser + "\n";
        tuInfo += "Change date: " + sysFields.ChangeDate + "\n";
        tuInfo += "Change user: " + sysFields.ChangeUser + "\n";
        tuInfo += "Usage count: " + sysFields.UseCount + "\n";
        tuInfo += "Last used on: " + sysFields.UseDate + "\n";
        tuInfo += "Last used by: " + sysFields.UseUser + "\n";
        break;
    }
}

MessageBox.Show(tuInfo, "TU Information");
```
***

Putting it All Together
--

The complete class should now look as shown below:

# [C#](#tab/tabid-3)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemory;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TuSystemInfo
    {
        #region "GetInfo"
        public void GetInfo(string tmPath)
        {
            #region "open"
            var tm = new FileBasedTranslationMemory(tmPath);

            SearchResults results = tm.LanguageDirection.SearchText(this.GetSearchSettings(), "A dialog box will open.");
            #endregion

            #region "output"
            string tuInfo = string.Empty;
            foreach (SearchResult item in results)
            {
                if (item.ScoringResult.Match == 100)
                {
                    TranslationUnit tu = item.MemoryTranslationUnit;
                    SystemFields sysFields = tu.SystemFields;

                    tuInfo = "Creation date: " + sysFields.CreationUser + "\n";
                    tuInfo += "Creation user: " + sysFields.CreationUser + "\n";
                    tuInfo += "Change date: " + sysFields.ChangeDate + "\n";
                    tuInfo += "Change user: " + sysFields.ChangeUser + "\n";
                    tuInfo += "Usage count: " + sysFields.UseCount + "\n";
                    tuInfo += "Last used on: " + sysFields.UseDate + "\n";
                    tuInfo += "Last used by: " + sysFields.UseUser + "\n";
                    break;
                }
            }

            MessageBox.Show(tuInfo, "TU Information");
            #endregion
        }
        #endregion

        #region "settings"
        private SearchSettings GetSearchSettings()
        {
            var settings = new SearchSettings();
            settings.MinScore = 100;
            return settings;
        }
        #endregion
    }
}
```
***
