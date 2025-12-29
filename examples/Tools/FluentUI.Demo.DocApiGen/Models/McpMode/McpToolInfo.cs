// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.McpMode;

/// <summary>
/// Represents information about an MCP tool.
/// </summary>
public class McpToolInfo
{
    /// <summary>
    /// Gets or sets the name of the tool (method name).
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

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
    /// Gets or sets the class that contains this tool.
    /// </summary>
    [JsonPropertyName("className")]
    public required string ClassName { get; set; }

    /// <summary>
    /// Gets or sets the parameters of the tool.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<McpParameterInfo>? Parameters { get; set; }

    /// <summary>
    /// Gets or sets the return type of the tool.
    /// </summary>
    [JsonPropertyName("returnType")]
    public string? ReturnType { get; set; }
}
