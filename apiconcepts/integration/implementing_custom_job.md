# Implementing a custom job

The `IExternalJob` and `IExternalJobWithProgress` interfaces are the bridge that will allow you to inject custom code into the wizards as well as into Var:ProductName's job mechanism. This gives you the advantage to achieve the same look and feel for long processing jobs besides helping you complete your work faster. Once you have an implementation ready all you need to do is publish  `ExecuteExternalJobEvent` using `IStudioEventAggregator`. As a result, Var:ProductName will pick the job and execute it on a background thread or inject it into the previously-mentioned events. If the job is cancelled for any reason, you can handle that from the `JobCanceled` method.

| Interface        |  Purpose  |
| ------------- | -----|
| `IExternalJob`| Implement this interface when you want to run a long processing job. The logic will be automatically executed on a background thread.|
| `IExternalJobWithProgress` | Implement this interface when you want to run a long processing job that can also report progress to the user. Triggering the `ProgressReported` event handler will allow you to report progress on the execution of your job to the user. |

Here is a code sample on how such an implementation should look like:

```cs
public class SampleJob : IExternalJobWithProgress
{
    public SampleJob()
    {
    }

    public string JobName { get; set; }
    
    public IDictionary<string, object> JobData { get; set; }

    public event EventHandler<JobProgressArgs> ProgressReported;

    public void JobCanceled(object sender, EventArgs e)
    {
        ProgressReported?.Invoke(this, new JobProgressArgs()
        {
            PercentComplete = 100,
            StatusMessage = "Sample Job was cancelled"
        });           
    }

    public void Execute(IExternalJobExecutionContext externalExecutionContext)
    {
        var packagelocation = JobData[PackagingConstants.ReturnPackageTargetPackageFilePath] as string;
        ProgressReported?.Invoke(this, new JobProgressArgs()
        {
            PercentComplete = 0,
            StatusMessage = "Sample Job Started for package at location: " + packagelocation
        });
        System.Threading.Thread.Sleep(1000);
        ProgressReported?.Invoke(this, new JobProgressArgs()
        {
            PercentComplete = 50,
            StatusMessage = "Sample Job Processing for package at location: " + packagelocation
        });
        System.Threading.Thread.Sleep(1000);
        ProgressReported?.Invoke(this, new JobProgressArgs()
        {
            PercentComplete = 100,
            StatusMessage = "Sample Job Completed for package at location: " + packagelocation
        });
    }
}
```

> [!NOTE]
> The `JobData` property acts as a bridge between Var:ProductName and your plugin in case of using the job inside the **Open Package** and **Create Return Package** wizards. Var:ProductName will set the file path on the property as follows:
> - the file path to the project that was just imported into Var:ProductName, in the case of the **Open Package** wizard
> - the file path for the created package, in the case of the **Create Return Package** wizard.
> All constants used can be referenced from [PackagingConstants](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.PackagingConstants.yml)
