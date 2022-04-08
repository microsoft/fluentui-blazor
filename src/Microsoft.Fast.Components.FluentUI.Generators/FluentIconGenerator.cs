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
        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder? sb = new();
            Regex? regex = new(@"(\w*)_(\d*)_(\w*)");

            List<(string folder, string iconbase)> constants = new();
            List<(string iconbase, int size, bool filled)> icons = new();

            (string name, int size, bool filled) icon;


            context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.MSBuildProjectDirectory", out var projectDirectory);
            string iconsFolder = Path.Combine(Directory.GetParent(projectDirectory).FullName, @"Microsoft.Fast.Components.FluentUI\wwwroot\icons\");

            sb.AppendLine("using System.Collections.Generic;\r\n");
            sb.AppendLine("namespace Microsoft.Fast.Components.FluentUI;\r\n");
            sb.AppendLine("public static partial class FluentIcons");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static List<IconModel> IconMap = new ()");
            sb.AppendLine("\t{");


            foreach (string foldername in Directory.EnumerateDirectories(iconsFolder))
            {
                string folder = foldername.Substring(foldername.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                string iconbase = string.Empty;
                icons.Clear();
                foreach (string file in Directory.EnumerateFiles(foldername, "*.svg"))
                {

                    string name = Path.GetFileNameWithoutExtension(file);

                    MatchCollection? matches = regex.Matches(name);
                    if (matches.Count == 0)
                        continue;

                    icon = (matches[0].Groups[1].Value,
                        int.Parse(matches[0].Groups[2].Value),
                        matches[0].Groups[3].Value == "filled");

                    sb.AppendLine($"\t\tnew IconModel(\"{icon.name}\", IconSize.Size{icon.size}, {icon.filled.ToString().ToLower()}),");

                    if (string.IsNullOrEmpty(iconbase))
                    {
                        iconbase = icon.name;
                    }
                }

                constants.Add((folder, iconbase));

                //StringBuilder sb2 = new();
                //foreach (var (iconbase, size, filled) in icons)
                //{
                //    sb2.AppendLine($"\t\t\t\tnew IconModel(\"{iconbase}\",IconSize.Size{size}, {filled.ToString().ToLower()}),");
                //}
                //sb.AppendLine($"\t\t{{ \"{iconbase}\", new List<IconModel>()");
                //sb.AppendLine("\t\t\t{");
                //sb.AppendLine($"{sb2.ToString().TrimEnd(',')}");
                //sb.AppendLine("\t\t\t}");
                //sb.AppendLine("\t\t},");
            }
            sb.AppendLine("\t};");

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
    }
}
