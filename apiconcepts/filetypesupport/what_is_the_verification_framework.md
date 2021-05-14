What is the Verification Framework?
==

The verification framework is the service build up on the top of the SDL File Type Support Framework. It is used for the verification of translated documents.

Users can verify documents in three different ways:

* Interactive verification: the segment is verified immediately when it is confirmed by the user.
* Document verification (on-demand verification): all target segments of the document are checked when the user presses the F8 key in <Var:ProductName>.
* Batch verification: when user perform a **Verify** batch task in the project (e.g. in the Files view of <Var:ProductName>).

The verification framework supports three types of verification plug-ins:

 * **Bilingual verifiers**: Bilingual verifiers are used to check documents of a particular file type for potential problems. Example: sheet names of MS Excel files must not be longer than 31 characters. If a sheet name exceeds this length limit during translation, the intermediary (SDL XLIFF) file cannot be converted back into a valid Excel target file. To check a translation for violations of this length limit, you can enhance the MS Excel filter with a bilingual verification plug-in that points out sheet name translations that are longer than 31 characters.
 * **Native verifiers**: While bilingual verifiers work on the intermediary (SDL XLIFF) document, native verifiers are applied to the native output format, e.g. DOC, MIF, XML, etc. Example: You have created a File Type Component Builder for a particular XML document type in which the length of a particular element must not exceed a certain number of characters. To check the validity of the translated XML output files, you can enhance your XML File Type Component  * Builder with a verification plug-in that checks whether the maximum length in the XML target file has been exceeded.
 * **Global verifiers**: Global verifiers are not associated with a particular file type. They rather apply to any file type. By default, SDL Trados Studio 2017 is delivered with two global verifiers: **QA Checker** and **Terminology Verifier**. These plug-ins are used to check any inermediary (SDL XLIFF) files for, e.g. number and punctuation errors, failure to use the correct terminology, etc. This means that global verifiers are used to check bilingual documents for problems that are not specific to a particular file type. The verification framework allows you to develop additional global verifiers to check documents for criteria that are not covered by the standard QA Checker and Terminology Verifier plug-ins.


See Also
--

**Reference**

[IBilingualVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.BilingualApi.IBilingualVerifier.yml)

[INativeFileVerifier](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.NativeApi.INativeFileVerifier.yml)

[IGlobalVerifier](../../api/verification/Sdl.Verification.Api.IGlobalVerifier.yml)

**Other Resources**

[How to Create a Native Verifier](create_a_native_verifier_introduction.md)

[How to Create a Bilingual Verifier](create_a_bilingual_verifier_introduction.md)

[How to Create a Global Verifier](global_verifier_introduction.md)

[Verifying files](verifying_files.md)

>**!NOTE**
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.