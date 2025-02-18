Updating Translation Memories
====
A TM is updated  - if the option to do so is enabled - while the user is translating or editing a document in Var:ProductName.

While a translator is translating the following update scenarios will apply:

* When users translate a new segment, a new translation unit (TU) gets added to the TM.
* When a fuzzy match is found, it will usually be edited by the user, which will also save a new TU in the TM.
* When an exact match is found, but the suggested translation is edited by the user, the edited translation will overwrite the original translation.
* When an exact match is found, but the user decides that in the current context an alternative translation is required (rather than the translation suggested by the TM), the user can use a special command to add the alternate translation alongside the existing translation. In this case, two TUs for the same source segment will be created, which will continue to exist side-by-side in the TM. Whenever two exact (100%) )matches are found in a TM, by default a penalty will be applied to these two TUs, i.e. they will both be reduced to 99% matches. The penalty is applied in order to point out to the user that two possible solutions exist in the TM. The user then has to choose the translation that best fits the current context.

See Also
--------
[Updating a Translation Memory](updating_a_translation_memory.md)
