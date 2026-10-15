// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Menu component for handling menus and menu items in a user interface.
/// </summary>
public partial class FluentMenu : FluentComponentBase, ITooltipComponent
{
    private DotNetObjectReference<FluentMenu>? _dotNetHelper;
    private bool _openedChangedSubscribed;
    private bool _renderMenu = true;
    private bool _wasRendered;

    /// <summary>
    /// Constructs a new instance of <see cref="FluentMenu"/>.
    /// Sets the Id to a new random value
    /// </summary>
    public FluentMenu(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("--menu-max-height", Height, when: !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary>
    /// Gets or sets whether the menu opens on hover.
    /// </summary>
    [Parameter]
    public bool? OpenOnHover { get; set; }

    /// <summary>
    /// Gets or sets whether the menu opens on right click.
    /// Removes all other menu open interactions.
    /// </summary>
    [Parameter]
    public bool? OpenOnContext { get; set; }

    /// <summary>
    /// Gets or sets whether the menu closes automatically when the user scrolls outside of it.
    /// </summary>
    [Parameter]
    public bool? CloseOnScroll { get; set; }

    /// <summary>
    /// Gets or sets whether the menu stays open when an item is clicked.
    /// </summary>
    [Parameter]
    public bool? PersistOnItemClick { get; set; }

    /// <summary>
    /// Gets or sets the HTML element ID of the trigger element that opens this menu (e.g., <c>Trigger="my-button-id"</c>).
    /// When set, clicking the referenced element toggles the menu open or closed.
    /// </summary>
    [Parameter]
    public string? Trigger { get; set; }

    /// <summary>
    /// Gets or sets the max height of the menu, e.g. 300px
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the child content rendered inside the menu, typically <see cref="FluentMenuItem"/> elements.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Raised when a FluentMenuItem is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MenuItemEventArgs> OnClick { get; set; }

    /// <summary>
    /// Raised when a FluentMenuItem's Checked state changes.
    /// </summary>
    [Parameter]
    public EventCallback<MenuItemEventArgs> OnCheckedChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the menu's open state changes.
    /// </summary>
    /// <remarks>
    /// The callback receives <see langword="true"/> when the menu opens and <see langword="false"/> when it closes.
    /// </remarks>
    [Parameter]
    public EventCallback<bool> OpenedChanged { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the condition that determines whether the menu is rendered.
    /// </summary>
    [Parameter]
    public Func<bool>? RenderWhen { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    /// <summary />
    protected override void OnParametersSet()
    {
        _renderMenu = RenderWhen?.Invoke() ?? true;
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var subscribeToOpenedChanged = OpenedChanged.HasDelegate;

        if (Trigger != null && _renderMenu && (firstRender || !_wasRendered || (subscribeToOpenedChanged && !_openedChangedSubscribed)))
        {
            if (subscribeToOpenedChanged)
            {
                _dotNetHelper ??= DotNetObjectReference.Create(this);
            }

            await JSRuntime.InvokeFluentVoidAsync(
                "Microsoft.FluentUI.Blazor.Components.Menu.Initialize",
                Id,
                Trigger,
                RenderWhen is not null,
                subscribeToOpenedChanged ? _dotNetHelper : null);
        }

        _openedChangedSubscribed = subscribeToOpenedChanged;
        _wasRendered = _renderMenu;
    }

    /// <summary>
    /// Called by JavaScript when the menu's open state changes.
    /// </summary>
    /// <param name="opened">A value indicating whether the menu is open.</param>
    [JSInvokable("FluentMenu.OpenedChangedAsync")]
    public async Task OnOpenedChangedAsync(bool opened)
    {
        if (OpenedChanged.HasDelegate)
        {
            await OpenedChanged.InvokeAsync(opened);
        }
    }

    /// <summary>
    /// Close the menu.
    /// </summary>
    public async Task CloseMenuAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Menu.CloseMenu", Id);
    }

    /// <summary>
    /// Open the menu.
    /// </summary>
    public Task OpenMenuAsync()
    {
        return OpenMenuAsync(targetId: null, targetOffsetLeft: 0, targetOffsetTop: 0);
    }

    /// <summary>
    /// Open the menu.
    /// </summary>
    /// <param name="targetId">The id of the element to anchor the menu to. If null, it will open relative to the trigger.</param>
    /// <param name="targetOffsetLeft">The left offset from the target element to open the menu. Default is 0.</param>
    /// <param name="targetOffsetTop">The top offset from the target element to open the menu. Default is 0.</param>
    public Task OpenMenuAsync(string? targetId = null, int targetOffsetLeft = 0, int targetOffsetTop = 0)
    {
        return JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Menu.OpenMenu", Id, targetId, targetOffsetLeft, targetOffsetTop).AsTask();
    }

    internal async Task NotifyCheckedChangedAsync(MenuItemEventArgs args)
    {
        if (OnCheckedChanged.HasDelegate)
        {
            await OnCheckedChanged.InvokeAsync(args);
        }
    }

    internal async Task NotifyClickedAsync(MenuItemEventArgs args)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(args);
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_dotNetHelper is not null)
        {
            await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.Menu.Dispose", Id);
            _dotNetHelper.Dispose();
        }

        await base.DisposeAsync();
    }
}
