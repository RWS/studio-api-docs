# Processing Files and Application Logic

Implement the functionality that changes the status of document segments.

How to trigger the batch task
---------------------------
Go back to the **MyCustomBatchTask.cs** class. This class is triggered when you decide to run the batch task. This class inherits from the following abstract class:

# [The Abstract Task Class](#tab/tabid-1)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L30-L31 "The Abstract Task Class")]
***
	
Declare a member that stores the plug-in settings, as well as a string variable that is used to construct the XML stream for the task report content:
# [The Task Settings](#tab/tabid-2)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L35-L39 "The Task Settings")]
****

Initialise the task settings object and start constructing the report XML string by adding the root element. The root element should contain the selected confirmation level value in an attribute:
# [Report XML String](#tab/tabid-3)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L43-L50 "Report XML String")]
***
How to process the SDL XLIFF file
-----------------------------------
You can programmatically access the file that is currently processed through the following member. In a "Hello World"-type implementation you could output the name and path of the processed file.
# [Configuring the Converter](#tab/tabid-4)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L54-L71 "Configuring the Converter")]
***

As SDL XLIFF is an XML-compliant file type, you could  process it using the standard XML API. However, we recommend that you use the <Var:ProductName> Bilingual API to process the file. This way, we add a new class to our project called **FileReader.cs**.

The **FileReader.cs** class needs to reference the following libraries:
```cs
using Sdl.Core.Globalization
using Sdl.FileTypeSupport.Framework.BilingualApi
```
It also needs to inherit from the following abstract class:
# [Abstract class to Implement](#tab/tabid-5)
[!code-csharp[FileReader](code_samples/FileReader.cs#L15-L17 "Abstract class to Implement")]
***
Add the following members to store the task settings, the file name and path of the SDL XLIFF file to be processed, and the text file name and path that is used to output the exported segments:
# [The Required Variables](#tab/tabid-6)
[!code-csharp[FileReader](code_samples/FileReader.cs#L21-L26 "The Required Variables")]
***
How to output the segment content to a text file</title>
------------------------------------------------
With the following member we start by creating the text output file. This file is created in the same folder as the corresponding SDL XLIFF file and the *.txt extension is appended.
# [Creating the Output File](#tab/tabid-7)
[!code-csharp[FileReader](code_samples/FileReader.cs#L40-L44 "Creating the Output File")]
***
In the next step, we loop through each paragraph unit of the SDL XLIFF file. We make sure to process only paragraph units that actually contain segments. When we encounter a paragraph unit that only contains structure tags (i.e. no localizable segments), we abort. When looping through the segment pairs, we write all segments with the selected confirmation status to the output text file:
# [Outputing the Segment Pairs](#tab/tabid-8)
[!code-csharp[FileReader](code_samples/FileReader.cs#L48-L68 "Outputing the Segment Pairs")]
***
Once the file processing is done, we close the text output file.
# [File is Complete](#tab/tabid-9)
[!code-csharp[FileReader](code_samples/FileReader.cs#L72-L77 "File is Complete")]
***
The following member is required by the interface, although our implementation does not actually use it. The file complete member is called when processing a file is finished. However, as users could merge SDL XLIFF files, the following member must be present to determine what happens when the process has been completed for the entire merged file.
# [Job is Complete](#tab/tabid-10)
[!code-csharp[FileReader](code_samples/FileReader.cs#L81-L88 "Job is Complete")]
***
How to complete the Report String
----------------------------------------
Go back to the **MyCustomBatchTask.cs** class. Here we do the following:

We continue constructing the XML string for the report by adding the name of the file currently processed, its target language and the date/time at which it was processed.

We create a **FileReader** object to which we pass the current SDL XLIFF file name, as well as our settings object:
# [Configure converter](#tab/tabid-11)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L54-L71 "Configure converter")]
***
We complete the task by adding the closing XML report string with the closing root element. Then we generate the report using the **CreateReport** method implemented by the interface.
# [Complete task and report string](#tab/tabid-12)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L75-L85 "Complete task and report string")]
***
This method requires the report name, description and the XML string for the report content. You  may also add an optional language direction parameter. If this parameter is missing, the report will not be listed under the specific target language, but rather above all available target languages of the corresponding project.