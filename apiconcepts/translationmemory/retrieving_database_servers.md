Retrieving Database Servers
===

In this chapter you will learn how to get programmatic access to the database server(s) that is/are used by the TM Server.

Add a New Class
----

Start by adding a new class called `ServerDBServers` to your project. Add a public function called `GetDBServers`, which takes a `TranslationProviderServer` object as parameter. Call this function from Connect as shown below:

```
ServerDBServers dbServ = new ServerDBServers();
dbServ.GetDBServers(tmServer);
```
In the GetDBServers function the GetDatabaseServers method is applied to the server object. Then the function loops through the database servers that are registered for the given TM server. Although most setups involve only a single database server, it is conceivable that the SDL TM Server connects to several DB servers.

```
public void GetDBServers(TranslationProviderServer tmServer)
{
    string serverInfo = string.Empty;

    foreach (DatabaseServer dbServer in tmServer.GetDatabaseServers(DatabaseServerProperties.None))
    {
        serverInfo += "Server name: " + dbServer.ServerName + "\n";
        serverInfo += "Friendly server name: " + dbServer.Name + "\n";
        serverInfo += "Server description: " + dbServer.Description + "\n";
        serverInfo += "Server type: " + dbServer.ServerType + "\n\n";
    }

    MessageBox.Show(serverInfo, "Container Information");
}
```
In the `foreach` loop we build up the string that contains all DB servers and their properties. `ServerName` gets the full DNS name or IP address of the DB server, while Name is used to retrieve the friendly DB server name. Note that when registering a DB server the administrator can enter the actual server name (e.g. sqlserv\sqlexpress) and assign a friendly name like *TestDB*. Among other things, you can retrieve a server Description (which is optional) as well the `ServerType`, which can currently be SQL Server or Oracle.
Below you see an example of a box containing information on a particular DB server in the TM Server Manager:

<img style="display:block; " src="images/DbServerInfo.jpg"/>

Putting it All Together
----
The complete class looks as shown below:

```
namespace Sdl.SDK.LanguagePlatform.Samples.TmAutomation
{
    using System.Windows.Forms;
    using Sdl.LanguagePlatform.TranslationMemoryApi;

    public class ServerDBServers
    {
        #region "get"
        public void GetDBServers(TranslationProviderServer tmServer)
        {
            string serverInfo = string.Empty;

            foreach (DatabaseServer dbServer in tmServer.GetDatabaseServers(DatabaseServerProperties.None))
            {
                serverInfo += "Server name: " + dbServer.ServerName + "\n";
                serverInfo += "Friendly server name: " + dbServer.Name + "\n";
                serverInfo += "Server description: " + dbServer.Description + "\n";
                serverInfo += "Server type: " + dbServer.ServerType + "\n\n";
            }

            MessageBox.Show(serverInfo, "Container Information");
        }
        #endregion
    }
}
```