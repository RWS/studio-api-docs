using Docfx.Common;
using Docfx.Plugins;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.IO;
using System.Linq;
using System.Threading;

namespace TradosStudioDocsPlugin;

[Export(nameof(EnvironmentVariableProcessor), typeof(IPostProcessor))]
public class EnvironmentVariableProcessor : IPostProcessor
{
    public ImmutableDictionary<string, object> PrepareMetadata(ImmutableDictionary<string, object> metadata)
    {
        return metadata;
    }

    public Manifest Process(Manifest manifest, string outputFolder, CancellationToken cancellationToken = default)
    {
        if (outputFolder == null)
        {
            throw new ArgumentNullException(nameof(outputFolder), "Base directory can not be null");
        }
   
        foreach (var manifestItem in manifest.Files.Where(x => x.Type == "Conceptual"))
        {
            foreach (var manifestItemOutputFile in manifestItem.Output)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var outputPath = Path.Combine(outputFolder, manifestItemOutputFile.Value.RelativePath);

                var content = File.ReadAllText(outputPath);

                Logger.LogInfo($"Replacing environment variables in {outputPath}");

                var newContent = EnvironmentVariableUtil.ReplaceEnvironmentVariables(content);

                if (content == newContent)
                {
                    continue;
                }

                Logger.LogInfo($"Writing new content to {outputPath}");

                File.WriteAllText(outputPath, newContent);
            }
        }
        return manifest;
    }
}