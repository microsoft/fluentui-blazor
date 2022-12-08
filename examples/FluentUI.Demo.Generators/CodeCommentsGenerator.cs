using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FluentUI.Demo.Generators
{
    [Generator]
    public class CodeCommentsGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            Debug.WriteLine("Execute code generator");

            IEnumerable<AdditionalText> files = context.AdditionalFiles.Where(y => y.Path.EndsWith(".xml"));
            List<XElement> members = new();

            foreach (AdditionalText file in files)
            {
                XDocument xml = null;
                xml = XDocument.Load(file.Path);

                members.AddRange(xml.Descendants("member"));
            }

            StringBuilder sb = new();

            sb.AppendLine("#pragma warning disable CS1591");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("");
            sb.AppendLine("namespace FluentUI.Demo.Shared;");
            sb.AppendLine("public static class CodeComments");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static string GetSummary(string name)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tDictionary<string, string> summarydata = new Dictionary<string, string>() {");
            foreach (var m in members)
            {
                string paramName = CleanupParamName(m.Attribute("name").Value.ToString());
                string summary = CleanupSummary(m.Descendants().First().ToString());

                sb.AppendLine("\t\t\t[\"" + paramName + "\"] = \"" + summary + "\", ");
            }
            int lastComma = sb.ToString().LastIndexOf(',');

            sb.Remove(lastComma, 1);
            sb.AppendLine("\t\t};");
            sb.Append("\t\t");
            sb.AppendLine("KeyValuePair<string, string> foundPair = summarydata.FirstOrDefault(x => x.Key.StartsWith(name));");

            sb.AppendLine("\t\treturn foundPair.Value;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("}");

            context.AddSource("CodeComments.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private static string CleanupParamName(string value)
        {
            Regex regex = new("[P,T,M,F]:Microsoft\\.Fast\\.Components\\.FluentUI\\.");
            value = regex.Replace(value, "");
            regex = new("[P,T,M,F]:FluentUI\\.Demo\\.Shared\\.Components\\.");
            value = regex.Replace(value, "");
            regex = new("[P,T,M,F]:FluentUI\\.Demo\\.Shared\\.");
            value = regex.Replace(value, "");

            return value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "Need to use Envrionment to ensure the right newline is generated for platform it is being builtand run on")]
        private static string CleanupSummary(string value)
        {
            Regex regex = new(@"[ ]{2,}");
            value = regex.Replace(value, "");

            regex = new("<see(?:also)? cref=[\"|'][!,P,T,M,F]+:+Microsoft\\.Fast\\.Components\\.FluentUI\\.([\\w|\\.]*)(?<generic>`\\d)?[\"|']\\s*/>");
            value = regex.Replace(value, m => m.Groups["generic"].Success ? $"<code>{m.Groups[1].Value}&lt;T&gt;</code>" : $"<code>{m.Groups[1].Value}</code>");

            regex = new("<see(?:also)? cref=[\"|'][!,P,T,M,F]+:+FluentUI\\.Demo\\.Shared\\.([\\w|\\.]*)(?<generic>`\\d)?[\"|']\\s*/>");
            value = regex.Replace(value, m => m.Groups["generic"].Success ? $"<code>{m.Groups[1].Value}&lt;T&gt;</code>" : $"<code>{m.Groups[1].Value}</code>");

            regex = new("<see(?:also)? cref=[\"|'][!,P,T,M,F]+:+([\\w|\\.|`|\\(|\\)|\\{|\\}|\\,]*)[\"|']\\s*/>");
            value = regex.Replace(value, m => $"<code>{m.Groups[1].Value}</code>");

            regex = new("<see langword=[\"|']([\\w|\\.|`|(|)|{|}|,]*)[\"|']\\s*/>");
            value = regex.Replace(value, m => $"<code>{m.Groups[1].Value}</code>");

            regex = new("<see href=\"(.*?)\">(.*?)</see>");
            value = regex.Replace(value, "<a href=\"$1\">$2</a>");


            return value.Trim()
                        .Replace("<summary>" + Environment.NewLine, "")
                        .Replace(Environment.NewLine + "</summary>", "")
                        .Replace(Environment.NewLine, "<br />")
                        .Replace("\"", "'")
                        .Replace("Microsoft.Fast.Components.FluentUI.", "")
                        .Replace("FluentDataGrid`1.", "")
                        .Replace("System.Linq.", "")
                        .Replace("System.Linq.Queryable.", "")
                        .Replace("System.Collections.", "")
                        .Replace("`1", "<T>")
                        .Replace("`0", "<U>");

        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
#endif
        }
    }
}
