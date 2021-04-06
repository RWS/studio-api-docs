The Plug-in Manifest Generator
======
The section describes how plug-in projects are built and how the plug-in manifest is generated.

Plug-in Manifest Generator
----
When creating a plug-in project using the SDL Plug-in project template, the project file will contain the following piece of custom content:

```xml
<Import Project="$(MSBuildBinPath)\Sdl.Core.PluginFramework.Build.targets" />
<Target Name="AfterBuild">
  <CreatePluginManifestTask ProjectDirectory="$(ProjectDir)" AssemblyFilePath="$(TargetPath)" PluginResxFilePath="$(ProjectDir)PluginResources.resx" />
</Target>
```

This piece of content tells MSBuild to run an additional build task after the assembly has successfully been built: the `CreatePluginManifestTask`.

This task will use .Net reflection to enumerate all the types in the assembly and identify types that are annotated with an attribute deriving from [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml), i.e. the extensions. It will write the type name of the extensions accompanied by the serialized extension attribute to the plug-in manifest file, which is created in a "plugins" subfolder of the project output folder and is named `ASSEMBLYNAME.plugin.xml`.

On top of creating the plug-in manifest, the task will also create .Net resource files for resources defined in the `PluginResources.resx` file. This resource file contains all strings and images referred to by extension attribute declarations in the plug-in assembly. When multiple language version of the `PluginResources.resx` file are present, it the task will also create resource files for those, all in the "plugins" folder.

The `CreatePluginManifestTask` also performs validation of the extension attribute declarations, checking for instance whether a value has been specified for all required properties and whether the all extensions implement the interface imposed by the extension point they are targeting. See also [Compile-time Validation](compile_time_validation.md).

Currently the `Sdl.Core.PluginFramework.Build` assembly, containing the `CreatePluginManifestTask` is deployed in the GAC.


> [!NOTE]
> 
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.