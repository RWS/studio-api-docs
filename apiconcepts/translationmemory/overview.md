Introduction
===
Welcome to the Translation Memory API. This API provides access to all translation memory functionality, both for file- and server-based translation memories. In addition it also provides the capability to create custom translation providers, which allow applications like Trados Studio 2017 to interact with automated translation engines, third-party translation memory implementations or other translation engines in a unified way.

Translation Memories
----
The majority of the functionality in this API centers around working with translation memories such as:

* Creating server and file-based TMs
* Configuring a TM
* Maintaining and tuning a TM
* Performing TM lookup operations
* Adding and editing translation units
* Retrieving information on translation units
* Creating language resources
* Performing TM imports and exports
* Connecting to a TM server
* Retrieving and creating container databases on a TM server
  
Translation Provider Plug-ins
----
Apart from translation memories, Trados Studio 2017 can also leverage other translation providers such as Web-based translation services (e.g. Google Translate), and any other kind of resource that provides translation solutions. This SDK contains an example of how to develop a simple translation provider plug-in that performs lookup operations in delimited text files.