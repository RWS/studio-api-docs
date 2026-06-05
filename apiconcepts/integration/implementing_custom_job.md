# Implementing a Custom Job

The `IExternalJob` and `IExternalJobWithProgress` interfaces enable you to inject custom code into wizards and Var:ProductName's job mechanism. This approach maintains consistent look and feel for long-running processing jobs while improving performance.

To use a custom job, implement one of these interfaces and publish `ExecuteExternalJobEvent` using `IStudioEventAggregator`. Var:ProductName picks up the job and executes it on a background thread or injects it into the specified events. If the job is cancelled, handle it in the `JobCanceled` method.

| Interface        |  Purpose  |
| ------------- | -----|
| `IExternalJob`| Use this interface for long-running processing jobs. The logic executes automatically on a background thread.|
| `IExternalJobWithProgress` | Use this interface for long-running processing jobs that report progress. Trigger the `ProgressReported` event handler to report job progress to the user. |

## Code Sample

The following example shows a custom job implementation:

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
> The `JobData` property acts as a bridge between Var:ProductName and your plug-in when using the job inside the **Open Package** and **Create Return Package** wizards. Var:ProductName sets the file path on the property as follows:
> - The file path to the project imported into Var:ProductName in the **Open Package** wizard
> - The file path for the created package in the **Create Return Package** wizard
>
> Reference the available constants from [PackagingConstants](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Packaging.PackagingConstants.yml).
