// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentUI.Demo.DocApiGen.Extensions;
using FluentUI.Demo.DocApiGen.Models.McpDocumentation;

namespace FluentUI.Demo.DocApiGen;

/// <summary>
/// Generates MCP-compatible JSON documentation for the McpServer.
/// This allows the McpServer to consume pre-generated documentation
/// without needing the LoxSmoke.DocXml dependency at runtime.
/// </summary>
public class McpDocumentationGenerator
{
    private static readonly string[] MEMBERS_TO_EXCLUDE =
    [
        "AdditionalAttributes",
        "ParentReference",
        "Element",
        "Equals",
        "GetHashCode",
        "GetType",
        "SetParametersAsync",
        "ToString",
        "Dispose",
        "DisposeAsync",
        "ValueExpression",
    ];

    private static readonly string[] EXCLUDE_TYPES =
    [
        "TypeInference",
        "InternalListContext`1",
        "SpacingGenerator",
        "FluentLocalizerInternal",
        "FluentJSModule",
        "FluentServiceProviderException`1",
    ];

    private readonly Assembly _assembly;
    private readonly LoxSmoke.DocXml.DocXmlReader _docXmlReader;

    // Cached type references discovered from the assembly
    private readonly Type? _fluentComponentBaseInterface;
    private readonly Type? _parameterAttributeType;
    private readonly Type? _eventCallbackType;
    private readonly Type? _eventCallbackGenericType;
    private readonly Type? _jsInvokableAttributeType;

    /// <summary>
    /// Initializes a new instance of the <see cref="McpDocumentationGenerator"/> class.
    /// </summary>
    public McpDocumentationGenerator(Assembly assembly, FileInfo xmlDocumentation)
    {
        _assembly = assembly;
        _docXmlReader = new LoxSmoke.DocXml.DocXmlReader(xmlDocumentation.FullName);

        // Discover types from the loaded assembly and its references
        _fluentComponentBaseInterface = FindTypeByName("IFluentComponentBase");
        _parameterAttributeType = FindTypeByName("ParameterAttribute") 
            ?? Type.GetType("Microsoft.AspNetCore.Components.ParameterAttribute, Microsoft.AspNetCore.Components");
        _eventCallbackType = Type.GetType("Microsoft.AspNetCore.Components.EventCallback, Microsoft.AspNetCore.Components");
        _eventCallbackGenericType = Type.GetType("Microsoft.AspNetCore.Components.EventCallback`1, Microsoft.AspNetCore.Components");
        _jsInvokableAttributeType = Type.GetType("Microsoft.JSInterop.JSInvokableAttribute, Microsoft.JSInterop");
    }

