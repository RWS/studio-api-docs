Setting up a Developer Machine
=====
Make sure you have the right prerequisites and meet the system requirements to develop applications that leverage the <Var:ProductName> public APIs.

Prerequisites
----

* For the development environment, we recommend using Microsoft Visual Studio 2019.
* You need a licensed <Var:ProductNameWithEdition> or later installed on your development machine
* We also recommend installation of the **<Var:ProductName> SDK** on your machine. Note that after the installation of **<Var:ProductName> SDK**, the **New Project** dialog box from Microsoft Visual Studio will feature additional project templates specific to the <Var:ProductName> application development.
  * You can get the latest version from the [developer hub](https://appstore.sdl.com/language/developers/sdk.html). 
  * Or you can install directly from Visual Studio 2019 -> Tools -> Extensions
  <img style="display:block; " src="images/VisualStudioExtensions.png" />
* If your implementations need to connect to a TM Server system, then make sure that the TM Server also runs the latest version.
* If you are developing against a version of API the which is distributed with one of SDL's publicly released applications (i.e. <Var:ProductNameWithEdition>), then all required assemblies and files should be available alongside the application. Make sure that you have the latest release of <Var:ProductName> installed.
  
> [!NOTE]
>
> As build output path for your implementations please choose the c:\Users\ {username}\AppData\Roaming\SDL\SDL Trados Studio\ <Var:ProductVersion>\Plugins\Packages
>
> Also check that your library references are pointing to the <Var:ProductName> folder. e.g. C:\Program Files\SDL\SDL Trados Studio\ <Var:ProductVersion>.
>
> For more informations regarding how to build and deploy a Studio plug-in see [Building a plug-in](building_a_plugin.md) and [Plug-in deployment](plugin_deployment.md)
>
> Sign and use Strong-Named Assemblies to enable the loading of your plug-ins inside <Var:ProductName>. For more information see [How to: Sign an Assembly with a Strong Name](https://docs.microsoft.com/en-us/dotnet/standard/assembly/sign-strong-name?redirectedfrom=MSDN)
> 
> Choosing a different build output path or not signing your assembly will prevent your plugin to be loaded.


System requirements for running <Var:ProductNameWithEdition>:
----
* A Microsoft Windows-based PC or an Intel-based Apple Mac computer running Windows as an operating system. <Var:ProductNameWithEdition> runs on the latest build of Windows 10 and the latest updated version of Windows 8.1.
* Up to 2.5 GB of available disk space to run the <Var:ProductNameWithEdition> installer
* 2 GB of available disk space to run <Var:ProductNameWithEdition>
* A recent processor with dual or multi-core technology
* At least 8 GB RAM
* <Var:ProductNameWithEdition> requires Microsoft .Net Framework 4.8.
* Additional details can be found on the [product release notes](https://docs.sdl.com/binary/813470/802652/sdl-trados-studio-2021-sr1/sdl-trados-studio-release-notes)

System requirements for running SDL GroupShare (SDL TM Server, SDL Multiterm, SDL Project Server):
----

* Windows Server 2019, with IIS 10 / Windows Server 2016, with IIS 10 / Windows Server 2012 R2, with IIS 8.5
* The database servers supported are: SQL Server 2019 / SQL Server 2017 /  SQL Server 2016
* Additional details can be found on the [product release notes](https://docs.sdl.com/binary/797358/802198/sdl-trados-groupshare-2020-sr1/groupshare2020sr1-release-notes)
