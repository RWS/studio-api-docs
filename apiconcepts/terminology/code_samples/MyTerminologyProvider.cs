using Sdl.Terminology.TerminologyProvider.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace SDL_Terminology_Provider_Plug_in
{
    public class MyTerminologyProvider : AbstractTerminologyProvider
    {

        #region "EntryList"
        private List<IEntry> _entry = new List<IEntry>();
        #endregion

        #region "FileName"
        //Stores the glossary text file name and path
        public readonly string fileName;
        #endregion

        #region "ProviderSettings"
        //Sets the terminology provider settings, i.e. in our implementation the glossary file name
        public MyTerminologyProvider(string providerSettings)
        {
            fileName = providerSettings;
        }
        #endregion

        #region "Definition"
        //Creates the terminology source definition, i.e. the languages in the glossary and any
        //additional descriptive fields, in this case the 'Definition' field
        public override IDefinition Definition
        {
            get
            {
                return new Definition(GetDescriptiveFields(), GetLanguages().Cast<IDefinitionLanguage>().ToList());
            }
        }
        #endregion

        #region "DescriptiveFields"
        // Creates the termbase definition by declaring the Definition text field.
        // This allows our terminology provider to also display the Definition field content
        // in the Termbase Search and Terminology Recognition windows.
        public IList<IDescriptiveField> GetDescriptiveFields()
        {
            var result = new List<IDescriptiveField>();

            var definitionField = new DescriptiveField
            {
                Label = "Definition",
                Level = FieldLevel.EntryLevel,
                Type = FieldType.String
            };
            result.Add(definitionField);

            return result;
        }
        #endregion

        #region "NameDescrptionUri"
        //Return the terminology provider name, uri, and description to show in the Studio UI
        public override string Description
        {
            get
            {
                return PluginResources.My_Terminology_Provider_Description;
            }
        }

        public override string Name
        {
            get
            {
                return PluginResources.My_Terminology_Provider_Name;
            }
        }

        public override Uri Uri
        {
            get
            {
                return new Uri(fileName);
            }
        }
        #endregion

        #region "GetEntry"
        // Returns the entry for a search result. The entries are associated with the search results
        // through the entry id.
        public override IEntry GetEntry(int id)
        {
            return _entry.FirstOrDefault(_entry => _entry.Id == id);
        }
        #endregion

        #region GetEntryExt"
        public override IEntry GetEntry(int id, IEnumerable<ILanguage> languages)
        {
            return _entry.FirstOrDefault(_entry => _entry.Id == id);
        }
        #endregion

        #region "GetLanguages"
        //We parse the first line of the glossary text file to retrieve the the source and target language.
        //Then we create two language objects that we return as source and target language to populate the
        //termbase language dropdown lists in Studio.
        public override IList<ILanguage> GetLanguages()
        {
            StreamReader _inFile = new StreamReader(fileName.Replace("file:///", ""));
            string[] languages = _inFile.ReadLine().Split(';');
            string srgLanguage = languages[1], trgLanguage = languages[2];
            string srcLabel = srgLanguage.Split(',')[0], srcLocale = srgLanguage.Split(',')[1];
            string trgLabel = trgLanguage.Split(',')[0], trgLocale = trgLanguage.Split(',')[1];
            _inFile.Close();

            var result = new List<IDefinitionLanguage>();

            var tbSrcLanguage = new DefinitionLanguage
            {
                Name = srcLabel,
                Locale = new System.Globalization.CultureInfo(srcLocale)
            };

            var tbTrgLanguage = new DefinitionLanguage
            {
                Name = trgLabel,
                Locale = new System.Globalization.CultureInfo(trgLocale)
            };


            result.Add(tbSrcLanguage);
            result.Add(tbTrgLanguage);

            return result.Cast<ILanguage>().ToList();
        }
        #endregion


        #region "Search"
        //Is executed when the user launches a lookup operation in the Termbase Search window, 
        //or when the user moves to a segment in the Editor of Studio. Moving to a segment
        //automatically launches a fuzzy search to retrieve any known terminology.
        public override IList<ISearchResult> Search(string text, ILanguage source, ILanguage destination, int maxResultsCount, SearchMode mode, bool targetRequired)
        {
            string[] chunks;
            List<string> hits = new List<string>();

            // open the glossary text file
            using (StreamReader glossary = new StreamReader(fileName.Replace("file:///", "")))
            {
                // skip the first line, as it contains only the language settings
                glossary.ReadLine();

                while (!glossary.EndOfStream)
                {
                    string thisLine = glossary.ReadLine();
                    if (thisLine.Trim() == "")
                        continue;
                    chunks = thisLine.Split(';');
                    string sourceTerm = chunks[1].ToLower();

                    // normal search (triggered from the Termbase Search window)
                    if (mode.ToString() == "Normal" && sourceTerm.StartsWith(text.ToLower()))
                        hits.Add(thisLine);

                    // fuzzy search (corresponds to the Terminology Eecognition)
                    if (mode.ToString() == "Fuzzy" && text.ToLower().Contains(sourceTerm))
                        hits.Add(thisLine);
                }
            }
    
            // Create search results object (hitlist)
            var results = new List<ISearchResult>();

            for (int i = 0; i < hits.Count; i++)
            {
                chunks = hits[i].Split(';');
                // We create the search result object based on the source term
                // found in the glossary, we assign the id, which associates the search
                // result to the correspoinding entry, and we assume that the search score 
                // is always 100%.
                SearchResult result = new SearchResult
                {
                    Text = chunks[1], // source term
                    Score = 100,
                    Id = Convert.ToInt32(chunks[0]) // entry id
                };

                // Construct the entry object for the current search result
                _entry.Add(CreateEntry(chunks[0], chunks[1], chunks[2], chunks[3], destination.Name));

                results.Add(result);
            }
            return results;            
        }
        #endregion

        #region "ConstructEntryContent"
        // This helper function is used to construct the entry content with entry id, source and target term, and
        // definition (if applicable)
        private IEntry CreateEntry(string id, string sourceTerm, string targetTerm, string definitionText, string targetLanguage)
        {
            // Assign the entry id
            IEntry thisEntry = new Entry
            {
                Id = Convert.ToInt32(id)
            };

            // Add the target language
            IEntryLanguage trgLanguage = new EntryLanguage
            {
                Name = targetLanguage
            };

            // Create the target term
            IEntryTerm _term = new EntryTerm
            {
                Value = targetTerm
            };
            trgLanguage.Terms.Add(_term);
            thisEntry.Languages.Add(trgLanguage);

            // Also add the definition (if available)
            if (definitionText != "")
            { 
                IEntryField _definition = new EntryField
                {
                    Name = "Definition",
                    Value = definitionText
                };
                thisEntry.Fields.Add(_definition);
            }            

            return thisEntry;
        }
        #endregion
    }
}
