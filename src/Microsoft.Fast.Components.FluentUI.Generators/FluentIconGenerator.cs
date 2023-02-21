using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;


#nullable enable

namespace Microsoft.Fast.Components.FluentUI.Generators
{
    [Generator]
    public class FluentIconGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Debugger.Launch();
            // No initialization required for this one
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "The whole purpose of this generator is to process directories...")]
        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder? sb = new();
            int iconcount = 0;
            string? baseFolder;


            List<(string folder, string iconbase)> constants = new();

            bool getResult = context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.FluentUISourceBaseFolder", out string? sourceFolder);
            if (!getResult || string.IsNullOrEmpty(sourceFolder))
            {
                context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.MSBuildProjectDirectory", out string? projectDirectory);
                baseFolder = Directory.GetParent(projectDirectory).FullName;
            }
            else
            {
                baseFolder = $"{sourceFolder}{Path.DirectorySeparatorChar}src{Path.DirectorySeparatorChar}";
            }

            string iconsFolder = Path.Combine(baseFolder, $"Microsoft.Fast.Components.FluentUI{Path.DirectorySeparatorChar}Assets{Path.DirectorySeparatorChar}icons{Path.DirectorySeparatorChar}");

            sb.AppendLine($"#pragma warning disable CS1591");
            sb.AppendLine("using System.Collections.Generic;\r\n");
            sb.AppendLine("namespace Microsoft.Fast.Components.FluentUI;\r\n");
            sb.AppendLine("public partial class FluentIcons");
            sb.AppendLine("{");
            sb.AppendLine("\tprivate static IEnumerable<IconModel> FullIconMap = new IconModel[$iconcount$]");
            sb.AppendLine("\t{");


            foreach (string foldername in Directory.EnumerateDirectories(iconsFolder))
            {
                string folder = foldername.Substring(foldername.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                string iconbase = string.Empty;

                foreach (string file in Directory.EnumerateFiles(foldername, "*.svg"))
                {
                    string[] nameparts = Path.GetFileNameWithoutExtension(file).Split('_');

                    string iconVariant = nameparts[1] switch
                    {
                        "f" => "Filled",
                        "r" => "Regular",
                        _ => ""
                    };

                    sb.AppendLine($"\t\tnew IconModel(\"{folder}\", IconSize.Size{int.Parse(nameparts[0])}, IconVariant.{iconVariant}),");
                    iconcount++;

                    if (string.IsNullOrEmpty(iconbase))
                    {
                        iconbase = folder;
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
