using System.Diagnostics;
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
            // begin creating the source we'll inject into the users compilation
            StringBuilder sb = new();
            sb.Clear();
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
            var dictionary = files.ToDictionary(x => x.Path, x => x.GetText().ToString().Replace(@"""", @""""""));
            sb.AppendLine("\t\tvar metadata = new Dictionary<string,string>() {");
            foreach (var pair in dictionary)
            {
                sb.AppendLine($@"{{ @""{pair.Key}"", @""{pair.Value}"" }},");
            }
            sb.AppendLine("\t\t};");

            sb.AppendLine($@"var foundPair = metadata.FirstOrDefault(x => x.Key.EndsWith(""\\"" + name ));");

            sb.AppendLine("\t\treturn foundPair.Value;");

            // finish creating the source to inject
            sb.AppendLine("\t\t}");
            sb.AppendLine("}");

            // inject the created source into the users compilation
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