Setting up a Development Machine
=====

Make sure you have the right prerequisites and meet the system requirements to develop applications that leverage the Integration API.

* For the development environment, we recommend that you use Microsoft Visual Studio 2013 or Microsoft Visual Studio 2015.
* You need a licensed Trados Studio.
* We also recommend installation of the Trados Studio SDK on your machine. You can get the latest version from the developer hub. Note that after the installation of Trados Studio SDK, the **New Project** dialog box from Microsoft Visual Studio will feature additional project templates specific to the Trados Studio application development.

> [!NOTE]
> As build output path for your implementations, please choose the *C:\Users\[username]\AppData\Roaming\SDL\SDL Trados Studio\[StudioVersionNumber]\Plugins\Packages.*

> [!NOTE]
>Also check that your library references are pointing to the Trados Studio folder. e.g. *C:\Program Files\SDL\SDL Trados Studio\Studio[StudioVersionNumber]*.

For more information on building and deploying a Trados Studio plug-in, see (Building a plug-in and Plug-in deployment)

Sign and use Strong-Named Assemblies to enable the loading of your plug-ins inside the Trados Studio.

How to: Sign an Assembly with a Strong Name

Choosing a different build output path or not signing your assembly will prevent your plugin from loading.

System requirements for running Trados Studio:

* The minimum requirements for running Trados Studio consist of a PC with a Pentium IV or higher compatible processor and 1 GB RAM. SDL recommends a recent Intel Core 2 processor or equivalent and 2 GB RAM.
* A mouse or similar pointing device is required.
* A minimum of 2 GB of hard disk space should be available.
* Trados Studio runs with Windows 7 32-bit/64-bit, Windows 8/10 32-bit/64-bit. Earlier operating system generations such as Windows XP, 2000, Windows NT and Windows 98 are not supported.
* Trados Studio requires Microsoft .Net Framework 4.5.2.
