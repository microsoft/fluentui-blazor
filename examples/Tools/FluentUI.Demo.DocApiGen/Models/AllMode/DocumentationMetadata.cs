// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models.AllMode;

/// <summary>
/// Metadata about the generated documentation.
/// </summary>
public class DocumentationMetadata
{
    /// <summary>
    /// Gets or sets the assembly version.
    /// </summary>
    public string AssemblyVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the generation date in UTC.
    /// </summary>
    public string GeneratedDateUtc { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total component count.
    /// </summary>
    public int ComponentCount { get; set; }

    /// <summary>
    /// Gets or sets the total enum count.
    /// </summary>
    public int EnumCount { get; set; }
}
