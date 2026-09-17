// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Abstractions;

namespace FluentUI.Demo.DocApiGen.Formatters;

/// <summary>
/// Factory for creating output formatters.
/// </summary>
public static class OutputFormatterFactory
{
    /// <summary>
    /// Creates an output formatter for the specified format name.
    /// </summary>
    /// <param name="formatName">The format name ("json" or "csharp").</param>
    /// <param name="indented">Whether to indent the output (JSON only).</param>
    /// <param name="useCompactFormat">Whether to use compact format for Summary mode (default true).</param>
    /// <returns>An output formatter instance.</returns>
    /// <exception cref="ArgumentException">Thrown when formatName is null or empty.</exception>
    /// <exception cref="NotSupportedException">Thrown when the format is not supported.</exception>
    public static IOutputFormatter Create(string formatName, bool indented = true, bool useCompactFormat = true)
    {
        if (string.IsNullOrWhiteSpace(formatName))
        {
            throw new ArgumentException("Format name cannot be null or empty.", nameof(formatName));
        }

        return formatName.ToLowerInvariant() switch
        {
            "json" => new JsonOutputFormatter(indented, useCompactFormat),
            "csharp" or "cs" => new CSharpOutputFormatter(),
            _ => throw new NotSupportedException($"Output format '{formatName}' is not supported. Supported formats: json, csharp.")
        };
    }

    /// <summary>
    /// Creates a JSON output formatter.
    /// </summary>
    /// <param name="indented">Whether to indent the JSON output.</param>
    /// <param name="useCompactFormat">Whether to use compact format for Summary mode (default true).</param>
    /// <returns>A JSON output formatter.</returns>
    public static IOutputFormatter CreateJsonFormatter(bool indented = true, bool useCompactFormat = true)
    {
        return new JsonOutputFormatter(indented, useCompactFormat);
    }

    /// <summary>
    /// Creates a C# output formatter.
    /// </summary>
    /// <returns>A C# output formatter.</returns>
    public static IOutputFormatter CreateCSharpFormatter()
    {
        return new CSharpOutputFormatter();
    }
}
