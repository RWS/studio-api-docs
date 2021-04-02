Platform support
=====
Find information on the platform support for the <Var:ProductName> public APIs.

Platform Support
-----

Even though the <Var:ProductName> API is a pure .NET Framework API, it relies on certain 32 bit-only components. Therefore, you can only run applications that use this API in 32-bit mode. If your application also targets 64-bit platforms, make sure to set its platform target to 'x86', which will force your application to run in WoW64 mode (Windows 32-bit On Windows 64-bit).
