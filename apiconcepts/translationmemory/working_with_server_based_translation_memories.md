Working with Server-based Translation Memories
=====
This section describes how to work with server-based translation memories.

Overview
------
A server-based translation memory is a translation memory (ITranslationMemory) that is hosted on TM Server. It is designed for multiple-user access and supports multiple language directions. A server-based translation memory is stored in a translation memory container (TranslationMemoryContainer), which is a Microsoft SQL Server database.

Server-based translation memories are represented by the ServerBasedTranslationMemory class. To create a server-based translation memory, create a new ServerBasedTranslationMemory object, passing the TranslationProviderServer on which the translation memory should be created into the constructor. Subsequently set all the required properties such as Name and Container and add language directions to the LanguageDirections collection. Optionally add custom field definitions and language resources and finally call Save to create the translation memory on the server.

In addition to the standard translation memory functionality, server-based translation memories have the following additional capabilities:

* **Field Templates**: instead of defining field definitions for every translation memory individually, server-based translation memories can inherit their field defintions from a fields template, represented by ServerBasedFieldsTemplate. For more information, see Working with Field Templates.
* **Language Resource Templates**: instead of defining language resources for every translation memory individually, server-based translation memories can inherit their language resources from a language resources template, represented by ServerBasedLanguageResourcesTemplate. For more information, see Working with Language Resource Templates.
* **Scheduled import and export**: instead of importing remotely over the network or internet, you can schedule an import or export to be done on the server. For more information, see Performing a Scheduled Import or Export.




<img style="display:block; " src="images/ServerBasedTranslationMemory.png"/>