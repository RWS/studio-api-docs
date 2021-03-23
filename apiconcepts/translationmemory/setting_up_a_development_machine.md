Setting up a Development Machine
====
* As development environment we recommend that you use **Microsoft Visual Studio 2010**.
* If you are developing against a version of the which is distributed with one of SDL's publicly released applications (i.e. Trados Studio), then all required assemblies and files should be available alongside the application. Make sure that you have the latest release of Trados Studio installed.
* If your implementations need to connect to a TM Server system, then make sure that the TM Server also runs the latest version.
* Your development machine also requires installation of the SDL SDK on your machine by running the corresponding **Setup.exe**. Note that after installation of the **SDL SDK** the **New Project** dialog box of Microsoft Visual Studio 2008 will feature additional **SDL Plug-in Project** templates, which are required for developing global verifier and translation provider plug-ins.

> [!NOTE]
> As build output path for your implementations please choose the installation folder of Trados Studio, e.g. C:\Program Files\SDL\SDL Trados Studio\Studio5. The only exceptions are currently global verifier and the translation provider plug-ins (see About the Sample Translation Service Provider Plug-in).

System requirements for running Trados Studio:
----

* The minimum specification to run Trados Studio is a PC with a Pentium IV or higher compatible processor and 1 GB RAM. We recommend a recent Intel Core 2 processor or equivalent and 2 GB RAM.
* A mouse or similar pointing device is required.
* A minimum of 2 GB of hard disk space should be available.
* Trados Studio runs with Windows XP, Windows Vista 32-bit/64-bit, Windows 7 32-bit/64-bit and Windows 8 32-bit/64-bit. Earlier operating system generations such as Windows 2000, Windows NT and Windows 98 are not supported.
* Trados Studio requires the Microsoft .Net Framework 3.5 SP1
  
**System requirements for running GroupShare (TM Server, Multiterm, Project Server):**

* GroupShare runs with Windows Server 2003 and Windows Server 2008 (32-bit/64-bit).
* Single CPU and multi-CPU computers are supported.
* We recommend a recent mid-range server with an Intel Xeon CPU and 4 GB of RAM.
* The database server should have SCSI hard disks with 100 GB or more storage space in a RAID architecture.
* To administer TM Server, or to provide web services from the SDL Server, you need Microsoft IIS.
