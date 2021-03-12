Connecting to TM Server
=====
This section describes how to connect to TM server.

Connecting to TM Server
----
The central class that allows connecting to TM server is TranslationProviderServer. The constructor of TranslationProviderServer accepts two parameters:

* **Uri**: the Uri representing the server. This should always be of the form http://SERVERNAME:PORT, where SERVERNAME is the IP address or DNS name of the web server that serves as the access point for the TM Server. The initial communication is always done via HTTP, but the system automatically detects the most optimal protocol and uses a TCP connection if that is possible in that particular scenario.
* **Credentials**: one of the supported types of credentials, representing a valid user defined on the TM Server.
The TranslationProviderServer class provides a number of methods to get the following objects by id (Guid) or name or to get a list of all available objects (visible to the current user) of that type:

* DatabaseServer
* TranslationMemoryContainer
* ServerBasedTranslationMemory
* ServerBasedFieldsTemplate
* ServerBasedLanguageResourcesTemplate


Each method that retrieves one of these objects accepts an additionalProperties parameter. This parameter allows the caller to specify how much data to retrieve about the object or objects that are being retrieved.Any data that is not initially retrieved, will be retrieved on demand through lazily initialized properties, but it is generally advisable to retrieve all the necessary data upfront to reduce the number of roundtrips to the server if it is known upfront that all that data will be required. For example the GetDatabaseServers method accepts a parameter of type DatabaseServerProperties which allows the caller to specify whether to just retrieve the data for the database servers themselves, or to also return all the containers contained in the every database server and optionally all the translation memories in every container.

Another general feature that reduces the chattiness of the API is saving changes. Every of the top-level objects has a Save method. When setting properties on the object, these do not result in the property change being communicated to the server immediately. Only when the called calls Save explicitly are all the changes sent back to the server. This allows making a number of changes in one go, without every single change resulting in a roundtrip to the server.


<img style="display:block; " src="images/TranslationProviderServer.png"/>
