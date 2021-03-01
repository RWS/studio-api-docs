Setting the Source and Target Language
====
In this chapter you will learn how to retrieve and set the terminology provider source and target language.

How to read the source and target language from the glossary file header
-----
Open the **MyTerminologyProvider.cs** class and go to the **GetLanguages()** function. Our implementation is based on the assumption that the first line in the text file contains the source and target language name and locale in the following form: *1;English,en-US;German,de-DE*

The source and target language are separated with a semicolon, the language name and locale are comma-separated.

Modify the **GetLanguages()** function as shown below. In this function, we parse the first line of the text file to retrieve the language label (for example 'English') and the locale (for example 'en-US'). Based on the locale, Trados Studio assigns the glossary languages to the corresponding project language. After parsing the first line, the method creates two language objects which are added to the results list that the method returns.

Note that you can add more than two languages. If Trados Studio cannot assign the glossary languages automatically to the project languages, then the user has to pick the correct glossary language manually from the dropdown list.

<img style="display:block; " src="images/project_01_selected_languages.jpg" />