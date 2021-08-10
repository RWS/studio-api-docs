<Var:ProductName> Command line processor 
====
In case you need to run <Var:ProductName> from a command line and you what to execute a custom processing based on a command with arguments the Integration API makes it possible by letting you to create your own command line processor. 

Creating a <Var:ProductName> command line argument processor
---

In order to create a <Var:ProductName> command line processor, a third-party developer will require the following steps:

* implement the [IExternalCommandLineProcessor](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.IExternalCommandLineProcessor.yml) or [IExternalWindowAwareCommandLineProcessor](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.IExternalWindowAwareCommandLineProcessor.yml) depending if you want to run the command line processor before or after the main screen is shown. 

* from `Sdl.Desktop.IntegrationApi.Extensions.CommandLine` namespace and decorate your class with [ExternalCommandLineProcessorAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.ExternalCommandLineProcessorAttribute.yml) attribute. 

If you fail to implement [IExternalCommandLineProcessor](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.IExternalCommandLineProcessor.yml) or [IExternalWindowAwareCommandLineProcessor](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.IExternalWindowAwareCommandLineProcessor.yml),  an exception will be thrown, but if you omit the [ExternalCommandLineProcessorAttribute](../../api/integration/Sdl.Desktop.IntegrationApi.Extensions.CommandLine.ExternalCommandLineProcessorAttribute.yml), your plugin will simply be ignored when <Var:ProductName> starts.

After successufly crating your command line processor you are able to see the help generated for it by and running the following command in cmd.exe : <var:InstallationFolder>\SdlTradosStudio.exe ? . Your command will appear, among other commands available, in the following screen :
<img style="display:block; " src="images/cmdHelpWindow.jpg"/>

[Creating a <Var:ProductName> Command line processor Sample]
(trados_studio_command_processor.md)