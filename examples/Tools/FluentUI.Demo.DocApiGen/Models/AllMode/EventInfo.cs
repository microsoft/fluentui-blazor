// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.AllMode;

/// <summary>
/// Represents an event of a component.
/// </summary>
public class EventInfo
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
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets whether this event is inherited.
    /// Only serialized when true to reduce JSON size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsInherited { get; set; }
}
