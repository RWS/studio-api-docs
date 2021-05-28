Introduction
===

While the first version of the File Type Support Framework, which was used in SDL Trados 2007, could only process native, monolingual files, the new version also supports bilingual documents such as TTX, ITD, and XLIFF.

About Bilingual File Type Plug-in
--
The processing of bilingual formats follows the same basic concept as the processing of monolingual (native) formats: the bilingual format is opened in <Var:ProductName>, and then converted to an intermediary format (e.g. SDLXliff). The intermediary document that is derived from the bilingual format is then further processed, e.g. it is translated, analyzed, etc. Afterwards, the intermediary file is converted back into the original bilingual format.

As the name suggests, bilingual files contain strings in more than one language. They usually have source-language segments and (possibly) segments in the target language. Bilingual files can thus be partly pre-translated documents.

Probably the most common example of this is the processing of TTX (TradosTag) and ITD files in <Var:ProductName>. TTX and ITD are the predecessor formats of SDLXliff, which are used in SDL Trados 2007 and SDLX 2007. For compatibility reasons, these formats are still supported in <Var:ProductName>. This allows users of <Var:ProductName> to e.g. open a partly-translated TTX file, finish translating it, and generate the fully translated TTX file, which can then be further processed in an SDL Trados 2007-based workflow.

![TTXandITD](images/TTXandITD.jpg)

The TTX and ITD file types as examples of bilingual file type plug-ins.

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.