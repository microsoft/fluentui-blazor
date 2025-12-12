// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models.McpDocumentation;

/// <summary>
/// Represents a Fluent UI component with its full documentation.
/// </summary>
public class McpComponentInfo
{
    /// <summary>
    /// Gets or sets the name of the component.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full type name of the component.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief description of the component.
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category of the component.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the component is a generic type.
    /// </summary>
    public bool IsGeneric { get; set; }

    /// <summary>
    /// Gets or sets the base class name.
    /// </summary>
    public string? BaseClass { get; set; }

    /// <summary>
    /// Gets or sets the list of properties.
    /// </summary>
    public List<McpPropertyInfo> Properties { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of events.
    /// </summary>
    public List<McpEventInfo> Events { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of methods.
    /// </summary>
    public List<McpMethodInfo> Methods { get; set; } = [];
}
