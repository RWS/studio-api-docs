# Processing Files and Application Logic

Implement this functionality to update the status of document segments.

## Triggering the Batch Task
Go to the **MyCustomBatchTask.cs** class. This class is triggered when you run the batch task. It inherits from the following abstract class:

# [The Abstract Task Class](#tab/tabid-1)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L30-L31 "The Abstract Task Class")]
***
	
Declare a member to store the plug-in settings, and a string variable used to construct the XML stream for the task report content:
# [The Task Settings](#tab/tabid-2)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L35-L39 "The Task Settings")]
****

Initialize the task settings object and start constructing the report XML string by adding the root element. The root element should include the selected confirmation level value as an attribute:
# [Report XML String](#tab/tabid-3)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L43-L50 "Report XML String")]
***
## Processing the SDLXliff File
You can programmatically access the file currently being processed through the following member. In a "Hello World" implementation, you could output the name and path of the processed file.
# [Configuring the Converter](#tab/tabid-4)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L54-L71 "Configuring the Converter")]
***

SDLXliff is an XML-compliant file type, so you could process it through the standard XML API. However, we recommend using the Var:ProductName Bilingual API. To do this, add a new class to your project called **FileReader.cs**.

The **FileReader.cs** class must reference the following libraries:
```cs
using Sdl.Core.Globalization
using Sdl.FileTypeSupport.Framework.BilingualApi
```
It must also inherit from the following abstract class:
# [Abstract class to Implement](#tab/tabid-5)
[!code-csharp[FileReader](code_samples/FileReader.cs#L15-L17 "Abstract class to Implement")]
***
Add the following members to store the task settings, the file name and path of the SDLXliff file to process, and the text file name and path used to output the exported segments:
# [The Required Variables](#tab/tabid-6)
[!code-csharp[FileReader](code_samples/FileReader.cs#L21-L26 "The Required Variables")]
***
## Outputting Segment Content to a Text File
Use the following member to create the text output file. This file is created in the same folder as the corresponding SDLXliff file, with the *.txt extension appended.
# [Creating the Output File](#tab/tabid-7)
[!code-csharp[FileReader](code_samples/FileReader.cs#L40-L44 "Creating the Output File")]
***
Next, loop through each paragraph unit in the SDLXliff file. Process only paragraph units that contain segments. When a paragraph unit contains only structure tags (that is, no localizable segments), skip it. While looping through segment pairs, write all segments with the selected confirmation status to the output text file:
# [Outputting the Segment Pairs](#tab/tabid-8)
[!code-csharp[FileReader](code_samples/FileReader.cs#L48-L68 "Outputing the Segment Pairs")]
***
Once file processing is complete, close the text output file.
# [File is Complete](#tab/tabid-9)
[!code-csharp[FileReader](code_samples/FileReader.cs#L72-L77 "File is Complete")]
***
The following member is required by the interface, although this implementation does not use it directly. The file complete member is called when file processing finishes. Because users can merge SDLXliff files, this member must be present to determine what happens when processing is complete for the entire merged file.
# [Job is Complete](#tab/tabid-10)
[!code-csharp[FileReader](code_samples/FileReader.cs#L81-L88 "Job is Complete")]
***
## Completing the Report String
Go back to the **MyCustomBatchTask.cs** class. Then do the following:

Continue constructing the XML string for the report by adding the name of the file currently processed, its target language, and the date and time it was processed.

Create a **FileReader** object and pass the current SDLXliff file name and your settings object:
# [Configure converter](#tab/tabid-11)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L54-L71 "Configure converter")]
***
Complete the task by adding the closing XML report string with the closing root element. Then generate the report by using the **CreateReport** method implemented by the interface.
# [Complete task and report string](#tab/tabid-12)
[!code-csharp[MyCustomBatchTask](code_samples/MyCustomBatchTask.cs#L75-L85 "Complete task and report string")]
***
This method requires the report name, description, and the XML string for the report content. You may also add an optional language direction parameter. If this parameter is omitted, the report is not listed under a specific target language, but above all available target languages for the corresponding project.
