# Creating a Container Database

This chapter shows how to create a container on a database server for storing translation memories.

## Add a New Class

Start by adding a new class called `ServerContainerManager`. Then implement a public `Create` function that takes a [TranslationProviderServer](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml) object as a parameter. The function should look like this:

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
First, create the database server object from the TM Server. Use the [GetDatabaseServer](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetDatabaseServer_System_Guid_) method, which takes the server ID or path. The database server properties come from the helper function shown below. In this sample, the helper is simplified and returns an empty property object, but it still supplies the required parameter for the [GetDatabaseServer](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationProviderServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationProviderServer_GetDatabaseServer_System_Guid_) method.

# [C#](#tab/tabid-2)
```cs
private DatabaseServerProperties GetDbServerProperties()
{
    return new DatabaseServerProperties();
}
```
When you create the container, specify the following values:

* Database server object
* Physical database name, which must follow database naming conventions
* Friendly name for the container database

Then call the [Save](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryContainer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryContainer_Save) method to create the container database.

## Enhance the Code Example

Next, extend the container-creation logic. Add a new function called `CreateAdvanced` to your class. It should take a TM Server object and the container database name as parameters. The goal is to make the function more robust by first checking whether the TM Server has any registered database servers. If the count is zero, throw an error message:
# [C#](#tab/tabid-4)
```cs
ReadOnlyCollection<DatabaseServer> dbs = tmServer.GetDatabaseServers(DatabaseServerProperties.Containers);
if (dbs.Count == 0)
{
    throw new Exception("No DB server registered.");
}
```
Then check whether a container with the requested name already exists on the first available database server. For simplicity, this sample checks only the first database server, because most TM Server setups use a single database server.
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
If at least one database server is available and no container with that name exists, create the container on the database server:
# [C#](#tab/tabid-6)
```cs
var container = new TranslationMemoryContainer(tmServer);
container.DatabaseServer = dbs[0];
container.DatabaseName = newContainerName + "DB";
container.Name = newContainerName;
container.ParentResourceGroupPath = organization;
container.Save();
```

Last, check whether the container was actually created by applying the `Contains` method to the [Containers](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.DatabaseServer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_DatabaseServer_Containers) collection. If the container object was not found on the first available database server, an error message should be thrown:

# [C#](#tab/tabid-7)
```cs
if (!dbs[0].Containers.Contains(container))
{
    throw new Exception("Container was not created.");
}
```
The complete function should look like this:

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
## Delete a Container

The sample function below shows how to remove a container database from the TM Server system by using the [Delete](../../api/translationmemory/Sdl.LanguagePlatform.TranslationMemoryApi.TranslationMemoryContainer.yml#Sdl_LanguagePlatform_TranslationMemoryApi_TranslationMemoryContainer_Delete) method. This method takes a Boolean parameter that determines whether to delete the physical container database (`true`) or unregister it from the system (`false`). Deleting the physical database can be risky, because it may contain valuable data. Unregistering the database is safer, because you can re-register it later. Use this operation only when you are certain that the database content is no longer needed. The delete operation also requires the necessary credentials.

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
## Putting It All Together

The complete class should now look like this:
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

## See Also

[Retrieving TM Containers](retrieving_tm_containers.md)
