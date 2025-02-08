// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Models;
using FluentUI.Demo.DocViewer.Services;
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
    internal DocViewerService DocViewerService { get; set; } = default!;

    /// <summary />
    [Inject]
    internal HttpClient HttpClient { get; set; } = default!;

    /// <summary />
    [Inject]
    internal NavigationManager NavigationManager { get; set; } = default!;

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

        Sections = await page.ExtractSectionsAsync();

        // Read CodeComments.json
        if (ApiDocSummary.Cached is null)
        {
            HttpClient.BaseAddress ??= new Uri(NavigationManager.BaseUri);
            ApiDocSummary.Cached = await HttpClient.LoadSummariesAsync("/CodeComments.json");
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);

            foreach (var section in Sections.Where(i => i.Type == SectionType.Component))
            {
                // Source Tab
                if (section.ExtraFiles.Count <= 0)
                {
                    var url = string.Format(System.Globalization.CultureInfo.InvariantCulture, DocViewerService.Options.SourceCodeUrl, section.Value + ".razor");
                    await _jsModule.InvokeVoidAsync("loadAndHighlightCode", section.Id, url);
                }

                // Extra files
                else
                {
                    foreach (var (tabName, file) in section.ExtraFiles)
                    {
                        var url = string.Format(System.Globalization.CultureInfo.InvariantCulture, DocViewerService.Options.SourceCodeUrl, file);
                        await _jsModule.InvokeVoidAsync("loadAndHighlightCode", $"{section.Id}-{tabName}", url);
                    }
                }
            }

            foreach (var section in Sections.Where(i => i.Type == SectionType.Code))
            {
                await _jsModule.InvokeVoidAsync("applyHighlight", section.Id);
            }
        }
    }

    private ApiClass? GetApiClassFromName(string? name, bool allProperties = false)
    {
        var type = DocViewerService.ApiAssembly
                                  ?.GetTypes()
                                  ?.FirstOrDefault(i => i.Name == name || i.Name.StartsWith($"{name}`1"));

        return type is null ? null : new ApiClass(DocViewerService, type, allProperties);
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

    /// <summary />
    private static string GetLanguageClassName(string? file = null)
    {
        if (string.IsNullOrEmpty(file))
        {
            return "language-razor";
        }

        return Path.GetExtension(file) switch
        {
            ".cs" => "language-csharp",
            ".razor" => "language-razor",
            ".html" => "language-html",
            ".css" => "language-css",
            ".js" => "language-javascript",
            _ => "language-plaintext"
        };
    }
}
