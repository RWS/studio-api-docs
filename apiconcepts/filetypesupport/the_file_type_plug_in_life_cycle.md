The File Type Plug-in Lifecycle
==

This section describes the lifecycle and in particular the initialization process for components in the  File Type Support Framework.

During its lifecycle a filter component may go through the following steps:

1. The component is instantiated.
The components are explicitly created and initialized using PocoFilterManger (see [Initializing the File Type Manager](initializing_the_file_type_manager.md)).

2. If the component implements the `IComponentsAware` interface, the `SetComponents()` method will be called before the component is used. Every time the set of components change, this component will be included in the changes.
This allows the component to retrieve information on other components with which it is being used. This may be useful e.g. to determine if it should synchronize its settings with any other known components.

3. If the component implements the `INativeFilterComponent` interface the `PropertiesFactory` and `MessageReporter` properties will be set. Components should only create property objects through the property factory. This ensures that all filter components that are used together use the same type of property objects.

    Normally the  File Type Support Framework provides a default factory. However, it is possible for the parser to set the factory that should be used by all the components. If a native parser exposes an existing properties factory through the `INativeFilterComponent` interface at initialization time, that factory will be used.

4. If the component implements the `IBilingualFilterComponent` interface, the `ItemsFactory` property will be set. Components should only create bilingual content model items through this factory.

    Normally the  File Type Support Framework provides a default factory. However, it is possible for the parser to set the factory that should be used by all the components. If a bilingual parser implements the `IBilingualFilterComponent` and returns a non-null value from the `ItemsFactory` property, it is that factory that will be used.

5. If the component implements the `INativeContentCycleAware` interface, the `SetFileProperties()` method will be called. The file conversion properties (a member of the file properties), among other things, contain the name of the native file that is being processed (in the OriginalFilePath property). Native file parsers must implement this interface and obtain the name of the file to parse from this property.

    The file conversion properties can be changed by the filter components at any time. Since the same object is referenced by all components, changes to the object immediately affect all other components.

    Bilingual processor components normally should not implement this interface. They will obtain the corresponding information (and more) through the `SetFileProperties()` call on the `IBilingualContentProcessor` interface.
    For some components this method may be called several times during their lifetime, due to the fact that an XLIFF document may contain content from multiple native files. The method will be invoked before content originating from each native file is processed.

    The file conversion properties will be serialized into the bilingual document format and de-serialized and applied applied to filter components as part of the initialization.

    Filter components can store dynamically configured settings and any other type of information in this object. They rely on this information being available the next time the same content is processed. Settings are stored and retrieved as key/value pairs. To avoid conflicts with other components each filter component should use a unique prefix for their settings keys. Built-in framework components use the "SDL:" prefix for all their settings keys.

6. If a native component implements the `INativeOutputSettingsAware` interface, the `SetOutputProperties()` method is invoked.
This allows components to find out about the purpose of the conversion, whether it will be processing source or target content, what the file path to the output file is, the preferred encoding for the output file, etc.

7. If a native filter component implements the INativeContentCycleAware interface, the `StartOfInput()` method will be called immediately before parsing begins.
Bilingual processors receive calls to `Initialize()` and `SetFileProperties`() on the IBilingualContentProcessor interface before content is passed through them.

8. The components will now be used to process actual content.
9. If a native filter component implements the INativeContentCycleAware interface, the `EndOfInput()` method will be called immediately after the parsing has completed.
Bilingual processor components receive calls to `FileComplete()` and `Complete()` as appropriate.


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
