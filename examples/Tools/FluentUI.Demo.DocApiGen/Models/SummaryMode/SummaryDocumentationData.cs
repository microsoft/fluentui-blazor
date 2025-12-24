// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models.SummaryMode;

/// <summary>
/// Root object for Summary mode documentation.
/// </summary>
public class SummaryDocumentationData
{
    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    public required SummaryMetadata Metadata { get; set; }

    /// <summary>
    /// Gets or sets the components dictionary.
    /// Key: Full member name (Type.Member)
    /// Value: Summary and Signature
    /// </summary>
    public required Dictionary<string, ComponentEntry> Components { get; set; }
}

/// <summary>
/// Represents a component entry in Summary mode.
/// </summary>
public class ComponentEntry
{
    /// <summary>
    /// Gets or sets the summary text.
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the signature.
    /// </summary>
    public string Signature { get; set; } = string.Empty;
}

/// <summary>
/// Metadata for Summary mode generated documentation.
/// </summary>
public class SummaryMetadata
{
    /// <summary>
    /// Gets or sets the assembly version.
    /// </summary>
    public string AssemblyVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the generation date (UTC).
    /// </summary>
    public string DateUtc { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the generation mode.
    /// </summary>
    public string Mode { get; set; } = string.Empty;
}
