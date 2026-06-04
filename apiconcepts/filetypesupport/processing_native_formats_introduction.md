# Introduction

This guide walks you through the creation of a custom file type plug-in that processes simple text files.

## Implement a file type plug-in for simple text files

Suppose you need to create a file type plug-in for simple monolingual text files such as the following:

```text
[Version=0]
[Element=text1]
Automatically re-open previously edited documents. 
[Element=text2]
Prompt me to re-open previously edited documents. Opens a dialog box on start-up.
[Element=text3]
Do <b>not</b> automatically re-open previously edited documents. This is the default option.
[Element=text4]
Prd-Code NCC1504
```

## Localization requirements

This text format has the following localization requirements:

- Protect lines enclosed in brackets (`[]`) from editing and translation.
- Localize lines that are not enclosed in brackets.
- Recognize common HTML tags such as `<b>` as inline elements and apply bold formatting in the Var:ProductName editor.
- Assume that this file type uses the `.text` file name extension.
- Lock specific strings, such as `Prd-Code NCC1504`, so translators cannot change them.

You could also process this format with the standard regular expressions text file type plug-in that Var:ProductName includes out of the box. This exercise instead shows how to implement a simple file type plug-in for a highly simplified text format. That approach lets you focus on the minimum native file processing logic required.

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.

