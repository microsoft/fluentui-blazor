// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocViewer.Models.Mcp;

/// <summary>
/// Root object for MCP documentation.
/// </summary>
public class McpDocumentation
{
    /// <summary>
    /// Gets or sets the metadata about the documentation generation.
    /// </summary>
    [JsonPropertyName("metadata")]
    public McpMetadata? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the list of MCP tools.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<McpTool> Tools { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of MCP resources.
    /// </summary>
    [JsonPropertyName("resources")]
    public List<McpResource> Resources { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of MCP prompts.
    /// </summary>
    [JsonPropertyName("prompts")]
    public List<McpPrompt> Prompts { get; set; } = [];
}
