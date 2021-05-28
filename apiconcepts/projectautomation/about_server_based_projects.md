About Server Based Projects
=====
Group Share 2017 Project Server allows projects to be shared between teams. This chapter explains how to work with server-based projects.

How server based projects work
------
Server projects are stored online and allow Studio users to collaborate on translation projects in a more efficient and centralized way. Server projects can have multiple users working on different project files at the same time without having to distribute project packages containing the work to be completed. Project managers are also able to track the progress of the work that has been checked in.

Project Server is a central repository for your server project and the files within that project. Project Server offers project management functionality and controls the permissions for accessing the project and resources within that project

As all work is carried out on a local copy, files must be checked out and downloaded before changes can be made. Once you have finished with a file it must be checked back in and uploaded to the server. All other operations are performed locally on the files just like a standard project.

Project Server API overview
-----
The API to work with server projects consists of the ProjectServer class and extensions to the FileBasedProject class which allow you to connect to a server and work with the files stored there.

**ProjectServer Class**

This class allows you to connect to a Project Server and provides the following methods

* Open an project from a server and create a new local copy

* List projects on the server

* Delete a project from the server
  

**Project Server Extensions to the FileBasedProject Class**

Extensions have been added to the FileBasedProject class to allow the class to work with server files which include:

* A constructor to open existing project server from a local copy.

* Publishing a project to the server.

* Checking out files.

* Checking in files.

* Undoing checked in files.

* Downloading the latest version of a file.

* Listing previous file versions.

* Downloading a previous version of a file.

* Uploading files.

* Marking the server project as complete.

See Also
--------
[Connecting a Project to a Project Server](connecting_a_project_to_a_project_server.md)

[Viewing and Deleting Published Projects](viewing_and_deleting_published_projects.md)

[Checking Files In and Out](checking_files_in_and_out.md)

[Downloading and Uploading Files](checking_files_in_and_out.md)

[Putting it All Together](putting_it_all_together.md)