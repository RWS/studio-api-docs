# Setting and Retrieving TM Properties

After you create a translation memory, you can retrieve and update properties such as the description, expiration date, and translation unit count.

## Add a New Class

Implement the property logic in a separate class named `TmProperties`. Add a method named `GetAndSetProperties()` that takes the TM file path as a string parameter, as shown below:

# [C#](#tab/tabid-1)
```cs
var tmProperties = new TmProperties();
tmProperties.GetAndSetProperties(_translationMemoryFilePath);
```
***

After you open the TM, set the properties you need. Many more properties are available, but this example shows only a few:

# [C#](#tab/tabid-2)
```cs
tm.Description = "This is the TM description.";
tm.Copyright = "(c) 2021 RWS Group";
tm.ExpirationDate = DateTime.Now.AddDays(7);
```
***

The code sets a TM description, a copyright string, and an expiration date. In this example, the TM expires after seven days. An expiration date lets you time-limit a TM, which can help when you share it with a freelancer and want to prevent use after a deadline.

The following example retrieves a few TM properties and displays them in a message box.

# [C#](#tab/tabid-3)
```cs
string tmInfo;
tmInfo = "TU count: " + tm.GetTranslationUnitCount().ToString();
tmInfo += "; Password-protected: " + tm.IsProtected.ToString();
tmInfo += "; Expires on: " + tm.ExpirationDate.ToString();
MessageBox.Show(tmInfo);
```
***


## Putting it All Together

The complete class should now look like this:

# [C#](#tab/tabid-4)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TmProperties
    {
        public void GetAndSetProperties(string tmPath)
        {
            var tm = new FileBasedTranslationMemory(tmPath);

            #region "set"
            tm.Description = "This is the TM description.";
            tm.Copyright = "(c) 2021 RWS Group";
            tm.ExpirationDate = DateTime.Now.AddDays(7);
            #endregion

            #region "get"
            string tmInfo;
            tmInfo = "TU count: " + tm.GetTranslationUnitCount().ToString();
            tmInfo += "; Password-protected: " + tm.IsProtected.ToString();
            tmInfo += "; Expires on: " + tm.ExpirationDate.ToString();
            MessageBox.Show(tmInfo);
            #endregion
        }
    }
}
```
***

