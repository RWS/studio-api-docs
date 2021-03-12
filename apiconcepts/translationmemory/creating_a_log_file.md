Creating the Log File
======
Let us assume that at the end of the import process you would like to create a log file that lists all master TMs that were created alongside the total TU count per TM.

Add a New Class
-----
Add a class called TmLog, which includes the function CreateLogFile. This function, which takes the main path for the *.tmx import files (i.e. the folder specified by the user) as parameter, and is called from Main.

First, create the log text file as follows:

In the next step, loop through the master TM files and output the master TM names and the total translation unit count:

Finally, close the log file:

Putting it All Together
-----
The complete class should look as shown below: