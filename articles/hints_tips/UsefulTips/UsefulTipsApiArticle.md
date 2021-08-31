# How to implement the Studio Useful Tips service

The `RwsAppStore.UsefulTips.Service` is a service provider for updating the Useful Tips that are displayed in <var:ProductName>.

***

## Add the Studio Useful Tips service to your project
You can add the nuget package to your project via the package manager user interface or console.

### Package Manager UI
* In Solution Explorer, right-click References and choose Manage NuGet Packages.
* Select nuget.org as the Package source.
* Search for `RwsAppStore.UsefulTips.Service` from the Browse tab.
* Select the package from the list and click Install.
* Accept any license prompts to finnish the installation.
<img style="display:block; " src="images/InstallFromNuget.png" />

### Package Manager Console 
* Alternatively, go to Tools > NuGet Package Manager > Package Manager Console.
* In the Package Manager Console, enter the command:
`Install-Package RwsAppStore.UsefulTips.Service -Version 2.1.9.5`

***
  
## Remarks

The Useful Tips service first checks for already existing tips before adding new ones. If tips exist in the Useful Tips collection, only those identified as new are added.  
If Trados Studio was not launched as administrator, the user may receive a message from the service asking to elevate the user rights before updating the Useful Tips collection in Trados Studio with the new tips from the plugin.
> [!Note]
> Administrator rights are required, as the local tip files that manage the Useful Tips collection in Trados Studio reside in the <var:ProductName> installation directory.
> Only a user with administrator access rights can modify files from the installation direcotry.  

### Update History

The Useful Tips service records all attempts made to add new tips to the Useful Tips collection in Trados Studio.  
This is necessary if the user did not update the Useful Tips collection when prompted in Trados Studio; in this case, the decision from the user will be persisted and no further attempt is made to add those tips to Studio.

**Q:** Where can I locate the _UpdateHistory.xml_ and _Settings.xml_ files of the Useful Tips service?  
**A:** They are both located in the users roaming directory:   
_C:\Users\\**[username]**\AppData\Roaming\RWS Community\UsefulTipsService\Settings_
> [!NOTE]
> Replace **[username]** with your OS login account name  

**Q:** How can the user add tips from the plugin to the Useful Tips collection in Trados Studio if they previously opted-out to adding them?  
**A:** The decision taken by the user to add tips from the plugin to the Useful Tips collection in Trados Studio is persisted in the _UpdateHistory.xml_ file. You can simply delete the _UpdateHistory.xml_ file, or change the **UpdateAttempts** property value for each record to be less than the **MaxUpdateAttempts** value managed in the _Settings.xml_  

***

## Sample Implementation

The following example creates an instance of the TipsProvider and adds a new Tip for a single language (i.e. en).
```cs
namespace RwsAppStore.Example.Services
{
    public class UsefulTipsService
    {
        public void AddUsefulTips()
        {
            var tipsProvider = new TipsProvider();
            tipsProvider.AddTips(GetTipContexts(), null);
        }

        private static List<TipContext> GetTipContexts()
        {
            var tipContexts = new List<TipContext>
            {
                new TipContext
                {
                    LanguageId = "en",
                    Tips = new List<Tip>
                    {
                        new Tip
                        {
                            Category = "[the plugin name]",
                            Context = "[the Id associated wth the plugin View]",
                            Content = "[full path to the Markdown File]",
                            Title = "My Tip",
                            Description = "This is an awesome Tip",
                            DescriptionImage = "[full path to the image file]"
                        }
                    }
                }
            };
            return tipContexts;
        }
    }
}
```

