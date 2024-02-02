using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentHorizontalScroll : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the scroll speed in pixels per second.
    /// </summary>
    [Parameter]
    public int Speed { get; set; } = 600;

    /// <summary>
    /// Gets or sets the CSS time value for the scroll transition duration. Overrides the `speed` attribute.
    /// </summary>
    [Parameter]
    public string? Duration { get; set; }

    /// <summary>
    /// Gets or sets the attribute used for easing, defaults to ease-in-out.
    /// </summary>
    [Parameter]
    public ScrollEasing? Easing { get; set; } = ScrollEasing.EaseInOut;

    /// <summary>
    /// Gets or sets the attribute to hide flippers from assistive technology.
    /// </summary>
    [Parameter]
    public bool? FlippersHiddenFromAt { get; set; }

    /// <summary>
    /// Gets or sets the view: default | mobile.
    /// </summary>
    [Parameter]
    public HorizontalScrollView? View { get; set; } = AspNetCore.Components.HorizontalScrollView.Default;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? _jsModule;

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(HorizontalScrollEventArgs))]

    public FluentHorizontalScroll()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/HorizontalScroll/FluentHorizontalScroll.razor.js");
        }
    }

    public void ScrollToPrevious()
    {
        _jsModule?.InvokeVoidAsync("scrollToPrevious", Element);
    }

    public void ScrollToNext()
    {
        _jsModule?.InvokeVoidAsync("scrollToNext", Element);
    }

    public void ScrollInView(int viewIndex)
    {
        _jsModule?.InvokeVoidAsync("scrollInView", Element, viewIndex);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
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
