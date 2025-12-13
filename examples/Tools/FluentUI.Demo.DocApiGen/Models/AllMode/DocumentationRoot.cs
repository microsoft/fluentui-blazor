// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models.AllMode;

/// <summary>
/// Root model for the MCP documentation JSON file.
/// Contains all components and enums documentation.
/// </summary>
public class DocumentationRoot
{
    /// <summary>
    /// Gets or sets metadata about the generated documentation.
    /// </summary>
    public DocumentationMetadata Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of all components.
    /// </summary>
    public List<ComponentInfo> Components { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of all enums.
    /// </summary>
    public List<EnumInfo> Enums { get; set; } = [];
}
