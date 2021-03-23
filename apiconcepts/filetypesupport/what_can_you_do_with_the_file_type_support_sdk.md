What can you do with the File Type Support SDK?
====
Below you will find a number of examples of what you can do with the File Type Support Framework SDK. Note that several example projects are available with this documentation.

* Let us assume that you need to process a document format that is not supported out-of-the box by Trados Studio. This SDK includes a sample project that demonstrates a filter for localizing custom text files.
* Suppose that the target segments must not exceed a user-configurable length limit or must not be below a specific length limit. To make sure this requirement is fulfilled in all target documents, you can develop a custom plug-in that applies to all document formats.
* Suppose you need to process an XML format in which the tags contain information on the specific length limit. With this SDK, you can create a custom file type and enhance it with an extension that checks whether the length limit has been observed.
* Imagine that all headings of an HTML document must not be translated, but rather stay in the original language. With this SDK, you can develop an extension that allows you to check whether headings that are not supposed to be translated have been accidentally changed by the user.


This SDK contains numerous examples of how to develop plug-ins to meet specific localization requirements. Plug-ins can relieve you from time-consuming, tedious and ultimately error-prone manual tasks, such as counting characters or copy/pasting multiple segments.