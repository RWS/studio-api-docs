# Threading Support
This section covers multithreading considerations for the Project Automation API.

## Multithreading
The [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) class is not thread-safe. This means it is not safe to access the same instance of [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) from two or more different threads.

Working with multiple projects simultaneously on different threads is not recommended. To create projects in parallel, use separate processes rather than separate threads within the same process.
