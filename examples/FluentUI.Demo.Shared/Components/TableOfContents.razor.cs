using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

// Remember to replace the namespace below with your own project's namespace.
namespace FluentUI.Demo.Shared.Components;

public partial class TableOfContents : IAsyncDisposable
{
    private record Anchor(string Level, string Text, string Href, Anchor[] Anchors);
    private Anchor[]? _anchors;
    private bool _expanded = true;

    private IJSObjectReference _jsModule = default!;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets or sets the heading for the ToC 
    /// Defaults to 'In this article'
    /// </summary>
    [Parameter]
    public string Heading { get; set; } = "In this article";

    /// <summary>
    /// Gets or sets if a 'Back to top' button should be rendered.
    /// Defaults to true
    /// </summary>
    [Parameter]
    public bool ShowBackButton { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>

    [Parameter]
    public RenderFragment? ChildContent { get; set; }


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        //if (firstRender)
        //{
        // Remember to replace the location of the script with your own project specific location.
        _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
            "./_content/FluentUI.Demo.Shared/Components/TableOfContents.razor.js");

        bool mobile = await _jsModule!.InvokeAsync<bool>("isDevice");

        if (mobile)
            _expanded = false;

        await QueryDom();
        //}
    }

    private async Task BackToTop()
    {
        await _jsModule.InvokeAsync<Anchor[]?>("backToTop");
    }

    private async Task QueryDom()
    {
        _anchors = await _jsModule.InvokeAsync<Anchor[]?>("queryDomForTocEntries");
        StateHasChanged();


    }

    protected override void OnInitialized()
    {
        // Subscribe to the event
        NavigationManager.LocationChanged += LocationChanged;
    }

    private async void LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        try
        {
            await QueryDom();
        }
        catch (Exception)
        {
            // Already disposed
        }
    }

    public async Task Refresh()
    {
        await QueryDom();
        StateHasChanged();
    }

    private RenderFragment? GetTocItems(IEnumerable<Anchor>? items)
    {
        if (items is not null)
        {
            return new RenderFragment(builder =>
            {
                int i = 0;

                builder.OpenElement(i++, "ul");
                foreach (Anchor item in items)
                {
                    builder.OpenElement(i++, "li");
                    builder.OpenComponent<FluentAnchor>(i++);
                    builder.AddAttribute(i++, "Href", item.Href);
                    builder.AddAttribute(i++, "Appearance", Appearance.Hypertext);
                    builder.AddAttribute(i++, "ChildContent", (RenderFragment)(content =>
                    {
                        content.AddContent(i++, item.Text);
                    }));
                    builder.CloseComponent();
                    if (item.Anchors is not null)
                    {
                        builder.AddContent(i++, GetTocItems(item.Anchors));
                    }
                    builder.CloseElement();
                }
                builder.CloseElement();
            });
        }
        else
        {
            return new RenderFragment(builder =>
            {
                builder.AddContent(0, ChildContent);
            });
        }

    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        try
        {
            // Unsubscribe from the event when our component is disposed
            NavigationManager.LocationChanged -= LocationChanged;

            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}