// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text;
using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Helpers;

/// <summary>
/// Helper methods for formatting MCP tool output.
/// </summary>
internal static class ToolOutputHelper
{
    /// <summary>
    /// Truncates a summary to the specified maximum length.
    /// </summary>
    public static string TruncateSummary(string? summary, int maxLength)
    {
        if (string.IsNullOrEmpty(summary))
        {
            return "-";
        }

        if (summary.Length <= maxLength)
        {
            return summary;
        }

        return summary[..(maxLength - 3)] + "...";
    }

    /// <summary>
    /// Checks if a parameter name is common enough to include in examples.
    /// </summary>
    public static bool IsCommonExampleParam(string paramName)
    {
        var commonParams = new[]
        {
            "Id", "Label", "Placeholder", "Value", "Disabled", "ReadOnly",
            "Appearance", "Size", "Color", "IconStart", "IconEnd"
        };

        return commonParams.Contains(paramName, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets an example value for a property based on its type.
    /// </summary>
    public static string GetExampleValue(PropertyInfo param)
    {
        if (param.EnumValues.Length > 0)
        {
            return $"@{param.Type}.{param.EnumValues[0]}";
        }

        return param.Type switch
        {
            "string" => $"your-{param.Name.ToLowerInvariant()}",
            "bool" => "true",
            "int" => "42",
            _ => "..."
        };
    }

    /// <summary>
    /// Appends a markdown header.
    /// </summary>
    public static void AppendHeader(StringBuilder sb, string title, int level = 1)
    {
        sb.AppendLine(CultureInfo.InvariantCulture, $"{new string('#', level)} {title}");
        sb.AppendLine();
    }

    /// <summary>
    /// Appends a markdown table header.
    /// </summary>
    public static void AppendTableHeader(StringBuilder sb, params string[] columns)
    {
        sb.AppendLine(CultureInfo.InvariantCulture, $"| {string.Join(" | ", columns)} |");
        sb.AppendLine(CultureInfo.InvariantCulture, $"|{string.Join("|", columns.Select(_ => "------"))}|");
    }
}
