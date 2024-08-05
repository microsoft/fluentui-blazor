// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;
using FluentUI.Demo.DocViewer.Models;
using FluentUI.Demo.DocViewer.Services;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.DocViewer.Components;

/// <summary>
/// Component to display a markdown file.
/// </summary>
public partial class MarkdownViewer
{
    private bool _isPageNotFound;
    private const string JAVASCRIPT_FILE = "./_content/FluentUI.Demo.DocViewer/Components/MarkdownViewer.razor.js";
    private IJSObjectReference _jsModule = default!;

    /// <summary />
    [Inject]
    internal FactoryService Factory { get; set; } = default!;

    /// <summary />
    [Inject]
    internal IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the Page route of the markdown file to display.
    /// </summary>
    [Parameter]
    public required string Route { get; set; }

    /// <summary />
    internal string PageTitle { get; private set; } = string.Empty;

    /// <summary />
    protected IEnumerable<Section> Sections { get; private set; } = [];

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        // Markdown
        var page = Factory.DocViewerService.FromRoute(Route);

        if (page is null)
        {
            PageTitle = "Page not found";
            _isPageNotFound = true;
            return;
        }

        // Extract the sections from the markdown content
        _isPageNotFound = false;
        PageTitle = page.Title;

        var html = Markdown.ToHtml(page.Content, FactoryService.MarkdownPipeline);
        Sections = await ExtractSectionsAsync(html);
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        }

        await _jsModule.InvokeVoidAsync("applyHighlight");
    }

    private ApiClass? GetApiClassFromName(string? name)
    {
        var type = Factory.DocViewerService.ApiAssembly
                                          ?.GetTypes()
                                          ?.FirstOrDefault(i => i.Name == name);

        return type is null ? null : new ApiClass(Factory, type);
    }

    /// <summary />
    private async Task<List<Section>> ExtractSectionsAsync(string content)
    {
        string[] tags =
        [
            @"({{(.*?)}})",                         // {{ MyComponent }}, {{ API => MyComponent }}
            @"(<pre><code.*?>.*?</code></pre>)"     // <pre><code>...</code></pre>
        ];

        var sections = new List<Section>();

        var regex = new Regex(string.Join('|', tags), RegexOptions.Singleline);
        var matches = regex.Matches(content);

        var lastIndex = 0;
        foreach (Match match in matches)
        {
            if (match.Index > lastIndex)
            {
                // String before the Tag
                await AddSectionAsync(content[lastIndex..match.Index]);
            }

            // Tag page
            await AddSectionAsync(match.Value);

            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < content.Length)
        {
            await AddSectionAsync(content[lastIndex..]);
        }

        return sections;

        // Add a section to the list
        async Task AddSectionAsync(string content)
        {
            var section = new Section(Factory);
            await section.ReadAsync(content);
            sections.Add(section);
        }
    }

    /// <summary />
    private Type? GetComponentFromName(string name)
    {
        if (Factory.DocViewerService.ComponentsAssembly is null)
        {
            return null;
        }

        return Factory.DocViewerService.ComponentsAssembly
                                       .GetTypes()
                                       .Where(t => t.IsSubclassOf(typeof(ComponentBase)) && !t.IsAbstract)
                                       .FirstOrDefault(i => i.Name == name);
    }
}
