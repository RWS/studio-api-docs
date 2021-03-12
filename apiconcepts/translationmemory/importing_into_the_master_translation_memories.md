Importing into the Master Translation Memories
======
The application is required to merge multiple *.tmx files of the same language direction into one master TM. If the master TM with the required language direction does not exist, the application needs to create one before it starts the import.

Add a New Class
-----
Start by adding a class called TmImport. The main purpose of this class is to import the *.tmx files into master TMs and to trigger a separate function for creating the master TMs with the corresponding language direction if they are not yet available.

Implement a public function called Import, which takes the *.tmx file name and path as parameter. Like the export functionality, this function, too, is called from the ProcessDir function of the TmIterator class (see Looping through the Folder(s)).

Create the Master TM Path
------
In the next step, set up the path in which the master TMs should be created. To keep this implementation as simple as possible, we will just hard-code the path name. The path should only be created when it does not yet exist:

Determine the Language Direction
-------
When generating the *.tmx files we appended the language direction to the TM file names, so that we can retrieve this information for creating and naming the master TMs accordingly. Start by parsing the file name string to retrieve the language direction information. To determine the language direction we use the XML DOM API. Below you see an example of a translation unit in TMX format:

The source/target language locales can be retrieved from the lang attributes of the tuv elements inside the tu nodes. For our implementation we make the following assumptions about the *.tmx files :
* the files are all bilingual
* each file contains at least one TU
* all TUs in one *.tmx file have the same source/target language locales
* 
We therefore proceed as follows: We select the first tu element, and then retrieve the values of the lang attributes of the first (i.e. the source) tuv, and then the second (i.e. the target) tuv element:

Create the Master TMs
------
Next, check whether a master TM for the language direction of the current *.tmx file already exists. If this is not the case, a separate function should be called to create the TM (see Creating the Master Translation Memories).

Process the Import
------
Now import the given *.tmx file into the master TM:

Note that at this point we call a separate function to configure the import settings.

For more information on the import functionality and possible settings, please refer to Importing a TMX File.

Putting it All Together
-----

The complete class should look as shown below: