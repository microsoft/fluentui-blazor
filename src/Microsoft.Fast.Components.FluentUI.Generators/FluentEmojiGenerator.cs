using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

#nullable enable

namespace Microsoft.Fast.Components.FluentUI.Generators
{
#pragma warning disable IDE1006 // Naming Styles
    public class Metadata
    {
        public string? cldr { get; set; }
        public string? fromVersion { get; set; }
        public string? glyph { get; set; }
        public string[]? glyphAsUtfInEmoticons { get; set; }
        public string? group { get; set; }
        public string[]? keywords { get; set; }
        public string[]? mappedToEmoticons { get; set; }
        public string? tts { get; set; }
        public string? unicode { get; set; }
        public string[]? unicodeSkintones { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles

    [Generator]
    [SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "The whole purpose of this generator is to process directories...")]
    public class FluentEmojiGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var emojiPathValueProvider = context.AnalyzerConfigOptionsProvider
               .Select((provider, _) => GetFilePathFromOptions(provider.GlobalOptions));

            context.RegisterSourceOutput(emojiPathValueProvider, GenerateSource);
        }

        public string GetFilePathFromOptions(AnalyzerConfigOptions options)
        {
            string? baseFolder;
            bool getResult = options.TryGetValue("build_property.FluentUISourceBaseFolder", out string? sourceFolder);
            if (!getResult || (string.IsNullOrEmpty(sourceFolder)))
            {
                options.TryGetValue("build_property.MSBuildProjectDirectory", out string? projectDirectory);
                baseFolder = Directory.GetParent(projectDirectory).FullName;
            }
            else
            {
                baseFolder = $"{sourceFolder}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}";
            }

            return Path.Combine(baseFolder, $"Microsoft.Fast.Components.FluentUI{Path.DirectorySeparatorChar}Assets{Path.DirectorySeparatorChar}emojis{Path.DirectorySeparatorChar}");
        }

        public void GenerateSource(SourceProductionContext context, string basePath)
        {
            StringBuilder? sb = new();
            Regex? variantandtone = new(@"([\w]*)_([\w]*)");
            //Regex? variant = new(@"([\w]*)");

            List<(string folder, string emojibase)> constants = new();

            int emojicount = 0;



            sb.AppendLine($"#pragma warning disable CS1591");
            sb.AppendLine("using System.Collections.Generic;\r\n");
            sb.AppendLine("namespace Microsoft.Fast.Components.FluentUI;\r\n");
            sb.AppendLine("public partial class FluentEmojis");
            sb.AppendLine("{");
            sb.AppendLine("\tprivate static IEnumerable<EmojiModel> FullEmojiMap = new EmojiModel[$emojicount$]");
            sb.AppendLine("\t{");

            foreach (string grouppath in Directory.EnumerateDirectories(basePath))
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    return;
                }
                string group = Path.GetFileName(grouppath);

                foreach (string emojifolder in Directory.EnumerateDirectories(grouppath))
                {
                    string folder = Path.GetFileName(emojifolder);
                    bool hasTone = false;

                    Metadata? metadata = JsonSerializer.Deserialize<Metadata>(File.ReadAllText(Path.Combine(emojifolder, "metadata.json")));

                    //string basefilename = metadata!.basefilename!;
                    //string shortenedBaseFilename = basefilename.Substring(0, Math.Min(basefilename.Length, maxnamelength));
                    string keywords = string.Join(",", metadata?.keywords);

                    if (metadata?.unicodeSkintones != null)
                        hasTone = true;

                    Match? match;
                    foreach (string filepath in Directory.EnumerateFiles(emojifolder, "*.svg"))
                    {
                        string file = Path.GetFileNameWithoutExtension(filepath);
                        string? shortEmojiStyle = string.Empty;
                        string? shortEmojiSkintone = string.Empty;

                        if (hasTone)
                        {
                            match = variantandtone.Match(file);
                            if (match.Success)
                            {
                                shortEmojiSkintone = match.Groups[1].Value;
                                shortEmojiStyle = match.Groups[2].Value;

                            }
                        }
                        else
                            shortEmojiStyle = file;

                        string emojiStyle = shortEmojiStyle switch
                        {
                            "c" => "Color",
                            "f" => "Flat",
                            "h" => "HighContrast",
                            _ => ""
                        };

                        string emojiSkintone = shortEmojiSkintone switch
                        {
                            "de" => "Default",
                            "li" => "Light",
                            "ml" => "MediumLight",
                            "me" => "Medium",
                            "md" => "MediumDark",
                            "da" => "Dark",
                            _ => ""
                        };

                        if (hasTone)
                        {
                            sb.AppendLine($"\t\tnew EmojiModel(\"{folder}\", EmojiGroup.{group}, \"{keywords}\", EmojiStyle.{emojiStyle}, EmojiSkintone.{emojiSkintone}),");
                        }
                        else
                        {
                            sb.AppendLine($"\t\tnew EmojiModel(\"{folder}\", EmojiGroup.{group}, \"{keywords}\", EmojiStyle.{emojiStyle}),");
                        }
                        emojicount++;
                    }


                    if (char.IsDigit(folder[0]))
                    {
                        folder = $"_{folder}";
                    }


                    constants.Add((folder.Replace("!", ""), folder));
                }
            }

            sb.AppendLine("\t};");
            sb.Replace("$emojicount$", emojicount.ToString());

            foreach ((string name, string folder) in constants)
            {
                sb.AppendLine($"\tpublic const string {name} = \"{folder}\";");
            }
            sb.AppendLine("}");

            context.AddSource($"FluentEmojis.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }
    }
}
