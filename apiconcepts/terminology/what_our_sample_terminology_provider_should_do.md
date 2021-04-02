What our Sample Provider Should do
=====

Learn how to develop a custom terminology provider by folowing the creation of a simplified sample project which uses the Terminology Provider API.

The example terminology provider will:

* Use a semicolon-separated, two-column text file as a terminology resource. The lines in the glossary file have the following struture: line number (which corresponds to the entry number), the source term, the target term and a definition. For example, *2;photo printer;Fotodrucker;Peripheral device used create hardcopies of images*.
* Select the terminology text file through a standard **Open File** dialog box.
* Read the header of the text file to retrieve the languages and the locales of the terminology resource. The header line has the following format: *English,en-US;German,de-DE*.
* Search the current segment in <Var:ProductName> for any known terms.
* Allow the user to enter a search string and look it up in the text file.
* Add term pairs to the text file.
* View the content of entries in an Internet Explorer control.

Note that our sample project only involves a highly simplified, 'Hello World'-style implementation to familiarize you with the basic concepts of terminology provider plug-ins. As terminology resource, we support only simple, two-column (source/target language) text files. The conceived search function is based on simple text searches with no stemming or fuzzy matching logic.