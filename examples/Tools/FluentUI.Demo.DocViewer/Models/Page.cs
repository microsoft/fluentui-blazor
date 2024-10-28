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
    ///   category: 20|Components
    ///   title: Button
    ///   route: /Button
    ///   hidden: true
    ///   ---
    ///   My content
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="resourceName"></param>
    /// <param name="fileContent"></param>
    internal Page(DocViewerService service, string resourceName, string fileContent)
    {
        _docViewerService = service;
        var items = ExtractHeaderContent(fileContent);

        ResourceName = resourceName;
        Content = ReplaceIncludes(GetItem(items, "content"));
        Headers = items.Where(i => i.Key != "content").ToDictionary();
        Title = GetItem(items, "title");
        Order = GetItem(items, "order");
        Route = GetItem(items, "route");
        Hidden = GetItem(items, "hidden") == "true";

        var category = GetItem(items, "category");
        if (!string.IsNullOrEmpty(category))
        {
            var parts = category.Split(['|'], 2);
            if (parts.Length == 2)
            {
                Category = (parts[0].Trim(), parts[1].Trim());
            }
            else
            {
                Category = (string.Empty, category.Trim());
            }
        }
    }

    /// <summary>
    /// Gets the header items.
    /// </summary>
    public IDictionary<string, string> Headers { get; }

    /// <summary>
    /// Gets the resource name (the file name).
    /// Example: `FluentUI.Demo.Client.Documentation.Components.Button.FluentButton.md`
    /// </summary>
    public string ResourceName { get; }

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
    /// Gets the page order defined in the <see cref="Headers"/>
    /// </summary>
    public string Order { get; } = string.Empty;

    /// <summary>
    /// Gets the page category defined in the <see cref="Headers"/>
    /// </summary>
    public (string Key, string Title) Category { get; } = (string.Empty, string.Empty);

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

    /// <summary />
    private string ReplaceIncludes(string content, int numberOfReplaces = 1)
    {
        for (var i = 0; i < numberOfReplaces; i++)
        {
            // Regular expression to match both "{{ INCLUDE File=... }}" and "{{ INCLUDE ... }}"
            var pattern = @"\{\{ INCLUDE (?:File=)?(.+?) \}\}";
            //var pattern = @"\{\{ INCLUDE File=([^\s\}]+) \}\}";
            content = Regex.Replace(content, pattern, match => GetFileContent(match.Groups[1].Value));
        }

        return content;
    }

    /// <summary />
    private string GetFileContent(string resourceName)
    {
        if (_docViewerService.ResourcesAssembly == null)
        {
            return string.Empty;
        }

        if (!resourceName.Contains('.'))
        {
            resourceName += ".md";
        }

        if (resourceName.EndsWith(".md"))
        {
            return _docViewerService.LoadResource(resourceName, isFullName: false, removeHeaders: true);
        }

        return $"FILE NOT FOUND: ${resourceName}";
    }
}
