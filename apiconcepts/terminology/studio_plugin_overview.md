Studio plug-in overview
====
This section describes how to build and deploy third-party plug-ins for Trados Studio. For more information on building specific types of plug-ins, refer to the relevant SDK documentation.

Trados Studio Plug-in Support
----
The core Trados Studio application is built in a very modular fashion, consisting entirely of plug-ins. These types of plug-ins are known as *system plug-ins*. Plug-ins of this type cannot be added to an existing Trados Studio installation and can also not be disabled by the end-user.

In addition to system plug-ins, Trados Studio supports various types of so called *third-party plug-ins*. These plug-ins can be developed by third-party developers using the Add token content here and can be deployed into an existing Trados Studio installation by the end-user. This section concentrates on third-party plug-in development and deployment.

Since Trados Studio is a Microsoft .NET application, third-party plug-ins should be developed using Add token content here. The Add token content here comes with a number of Visual Studio project templates which give you a quick start to creating various types of plug-ins. For more information on this, see Building a plug-in.

Once you've built a third-party plug-in, this plug-in can be easily deployed for use by Trados Studio. See Plug-in deployment for more information on how this works.

While the standard project templates that are included in the Add token content here allow you to create a basic plug-in package for distribution to end-user, you might need to add additional assemblies or other files to the plug-in package. For this purpose you need to edit the *pluginpackage.manifest.xml* file that is found inside the Visual Studio project folder.

For more information on specific types of plug-ins please have a look at our developer hub