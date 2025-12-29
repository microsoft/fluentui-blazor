// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using FluentUI.Demo.DocApiGen.Abstractions;
using FluentUI.Demo.DocApiGen.Extensions;
using FluentUI.Demo.DocApiGen.Models.McpMode;

namespace FluentUI.Demo.DocApiGen.Generators;

/// <summary>
/// Generates MCP server documentation (tools, resources, prompts).
/// Supports JSON output format only.
/// </summary>
public sealed class McpDocumentationGenerator : DocumentationGeneratorBase
{
    private readonly LoxSmoke.DocXml.DocXmlReader _docXmlReader;

    // MCP attribute type names (we check by name to avoid assembly dependency)
    private const string McpServerToolTypeAttribute = "McpServerToolTypeAttribute";
    private const string McpServerToolAttribute = "McpServerToolAttribute";
    private const string McpServerResourceTypeAttribute = "McpServerResourceTypeAttribute";
    private const string McpServerResourceAttribute = "McpServerResourceAttribute";
    private const string McpServerPromptTypeAttribute = "McpServerPromptTypeAttribute";
    private const string McpServerPromptAttribute = "McpServerPromptAttribute";

    /// <summary>
    /// Initializes a new instance of the <see cref="McpDocumentationGenerator"/> class.
    /// </summary>
    /// <param name="assembly">The assembly to generate documentation for.</param>
    /// <param name="xmlDocumentation">The XML documentation file.</param>
    public McpDocumentationGenerator(Assembly assembly, FileInfo xmlDocumentation)
        : base(assembly, xmlDocumentation)
    {
        _docXmlReader = new LoxSmoke.DocXml.DocXmlReader(xmlDocumentation.FullName);
    }

    /// <inheritdoc/>
    public override GenerationMode Mode => GenerationMode.Mcp;

    /// <inheritdoc/>
    public override string Generate(IOutputFormatter formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);

        if (formatter.FormatName != "json")
        {
            throw new NotSupportedException(
                $"McpDocumentationGenerator only supports JSON format. Requested format: {formatter.FormatName}");
        }

