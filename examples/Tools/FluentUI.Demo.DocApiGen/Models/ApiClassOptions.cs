// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;

namespace FluentUI.Demo.DocApiGen.Models;

/// <summary>
/// Represents the options for the class generation.
/// </summary>
public class ApiClassOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClassOptions"/> class.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="xmlDocumentation"></param>
    public ApiClassOptions(Assembly assembly, FileInfo xmlDocumentation)
    {
        Assembly = assembly;
        XmlDocumentation = xmlDocumentation;
        DocXmlReader = new(xmlDocumentation.FullName);
    }

    /// <summary>
    /// Gets the assembly to generate the documentation.
    /// </summary>
    public Assembly Assembly { get; }

    /// <summary>
    /// Gets the XML documentation file.
    /// </summary>
    public FileInfo XmlDocumentation { get; }

    /// <summary>
    /// Gets the summary reader.
    /// </summary>
    internal LoxSmoke.DocXml.DocXmlReader DocXmlReader { get; }

    /// <summary>
    /// Gets or sets whether to include all properties (false) or only those with [Parameter] attribute (true).
    /// </summary>
    public bool PropertyParameterOnly { get; set; } = true;
}
