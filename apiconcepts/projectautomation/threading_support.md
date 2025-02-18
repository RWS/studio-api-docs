Threading Support
=====
This section contains information about multithreading when using the Project Automation API.

Multithreading
-----
The [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) class is not thread-safe. This means it is not safe to access the same instance of [FileBasedProject](../../api/projectautomation/Sdl.ProjectAutomation.FileBased.FileBasedProject.yml) from two or more different threads.

In addition, it is not recommended to work with multiple projects simultaneously on different threads. If you do want to create an application that, for example, creates projects simultaneously, we recommend that you to do this in separate processes, rather than in separate threads within the same process.
