// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using FluentUI.Demo.DocApiGen.Extensions;

namespace FluentUI.Demo.DocApiGen.Models;

/// <summary>
/// Represents a member of a class (Property, Method, Event).
/// </summary>
public record ApiMember
{
    /// <summary>
    /// Gets the MemberInfo for the member.
    /// </summary>
    public required MemberInfo MemberInfo { get; init; }

    /// <summary>
    /// Gets the type of the MemberInfo: Property, Method, Event.
    /// </summary>
    public MemberTypes MemberType { get; init; } = MemberTypes.Property;

    /// <summary>
    /// Gets the name of the MemberInfo.
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// Gets the return type of the MemberInfo.
    /// </summary>
    public string Type { get; init; } = "";

    /// <summary>
    /// Gets the list of enum values for the method / property (empty for Event).
    /// </summary>
    public string[] EnumValues { get; init; } = [];

    /// <summary>
    /// Gets the list of parameters for the method (empty for Property or Event).
    /// </summary>
    public string[] Parameters { get; init; } = [];

    /// <summary>
    /// Gets the default value for the MemberInfo.
    /// </summary>
    public string? Default { get; init; }

    /// <summary>
    /// Gets the description comment for the MemberInfo.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets true if the property is flagged with [Parameter] attribute
    /// </summary>
    public bool IsParameter { get; init; }

    /// <summary>
    /// Returns the identifier of the member.
    /// </summary>
    /// <returns></returns>
    public string GetSignature()
    {
        return MemberInfo.GetSignature();
    }

    /// <summary>
    /// Returns the signature of the method.
    /// </summary>
    /// <returns></returns>
    public string GetMethodSignature()
    {
        return $"{Type} {Name}({string.Join(", ", Parameters)})";
    }
}
