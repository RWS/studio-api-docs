Platform support
=====
Find information on the platform support for the Trados Studio public APIs.

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.


Platform Support
-----

* Terminology Provider
    - Available starting with Trados Studio 2015 SR2 or later. 
    - Even though the Terminology Provider API is a pure .Net API, it relies on certain 32 bit-only components. Therefore, you can only run applications that use this API in 32-bit mode. If your application also targets 64-bit platforms, make sure to set its platform target to 'x86', which will force your application to run in WoW64 mode (Windows 32-bit On Windows 64-bit).
* Batch Task
    - Available starting with SDL Studio 2015 SR2 or later.
    - Even though the Batch Task API is a pure .Net API, it relies on certain 32 bit-only components. Therefore you can only run applications that use this API in 32-bit mode. If your application also targets 64-bit platforms, then you must make sure to set its platform target to 'x86', which will force your application to run in WoW64 mode (Windows 32-bit On Windows 64-bit).
* Project Automation
    - Event through the Project Automation API is a pure .Net API, it relies on certain 32 bit-only components. Therefore you can only run applications that use this API in 32-bit mode. If your application also targets 64-bit platforms, then you must make sure to set it's platform target to 'x86', which will force your application to run on WoW64 mode (Windows 32-bit On Windows 64-bit).