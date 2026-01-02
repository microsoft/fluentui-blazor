// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Models.McpDocumentation;

/// <summary>
/// JSON representation of a property.
/// </summary>
internal class JsonPropertyInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("isParameter")]
    public bool IsParameter { get; set; }

    [JsonPropertyName("isInherited")]
    public bool IsInherited { get; set; }

    [JsonPropertyName("defaultValue")]
    public string? DefaultValue { get; set; }

    [JsonPropertyName("enumValues")]
    public string[] EnumValues { get; set; } = [];
}
