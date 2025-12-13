// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using FluentUI.Demo.DocApiGen.Abstractions;

namespace FluentUI.Demo.DocApiGen.Generators;

/// <summary>
/// Base class for documentation generators implementing common functionality.
/// </summary>
public abstract class DocumentationGeneratorBase : IDocumentationGenerator
{
    /// <summary>
    /// Represents the assembly associated with the current context or operation.
    /// </summary>
    protected readonly Assembly Assembly;

    /// <summary>
    /// Represents the XML documentation file associated with the assembly.
    /// </summary>
    protected readonly FileInfo XmlDocumentation;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationGeneratorBase"/> class.
    /// </summary>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    protected DocumentationGeneratorBase(Assembly assembly, FileInfo xmlDocumentation)
    {
        Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        XmlDocumentation = xmlDocumentation ?? throw new ArgumentNullException(nameof(xmlDocumentation));

        if (!xmlDocumentation.Exists)
        {
            throw new FileNotFoundException($"XML documentation file not found: {xmlDocumentation.FullName}");
        }
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

        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

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
