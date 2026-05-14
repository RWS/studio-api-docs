# Studio plug-in overview

> [!NOTE]
> Var:ProductName 2026 Release will also support a new plugin system.
> Check back for details on the new plugin system when it becomes available.

This section describes how to build and deploy third-party plug-ins for Var:ProductName.
For more information on building specific types of plug-ins, refer to the relevant SDK documentation.

## Var:ProductName Plug-in Support

The core Var:ProductName application uses a modular architecture consisting entirely of plug-ins.
These plug-ins are known as *system plug-ins*.
You cannot add system plug-ins to an existing Var:ProductName installation, and end-users cannot disable them.

In addition to system plug-ins, Var:ProductName supports various types of *third-party plug-ins*.
Third-party developers can build these plug-ins using the **Var:ProductName SDK** and deploy them into an existing Var:ProductName installation.
This section focuses on third-party plug-in development and deployment.

Since Var:ProductName is a Microsoft .NET application, develop third-party plug-ins using **Microsoft .NET Framework 4.8**.
The **Var:ProductName SDK** includes Visual Studio project templates that provide a quick start for creating various types of plug-ins.
For more information, see [Building a plugin](building_a_plugin.md).

Once you build a third-party plug-in, you can easily deploy it for use with Var:ProductName.
See [Plug-in deployment](plugin_deployment.md) for details on the deployment process.

While the standard project templates included in the **Var:ProductName SDK** allow you to create a basic plug-in package for distribution to end-users, you might need to add additional assemblies or other files to the plug-in package.
To do so, edit the `pluginpackage.manifest.xml` file that resides in the Visual Studio project folder.

For more information on specific types of plug-ins, explore [our developer hub](https://developers.rws.com/).