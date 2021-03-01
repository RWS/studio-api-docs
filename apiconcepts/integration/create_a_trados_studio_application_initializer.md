Create a Studio application initializer
====

Trados Studio Integration API provides support for the third party developers to add specific initializations or functionalities at Studio application startup.

Creating a Studio application initializer
----
In order to create a Studio application initializer a third party developer will require the following steps:

* Create an initialization class which implements the IApplicationInitializer interface.
* Decorate the initialization class with the ApplicationInitializerAttribute attribute


Creating a Studio application initializer Sample