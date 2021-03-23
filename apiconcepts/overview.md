## [Core](core/overview.md)
This is the foundation which provides the plug-in framework available in Trados Studio, used by other APIs to define extension points inside the software. As well as the plug-in framework, this API also provides the engine besides the settings mechanism you find inside Trados Studio.

## [File Type Support Framework](filetypesupport/overview.md)
In order to translate content from a certain file type, Trados Studio extracts it into an SDLXliff. If you want to work with an unsupported file type that isn't already available with Trados Studio, you can use this API to extract the content and create the SDLXliff needed for translation.

## [Project Automation](projectautomation/overview.md)
There are many activities that must be done as part of the translation process and this is why Trados Studio provides project management features, such as analysis, pre-translation and more. Using the Project Automation API, you can build a customized translation workflow based on activities specific to your needs.

## [Translation Memory](translationmemory/overview.md)
Translation memories are an essential piece of technology for translators. Trados Studio comes with this capability, but if you're interested in using a different piece of technology for translation memories, you can enable that in Trados Studio by creating a new translation memory provider.

## [Integration](integration/overview.md)
This API enables you to extend or customize the user interface or create custom functionalities for Trados Studio. You can create new views, new sections in the menu ribbon, new options in the context menu or hook into the editor to create, update or delete certain information.

## [Verification](verification/overview.md)
Trados Studio allows translators to check their work by running verifiers during translation. There are many ways to verify the quality of a translation and it can also become very specific. So to address that need, Trados Studio allows custom verifiers to be built and run.

## [Batch Tasks](batchtasks/overview.md)
As part of a project management workflow, there are certain tasks that need to be applied, such as pre-translation, analysis and more. Trados Studio comes with a predefined set of tasks, but with this API, you can also create your own custom tasks that can be included in your workflows.

## [Terminology Provider](terminology/overview.md)
This API allows you to enable the use of different terminology technology in Trados Studio, by creating new terminology providers.
