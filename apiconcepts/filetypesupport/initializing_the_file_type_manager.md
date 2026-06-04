# Initializing the File Type Manager

Learn how applications hosting the file type support framework can initialize the File Type Manager.

## Overview

The [IFileTypeManager](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeManager.yml) is the main integration point between an application and the file type support framework. The file type manager is typically initialized to recognize all file types installed on the system. This section discusses different approaches to initialization.

## Initializing the Filter Manager

The File Type Support Framework is used in an application through the File Type Manager. There are different ways for an application to host the File Type Manager. The most straightforward way is to create and initialize it explicitly with a list of File Type Component Builders and related settings.

> [!Note]
> You need a reference to `Sdl.FileTypeSupport.Framework.Core.Utilities` in your project. The example below shows how to do this:

# [C#](#tab/tabid-1)
```cs
IFileTypeManager filterManager = DefaultFileTypeManager.CreateInstance();
filterManager.AddFileTypeDefinition(new SimpleTextFilterComponentBuilder());
filterManager.AddFileTypeDefinition(new SdlXliffFilterComponentBuilder());
// ...additional filter manager initialization goes here...
UseFilterManagerForSomething(filterManager);
```

The first line creates a `PocoFilterManager` (an implementation of [IFileTypeManager](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeManager.yml)) that provides functionality to add new filter definitions. It uses the [IFileTypeComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeComponentBuilder.yml) to build File Type Definitions. The second line adds an Open Office Text filter definition using the `OdtFilterDefinitionComponentBuilder` (see [the filter component builder](the_filter_component_builder.md) example).

This approach is straightforward, but it has several implications worth noting:

- Your application must explicitly create and manage all File Type Component Builder classes
- Your application must manage their order or priority, which affects how file types are discovered and applied
- Your application is responsible for managing access to the File Type Manager if it is shared across the application (which is preferable to creating a new instance every time, as initialization is resource-intensive)
- It is not easy to modify the [FileTypeInformation](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.IFileTypeInformation.yml) inside a FilterDefinition. The FilterFramework provides a mechanism to simplify this (see XML override).

> [!NOTE]
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.