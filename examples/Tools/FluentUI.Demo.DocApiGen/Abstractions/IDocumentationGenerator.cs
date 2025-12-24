// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Abstractions;

/// <summary>
/// Defines the contract for documentation generation.
/// </summary>
public interface IDocumentationGenerator
{
    /// <summary>
    /// Gets the generation mode (Summary or All).
    /// </summary>
    GenerationMode Mode { get; }

    /// <summary>
    /// Generates documentation and returns it in the specified format.
    /// </summary>
    /// <param name="formatter">The output formatter to use.</param>
    /// <returns>The formatted documentation string.</returns>
    string Generate(IOutputFormatter formatter);

    /// <summary>
    /// Saves the generated documentation to a file.
    /// </summary>
    /// <param name="filePath">The output file path.</param>
    /// <param name="formatter">The output formatter to use.</param>
    void SaveToFile(string filePath, IOutputFormatter formatter);
}

/// <summary>
/// Defines the generation mode.
/// </summary>
public enum GenerationMode
{
    /// <summary>
    /// Generate summary documentation with only [Parameter] properties.
    /// </summary>
    Summary,

    /// <summary>
    /// Generate complete documentation including all properties, methods, and events.
    /// </summary>
    All
}
