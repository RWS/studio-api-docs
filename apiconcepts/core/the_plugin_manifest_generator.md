---
uid: the_plugin_manifest_generator.md
---
## The Plug-in Manifest Generator
This section describes how plug-in projects are built and how the plug-in manifest is generated.

### Overview
The plug-in manifest prevents the need to load all plug-in assemblies at runtime and reflect over them to discover available extensions. This optimization significantly improves startup performance, which is critical for desktop applications.

### Implementation
The plug-in manifest generator is implemented as an MSBuild task, running as part of the standard Var:VisualStudioEdition build. This build task is included in the [Sdl.Core.PluginFramework.Build](https://www.nuget.org/packages/Sdl.Core.PluginFramework.Build/) package, which must be referenced by the project.

```xml
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <UsingTask TaskName="Sdl.Core.PluginFramework.Build.CreatePluginManifestTask" AssemblyFile="Sdl.Core.PluginFramework.BuildTasks.dll" />

  <Target Name="GeneratePluginManifestTarget" AfterTargets="AfterBuild">
    <CreatePluginManifestTask
      ProjectDirectory="$(ProjectDir)"
      AssemblyFilePath="$(TargetPath)"
      PluginResxFilePath="$(ProjectDir)PluginResources.resx"
      ReferenceCopyLocalPaths="@(ReferenceCopyLocalPaths)"
      ReferenceSatellitePaths="@(ReferenceSatellitePaths)"
      CreatePluginPackage="$(CreatePluginPackage)"
      DeployPluginPackage="$(DeployPluginPackage)"
      PluginDeploymentPath="$(PluginDeploymentPath)"
      />
  </Target>
</Project>
```

This task will use .Net reflection to enumerate all the types in the assembly and identify types that are annotated with an attribute deriving from [ExtensionAttribute](../../api/core/Sdl.Core.PluginFramework.ExtensionAttribute.yml), i.e. the extensions. It will write the type name of the extensions accompanied by the serialized extension attribute to the plug-in manifest file, which is created in a "plugins" subfolder of the project output folder and is named `ASSEMBLYNAME.plugin.xml`.

On top of creating the plug-in manifest, the task will also create .Net resource files for resources defined in the `PluginResources.resx` file. This resource file contains all strings and images referred to by extension attribute declarations in the plug-in assembly. When multiple language version of the `PluginResources.resx` file are present, it the task will also create resource files for those, all in the "plugins" folder.

The `CreatePluginManifestTask` also performs validation of the extension attribute declarations, checking for instance whether a value has been specified for all required properties and whether the all extensions implement the interface imposed by the extension point they are targeting. See also [Compile-time Validation](extensions.md#compile-time-validation).

When creating a plug-in project using the project template from the SDK, the project file will contain the following pieces of custom content:

*  `CreatePluginPackage` - if set to true will generate the `*.sdlplugin`
*  `PluginDeploymentPath` - path to the root folder where the `Packages` and `Unpacked` folders are present, default path is set to *Var:PluginPackedPath*
