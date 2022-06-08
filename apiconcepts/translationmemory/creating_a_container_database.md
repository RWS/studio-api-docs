Creating a Container Database
====
In this chapter you will learn how to programmatically create a container on a database server for storing translation memories.

Add a New Class
---
Start by adding a new class called `ServerContainerManager`. Then implement a public Create function, which takes a [TranslationProviderServer](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml) object as parameter. The function should look as shown below:

# [C#](#tab/tabid-1)
```cs
public void Create(TranslationProviderServer tmServer)
{
    var container = new TranslationMemoryContainer(tmServer);

    DatabaseServer dbServ = tmServer.GetDatabaseServer("DB01", this.GetDbServerProperties());
    container.DatabaseServer = dbServ;
    container.DatabaseName = "DbName";
    container.Name = "FriendlyName";
    container.Save();
}
```
****
First, we create the database server object from the TM Server. For this, the [GetDatabaseServer](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetDatabaseServer_System_Guid_Sdl_LanguagePlatform_TranslationMemoryApi_DatabaseServerProperties_) method is applied, which takes the (friendly) database server name and the database server properties as parameters. The database server properties are returned by function that is shown below. Note that this function has been simplified for our sample implementation. It actually returns only an empty property, however, we need it to provide the required parameter for the [GetDatabaseServer](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetDatabaseServer_System_Guid_Sdl_LanguagePlatform_TranslationMemoryApi_DatabaseServerProperties_) method.

# [C#](#tab/tabid-2)
```cs
private DatabaseServerProperties GetDbServerProperties()
{
    return new DatabaseServerProperties();
}
```
*****
When creating the container we specify the
* database server object
* physical database name (note that this name is subject to DB naming conventions)
* friendly name for the container database

Afterwards, we apply the [Save](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryContainer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryContainer_Save) method, which creates the actual container database.

Enhance the Code Example
---
In the next step we would like to show to you how to enhance the function for creating containers. Add a new function called `CreateAdvanced` to your class, which takes a TM Server object and the container database name as parameters. The aim is to make the function for creating the container more robust by first checking whether the TM Server has any database servers registered in the first place. To do this, determine the number of available database servers. If the count equals zero, an error message should be thrown:
# [C#](#tab/tabid-4)
```cs
ReadOnlyCollection<DatabaseServer> dbs = tmServer.GetDatabaseServers(DatabaseServerProperties.Containers);
if (dbs.Count == 0)
{
    throw new Exception("No DB server registered.");
}
```
****

Afterwards, check whether a container with the name that we want to give already exists on the first available database server. For simplicity reasons we are just checking the first database server, as we can assume that most TM Server setups bank on only a single DB server.
# [C#](#tab/tabid-5)
```cs
foreach (TranslationMemoryContainer item in dbs[0].Containers)
{
    if (item.Name == newContainerName)
    {
        throw new Exception("Container with that name already exists.");
    }
}
```
****

If at least one database server is available, and a container by that name does not exist yet, we create the container on the database server:
# [C#](#tab/tabid-6)
```cs
var container = new TranslationMemoryContainer(tmServer);
container.DatabaseServer = dbs[0];
container.DatabaseName = newContainerName + "DB";
container.Name = newContainerName;
container.ParentResourceGroupPath = organization;
container.Save();
```
****

Last, check whether the container was actually created by applying the `Contains` method to the [Containers](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.DatabaseServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_DatabaseServer_Containers) collection. If the container object was not found on the first available database server, an error message should be thrown:

# [C#](#tab/tabid-7)
```cs
if (!dbs[0].Containers.Contains(container))
{
    throw new Exception("Container was not created.");
}
```
****
The complete function should look as shown below:

