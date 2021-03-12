Introduction
=====
This documentation provides practical examples of how to use the Project Automation API, which allows programmatic access to the project automation functionality of Trados Studio 2017. Apart from various code snippets that demonstrate common use cases you will also find a fully-documented command-line sample application that showcases how to automate project creation and batch task functionality.

What you can do with this SDK
----
The Project Automation API is concerned with the automation of common project management tasks such as file analysis, pre-translation, generation of finalized target documents, etc. Based on this API you could potentially build fully-fledged, customized workflow systems. Below you find examples of applications for the API:

* Developing a Comand-line application that traverses a folder / sub-folder structure to analyze or pre-translate the translatable documents contained therein.
* Develop an application that creates custom analysis reports for importing into an accounting system.
* Develop an application for fully automating the project creation process, e.g. create multiple projects according to a template with one mouse-click.
* Schedule automated project package creation and assigning manual tasks to a number of users. The application can then upload the packages to an FTP server or forward them via e-mail.
* An application that is timed to automatically loop through a folder into which translators/editors have delivered their return packages. The application then imports the return packages, thereby updating the corresponding projects, generates an updated project statistics report, which is then forwarded to the responsible project manager.
* An application that loops through a number of translated bilingual (SDL XLIFF) documents and that batch-updates the corresponding master translation memories on a regular basis.
* An application for testing whether a set of native documents can be converted to a translatable (SDL XLIFF) format prior to creating a project. The application loops through a folder structure, identifies any translatable documents and converts them SDL XLIFF. Upon failure, a report is generated that states the file and (if applicable) the reason why the conversion failed (e.g. a Microsoft Word document has the track changes functionality switched on)
* An application that loops through multiple projects and generates the finalized target files, which are then automatically forwarded to the end customer via e-mail or uploaded to an FTP share. In case of any failure to generate the target documents, an automated mail can be send to the project manager.


Note that the Project Automation API might sometimes have to be used in conjunction with the Translation Memory API to cover specific use-cases, e.g. when certain information such as the number of translation units, the TM languages, etc. needs to be read from a TM. This SDK contains an example of how to retrieve the languages of a specified TM using the Translation Memory API and then create a project based on the TM language direction.