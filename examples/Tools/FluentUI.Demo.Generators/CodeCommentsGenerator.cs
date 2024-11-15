// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace FluentUI.Demo.Generators;

[Generator]
public class CodeCommentsGenerator : IIncrementalGenerator
{
    private static readonly string[] REGEX_CLEANUP =
    [
        @"Microsoft\.FluentUI\.AspNetCore\.Components\.",
        @"FluentUI\.Demo\.Client\.",
        @"\[\[.*?\]\]",
        @"\[.*?\]"
    ];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var files = context.AdditionalTextsProvider.Where(at => at.Path.EndsWith(".xml")).Collect();
        context.RegisterSourceOutput(files, GenerateSource);
    }

    public void GenerateSource(SourceProductionContext context, ImmutableArray<AdditionalText> files)
    {
        List<XElement> members = [];

        foreach (var file in files)
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                return;
            }

            var f = file.GetText(context.CancellationToken);
            var xml = XDocument.Parse(f.ToString(), LoadOptions.None);

            members.AddRange(xml.Descendants("member"));
        }

        StringBuilder sb = new();

        sb.AppendLine("#pragma warning disable CS1591");
        sb.AppendLine("");
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Linq;");
        sb.AppendLine("");
        sb.AppendLine("namespace FluentUI.Demo.Client;");
        sb.AppendLine("");
        sb.AppendLine("public static partial class CodeComments");
        sb.AppendLine("{");
        sb.AppendLine();
        sb.AppendLine("    private static readonly string[] REGEX_CLEANUP = [\"" + string.Join("\", \"", REGEX_CLEANUP.Select(i => i.Replace("\\", "\\\\"))) + "\"];");
        sb.AppendLine();
        sb.AppendLine("    public static string GetSummary(string name)");
        sb.AppendLine("    {");
        sb.AppendLine("        Dictionary<string, string> summaryData = new Dictionary<string, string>()");
        sb.AppendLine("        {");

        foreach (var m in members)
        {
            var paramName = CleanupParamName(m.Attribute("name").Value.ToString());
            var summary = CleanupSummary(m.Descendants().First().ToString());

            if (summary != "<summary />")
            {
                sb.AppendLine("            [\"" + paramName + "\"] = \"" + summary + "\", ");
            }
        }

        var lastComma = sb.ToString().LastIndexOf(',');

        sb.Remove(lastComma, 1);
        sb.AppendLine("        };");
        sb.AppendLine();
        sb.AppendLine("        KeyValuePair<string, string> foundPair = summaryData.FirstOrDefault(x => x.Key.Equals(name));");
        sb.AppendLine();
        sb.AppendLine("        return foundPair.Value;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    public static string GetSummary(System.Reflection.MemberInfo memberInfo) => GetSummary(GetApiCommentName(memberInfo));");
        sb.AppendLine();
        sb.AppendLine("    public static string GetApiCommentName(System.Reflection.MemberInfo memberInfo)");
        sb.AppendLine("    {");
        sb.AppendLine("        if (memberInfo is System.Reflection.MethodInfo methodInfo)");
        sb.AppendLine("        {");
        sb.AppendLine("            var parameters = string.Join(\", \", methodInfo.GetParameters().Select(p => $\"{p.ParameterType.FullName}\"));");
        sb.AppendLine("            return CleanupName($\"{methodInfo.DeclaringType?.FullName}.{methodInfo.Name}({parameters})\");");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine("        if (memberInfo is System.Reflection.PropertyInfo propertyInfo)");
        sb.AppendLine("        {");
        sb.AppendLine("            return CleanupName($\"{propertyInfo.DeclaringType?.FullName}.{propertyInfo.Name}\");");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine("        return string.Empty;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("    private static string CleanupName(string value)");
        sb.AppendLine("    {");
        sb.AppendLine("        foreach (var cleanup in REGEX_CLEANUP)");
        sb.AppendLine("        {");
        sb.AppendLine("            System.Text.RegularExpressions.Regex r = new(cleanup);");
        sb.AppendLine("            value = r.Replace(value, string.Empty);");
        sb.AppendLine("        }");
        sb.AppendLine();
        sb.AppendLine("        return value;");
        sb.AppendLine("    }");
        sb.AppendLine();
        sb.AppendLine("}");

        context.AddSource($"CodeComments.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
    }

    private static string CleanupParamName(string value)
    {
        foreach (var cleanup in REGEX_CLEANUP)
        {
            Regex r = new($"[P,T,M,F]:{cleanup}");
            value = r.Replace(value, string.Empty);
        }

        return value;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "Need to use Environment to ensure the right newline is generated for platform it is being built and run on")]
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

        regex = new("<param(?:ref)? name=[\"|'](.*)[\"|']\\s*/>");
        value = regex.Replace(value, m => $"<code>{m.Groups[0].Value}</code>{m.Groups[1].Value}");

        return value.Trim()
                    .Replace("<summary>" + Environment.NewLine, "")
                    .Replace(Environment.NewLine + "</summary>", "")
                    .Replace(Environment.NewLine, "<br />")
                    .Replace("\"", "'")
                    .Replace("Microsoft.FluentUI.AspNetCore.Components.", "")
                    .Replace("FluentDataGrid`1.", "")
                    .Replace("System.Linq.", "")
                    .Replace("System.Linq.Queryable.", "")
                    .Replace("System.Collections.", "")
                    .Replace("`1", "<T>")
                    .Replace("`0", "<U>");

    }
}
