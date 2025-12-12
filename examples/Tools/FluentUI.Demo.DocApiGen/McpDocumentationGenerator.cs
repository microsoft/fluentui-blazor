// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentUI.Demo.DocApiGen.Extensions;
using FluentUI.Demo.DocApiGen.Models;
using FluentUI.Demo.DocApiGen.Models.McpDocumentation;

namespace FluentUI.Demo.DocApiGen;

/// <summary>
/// Generates MCP-compatible JSON documentation for the McpServer.
/// This allows the McpServer to consume pre-generated documentation
/// without needing the LoxSmoke.DocXml dependency at runtime.
/// </summary>
public class McpDocumentationGenerator
{
    private readonly Assembly _assembly;
    private readonly LoxSmoke.DocXml.DocXmlReader _docXmlReader;

    /// <summary>
    /// Initializes a new instance of the <see cref="McpDocumentationGenerator"/> class.
    /// </summary>
    public McpDocumentationGenerator(Assembly assembly, FileInfo xmlDocumentation)
    {
        _assembly = assembly;
        _docXmlReader = new LoxSmoke.DocXml.DocXmlReader(xmlDocumentation.FullName);
    }

    /// <summary>
    /// Generates the MCP documentation root containing all components and enums.
    /// </summary>
    public McpDocumentationRoot Generate()
    {
        var assemblyInfo = ApiClassGenerator.GetAssemblyInfo(_assembly);
        var components = GenerateComponents().ToList();
        var enums = GenerateEnums().ToList();

        return new McpDocumentationRoot
        {
            Metadata = new McpDocumentationMetadata
            {
                AssemblyVersion = assemblyInfo.Version,
                GeneratedDateUtc = assemblyInfo.Date,
                ComponentCount = components.Count,
                EnumCount = enums.Count
            },
            Components = components,
            Enums = enums
        };
    }

    /// <summary>
    /// Generates the JSON string for MCP documentation.
    /// </summary>
    public string GenerateJson(bool indented = true)
    {
        var root = Generate();
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = indented,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        return JsonSerializer.Serialize(root, jsonSerializerOptions);
    }

    /// <summary>
    /// Saves the MCP documentation to a JSON file.
    /// </summary>
    public void SaveToFile(string fileName, bool indented = true)
    {
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        File.WriteAllText(fileName, GenerateJson(indented));
    }

    /// <summary>
    /// Generates component information for all valid component types.
    /// </summary>
    private IEnumerable<McpComponentInfo> GenerateComponents()
    {
        foreach (var type in _assembly.GetTypes().Where(IsValidComponentType))
        {
            var componentInfo = GenerateComponentInfo(type);
            if (componentInfo != null)
            {
                yield return componentInfo;
            }
        }
    }

    /// <summary>
    /// Generates enum information for all public enums.
    /// </summary>
    private IEnumerable<McpEnumInfo> GenerateEnums()
    {
        foreach (var type in _assembly.GetTypes().Where(t => t.IsEnum && t.IsPublic))
        {
            yield return GenerateEnumInfo(type);
        }
    }

