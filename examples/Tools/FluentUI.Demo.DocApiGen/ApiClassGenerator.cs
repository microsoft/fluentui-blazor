// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocApiGen.Extensions;
using FluentUI.Demo.DocApiGen.Models;
using System.Reflection;
using System.Text;

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

    /// <summary>
    /// Saves the documentation to a file.
    /// </summary>
    /// <param name="fileName"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "Not necessary")]
    public void SaveToFile(string fileName)
    {
        var code = new StringBuilder();

        code.AppendLine("/// <summary />");
        code.AppendLine("public static readonly IDictionary<string, IDictionary<string, string>> SummaryData = new Dictionary<string, IDictionary<string, string>>");
        code.AppendLine("{");

        foreach (var type in Assembly.GetTypes().Where(i => i.IsValidType()))
        {
            var apiClass = FromTypeName(type);
            var apiClassMembers = apiClass.ToDictionary();

            if (apiClassMembers.Any())
            {
                code.AppendLine($"    {{ \"{apiClass.Name}\", new Dictionary<string, string>");
                code.AppendLine($"        {{");

                foreach (var member in apiClass.ToDictionary())
                {
                    code.AppendLine($"            {{ \"{member.Key}\", \"{member.Value}\" }},");
                }

                code.AppendLine($"       }}");
                code.AppendLine($"    }}");
            }
        }

        code.AppendLine("}");

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        File.WriteAllText(fileName, code.ToString());
    }
}
