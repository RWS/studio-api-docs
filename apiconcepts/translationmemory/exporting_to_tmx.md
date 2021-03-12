Exporting to TMX
-----
In this step you will learn how to implement the functionality for exporting the content of file TMs (*.sdltm files) to *.tmx.

Add a class called TmExporter to your project. Then implement a function called Export, which takes the name and path of the TM file as a string parameter. Within this function you first open the file TM using the tmPath parameter.Then create an exporter object based on the TM and its language direction. When applying the Export method to the exporter object, you specify the export file name and path. Let us assume that the TMX file should be stored in the same path as the file TM. Also, the name should be the same as the file TM plus the language direction and the extension TMX, e.g. *sample.sdltm_en-US_de-DE.tmx*. By including the language direction in the export file name, we can determine into which master TM the *.tmx file should be imported later. That way we can avoid futile import operations (e.g. importing a French-English TMX file into a German-Spanish master TM).

Putting it All Together
----
The complete class should look as shown below: