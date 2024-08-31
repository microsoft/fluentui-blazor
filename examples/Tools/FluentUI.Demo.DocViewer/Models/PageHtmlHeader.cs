// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;

namespace FluentUI.Demo.DocViewer.Models;

/// <summary>
/// Represents a page header of a markdown document.
/// </summary>
public record PageHtmlHeader
{
    /// <summary>
    /// Gets a reference to the associated <see cref="Page"/>.
    /// </summary>
    public required Page Page { get; init; }

    /// <summary>
    /// Gets the header level.
    /// </summary>
    public required int Level { get; init; }

    /// <summary>
    /// Gets the header id.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the link anchor id: Route#Id.
    /// </summary>
    public string AnchorId => $"{Page.Route}#{Id}";

    /// <summary>
    /// Gets the header title.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Extract the headers from the HTML content
    /// </summary>
    /// <param name="page"></param>
    /// <param name="html"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "SYSLIB1045:Convert to 'GeneratedRegexAttribute'.", Justification = "To optimize later")]
    internal static List<PageHtmlHeader> ExtractHeaders(Page page, string html)
    {
        var headers = new List<PageHtmlHeader>();
        var regex = new Regex(@"<h(?<level>[123456]) id=""(?<id>[^""]+)"">(?<title>[^<]+)</h[123456]>",
                                RegexOptions.IgnoreCase);

        var matches = regex.Matches(html);

        foreach (Match match in matches)
        {
            headers.Add(new PageHtmlHeader
            {
                Page = page,
                Level = int.Parse(match.Groups["level"].Value, CultureInfo.InvariantCulture),
                Id = match.Groups["id"].Value,
                Title = match.Groups["title"].Value,
            });
        }

        return headers;
    }
}
