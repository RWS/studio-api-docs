Instantiating the Plug-in
====

A translation provider plug-in has to be properly instantiated, so that it can be used in and displayed in SDL Trados Studio 2017. Instantiation is done by a component that is also provided in the plug-in template, and which is called **MyTranslationProviderFactory.** In our implementation we will rename this component to **ListTranslationProviderFactory**.

The Translation Provider Factory Interface
------
The factory component implements the interface ITranslationProviderFactory. This interface, in turn, consists of three methods. The following sections outline how we implement these methods for our sample plug-in.

The class is preceded by the following declaration, which defines it as an extension class, which will be referenced and declared to the SDL Trados Studio 2017 application through the plug-in manifest *.xml file (see also Building the Plug-in).

Create the Translation Provider
------
The CreateTranslationProvider: method is used to create the translation provider, which is specified in the translation provider URI. In our implementation we use the string *listprovider*. As this value is used for identification, it needs to be unique. The URI is also used to store any plug-in settings information, which are loaded through the below method. In our implementation this would be the delimiting character and the delimited text file name and path.

The plug-in URI is stored in an *.sdlproj or an *.sdltlp file. These are the file types that contain, among other things, the translation providers used for a translation project and the corresponding settings.
Below you see an example of what a plug-in URI looks like for this sample implementation. The URI is prefixed with *listprovider///* and followed by the plug-in settings parameters, i.e. *delimiter* and *listfile*.

Determine Whether the URI is Supported by the Plug-in
------
SupportsTranslationProviderUri method you should determine whether the provided URI is actually valid and supported by the plug-in as shown in the sample code below:

Setting the Plug-in Info
-------

This component is also responsible for setting the plug-in GUI elements, i.e. the name and the plug-in icon. Through the GetTranslationProviderInfo: method you set the plug-in name, which should be shown in the user interface of SDL Trados Studio 2017 as well as the translation method.

First, create a TranslationProviderInfo object to hold and set the plug-in properties. The string fro the (nice) plug-in name is accessed from the resources file:

The TranslationMethod is retrieved from a separate ListTranslationOptions class, which we will implement in a later step (see Storing and Retrieving the Plug-in Settings). The translation method is used to specify what kind of translations a given plug-in provides, e.g. translation solutions from a translation memory, a machine translation system, etc.


After the specifying these plug-in properties, the info object is returned as shown below:

Putting it All Together
------
The complete factory class should look as shown below: