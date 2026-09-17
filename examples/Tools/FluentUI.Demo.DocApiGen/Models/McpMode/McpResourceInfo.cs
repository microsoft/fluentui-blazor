// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.McpMode;

/// <summary>
/// Represents information about an MCP resource.
/// </summary>
public class McpResourceInfo
{
    /// <summary>
    /// Gets or sets the name of the resource (from McpServerResource.Name).
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the URI template (from McpServerResource.UriTemplate).
    /// </summary>
    [JsonPropertyName("uriTemplate")]
    public required string UriTemplate { get; set; }

    /// <summary>
    /// Gets or sets the title (from McpServerResource.Title).
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the MIME type (from McpServerResource.MimeType).
    /// </summary>
    [JsonPropertyName("mimeType")]
    public string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets the description from the [Description] attribute.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the XML summary documentation.
    /// </summary>
    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the class that contains this resource.
    /// </summary>
    [JsonPropertyName("className")]
    public required string ClassName { get; set; }

    /// <summary>
    /// Gets or sets the method name.
    /// </summary>
    [JsonPropertyName("methodName")]
    public required string MethodName { get; set; }

    /// <summary>
    /// Gets or sets the parameters extracted from the URI template.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<McpParameterInfo>? Parameters { get; set; }
}
