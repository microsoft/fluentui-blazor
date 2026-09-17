// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models.Mcp;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components.Markdown.Summaries;

/// <summary>
/// Represents a summary of MCP resources for display purposes.
/// </summary>
public partial class McpResourcesSummary
{
    /// <summary>
    /// Gets or sets the list of MCP resources to display.
    /// </summary>
    [Parameter]
    public required List<McpResource> Resources { get; set; }
}
