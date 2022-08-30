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

            var files = context.AdditionalFiles.Where(y => y.Path.EndsWith(".xml"));
            string documentationPath = files.First().Path;

            //string[] MEMBERS_TO_EXCLUDE = new[] { "AdditionalAttributes", "Equals", "GetHashCode", "GetType", "SetParametersAsync", "ToString" };

            XDocument xml = null;
            xml = XDocument.Load(documentationPath);

            IEnumerable<XElement> members = xml.Descendants("member");
            StringBuilder sb = new();

            sb.AppendLine($"#pragma warning disable CS1591");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("");
            sb.AppendLine("namespace FluentUI.Demo.Generators;");
            sb.AppendLine("public static class CodeComments");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static string GetSummary(string name)");
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tvar metadata = new Dictionary<string,string>() {");
            foreach (var m in members)
            {
                string paramName = CleanupParamName(m.Attribute("name").Value.ToString());
                string summary = CleanupSummary(m.Descendants().First().ToString());


                sb.Append("\t\t");
                sb.AppendLine($@"{{ ""{paramName}"", ""{summary}"" }},");

            }
            sb.AppendLine("\t\t};");
            sb.Append("\t\t");
            sb.AppendLine($@"var foundPair = metadata.FirstOrDefault(x => x.Key.EndsWith(name));");

            sb.AppendLine("\t\treturn foundPair.Value;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("}");

            context.AddSource("CodeComments.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private static string CleanupParamName(string value)
        {

            Regex regex = new("[P,T,M,F]:");
            value = regex.Replace(value, "");


            value = value.Replace("Microsoft.Fast.Components.FluentUI.", "");


            return value;
        }

        private static string CleanupSummary(string value)
        {
            Regex regex = new(@"[ ]{2,}");
            value = regex.Replace(value, "");

            regex = new("<seealso cref=\"[!,P,T,M]+:+");
            value = regex.Replace(value, "");

            regex = new("\"\\s+/>");
            value = regex.Replace(value, "");

            return value.Trim()
                      .Replace("<summary>\r\n", "")
                      .Replace("\r\n</summary>", "")
                      .Replace("<see cref=", " ")
                      .Replace("<see href=\"", "<a href=\"")
                      .Replace("</see>", "</a>")
                      .Replace("\r\n", "<br />")
                      .Replace("\"", "\\\"")
                      .Replace("Microsoft.Fast.Components.FluentUI.", "");
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
