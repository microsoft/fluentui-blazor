// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models.Mcp;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components.Markdown.Details;

/// <summary>
/// Component to display MCP prompts in detailed view.
/// </summary>
public partial class McpPromptsDetail
{
    /// <summary>
    /// Gets or sets the list of MCP prompts to display.
    /// </summary>
    [Parameter]
    public required List<McpPrompt> Prompts { get; set; }

    /// <summary>
    /// Gets or sets the filter applied to the prompts (for display in empty message).
    /// </summary>
    [Parameter]
    public string? Filter { get; set; }
}
