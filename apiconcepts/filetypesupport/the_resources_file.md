The Resources File
==

The project template includes a **PluginResources.resx** resources file, which stores strings and plug-in UI elements (e.g. the plug-in name, the message texts for any problems that the plug-in reports) that are displayed in the user interface of <Var:ProductName>.

By default, this resources file only includes the **Plugin_Name** string. In our implementation we need a number of other strings, e.g. to set the plug-in description and the error message(s) that the plug-in should display after verification. The resources table should therefore look as shown below:

![resources_identical_check](images/resources_identical_check.jpg)

The ```Sdl.Sdk.FileTypeSupport.Samples.SimpleText``` sample project folder also contains an icon (**icon.ico**) file, which you can add to the resource file of your project. This is the icon that will be displayed next to the plug-in name in the **Options** dialog box.

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
