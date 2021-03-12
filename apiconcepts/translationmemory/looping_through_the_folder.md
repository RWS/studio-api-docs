Looping through the Folder(s)
====
The user of your sample application is required to enter a main path. The application then loops through the files in that path and any files contained in sub-folders of the main path. The application logic for traversing the folders and sub-folders will be implemented in a dedicated iterator class. This class will be used both for looping through all *.sdltm, which then need to be exported to *.tmx.

Add a New Class
-----
Add a class called TmIterator to your project. At the beginning of the class, declare two constants, one to specify how deep into the sub-folder structure the recursion should go. For example, we will hard-code the depth to 10, so that the application will go down to 10 sub-folder levels. The second constant sets the recursion level, which should be 1:

Then add a public function called ProcessDir:

This function takes the main folder entered by the user in the command line interface as string parameter, and a boolean parameter that indicates whether sub-folders should be processed through self-recursion.

Implement the Recursion
----

Within the function implement an if, which makes the function loop through all sub-folders until the recursion level has reached the maximum depth.

Next, you iterate through the files found in a given directory. However, the files should only be processed if they match the provided extension, i.e. *.sdltm. If the extension sdltm is encountered, an export will be triggered, which we will implement in a separate class in a later step (see Importing into the Master Translation Memories).

After all files of the given folder have been processed accordingly, the function needs to re-trigger itself, so that any files in a sub-folder (if applicable) can be processed. The boolean processSubFolders parameter determines whether the recursion should be triggered or not. If this parameter is False, then only the main path will be processed.

Trigger the Function
-----
The ProcessDir function needs to be called from Main in the Program class as shown below. The user provides two parameters:

The main translation memory path
the /y parameter to indicate that sub-folders (if any) should be processed
Example:

The following code in Program.cs is used for explaining the application use and retrieves the parameters for the file processing. It also checks the validity of the input folder:

Putting it All Together
-----
The complete class should now look as shown below: