// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models.McpDocumentation;

/// <summary>
/// Root model for the MCP documentation JSON file.
/// Contains all components and enums documentation.
/// </summary>
public class McpDocumentationRoot
{
    /// <summary>
    /// Gets or sets metadata about the generated documentation.
    /// </summary>
    public McpDocumentationMetadata Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of all components.
    /// </summary>
    public List<McpComponentInfo> Components { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of all enums.
    /// </summary>
    public List<McpEnumInfo> Enums { get; set; } = [];
}