    /// <summary>
    /// Finds a type by name in the loaded assembly or its referenced assemblies.
    /// </summary>
    private Type? FindTypeByName(string typeName)
    {
        // First, search in the main assembly
        var type = _assembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
        if (type != null)
        {
            return type;
        }

        // Search in referenced assemblies
        foreach (var refAssemblyName in _assembly.GetReferencedAssemblies())
        {
            try
            {
                var refAssembly = Assembly.Load(refAssemblyName);
                type = refAssembly.GetTypes().FirstOrDefault(t => t.Name == typeName);
                if (type != null)
                {
                    return type;
                }
            }
            catch
            {
                // Ignore assemblies that can't be loaded
            }
        }

        return null;
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
        var options = new JsonSerializerOptions
        {
            WriteIndented = indented,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        return JsonSerializer.Serialize(root, options);
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
    /// Generates component information for a specific type.
    /// </summary>
    private McpComponentInfo? GenerateComponentInfo(Type type)
    {
        try
        {
            var summary = GetTypeSummary(type);
            var category = DetermineCategory(type);

            var component = new McpComponentInfo
            {
                Name = type.Name,
                FullName = type.FullName ?? type.Name,
                Summary = summary,
                Category = category,
                IsGeneric = type.IsGenericType,
                BaseClass = type.BaseType?.Name,
                Properties = [],
                Events = [],
                Methods = []
            };

            ExtractMembers(type, component);

            return component;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[McpDocGen] Warning: Could not process component {type.Name}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Extracts properties, events, and methods from a type.
    /// </summary>
    private void ExtractMembers(Type type, McpComponentInfo component)
    {
        // Extract properties and events
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (MEMBERS_TO_EXCLUDE.Contains(prop.Name))
            {
                continue;
            }

            var isObsolete = prop.GetCustomAttribute<ObsoleteAttribute>() != null;
            if (isObsolete)
            {
                continue;
            }

            var isParameter = HasAttribute(prop, _parameterAttributeType);
            var isEvent = IsEventCallback(prop.PropertyType);
            var isInherited = prop.DeclaringType != type;
            var description = GetMemberSummary(prop);

            if (isEvent)
            {
                component.Events.Add(new McpEventInfo
                {
                    Name = prop.Name,
                    Type = prop.ToTypeNameString(),
                    Description = description,
                    IsInherited = isInherited
                });
            }
            else
            {
                component.Properties.Add(new McpPropertyInfo
                {
                    Name = prop.Name,
                    Type = prop.ToTypeNameString(),
                    Description = description,
                    IsParameter = isParameter,
                    IsInherited = isInherited,
                    DefaultValue = GetDefaultValue(type, prop),
                    EnumValues = GetEnumValues(prop.PropertyType)
                });
            }
        }

        // Extract methods
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (method.IsSpecialName || MEMBERS_TO_EXCLUDE.Contains(method.Name))
            {
                continue;
            }

            var isObsolete = method.GetCustomAttribute<ObsoleteAttribute>() != null;
            var isJSInvokable = HasAttribute(method, _jsInvokableAttributeType);
            if (isObsolete || isJSInvokable)
            {
                continue;
            }

            var genericArgs = method.IsGenericMethod
                ? "<" + string.Join(", ", method.GetGenericArguments().Select(a => a.Name)) + ">"
                : "";

            component.Methods.Add(new McpMethodInfo
            {
                Name = method.Name + genericArgs,
                ReturnType = method.ToTypeNameString(),
                Description = GetMemberSummary(method),
                Parameters = method.GetParameters().Select(p => $"{p.ToTypeNameString()} {p.Name}").ToArray(),
                IsInherited = false
            });
        }
    }

    /// <summary>
    /// Checks if a member has a specific attribute type.
    /// </summary>
    private static bool HasAttribute(MemberInfo member, Type? attributeType)
    {
        if (attributeType == null)
        {
            return false;
        }

        return member.GetCustomAttributes(attributeType, true).Length > 0;
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
            var description = field != null ? GetMemberSummary(field) : string.Empty;

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
            Description = GetTypeSummary(type),
            Values = values
        };
    }

    /// <summary>
    /// Gets the summary for a type from XML documentation.
    /// </summary>
    private string GetTypeSummary(Type type)
    {
        try
        {
            var comments = _docXmlReader.GetTypeComments(type);
            return CleanSummary(comments.Summary);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Gets the summary for a member from XML documentation.
    /// </summary>
    private string GetMemberSummary(MemberInfo member)
    {
        try
        {
            var comments = _docXmlReader.GetMemberComments(member);
            return CleanSummary(comments.Summary);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Cleans up the summary text.
    /// </summary>
    private static string CleanSummary(string? summary)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            return string.Empty;
        }

        // Remove XML tags and normalize whitespace
        var cleaned = System.Text.RegularExpressions.Regex.Replace(summary, @"<[^>]+>", " ");
        cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s+", " ");
        return cleaned.Trim();
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
               !EXCLUDE_TYPES.Contains(type.Name) &&
               !type.Name.Contains('<') &&
               !type.Name.Contains('>') &&
               !type.Name.EndsWith("_g") &&
               IsFluentComponent(type);
    }

    /// <summary>
    /// Checks if a type implements IFluentComponentBase.
    /// </summary>
    private bool IsFluentComponent(Type type)
    {
        if (_fluentComponentBaseInterface == null)
        {
            // Fallback: check if the type name starts with "Fluent" and has a base class containing "Component"
            return type.Name.StartsWith("Fluent", StringComparison.Ordinal) &&
                   (type.BaseType?.Name.Contains("Component") ?? false);
        }

        return _fluentComponentBaseInterface.IsAssignableFrom(type);
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

    /// <summary>
    /// Checks if a type is an EventCallback.
    /// </summary>
    private bool IsEventCallback(Type type)
    {
        if (_eventCallbackType != null && type == _eventCallbackType)
        {
            return true;
        }

        if (_eventCallbackGenericType != null && type.IsGenericType)
        {
            var genericDef = type.GetGenericTypeDefinition();
            return genericDef == _eventCallbackGenericType || 
                   genericDef.FullName == "Microsoft.AspNetCore.Components.EventCallback`1";
        }

        // Fallback: check by type name
        return type.FullName?.StartsWith("Microsoft.AspNetCore.Components.EventCallback") ?? false;
    }

    /// <summary>
    /// Gets the enum values for a type.
    /// </summary>
    private static string[] GetEnumValues(Type type)
    {
        var actualType = Nullable.GetUnderlyingType(type) ?? type;
        if (actualType.IsEnum)
        {
            return Enum.GetNames(actualType);
        }

        return [];
    }

    /// <summary>
    /// Gets the default value for a property.
    /// </summary>
    private static string? GetDefaultValue(Type componentType, PropertyInfo property)
    {
        try
        {
            // Only get default values for value types and strings
            if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string))
            {
                return null;
            }

            // Try to create an instance to get default values
            object? instance;
            if (componentType.IsGenericType)
            {
                var genericType = componentType.MakeGenericType(typeof(string));
                instance = Activator.CreateInstance(genericType);
            }
            else
            {
                instance = Activator.CreateInstance(componentType);
            }

            if (instance == null)
            {
                return null;
            }

            var value = property.GetValue(instance);
            return value?.ToString();
        }
        catch
        {
            return null;
        }
    }
}
