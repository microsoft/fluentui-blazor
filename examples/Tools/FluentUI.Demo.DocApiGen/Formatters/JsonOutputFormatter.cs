// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentUI.Demo.DocApiGen.Abstractions;
using FluentUI.Demo.DocApiGen.Models.SummaryMode;

namespace FluentUI.Demo.DocApiGen.Formatters;

/// <summary>
/// Formats documentation output as JSON.
/// </summary>
public class JsonOutputFormatter : IOutputFormatter
{
    private readonly bool _indented;
    private readonly bool _useCompactFormat;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonOutputFormatter"/> class.
    /// </summary>
    /// <param name="indented">Whether to indent the JSON output.</param>
    /// <param name="useCompactFormat">Whether to use the compact format for Summary mode (default true).</param>
    public JsonOutputFormatter(bool indented = true, bool useCompactFormat = true)
    {
        _indented = indented;
        _useCompactFormat = useCompactFormat;
    }

    /// <inheritdoc/>
    public string FormatName => "json";

    /// <inheritdoc/>
    public string Format(object data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        // Si c'est un SummaryDocumentationData et qu'on veut le format compact (Summary mode standard)
        if (_useCompactFormat && data is SummaryDocumentationData summaryData)
        {
            return FormatSummary(summaryData);
        }

        // Format structuré (pour All mode ou mode étendu)
        var options = new JsonSerializerOptions
        {
            WriteIndented = _indented,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(data, options);
    }

    /// <summary>
    /// Formats the documentation data in the Summary mode compact format.
    /// This format groups members by component type with simple keys.
    /// </summary>
    private static string FormatSummary(SummaryDocumentationData data)
    {
        var sb = new StringBuilder();
        sb.AppendLine("{");

        // Ajouter les métadonnées
        sb.AppendLine("  \"__Generated__\": {");
        sb.AppendLine(CultureInfo.InvariantCulture, $"    \"AssemblyVersion\": \"{EscapeJson(data.Metadata.AssemblyVersion)}\",");
        sb.AppendLine(CultureInfo.InvariantCulture, $"    \"DateUtc\": \"{EscapeJson(data.Metadata.DateUtc)}\"");
        sb.Append("  }");

        // Grouper les composants par type
        var componentsByType = new Dictionary<string, Dictionary<string, string>>();

        foreach (var kvp in data.Components)
        {
            // Format de la clé: "Namespace.TypeName.__summary__" ou "Namespace.TypeName.MemberName"
            var fullKey = kvp.Key;
            var lastDotIndex = fullKey.LastIndexOf('.');
            
            if (lastDotIndex == -1)
                continue;

            var beforeLastDot = fullKey[..lastDotIndex];
            var memberName = fullKey[(lastDotIndex + 1)..];

            // Extraire le nom du type (dernière partie avant le membre)
            var typeNameStartIndex = beforeLastDot.LastIndexOf('.') + 1;
            var typeName = beforeLastDot[typeNameStartIndex..];

            if (!componentsByType.TryGetValue(typeName, out var members))
            {
                members = new Dictionary<string, string>();
                componentsByType[typeName] = members;
            }

            members[memberName] = kvp.Value.Summary ?? string.Empty;
        }

        // Écrire chaque type dans l'ordre
        foreach (var typeEntry in componentsByType.OrderBy(x => x.Key))
        {
            sb.AppendLine(",");
            sb.AppendLine(CultureInfo.InvariantCulture, $"  \"{EscapeJson(typeEntry.Key)}\": {{");

            var membersList = typeEntry.Value.OrderBy(x => x.Key).ToList();
            for (int i = 0; i < membersList.Count; i++)
            {
                var member = membersList[i];
                var isLast = i == membersList.Count - 1;

                var escapedValue = EscapeJson(member.Value);
                sb.Append(CultureInfo.InvariantCulture, $"    \"{EscapeJson(member.Key)}\": \"{escapedValue}\"");
                
                if (!isLast)
                {
                    sb.AppendLine(",");
                }
                else
                {
                    sb.AppendLine();
                }
            }

            sb.Append("  }");
        }

        sb.AppendLine();
        sb.AppendLine("}");

        return sb.ToString();
    }

    /// <summary>
    /// Escapes special characters for JSON strings.
    /// </summary>
    private static string EscapeJson(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\r\n", " ")
            .Replace("\n", " ")
            .Replace("\r", " ");
    }
}