## API
```cs
/// <summary>
/// Add Tips to the 'Useful Tips' collection in Trados Studio
/// </summary>
/// <param name="tipContexts">A list of Tips that you would like to add to the 
/// 'Useful Tips' 
/// collection in Trados Studio</param>
/// <param name="applicationName">The name of the application; can be null
/// </param>
/// <param name="runasAdmin">
/// Elevate the user rights to admin; default: true.  If the app environment
/// is not running with Admin rights, then the user will receive a message from
/// the User Account Control (UAC) in Windows</param>
/// <returns>The number of Tips added to 'Useful Tips' collection in Trados
/// Studio</returns>
public int AddTips(List<TipContext> tipContexts, string applicationName, 
 bool runasAdmin = true)

/// <summary>
/// Remove Tips from the 'Useful Tips' collection in Trados Studio
/// </summary>
/// <param name="tipContexts">A list of Tips that you would like to remove from
/// the 'Useful Tips' collection.</param>
/// <param name="applicationName">The name of the application; can be null
/// </param>
/// <param name="runasAdmin">
/// Elevate the user rights to admin; default: true.  If the app environment
/// is not running with Admin rights, then the user will receive a message from
/// the User Account Control (UAC) in Windows</param>
/// <returns>The number of Tips removed from the collection</returns>
public int RemoveTips(List<TipContext> tipContexts, string applicationName, 
 bool runasAdmin = true)

/// <summary>
/// Get all Tips from the 'Useful Tips' collection in Trados Studio
/// </summary>
/// <returns>A list of Tips</returns>
public List<TipContext> GetAllTips()

/// <summary>
/// Read the Tip Contexts from the import file; required in the transaction when 
/// reading in the Tips with elevated access rights via UAC.
/// </summary>
/// <param name="filePath">full path to the Tips import file</param>
/// <returns>A list of Tips</returns>
public List<TipContext> ReadTipContextsImportFile(string filePath)

/// <summary>
/// Read the tips the import file
/// </summary>
/// <param name="filePath">full path to the Tips import file</param>
/// <returns>A list of Tips</returns>
public List<Tip> ReadTipsImportFile(string filePath)

/// <summary>
/// Creates an Tips import file, required during the transaction when reading
/// in Tips with elevated access rights via UAC to update the 'Tips.xml' file
/// in the Trados Studio installation directory.
/// </summary>
/// <param name="filePath">full path to the Tips import file</param>
/// <param name="tips">A list of Tips used to import to the 'Useful Tips' 
/// collection</param>
/// <returns>True if the file was created successfully</returns>
public bool CreateTipContextsImportFile(string filePath, List<TipContext> tips)

/// <summary>
/// Create a Tips import file
/// </summary>
/// <param name="filePath">full path to the Tips import file</param>
/// <param name="tips">A list of Tips used to import to the 'Useful Tips' 
/// collection</param>
/// <returns>Returns true if successful</returns>
public bool CreateTipsImportFile(string filePath, List<Tip> tips)
```
```cs
/// <summary>
/// The supported UI languages for Trados Studio
/// supported values [de, en, es, fr, it, ja, ko, ru, zh]</summary>
public List<string> SupportedLanguages
```
### Models
```cs
public class TipContext
{
    /// <summary>
    /// The UI language Id supported by Trados Studio; 
    /// supported values [de, en, es, fr, it, ja, ko, ru, zh]
    /// </summary>
    public string LanguageId { get; set; }

    /// <summary>
    /// Tips available in the current language context
    /// </summary>
    public List<Tip> Tips { get; set; }
}
```
 
```cs
public class Tip
{
    /// <summary>
    /// The unique Id that identifies the tip in the collection.
    /// If an Id is provided then the service will first confirm if  
    /// it is unique in the 'Useful Tips' collection; if not a new 
    /// unique Id will be provided automatically.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The title displayed in the 'Useful Tips' view part
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The description displayed in the 'Useful Tips' view
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Full path to the description image that is displayed in the 
    /// 'Useful Tips' view part
    /// </summary>
    public string DescriptionImage { get; set; }

    /// <summary>
    /// The link text displayed in the 'Useful Tips' view part
    /// </summary>
    public string LinkText { get; set; }

    /// <summary>
    /// The view or view part Id; this ensure that the tip is only 
    /// visible in that context
    /// </summary>
    public string Context { get; set; }

    /// <summary>
    /// The category used to group the Tips; recommend to use the 
    /// plugin name or view name
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Full path to the icon
    /// </summary>
    public string Icon { get; set; }        

    /// <summary>
    /// Full path to the Markdown file that is loaded when the user 
    //// clicks on the 'Link Text' link
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Identify whether the Tip should be recognized as a new Tip
    /// </summary>
    public bool IsNew { get; set; }

    /// <summary>
    /// Display the Tip on the welcome wizard
    /// </summary>
    public bool ShowOnWelcomeWizard { get; set; }
}
```