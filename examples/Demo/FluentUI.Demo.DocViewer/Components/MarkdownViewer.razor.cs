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

    [Inject]
    public DocViewerService DocViewerService { get; set; } = default!;

    [Inject]
    internal IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public required string Route { get; set; }

    internal string PageTitle { get; private set; } = string.Empty;

    protected IEnumerable<Section> Sections { get; private set; } = [];

    protected override void OnInitialized()
    {
        var page = DocViewerService.FromRoute(Route);

        if (page is null)
        {
            PageTitle = "Page not found";
            _isPageNotFound = true;
            return;
        }

        _isPageNotFound = false;
        PageTitle = page.Title;
        var html = Markdown.ToHtml(page.Content, MarkdownPipeline);
        Sections = ExtractSections(html);
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await _jsModule.InvokeVoidAsync("applyHighlight");
        }
    }

    private RenderFragment RenderHtmlContent() => builder =>
    {
        var i = 0;

        foreach (var section in Sections)
        {
            switch (section.Type)
            {
                case SectionType.Html:
                    builder.AddMarkupContent(i++, section.Value);
                    break;

                case SectionType.Code:

                    var language = section.Arguments?[Section.ARGUMENT_LANGUAGE] ?? "text";

                    builder.OpenElement(i++, "pre");
                    builder.OpenElement(i++, "code");
                    builder.AddAttribute(i++, "id", section.Id);
                    builder.AddAttribute(i++, "class", $"language-{language}");

                    builder.AddMarkupContent(i++, section.Value);

                    builder.CloseElement();
                    builder.CloseElement();

                    break;

                case SectionType.Component:
                    var component = GetComponentFromName(section.Value);

                    if (component == null)
                    {
                        builder.AddMarkupContent(i++, $"<div class='component-not-found'>&#9888; The component \"{{{{ {section.Value} }}}}\" was not found.</div>");
                    }
                    else
                    {
                        builder.OpenComponent(i++, component);
                        builder.CloseComponent();
                    }

                    break;

                case SectionType.Api:
                    break;

                default:
                    break;
            }
        }
    };

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

    private Type? GetComponentFromName(string name)
    {
        return DocViewerService.ComponentsAssembly
                               .GetTypes()
                               .Where(t => t.IsSubclassOf(typeof(ComponentBase)) && !t.IsAbstract)
                               .FirstOrDefault(i => i.Name == name);
    }
}
