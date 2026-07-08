# What is the Verification Framework?

The verification framework is a service built on top of the File Type Support Framework. It is used to verify translated documents.

## Verification methods

Users can verify documents in three different ways:

* **Interactive verification**: The segment is verified immediately when it is confirmed by the user.
* **Document verification (on-demand verification)**: All target segments of the document are checked when the user presses the F8 key in Var:ProductName.
* **Batch verification**: The user performs a **Verify** batch task in the project (for example, in the Files view of Var:ProductName).

## Types of verification plug-ins

The verification framework supports three types of verification plug-ins:

### Bilingual verifiers

Bilingual verifiers are used to check documents of a particular file type for potential problems. For example, sheet names in MS Excel files must not be longer than 31 characters. If a sheet name exceeds this limit during translation, the intermediary (XLIFF) file cannot be converted back into a valid Excel target file. To check a translation for violations of this length limit, you can enhance the MS Excel filter with a bilingual verification plug-in that points out sheet name translations longer than 31 characters.

### Native verifiers

While bilingual verifiers work on the intermediary (XLIFF) document, native verifiers are applied to the native output format (for example, DOC, MIF, or XML). For example, you might create a component builder for a particular XML document type in which the length of a particular element must not exceed a certain number of characters. To check the validity of translated XML output files, you can enhance your component builder with a verification plug-in that checks whether the maximum length in the XML target file has been exceeded.

### Global verifiers

Global verifiers are not associated with a particular file type. Instead, they apply to any file type. By default, Var:ProductName is delivered with three global verifiers: **QA Checker**, **Terminology Verifier**, and **Tag Verifier**. These plug-ins are used to check intermediary (XLIFF) files for issues such as number and punctuation errors or failure to use the correct terminology. This means global verifiers are used to check bilingual documents for problems that are not specific to a particular file type. The verification framework also allows you to develop additional global verifiers to check documents for criteria not covered by the standard QA Checker and Terminology Verifier plug-ins.

## Custom message controls

The verification framework allows verification plug-ins to create verification messages that can be viewed in custom message controls.
