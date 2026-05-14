# Enhancing Studio Interactivity

With the release of Var:ProductName 2019 SR2, we introduced significant improvements to our public APIs. These enhancements provide third-party developers with greater access to Studio's functional flows and user interface styling. This article outlines the new functionalities available in the APIs and how to leverage them effectively.
## Improved Notification System

The notification system in Var:ProductName, accessible via the Notification View pane (on the right-hand side), was designed to promote notifications non-intrusively while translators interact with Studio. Recognizing its shortcomings during new feature development, we addressed these issues and made the improvements available in the public APIs.

To enable translators to remove specific notifications, we introduced two new properties in the `IStudioNotification` interface. These properties give developers full control over which notifications can be dismissed and the actions triggered during the removal process.

| Property        | Type           | Purpose  |
| ------------- |:-------------| -----|
| `AllowsUserToDismiss`| `bool` | Set this to *true* for the user to see a *dismiss* button when hovering over a notification. |
| `ClearNotificationAction`| `IStudioNotificationCommand`      |   Assign an instance of the implemented interface to this property. The *dismiss* button will trigger the behavior specified in your implementation. |

We further enhanced the notification system by exposing additional events. These events provide full access to creating and managing notification groups, including adding notifications to groups, removing them, or creating new groups if they do not exist.

| Event        |  Purpose  |
| ------------- | -----|
| `AddStudioNotificationOrCreateGroupEvent`| Trigger this event to add an `IStudioNotification` instance to an existing group or create a new group if the group with the given key already exists. The event expects the unique identifier of the group, the notification to be added and a group title in case it needs to create a new group.|
| `RemoveStudioNotificationFromGroupEvent` | Trigger this event via the **EventAggregator** to remove a specific notification identified by its unique id from the group with the given key. |

### Code Example

The following code snippet demonstrates how to publish dismissible notifications using the `IStudioEventAggregator`.

```cs
    //create a unique identifier for the notification
    var notificationId = Guid.NewGuid();
    //create an instance of IStudioNotification
    var sampleNotification = new StudioTestNotification(notificationId)
    {
        Title = "Sample Notification Title",
        AlwaysVisibleDetails = new List<string> 
        { 
            "Will be added to an existing group or will create a new one." 
        },
        AllowsUserToDismiss = true,
        ClearNotificationAction = new ClearNotificationAction(GroupId, notificationId),
        IsExpanderVisible = false,
        IsLinkVisible = true,
        LinkAction = new OpenLinkCommand("https://appstore.sdl.com/language/developers/sdk.html")
        {
            CommandText = "Learn more on the Studio 2019 SDK",
            CommandToolTip = "Learn more on the Studio 2019 SDK"
        },
        IsActionVisible = false
    };

    //create an instance of the event
    var addTestGroup = new AddStudioNotificationOrCreateGroupEvent(
                                GroupId, 
                                sampleNotification, 
                                NotificationGroupTitle);
    //publish the event via the Event Aggregator
    var ea = SdlTradosStudio.Application.GetService<IStudioEventAggregator>();
    ea.Publish(addTestGroup);
```
The `ClearNotificationAction` class implements the logic required to remove the notification from the group. It implements the `IStudioNotificationCommand` interface which is just an extension over the standard WPF `ICommand`.

```cs
public class ClearNotificationAction : IStudioNotificationCommand
{
    private readonly string _groupId;
    private readonly Guid _notificationId;

    public ClearNotificationAction(string groupNotificationKey, Guid notificationId)
    {
        .......//some constructor logic
    }

    ....... //Interface implementation (properties and methods)       
        
    public void Execute(object parameter)
    {
        var ea = SdlTradosStudio.Application.GetService<IStudioEventAggregator>();
        ea.Publish(new RemoveStudioNotificationFromGroupEvent(_groupId, _notificationId));
    }
}
```
If you are unclear about which properties need to be set in order to control various UI elements and inject custom behavior, we created a diagram mapping the elements of a **Notification Group** and a **Notification** to the properties of the `IStudioNotificationGroup` and `IStudioNotification` interfaces.

<img style="display:block; " src="images/NotificationDiagram.png" />


## Improved interactivity with projects
In this section we will present some of the new API events and interfaces that we exposed to help your plugins integrate better with Var:ProductName's project and package flows. 

#### Accessing the ***New Project*** wizard

With Studio 2019 SR2 you can now request for **New Project** wizard to be opened from your code by publishing the `OpenNewProjectWizardEvent` via the `IStudioEventAggregator`. This event will also allow you to pre-specify data for the new project in a `ProjectWizardData` object.

| Property        |  Description  |
| -------------  | -----|
| `ProjectName` | Use this property to pre-set a value for your project name.|
| `Content` | Use this enumeration to send a list of files or folders given by their full path. |
| `ProjectTemplate` | Set this property to the full path of a project template file (*.sdltpl) that will be used by the wizard for creating the project. |
| `ProjectReference` | Set this property to the full path of a project file (*.sdlproj) that will be used by the wizard for creating the project. |

Please note that you can either pass a project template or a project reference template. In the event you pass values for both, Var:ProductName will use the project template file.

Here is a small code sample on how to implement the event publishing:
```cs
var wizardData = new ProjectWizardData()
{
    ProjectName = "My Project",
    Content = new List<string>() { "C:\MySample.doc" }
};
_eventAggregator.Publish(new OpenNewProjectWizardEvent(wizardData));
```

And this is how it should behave in Studio:

<img style="display:block; " src="images/NewProject.gif" />

#### Executing custom logic within package flows

