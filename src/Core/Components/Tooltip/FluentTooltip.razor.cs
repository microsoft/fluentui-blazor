// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentTooltip displays additional information about another component.
/// The information is displayed above and near the target component.
/// </summary>
public partial class FluentTooltip : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Tooltip/FluentTooltip.razor.js";

    /// <summary />    
    [DynamicDependency(nameof(OnToggleAsync))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DialogToggleEventArgs))]
    public FluentTooltip()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("max-width", MaxWidth, when: () => !string.IsNullOrWhiteSpace(MaxWidth))
        .AddStyle("margin-inline", SpacingHorizontal, when: () => !string.IsNullOrWhiteSpace(SpacingHorizontal))
        .AddStyle("margin-block", SpacingVertical, when: () => !string.IsNullOrWhiteSpace(SpacingVertical))
        .Build();

    /// <summary>
    /// Gets or sets the injected service provider.
    /// </summary>
    /// <remarks>
    /// We cannot inject `ITooltipService` directly, as an exception will be thrown if the service is not injected.
    /// https://github.com/dotnet/aspnetcore/issues/24193
    /// </remarks>
    private ITooltipService? TooltipService => GetCachedServiceOrNull<ITooltipService>();

    /// <summary>
    /// Use ITooltipService to create the tooltip, if this service was injected.
    /// If the <see cref="ChildContent"/> is dynamic, set this to false. Default, true.
    /// </summary>
    [Parameter]
    public bool UseTooltipService { get; set; } = true;

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

    /// <summary>
    /// Callback for when the tooltip is opened or closed.
    /// </summary>  
    [Parameter]
    public EventCallback<TooltipEventArgs> OnToggle { get; set; }

    /// <summary />
    private bool DrawTooltipWithService => TooltipService is not null && UseTooltipService;

    /// <summary />
    private bool DrawTooltipWithoutService => !DrawTooltipWithService;

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        ArgumentNullException.ThrowIfNullOrEmpty(Id);

        if (DrawTooltipWithService && TooltipService != null)
        {
            if (string.IsNullOrEmpty(TooltipService.ProviderId))
            {
                throw new ArgumentNullException(nameof(UseTooltipService), "<FluentTooltipProvider /> needs to be added to the main layout of your application/site.");
            }

            TooltipService.Items.TryAdd(Id, this);
            await TooltipService.OnUpdatedAsync.Invoke(this);
        }

        await base.OnInitializedAsync();
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(Id);
        ArgumentNullException.ThrowIfNullOrEmpty(Anchor);

        if (firstRender)
        {
            // FluentTooltipInitialize will be removed when the WebComponents Teams will be ready.
            var jsModule = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
            await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Tooltip.FluentTooltipInitialize", Id);
        }
    }

    /// <summary />
    internal async Task OnToggleAsync(DialogToggleEventArgs args)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(Id);

        if (string.CompareOrdinal(args.Id, Id) != 0)
        {
            return;
        }

        var opened = string.CompareOrdinal(args.NewState, "open") == 0;

        if (OnToggle.HasDelegate)
        {
            await OnToggle.InvokeAsync(new TooltipEventArgs(Id, opened));
        }

        if (OnDismissed.HasDelegate && !opened)
        {
            await OnDismissed.InvokeAsync(EventArgs.Empty);
        }
    }
}
