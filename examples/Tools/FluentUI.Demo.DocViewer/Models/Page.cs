// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;
using FluentUI.Demo.DocViewer.Services;
using Markdig;

namespace FluentUI.Demo.DocViewer.Models;

/// <summary>
/// Represents a page of a markdown document.
/// </summary>
public record Page
{
    private readonly DocViewerService _docViewerService;
    private IEnumerable<PageHtmlHeader>? _pageHtmlHeaders;

    /// <summary>
    /// 
    ///   ---
    ///   title: Button
    ///   route: /Button
    ///   hidden: true
    ///   ---
    ///   My content
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="content"></param>
    internal Page(DocViewerService service, string content)
    {
        _docViewerService = service;
        var items = ExtractHeaderContent(content);

        Headers = items.Where(i => i.Key != "content").ToDictionary();
        Content = GetItem(items, "content");
        Title = GetItem(items, "title");
        Route = GetItem(items, "route");
        Hidden = GetItem(items, "hidden") == "true";
    }

    /// <summary>
    /// Gets the header items.
    /// </summary>
    public IDictionary<string, string> Headers { get; }

    /// <summary>
    /// Gets the page content
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// Gets the route defined in the <see cref="Headers"/>
    /// </summary>
    public string Route { get; } = string.Empty;

    /// <summary>
    /// Gets the page title defined in the <see cref="Headers"/>
    /// </summary>
    public string Title { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the visibility of the page, defined in the <see cref="Headers"/>.
    /// When the page is Hidden, it will not be displayed in the navigation, but it can be accessed directly.
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// Returns the list of HTML headers included in the content.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<PageHtmlHeader> GetHtmlHeaders() => _pageHtmlHeaders ??= PageHtmlHeader.ExtractHeaders(this, Markdown.ToHtml(Content, DocViewerService.MarkdownPipeline));

    /// <summary />
    internal async Task<List<Section>> ExtractSectionsAsync()
    {
        var html = Markdown.ToHtml(Content, DocViewerService.MarkdownPipeline);

        string[] tags =
        [
            @"({{(.*?)}})",                         // {{ MyComponent }}, {{ API => MyComponent }}
            @"(<pre><code.*?>.*?</code></pre>)"     // <pre><code>...</code></pre>
        ];

        var sections = new List<Section>();

        var regex = new Regex(string.Join('|', tags), RegexOptions.Singleline);
        var matches = regex.Matches(html);

        var lastIndex = 0;
        foreach (Match match in matches)
        {
            if (match.Index > lastIndex)
            {
                // String before the Tag
                await AddSectionAsync(html[lastIndex..match.Index]);
            }

            // Tag page
            await AddSectionAsync(match.Value);

            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < html.Length)
        {
            await AddSectionAsync(html[lastIndex..]);
        }

        // TOC: HTML Headers
        _pageHtmlHeaders = PageHtmlHeader.ExtractHeaders(this, html);

        return sections;

        // Add a section to the list
        async Task AddSectionAsync(string content)
        {
            var section = new Section(_docViewerService);
            await section.ReadAsync(content);
            sections.Add(section);
        }
    }

    /// <summary>
    /// Returns the key value or a string.empty (if not found)
    /// </summary>
    /// <param name="items"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static string GetItem(Dictionary<string, string> items, string key)
    {
        return items.TryGetValue(key, out var value) ? value : string.Empty;
    }

    /// <summary>
    /// Extract the Markdown header content
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static Dictionary<string, string> ExtractHeaderContent(string input)
    {
        var dictionary = new Dictionary<string, string>();
        var regex = new Regex(@"---\s*(.*?)\s*---", RegexOptions.Singleline);
        var match = regex.Match(input);

        if (match.Success)
        {
            var lines = match.Groups[1].Value.Split('\n');
            foreach (var line in lines)
            {
                var parts = line.Split([':'], 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Split('#')[0].Trim(); // Remove comments
                    dictionary[key] = value;
                }
            }

            var content = input[(match.Index + match.Length)..].Trim();
            dictionary["content"] = content;

        }
        else
        {
            dictionary["content"] = input;
        }

        return dictionary;
    }
}
