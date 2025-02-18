Setting up the Visual Studio project
====================================
To start setting up your batch task plug-in project, you need to generate a plug-in that can compile and that implements an empty batch task which can be seen and selected in Var:ProductName. For the moment, it will not contain any application logic, that is it will not actually perform a real task.

How to create the Visual Studio Project
----------------------------------
Assuming that you already installed the Visual Studio extension Var:ProductName templates, open Var:VisualStudioEdition. You will see the following options when you create a new project:
<img style="display:block; " src="images/CustomBatchTemplate.jpg" />
With the above templates you can set up the skeleton of an Var:ProductName plug-in project. Select **Custom Batch Task (2021)*

The Plug-in Skeleton
-------------------------------------
The plug-in template will add the required references to your project:
<img style="display:block; " src="images/References.jpg" />
It will also add the following skeleton classes to your project:
<img style="display:block; " src="images/Stubs.jpg" />

The Plug-in Declaration: ID, Name, Description</title>
--------------------------------
Open the **MyCustomBatchTask.cs** class. This class contains the plug-in declaration - the plug-in name and description that will be visible in Var:ProductName:
# [Plug-in Declaration](#tab/tabid-1)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L16-L20)]
***
Give the batch task plug-in a new name, ID and description. Instead of doing it directly inside this class, enter the strings into the **PluginResources.resx** file:
<img style="display:block; " src="images/Resource.jpg" />

> [!NOTE]
> You also declare what kind of files the batch task works on here. Most batch tasks are used to process bilingual SDLXliff files, not native files such as DOCX or PPTX. This also applies to our sample implementation.


In this class, you also reference the settings page that allows the user to configure the batch tasks settings via the plug-in UI:
# [Plug-in Declaration](#tab/tabid-1)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L24-L26)]
***

The Plug-in Build Folder
---------------------------------------------
Make sure that you sign your assembly. Then build the assembly. The project is automatically configured to build the plug-in file into the folder: <em> Var:PluginPackedPath </em>. After you have built the plug-in, you should find the file *Custom Batch Task1.sdlplugin*. Now start Var:ProductName. Because the plug-in is not yet officially signed by RWS, you will see the following message after you start the application:
<img style="display:block; " src="images/Plugin_NotSigned.jpg" />
For the moment, ignore this message. Click **Yes** to make sure that Var:ProductName extracts the plug-in file. Once Studio is started, you should find the sub-folder *Custom Batch Task1* under <em> Var:PluginUnpackedPath </em>. This sub-folder contains the unpacked plug-in assemblies.

In the batch tasks list of Var:ProductName, you will see the name of your newly compiled plug-in:
<img style="display:block; " src="images/SampleTaskName.jpg" />

When you select your sample batch task you will see the following window with the plug-in description:
<img style="display:block; " src="images/SampleTaskDescription.jpg" />

At this point your batch task is not actually doing anything, but as a first step you have managed to integrate your plug-in into Var:ProductName. In the following pages, we will enhance this basic plug-in with some added functionality. Close Var:ProductName and go back to your Microsoft Visual Studio project.
