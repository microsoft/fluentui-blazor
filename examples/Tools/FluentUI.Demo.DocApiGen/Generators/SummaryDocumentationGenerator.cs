// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using FluentUI.Demo.DocApiGen.Abstractions;
using FluentUI.Demo.DocApiGen.Extensions;
using FluentUI.Demo.DocApiGen.Models.SummaryMode;

namespace FluentUI.Demo.DocApiGen.Generators;

/// <summary>
/// Generates Summary mode documentation (only [Parameter] properties).
/// Supports JSON and C# output formats.
/// </summary>
public sealed class SummaryDocumentationGenerator : DocumentationGeneratorBase
{
    private readonly LoxSmoke.DocXml.DocXmlReader _docXmlReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="SummaryDocumentationGenerator"/> class.
    /// </summary>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    public SummaryDocumentationGenerator(Assembly assembly, FileInfo xmlDocumentation)
        : base(assembly, xmlDocumentation)
    {
        _docXmlReader = new LoxSmoke.DocXml.DocXmlReader(xmlDocumentation.FullName);
    }

    /// <inheritdoc/>
    public override GenerationMode Mode => GenerationMode.Summary;

    /// <inheritdoc/>
    public override string Generate(IOutputFormatter formatter)
    {
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        var data = BuildDocumentationData();
        return formatter.Format(data);
    }

    /// <summary>
    /// Builds the documentation data structure.
    /// </summary>
    private SummaryDocumentationData BuildDocumentationData()
    {
        var (version, date) = GetAssemblyInfo();
        var components = new Dictionary<string, ComponentEntry>();

        var options = new ApiClassOptions(Assembly, _docXmlReader)
        {
            Mode = GenerationMode.Summary
        };

        var validTypes = Assembly.GetTypes().Where(t => t.IsValidType()).ToList();
        Console.WriteLine($"Processing {validTypes.Count} valid types...");

        var processedCount = 0;
        foreach (var type in validTypes)
        {
            processedCount++;
            if (processedCount % 10 == 0)
            {
                Console.Write($"\rProcessed {processedCount}/{validTypes.Count} types...");
            }

            try
            {
                var apiClass = new ApiClass(type, options);
                var members = apiClass.ToDictionary();

                if (members.Any())
                {
                    // Add class summary
                    var classKey = $"{type.FullName}.__summary__";
                    components[classKey] = new ComponentEntry
                    {
                        Summary = apiClass.Summary,
                        Signature = type.Name
                    };

                    // Add members
                    foreach (var member in members)
                    {
                        var memberKey = $"{type.FullName}.{member.Key}";
                        components[memberKey] = new ComponentEntry
                        {
                            Summary = member.Value,
                            Signature = member.Key
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"[WARNING] Error processing type {type.FullName}: {ex.Message}");
            }
        }

        Console.WriteLine();
        Console.WriteLine($"✓ Processed {validTypes.Count} types, generated {components.Count} documentation entries.");

        return new SummaryDocumentationData
        {
            Metadata = new SummaryMetadata
            {
                AssemblyVersion = version,
                DateUtc = date,
                Mode = Mode.ToString()
            },
            Components = components
        };
    }

    /// <summary>
    /// Gets assembly version and current date.
    /// </summary>
    private (string Version, string Date) GetAssemblyInfo()
    {
        var version = "Unknown";
        
        var versionAttribute = Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (versionAttribute != null)
        {
            var versionString = versionAttribute.InformationalVersion;
            var plusIndex = versionString.IndexOf('+');
            
            if (plusIndex >= 0 && plusIndex + 9 < versionString.Length)
            {
                version = versionString[..(plusIndex + 9)];
            }
            else
            {
                version = versionString;
            }
        }

        var date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        
        return (version, date);
    }
}
