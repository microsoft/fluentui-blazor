// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using FluentUI.Demo.DocApiGen.Abstractions;
using FluentUI.Demo.DocApiGen.Models;

namespace FluentUI.Demo.DocApiGen.Generators;

/// <summary>
/// Base class for documentation generators implementing common functionality.
/// </summary>
public abstract class DocumentationGeneratorBase : IDocumentationGenerator
{
    /// <summary>
    /// Represents the documentation inputs associated with the current operation.
    /// </summary>
    protected readonly IReadOnlyList<DocumentationInput> Inputs;

    /// <summary>
    /// Gets the primary documentation input.
    /// </summary>
    protected DocumentationInput PrimaryInput => Inputs[0];

    /// <summary>
    /// Gets the primary assembly.
    /// </summary>
    protected Assembly PrimaryAssembly => PrimaryInput.Assembly;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationGeneratorBase"/> class.
    /// </summary>
    /// <param name="inputs">The documentation inputs to generate documentation for.</param>
    protected DocumentationGeneratorBase(IReadOnlyList<DocumentationInput> inputs)
    {
        ArgumentNullException.ThrowIfNull(inputs);

        if (inputs.Count == 0)
        {
            throw new ArgumentException("At least one documentation input is required.", nameof(inputs));
        }

        Inputs = inputs;
    }

    /// <inheritdoc/>
    public abstract GenerationMode Mode { get; }

    /// <inheritdoc/>
    public abstract string Generate(IOutputFormatter formatter);

    /// <inheritdoc/>
    public virtual void SaveToFile(string filePath, IOutputFormatter formatter)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        ArgumentNullException.ThrowIfNull(formatter);

        var output = Generate(formatter);

        // Delete existing file if it exists
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // Ensure directory exists
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, output);
    }
}
