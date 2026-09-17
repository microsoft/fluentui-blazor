// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocViewer.Models.Mcp;

/// <summary>
/// Represents information about a parameter of an MCP tool, resource, or prompt.
/// </summary>
public class McpParameter
{
    /// <summary>
    /// Gets or sets the name of the parameter.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the parameter.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description from the [Description] attribute.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets whether the parameter is required.
    /// </summary>
    [JsonPropertyName("isRequired")]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the default value if optional.
    /// </summary>
    [JsonPropertyName("defaultValue")]
    public string? DefaultValue { get; set; }
}
