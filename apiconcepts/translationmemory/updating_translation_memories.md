## Updating Translation Memories

If the option is enabled, Var:ProductName updates a TM while users translate or edit a document.

During translation, the following update scenarios apply:

* When users translate a new segment, a new translation unit (TU) is added to the TM.
* When users edit a fuzzy match, the TM saves a new TU.
* When users edit the translation from an exact match, the edited translation overwrites the original translation.
* When users need an alternative translation for the current context instead of the TM suggestion, they can use a special command to add the alternative alongside the existing translation. In this case, two TUs are created for the same source segment and remain side-by-side in the TM. When two exact (100%) matches are found in a TM, a penalty is applied by default, so both are reduced to 99% matches. This penalty indicates that two possible solutions exist in the TM, and users choose the translation that best fits the current context.

## See Also
[Updating a Translation Memory](updating_a_translation_memory.md)
