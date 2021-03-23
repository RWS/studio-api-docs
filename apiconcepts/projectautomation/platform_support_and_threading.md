Platform Support and Threading
=====
This section contains information on platform support and information about multithreading when using the Project Automation API.

Platform Support
------
Event through the Project Automation API is a pure .Net API, it relies on certain 32 bit-only components. Therefore you can only run applications that use this API in 32-bit mode. If your application also targets 64-bit platforms, then you must make sure to set it's platform target to 'x86', which will force your application to run on WoW64 mode (Windows 32-bit On Windows 64-bit).

Multithreading
-----
The `FileBasedProject` class is not thread-safe. This means it is not safe to access the same instance of `FileBasedProject` from two or more different threads.

In addition, it is not recommended to work with multiple projects simultaneously on different threads. If you do want to create an application that, for example, creates projects simultaneously, we recommend that you to do this in separate processes, rather than in separate threads within the same process.