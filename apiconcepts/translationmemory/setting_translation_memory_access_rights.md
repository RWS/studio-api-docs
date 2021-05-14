Setting Translation Memory Access Rights
==

As a general rule, file-based TMs can generally be opened and used by anyone who is equipped with <Var:ProductName>. As long as you do not assign passwords to a TM file, any user can have unrestricted access to it (e.g. read/write, import/export, etc.) to the translation memory. However, you may protect a TM file with passwords that allow access to only a certain level of functionality.

There are four different access levels for file based TMs:

* **Administrator**: Can perform any TM-related operation, i.e. read/write, change settings, import/export.
* **Maintenance**: Can perform operations such as global find/replace in a TM (but no change of TM settings and no import/export).
* **Read/Write**: This access level is typically used by translators, who need to be able to add/change TUs and search the TM.
* **Read-only**: Guest access that allows users to perform TM look-ups.

>**Note**
>
>Server TMs use a different, much more sophisticated and granular access rights model.


When a user opens a password-protected TM in <Var:ProductName> the following prompt will be shown where you can select the required access level and then enter the corresponding password:

![PwPompt](images/PwPompt.jpg)

Setting Passwords Programmatically
--

Add a new class to your project, which you call ```TmProtector```. Add a public function called ```AssignPasswords``` to your newly-created class, which takes the file name and path as parameter. This function can be called as shown below:

```cs
TMProtector objProtect = new TMProtector();
objProtect.AssignPasswords(_translationMemoryFilePath);
```


The API offers four different methods for setting passwords corresponding to the four access levels that are available for file-based TMs. When applying these methods you need to provide the password to set as string parameter. Note that when setting passwords, a specific order has to be observed. For example, a read-only password can only be set *after* a read/write password has been assigned. The function for setting the passwords can look as shown in the following example:

```cs
public void AssignPasswords(string tmPath)
{
    FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);

    tm.SetAdministratorPassword("super");
    tm.SetMaintenancePassword("maintain");
    tm.SetReadWritePassword("translator");
    tm.SetReadOnlyPassword("guest");
    tm.Save();

    this.OpenProtectedTm(tmPath, "super");
}
```

Note that the function for setting the passwords calls a separate helper function to open the password-protected TM.

Open a Password-protected TM
--

Add the following function to your class, which opens the TM with the previously assigned administrator password. To open a password-protected TM, the password needs to be provided as a string parameter. Of course, you should catch any exception, e.g. in cases in which the wrong password was entered.

```cs
private void OpenProtectedTm(string tmPath, string password)
{
    try
    {
        FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath, password);
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
    }
}
```

Putting it All Together
--

The complete class should look as shown below:

```cs
namespace Sdl.SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class TMProtector
    {
        #region "assign"
        public void AssignPasswords(string tmPath)
        {
            FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath);

            tm.SetAdministratorPassword("super");
            tm.SetMaintenancePassword("maintain");
            tm.SetReadWritePassword("translator");
            tm.SetReadOnlyPassword("guest");
            tm.Save();

            this.OpenProtectedTm(tmPath, "super");
        }
        #endregion

        #region "openTMwithPW"
        private void OpenProtectedTm(string tmPath, string password)
        {
            try
            {
                FileBasedTranslationMemory tm = new FileBasedTranslationMemory(tmPath, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
```
