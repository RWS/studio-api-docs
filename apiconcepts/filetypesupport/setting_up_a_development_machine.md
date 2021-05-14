Setting Up a Development Machine
==

* As development environment we recommend that you use **<Var:VisualStudioEdition>**.
* If you are developing against a version of the SDL File Type Support Framework which is distributed with one of SDL's publicly released applications (i.e. <Var:ProductName>), then all required assemblies and file type plug-in files should be available alongside the application. Make sure that you have the latest release of SDL Trados Studio 2017 installed, which is as the time of writing SDL Trados Studio 2017.
* If your implementations need to connect to a TM Server system, then make sure that the TM Server also runs the latest version.
* Your development machine also requires installation of the **SDL SDK** on your machine by running the corresponding **Setup.exe**. Note that after installation of the SDL SDK the New Project dialog box of Microsoft **<Var:VisualStudioEdition>** will feature additional **SDL Plug-in Project** templates, which are required for developing filters, global verifier and translation provider plug-ins.


**System requirements for running <Var:ProductName>:**

* The minimum specification to run <Var:ProductName> is a PC with a Pentium IV or higher compatible processor and 1 GB RAM. SDL recommends a recent Intel Core 2 processor or equivalent and 2 GB RAM.
* A mouse or similar pointing device is required.
* A minimum of 2 GB of hard disk space should be available.
* <Var:ProductName> runs with Windows XP, Windows Vista 32-bit/64-bit, Windows 7 32-bit/64-bit and Windows 8 32-bit/64-bit. Earlier operating system generations such as Windows 2000, Windows NT and Windows 98 are not supported.
* <Var:ProductName> requires the Microsoft .Net Framework 3.5 SP1

**System requirements for running SDL GroupShare (SDL TM Server, SDL Multiterm, SDL Project Server):**

* SDL GroupShare runs with Windows Server 2003 and Windows Server 2008 (32-bit/64-bit)
* Single CPU and multi-CPU computers are supported.
* We recommend a recent mid-range server with an Intel Xeon CPU and 4 GB of RAM.
* The database server should have SCSI hard disks with 100 GB or more storage space in a RAID architecture.
* To administer TM Server, or to provide web services from the SDL Server, you need Microsoft IIS.

>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.