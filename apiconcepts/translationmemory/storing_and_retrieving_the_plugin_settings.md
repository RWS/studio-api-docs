Storing and Retrieving the Plug-in Settings
======
The settings that are configured through the plug-in user interface (see Implementing the Plug-in User Interface) need to be programmatically set and retrieved. This chapter describes how to implement a separate class for storing the plug-in settings. These settings are physically stored in an *.sdlproj or in an *.sdltlp file.

Implement the Class for Handling the Plug-in Settings
------
Add a helper class called ListTranslationOptions to the project. This class is not included in the plug-in template.

Define the Translation Method
------
First, we define the TranslationMethod that best applies to our translation provider source. The translation method could, for example, be translation memory, machine translation, pseudo-translation, etc. A delimited list is basically like a rudimentary TM. Therefore you could, for example, select TranslationMemory. However, to set our plug-in apart from a 'normal' TM we select the option Other:

Create the Plug-in URI Builder
------
Each plug-in uses a URI to store and retrieve settings. Through the following code we create the URI builder object, which is used to store and persist the plug-in settings:

At the end we return the URI as shown below:

Set and Retrieve the Plug-in Parameters
------
Our plug-in implements two string settings, i.e. the list file name and path and the delimiter character. In the following we declare the two public string properties which are set by the plug-in user form. The properties call on two separate private functions to get and to set the corresponding values:

The following function sets the string parameter. It takes the property name and the corresponding value as string parameters:

The following function is used to retrieve the value for a specified URI property:

The following is an example of what a URI string will look like in our implementation:

The translation provider plug-in settings are physically stored in the project file (*.sdlproj) or in the project template (*.sdltpl) file. These are XML-compliant files that store project- or project-template specific information. The file excerpt below provides an example of how a translation provider is listed in a project or template file alongside with its respective settings:

Note that for every project that is created in Trados Studio 2017 as well as for each single file that is opened for translation, an *.sdlproj file will be created, which contains the translation providers used for the given project or document, the document(s) to translate, the termbase(s) used, etc.

Putting it all Together
----
The complete class should look as shown below: