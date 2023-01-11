using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;


#nullable enable

namespace Microsoft.Fast.Components.FluentUI.Generators
{
    public class Metadata
    {
        public string? basefilename { get; set; }
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

    [Generator]
    public class FluentEmojiGenerator : ISourceGenerator
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "The whole purpose of this generator is to process directories...")]
        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder? sb = new();
            Regex? variantandtone = new(@"([\w|-]*)_([\w|-]*)");
            Regex? variant = new(@"([\w|-]*)");

            List<(string folder, string emojibase)> constants = new();

            int emojicount = 0;

            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.MSBuildProjectDirectory", out var projectDirectory);

            string basepath = Path.Combine(Directory.GetParent(projectDirectory).FullName, $"Microsoft.Fast.Components.FluentUI{Path.DirectorySeparatorChar}Assets{Path.DirectorySeparatorChar}emojis{Path.DirectorySeparatorChar}");

            sb.AppendLine($"#pragma warning disable CS1591");
            sb.AppendLine("using System.Collections.Generic;\r\n");
            sb.AppendLine("namespace Microsoft.Fast.Components.FluentUI;\r\n");
            sb.AppendLine("public static partial class FluentEmojis");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static IEnumerable<EmojiModel> EmojiMap = new EmojiModel[$emojicount$]");
            sb.AppendLine("\t{");

            foreach (string grouppath in Directory.EnumerateDirectories(basepath))
            {
                string group = Path.GetFileName(grouppath);

                foreach (string emojifolder in Directory.EnumerateDirectories(grouppath))
                {
                    string folder = Path.GetFileName(emojifolder);
                    bool hasTone = false;

                    Metadata? metadata = JsonSerializer.Deserialize<Metadata>(File.ReadAllText(Path.Combine(emojifolder, "metadata.json")));

                    string basefilename = metadata!.basefilename!;
                    string keywords = string.Join(",", metadata.keywords);

                    if (metadata.unicodeSkintones != null)
                    {
                        hasTone = true;
                    }

                    foreach (string filepath in Directory.EnumerateFiles(emojifolder, "*.svg"))
                    {
                        string file = Path.GetFileNameWithoutExtension(filepath).Substring(basefilename.Length + 1);

                        Match? match;

                        if (hasTone)
                            match = variantandtone.Match(file);
                        else
                            match = variant.Match(file);

                        string? emojiStyle = string.Empty;
                        string? emojiSkintone = string.Empty;

                        if (match.Success)
                        {

                            emojiStyle = match.Groups[1].Value;
                            emojiStyle = Regex.Replace(emojiStyle, @"(^|-)(\w)", m => m.Groups[2].Value.ToUpper());
                            if (hasTone)
                            {
                                emojiSkintone = match.Groups[2].Value;
                                emojiSkintone = Regex.Replace(emojiSkintone, @"(^|-)(\w)", m => m.Groups[2].Value.ToUpper());
                            }

                        }

                        if (!string.IsNullOrEmpty(emojiSkintone))
                        {
                            sb.AppendLine($"\t\tnew EmojiModel(\"{basefilename}\", \"{folder}\", EmojiGroup.{group}, \"{keywords}\", EmojiStyle.{emojiStyle}, EmojiSkintone.{emojiSkintone}),");

                        }
                        else
                        {
                            sb.AppendLine($"\t\tnew EmojiModel(\"{basefilename}\", \"{folder}\", EmojiGroup.{group}, \"{keywords}\", EmojiStyle.{emojiStyle}),");
                        }
                        emojicount++;
                    }


                    if (char.IsDigit(folder[0]))
                    {
                        folder = $"_{folder}";
                    }


                    constants.Add((folder.Replace("!", ""), basefilename));
                }
            }

            sb.AppendLine("\t};");
            sb.Replace("$emojicount$", emojicount.ToString());

            foreach ((string folder, string emojibase) in constants)
            {
                sb.AppendLine($"\tpublic const string {folder} = \"{emojibase}\";");
            }
            sb.AppendLine("}");

            context.AddSource($"FluentEmojis.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
#if DEBUG
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
#endif
        }
    }
}
