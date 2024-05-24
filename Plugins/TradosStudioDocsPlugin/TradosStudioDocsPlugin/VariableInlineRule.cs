using Microsoft.DocAsCode.MarkdownLite;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TradosStudioDocsPlugin
{
    public class VariableInlineRule : IMarkdownRule
    {
        public string Name => "VariableToken";

        // define my regex to match
        //    private static readonly Regex _envVarRegex = new Regex(@"^\<Var:(\w+?)\>", RegexOptions.IgnoreCase);

        public IMarkdownToken TryMatch(IMarkdownParser parser, IMarkdownParsingContext context)
        {
            var _envVarRegex = new Regex(@"^\<Var:(\w+?)\>", RegexOptions.IgnoreCase);
            var match = _envVarRegex.Match(context.CurrentMarkdown);
            if (match.Length == 0)
            {
                return null;
            }

            var envVar = match.Groups[1].Value;
            string text = GetValueForVariable(envVar);
            if (text == null)
            {
                return null;
            }

            // 'eat' the characters of the current markdown token so they anr
            var sourceInfo = context.Consume(match.Length);
            return new MarkdownTextToken(this, parser.Context, text, sourceInfo);
        }

        private static string GetValueForVariable(string envVar)
        {

            var replacementValues = new Dictionary<string, string>
            {
                { "ProductName", "Trados Studio" },
                { "ProductNameWithEdition", "Trados Studio 2024" },
                { "ProductVersion", "Studio18" },
                { "VersionNumber", "18" },
                { "VisualStudioEdition", "Microsoft Visual Studio 2022" },
                { "PluginPackedPath", "%AppData%\\SDL\\SDL Trados Studio\\18\\Plugins\\Packages\\" },
                { "PluginUnpackedPath", "%AppData%\\SDL\\SDL Trados Studio\\18\\Plugins\\Unpacked\\" },
                { "InstallationFolder", "C:\\Program Files\\SDL\\SDL Trados Studio\\Studio18" },
                { "AppSigningEmail", "app-signing@sdl.com" }
            };

            return Environment.GetEnvironmentVariable(envVar) ??
                (replacementValues.ContainsKey(envVar) ? replacementValues[envVar] : null);

        }
    }
}
