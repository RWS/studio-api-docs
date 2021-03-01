Implementing filtering capabilities to your Display Filter
======
Add the basic filtering functionality to your custom filter.

To work with the Display Filter API, you need to create a class that implements the *IDisplayFilter* interface from the *Sdl.TranslationStudioAutomation.IntegrationApi.DisplayFilters* namespace, which exposes one method called *EvaluateRow*. This method will then be called by the API for each row after the filter is applied on the document, passing in the *IDisplayFilterRowInfo* model (which includes the *ISegmentPair* and other relevant properties) that will permit the developer to assert whether or not the segments are visible in the editor.

Create a new class called **FilterSettings**. This class will manage some basic settings that will persist on the document once the filter has been applied. Make reference to the following example:

To recover the source and target content, we will need to create a class that implements the *IMarkupDataVisitor*. This interface is designed in such a way that you decide what properties from the *ISegment* are relevant and then process only those.

For the purpose of this example, we will recover the content as plain text only, including tags and revisions. This should be sufficient in allowing us to apply a filter to the content.

Create a new class called **ContentProcessor** with reference to the following code:

Next, create a new class called **DisplayFilter**. This will implement the *IDisplayFilter* interface so that we can evaluate each of the segments and decide whether or not they should be displayed in the Trados Studio Editor.

It is good design to include a reference to *IFilterSetting* interface in this class as it will be persisted on the document once the filter has been applied. This is useful to understand the type of filter that is applied (if any), especially in the case when the user is moving between documents in the editor. It permits the developer to differentiate between the internal system filter provider and their own implementation or multiple implementations and then take the appropriate action based on that.

This example demonstrates how to implement a few filters that are evaluated as the API is iterating over each of the segments after the filter has been applied on the document. Further on in the walkthrough we will discuss how to apply this implementation of the *IDisplayFilter* on the document itself.

You will notice from the filter examples that we are using the *ContentProcessor* functionality that we created earlier to recover the plain text from both the source and target segments and then apply either a regular expression or normal search as criteria for the filter