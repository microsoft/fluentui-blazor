// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text.Json;
using Microsoft.FluentUI.AspNetCore.McpServer.Models.McpDocumentation;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Services;

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

        // Try matching generic types (e.g., FluentDataGrid -> FluentDataGrid`1)
        for (var i = 1; i <= 3; i++)
        {
            if (_componentsByName.TryGetValue($"{name}`{i}", out component))
            {
                return component;
            }

            // Also try with Fluent prefix for generic types
            if (_componentsByName.TryGetValue($"Fluent{name}`{i}", out component))
            {
                return component;
            }
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
            Console.Error.WriteLine($"{McpServerConstants.LogPrefix} Loading documentation from: {jsonDocumentationPath}");
            jsonContent = File.ReadAllText(jsonDocumentationPath);
        }
        else
        {
            // Try to load from embedded resource
            jsonContent = LoadFromEmbeddedResource();
        }

        if (string.IsNullOrEmpty(jsonContent))
        {
            Console.Error.WriteLine($"{McpServerConstants.LogPrefix} Warning: JSON documentation not found. Documentation will be limited.");
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
                Console.Error.WriteLine($"{McpServerConstants.LogPrefix} Loaded {documentation.Components.Count} components and {documentation.Enums.Count} enums from documentation.");
            }

            return documentation;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"{McpServerConstants.LogPrefix} Error parsing JSON documentation: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Loads documentation from the embedded resource.
    /// </summary>
    private static string? LoadFromEmbeddedResource()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Microsoft.FluentUI.AspNetCore.McpServer.FluentUIComponentsDocumentation.json";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            // List available resources for debugging
            var availableResources = assembly.GetManifestResourceNames();
            Console.Error.WriteLine($"{McpServerConstants.LogPrefix} Available embedded resources: {string.Join(", ", availableResources)}");
            return null;
        }

        using var reader = new StreamReader(stream);
        Console.Error.WriteLine($"{McpServerConstants.LogPrefix} Loading documentation from embedded resource.");
        return reader.ReadToEnd();
    }
}
