// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Models;
using System.Reflection;

namespace FluentUI.Demo.DocApiGen;

/// <summary>
/// Engine to generate the documentation classes.
/// </summary>
public class ApiClassGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClassOptions"/> class.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="xmlDocumentation"></param>
    public ApiClassGenerator(Assembly assembly, FileInfo xmlDocumentation)
    {
        Assembly = assembly;
        DocXmlReader = new LoxSmoke.DocXml.DocXmlReader(xmlDocumentation.FullName);
    }

    /// <summary>
    /// Gets the assembly to generate the documentation.
    /// </summary>
    public Assembly Assembly { get; }

    /// <summary>
    /// Gets the summary reader.
    /// </summary>
    public LoxSmoke.DocXml.DocXmlReader DocXmlReader { get; }

    /// <summary>
    /// Gets the <see cref="ApiClass"/> for the specified component.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public ApiClass FromTypeName(Type type)
    {
        var options = new ApiClassOptions(Assembly, DocXmlReader)
        {
            PropertyParameterOnly = false,
        };

        return new ApiClass(type, options);
    }
}
