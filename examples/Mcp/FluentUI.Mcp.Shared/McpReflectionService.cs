// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Reflection;

namespace FluentUI.Mcp.Shared;

/// <summary>
/// Provides reflection-based discovery of MCP tools, prompts, and resources.
/// </summary>
public static class McpReflectionService
{
    /// <summary>
    /// Gets all MCP tools defined in the assembly.
    /// </summary>
    public static IReadOnlyList<McpToolInfo> GetTools(Assembly assembly)
    {
        var tools = new List<McpToolInfo>();

        // Find all types with [McpServerToolType] attribute
        var toolTypes = assembly.GetTypes()
            .Where(t => t.GetCustomAttributes()
                .Any(a => a.GetType().Name == "McpServerToolTypeAttribute"));

        foreach (var type in toolTypes)
        {
            // Find all methods with [McpServerTool] attribute
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttributes()
                    .Any(a => a.GetType().Name == "McpServerToolAttribute"));

            foreach (var method in methods)
            {
                var toolAttr = method.GetCustomAttributes()
                    .FirstOrDefault(a => a.GetType().Name == "McpServerToolAttribute");

                var name = GetAttributeProperty<string>(toolAttr, "Name") ?? method.Name;
                var description = method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";

                var parameters = method.GetParameters()
                    .Where(p => p.ParameterType != typeof(CancellationToken))
                    .Select(p => new McpParameterInfo(
                        p.Name ?? "",
                        GetFriendlyTypeName(p.ParameterType),
                        p.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "",
                        !p.HasDefaultValue))
                    .ToList();

                tools.Add(new McpToolInfo(name, description, type.Name, parameters));
            }
        }

        return tools.OrderBy(t => t.Name).ToList();
    }

    /// <summary>
    /// Gets all MCP prompts defined in the assembly.
    /// </summary>
    public static IReadOnlyList<McpPromptInfo> GetPrompts(Assembly assembly)
    {
        var prompts = new List<McpPromptInfo>();

        // Find all types with [McpServerPromptType] attribute
        var promptTypes = assembly.GetTypes()
            .Where(t => t.GetCustomAttributes()
                .Any(a => a.GetType().Name == "McpServerPromptTypeAttribute"));

        foreach (var type in promptTypes)
        {
            // Find all methods with [McpServerPrompt] attribute
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttributes()
                    .Any(a => a.GetType().Name == "McpServerPromptAttribute"));

            foreach (var method in methods)
            {
                var promptAttr = method.GetCustomAttributes()
                    .FirstOrDefault(a => a.GetType().Name == "McpServerPromptAttribute");

                var name = GetAttributeProperty<string>(promptAttr, "Name") ?? method.Name;
                var description = method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";

                var parameters = method.GetParameters()
                    .Select(p => new McpParameterInfo(
                        p.Name ?? "",
                        GetFriendlyTypeName(p.ParameterType),
                        p.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "",
                        !p.HasDefaultValue))
                    .ToList();

                prompts.Add(new McpPromptInfo(name, description, type.Name, parameters));
            }
        }

        return prompts.OrderBy(p => p.Name).ToList();
    }

    /// <summary>
    /// Gets all MCP resources defined in the assembly.
    /// </summary>
    public static IReadOnlyList<McpResourceInfo> GetResources(Assembly assembly)
    {
        var resources = new List<McpResourceInfo>();

        // Find all types with [McpServerResourceType] attribute
        var resourceTypes = assembly.GetTypes()
            .Where(t => t.GetCustomAttributes()
                .Any(a => a.GetType().Name == "McpServerResourceTypeAttribute"));

        foreach (var type in resourceTypes)
        {
            // Find all methods with [McpServerResource] attribute
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttributes()
                    .Any(a => a.GetType().Name == "McpServerResourceAttribute"));

            foreach (var method in methods)
            {
                var resourceAttr = method.GetCustomAttributes()
                    .FirstOrDefault(a => a.GetType().Name == "McpServerResourceAttribute");

                var uriTemplate = GetAttributeProperty<string>(resourceAttr, "UriTemplate") ?? "";
                var name = GetAttributeProperty<string>(resourceAttr, "Name") ?? method.Name;
                var title = GetAttributeProperty<string>(resourceAttr, "Title") ?? name;
                var mimeType = GetAttributeProperty<string>(resourceAttr, "MimeType") ?? "text/plain";
                var description = method.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";

                var isTemplate = uriTemplate.Contains('{') && uriTemplate.Contains('}');

                resources.Add(new McpResourceInfo(uriTemplate, name, title, description, mimeType, isTemplate, type.Name));
            }
        }

        return resources.OrderBy(r => r.Uri).ToList();
    }

    /// <summary>
    /// Gets a summary of all MCP primitives in the assembly.
    /// </summary>
    public static McpSummary GetSummary(Assembly assembly)
    {
        return new McpSummary(
            GetTools(assembly),
            GetPrompts(assembly),
            GetResources(assembly));
    }

    private static T? GetAttributeProperty<T>(object? attribute, string propertyName)
    {
        if (attribute == null)
        {
            return default;
        }

        var property = attribute.GetType().GetProperty(propertyName);
        return property != null ? (T?)property.GetValue(attribute) : default;
    }

    private static string GetFriendlyTypeName(Type type)
    {
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

        if (type == typeof(double))
        {
            return "double";
        }

        if (type == typeof(float))
        {
            return "float";
        }

        if (type == typeof(long))
        {
            return "long";
        }

        if (Nullable.GetUnderlyingType(type) is Type underlyingType)
        {
            return $"{GetFriendlyTypeName(underlyingType)}?";
        }

        if (type.IsGenericType)
        {
            var genericName = type.Name.Split('`')[0];
            var genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetFriendlyTypeName));
            return $"{genericName}<{genericArgs}>";
        }

        return type.Name;
    }
}

