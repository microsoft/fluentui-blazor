using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;


#nullable enable

namespace Microsoft.Fast.Components.FluentUI.Generators
{
    [Generator]
    public class FluentIconGenerator : ISourceGenerator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "The whole purpose of this generator is to process directories...")]
        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder? sb = new();
            Regex? regex = new(@"(\w*)_(\d*)_(\w*)");

            List<(string folder, string iconbase)> constants = new();
            (string name, int size, string variant) icon;
            int iconcount = 0;

            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.MSBuildProjectDirectory", out var projectDirectory);

            string iconsFolder = Path.Combine(Directory.GetParent(projectDirectory).FullName, $"Microsoft.Fast.Components.FluentUI{Path.DirectorySeparatorChar}Assets{Path.DirectorySeparatorChar}icons{Path.DirectorySeparatorChar}");

            sb.AppendLine($"#pragma warning disable CS1591");
            sb.AppendLine("using System.Collections.Generic;\r\n");
            sb.AppendLine("namespace Microsoft.Fast.Components.FluentUI;\r\n");
            sb.AppendLine("public static partial class FluentIcons");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static IEnumerable<IconModel> IconMap = new IconModel[$iconcount$]");
            sb.AppendLine("\t{");


            foreach (string foldername in Directory.EnumerateDirectories(iconsFolder))
            {
                string folder = foldername.Substring(foldername.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                string iconbase = string.Empty;

                foreach (string file in Directory.EnumerateFiles(foldername, "*.svg"))
                {
                    string name = Path.GetFileNameWithoutExtension(file);

                    MatchCollection? matches = regex.Matches(name);
                    if (matches.Count == 0)
                        continue;

                    icon = (
                        matches[0].Groups[1].Value,
                        int.Parse(matches[0].Groups[2].Value),
                        ToSentenceCase(matches[0].Groups[3].Value)
                    );

                    sb.AppendLine($"\t\tnew IconModel(\"{icon.name}\", \"{folder}\", IconSize.Size{icon.size}, IconVariant.{icon.variant}),");
                    iconcount++;

                    if (string.IsNullOrEmpty(iconbase))
                    {
                        iconbase = icon.name;
                    }
                }

                constants.Add((folder, iconbase));

            }
            sb.AppendLine("\t};");

            sb.Replace("$iconcount$", iconcount.ToString());

            foreach ((string folder, string iconbase) in constants)
            {
                sb.AppendLine($"\tpublic const string {folder} = \"{iconbase}\";");
            }
            sb.AppendLine("}");

            context.AddSource($"FluentIcons.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
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

        static string ToSentenceCase(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
