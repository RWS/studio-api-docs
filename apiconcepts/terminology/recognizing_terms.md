# Recognizing Terms in Text

This article explains how to find the termbase terms that occur in a text, such as a source segment, with the same terms and scores as the **Term Recognition** window in **Var:ProductName**.

Use term recognition when your plugin *consumes* terminology. For example:

* A translation provider sends the recognized terms and their translations to a machine translation or AI service, so the translation uses the approved terminology.
* A verification plugin checks that the translation uses the target terms of the source terms in the segment.

> [!NOTE]
> The term recognition API (`TerminologyRecognition`) is available from Trados Studio 2026 CU1. To support earlier versions as well, see [Supporting earlier versions](#supporting-earlier-versions).

## Searching versus recognizing terms

The terminology API offers two ways to find terms. They answer different questions.

| | `ITerminologyProvider.Search` | `TerminologyRecognition.RecognizeTerms` |
|---|---|---|
| Question it answers | Which entries in this termbase match this text? | Which terms occur in this text, and where? |
| Comparable window | **Termbase Search** | **Term Recognition** |
| Termbases | One termbase. | One or more termbases, in a search order. |
| Score | Not meaningful for file-based termbases. Use `Search` to look up entries. | The score of each term against the part of the text it matches, as shown in the **Term Recognition** window. |
| Inflected and compound forms | Returned as candidates, without a score. | Recognized and scored. For example, *USB cables* matches the term *USB cable*. |
| Position in the text | Not returned. | Returned for each term. |

If your plugin needs the terms of a segment, use `TerminologyRecognition`. Use `Search` to look up entries, for example in a search box.

## Recognizing terms in a segment

The term recognition types are in the `Sdl.Terminology.TerminologyProvider.Core.Recognition` namespace of the `Sdl.Terminology.TerminologyProvider.Core` assembly.

1. Create a `TermRecognitionRequest` with the text, the source and target languages, and the URIs of the termbases to search.
1. Call `RecognizeTerms` on a `TerminologyRecognition` instance.

```cs
using System;
using System.Collections.Generic;
using Sdl.Core.Globalization;
using Sdl.Terminology.TerminologyProvider.Core;
using Sdl.Terminology.TerminologyProvider.Core.Recognition;

public List<RecognizedTerm> RecognizeTerms(string sourceText)
{
    var request = new TermRecognitionRequest
    {
        Text = sourceText,
        SourceLanguage = new DefinitionLanguage { Locale = new CultureCode("en-US") },
        TargetLanguage = new DefinitionLanguage { Locale = new CultureCode("de-DE") }
    };
    request.Termbases.Add(new Uri("ttb.file:///C:/Termbases/Printer.ttb"));

    return new TerminologyRecognition().RecognizeTerms(request);
}
```

For example, take this segment from *SamplePhotoPrinter.doc* and the *Printer* termbase, both from the **Var:ProductName** sample project:

*When connecting power or USB cables, keep the cables clear of the paper path to the front and rear of the photo printer.*

`RecognizeTerms` returns two terms, each with its position in the text and its termbase entry:

* *photo printer*, an exact match with a score of 100. Its German translation is *Fotodrucker*.
* *USB cable*, matched to the plural *USB cables* with a score below 100. Its German translation is *USB-Kabel*.

The default settings of a request match the default termbase settings of a new project. You only need to set the text, the languages and the termbases.

## Choosing the languages and termbase indexes

A termbase stores each language in an *index*, such as *English* or *German*. `RecognizeTerms` finds the source and target index of each termbase from `SourceLanguage` and `TargetLanguage`. You pass these as `ILanguage` objects; `DefinitionLanguage` is a ready-made implementation.

* **Locale**: the language of the text. `RecognizeTerms` looks for an index with the same culture, for example `de-DE`, and then for an index with the same language, for example `de`.
* **Name** (optional): the name of a termbase index, for example *German*. When a termbase has an index with this name, it is used before the culture is matched. Set it when you follow the language index mapping of a project, as described in [Matching the project settings](#matching-the-project-settings).

`RecognizeTerms` skips a termbase that has no index for the source language. When `TargetRequired` is `true`, it also skips a termbase that has no index for the target language.

## Setting the recognition options

All the options have defaults, so set only the ones you need.

| Property | Default | Description |
|---|---|---|
| `MinimumMatchValue` | 70 | The minimum score, from 0 to 100, that a term must have to be recognized. |
| `SearchDepth` | 200 | The maximum number of candidate terms each termbase returns before scoring. |
| `SearchOrder` | `Sequential` | How several termbases are searched. `Hierarchical`: only the terms of the first termbase that returns results. `Sequential`: the terms of all termbases. `Parallel`: currently the same as `Sequential`. |
| `AllowOverlappingTerms` | `false` | Whether recognized terms can overlap in the text, for example *power cable* and *AC power cable*. |
| `RepetitionThreshold` | 10 | The maximum number of times the same term is recognized in the text. |
| `EnableTwoLetterTermRecognition` | `false` | Whether terms of two letters are recognized. |
| `MatchCase` | `false` | Whether the case of a term must match the text. |
| `TargetRequired` | `false` | Whether only termbases with an index for the target language are searched, and only terms with a translation are returned. |
| `IncludeHomonyms` | `true` | Whether terms with the same text from different entries or termbases are all returned. See [Homonyms and synonyms](#homonyms-and-synonyms). |

## Reading the results

Each `RecognizedTerm` contains:

| Property | Description |
|---|---|
| `Text` | The term, as it is in the termbase. |
| `MatchedText` | The part of the searched text that matches the term, for example *USB cables* for the term *USB cable*. |
| `Score` | The score of the term against the matched text, from 0 to 100. This is the score shown in the **Term Recognition** window. |
| `Positions` | The start (zero-based) and length of each place where the term occurs in the text. |
| `Entry` | The termbase entry, with the terms and fields in all its languages. |
| `EntryId`, `TermbaseUri`, `TermbaseName` | The entry and the termbase the term comes from. |
| `SourceLanguage`, `TargetLanguage` | The termbase indexes that were used. `TargetLanguage` is `null` when the termbase has no index for the target language. |

### Getting the translations

The translations of a term are the terms of its entry in the target index:

```cs
using System.Linq;

foreach (RecognizedTerm term in terms)
{
    EntryLanguage target = term.Entry?.Languages?
        .FirstOrDefault(language => language.Name == term.TargetLanguage?.Name);

    if (target is null)
    {
        // The entry has no translation in the target language.
        continue;
    }

    foreach (EntryTerm translation in target.Terms)
    {
        EntryField status = translation.Fields?.FirstOrDefault(field => field.Name == "Status");
        Console.WriteLine($"{term.MatchedText} -> {translation.Value} ({status?.Value}, score {term.Score})");
    }
}
```

The fields of a term, such as *Status*, depend on the termbase definition. Use them, for example, to prefer *Preferred* translations and to avoid *Forbidden* ones.

### Homonyms and synonyms

* **Synonyms** are several terms of the *same* entry, for example two translations of one concept. They are all in `target.Terms`. Prefer the one with the best status.
* **Homonyms** are terms with the *same text* in *different* entries, for example *machine distributor* as a dealer and as an electrical distribution unit. `RecognizeTerms` returns one result for each entry. Tell them apart by `TermbaseUri` and `EntryId`, and choose by context.

The same entry can be returned more than once, for example when its term occurs at several positions. To get one result per entry, group the results by `TermbaseUri` and `EntryId`.

When you send terms to a machine translation or AI service, keep the entries apart, for example by numbering them. Otherwise, the service can't tell the translations of two meanings from two translations of one meaning.

## Matching the project settings

To get exactly the terms of the **Term Recognition** window, use the termbases and settings of the project. The project stores them in two places:

* `TermbaseConfiguration`: the termbases, the language index mapping, and the main recognition options.
* The `TermRecognitionSettings` settings group: the other recognition options.

```cs
using System;
using System.Linq;
using Sdl.Core.Globalization;
using Sdl.Core.Settings;
using Sdl.ProjectAutomation.Core;
using Sdl.ProjectAutomation.FileBased;
using Sdl.Terminology.TerminologyProvider.Core;
using Sdl.Terminology.TerminologyProvider.Core.Recognition;

public TermRecognitionRequest CreateRequest(FileBasedProject project, string sourceText,
    CultureCode sourceLanguage, CultureCode targetLanguage)
{
    TermbaseConfiguration configuration = project.GetTermbaseConfiguration();
    TermRecognitionOptions options = configuration.TermRecognitionOptions;
    ISettingsGroup settings = project.GetSettings().GetSettingsGroup("TermRecognitionSettings");

    var request = new TermRecognitionRequest
    {
        Text = sourceText,
        SourceLanguage = new DefinitionLanguage
        {
            Name = GetIndexName(configuration, sourceLanguage),
            Locale = sourceLanguage
        },
        TargetLanguage = new DefinitionLanguage
        {
            Name = GetIndexName(configuration, targetLanguage),
            Locale = targetLanguage
        },
        MinimumMatchValue = options.MinimumMatchValue,
        SearchDepth = options.SearchDepth,
        SearchOrder = (TermRecognitionSearchOrder)options.SearchOrder,
        AllowOverlappingTerms = GetSetting(settings, "AllowOverlappingTerms", false),
        RepetitionThreshold = GetSetting(settings, "RepetitionThreshold", TermRecognitionRequest.DefaultRepetitionThreshold),
        EnableTwoLetterTermRecognition = GetSetting(settings, "EnableTwoLetterTermRecognition", false),
        MatchCase = GetSetting(settings, "MatchCaseTerms", false)
    };

    // The enabled termbases, in the project's order.
    foreach (LocalTermbase termbase in configuration.Termbases.OfType<LocalTermbase>().Where(termbase => termbase.Enabled))
    {
        request.Termbases.Add(new Uri("ttb." + new Uri(termbase.FilePath).AbsoluteUri));
    }

    return request;
}

// The termbase index that the project maps to a language, for example "German" for de-DE.
private static string GetIndexName(TermbaseConfiguration configuration, CultureCode language)
{
    return configuration.LanguageIndexes
        .FirstOrDefault(index => string.Equals(index.ProjectLanguage.CultureInfo.Name, language.Name, StringComparison.OrdinalIgnoreCase))?
        .TermbaseIndex;
}

private static T GetSetting<T>(ISettingsGroup settings, string settingId, T defaultValue)
{
    return settings != null && settings.ContainsSetting(settingId)
        ? settings.GetSetting<T>(settingId).Value
        : defaultValue;
}
```

The project's language index mapping applies to all its termbases, as in the **Term Recognition** window. If a termbase has no index with the mapped name, `RecognizeTerms` matches the index by language.

> [!NOTE]
> The example adds the file-based termbases of the project. The project settings describe a server-based termbase by its server and name, not by a termbase URI. For a server-based termbase, use the URI of its terminology provider, for example `ITerminologyProvider.Uri`.

In a plugin that runs in **Var:ProductName**, get the current project from the `ProjectsController`:

```cs
FileBasedProject project = SdlTradosStudio.Application.GetController<ProjectsController>().CurrentProject;
```

## Performance and behavior

* `RecognizeTerms` searches the termbases and scores the candidates each time you call it. Call it once per segment, and reuse the results.
* Calls are processed one at a time. Parallel calls, for example from a batch task, wait for each other.
* Read the project settings once, for example per batch or per document, rather than for each segment.
* A termbase that can't be opened is logged and skipped, so the other termbases still return their terms.
* An empty text returns no terms. A `null` request throws an `ArgumentNullException`.

## Supporting earlier versions

`TerminologyRecognition` is available from Trados Studio 2026 CU1. You have two options:

* **Require Trados Studio 2026 CU1.** Set the minimum version in your plugin manifest, for example `<RequiredProduct name="TradosStudio" minversion="19.0.1" maxversion="19.0.9"/>`.
* **Support earlier versions as well.** Check at run time whether the API is there, and use another way to find terms, such as `ITerminologyProvider.Search`, when it isn't:

```cs
private static readonly bool IsTermRecognitionAvailable =
    typeof(ITerminologyProviderManager).Assembly
        .GetType("Sdl.Terminology.TerminologyProvider.Core.Recognition.TerminologyRecognition") != null;
```

Keep all the code that uses the term recognition types in a separate class, and only use that class when `IsTermRecognitionAvailable` is `true`. On an earlier version these types don't exist, so .NET fails when it compiles a method that uses them, even if that code never runs.

## See Also

* [Searching Terms](searching_terms.md)
* [Managing Terms in a File-based Termbase](managing_terms_in_a_file_based_termbase.md)
