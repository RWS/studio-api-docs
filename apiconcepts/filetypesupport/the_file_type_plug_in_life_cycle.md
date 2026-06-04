# The File Type Plug-in lifecycle

This page describes the lifecycle of File Type Support Framework components, with a focus on initialization.

During its lifecycle, a filter component can go through the following steps:

1. **Instantiate the component.**  
   `PocoFilterManager` explicitly creates and initializes the components. For more information, see [Initializing the File Type Manager](initializing_the_file_type_manager.md).

2. **Set related components.**  
   If the component implements `IComponentsAware`, the framework calls `SetComponents()` before the component is used. The framework also calls this method whenever the component set changes. This lets the component inspect related components and, for example, decide whether it should synchronize settings with them.

3. **Set native component services.**  
   If the component implements `INativeFilterComponent`, the framework sets the `PropertiesFactory` and `MessageReporter` properties. Components should create property objects only through the property factory. This ensures that cooperating filter components use the same property object types.

   The File Type Support Framework normally provides a default factory. However, a parser can provide the factory that all components should use. If a native parser exposes an existing properties factory through `INativeFilterComponent` during initialization, the framework uses that factory.

4. **Set bilingual item services.**  
   If the component implements `IBilingualFilterComponent`, the framework sets the `ItemsFactory` property. Components should create bilingual content model items only through this factory.

   The File Type Support Framework normally provides a default factory. However, a parser can provide the factory that all components should use. If a bilingual parser implements `IBilingualFilterComponent` and returns a non-null value from `ItemsFactory`, the framework uses that factory.

5. **Set file properties.**  
   If the component implements `INativeContentCycleAware`, the framework calls `SetFileProperties()`. The file conversion properties, which are part of the file properties, include the name of the native file being processed in `OriginalFilePath`. Native file parsers must implement this interface and read the file name from that property.

   Filter components can change the file conversion properties at any time. Because all components reference the same object, any change becomes visible to the other components immediately.

   Bilingual processor components should not usually implement this interface. They receive the corresponding information, and more, through the `SetFileProperties()` call on `IBilingualContentProcessor`.

   Some components may receive this method call several times during their lifetime because an XLIFF document can contain content from multiple native files. The framework invokes the method before it processes content from each native file.

   The framework serializes the file conversion properties into the bilingual document format, then deserializes and reapplies them to filter components during initialization.

   Filter components can also store dynamic settings and other information in this object. They rely on that information being available the next time the same content is processed. Store settings as key/value pairs. To avoid conflicts, each filter component should use a unique prefix for its setting keys. Built-in framework components use the `SDL:` prefix.

6. **Set output properties.**  
   If a native component implements `INativeOutputSettingsAware`, the framework calls `SetOutputProperties()`. This lets the component determine the conversion purpose, whether it will process source or target content, the output file path, the preferred output encoding, and related settings.

7. **Start content processing.**  
   If a native filter component implements `INativeContentCycleAware`, the framework calls `StartOfInput()` immediately before parsing begins. Bilingual processors receive `Initialize()` and `SetFileProperties()` on `IBilingualContentProcessor` before content passes through them.

8. **Process the content.**  
   The components now process the actual content.

9. **Complete content processing.**  
   If a native filter component implements `INativeContentCycleAware`, the framework calls `EndOfInput()` immediately after parsing completes. Bilingual processor components receive `FileComplete()` and `Complete()` as appropriate.


>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
