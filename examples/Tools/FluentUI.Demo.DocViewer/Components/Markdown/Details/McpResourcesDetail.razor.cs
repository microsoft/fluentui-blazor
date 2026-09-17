// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models.Mcp;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components.Markdown.Details;

/// <summary>
/// Component to display MCP resources in detailed view.
/// </summary>
public partial class McpResourcesDetail
{
    /// <summary>
    /// Gets or sets the list of MCP resources to display.
    /// </summary>
    [Parameter]
    public required List<McpResource> Resources { get; set; }

    /// <summary>
    /// Gets or sets the filter applied to the resources (for display in empty message).
    /// </summary>
    [Parameter]
    public string? Filter { get; set; }
}
