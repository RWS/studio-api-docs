Merging files
=====
When creating projects that involve multiple files, users have the option of merging several files into one. This can facilitiate typical editing operations such as find/replace, document verification, etc.

During project creation, users can choose to merge several files into one intermediary (SDLXliff) document. Example: You want to generate a single intermediary document from 100 small XML documents. Handling only one file instead of 100 can faciliate common editing operations such as find/replace, QA check, etc. considerably, as these tasks can then be run on a single file rather than on multiple documents.

<img style="display:block; " src="images/Merge01.jpg"/>

Merging several native files of different formats into one intermediary (SDLXliff) document

Var:ProductName even allows you to merge different native file formats into one intermediary document (e.g. PPT, XLS, DOC, XML, etc.). In the editor of Var:ProductName markers indicate where a file ends and where the next file begins. Depending on where the user has currently positioned the cursor, a native preview is generated for that particular document (provided that the file type plug-in for that specific file format supports the generation of a preview).


<img style="display:block; " src="images/Merge02.jpg"/>

Delimiters indicate where one file ends and where the next one begins

When the user generates the native documents, the intermediary file that was generated during the merge operation is split into the individual native target documents. (For example, when merging 10 PPT files, you will not generate one big PPT document, but one SDLXliff document.)

See Also
--------
[Creating projects](creating_projects.md)
