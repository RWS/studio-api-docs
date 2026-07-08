# What is the Verification Framework?

The verification framework provides built-in verification services on top of the File Type Support Framework. It checks translated documents for quality and compliance issues.

Users can verify documents in three ways:

* **Interactive verification**: The system verifies each segment immediately when the user confirms it.
* **Document verification (on-demand verification)**: The system checks all target segments of the document when the user presses the F8 key in Var:ProductName.
* **Batch verification**: The system verifies documents when the user runs a **Verify** batch task in the project (for example, in the Files view of Var:ProductName).

The verification framework supports three types of verification plug-ins:

* **Bilingual verifiers**: These plug-ins check documents of a particular file type for problems in the intermediary (SDLXliff) format. For example, sheet names in Microsoft Excel files must not exceed 31 characters. If a sheet name exceeds this limit during translation, the SDLXliff file cannot convert back to a valid Excel target file. You can enhance the MS Excel filter with a bilingual verification plug-in to identify sheet name translations longer than 31 characters.
* **Native verifiers**: While bilingual verifiers work on SDLXliff documents, native verifiers check the native output format (for example, DOC, MIF, or XML). For instance, if you create a File Type Component Builder for an XML document type with specific element length constraints, you can add a verification plug-in to check whether translated XML output files exceed the maximum allowed length.
* **Global verifiers**: These plug-ins apply to any file type rather than a specific format. Var:ProductName includes two default global verifiers: **QA Checker** and **Terminology Verifier**. These check SDLXliff files for issues like number errors, punctuation inconsistencies, and incorrect terminology use. Global verifiers identify language-related problems that are not specific to a particular file type. You can develop additional global verifiers to check documents for criteria not covered by the standard QA Checker and Terminology Verifier.


## Reference

- [IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml)
- [INativeFileVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml)
- [IGlobalVerifier](../../api/verification/Sdl.Verification.Api.IGlobalVerifier.yml)

## See Also

- [How to Create a Native Verifier](create_a_native_verifier_introduction.md)
- [How to Create a Bilingual Verifier](create_a_bilingual_verifier_introduction.md)
- [How to Create a Global Verifier](global_verifier_introduction.md)
- [Verifying files](verifying_files.md)

>[!NOTE]
>
>This content may be out-of-date. Inspect the libraries in the Visual Studio Object Browser to verify the latest information on this topic.
