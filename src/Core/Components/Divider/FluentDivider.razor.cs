using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDivider : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Divider/FluentDivider.razor.js";

    private IJSObjectReference _jsModule = default!;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the role of the element.
    /// </summary>
    [Parameter]
    public DividerRole? Role { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the divider.
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; } = AspNetCore.Components.Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await _jsModule.InvokeVoidAsync("setDividerAriaOrientation");
        }
    }

    /// <inheritdoc />
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

