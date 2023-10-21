using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDivider : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.Fast.Components.FluentUI/Components/Divider/FluentDivider.razor.js";

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    /// <summary>
    /// The role of the element.
    /// </summary>
    [Parameter]
    public DividerRole? Role { get; set; }

    /// <summary>
    /// The orientation of the divider.
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; } = AspNetCore.Components.Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        await Module.InvokeVoidAsync("setDividerAriaOrientation");
     
        await base.OnInitializedAsync();
    }
}

