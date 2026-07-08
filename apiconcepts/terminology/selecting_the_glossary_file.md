# Selecting the Glossary File

Learn how to configure a delimited text file as a terminology provider in your implementation.

## Returning the Term Provider URI, Name, and Description

Open the **MyTerminologyProvider.cs** class.
Add the following public string variable to store the delimited text file name:

# [The Text File Name](#tab/tabid-1)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L18-L19)]
***

Modify the following members to return the term provider URI, including the text file name:

# [The Term Provider Uri](#tab/tabid-2)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L79-L86)]
***

Add the following member to pass settings to the term provider. In this implementation, it specifies the glossary file path:

# [The Term Provider Settings](#tab/tabid-3)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L23-L27)]
***

Modify the following members to return the term provider name and definition:

# [The Term Provider Name and Definition](#tab/tabid-4)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L63-L86)]
***

# [The Term Provider Initialization](#tab/tabid-4)
[!code-csharp[MyTerminologyProvider](code_samples/MyTerminologyProvider.cs#L246-L286)]
***

How to select the Glossary File
-------
Open the **MyTerminologyProviderWinFormsUI.cs** class and modify the **Browse()** method as shown below. This method allows you to raise the UI in which the user selects the text file, that is the **Open File** dialog box. Create a terminology provider object based on the **MyTerminologyProvider** class and set the **fileName** variable to the selected file name and path.

# [Browsing for the Glossary](#tab/tabid-5)
[!code-csharp[MyTerminologyProviderWinFormsUI](code_samples/MyTerminologyProviderWinFormsUI.cs#L43-L58)]
***

Open the **MyTerminologyProviderViewerWinFormsUI.cs** class.
Create and initialize a terminology provider object as follows:

# [The Term Provider Object](#tab/tabid-6)
[!code-csharp[MyTerminologyProviderViewerWinFormsUI](code_samples/MyTerminologyProviderViewerWinFormsUI.cs#L12-L14)]
***

# [Initializing the Provider](#tab/tabid-7)
[!code-csharp[MyTerminologyProviderViewerWinFormsUI](code_samples/MyTerminologyProviderViewerWinFormsUI.cs#L116-L119)]
***

# [Provider is Initialized](#tab/tabid-8)
[!code-csharp[MyTerminologyProviderViewerWinFormsUI](code_samples/MyTerminologyProviderViewerWinFormsUI.cs#L34-L40)]
***

When selecting the text file, Var:ProductName attempts to retrieve the languages from the terminology list. Because the corresponding functionality is not implemented yet, change the **GetLanguages()** method of the **MyTerminologyProvider.cs** class to return null for now.

We will implement the required language retrieval functionality in the next chapter.

## The Terminology Provider Factory Class

Open the **MyTerminologyProviderFactory.cs** class and create your term provider object as shown below. Only then can Var:ProductName access and display your provider in the UI:

# [Creating the Terminology Provider Object](#tab/tabid-9)
[!code-csharp[MyTerminologyProviderFactory](code_samples/MyTerminologyProviderFactory.cs#L10-L15)]
***
