// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Abstractions;

/// <summary>
/// Defines the contract for output formatting.
/// </summary>
public interface IOutputFormatter
{
    /// <summary>
    /// Gets the format name (e.g., "json", "csharp").
    /// </summary>
    string FormatName { get; }

    /// <summary>
    /// Formats the documentation data into a string.
    /// </summary>
    /// <param name="data">The documentation data to format.</param>
    /// <returns>The formatted string.</returns>
    string Format(object data);
}
