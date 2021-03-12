Creating the Master Translation Memories
======
Remember that all *.tmx files of the same language direction need to be consolidated in a master TM. If the importer class (see Importing into the Master Translation Memories) determines that the corresponding master TM does not yet exist, it should be created through a separate class. On this page you will learn how to implement the class for creating the master TMs.

Add a New Class
-----
Start by adding a new class called TmCreator to your project. Add a public function called CreateMasterTm, which takes the path in which the master TMs should be created and the source/target language locales as string parameters.

Create the Master TM
------
In the next step, implement the logic required for creating the master TM file. Note that for simplicity's sake, the file name is a combination of a hard-coded string *(MasterTM_)*, the language direction string, and the extension *.sdltm.

The fuzzy indexes are determined through a separate helper function:

Also, the recognition settings are set in a distinct helper function:

For more information, on these settings and on TM creation see also Creating a File-based Translation Memory.

Putting it All Together
----
The complete class should look as shown below: