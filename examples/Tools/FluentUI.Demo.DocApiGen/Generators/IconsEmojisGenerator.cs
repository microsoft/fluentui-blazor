// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using FluentUI.Demo.DocApiGen.Abstractions;

namespace FluentUI.Demo.DocApiGen.Generators;

/// <summary>
/// Generates Summary mode documentation (only [Parameter] properties).
/// Supports JSON and C# output formats.
/// </summary>
public sealed class IconsEmojisGenerator : DocumentationGeneratorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IconsEmojisGenerator"/> class.
    /// </summary>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    public IconsEmojisGenerator(Assembly assembly, FileInfo xmlDocumentation)
        : base(assembly, xmlDocumentation)
    {
    }

    /// <inheritdoc/>
    public override GenerationMode Mode => GenerationMode.Summary;
 
    /// <inheritdoc/>
    public override string Generate(IOutputFormatter formatter)
    {
        return "TODO";
    }
}