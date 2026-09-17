// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.McpServer.Models;

/// <summary>
/// Represents general information about a documentation page.
/// </summary>
public class GeneralInfo
{
    /// <summary>
    /// Gets or sets the title of the documentation page.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the order of the documentation page.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets the category of the documentation page.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the route of the documentation page.
    /// </summary>
    public string Route { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the icon of the documentation page.
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file name of the documentation page.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the documentation page is hidden.
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// Gets or sets the content of the documentation page (markdown).
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the summary (first paragraph) of the documentation page.
    /// </summary>
    public string Summary { get; set; } = string.Empty;
}
