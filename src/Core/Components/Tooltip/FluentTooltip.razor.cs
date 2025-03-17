// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// 
/// </summary>
public partial class FluentTooltip : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Tooltip/FluentTooltip.razor.js";

    /// <summary>
    /// 
    /// </summary>
    public FluentTooltip()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("max-width", MaxWidth, when: () => !string.IsNullOrWhiteSpace(MaxWidth))
        .AddStyle("margin-inline", SpacingHorizontal, when: () => !string.IsNullOrWhiteSpace(SpacingHorizontal))
        .AddStyle("margin-block", SpacingVertical, when: () => !string.IsNullOrWhiteSpace(SpacingVertical))
        .Build();

    /// <summary>
    /// Gets or sets the component identifier associated with the tooltip (Required).
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Anchor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets number of milliseconds to delay the tooltip from showing/hiding on hover. Default is 250ms.
    /// </summary>
    [Parameter]
    public int? Delay { get; set; }

    /// <summary>
    /// Gets or sets the tooltip's position. See <see cref="Components.Positioning"/>.
    /// </summary>
    [Parameter]
    public Positioning? Positioning { get; set; }

    /// <summary>
    /// Gets or sets the maximum width of tooltip panel. Default is 240px.
    /// </summary>
    [Parameter]
    public string? MaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the tooltip's horizontal spacing. Default is 4px;
    /// </summary>
    [Parameter]
    public string? SpacingHorizontal { get; set; }

    /// <summary>
    /// Gets or sets the tooltip's horizontal spacing. Default is 4px;
    /// </summary>
    [Parameter]
    public string? SpacingVertical { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Callback for when the tooltip is dismissed.
    /// </summary>  
    [Parameter]
    public EventCallback<EventArgs> OnDismissed { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            var jsModule = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            // Call a function from the JavaScript module
            await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Tooltip.FluentTooltipInitialize", Id);
        }
    }
}
