using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

// Remember to replace the namespace below with your own project's namespace.
namespace FluentUI.Demo.Shared.Components;

public partial class TableOfContents : IAsyncDisposable
{
    private record Anchor(string Level, string Text, string Href, Anchor[] Anchors)
    {
        public virtual bool Equals(Anchor? other)
        {
            if (other is null)
            {
                return false;
            }

            if (Level != other.Level ||
                Text != other.Text ||
                Href != other.Href ||
                (Anchors?.Length ?? 0) != (other.Anchors?.Length ?? 0))
            {
                return false;
            }

            if (Anchors is not null &&
                Anchors.Length > 0)
            {
                for (var i = 0; i < Anchors.Length; i++)
                {
                    if (!Anchors[i].Equals(other.Anchors![i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
             => HashCode.Combine(Level, Text, Href);
    }

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
    /// Gets or sets a value indicating whether a 'Back to top' button should be rendered.
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
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
            "./_content/FluentUI.Demo.Shared/Components/TableOfContents.razor.js");
            var mobile = await _jsModule!.InvokeAsync<bool>("isDevice");

            if (mobile)
            {
                _expanded = false;
            }

            await BackToTopAsync();
            await QueryDomAsync();

        }
    }

    private async Task BackToTopAsync()
    {
        if (_jsModule is null)
        {
            return;
        }
        await _jsModule.InvokeAsync<Anchor[]?>("backToTop");
    }

    private async Task QueryDomAsync()
    {
        if (_jsModule is null)
        {
            return;
        }

        Anchor[]? foundAnchors = await _jsModule.InvokeAsync<Anchor[]?>("queryDomForTocEntries");

        if (AnchorsEqual(_anchors, foundAnchors))
        {
            return;
        }

        _anchors = foundAnchors;
        StateHasChanged();
    }

    private bool AnchorsEqual(Anchor[]? firstSet, Anchor[]? secondSet)
    {
        return (firstSet ?? [])
            .SequenceEqual(secondSet ?? []);
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
            await BackToTopAsync();
            await QueryDomAsync();
        }
        catch (Exception)
        {
            // Already disposed
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use `Async` suffix for async methods", Justification = "#vNext: To update in the next version")]
    public async Task Refresh()
    {
        await QueryDomAsync();
    }

    private RenderFragment? GetTocItems(IEnumerable<Anchor>? items)
    {
        if (items is not null)
        {
            return new RenderFragment(builder =>
            {
                var i = 0;

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
