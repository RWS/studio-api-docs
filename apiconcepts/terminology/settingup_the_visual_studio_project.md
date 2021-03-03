Setting up the Visual Studio project
====================================
To start setting up your batch task plug-in project, you need to generate a plug-in that can compile and that implements an empty batch task which can be seen and selected in Trados Studio. For the moment, it will not contain any application logic, that is it will not actually perform a real task.

How to create the Visual Studio Project
----------------------------------
Assuming that you already installed the Trados Studio SDK, open Microsoft Visual Studio. You will see the following options when you create a new project:
<img style="display:block; " src="images/CustomBatchTemplate.jpg" />
With the above templates you can set up the skeleton of an Trados Studio plug-in project. Select **SDL Terminology Provider Task Plug-in 2015**.

The Plug-in Skeleton
-------------------------------------
The plug-in template will add the required references to your project:
<img style="display:block; " src="images/References.jpg" />
It will also add the following skeleton classes to your project:
<img style="display:block; " src="images/Stubs.jpg" />

The Plug-in Declaration: ID, Name, Description</title>
--------------------------------
Open the **MyTerminologyProviderFactory.cs** class. This class contains the plug-in declaration - the plug-in name and description that will be visible in Trados Studio:
# [Plug-in Declaration](#tab/tabid-1)
[!code-csharp[MyTerminologyProviderFactory](code_samples/MyTerminologyProviderFactory.cs#L10-L15)]
***
Give the terminology provider plug-in a new name, ID and description. Instead of doing it directly inside this class, enter the strings into the **PluginResources.resx** file:

<img style="display:block; " src="images/Resource.jpg" />

Make sure the resource file access modifier is set to public and that you treat it as an *embedded* resource.

Open the **MyTerminologyProviderWinFormsUI.cs** class. This class controls how the plug-in manifests in the SDL Trados Studio UI. Change the **TypeName** and **TypeDescription** members as shown below.


How to build and load the plugin in SDL Studio
---------------------------------------------
Build the assembly. The project is automatically configured to build the plug-in file into the *%AppData%\Roaming\SDL\SDL Trados Studio\12\Plugins\Packages\* folder. After you have built the plug-in you should find the SDL Terminology Provider Plug-in 2015.sdlplugin file in the folder. Start Trados Studio. As the plug-in is not yet officially signed by SDL, you will see the following message when starting the application:
<img style="display:block; " src="images/Plugin_NotSigned.jpg" />
For the moment, ignore this message and click **Yes** to make sure that Trados Studio extracts the plug-in file. Once Studio is started, you should find the sub-folder *SDL Terminology Provider Plug-in 2015* under <em>%AppData%\Roaming\SDL\SDL Trados Studio\12\Plugins\Unpacked\ </em>. This sub-folder contains the unpacked plug-in assemblies.

> [!NOTE]
> To officially sign the plugin, send the sdlplugin file to app-signing@sdl.com.

Open Trados Studio and go to **File -> Options -> Language Pairs -> All Language Pairs -> Termbases** as illustrated below. When you click Add, the name of your newly-created terminology provider should be listed:

<img style="display:block; " src="images/tb_provider_name.jpg" />

You have now managed to make your terminology provider available in Trados Studio. The plug-in does not perform any function yet. In the following pages we will provide functionality to the plug-in.