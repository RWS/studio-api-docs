## Introduction
This API gives you access to translation memory functionality for file-based translation memories. 
It also lets you create custom translation providers so applications like Var:ProductName can interact with automated translation engines, third-party translation memory implementations, and other translation engines through a unified interface.

> [!NOTE]
> Interaction with server based translation memories is no longer supported in this API and will be removed in the future versions.
> To include functionality please use the Groupshare API instead, which provides access to server based translation memories through the [GroupShareKit](https://github.com/RWS/groupsharekit.net).

## Translation Memories
Most functionality in this API focuses on translation memory workflows, including:

* Creating file-based TMs
* Configuring a TM
* Maintaining and tuning a TM
* Performing TM lookup operations
* Adding and editing translation units
* Retrieving information on translation units
* Creating language resources
* Performing TM imports and exports
  
## Translation Provider Plug-ins
In addition to translation memories, Var:ProductName can use other translation 
providers, such as web-based translation services (for example, Google Translate
and other resources that return translation results. 
This API includes an example that shows how to build a simple translation provider plug-in that performs lookup operations in delimited text files.
