// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoAsidePanel
{
    [Inject]
    public Demo.DocViewer.Services.DocViewerService DocViewerService { get; set; } = default!;

    [Parameter]
    public string Page { get; set; } = string.Empty;

    private IEnumerable<PageHtmlHeader>? Headers => DocViewerService.FromRoute(Page)?.GetHtmlHeaders();
}
