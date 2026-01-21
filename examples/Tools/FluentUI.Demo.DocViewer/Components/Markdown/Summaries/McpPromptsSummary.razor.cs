// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models.Mcp;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components.Markdown.Summaries;

/// <summary>
/// Represents a summary of MCP prompts to be displayed.
/// </summary>
public partial class McpPromptsSummary
{
    /// <summary>
    /// Gets or sets the list of MCP prompts to display.
    /// </summary>
    [Parameter]
    public required List<McpPrompt> Prompts { get; set; }
}
