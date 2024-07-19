using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentTooltip : FluentComponentBase, IDisposable
{
    private readonly Guid _guid = Guid.NewGuid();
    private ITooltipService? _tooltipService = null;
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Tooltip/FluentTooltip.razor.js";

    private IJSObjectReference? JSModule { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary>
    /// Gets or sets a reference to the list of registered services.
    /// </summary>
    /// <remarks>
    /// https://github.com/dotnet/aspnetcore/issues/24193
    /// </remarks>
    [Inject]
    internal IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// Gets a reference to the tooltip service (if registered).
    /// </summary>
    protected virtual ITooltipService? TooltipService => _tooltipService;

    /// <summary>
    /// Gets the default tooltip options.
    /// </summary>
    protected virtual TooltipGlobalOptions? GlobalOptions => TooltipService?.GlobalOptions;

    /// <summary>
    /// Gets or sets the value indicating whether the library should close the tooltip if the cursor leaves the anchor and the tooltip.
    /// By default, the tooltip closes if the cursor leaves the anchor, but not the tooltip itself.
    /// You can configure this behavior globally using the <see cref="LibraryConfiguration.HideTooltipOnCursorLeave"/> property.
    /// </summary>
    [Parameter]
    public bool? HideTooltipOnCursorLeave { get; set; }

    /// <summary>
    /// Use ITooltipService to create the tooltip, if this service was injected.
    /// If the <see cref="ChildContent"/> is dynamic, set this to false.
    /// Default, true.
    /// </summary>
    [Parameter]
    public bool UseTooltipService { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the tooltip is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

    /// <summary>
    /// Required. Gets or sets the control identifier associated with the tooltip.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Anchor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the delay (in milliseconds). 
    /// Default is 300.
    /// </summary>
    [Parameter]
    public int? Delay { get; set; } = TooltipGlobalOptions.DefaultDelay;

    /// <summary>
    /// Gets or sets the tooltip's position. See <see cref="AspNetCore.Components.TooltipPosition"/>.
    /// Don't set this if you want the tooltip to use the best position.
    /// </summary>
    [Parameter]
    public TooltipPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the maximum width of tooltip panel.
    /// </summary>
    [Parameter]
    public string? MaxWidth { get; set; }

    /// <summary>
    /// Controls when the tooltip updates its position, default is anchor which only updates when
    /// the anchor is resized. Auto will update on scroll/resize events.
    /// Corresponds to anchored-region auto-update-mode.
    /// </summary>
    [Parameter]
    public AutoUpdateMode? AutoUpdateMode { get; set; }

    /// <summary>
    /// Gets or sets whether the horizontal viewport is locked.
    /// </summary>
    [Parameter]
    public bool HorizontalViewportLock { get; set; }

    /// <summary>
    /// Gets or sets whether the vertical viewport is locked.
    /// </summary>
    [Parameter]
    public bool VerticalViewportLock { get; set; }

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
    private bool DrawTooltip => TooltipService == null ||
                                (TooltipService != null && !UseTooltipService);

    /// <summary />
    private void HandleDismissed()
    {
        if (OnDismissed.HasDelegate)
        {
            OnDismissed.InvokeAsync(EventArgs.Empty);
        }
    }

    /// <summary />
    protected override void OnInitialized()
    {
        HideTooltipOnCursorLeave = HideTooltipOnCursorLeave ?? LibraryConfiguration?.HideTooltipOnCursorLeave;
        _tooltipService = ServiceProvider?.GetService<ITooltipService>();

        if (TooltipService != null && UseTooltipService)
        {
            TooltipService.Add(new TooltipOptions()
            {
                Id = _guid,
                Anchor = Anchor,
                ChildContent = ChildContent,
                MaxWidth = MaxWidth,
                Delay = Delay,
                Position = Position,
                OnDismissed = OnDismissed,
                Visible = Visible,
            });
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !string.IsNullOrEmpty(Anchor) && HideTooltipOnCursorLeave == true)
        {
            JSModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            await JSModule.InvokeVoidAsync("tooltipHideOnCursorLeave", Anchor);
        }
    }

    /// <summary />
    public void Dispose()
    {
        if (TooltipService != null)
        {
            TooltipService?.Remove(_guid);
        }
    }
}
