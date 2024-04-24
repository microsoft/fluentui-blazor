using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

#nullable enable

namespace Microsoft.FluentUI.AspNetCore.Components.Generators;

[Generator]
public class DesignTokenGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValueProvider<string?> assemblyName = context.CompilationProvider.Select(static (c, _) => c.AssemblyName);
        context.RegisterSourceOutput(assemblyName, GenerateSource);
    }

    public void GenerateSource(SourceProductionContext context, string? assemblyName)
    {
        StringBuilder? sb = new();

        sb.AppendLine($"#pragma warning disable CS1591");
        sb.AppendLine($"namespace Microsoft.FluentUI.AspNetCore.Components.DesignTokens;");
        sb.AppendLine($"");
        sb.AppendLine("public static class Constants");
        sb.AppendLine("{");
        foreach (FieldInfo info in GetConstants(typeof(DesignTokenConstants)))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var PascalCase = info.Name[0].ToString().ToUpperInvariant() + info.Name.Substring(1);
            sb.AppendLine($"\tpublic const string {PascalCase} = \"{info.Name}\";");
        }
        sb.AppendLine("}");
        context.AddSource($"Constants.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));

        sb.Clear();
        sb.AppendLine("using System.Drawing;");
        sb.AppendLine("using Microsoft.JSInterop;\n");
        sb.AppendLine("namespace Microsoft.FluentUI.AspNetCore.Components.DesignTokens;\n");
        foreach (FieldInfo info in GetConstants(typeof(DesignTokenConstants)))
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var name = info.Name[0].ToString().ToUpperInvariant() + info.Name.Substring(1);
            var type = info.GetValue(null).ToString();

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
            sb.AppendLine($"\tpublic {name}(IJSRuntime jsRuntime) : base(jsRuntime)");
            sb.AppendLine("\t{");
            sb.AppendLine($"\t\tName = Constants.{name};");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
        }
        context.AddSource($"DesignTokens.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));

        sb.Clear();
        sb.AppendLine("using Microsoft.Extensions.DependencyInjection;\n");
        sb.AppendLine("namespace Microsoft.FluentUI.AspNetCore.Components.DesignTokens;\n");
        sb.AppendLine("public static class ServiceCollectionExtensions");
        sb.AppendLine("{");
        sb.AppendLine("\tpublic static void AddDesignTokens(this IServiceCollection services)");
        sb.AppendLine("\t{");
        foreach (FieldInfo info in GetConstants(typeof(DesignTokenConstants)))
        {
            sb.AppendLine($"\t\tservices.AddTransient<{info.Name[0].ToString().ToUpperInvariant() + info.Name.Substring(1)}>();");
        }
        sb.AppendLine("\t}");
        sb.AppendLine("}");
        context.AddSource($"ServiceCollectionExtensions.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    private List<FieldInfo> GetConstants(Type type)
    {
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
    }
}