        var data = BuildMcpDocumentationData();
        return formatter.Format(data);
    }

    /// <summary>
    /// Builds the MCP documentation data structure.
    /// </summary>
    private McpDocumentationRoot BuildMcpDocumentationData()
    {
        var (version, date) = GetAssemblyInfo();
        var tools = new List<McpToolInfo>();
        var resources = new List<McpResourceInfo>();
        var prompts = new List<McpPromptInfo>();

        var allTypes = Assembly.GetTypes().Where(t => t.IsClass && t.IsPublic && !t.IsAbstract).ToList();

        Console.WriteLine($"Scanning {allTypes.Count} types for MCP attributes...");

        foreach (var type in allTypes)
        {
            // Check for Tool types
            if (HasAttribute(type, McpServerToolTypeAttribute))
            {
                tools.AddRange(
                    GetMethodsWithAttribute(type, McpServerToolAttribute)
                        .Select(method => ExtractToolInfo(type, method))
                        .Where(toolInfo => toolInfo != null)!);
            }

            // Check for Resource types
            if (HasAttribute(type, McpServerResourceTypeAttribute))
            {
                resources.AddRange(
                    GetMethodsWithAttribute(type, McpServerResourceAttribute)
                        .Select(method => ExtractResourceInfo(type, method))
                        .Where(resourceInfo => resourceInfo != null)!);
            }

            // Check for Prompt types
            if (HasAttribute(type, McpServerPromptTypeAttribute))
            {
                prompts.AddRange(
                    GetMethodsWithAttribute(type, McpServerPromptAttribute)
                        .Select(method => ExtractPromptInfo(type, method))
                        .Where(promptInfo => promptInfo != null)!);
            }
        }

        Console.WriteLine($"✓ Found {tools.Count} tools, {resources.Count} resources, {prompts.Count} prompts.");

        return new McpDocumentationRoot
        {
            Metadata = new McpDocumentationMetadata
            {
                AssemblyVersion = version,
                GeneratedDateUtc = date,
                ToolCount = tools.Count,
                ResourceCount = resources.Count,
                PromptCount = prompts.Count
            },
            Tools = tools,
            Resources = resources,
            Prompts = prompts
        };
    }

    /// <summary>
    /// Extracts tool information from a method.
    /// </summary>
    private McpToolInfo? ExtractToolInfo(Type type, MethodInfo method)
    {
        try
        {
            var xmlSummary = _docXmlReader.GetMemberSummary(method);
            var description = GetDescriptionAttribute(method);

            var parameters = ExtractMethodParameters(method);

            return new McpToolInfo
            {
                Name = method.Name,
                Description = !string.IsNullOrWhiteSpace(description) ? description : null,
                Summary = !string.IsNullOrWhiteSpace(xmlSummary) ? xmlSummary : null,
                ClassName = type.Name,
                ReturnType = GetFriendlyTypeName(method.ReturnType),
                Parameters = parameters.Count > 0 ? parameters : null
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARNING] Error extracting tool {type.Name}.{method.Name}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Extracts resource information from a method.
    /// </summary>
    private McpResourceInfo? ExtractResourceInfo(Type type, MethodInfo method)
    {
        try
        {
            var xmlSummary = _docXmlReader.GetMemberSummary(method);
            var description = GetDescriptionAttribute(method);
            var resourceAttr = GetResourceAttributeProperties(method);

            if (string.IsNullOrWhiteSpace(resourceAttr.UriTemplate))
            {
                Console.WriteLine($"[WARNING] Resource {type.Name}.{method.Name} has a null or empty UriTemplate and will be skipped.");
                return null;
            }

            var parameters = ExtractMethodParameters(method);
            return new McpResourceInfo
            {
                Name = resourceAttr.Name ?? method.Name,
                UriTemplate = resourceAttr.UriTemplate,
                Title = resourceAttr.Title,
                MimeType = resourceAttr.MimeType,
                Description = !string.IsNullOrWhiteSpace(description) ? description : null,
                Summary = !string.IsNullOrWhiteSpace(xmlSummary) ? xmlSummary : null,
                ClassName = type.Name,
                MethodName = method.Name,
                Parameters = parameters.Count > 0 ? parameters : null
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARNING] Error extracting resource {type.Name}.{method.Name}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Extracts prompt information from a method.
    /// </summary>
    private McpPromptInfo? ExtractPromptInfo(Type type, MethodInfo method)
    {
        try
        {
            var xmlSummary = _docXmlReader.GetMemberSummary(method);
            var description = GetDescriptionAttribute(method);

            var parameters = ExtractMethodParameters(method);

            return new McpPromptInfo
            {
                Name = method.Name,
                Description = !string.IsNullOrWhiteSpace(description) ? description : null,
                Summary = !string.IsNullOrWhiteSpace(xmlSummary) ? xmlSummary : null,
                ClassName = type.Name,
                Parameters = parameters.Count > 0 ? parameters : null
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WARNING] Error extracting prompt {type.Name}.{method.Name}: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Extracts parameter information from a method.
    /// </summary>
    private static List<McpParameterInfo> ExtractMethodParameters(MethodInfo method)
    {
        var parameters = new List<McpParameterInfo>();
        var methodParams = method.GetParameters();

        foreach (var param in methodParams)
        {
            var description = GetDescriptionAttribute(param);
            var isRequired = !param.HasDefaultValue && !IsNullableType(param.ParameterType);

            parameters.Add(new McpParameterInfo
            {
                Name = param.Name ?? string.Empty,
                Type = GetFriendlyTypeName(param.ParameterType),
                Description = !string.IsNullOrWhiteSpace(description) ? description : null,
                IsRequired = isRequired,
                DefaultValue = param.HasDefaultValue ? param.DefaultValue?.ToString() : null
            });
        }

        return parameters;
    }

    /// <summary>
    /// Checks if a type has a specific attribute by name.
    /// </summary>
    private static bool HasAttribute(Type type, string attributeName)
    {
        return type.GetCustomAttributes(true)
            .Any(a => a.GetType().Name == attributeName);
    }

    /// <summary>
    /// Gets methods with a specific attribute by name.
    /// </summary>
    private static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type type, string attributeName)
    {
        return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.GetCustomAttributes(true).Any(a => a.GetType().Name == attributeName));
    }

    /// <summary>
    /// Gets the Description attribute value from a method.
    /// </summary>
    private static string? GetDescriptionAttribute(MethodInfo method)
    {
        var attr = method.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description;
    }

    /// <summary>
    /// Gets the Description attribute value from a parameter.
    /// </summary>
    private static string? GetDescriptionAttribute(ParameterInfo param)
    {
        var attr = param.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description;
    }

    /// <summary>
    /// Gets properties from McpServerResource attribute.
    /// </summary>
    private static (string? Name, string? UriTemplate, string? Title, string? MimeType) GetResourceAttributeProperties(MethodInfo method)
    {
        var attr = method.GetCustomAttributes(true)
            .FirstOrDefault(a => a.GetType().Name == McpServerResourceAttribute);

        if (attr == null)
        {
            return (null, null, null, null);
        }

        var attrType = attr.GetType();
        var name = attrType.GetProperty("Name")?.GetValue(attr) as string;
        var uriTemplate = attrType.GetProperty("UriTemplate")?.GetValue(attr) as string;
        var title = attrType.GetProperty("Title")?.GetValue(attr) as string;
        var mimeType = attrType.GetProperty("MimeType")?.GetValue(attr) as string;

        return (name, uriTemplate, title, mimeType);
    }

    /// <summary>
    /// Gets a friendly type name for display.
    /// </summary>
    private static string GetFriendlyTypeName(Type type)
    {
        if (type == typeof(void))
        {
            return "void";
        }

        if (type == typeof(string))
        {
            return "string";
        }

        if (type == typeof(int))
        {
            return "int";
        }

        if (type == typeof(bool))
        {
            return "bool";
        }

        // Handle nullable types
        var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
        if (nullableUnderlyingType != null)
        {
            return GetFriendlyTypeName(nullableUnderlyingType) + "?";
        }

        // Handle generic types
        if (type.IsGenericType)
        {
            var genericTypeName = type.Name.Split('`')[0];
            var genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetFriendlyTypeName));
            return $"{genericTypeName}<{genericArgs}>";
        }

        return type.Name;
    }

    /// <summary>
    /// Checks if a type is nullable.
    /// </summary>
    private static bool IsNullableType(Type type)
    {
        return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
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

            version = plusIndex >= 0 && plusIndex + 9 < versionString.Length
                ? versionString[..(plusIndex + 9)]
                : versionString;
        }

        var date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        return (version, date);
    }
}
