# Creating a File-based Termbase

This article shows how to create a file-based termbase (`.ttb`) programmatically.

## Create the Class

Create a class named `FileBasedTermbaseCreator`.

Add a public asynchronous method named `CreateFileBasedTermbaseAsync()`.

This method accepts the following string parameters:

* `folderPath` - folder where the termbase file will be created
* `fileName` - name of the termbase file

Call the method as follows:

```csharp
var creator = new FileBasedTermbaseCreator();
await creator.CreateFileBasedTermbaseAsync(@"C:\MyTermbases", "MyTermbase.ttb");
```

## Create the File-based Termbase

Create a [CreateTermbaseRequest](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.CreateTermbaseRequest.yml) object and pass it to the `CreateTermbaseAsync()` method of the [TerminologyProviderManager](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.TerminologyProviderManager.yml) class.

`CreateTermbaseRequest` includes the following properties:

* **Name** - termbase name
* **Description** - termbase description
* **Path** - folder where the termbase file will be created
* **Languages** - list of languages to include in the termbase

Each language uses a [DefinitionLanguage](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.DefinitionLanguage.yml) object with these properties:

* **Locale** - locale code of the language, for example `en` or `de`
* **Name** - display name of the language
* **IsBidirectional** - indicates whether the language is bidirectional
* **TargetOnly** - indicates whether the language is target-only

`CreateTermbaseAsync()` returns an [OperationResult](../../api/terminology/Sdl.Terminology.TerminologyProvider.Core.OperationResult.yml) object. Check `IsSuccess` to confirm that the operation succeeded. If the operation fails, inspect `Error` for details.

The following example creates a file-based termbase with English and German:

```csharp
public async Task CreateFileBasedTermbaseAsync(string folderPath, string fileName)
{
    var request = new CreateTermbaseRequest
    {
        Name = fileName,
        Description = "My termbase description",
        Path = folderPath,
        Languages = new List<DefinitionLanguage>
        {
            new DefinitionLanguage
            {
                Locale = "en",
                Name = "English",
                IsBidirectional = true,
                TargetOnly = false
            },
            new DefinitionLanguage
            {
                Locale = "de",
                Name = "German",
                IsBidirectional = true,
                TargetOnly = false
            }
        }
    };

    OperationResult result = await TerminologyProviderManager.Instance.CreateTermbaseAsync(request);

    if (!result.IsSuccess)
    {
        // Handle result.Error.
    }
}
```

## See Also

* [Managing Terms in a File-based Termbase](managing_terms_in_a_file_based_termbase.md)
