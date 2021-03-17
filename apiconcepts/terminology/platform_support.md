Platform support
=====
Find information on the Terminology Provider API platform support.

Platform Support
-----

* Terminology Provider API is available starting with Studio 2015 SR2 or later.
* Even though the Terminology Provider API is a pure .Net API, it relies on certain 32 bit-only components. Therefore, you can only run applications that use this API in 32-bit mode. If your application also targets 64-bit platforms, then you must make sure to set its platform target to 'x86', which will force your application to run in WoW64 mode (Windows 32-bit On Windows 64-bit).