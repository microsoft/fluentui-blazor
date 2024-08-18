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
    /// 
    /// </summary>
    public required int Level { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Extract the headers from the HTML content
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    internal static List<PageHtmlHeader> ExtractHeaders(string html)
    {
        var headers = new List<PageHtmlHeader>();
        var regex = new Regex(@"<h(?<level>[12]) id=""(?<id>[^""]+)"">(?<title>[^<]+)</h[12]>",
                                RegexOptions.IgnoreCase);

        var matches = regex.Matches(html);

        foreach (Match match in matches)
        {
            headers.Add(new PageHtmlHeader
            {
                Level = int.Parse(match.Groups["level"].Value, CultureInfo.InvariantCulture),
                Id = match.Groups["id"].Value,
                Title = match.Groups["title"].Value,
            });
        }

        return headers;
    }
}
