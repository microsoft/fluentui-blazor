// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocViewer.Models;

/// <summary>
/// Types of sections in a markdown document.
/// </summary>
public enum SectionType
{
    /// <summary>
    /// HTML
    /// </summary>
    Html,

    /// <summary>
    /// Source code
    /// </summary>
    Code,

    /// <summary>
    /// Component to display
    /// </summary>
    Component,

    /// <summary>
    /// API array to display
    /// </summary>
    Api,

    /// <summary>
    /// MCP documentation (tools, resources, prompts) - detailed view
    /// </summary>
    Mcp,

    /// <summary>
    /// MCP documentation summary - lightweight view for overview pages
    /// </summary>
    McpSummary,
}

