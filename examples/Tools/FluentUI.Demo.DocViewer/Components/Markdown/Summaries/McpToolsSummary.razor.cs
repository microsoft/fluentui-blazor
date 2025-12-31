// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models.Mcp;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components.Markdown.Summaries;

/// <summary>
/// Represents a summary view model for displaying a collection of MCP tools.
/// </summary>
public partial class McpToolsSummary
{
    /// <summary>
    /// Gets or sets the list of MCP tools to display.
    /// </summary>
    [Parameter]
    public required List<McpTool> Tools { get; set; }
}
