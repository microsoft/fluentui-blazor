// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocViewer.Models.Mcp;

/// <summary>
/// Metadata about the MCP documentation generation.
/// </summary>
public class McpMetadata
{
    /// <summary>
    /// Gets or sets the assembly version.
    /// </summary>
    [JsonPropertyName("assemblyVersion")]
    public string? AssemblyVersion { get; set; }

    /// <summary>
    /// Gets or sets the generation date in UTC.
    /// </summary>
    [JsonPropertyName("generatedDateUtc")]
    public string? GeneratedDateUtc { get; set; }

    /// <summary>
    /// Gets or sets the number of tools.
    /// </summary>
    [JsonPropertyName("toolCount")]
    public int ToolCount { get; set; }

    /// <summary>
    /// Gets or sets the number of resources.
    /// </summary>
    [JsonPropertyName("resourceCount")]
    public int ResourceCount { get; set; }

    /// <summary>
    /// Gets or sets the number of prompts.
    /// </summary>
    [JsonPropertyName("promptCount")]
    public int PromptCount { get; set; }
}
