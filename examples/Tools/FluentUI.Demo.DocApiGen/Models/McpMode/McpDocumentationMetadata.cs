// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.McpMode;

/// <summary>
/// Metadata about the MCP documentation generation.
/// </summary>
public class McpDocumentationMetadata
{
    /// <summary>
    /// Gets or sets the assembly version.
    /// </summary>
    [JsonPropertyName("assemblyVersion")]
    public required string AssemblyVersion { get; set; }

    /// <summary>
    /// Gets or sets the UTC date when the documentation was generated.
    /// </summary>
    [JsonPropertyName("generatedDateUtc")]
    public required string GeneratedDateUtc { get; set; }

    /// <summary>
    /// Gets or sets the total number of tools.
    /// </summary>
    [JsonPropertyName("toolCount")]
    public int ToolCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of resources.
    /// </summary>
    [JsonPropertyName("resourceCount")]
    public int ResourceCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of prompts.
    /// </summary>
    [JsonPropertyName("promptCount")]
    public int PromptCount { get; set; }
}
