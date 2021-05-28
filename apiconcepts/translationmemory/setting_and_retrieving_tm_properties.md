Setting and Retrieving TM Properties
==

After creating a translation memory there are various properties that you can retrieve, set, or change such as the TM description, an expiration date, the translation unit count, etc.

Add a New Class
--

As we are going to implement the functionality for setting and retrieving the TM properties in a separate class. Create a new class called ```TmProperties```. Next, add a function called ```GetAndSetProperties```, which takes the TM file name and path as string parameter, and which can be called as shown below:

# [C#](#tab/tabid-1)
```cs
TMProperties objProps = new TMProperties();
objProps.GetAndSetProperties(_translationMemoryFilePath);
```
***

After opening the TM we set a number of properties. Note that there are many more properties that can be set. The code example only showcases a small selection:

# [C#](#tab/tabid-2)
```cs
tm.Description = "This is the TM description.";
tm.Copyright = "(c) 3021 RWS Group";
tm.ExpirationDate = DateTime.Now.AddDays(7);
```
***

The above part of the code sets a TM description and copyright string, and defines an expiry date for the TM. In this case, we select 7 days, after which the TM can no longer be used. Using an expiry date, you can 'time-bomb' a TM, which is useful, for example, if you give the TM to a freelancer, and you want to prevent him or her from using it after the project deadline has passed.

Below you see a few examples of properties that you can retrieve on a 
given TM and then output in a message box.

# [C#](#tab/tabid-3)
```cs
string tmInfo;
tmInfo = "TU count: " + tm.GetTranslationUnitCount().ToString();
tmInfo += "; Password-protected: " + tm.IsProtected.ToString();
tmInfo += "; Expires on: " + tm.ExpirationDate.ToString();
MessageBox.Show(tmInfo);
```
***


Putting it All Together
--

The complete class should now look as shown below:

# [C#](#tab/tabid-4)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TMProperties
    {
        public void GetAndSetProperties(string tmPath)
        {
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);

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

