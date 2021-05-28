
Initializing the File Type Manager
====

This section contains information about how applications hosting the file type support framework can handle initialization of the File Type Manager.

The [IFileTypeManager](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeManager.yml) is the main integration point between an application and the file type support framework. The file type manager is typically initialized so that it knows about all file type installed on the system. This section discussed different approaches to that initialization.

Initializing the Filter Manager
---
The File Type Support Framework is used in an application through the File Type Manager.

There are different ways for an application to host the File Type Manager. The most straightforward way is to simply create and initialize it expicitly with a list of File Type Component Builders and related settings. 

> [!Note]
>
> You need a reference to **Sdl.FileTypeSupport.Framework.Core.Utilities** in your project. The example below shows how to do this:

# [C#](#tab/tabid-1)
```cs
IFileTypeManager filterManager = DefaultFileTypeManager.CreateInstance();
filterManager.AddFileTypeDefinition(new SimpleTextFilterComponentBuilder());
filterManager.AddFileTypeDefinition(new SdlXliffFilterComponentBuilder());
// ...additional filter manager initialization goes here...
UseFilterManagerForSomething(filterManager);
```
***

The example defines in the first line a `PocoFilterManager` (an implementation of [IFileTypeManager](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeManager.yml)) that provides functionality to add new filter definitions. It relies on the [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) to build File Type Definitions. In the second line an Open Office Text filter definition is adding using the `OdtFilterDefinitionComponentBuilder` (See [the filter component builder](the_filter_component_builder.md) example) This approach is very straightforward, but it has several implications that are worth noting:

* The application have to explicitly create and manage all the File Type Component Builder classes.
*   The application needs to manage their order or priority. This has implications on how the file types are discovered and applied.
* The application is responsible for managing access to the File Type Manager, if it is shared across the application (which is probably preferrable to creating a new one every time, as the initialization could be resource-intensive)
* It is not easy to modify the [FileTypeInformation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeInformation.yml) inside a FilterDefinition. The FilterFramework provides a mechanism to simplify this (see Xml override).