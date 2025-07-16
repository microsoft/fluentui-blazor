// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentToolbar : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Toolbar/FluentToolbar.razor.js";

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// Gets or sets the toolbar's orentation. See <see cref="Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; } = AspNetCore.Components.Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether arrow key navigation within text input fields is enabled. Default is false.
    /// </summary>
    [Parameter]
    public bool? EnableArrowKeyTextNavigation { get; set; } = false;

    public FluentToolbar()
    {
        Id = Identifier.NewId();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));

            if (EnableArrowKeyTextNavigation ?? false)
            {
                await Module.InvokeVoidAsync("preventArrowKeyNavigation", Id);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (Module is not null && !string.IsNullOrEmpty(Id))
            {
                await Module.InvokeVoidAsync("removePreventArrowKeyNavigation", Id);
                await Module.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
