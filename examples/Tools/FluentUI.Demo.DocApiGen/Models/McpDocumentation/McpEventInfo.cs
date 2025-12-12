// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models.McpDocumentation;

/// <summary>
/// Represents an event of a component.
/// </summary>
public class McpEventInfo
{
    /// <summary>
    /// Gets or sets the name of the event.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the event callback.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the event.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this event is inherited.
    /// </summary>
    public bool IsInherited { get; set; }
}
