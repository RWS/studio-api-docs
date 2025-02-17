using Docfx.Common;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TradosStudioDocsPlugin;

public partial class EnvironmentVariableUtil
{
        [GeneratedRegex(@"Var\:(\w+)", RegexOptions.Compiled)]
    private static partial Regex EnvVarRegex();

    public static string ReplaceEnvironmentVariables(string sourceText)
    {
        var matches = EnvVarRegex().Matches(sourceText);

        if (matches.Count > 0)
        {
            var sb = new StringBuilder(sourceText.Length);

            int lastMatchEnd = 0;

            foreach (Match match in matches)
            {
                var envVar = match.Groups[1].Value;

                // Append the prefix that didn't match the regex (or text since last match)
                sb.Append(sourceText.AsSpan(lastMatchEnd, match.Index - lastMatchEnd));

                // Do the replacement of the regex match
                string envVarValue = Environment.GetEnvironmentVariable(envVar) ?? string.Empty;
                sb.Append(envVarValue);

                Logger.LogInfo($"Replaced environment variable '{envVar}' with value '{envVarValue}' on match {match.Value}");
                lastMatchEnd = match.Index + match.Length;
            }

            // Append the suffix that didn't match the regex
            sb.Append(sourceText.AsSpan(lastMatchEnd));

            return sb.ToString();
        }

        return sourceText;
    }
}