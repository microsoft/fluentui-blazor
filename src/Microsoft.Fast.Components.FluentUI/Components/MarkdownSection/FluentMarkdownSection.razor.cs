using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Infrastructure;


namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentMarkdownSection : FluentComponentBase
{
    private string? _content;

    [Inject]
    private IStaticAssetService StaticAssetService { get; set; } = default!;


    /// <summary>
    /// Gets or sets the Markdown content 
    /// </summary>
    [Parameter]
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets asset to read the Markdown from
    /// </summary>
    [Parameter]
    public string? FromAsset { get; set; }

    public string? InternalContent
    {
        get => _content;
        set
        {
            _content = value;
            HtmlContent = ConvertToMarkupString(_content);
            StateHasChanged();
        }
    }

    public MarkupString HtmlContent { get; private set; }


    protected override void OnInitialized()
    {
        if (Content is null && string.IsNullOrEmpty(FromAsset))
            throw new ArgumentException("You need to provide either Content or FromAsset parameter");

        InternalContent = Content;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {

        if (firstRender && !string.IsNullOrEmpty(FromAsset))
            InternalContent = await StaticAssetService.GetAsync(FromAsset);

    }

    private MarkupString ConvertToMarkupString(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            // Convert markdown string to HTML
            string? html = Markdown.ToHtml(value, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());

            // Return sanitized HTML as a MarkupString that Blazor can render
            return new MarkupString(html);
        }

        return new MarkupString();
    }
}