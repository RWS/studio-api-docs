Tag display modes
=====
Tags that occur inside segments are called inline tags. These tags can be placeholders that represent elements such as footnote references, index markers, etc. Tags can also be used to set the character formatting of a particular string, e.g. bold, underline, etc. In this case a tag par (i.e. an opening and a closing tag) are used to enclose the string that has bold, underline, etc. formatting.

 <Var:ProductName> offers a 'clutter-free' editing environment. This means that as a general rule, users should only see a minimum amount of inline tags. You can design a file type plug-in in such a way that inline tags are not shown by default. For example, you may decide that the inline tags that define bold character formatting in your particular document format should not be shown by default. If possible, your file type plug-in should be designed to show the actual display formatting, which is more user-friendly that displaying the tags. If required, users can still choose to show the (hidden) inline tags at runtime e.g. by clicking a toolbar button.

As a general rule, tags that define character formatting should not be displayed, while tags that act as placeholders for e.g. hyperlinks, footnote references, etc. should be shown to the user (i.e. not set to hidden).

When inline tags are shown, they have four possible display states that the user can choose from:

* No tag text: only a placeholder symbol is displayed
* Partial tag text (default): the name of the tag is shown, e.g. *footnotereference*, which helps the user ascertain what the tag actually represents
* Full tag text: shows the name of the tag and any attributes, e.g. *<footnotereference font='Arial'>*
* Tag id: in an intermediary (SDLXliff) file each tag has a unique id, starting with 1. If the user chooses this option, the tag ids are displayed (i.e. from 1 to n).
Example of an inline tag, which is a placeholder for a footnote reference. The tags that define character formatting are hidden, only the actual formatting is shown. The **footnotereference** tag is shown in the (default) partial tag text display mode.

<img style="display:block; " src="images/Tag01.jpg"/>

This is what the inline tag looks like when shown with no tag text:

<img style="display:block; " src="images/Tag02.jpg"/>


This is what the inline tag looks like when the full tag text (i.e. including attributes) is shown:

<img style="display:block; " src="images/Tag03.jpg"/>

The tag id (*ph* stands for placeholder):

<img style="display:block; " src="images/Tag04.jpg"/>


The same segment with character formatting tag pairs displayed:

<img style="display:block; " src="images/Tag05.jpg"/>

See Also
-----------
[Processing Inline Formatting](processing_inline_formatting.md)

[Processing Inline Tags](processing_inline_tags.md)

[Applying Character Formatting](applying_character_formatting.md)

[Processing Placeholder Tags](processing_placeholder_tags.md)

[Handling Tags During Segmentation](handling_tags_during_segmentation.md)