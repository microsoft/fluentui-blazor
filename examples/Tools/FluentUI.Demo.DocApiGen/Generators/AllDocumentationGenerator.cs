// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Reflection;
using FluentUI.Demo.DocApiGen.Abstractions;
using FluentUI.Demo.DocApiGen.Extensions;
using FluentUI.Demo.DocApiGen.Models.AllMode;
using FluentUI.Demo.DocApiGen.Models.SummaryMode;

namespace FluentUI.Demo.DocApiGen.Generators;

/// <summary>
/// Generates All mode documentation (complete: all properties, methods, events).
/// Supports JSON output format only.
/// </summary>
public sealed class AllDocumentationGenerator : DocumentationGeneratorBase
{
    private readonly LoxSmoke.DocXml.DocXmlReader _docXmlReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="AllDocumentationGenerator"/> class.
    /// </summary>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    public AllDocumentationGenerator(Assembly assembly, FileInfo xmlDocumentation)
        : base(assembly, xmlDocumentation)
    {
        _docXmlReader = new LoxSmoke.DocXml.DocXmlReader(xmlDocumentation.FullName);
    }

    /// <inheritdoc/>
    public override GenerationMode Mode => GenerationMode.All;

    /// <inheritdoc/>
    public override string Generate(IOutputFormatter formatter)
    {
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        if (formatter.FormatName != "json")
        {
            throw new NotSupportedException(
                $"AllDocumentationGenerator only supports JSON format. Requested format: {formatter.FormatName}");
        }

        var data = BuildDocumentationData();
        return formatter.Format(data);
    }

    /// <summary>
    /// Builds the complete documentation data structure.
    /// </summary>
    private DocumentationRoot BuildDocumentationData()
    {
        var (version, date) = GetAssemblyInfo();
        var components = new List<ComponentInfo>();
        var enums = new List<EnumInfo>();

        var validTypes = Assembly.GetTypes().Where(IsValidComponentType).ToList();
        var enumTypes = Assembly.GetTypes().Where(t => t.IsEnum && t.IsPublic).ToList();

        Console.WriteLine($"Processing {validTypes.Count} components and {enumTypes.Count} enums...");

        // Generate components
        var componentCount = 0;
        foreach (var type in validTypes)
        {
            componentCount++;
            if (componentCount % 10 == 0)
            {
                Console.Write($"\rProcessed {componentCount}/{validTypes.Count} components...");
            }

            var componentInfo = GenerateComponentInfo(type);
            if (componentInfo != null)
            {
                components.Add(componentInfo);
            }
        }

        Console.WriteLine();

        // Generate enums
        var enumCount = 0;
        foreach (var type in enumTypes)
        {
            enumCount++;
            enums.Add(GenerateEnumInfo(type));
        }

        Console.WriteLine($"✓ Processed {validTypes.Count} components and {enumTypes.Count} enums.");

        return new DocumentationRoot
        {
            Metadata = new DocumentationMetadata
            {
                AssemblyVersion = version,
                GeneratedDateUtc = date,
                ComponentCount = components.Count,
                EnumCount = enums.Count
            },
            Components = components,
            Enums = enums
        };
    }

    /// <summary>
    /// Generates component information for a specific type.
    /// </summary>
    private ComponentInfo? GenerateComponentInfo(Type type)
    {
        try
        {
            var options = new ApiClassOptions(Assembly, _docXmlReader)
            {
                Mode = GenerationMode.All
            };

            var apiClass = new ApiClass(type, options);

            var component = new ComponentInfo
            {
                Name = apiClass.Name,
                FullName = type.FullName ?? type.Name,
                Summary = apiClass.Summary,
                Category = DetermineCategory(type),
                IsGeneric = type.IsGenericType,
                BaseClass = type.BaseType?.Name,
                Properties = [],
                Events = [],
                Methods = []
            };

            // Extract properties
            foreach (var property in apiClass.Properties)
            {
                var isInherited = property.MemberInfo.DeclaringType != type;

                component.Properties.Add(new Models.AllMode.PropertyInfo
                {
                    Name = property.Name,
                    Type = property.Type,
                    Description = property.Description,
                    IsParameter = property.IsParameter,
                    IsInherited = isInherited,
                    DefaultValue = property.Default,
                    EnumValues = property.EnumValues
                });
            }

            // Extract events
            foreach (var evt in apiClass.Events)
            {
                var isInherited = evt.MemberInfo.DeclaringType != type;

                component.Events.Add(new Models.AllMode.EventInfo
                {
                    Name = evt.Name,
                    Type = evt.Type,
                    Description = evt.Description,
                    IsInherited = isInherited
                });
            }

            // Extract methods
            foreach (var method in apiClass.Methods)
            {
                var isInherited = method.MemberInfo.DeclaringType != type;

                component.Methods.Add(new Models.AllMode.MethodInfo
                {
                    Name = method.Name,
                    ReturnType = method.Type,
                    Description = method.Description,
                    Parameters = method.Parameters,
                    IsInherited = isInherited
                });
            }

            return component;
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"[WARNING] Error processing component {type.FullName}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Generates enum information for a specific type.
    /// </summary>
    private EnumInfo GenerateEnumInfo(Type type)
    {
        var values = new List<EnumValueInfo>();
        var names = Enum.GetNames(type);
        var enumValues = Enum.GetValues(type);

        for (var i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var value = Convert.ToInt32(enumValues.GetValue(i), CultureInfo.InvariantCulture);
            var field = type.GetField(name);
            var description = field != null ? _docXmlReader.GetMemberSummary(field) : string.Empty;

            values.Add(new EnumValueInfo
            {
                Name = name,
                Value = value,
                Description = description
            });
        }

        return new EnumInfo
        {
            Name = type.Name,
            FullName = type.FullName ?? type.Name,
            Description = _docXmlReader.GetComponentSummary(type),
            Values = values
        };
    }

    /// <summary>
    /// Checks if a type is a valid component type.
    /// </summary>
    private static bool IsValidComponentType(Type type)
    {
        return type != null &&
               type.IsPublic &&
               !type.IsAbstract &&
               !type.IsInterface &&
               type.IsClass &&
               !Constants.EXCLUDE_TYPES.Contains(type.Name) &&
               !type.Name.Contains('<') &&
               !type.Name.Contains('>') &&
               !type.Name.EndsWith("_g", StringComparison.Ordinal) &&
               type.IsValidType();
    }

    /// <summary>
    /// Determines the category of a component based on its name.
    /// </summary>
    private static string DetermineCategory(Type type)
    {
        var name = type.Name;

        if (name.Contains("Button", StringComparison.OrdinalIgnoreCase))
        {
            return "Button";
        }

        if (name.Contains("Input", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("TextField", StringComparison.OrdinalIgnoreCase))
        {
            return "Input";
        }

        if (name.Contains("Dialog", StringComparison.OrdinalIgnoreCase))
        {
            return "Dialog";
        }

        if (name.Contains("Menu", StringComparison.OrdinalIgnoreCase))
        {
            return "Menu";
        }

        if (name.Contains("Nav", StringComparison.OrdinalIgnoreCase))
        {
            return "Navigation";
        }

        if (name.Contains("Grid", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Table", StringComparison.OrdinalIgnoreCase))
        {
            return "DataGrid";
        }

        if (name.Contains("Card", StringComparison.OrdinalIgnoreCase))
        {
            return "Card";
        }

        if (name.Contains("Icon", StringComparison.OrdinalIgnoreCase))
        {
            return "Icon";
        }

        if (name.Contains("Layout", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Stack", StringComparison.OrdinalIgnoreCase))
        {
            return "Layout";
        }

        return "Components";
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
