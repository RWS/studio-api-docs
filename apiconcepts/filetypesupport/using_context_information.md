Using context information
======
If applicable, a file type plug-in should not only extract translatable text to an intermediary file, it should also make context information available. This allows translators and editors to determine easily whether a string is a headline, table cell content, etc. (without actually seeing the real document layout). This kind of information can help translators do their job more effectively.

File type plug-ins effectively separate translatable content from the document layout. This offers the advantage of translators/editors being able to focus on the content. However, it is often useful for users to know whether the string they are currently translating or editing belongs to a table cell, footnote, etc. This is why a good file type plug-in should present such context information to the user. Any context information is displayed in the document structure column of the editor. To save space this column shows a short display code, e.g. **H** for headline. By moving the mouse pointer over the display code, the full description is shown in a tooltip. By double-clicking the display code, users can display even more information (if available).

<img style="display:block; " src="images/Context01.jpg"/>

Example of a context display code for text box content extracted from a PPT document:

<img style="display:block; " src="images/Context02.jpg"/>

Double-clicking the document structure column reveals additional information, in this case the context information on text box content contained on a PowerPoint slide.

Also, <Var:ProductName> can display a document structure tree in the navigation pane on the left-hand side of the application. This tree allows users to quickly navigate in the document, for example, by clicking the corresponding headlines.

<img style="display:block; " src="images/Context03.jpg"/>

Example of a document structure tree, which helps users quickly navigate to the corresponding sections in the editor. In the example below the structure tree displays the level 1 and 2 headings found in a Microsoft Word document.

See Also
--------
[Implementing the File Parser](implementing_the_file_parser.md)

[Adding Context Information](adding_context_information.md)