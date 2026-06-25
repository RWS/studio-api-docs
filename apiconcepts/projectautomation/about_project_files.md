# About Project Files
This section provides an overview of file types in a localization project and explains how Var:ProductName models them.

## Project Files
The primary files in a localizable project are those that require translation—referred to here as *translatable files*. Specifically, translatable files are those from which the File Type Support Framework can extract and manipulate content. For more information, see [Translatable Files](#translatable-files).

Certain project types may consist of a large quantity of translatable files, which each only contain a small amount of content, for instance a website translation project, or files that were exported from a content management system. To facilitate processing these types of projects, the Var:ProductName offers the ability to merge a number of these files into a single translatable, so that it can be processed and edited as one big unit. Merging files could happen at various stages in the project: upfront as part of project preparation, or on a more ad-hoc basis by a translator who wants to edit multiple files at once. For more information, see [Merged Translatable Files](#merged-translatable-files).

Some files require localization but lack an RWS file type definition capable of extracting translatable content. Graphics files containing text are a common example. These *localizable files* can be localized within the project, but outside the standard RWS tool workflow—for instance, using a graphics editor. For more information, see [Localizable Files](#localizable-files).

*Reference files* belong to the project but remain unmodified during translation—for example, external graphics required to preview an HTML file. For more information, see [Reference Files](#reference-files).

Processing a translatable file sometimes produces additional linked files that must accompany it through the rest of the translation process. These *auxiliary files* include items such as PDF preview files generated for commenting. For more information, see [Auxiliary Files](#localizable-files).

## File Object Model
**IProjectFile** serves as the base for all project files and exposes common properties and functionality, such as downloading.

An **ILanguageFile** is a project file that is associated with a specific project language, which could be a source or a target language. Initially, when a file is added to the system, it is typically a source language file. The source language file then goes through some source language specific processing, for instance, conversion into the SDLXliff file format and segmentation. At some point, a target language file is created for every target language in the project, after which target language- specific processing takes place, such as translation memory analysis and translation. All types of project files are language files and are therefore associated with a specific project language. Every source language file has the ability to find its related target language files and vice-versa.

The system versions all modifiable project files, maintaining a full version history. Reference files are the exception.

## Translatable Files
Translatable files are files that can be processed and translated using RWS's tools, i.e. for which a file type definition exists in the  File Type Support Framework. As a result, translatable files can carry some additional information:

* **File Type**: Identifies with which file type definition this particular file will be processed. It also specifies any other file-type specific aspects, such as how to preview this file, how to verify the content of this file, which QuickInsert (=special characters or tags that can be inserted during editing) items are valid for use, etc.
* **Analysis Statistics**: Statistics on how much existing translations can be leveraged from translation memories or previously translated files. These statistics are expressed in terms of characters, words and segments. Statistics are calculated for various categories, such a 100% TM matches, fuzzy TM match categories (50%-74%, 75%-84%, 85%-94%, 95%-99%), repetitions, no match, etc. For more information, see [AnalysisStatistics](../..//api/projectautomation/Sdl.ProjectAutomation.Core.AnalysisStatistics.yml). Full analysis statistics are only available for target language translatable files. Source language translatable files can only have total character, word and segment count information.
* **Confirmation Statistics**: Statistics on how far the file has progressed throughout the translation process. Every segment in a translatable file is assigned a confirmation level (not translated, draft, translated, translation rejected, translation approved, sign-off rejected, signed off). The confirmation statistics for a translatable file constitute the sums of characters, words or segments for each of these levels. Based on these statistics, a combined confirmation level is also computed: this gives an indication of the status of the entire file. Confirmation statistics are only available for target language translatable files.
* **Auxiliary Files**: A number of files that are associated with the translatable file. These files are typically the result of some automatic task running on the file, but can also be manually attached to the file. For more information see Auxiliary Files.


Translatable files are represented by the `ITranslatableFile` interface in the object model.

## Merged Translatable Files
A merged translatable file is a translatable file which is created by merging a number of translation files into one. The Var:ProductName provides capabilities to define which files should be merged into one. Subsequently, the files can actually be merged by creating one consolidated SDLXliff file containing the merged content of all the files. After working on the merged file, it can be split into single files again. Apart from the fact that you can merge and split a merged translatable file, it is just a normal translatable file, and can be manipulated as such.

Merged translatable files are represented by the ```IMergedTranslatableFile``` interface in the object model.

## Localizable Files
Localizable files are files that need to be changed as part of the localization process, but that cannot be processed using standard RWS tools (i.e. the File Type Support Framework). As a result of that, localizable files do not have analysis or confirmation statistics. Other than that, localizable files are changeable, versioned files.

Localizable files are represented by the ```ILocalizableFile``` interface in the object model.

## Reference Files
Reference files are files that need to be part of the project, for instance for preview reasons, but that can be changed as part of the project. Reference files are language files, which means they are associated with a particular project language.

Reference files are represented by the `IReferenceFile` interface in the object model.

<img style="display:block; " src="images/NewProject03.jpg"/>

The screenshot above illustrates how project files are selected in Var:ProductNameWithEdition.

## See also
[Adding Files and Folders](adding_files_and_folders.md)

[Creating a Merged File](creating_a_merged_file.md)

[Updating Project Files](updating_project_files.md)

[Retrieving the Project Statistics](retrieving_the_project_statistics.md)

[Generating the Task Report](generating_the_task_report.md)
