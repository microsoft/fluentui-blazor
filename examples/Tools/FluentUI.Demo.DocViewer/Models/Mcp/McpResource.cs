// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocViewer.Models.Mcp;

/// <summary>
/// Represents information about an MCP resource.
/// </summary>
public class McpResource
{
    /// <summary>
    /// Gets or sets the name of the resource.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the URI template.
    /// </summary>
    [JsonPropertyName("uriTemplate")]
    public string UriTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the MIME type.
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
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the method name.
    /// </summary>
    [JsonPropertyName("methodName")]
    public string MethodName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parameters extracted from the URI template.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<McpParameter>? Parameters { get; set; }
}
