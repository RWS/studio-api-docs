# Overview
This section explains what a localization project is and how the Project Automation API supports it.

## Localization Projects
A localization project encompasses the data, resources, and processes required to localize source documents from a given language (the source language) into one or more target languages. As one would expect, a large part of this work will be accomplished using RWS's localization tools, primarily it's File Processing, the Translation Memory and Terminology tools. Beyond these low-level components, a higher-level framework is needed—one that models localization projects in their entirety and provides access to localization tools through standard, recommended workflows following best practices. At its core, the Project Automation API revolves around automated processing of source documents using RWS's localization tools.

A typical localization project consists of a preparation phase upfront, in which files are prepared for translation. This process can usually be automated to a large extent. However, some of it will require manual work, for instance when it comes to dealing with unexpected or faulty file formats or content. The main result of the preparation process are the prepared files, but there will also be important scoping information about the project, such as the number of words there are to translate, which part of the translation has been leveraged from previous translation or translation memories. This information, commonly referred to as the "analysis report" then feeds into project planning, resourcing and customer billing.

The preparation phase is followed by a number of manual steps, i.e. mainly the translation of the files itself and review of that translation. Finally, after review, the translated files are prepared for delivery to the customer. Again, this is a process that can be largely automated, but which for certain file formats requires manual work, for example, DTP work on Adobe FrameMaker or InDesign files.

Real-life localization projects vary widely. They rarely follow standard workflows—customer requirements, file format differences, and historically evolved practices all introduce unique challenges. The Project Automation API addresses these variations through its configuration and extensibility options.

The Project Automation API also supports integration with external systems such as content management systems, CRM systems, and workflow systems.

## Basic Project Automation API Concepts
This section focuses on how the Project Automation API models localization projects. The following topics are covered:

* [About Projects](about_projects.md): High-level overview of a project in the Project Automation API sense.
* [About Project Files](about_project_files.md): Overview of how various types of files are modeled in a project.
* [Project Configuration](project_configuration.md): Overview of various configuration options that are provided out-of-the-box to support standard workflows and use cases.
* [About Tasks](about_tasks.md): Describes how various aspects of the localization project can be automated though automatic tasks and how manual tasks are used to model the human work in a localization project.
* [About Packages](about_packages.md): Describes how packages are used to send work between the various participants of a project.
