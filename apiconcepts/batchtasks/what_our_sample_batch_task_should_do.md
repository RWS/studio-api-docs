# What Our Sample Batch Task Should Do

This guide demonstrates how to create a simplified batch task using the Batch Tasks API. Follow along to understand the key steps and concepts.

The example batch task plug-in exports segments with a specific status (for example, Translated, Draft, or Approved) from one or more SDLXliff files into text files (one TXT file per SDLXliff document). This sample project includes simplified code examples to demonstrate:
* Integration of the batch task into the Var:ProductName UI, including the plug-in name and description.
* A property page to configure the batch process, such as selecting the segment status to apply.
* Basic reporting functionality to output the number of segments whose status was changed.
* Simple processing of segment content in SDLXliff files.
