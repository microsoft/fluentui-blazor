// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Mcp.Shared;

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
