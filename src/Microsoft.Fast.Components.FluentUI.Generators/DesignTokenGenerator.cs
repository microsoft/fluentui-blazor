using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;


#nullable enable

namespace Microsoft.Fast.Components.FluentUI.Generators
{
    [Generator]
    public class DesignTokenGenerator : ISourceGenerator
    {
        private List<FieldInfo> GetConstants(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            StringBuilder? sb = new();

            sb.AppendLine($"namespace Microsoft.Fast.Components.FluentUI.DesignTokens;");
            sb.AppendLine($"");
            sb.AppendLine("public class Constants");
            sb.AppendLine("{");
            foreach (FieldInfo info in GetConstants(typeof(DesignTokenConstants)))
            {
                string PascalCase = info.Name[0].ToString().ToUpperInvariant() + info.Name.Substring(1);
                sb.AppendLine($"\tpublic const string {PascalCase} = \"{info.Name}\";");
            }
            sb.AppendLine("}");
            context.AddSource($"Constants.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));

            sb.Clear();
            sb.AppendLine("using Microsoft.Extensions.Configuration;");
            sb.AppendLine("using Microsoft.JSInterop;");
            sb.AppendLine("");
            sb.AppendLine("namespace Microsoft.Fast.Components.FluentUI.DesignTokens;");
            foreach (FieldInfo info in GetConstants(typeof(DesignTokenConstants)))
            {
                string name = info.Name[0].ToString().ToUpperInvariant() + info.Name.Substring(1);
                string type = info.GetValue(null).ToString();

                sb.AppendLine("");
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"/// The {name} design token");
                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public sealed class {name} : DesignToken<{type}>");
                sb.AppendLine("{");
                sb.AppendLine("\t/// <summary>");
                sb.AppendLine($"\t/// Constructs an instance of the {name} design token");
                sb.AppendLine("\t/// </summary>");
                sb.AppendLine($"\tpublic {name}()");
                sb.AppendLine("\t{");
                sb.AppendLine($"\t\tName = Constants.{name};");
                sb.AppendLine("\t}");
                sb.AppendLine("");
                sb.AppendLine("\t/// <summary>");
                sb.AppendLine($"\t/// Constructs an instance of the {name} design token");
                sb.AppendLine("\t/// </summary>");
                sb.AppendLine("\t/// <param name=\"jsRuntime\">IJSRuntime reference</param>");
                sb.AppendLine("\t/// <param name=\"configuration\">IConfiguration reference</param>");
                sb.AppendLine($"\tpublic {name}(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)");
                sb.AppendLine("\t{");
                sb.AppendLine($"\t\tName = Constants.{name};");
                sb.AppendLine("\t}");
                sb.AppendLine("}");
            }
            context.AddSource($"DesignTokens.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));

            sb.Clear();
            sb.AppendLine($"using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine($"");
            sb.AppendLine($"namespace Microsoft.Fast.Components.FluentUI.DesignTokens;");
            sb.AppendLine($"");
            sb.AppendLine("public static class ServiceCollectionExtensions");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic static void AddDesignTokens(this IServiceCollection services)");
            sb.AppendLine("\t{");
            foreach (FieldInfo info in GetConstants(typeof(DesignTokenConstants)))
            {
                sb.AppendLine($"\t\tservices.AddTransient<{info.Name[0].ToString().ToUpperInvariant() + info.Name.Substring(1)}>();");
            }
            sb.AppendLine("	}");
            sb.AppendLine("}");
            context.AddSource($"ServiceCollectionExtensions.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
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
