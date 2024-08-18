// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text;
using FluentUI.Demo.DocViewer.Models;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoAsidePanel
{
    /// <summary />
    [Inject]
    public NavigationManager Navigation { get; set; } = default!;

    /// <summary />
    [Inject]
    public Demo.DocViewer.Services.DocViewerService DocViewerService { get; set; } = default!;

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
    /// Convert the list of PageHtmlHeader to a hierarchical HTML list.
    /// </summary>
    /// <param name="headers"></param>
    /// <param name="firstLevel"></param>
    /// <returns></returns>
    private static MarkupString ConvertToMarkupString(IEnumerable<PageHtmlHeader> headers, int firstLevel)
    {
        var htmlList = new StringBuilder();
        var items = headers.Where(i => i.Level >= firstLevel).ToList();

        InternalConvertToHtml(items, htmlList, firstLevel, 0);

        return (MarkupString)htmlList.ToString();

        // Recursive method: Handles the nesting of <ul> and <li> elements based on the level of each item.
        static void InternalConvertToHtml(List<PageHtmlHeader> items, StringBuilder htmlList, int currentLevel, int index)
        {
            htmlList.AppendLine("<ul>");
            while (index < items.Count && items[index].Level == currentLevel)
            {
                var isApi = items[index].Id.StartsWith("api-fluent", StringComparison.Ordinal);
                var isFirstApi = isApi && !items.Take(index).Any(i => i.Id.StartsWith("api-fluent"));
                var content = $"<a href=\"{items[index].AnchorId}\">{items[index].Title}</a>";

                htmlList.AppendLine(
                    CultureInfo.InvariantCulture,
                    $"<li {(isFirstApi ? "first" : "")} {(isApi ? "api" : string.Empty)}>{content}");

                var nextIndex = index + 1;
                if (nextIndex < items.Count && items[nextIndex].Level > currentLevel)
                {
                    InternalConvertToHtml(items, htmlList, currentLevel + 1, nextIndex);
                    while (nextIndex < items.Count && items[nextIndex].Level > currentLevel)
                    {
                        nextIndex++;
                    }
                }

                htmlList.AppendLine("</li>");
                index = nextIndex;
            }

            htmlList.AppendLine("</ul>");
        }
    }
}
