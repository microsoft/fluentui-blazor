// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

/// <summary>
/// Root model for the MCP documentation JSON file.
/// </summary>
internal class McpDocumentationRoot
{
    /// <summary>
    /// Gets or sets metadata about the generated documentation.
    /// </summary>
    [JsonPropertyName("metadata")]
    public McpDocumentationMetadata Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of all components.
    /// </summary>
    [JsonPropertyName("components")]
    public List<JsonComponentInfo> Components { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of all enums.
    /// </summary>
    [JsonPropertyName("enums")]
    public List<JsonEnumInfo> Enums { get; set; } = [];
}

/// <summary>
/// Metadata about the generated documentation.
/// </summary>
internal class McpDocumentationMetadata
{
    /// <summary>
    /// Gets or sets the assembly version.
    /// </summary>
    [JsonPropertyName("assemblyVersion")]
    public string AssemblyVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the generation date in UTC.
    /// </summary>
    [JsonPropertyName("generatedDateUtc")]
    public string GeneratedDateUtc { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total component count.
    /// </summary>
    [JsonPropertyName("componentCount")]
    public int ComponentCount { get; set; }

    /// <summary>
    /// Gets or sets the total enum count.
    /// </summary>
    [JsonPropertyName("enumCount")]
    public int EnumCount { get; set; }
}

/// <summary>
/// JSON representation of a component.
/// </summary>
internal class JsonComponentInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("isGeneric")]
    public bool IsGeneric { get; set; }

    [JsonPropertyName("baseClass")]
    public string? BaseClass { get; set; }

    [JsonPropertyName("properties")]
    public List<JsonPropertyInfo> Properties { get; set; } = [];

    [JsonPropertyName("events")]
    public List<JsonEventInfo> Events { get; set; } = [];

    [JsonPropertyName("methods")]
    public List<JsonMethodInfo> Methods { get; set; } = [];
}

/// <summary>
/// JSON representation of a property.
/// </summary>
internal class JsonPropertyInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("isParameter")]
    public bool IsParameter { get; set; }

    [JsonPropertyName("isInherited")]
    public bool IsInherited { get; set; }

    [JsonPropertyName("defaultValue")]
    public string? DefaultValue { get; set; }

    [JsonPropertyName("enumValues")]
    public string[] EnumValues { get; set; } = [];
}

/// <summary>
/// JSON representation of an event.
/// </summary>
internal class JsonEventInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("isInherited")]
    public bool IsInherited { get; set; }
}

/// <summary>
/// JSON representation of a method.
/// </summary>
internal class JsonMethodInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("returnType")]
    public string ReturnType { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("parameters")]
    public string[] Parameters { get; set; } = [];

    [JsonPropertyName("isInherited")]
    public bool IsInherited { get; set; }
}

/// <summary>
/// JSON representation of an enum.
/// </summary>
internal class JsonEnumInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("values")]
    public List<JsonEnumValueInfo> Values { get; set; } = [];
}

/// <summary>
/// JSON representation of an enum value.
/// </summary>
internal class JsonEnumValueInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Service for reading pre-generated JSON documentation.
/// This replaces XmlDocumentationReader and eliminates the need for LoxSmoke.DocXml at runtime.
/// </summary>
internal sealed class JsonDocumentationReader
{
    private readonly McpDocumentationRoot? _documentation;
    private readonly Dictionary<string, JsonComponentInfo> _componentsByName = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, JsonEnumInfo> _enumsByName = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDocumentationReader"/> class.
    /// </summary>
    /// <param name="jsonDocumentationPath">Path to the JSON documentation file, or null to use embedded resource.</param>
    public JsonDocumentationReader(string? jsonDocumentationPath = null)
    {
        _documentation = LoadDocumentation(jsonDocumentationPath);

        if (_documentation != null)
        {
            // Build lookup dictionaries
            foreach (var component in _documentation.Components)
            {
                _componentsByName[component.Name] = component;
            }

            foreach (var enumInfo in _documentation.Enums)
            {
                _enumsByName[enumInfo.Name] = enumInfo;
            }
        }
    }

    /// <summary>
    /// Gets whether documentation is available.
    /// </summary>
    public bool IsAvailable => _documentation != null;

    /// <summary>
    /// Gets the documentation metadata.
    /// </summary>
    public McpDocumentationMetadata? Metadata => _documentation?.Metadata;

    /// <summary>
    /// Gets all components from the documentation.
    /// </summary>
    public IReadOnlyList<JsonComponentInfo> GetAllComponents()
    {
        return _documentation?.Components ?? [];
    }

    /// <summary>
    /// Gets a component by name.
    /// </summary>
    public JsonComponentInfo? GetComponent(string name)
    {
        if (_componentsByName.TryGetValue(name, out var component))
        {
            return component;
        }

        // Try with "Fluent" prefix
        if (_componentsByName.TryGetValue($"Fluent{name}", out component))
        {
            return component;
        }

        return null;
    }

    /// <summary>
    /// Gets all enums from the documentation.
    /// </summary>
    public IReadOnlyList<JsonEnumInfo> GetAllEnums()
    {
        return _documentation?.Enums ?? [];
    }

    /// <summary>
    /// Gets an enum by name.
    /// </summary>
    public JsonEnumInfo? GetEnum(string name)
    {
        _enumsByName.TryGetValue(name, out var enumInfo);
        return enumInfo;
    }

    /// <summary>
    /// Loads documentation from file or embedded resource.
    /// </summary>
    private static McpDocumentationRoot? LoadDocumentation(string? jsonDocumentationPath)
    {
        string? jsonContent;

        // Try to load from file first
        if (!string.IsNullOrEmpty(jsonDocumentationPath) && File.Exists(jsonDocumentationPath))
        {
            Console.Error.WriteLine($"[FluentUI.Mcp.Server] Loading documentation from: {jsonDocumentationPath}");
            jsonContent = File.ReadAllText(jsonDocumentationPath);
        }
        else
        {
            // Try to load from embedded resource
            jsonContent = LoadFromEmbeddedResource();
        }

        if (string.IsNullOrEmpty(jsonContent))
        {
            Console.Error.WriteLine("[FluentUI.Mcp.Server] Warning: JSON documentation not found. Documentation will be limited.");
            return null;
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var documentation = JsonSerializer.Deserialize<McpDocumentationRoot>(jsonContent, options);

            if (documentation != null)
            {
                Console.Error.WriteLine($"[FluentUI.Mcp.Server] Loaded {documentation.Components.Count} components and {documentation.Enums.Count} enums from documentation.");
            }

            return documentation;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[FluentUI.Mcp.Server] Error parsing JSON documentation: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Loads documentation from the embedded resource.
    /// </summary>
    private static string? LoadFromEmbeddedResource()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Microsoft.FluentUI.AspNetCore.Components.McpServer.FluentUIComponentsDocumentation.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            // List available resources for debugging
            var availableResources = assembly.GetManifestResourceNames();
            Console.Error.WriteLine($"[FluentUI.Mcp.Server] Available embedded resources: {string.Join(", ", availableResources)}");
            return null;
        }

        using var reader = new StreamReader(stream);
        Console.Error.WriteLine("[FluentUI.Mcp.Server] Loading documentation from embedded resource.");
        return reader.ReadToEnd();
    }
}
