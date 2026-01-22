// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models.Mcp;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components.Markdown.Details;

/// <summary>
/// Component to display MCP tools in detailed view.
/// </summary>
public partial class McpToolsDetail
{
    /// <summary>
    /// Gets or sets the list of MCP tools to display.
    /// </summary>
    [Parameter]
    public required List<McpTool> Tools { get; set; }

    /// <summary>
    /// Gets or sets the filter applied to the tools (for display in empty message).
    /// </summary>
    [Parameter]
    public string? Filter { get; set; }
}
