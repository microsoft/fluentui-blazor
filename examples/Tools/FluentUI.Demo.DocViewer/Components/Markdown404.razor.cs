// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Components;

/// <summary>
/// Component to display a 404 error message when a markdown file is not found.
/// </summary>
public partial class Markdown404
{
    /// <summary>
    /// Gets or sets the route that was not found.
    /// </summary>
    [Parameter]
    public string Route { get; set; } = string.Empty;
}
