// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models.Mcp;
using FluentUI.Demo.DocViewer.Services;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components.Markdown;

/// <summary>
/// Component to display MCP (Model Context Protocol) documentation in summary view.
/// </summary>
public partial class McpSummaryViewer
{
    /// <summary>
    /// Gets or sets the MCP type to display (tools, resources, or prompts).
    /// </summary>
    [Parameter]
    public McpType McpType { get; set; } = McpType.Tools;

    /// <summary>
    /// Gets or sets the filter to apply to the MCP items (filter by class name or item name).
    /// </summary>
    [Parameter]
    public string? McpFilter { get; set; }

    /// <summary>
    /// Gets the tools to display.
    /// </summary>
    protected List<McpTool> Tools { get; private set; } = [];

    /// <summary>
    /// Gets the resources to display.
    /// </summary>
    protected List<McpResource> Resources { get; private set; } = [];

    /// <summary>
    /// Gets the prompts to display.
    /// </summary>
    protected List<McpPrompt> Prompts { get; private set; } = [];

    /// <summary>
    /// Gets a value indicating whether the MCP documentation is loaded.
    /// </summary>
    protected bool IsDocumentationLoaded => McpDocumentationService.Cached != null;

    /// <summary>
    /// Gets a value indicating whether the MCP type is valid.
    /// </summary>
    protected bool IsValidMcpType => Enum.IsDefined(McpType);

    /// <summary />
    protected override void OnParametersSet()
    {
        if (!IsDocumentationLoaded)
        {
            return;
        }

        switch (McpType)
        {
            case McpType.Tools:
                Tools = [.. McpDocumentationService.GetTools(McpFilter)];
                break;

            case McpType.Resources:
                Resources = [.. McpDocumentationService.GetResources(McpFilter)];
                break;

            case McpType.Prompts:
                Prompts = [.. McpDocumentationService.GetPrompts(McpFilter)];
                break;
        }
    }
}
