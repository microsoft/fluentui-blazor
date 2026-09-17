// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.McpMode;

/// <summary>
/// Root object for MCP documentation output.
/// </summary>
public class McpDocumentationRoot
{
    /// <summary>
    /// Gets or sets the metadata about the documentation generation.
    /// </summary>
    [JsonPropertyName("metadata")]
    public required McpDocumentationMetadata Metadata { get; set; }

    /// <summary>
    /// Gets or sets the list of MCP tools.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<McpToolInfo> Tools { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of MCP resources.
    /// </summary>
    [JsonPropertyName("resources")]
    public List<McpResourceInfo> Resources { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of MCP prompts.
    /// </summary>
    [JsonPropertyName("prompts")]
    public List<McpPromptInfo> Prompts { get; set; } = [];
}
