// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Mcp.Server.Models;

/// <summary>
/// Represents a method of a component.
/// </summary>
public record MethodInfo
{
    /// <summary>
    /// Gets the name of the method.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the return type of the method.
    /// </summary>
    public required string ReturnType { get; init; }

    /// <summary>
    /// Gets the description of the method.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets the parameters of the method.
    /// </summary>
    public string[] Parameters { get; init; } = [];

    /// <summary>
    /// Gets the method signature.
    /// </summary>
    public string Signature => $"{ReturnType} {Name}({string.Join(", ", Parameters)})";

    /// <summary>
    /// Gets whether this method is inherited.
    /// </summary>
    public bool IsInherited { get; init; }
}
