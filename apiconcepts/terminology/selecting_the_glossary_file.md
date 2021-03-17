Selecting the Glossary File
=====
Learn how to select the delimited text file as a terminology provider for our implementation.

How to return the Term Provider URI, Name and Description
-------

Open the **MyTerminologyProvider.cs** class. Add the following public string variable which stores the delimited text file name:

# [The Text File Name](#tab/tabid-1)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L18-L19)]
***

Modify the following members to return the term provider URI, which includes the text file name:

# [The Term Provider Uri](#tab/tabid-2)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L63-L86)]
***

Then add the following member, which allows us to pass any settings to the term provider. In our implementation, this will be the path to the glossary file:

# [The Term Provider Settings](#tab/tabid-3)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L23-L27)]
***

Modify the two following members to return the term provider name and definition:

# [The Term Provider Name and Definition](#tab/tabid-4)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L63-L86)]
***

How to select the Glossary File
-------
Open the **MyTerminologyProviderWinFormsUI.cs** class and modify the **Browse()** method as shown below. This method allows you to raise the UI in which the user selects the text file, that is the **Open File** dialog box. Create a terminology provider object based on the **MyTerminologyProvider** class and set the **fileName** variable to the selected file name and path.

# [Browsing for the Glossary](#tab/tabid-5)
[!code-csharp[MyTerminologyProviderWinFormsUI](code_samples/MyTerminologyProviderWinFormsUI.cs#L43-L58)]
***

Go to the **MyTerminologyProviderViewerWinFormsUI.cs** class and create a terminology provider object. Make sure that your provider object is initialized as follows:

# [The Term Provider Object](#tab/tabid-6)
[!code-csharp[MyTerminologyProviderViewerWinFormsUI](code_samples/MyTerminologyProviderViewerWinFormsUI.cs#L13)]
***

# [Initializing the Provider](#tab/tabid-7)
[!code-csharp[MyTerminologyProviderViewerWinFormsUI](code_samples/MyTerminologyProviderViewerWinFormsUI.cs#L105-L108)]
***

# [Provider is Initialized](#tab/tabid-8)
[!code-csharp[MyTerminologyProviderViewerWinFormsUI](code_samples/MyTerminologyProviderViewerWinFormsUI.cs#L34-L40)]
***

When selecting the text file, Trados Studio will try to retrieve the languages from the terminology list. As we have not implemented the corresponding functionality yet, simply change the **GetLanguages()** method of the **MyTerminologyProvider.cs** class to return null for the moment.

We will implement the required language retrieval functionality in the next chapter.

The Terminology Provider Factory Class
-----

Open the class **MyTerminologyProviderFactory.cs** and create your term provider object as show below. Only then will Trados Studio be able to access and display your provider in the UI:

# [Creating the Terminology Provider Object](#tab/tabid-9)
[!code-csharp[MyTerminologyProviderFactory](code_samples/MyTerminologyProviderFactory.cs#L10-L15)]
***