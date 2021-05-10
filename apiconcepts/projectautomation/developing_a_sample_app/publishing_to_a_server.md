Publishing to a Server
==

It's a simple task to publish a project. To do so you will need to enter the details of your project server (as supplied by your project server administrator) You will also need to log in to the server with a user that has the "Publish Project" permission

You will need to change the details in the following line of code to your own server details

```cs
newProject.PublishProject(
    new Uri("ps.http://MyProjectServer:80"), 
    false, 
    "MyUser", 
    "MyPassword",
    "/MyOrganizationPath",
    null);
```

The following parameters are required in order
* Uri of the server in the form "**protocol://address:portnumber**". The protocol will he either "**http**" or "**https**", the address will be the location of the server, and the port number will be the port you set project server up on (default: 80). For example **http://projectserver.mycompany.com:80**
a boolean flag to indicate if you wish lo login with windows security
* The user name you wish to log in as. (leave blank for windows authentication)
* The password of the user. (leave blank for windows authentication)
* The location on the server to which the project should be published.
* A delegate function is needed should you require progress information or cancelation support. Enter **null** if these are not required

