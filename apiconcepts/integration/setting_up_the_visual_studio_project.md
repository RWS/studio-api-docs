Setting up the Visual Studio Project
=====
To start setting up your custom display filter project, you need to generate a plug-in that can compile and that implements an empty display filter which can be seen and selected in Trados Studio. For the moment, it will not contain any application logic, that is it will not actually perform a real task

How to create the Visual Studio Project
-----
Assuming that you already installed the Trados Studio SDK, open <var:VisualStudioEdition>. You will see the following options when you create a new project:

With the above templates you can set up the skeleton of an Trados Studio plug-in project.

Create a new project from <var:VisualStudioEdition> using the Trados Studio 2015 Plug-in Project template. Give the plugin a name, for example, AdvancedDisplayFilter.Example.

Creating the Visual Studio project using the Trados Studio 2015 Plug-in template automates the initial setup phase for your development project. It will automatically include all of the standard references to the studio assemblies that you might require for building your project, along with the plugin manifest and resource files. It will also setup the output path of your build environment to the correct location in your systems roaming directory.

Your project should look like this after creating the project using the Trados Studio 2015 Plug-in template:

**Important**:The *The Sdl.Core.Globalization* assembly is not automatically added to the project with the Trados Studio 2015 plug-in template. This assembly will be required for this project, as we will be making reference to some of the *ISegmentPair* enumerators.

To add this assembly, from the Solution Explorer, right-click on the **References** node and click **Add Reference** from the context menu. Then, navigate to the **<var:InstallationFolder>** and select the **Sdl.Core.Globalization.dll file**.

 How to sign the solution
 ----
To sign your development solution from the project properties area.

In <var:VisualStudioEdition>, go to **Project > AdvancedDisplayFilter.Example Properties.**
Go to the **Signing tab**
To sign the assembly of the project, select the **Sign the assembly checkbox** and then the **New…** option from the **Choose a strong name key file** combo box.
In the **Create Strong Name Key** dialog, provide a name, click **OK** and save the project.
Once you have saved the project properties, you will notice that a new file has been added to your project with the .snk extension. This file holds the Strong Name Key details that you provided.

> [!NOTE]
> Note: There are a few reasons why we sign the project, one of the most obvious is that it provides a level of authenticity with the assembly, ensuring that the origins of the assembly are in fact from that author; in turn, it prevents tampering with the assembly. It is also required if you want to include your assembly in the Global Assembly Cache (GAC).