    /// <summary>
    /// Generates component information for a specific type using the ApiClass model.
    /// </summary>
    private McpComponentInfo? GenerateComponentInfo(Type type)
    {
        try
        {
            var options = new ApiClassOptions(_assembly, _docXmlReader)
            {
                PropertyParameterOnly = false // Include all properties, not just [Parameter] ones
            };

            var apiClass = new ApiClass(type, options);
            var category = DetermineCategory(type);

            var component = new McpComponentInfo
            {
                Name = apiClass.Name,
                FullName = type.FullName ?? type.Name,
                Summary = apiClass.Summary,
                Category = category,
                IsGeneric = type.IsGenericType,
                BaseClass = type.BaseType?.Name,
                Properties = new List<McpPropertyInfo>(),
                Events = new List<McpEventInfo>(),
                Methods = new List<McpMethodInfo>()
            };

            // Extract properties from ApiClass
            foreach (var property in apiClass.Properties)
            {
                var isInherited = property.MemberInfo.DeclaringType != type;

                component.Properties.Add(new McpPropertyInfo
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

            // Extract events from ApiClass
            foreach (var evt in apiClass.Events)
            {
                var isInherited = evt.MemberInfo.DeclaringType != type;

                component.Events.Add(new McpEventInfo
                {
                    Name = evt.Name,
                    Type = evt.Type,
                    Description = evt.Description,
                    IsInherited = isInherited
                });
            }

            // Extract methods from ApiClass
            foreach (var method in apiClass.Methods)
            {
                var isInherited = method.MemberInfo.DeclaringType != type;

                component.Methods.Add(new McpMethodInfo
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
            Console.WriteLine($"[McpDocGen] Warning: Could not process component {type.Name}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Generates enum information for a specific type.
    /// </summary>
    private McpEnumInfo GenerateEnumInfo(Type type)
    {
        var values = new List<McpEnumValueInfo>();
        var names = Enum.GetNames(type);
        var enumValues = Enum.GetValues(type);

        for (var i = 0; i < names.Length; i++)
        {
            var name = names[i];
            var value = Convert.ToInt32(enumValues.GetValue(i), System.Globalization.CultureInfo.InvariantCulture);
            var field = type.GetField(name);
            var description = field != null ? _docXmlReader.GetMemberSummary(field) : string.Empty;

            values.Add(new McpEnumValueInfo
            {
                Name = name,
                Value = value,
                Description = description
            });
        }

        return new McpEnumInfo
        {
            Name = type.Name,
            FullName = type.FullName ?? type.Name,
            Description = _docXmlReader.GetComponentSummary(type),
            Values = values
        };
    }

    /// <summary>
    /// Checks if a type is a valid FluentUI Blazor component type.
    /// </summary>
    private bool IsValidComponentType(Type type)
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
               IsFluentComponent(type);
    }

    /// <summary>
    /// Checks if a type implements IFluentComponentBase.
    /// </summary>
    private bool IsFluentComponent(Type type)
    {
        // Search for IFluentComponentBase interface in the assembly
        var fluentComponentBaseInterface = _assembly.GetTypes()
            .FirstOrDefault(t => t.Name == "IFluentComponentBase");

        if (fluentComponentBaseInterface != null)
        {
            return fluentComponentBaseInterface.IsAssignableFrom(type);
        }

        // Fallback: check if the type name starts with "Fluent" and has a base class containing "Component"
        return type.Name.StartsWith("Fluent", StringComparison.Ordinal) &&
               (type.BaseType?.Name.Contains("Component") ?? false);
    }

    /// <summary>
    /// Determines the category of a component based on its namespace or name.
    /// </summary>
    private static string DetermineCategory(Type type)
    {
        var ns = type.Namespace ?? string.Empty;
        var name = type.Name;

        // Extract category from namespace
        if (ns.Contains(".Components.", StringComparison.OrdinalIgnoreCase))
        {
            var parts = ns.Split('.');
            var componentsIndex = Array.IndexOf(parts, "Components");
            if (componentsIndex >= 0 && componentsIndex < parts.Length - 1)
            {
                return parts[componentsIndex + 1];
            }
        }

        // Categorize by component name patterns
        return GetCategoryFromName(name);
    }

    /// <summary>
    /// Gets category from component name patterns.
    /// </summary>
    private static string GetCategoryFromName(string name)
    {
        if (name.Contains("Button", StringComparison.OrdinalIgnoreCase))
        {
            return "Button";
        }

        if (name.Contains("Input", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("TextField", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("TextArea", StringComparison.OrdinalIgnoreCase))
        {
            return "Input";
        }

        if (name.Contains("Dialog", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Modal", StringComparison.OrdinalIgnoreCase))
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
            name.Contains("Stack", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Splitter", StringComparison.OrdinalIgnoreCase))
        {
            return "Layout";
        }

        return "Components";
    }
}
