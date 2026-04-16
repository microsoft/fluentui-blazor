// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;

namespace FluentUI.Demo.DocApiGen.Models;

/// <summary>
/// Represents one documentation source consisting of an assembly and its XML documentation file.
/// </summary>
public sealed class DocumentationInput
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationInput"/> class.
    /// </summary>
    /// <param name="assembly">The assembly to document.</param>
    /// <param name="xmlDocumentation">The XML documentation file associated with the assembly.</param>
    public DocumentationInput(Assembly assembly, FileInfo xmlDocumentation)
    {
        Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        XmlDocumentation = xmlDocumentation ?? throw new ArgumentNullException(nameof(xmlDocumentation));

        if (!xmlDocumentation.Exists)
        {
            throw new FileNotFoundException($"XML documentation file not found: {xmlDocumentation.FullName}");
        }

        DocXmlReader = new LoxSmoke.DocXml.DocXmlReader(xmlDocumentation.FullName);
    }

    /// <summary>
    /// Gets the assembly to document.
    /// </summary>
    public Assembly Assembly { get; }

    /// <summary>
    /// Gets the XML documentation file associated with the assembly.
    /// </summary>
    public FileInfo XmlDocumentation { get; }

    /// <summary>
    /// Gets the XML documentation reader for the assembly.
    /// </summary>
    public LoxSmoke.DocXml.DocXmlReader DocXmlReader { get; }
}