/// <summary>
/// Information about an MCP tool.
/// </summary>
/// <param name="Name">The tool name.</param>
/// <param name="Description">The tool description.</param>
/// <param name="ClassName">The class containing the tool.</param>
/// <param name="Parameters">The tool parameters.</param>
public record McpToolInfo(
    string Name,
    string Description,
    string ClassName,
    IReadOnlyList<McpParameterInfo> Parameters);

/// <summary>
/// Information about an MCP prompt.
/// </summary>
/// <param name="Name">The prompt name.</param>
/// <param name="Description">The prompt description.</param>
/// <param name="ClassName">The class containing the prompt.</param>
/// <param name="Parameters">The prompt parameters.</param>
public record McpPromptInfo(
    string Name,
    string Description,
    string ClassName,
    IReadOnlyList<McpParameterInfo> Parameters);

/// <summary>
/// Information about an MCP resource.
/// </summary>
/// <param name="Uri">The resource URI or URI template.</param>
/// <param name="Name">The resource name.</param>
/// <param name="Title">The resource title.</param>
/// <param name="Description">The resource description.</param>
/// <param name="MimeType">The MIME type of the resource.</param>
/// <param name="IsTemplate">Whether this is a URI template.</param>
/// <param name="ClassName">The class containing the resource.</param>
public record McpResourceInfo(
    string Uri,
    string Name,
    string Title,
    string Description,
    string MimeType,
    bool IsTemplate,
    string ClassName);

/// <summary>
/// Information about an MCP parameter.
/// </summary>
/// <param name="Name">The parameter name.</param>
/// <param name="Type">The parameter type.</param>
/// <param name="Description">The parameter description.</param>
/// <param name="Required">Whether the parameter is required.</param>
public record McpParameterInfo(
    string Name,
    string Type,
    string Description,
    bool Required);

/// <summary>
/// Summary of all MCP primitives.
/// </summary>
/// <param name="Tools">All tools.</param>
/// <param name="Prompts">All prompts.</param>
/// <param name="Resources">All resources.</param>
public record McpSummary(
    IReadOnlyList<McpToolInfo> Tools,
    IReadOnlyList<McpPromptInfo> Prompts,
    IReadOnlyList<McpResourceInfo> Resources);
