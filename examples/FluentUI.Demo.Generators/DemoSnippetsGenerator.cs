using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FluentUI.Demo.Generators
{
    [Generator]
    public class DemoSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            Debug.WriteLine("Execute code generator");

            StringBuilder sb = new();

            sb.AppendLine($"#pragma warning disable CS1591");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("");
            sb.AppendLine("namespace FluentUI.Demo.Generators;");
            sb.AppendLine("public static class DemoSnippets");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static string GetRazor(string name)");
            sb.AppendLine("\t{");

            var files = context.AdditionalFiles;
            var dictionary = files.Where(y => y.Path.EndsWith(".razor")).ToDictionary(x => Path.GetFileName(x.Path), x => x.GetText().ToString().Replace(@"""", @""""""));
            sb.AppendLine("\t\tvar exampleData = new Dictionary<string,string>() {");
            foreach (var pair in dictionary)
            {
                sb.Append("\t\t");
                sb.AppendLine($@"{{ @""{pair.Key}"", @""{pair.Value}"" }},");
            }
            sb.AppendLine("\t\t};");
            sb.Append("\t\t");
            sb.AppendLine($@"var foundPair = exampleData.FirstOrDefault(x => x.Key.EndsWith(name + "".razor"" ));");
            sb.AppendLine("\t\treturn foundPair.Value;");
            sb.AppendLine("\t\t}");
            sb.AppendLine("}");

            context.AddSource("DemoSnippets.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
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