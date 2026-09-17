// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Models.McpDocumentation;

/// <summary>
/// Metadata about the generated documentation.
/// </summary>
internal class McpDocumentationMetadata
{
    /// <summary>
    /// Gets or sets the assembly version.
    /// </summary>
    [JsonPropertyName("assemblyVersion")]
    public string AssemblyVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the generation date in UTC.
    /// </summary>
    [JsonPropertyName("generatedDateUtc")]
    public string GeneratedDateUtc { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total component count.
    /// </summary>
    [JsonPropertyName("componentCount")]
    public int ComponentCount { get; set; }

    /// <summary>
    /// Gets or sets the total enum count.
    /// </summary>
    [JsonPropertyName("enumCount")]
    public int EnumCount { get; set; }
}
