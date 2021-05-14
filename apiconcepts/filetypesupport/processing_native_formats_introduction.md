Introduction
==

This guide takes you step by step through the creation of a custom file type plug-in for processing simple text files.

Implement a File Type Plug-in for Processing Simple Text Files
--

Imagine that you need to create a file type plug-in for processing simple (monolingual) text files that look as shown below:

```txt
[Version=0]
[Element=text1]
Automatically re-open previously edited documents. 
[Element=text2]
Prompt me to re-open previously edited documents. Opens a dialog box on start-up.
[Element=text3]
Do <b>not</b> automatically re-open previously edited couments. This is the default option.
[Element=text4]
Prd-Code NCC1504
```

This text format has the following localization requirements:
* The lines that are enclosed in brackets (i.e. []) must be protected from editing and thus from translation.
* The lines that are not enclosed in brackets require localization.
* Translatable text may be enclosed in common HTML tags (e.g. < b >). The file type plug-in needs to recognize tag text as inline elements and apply bold display formatting in the editor of <Var:ProductName>.
* Let us assume that the file name extension for this file type is **.text*.
* Certain strings (e.g. *Prd-Code NCC1504*) have to be locked, so that they cannot be changed by the translator.
To be sure, you could also process this format using the standard regular expressions text file type plug-in, which is included in <Var:ProductName> out-of-the-box. However, for this exercise we would like to show to you how to implement a 'no-frills' file type plug-in for a highly simplified text format. This allows you to develop a file type plug-in with a minimum amount of native file processing logic (and thus source code).

>**NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.

