using Microsoft.DocAsCode.Dfm;
using System.Collections.Generic;
using System.Composition;

namespace TradosStudioDocsPlugin
{
    /// <summary>
    /// Exports our custom markdown parser via MEF to DocFx
    /// </summary>
    [Export(typeof(IDfmEngineCustomizer))]
    public class TradosStudioDocsDfmEngineCustomizer : IDfmEngineCustomizer
    {
        public void Customize(DfmEngineBuilder builder, IReadOnlyDictionary<string, object> parameters)
        {
            // insert inline rule at the top
            builder.InlineRules = builder.InlineRules.Insert(0, new VariableInlineRule());
        }
    }
}
