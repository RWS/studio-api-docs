Working with File-based Translation Memories
=====
This section describes how to work with file-based translation memories.

File-based Translation Memories
-----
A file-based translation memory is a translation memory ([ITranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.ITranslationMemory.yml)) that is stored in a file with the ".sdltm" extension. It is designed for single-user access and supports only one language direction, which is defined when the translation memory is created and cannot be changed afterwards.

File-based translation memories are represented by the [FileBasedTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.FileBasedTranslationMemory.yml) class, which in turn inherits from [AbstractLocalTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.AbstractLocalTranslationMemory.yml), the base class for file-based and in-memory translation memories ([InMemoryTranslationMemory](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.InMemoryTranslationMemory.yml)).

<img style="display:block; " src="images/Cd-FileBasedTranslationMemory.png"/>

Password Protection
-----
File-based translation memories support a password protection mechanism, which protects certain functionality using a password that is securely stored in the translation memory. In order to set a password for a certain access level, all the passwords for higher access levels need to be set first. If you decide to password protect a translation memory, you have to define passwords for all of the available access levels and everyone who tries to open the translation memory will be asked to provide a password (including the administrator). The following access levels can be protected by a password:

* **Administrator password**: An administrator has complete access rights over the translation memory and its contents.
* **Maintenance password**: A maintenance user can open the translation memory and view, edit and delete translation units and they can use the batch edit and delete tools to maintain translation units. However, they cannot perform imports and exports.
* **Translator password**: A translator can open the translation memory in the Editor view and edit and delete individual translation units. They can also add new translation units to the translation memory.
* **Guest password**: A user with guest access can open the translation memory and work with the translation memory contents. They cannot edit or delete translation units but they can add new units to the translation memory.
In order to access a password-protected translation memory, call the Unlock method and pass the password that gives you the required access level.

See Also
-----------
[Creating a File-based Translation Memory](creating_a_file_based_translation_memory.md)

[Working with Field Definitions](working_with_field_definitions.md)

[Working with Language Resources](working_with_language_resources.md)

[Importing Content into a Translation Memory](importing_content_into_a_translation_memory.md)

[Exporting Content from a Translation Memory](exporting_content_from_a_translation_memory.md)

[Performing Translation Memory Lookups](performing_filebased_tm_lookups.md)

[Working with Server-based Translation Memories](working_with_server_based_translation_memories.md)