# [C#](#tab/tabid-8)
```cs
public void CreateAdvanced(TranslationProviderServer tmServer, string organization, string newContainerName)
{
    #region "count"
    ReadOnlyCollection<DatabaseServer> dbs = tmServer.GetDatabaseServers(DatabaseServerProperties.Containers);
    if (dbs.Count == 0)
    {
        throw new Exception("No DB server registered.");
    }
    #endregion

    #region "CheckExists"
    foreach (TranslationMemoryContainer item in dbs[0].Containers)
    {
        if (item.Name == newContainerName)
        {
            throw new Exception("Container with that name already exists.");
        }
    }
    #endregion

    #region "CreateContainer"
    var container = new TranslationMemoryContainer(tmServer);
    container.DatabaseServer = dbs[0];
    container.DatabaseName = newContainerName + "DB";
    container.Name = newContainerName;
    container.ParentResourceGroupPath = organization;
    container.Save();
    #endregion

    #region "CheckCreated"
    if (!dbs[0].Containers.Contains(container))
    {
        throw new Exception("Container was not created.");
    }
    #endregion
}
```
****
Delete a Container
----
The short sample function below demonstrates how to remove a container database from the TM Server system by applying the [Delete](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryContainer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryContainer_Delete_System_Boolean_) method. This method takes a boolean parameter to indicate whether the actual physical container database should be deleted (True), or whether the database should only be unregistered from the system (False). Note that deleting the physical database represents a risk, as it may contain a lot of valuable data. Therefore, unregistering the database is the safer option, as it can be re-registered later. This method should only be applied when you are absolutely certain that the database content is no longer needed. Note that the delete operation can only be performed by a user who has the required credentials.

# [C#](#tab/tabid-9)
```cs
public void DeleteContainer(TranslationProviderServer tmServer, string organization, string containerName)
{
    if (!organization.EndsWith("/")) 
    {
        organization += "/";
    }

    TranslationMemoryContainer container = tmServer.GetContainer(organization + containerName, ContainerProperties.None);
    container.Delete(false);
}
```
*****
Putting it All Together
----

The complete class should now look as shown below:
# [C#](#tab/tabid-10)
```cs
namespace SDK.LanguagePlatform.Samples.TmAutomation
{
    using System;
    using System.Collections.ObjectModel;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerContainerManager
    {
        #region "CreateSimple"
        public void Create(TranslationProviderServer tmServer)
        {
            var container = new TranslationMemoryContainer(tmServer);

            DatabaseServer dbServ = tmServer.GetDatabaseServer("DB01", this.GetDbServerProperties());
            container.DatabaseServer = dbServ;
            container.DatabaseName = "DbName";
            container.Name = "NiceName";
            container.Save();
        }
        #endregion

        #region "props"
        private DatabaseServerProperties GetDbServerProperties()
        {
            return new DatabaseServerProperties();
        }
        #endregion

        #region "CreateAdvanced"
        public void CreateAdvanced(TranslationProviderServer tmServer, string organization, string newContainerName)
        {
            #region "count"
            ReadOnlyCollection<DatabaseServer> dbs = tmServer.GetDatabaseServers(DatabaseServerProperties.Containers);
            if (dbs.Count == 0)
            {
                throw new Exception("No DB server registered.");
            }
            #endregion

            #region "CheckExists"
            foreach (TranslationMemoryContainer item in dbs[0].Containers)
            {
                if (item.Name == newContainerName)
                {
                    throw new Exception("Container with that name already exists.");
                }
            }
            #endregion

            #region "CreateContainer"
            var container = new TranslationMemoryContainer(tmServer);
            container.DatabaseServer = dbs[0];
            container.DatabaseName = newContainerName + "DB";
            container.Name = newContainerName;
            container.ParentResourceGroupPath = organization;
            container.Save();
            #endregion

            #region "CheckCreated"
            if (!dbs[0].Containers.Contains(container))
            {
                throw new Exception("Container was not created.");
            }
            #endregion
        }
        #endregion

        #region "DeleteContainer"
        public void DeleteContainer(TranslationProviderServer tmServer, string organization, string containerName)
        {
            if (!organization.EndsWith("/")) 
            {
                organization += "/";
            }
            
            TranslationMemoryContainer container = tmServer.GetContainer(organization + containerName, ContainerProperties.None);
            container.Delete(false);
        }
        #endregion
    }
}
```
***

See Also
------
[Retrieving TM Containers](retrieving_tm_containers.md)