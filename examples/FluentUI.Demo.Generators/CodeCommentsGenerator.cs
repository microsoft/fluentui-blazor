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
        private static Dictionary<string, XDocument> cachedXml = new(StringComparer.OrdinalIgnoreCase);

        public Type Component { get; set; } = default!;

        public void Execute(GeneratorExecutionContext context)
        {
            //IEnumerable<MemberDescription> Properties = GetMembers(MemberTypes.Property);

            Debug.WriteLine("Execute code generator");

            var files = context.AdditionalFiles.Where(y => y.Path.EndsWith(".xml"));


            string documentationPath = files.First().Path;

            //string[] MEMBERS_TO_EXCLUDE = new[] { "AdditionalAttributes", "Equals", "GetHashCode", "GetType", "SetParametersAsync", "ToString" };


            XDocument xml = null;
            xml = XDocument.Load(documentationPath);




            IEnumerable<XElement> members = xml.Descendants("member");

            // begin creating the source we'll inject into the users compilation
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
                string paramName = m.Attribute("name").Value.ToString();
                string summary = CleanupSummary(m.Descendants().First().ToString());



                if (!paramName.StartsWith("T:"))
                {
                    paramName = paramName.Replace("P:", "")
                                         .Replace("M:", "")
                                         .Replace("Microsoft.Fast.Components.FluentUI.", "");

                    sb.Append("\t\t");
                    sb.AppendLine($@"{{ ""{paramName}"", ""{summary}"" }},");
                }
            }
            sb.AppendLine("\t\t};");
            sb.Append("\t\t");
            sb.AppendLine($@"var foundPair = metadata.FirstOrDefault(x => x.Key.EndsWith(name));");

            sb.AppendLine("\t\treturn foundPair.Value;");

            // finish creating the source to inject
            sb.AppendLine("\t\t}");
            sb.AppendLine("}");

            // inject the created source into the users compilation
            context.AddSource("CodeComments.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
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

        private static string CleanupSummary(string value)
        {
            Regex regex = new(@"[ ]{2,}");
            string ret = regex.Replace(value, "");

            regex = new("<seealso cref=\"[!,P,T,M]+:+");
            ret = regex.Replace(ret, "");

            regex = new("\"\\s+/>");
            ret = regex.Replace(ret, "");

            return ret.Trim()
                      .Replace("<summary>\r\n", "")
                      .Replace("\r\n</summary>", "")
                      .Replace("<see cref=\"", " ")
                      .Replace("<see href=\"", "<a href=\"")
                      .Replace("</see>", "</a>")
                      .Replace("\r\n", "<br />")
                      .Replace("\"", "\\\"")
                      .Replace("Microsoft.Fast.Components.FluentUI.", "");
        }

    }
}
