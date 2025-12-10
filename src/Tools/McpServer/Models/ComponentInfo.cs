// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Models;

/// <summary>
/// Represents a Fluent UI component with its metadata.
/// </summary>
public record ComponentInfo
{
    /// <summary>
    /// Gets the name of the component.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the full type name of the component.
    /// </summary>
    public required string FullName { get; init; }

    /// <summary>
    /// Gets a brief description of the component.
    /// </summary>
    public string Summary { get; init; } = string.Empty;

    /// <summary>
    /// Gets the category of the component (e.g., "Button", "Input", "Layout").
    /// </summary>
    public string Category { get; init; } = string.Empty;

    /// <summary>
    /// Gets whether the component is a generic type.
    /// </summary>
    public bool IsGeneric { get; init; }

    /// <summary>
    /// Gets the base class name.
    /// </summary>
    public string? BaseClass { get; init; }
}
