// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Models.McpDocumentation;

/// <summary>
/// JSON representation of a component.
/// </summary>
internal class JsonComponentInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("isGeneric")]
    public bool IsGeneric { get; set; }

    [JsonPropertyName("baseClass")]
    public string? BaseClass { get; set; }

    [JsonPropertyName("properties")]
    public List<JsonPropertyInfo> Properties { get; set; } = [];

    [JsonPropertyName("events")]
    public List<JsonEventInfo> Events { get; set; } = [];

    [JsonPropertyName("methods")]
    public List<JsonMethodInfo> Methods { get; set; } = [];
}
