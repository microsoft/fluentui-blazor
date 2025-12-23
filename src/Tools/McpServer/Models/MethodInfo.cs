// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.McpServer.Models;

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

    /// <summary>
    /// Determines whether the specified <see cref="MethodInfo"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">The other <see cref="MethodInfo"/> to compare.</param>
    /// <returns>True if the specified instance is equal to the current instance; otherwise, false.</returns>
    public virtual bool Equals(MethodInfo? other)
    {
        if (other is null)
        {
            return false;
        }

        return Name == other.Name &&
               ReturnType == other.ReturnType &&
               Description == other.Description &&
               Parameters.SequenceEqual(other.Parameters) &&
               IsInherited == other.IsInherited;
    }

    /// <summary>
    /// Returns a hash code for the current instance.
    /// </summary>
    /// <returns>A hash code for the current instance.</returns>
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Name);
        hashCode.Add(ReturnType);
        hashCode.Add(Description);
        foreach (var param in Parameters)
        {
            hashCode.Add(param);
        }

        hashCode.Add(IsInherited);
        return hashCode.ToHashCode();
    }
}
