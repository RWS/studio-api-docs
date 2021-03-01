Introduction
=====
This part of the help system explains what a localization project is and how the Project Automation API supports localization projects.

Localization Projects
-----
We define a localization project as the data, resources and process required to localize a set of source documents in a given language (i.e. the source language) into one or more target languages. As one would expect, a large part of this work will be accomplished using SDL's localization tools, primarily it's File Processing, the Translation Memory and Terminology tools. However, beyond these low-level tools and components, there is a need for a higher-level framework or environment, which models localization projects in their entirety and which provides an easy way to use the variety of localization tools through standard, recommended workflows, following best practices. Indeed, at it's core, a lot of what the Project Automation API provides revolves around automated processing of source documents using SDL's localization tools.

A typical localization project consists of a preparation phase upfront, in which files are prepared for translation. This process can usually be automated to a large extent. However, some of it will require manual work, for instance when it comes to dealing with unexpected or faulty file formats or content. The main result of the preparation process are the prepared files, but there will also be important scoping information about the project, such as the number of words there are to translate, which part of the translation has been leveraged from previous translation or translation memories. This information, commonly referred to as the "analysis report" then feeds into project planning, resourcing and customer billing.

The preparation phase is followed by a number of manual steps, i.e. mainly the translation of the files itself and review of that translation. Finally, after review, the translated files are prepared for delivery to the customer. Again, this is a process that can be largely automated, but which for certain file formats requires manual work, for example, DTP work on Adobe FrameMaker or InDesign files.

Real-life localization projects are notorious for their variety. They rarely follow standard workflows, and every customer and project will present their own challenges, which is due to customer requirements, differences in file formats, and different working practices that have evolved historically. The Project Automation API aims at providing a way to deal with these variations, through its configuration and extensibility options.

Another aspect is the integration into external systems. The Project Automation API also aims at providing various ways to integrate with external systems, such as content management systems, CRM systems, workflow systems, etc.

Basic Project Automation API Concepts
-----
This section of the help system focuses on explaining how the Project Automation API models localization projects. Throughout this section you will find references to how to this is done. The following topics will be covered:

About projects: High-level overview of a project in the Project Automation API sense.
About project files: Overview of how various types of files are modeled in a project.