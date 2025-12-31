// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Models.McpDocumentation;

/// <summary>
/// Root model for the MCP documentation JSON file.
/// </summary>
internal class McpDocumentationRoot
{
    /// <summary>
    /// Gets or sets metadata about the generated documentation.
    /// </summary>
    [JsonPropertyName("metadata")]
    public McpDocumentationMetadata Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of all components.
    /// </summary>
    [JsonPropertyName("components")]
    public List<JsonComponentInfo> Components { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of all enums.
    /// </summary>
    [JsonPropertyName("enums")]
    public List<JsonEnumInfo> Enums { get; set; } = [];
}
