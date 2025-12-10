// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Models;

/// <summary>
/// Represents detailed information about a component.
/// </summary>
public record ComponentDetails
{
    /// <summary>
    /// Gets the basic component information.
    /// </summary>
    public required ComponentInfo Component { get; init; }

    /// <summary>
    /// Gets the list of parameters (properties with [Parameter] attribute).
    /// </summary>
    public IReadOnlyList<PropertyInfo> Parameters { get; init; } = [];

    /// <summary>
    /// Gets the list of all properties.
    /// </summary>
    public IReadOnlyList<PropertyInfo> Properties { get; init; } = [];

    /// <summary>
    /// Gets the list of events.
    /// </summary>
    public IReadOnlyList<EventInfo> Events { get; init; } = [];

    /// <summary>
    /// Gets the list of public methods.
    /// </summary>
    public IReadOnlyList<MethodInfo> Methods { get; init; } = [];
}
