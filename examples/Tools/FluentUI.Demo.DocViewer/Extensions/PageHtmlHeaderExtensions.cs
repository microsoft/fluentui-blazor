// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text;
using FluentUI.Demo.DocViewer.Models;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.DocViewer.Extensions;

/// <summary>
/// Reflection extension methods with supporting properties.
/// </summary>
public static class PageHtmlHeaderExtensions
{
    /// <summary>
    /// Convert the list of PageHtmlHeader to a hierarchical HTML list.
    /// </summary>
    /// <param name="headers"></param>
    /// <param name="firstLevel"></param>
    /// <param name="itemAttribute"></param>
    /// <returns></returns>
    public static MarkupString ToMarkupString(this IEnumerable<PageHtmlHeader> headers, int firstLevel, Func<PageHtmlHeader, string> itemAttribute)
    {
        var htmlList = new StringBuilder();
        var items = headers.Where(i => i.Level >= firstLevel + 1).ToList();

        InternalConvertToHtml(items, htmlList, firstLevel + 1, 0, itemAttribute);

        return (MarkupString)htmlList.ToString();
    }

    /// <summary>
    /// Recursive method: Handles the nesting of `ul` and `li` elements based on the level of each item.
    /// </summary>
    /// <param name="items"></param>
    /// <param name="htmlList"></param>
    /// <param name="currentLevel"></param>
    /// <param name="index"></param>
    /// <param name="itemAttribute"></param>
    private static void InternalConvertToHtml(List<PageHtmlHeader> items, StringBuilder htmlList, int currentLevel, int index, Func<PageHtmlHeader, string> itemAttribute)
    {
        htmlList.AppendLine("<ul>");
        while (index < items.Count && items[index].Level == currentLevel)
        {
            var attribute = itemAttribute(items[index]);
            var content = $"<a href=\"{items[index].AnchorId}\">{items[index].Title}</a>";

            if (!string.IsNullOrEmpty(attribute))
            {
                attribute = $" {attribute}";
            }

            var startTag = $"<li{attribute}>";

            htmlList.AppendLine(startTag);
            htmlList.AppendLine(content);

            var nextIndex = index + 1;
            if (nextIndex < items.Count && items[nextIndex].Level > currentLevel)
            {
                InternalConvertToHtml(items, htmlList, currentLevel + 1, nextIndex, itemAttribute);
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
