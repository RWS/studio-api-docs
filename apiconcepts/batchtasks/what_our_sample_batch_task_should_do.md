What our sample batch task should do
============================

Learn how to develop a batch task by following the creation of a simplified sample project which uses the Batch Tasks API.

The example batch task plug-in exports all segments with a particular status (for example, Translated, Draft or Approved) from one or several SDLXliff files into simple text files (one TXT file per SDLXliff document). Our sample project will contain simplified code examples that show you how to implement:
* An integration of the batch task into the Var:ProductName UI with plug-in name and description
* A property page that allows you to configure the batch process, in this case the segment status that should be applied
* Basic reporting functionality, i.e. output the number of segments whose status was changed
* Simple processing of segment content in SDLXliff files
