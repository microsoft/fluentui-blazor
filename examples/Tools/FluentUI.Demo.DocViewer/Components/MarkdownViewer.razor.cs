// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using FluentUI.Demo.DocViewer.Extensions;
using FluentUI.Demo.DocViewer.Models;
using FluentUI.Demo.DocViewer.Models.Mcp;
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

        // Read api-comments.json
        if (ApiDocSummary.Cached is null)
        {
            HttpClient.BaseAddress ??= new Uri(NavigationManager.BaseUri);
            ApiDocSummary.Cached = await HttpClient.LoadSummariesAsync("/api-comments.json");
        }

        // Load MCP documentation if any MCP or McpSummary section is present
        if (Sections.Any(s => s.Type == SectionType.Mcp || s.Type == SectionType.McpSummary) && McpDocumentationService.Cached is null)
        {
            HttpClient.BaseAddress ??= new Uri(NavigationManager.BaseUri);
            await McpDocumentationService.LoadAsync(HttpClient, "/mcp-documentation.json").ConfigureAwait(true);
            StateHasChanged();
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

        if (string.IsNullOrEmpty(name))
        {
            return null;
        }

        var componentName = name;
        var componentType = "";

        // Convert "MyComponent<MyType>" to ("MyComponent", "MyType")
        var match = Regex.Match(name, @"(\w+)(&lt;|<)(.+)(>|&gt;)");
        if (match.Success)
        {
            componentName = match.Groups[1].Value;
            componentType = match.Groups[3].Value;
        }

        // Get the component type
        var type = DocViewerService.ApiAssembly
                                  ?.GetTypes()
                                  ?.FirstOrDefault(i => i.Name == componentName
                                                     || i.Name.StartsWith($"{componentName}`1")
                                                     || i.Name.StartsWith($"{componentName}`2"));

        // Create the ApiClass
        var result = type is null ? null : new ApiClass(DocViewerService, type, allProperties);

        // if the component type is specified, set the InstanceTypes
        if (result != null && !string.IsNullOrEmpty(componentType))
        {
            var listOfTypes = new List<Type>();

            foreach (var typeName in componentType.Split(','))
            {
                var t = ReflectionExtensions.KnownTypeNames
                                               .FirstOrDefault(i => string.Compare(i.Value, typeName.Trim(), StringComparison.InvariantCultureIgnoreCase) == 0)
                                               .Key;

                if (t is not null)
                {
                    listOfTypes.Add(t);
                }
            }

            
            result.InstanceTypes = listOfTypes.ToArray();
        }

        return result;
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

    /// <summary>
    /// Parses a string to an McpType enum value.
    /// </summary>
    private static McpType ParseMcpType(string? value)
    {
        return value?.ToLower() switch
        {
            "resources" => McpType.Resources,
            "prompts" => McpType.Prompts,
            _ => McpType.Tools
        };
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
