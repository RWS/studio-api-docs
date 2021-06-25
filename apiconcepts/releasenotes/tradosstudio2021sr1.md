Release Notes for <Var:ProductNameWithEdition> SR1
===================

Integration API
----

### Introduced enhancements to [FilesController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.FilesController.yml)

The [FilesController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.FilesController.yml) capabilities have been extended with support for the following:
* [CurrentVisibleFiles](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.FilesController.yml#Sdl_TranslationStudioAutomation_IntegrationApi_FilesController_CurrentVisibleFiles) - returns the list of project files that is currently displayed in the Files View
* [CurrentSelectedLanguage](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.FilesController.yml#Sdl_TranslationStudioAutomation_IntegrationApi_FilesController_CurrentSelectedLanguage) - returns the currently selected language in the Files View
* [SelectedFiles](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.FilesController.yml#Sdl_TranslationStudioAutomation_IntegrationApi_FilesController_SelectedFiles) - marks as selected in the Files View the given list of Files 

# [Declare the actions individually](#tab/tabid-1)
```cs
public bool SelectFileByName(string fileName)
      {
          var matchingFile = _filesController.CurrentVisibleFiles.FirstOrDefault(f => f.Name == fileName);
          if (matchingFile != null)
          {
              _filesController.SelectFiles(new List<ProjectFile> { matchingFile });
              return true;
          }
          return false;
      }
```
***

### Ability to delete all comments from the document

All active file comments can be deleted using [IStudioDocument.DeleteAllCommentsWithoutNotification()](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml#Sdl_TranslationStudioAutomation_IntegrationApi_IStudioDocument_DeleteAllCommentsWithoutNotification). The user does not need to save the project explicitly as this method also saves the file.

### Ability to retrieve the selected segment pairs from the editor

Exposed [GetSelectedSegmentPairs](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml#Sdl_TranslationStudioAutomation_IntegrationApi_IStudioDocument_GetSelectedSegmentPairs) method which returns a list of the selected segment pairs from the editor.

### Ability to get the parent paragraph of a segment pair

Exposed [GetParentParagraphUnit](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml#Sdl_TranslationStudioAutomation_IntegrationApi_IStudioDocument_GetParentParagraphUnit_Sdl_FileTypeSupport_Framework_BilingualApi_ISegmentPair_) method which returns a clone of the [ParentParagraphUnit](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml) of a [SegmentPair](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IParagraphUnit.yml).

### Exposed the based url used for communicating with LC API

In order for a plugin to obtain the Language Cloud API URL used, it had to read the <Var:ProductName> configuration file. We now expose the base URL for the Language Cloud API in the [LanguageCloudIdentityApi](../../api/integration/Sdl.LanguageCloud.IdentityApi.LanguageCloudIdentityApi.yml) class.

### Ability to interact with track changes from the API

Via the [IStudioDocument](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml) interface, third party developers can now use the following methods:

* [AcceptAllTrackedChanges()](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml#Sdl_TranslationStudioAutomation_IntegrationApi_IStudioDocument_AcceptAllTrackedChanges_Sdl_Core_Globalization_ConfirmationLevel_) - allows accepting all tracked changes and setting the confirmation level.
* [SelectedSegmentsContainRevisions()](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml#Sdl_TranslationStudioAutomation_IntegrationApi_IStudioDocument_SelectedSegmentsContainRevisions) - checks if the selected segments in target contains Revisions or TQA.

### Ability to find and select a text from a given segment

Method [FindTextInSegment](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.IStudioDocument.yml#Sdl_TranslationStudioAutomation_IntegrationApi_IStudioDocument_FindTextInSegment_System_String_System_String_System_Boolean_System_Boolean_) returns true if a given text value is present in the specified segment number. It also has the ability to select the found text in the segment and bringing it into view. It can search in either the source or the target.

### Ability to run a method on the UI thread through the Controllers

Developers can now marshal method execution on the UI thread if that is required using the [BeginInvoke](../../api/integration/Sdl.Desktop.IntegrationApi.Internal.AbstractBindedController.yml#Sdl_Desktop_IntegrationApi_Internal_AbstractBindedController_BeginInvoke_System_Delegate_) and [Invoke](../../api/integration/Sdl.Desktop.IntegrationApi.Internal.AbstractBindedController.yml#Sdl_Desktop_IntegrationApi_Internal_AbstractBindedController_Invoke_System_Delegate_) method present on all the controllers, such as EditorController

### Ability to control Concordance Search functionality.

Developers can now use the [IConcordanceSearchController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Editor.ConcordanceSearch.IConcordanceSearchController.yml) to obtain access to the Concordance Search functionality in the editor. They can now programatically perform searches in either source or target and read the results back. The controller can be obtained from the [EditorController.ConcordanceSearchController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml#Sdl_TranslationStudioAutomation_IntegrationApi_EditorController_ConcordanceSearchController) property.

### Ability to obtain the Translation Results

Developers can now use the [ITranslationResultsController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.Editor.TranslationResults.ITranslationResultsController.yml) to obtain access to the translation results in the editor. They can now programatically select a result and apply it to the segment. The controller can be obtained from the [EditorController.ConcordanceSearchController](../../api/integration/Sdl.TranslationStudioAutomation.IntegrationApi.EditorController.yml#Sdl_TranslationStudioAutomation_IntegrationApi_EditorController_TranslationResultsController) property.