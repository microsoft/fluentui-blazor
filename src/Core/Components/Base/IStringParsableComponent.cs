// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines an interface for components with values that can be parsed from a string.
/// </summary>
public interface IStringParsableComponent
{
    /// <summary>
    /// Gets or sets the error message to show when the field can not be parsed.
    /// </summary>
    public string ParsingErrorMessage { get; set; }
}
