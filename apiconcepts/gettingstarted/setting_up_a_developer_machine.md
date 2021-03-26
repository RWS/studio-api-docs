Setting up a Developer Machine
=====
Make sure you have the right prerequisites and meet the system requirements to develop applications that leverage the Trados Studio public APIs.

Prerequisites
----

* For the development environment, we recommend using Microsoft Visual Studio 2019.
* You need a licensed Trados Studio 2021 SR1 or later installed on your development machine
* We also recommend installation of the **Trados Studio SDK** on your machine. You can get the latest version from the [developer hub](https://appstore.sdl.com/language/developers/sdk.html). Note that after the installation of **Trados Studio SDK**, the **New Project** dialog box from Microsoft Visual Studio will feature additional project templates specific to the Trados Studio application development.
* If your implementations need to connect to a TM Server system, then make sure that the TM Server also runs the latest version.
* If you are developing against a version of API the which is distributed with one of SDL's publicly released applications (i.e. SDL Trados Studio 2021), then all required assemblies and files should be available alongside the application. Make sure that you have the latest release of SDL Trados Studio installed.
> [!NOTE]
> As build output path for your implementations please choose the c:\Users\{username}\AppData\Roaming\SDL\SDL Trados Studio\{StudioVersionNumber}\Plugins\Packages
>
> Also check that your library references are pointing to the SDL Trados Studio folder. e.g. C:\Program Files\SDL\SDL Trados Studio\Studio{StudioVersionNumber}.
>
> For more informations regarding how to build and deploy a Studio plug-in see [Building a plug-in](building_a_plugin.md) and [Plug-in deployment](plugin_deployment.md)
>
> Sign and use Strong-Named Assemblies to enable the loading of your plug-ins inside the SDL Trados Studio. For more information see [How to: Sign an Assembly with a Strong Name](https://docs.microsoft.com/en-us/dotnet/standard/assembly/sign-strong-name?redirectedfrom=MSDN)
> 
> Choosing a different build output path or not signing your assembly will prevent your plugin to be loaded.


System requirements for running Trados Studio:
----
* A Microsoft Windows-based PC or an Intel-based Apple Mac computer running Windows as an operating system. Trados Studio runs on the latest build of Windows 10 and the latest updated version of Windows 8.1.
* Up to 2.5 GB of available disk space to run the SDL Trados Studio 2021 SR1 installer
* 2 GB of available disk space to run Trados Studio
* A recent processor with dual or multi-core technology
* At least 8 GB RAM
* Trados Studio requires Microsoft .Net Framework 4.8.

System requirements for running SDL GroupShare (SDL TM Server, SDL Multiterm, SDL Project Server):
----

* Windows Server 2012 and Windows Server 2008 (32-bit/64-bit)
* Single CPU and multi-CPU computers are supported.
* We recommend a recent mid-range server with an Intel Xeon CPU and 4 GB of RAM.
* The database server should have SCSI hard disks with 100 GB or more storage space in a RAID architecture.
* To administer TM Server, or to provide web services from the SDL Server, you need Microsoft IIS.

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.