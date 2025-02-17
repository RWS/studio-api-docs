Implementing the Element Visitor
====
In the previous chapter (see [Implementing the Search Logic](implementing_the_search_logic.md)) we generate the search result based on the search segment / string from the document that is currently open in the editor of Var:ProductName. To make certain that the search string only consists of plain text, we add a helper class called `ListTranslationProviderElementVisitor`. ( This class is not included in the plug-in template.) This component is used to loop through all the elements of a given segment (e.g. text, inline tags, etc), and returns the plain source segment string that will be used for the search and matching to the segments from the delimited list.

The class needs to implement the [ISegmentElementVisitor](../../api/translationmemory/Sdl.LanguagePlatform.Core.ISegmentElementVisitor.yml) interface.

The complete class looks as shown below:

# [C#](#tab/tabid-1)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.LanguagePlatform.Core;

namespace Sdk.LanguagePlatform.Samples.ListProvider
{
    class ListTranslationProviderElementVisitor : ISegmentElementVisitor
    {
        private ListTranslationOptions _options;
        private string _plainText;

        public string PlainText
        {
            get 
            {
                if (_plainText == null)
                {
                    _plainText = "";
                }
                return _plainText;
            }
            set 
            {
                _plainText = value;
            }
        }

        public void Reset()
        {
            _plainText = "";
        }

        public ListTranslationProviderElementVisitor(ListTranslationOptions options)
        {
            _options = options;
        }

        #region ISegmentElementVisitor Members

        public void VisitDateTimeToken(Sdl.LanguagePlatform.Core.Tokenization.DateTimeToken token)
        {
            _plainText += token.Text;
        }

        public void VisitMeasureToken(Sdl.LanguagePlatform.Core.Tokenization.MeasureToken token)
        {
            _plainText += token.Text;
        }

        public void VisitNumberToken(Sdl.LanguagePlatform.Core.Tokenization.NumberToken token)
        {
            _plainText += token.Text;
        }

        public void VisitSimpleToken(Sdl.LanguagePlatform.Core.Tokenization.SimpleToken token)
        {
            _plainText += token.Text;
        }

        public void VisitTag(Tag tag)
        {
            _plainText += tag.TextEquivalent;
        }

        public void VisitTagToken(Sdl.LanguagePlatform.Core.Tokenization.TagToken token)
        {
            _plainText += token.Text;
        }

        public void VisitText(Text text)
        {
            _plainText += text;
        }

        #endregion
    }
}
```
***

See Also
----
[Implementing the Search Logic](implementing_the_search_logic.md)
