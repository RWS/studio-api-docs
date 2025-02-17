About Project Translation Memories
=====
This section explains what project translation memories are, how they are modeled in the Project Automation API and how they should be used in the recommended workflow.

Project Translation Memories
------
As explained in the [Project Configuration](project_configuration.md) section, every language direction in a project can have one or more translation providers associated with it. The primary type of translation providers are translation memories, which are used within the project to look up translations. Any new translations created by the user are also added to one or more of these translation memories. The user can specify which of the translation memories in the list are updated.

One issue is that there is no control over which translation units end up in the translation memories. During the course of the project, unreviewed, potentially invalid content ends up in there with no easy way to clean this up. The solution to this is the use *project translation memories*: a project translation memory (project TM) is a translation memory that is created specifically for use within one project, and that is discarded once the project is complete. At the end of the project, when all the files have been translated, reviewed and signed off, all translations within these files are added to the main translation memories (we call the translation memories that are added to a project the "main translation memories" to make a clear distinction with the "project translation memories"). Doing this ensures that the only content that ever ends up in the main translation memories has been reviewed, and that any work in progress or invalid content is kept out of the main translation memories and effectively only occurs in the project translation memories which can be discarded at the end of the project.

A project translation memory is created for every main translation memory added to a project, so if there are two main translation memories for a certain language direction, there will be two corresponding project translation memories. The reason this is that the setup of a project translation memory needs to be identical to the setup of the corresponding main translation memory, to make sure the project translation memory can be used in exactly the same way as the main translation memory. More specifically, they should have the same custom field definitions and language resources.

Project translation memories are automatically created during the project preparation phase. First a new translation memory is created, with an identical setup of the corresponding main translation memory. After that, all the project files are analyzed against the main translation memory, and any relevant translation units for the project are imported into the newly created project translation memory. Relevant translation units are defined as translation units that match a segment in a project with a certain minimum match score, specified by the user. Doing this ensures that all project participants initially get the same search results as they would have gotten if they worked directly with the main translation memories.

Both file- and server-based project translation memories are supported. Server-based translation memories obviously have the benefit that multiple project participants can reuse translations from each other. Which type of project translation memories is created, is a user choice and is not necessarily determined by the type of the main translation memory used. A server-based project translation memory can be created for a file-based translation memory and vice-versa.

> [!Note]
> 
> The use of project translation memories is entirely optional. Users can choose not to use project translation memories for a project.

See Also
-------
[Project TM Creation Settings](project_tm_creation_settings.md)