Up until Studio 2019 SR2 the only option you had to invoke the **Open Package** wizard  from a plugin was similar to this:
```cs
 var app = new StudioApplication();
 app.ExecuteAction<OpenPackageAction>();
```
Going forward, you will now be able to trigger the same **Open Package** wizard and pass along additional details such as the package file path, the project icon and even a custom job to be executed after the package is imported in Studio. Same as above, all you need to do is publish an `OpenProjectPackageEvent` object with your desired data.

<img style="display:block; " src="images/ProjectsList.png" />

| Property        |  Description  |
| -------------  | -----|
| `packageFilePath` | Use this parameter to specify a path to the package to be opened.|
| `job` | Use this parameter to pass an `IExternalJobWithProgress` object to be executed once the package was loaded in Studio. Leave this `null` if you do not wish to execute a custom job.  |
| `iconPath` | Use this parameter to set an icon for your project. This allows you to customize the Projects View and have project icons specific to your plugin. |
| `projectOrigin` | Use this parameter to set a project type. This allows you to customize the Projects View and have the project type specific to your plugin. |

<img style="display:block; " src="images/OpenProject.gif" />

Here is a code sample from the above capture. The details on how to implement a custom job will be presented later on in this article.
```cs
  var sampleJob = new SampleJob() { JobName = "Sample" };
  eventAggregator.Publish(new OpenProjectPackageEvent(packagePath, sampleJob, iconPath, projectType));
```

The same improvements were added for creating return packages, where you can specify a custom job to be executed when triggering the **Create Return Package** wizard. You might find these usefull if your plugin needs to upload the package to a server, as everything will be processed together within the wizard. To achieve this, publish the `CreateReturnPackageEvent` using the `IStudioEventAggregator`.

| Property        |  Description  |
| -------------  | -----|
| `id` | This is the project `Guid` which you can obtain from the `ProjectInfo` class.|
| `job` | Use this parameter to pass an `IExternalJobWithProgress` object to be executed once the return package was created. Leave this `null` if you do not wish to execute a custom job.  |

<img style="display:block; " src="images/CreateReturnPackage.gif" />

And this is the code sample:
```cs
var sampleJob = new SampleJob() { JobName = "Sample" };
var project = SdlTradosStudio.Application.GetController<ProjectsController>().CurrentProject;
if (project != null)
{
    var selectedProjectId = project.GetProjectInfo().Id.ToString();
    eventAggregator.Publish(new CreateReturnPackageEvent(selectedProjectid, sampleJob));
}
```
The `IExternalJob` and `IExternalJobWithProgress` interfaces are the bridge that will allow you to inject custom code into the wizards as well as into Var:ProductName's job mechanism. This gives you the advantage to achieve the same look and feel for long processing jobs besides helping you complete your work faster. Once you have an implementation ready all you need to do is publish  `ExecuteExternalJobEvent` using `IStudioEventAggregator`. As a result, Var:ProductName will pick the job and execute it on a background thread or inject it into the previously-mentioned events. If the job is cancelled for any reason, you can handle that from the `JobCanceled` method.

| Interface        |  Purpose  |
| ------------- | -----|
| `IExternalJob`| Implement this interface when you want to run a long processing job. The logic will be automatically executed on a background thread.|
| `IExternalJobWithProgress` | Implement this interface when you want to run a long processing job that can also report progress to the user. Triggering the `ProgressReported` event handler will allow you to report progress on the execution of your job to the user. |

Here is a code sample on how such an implementation should look like:

```cs
public class SampleJob : IExternalJobWithProgress
{
    //Use this property to hold any type of data required for your job
    public object JobData { get; set; }

    public string JobName { get; set; }

    public event EventHandler<JobProgressArgs> ProgressReported;

    public void JobCanceled(object sender, EventArgs e)
    {
        //Job canceled...do something
    }

    public void Execute()
    {
        ProgressReported?.Invoke(this, new JobProgressArgs()
        {
             PercentComplete = 0,
             StatusMessage = "Sample Job started"
        });
        
        //some long running operation
        ProgressReported?.Invoke(this, new JobProgressArgs()
        {
            PercentComplete = 100,
            StatusMessage = "Sample Job done"
        });
    }
}
```
> Please note that the `JobData` property acts as a bridge between Var:ProductName and your plugin in case of using the job inside the **Open Package** and **Create Return Package** wizards. Var:ProductName will set the file path on the property as follows:
> - the file path to the project that was just imported into Var:ProductName, in the case of the **Open Package** wizard
> - the file path for the created package, in the case of the **Create Return Package** wizard.

## Other improvements

In this section we will present some additional changes which we consider useful for plugin development.

#### Knowing when the Var:ProductName main window was created
There are situations when you want to know when your application has completed loading in order to perform a specific user operation. For example, you want to display a log-in prompt for the translators from your plugin. For such a scenario (but not only), we exposed the `StudioWindowCreatedNotificationEvent` which will be published from Var:ProductName once it completes loading. Now you only need to subscribe to this event and you are set to go. 

```cs
    protected override void Initialize(IViewContext context)
    {
        var ea = SdlTradosStudio.Application.GetService<IStudioEventAggregator>();
        ea.GetEvent<StudioWindowCreatedNotificationEvent>().Subscribe(OnStudioWindowCreated);
    }

    private void OnStudioWindowCreated(StudioWindowCreatedNotificationEvent e)
    {
        MessageBox.Show("Studio was loaded");
    }
```
#### Refreshing the Projects view
For situations where you would like to have the list of Var:ProductName projects refreshed in the Projects view you can now publish a `RefreshProjectsEvent` object via the event aggregator.

```cs
   eventAggregator.Publish(new RefreshProjectsEvent());
```


With this we bring to an end our improvements for Studio 2019 SR2 public APIs. You can find more code examples on [GitHub](https://github.com/sdl/trados-studio-api-samples), and make sure to checkout our other article on the exposed [WPF Styles] (../Studio_Styles/StudioStyles.md).
