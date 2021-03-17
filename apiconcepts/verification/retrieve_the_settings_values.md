Retrieve the Settings Values
======
In this chapter you will learn how to retrieve the settings that have been configured through the plug-in user interface (see Implement the User Interface).

Add a Class for Retrieving the Settings Values
------

After implementing the user interface, you need to add a separate class for retrieving the plug-in settings values. Add a new class to your project, and call it e.g. `IdenticalVerifierSettings.cs`. The class needs to reference the **Sdl.Core.Settings** namespace. Your class needs to be derived from the **SettingsGroup** class. Below you see what the skeleton of your new class looks like:

# [C#](#tab/tabid-1)
[!code-csharp[Settings](code_samples/Settings.aml#L24-L30)]
***

Our sample application only has one setting, i.e. a (display code) string value that defines which context should be relevant for the verification, e.g. **H** for **Heading**. This setting will be implemented as a string property, which we will call, for example `CheckContext`:

We will also implement a method for setting the default value. Let us assume that headings are most likely to stay unchanged in the target language. Therefore it makes sense to apply the verification by default to segment pairs whose context has the context display code **H**:

The plug-in settings are physically stored in the project files (* .*sdlproj*) or project template files (* .*sdltpl*). The settings group in the (XML-compliant) project (template) file can look as shown below. As you can see, the setting id corresponds to the name of the property that we have implemented in this class.

Note that the **Enabled** property does not need to be implemented by your plug-in. The plug-in framework provides the mechanism for enabling/disabling global verifier plug-ins through the user interface of Trados Studio 2017.

Putting it All Together
----
The complete class that is used for retrieving the settings value should look as shown below: