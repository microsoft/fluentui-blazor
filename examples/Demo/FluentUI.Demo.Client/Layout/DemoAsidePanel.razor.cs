// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Extensions;
using FluentUI.Demo.DocViewer.Models;
using FluentUI.Demo.DocViewer.Services;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoAsidePanel
{
    /// <summary />
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    /// <summary />
    [Inject]
    public DocViewerService DocViewerService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the page route (e.g. "Button", "").
    /// </summary>
    [Parameter]
    public string Page { get; set; } = string.Empty;

    /// <summary>
    /// Gets the list of PageHtmlHeader for the current page.
    /// </summary>
    private IEnumerable<PageHtmlHeader>? Headers => DocViewerService.FromRoute(Page)?.GetHtmlHeaders();

    /// <summary>
    /// Gets the HTML headers for the current page.
    /// </summary>
    private MarkupString? GetHtmlHeaders() => Headers?.ToMarkupString(
        firstLevel: 1,  // Remove `H1` header
        itemAttribute: (header) => header.Id.StartsWith("api-fluent") ? "api" : string.Empty); // Add `<li api>` attribute for `## API Fluent ...` headers
}
