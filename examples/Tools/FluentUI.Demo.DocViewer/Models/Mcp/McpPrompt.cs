// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocViewer.Models.Mcp;

/// <summary>
/// Represents information about an MCP prompt.
/// </summary>
public class McpPrompt
{
    /// <summary>
    /// Gets or sets the name of the prompt (method name).
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description from the [Description] attribute.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the XML summary documentation.
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the class that contains this prompt.
    /// </summary>
    [JsonPropertyName("className")]
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parameters of the prompt.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<McpParameter>? Parameters { get; set; }
}
