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

public partial class MarkdownViewer
{
    private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    private bool _isPageNotFound;
    private const string JAVASCRIPT_FILE = "./_content/FluentUI.Demo.DocViewer/Components/MarkdownViewer.razor.js";
    private IJSObjectReference _jsModule = default!;

    /// <summary />
    [Inject]
    internal StaticAssetService StaticAssetService { get; set; } = default!;

    /// <summary />
    [Inject]
    internal DocViewerService DocViewerService { get; set; } = default!;

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
    protected async override Task OnInitializedAsync()
    {
        // Markdown
        var page = DocViewerService.FromRoute(Route);

        if (page is null)
        {
            PageTitle = "Page not found";
            _isPageNotFound = true;
            return;
        }

        // Extract the sections from the markdown content
        _isPageNotFound = false;
        PageTitle = page.Title;
        var html = Markdown.ToHtml(page.Content, MarkdownPipeline);
        Sections = ExtractSections(html);

        // Load Assets (Source Code, ...)
        foreach (var section in Sections)
        {
            await section.LoadStaticAssetsAsync(StaticAssetService, DocViewerService);
        }
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

    /// <summary />
    private static List<Section> ExtractSections(string content)
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
                sections.Add(new Section(content[lastIndex..match.Index]));
            }

            // Tag page
            sections.Add(new Section(match.Value));

            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < content.Length)
        {
            sections.Add(new Section(content[lastIndex..]));
        }

        return sections;
    }

    /// <summary />
    private Type? GetComponentFromName(string name)
    {
        if (DocViewerService.ComponentsAssembly is null)
        {
            return null;
        }

        return DocViewerService.ComponentsAssembly
                               .GetTypes()
                               .Where(t => t.IsSubclassOf(typeof(ComponentBase)) && !t.IsAbstract)
                               .FirstOrDefault(i => i.Name == name);
    }
}
