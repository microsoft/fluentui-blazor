// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using FluentUI.Demo.DocApiGen.Abstractions;

namespace FluentUI.Demo.DocApiGen.Generators;

/// <summary>
/// Factory for creating documentation generators.
/// </summary>
public static class DocumentationGeneratorFactory
{
    /// <summary>
    /// Creates a documentation generator for the specified mode.
    /// </summary>
    /// <param name="mode">The generation mode.</param>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    /// <returns>A documentation generator instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when assembly or xmlDocumentation is null.</exception>
    /// <exception cref="NotSupportedException">Thrown when the mode is not supported.</exception>
    public static IDocumentationGenerator Create(
        GenerationMode mode,
        Assembly assembly,
        FileInfo xmlDocumentation)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        ArgumentNullException.ThrowIfNull(xmlDocumentation);

        return mode switch
        {
            GenerationMode.Summary => new SummaryDocumentationGenerator(assembly, xmlDocumentation),
            GenerationMode.All => new AllDocumentationGenerator(assembly, xmlDocumentation),
            GenerationMode.Mcp => new McpDocumentationGenerator(assembly, xmlDocumentation),
            GenerationMode.Icons => new IconsEmojisGenerator(assembly, xmlDocumentation, mode),
            GenerationMode.Emojis => new IconsEmojisGenerator(assembly, xmlDocumentation, mode),
            _ => throw new NotSupportedException($"Generation mode '{mode}' is not supported.")
        };
    }

    /// <summary>
    /// Creates a Summary mode documentation generator.
    /// </summary>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    /// <returns>A Summary mode documentation generator.</returns>
    public static IDocumentationGenerator CreateSummaryGenerator(
        Assembly assembly,
        FileInfo xmlDocumentation)
    {
        return Create(GenerationMode.Summary, assembly, xmlDocumentation);
    }

    /// <summary>
    /// Creates an All mode documentation generator.
    /// </summary>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    /// <returns>An All mode documentation generator.</returns>
    public static IDocumentationGenerator CreateAllGenerator(
        Assembly assembly,
        FileInfo xmlDocumentation)
    {
        return Create(GenerationMode.All, assembly, xmlDocumentation);
    }

    /// <summary>
    /// Creates an MCP mode documentation generator.
    /// </summary>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    /// <returns>An MCP mode documentation generator.</returns>
    public static IDocumentationGenerator CreateMcpGenerator(
        Assembly assembly,
        FileInfo xmlDocumentation)
    {
        return Create(GenerationMode.Mcp, assembly, xmlDocumentation);
    }
}
