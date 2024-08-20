using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMenu : FluentComponentBase, IDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Menu/FluentMenu.razor.js";

    private bool _opened = false;
    private DotNetObjectReference<FluentMenu>? _dotNetHelper = null;
    private Point _clickedPoint = default;
    private bool _contextMenu = false;
    private readonly Dictionary<string, FluentMenuItem> items = [];
    private IMenuService? _menuService = null;
    private IJSObjectReference _jsModule = default!;

    /// <summary />
    internal string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    internal string? StyleValue => new StyleBuilder(Style)
        .AddStyle("min-width: max-content")
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("border-radius: calc(var(--layer-corner-radius) * 1px)")

        // For Anchored == false
        .AddStyle("z-index", $"{ZIndex.Menu}", () => !Anchored)
        .AddStyle("position", "fixed", () => !Anchored && !string.IsNullOrEmpty(Anchor))
        .AddStyle("width", "unset", () => !Anchored)
        .AddStyle("height", "unset", () => !Anchored)
        .AddStyle("left", $"{_clickedPoint.X}px", () => !Anchored && _clickedPoint.X != 0)
        .AddStyle("top", $"{_clickedPoint.Y}px", () => !Anchored && _clickedPoint.Y != 0)
        .Build();

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    [Inject]
    public IServiceProvider? ServiceProvider { get; set; } // https://github.com/dotnet/aspnetcore/issues/24193

    /// <summary />
    protected virtual IMenuService? MenuService => _menuService;

    /// <summary>
    /// Use IMenuService to create the menu, if this service was injected.
    /// This value must be defined before the component is rendered (you can't change it during the component lifecycle).
    /// Default, true.
    /// </summary>
    [Parameter]
    public bool UseMenuService { get; set; } = true;

    /// <summary>
    /// Gets or sets the identifier of the source component clickable by the end user.
    /// </summary>
    [Parameter]
    public string Anchor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the automatic trigger. See <seealso cref="MouseButton"/>
    /// Possible values are None (default), Left, Middle, Right, Back, Forward
    /// </summary>
    [Parameter]
    public MouseButton Trigger { get; set; } = MouseButton.None;

    /// <summary>
    /// Gets or sets the Menu status.
    /// </summary>
    [Parameter]
    public bool Open
    {
        get
        {
            return _opened;
        }

        set
        {
            if (_opened != value)
            {
                _opened = value;
                if (DrawMenuWithService)
                {
                    UpdateMenuProviderAsync().ConfigureAwait(true);
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the horizontal menu position.
    /// </summary>
    [Parameter]
    public HorizontalPosition HorizontalPosition { get; set; } = HorizontalPosition.Unset;

    /// <summary>
    /// Gets or sets a value indicating whether the region overlaps the anchor on the horizontal axis. 
    /// Default is false which places the region adjacent to the anchor element.
    /// </summary>
    [Parameter]
    public bool HorizontalInset { get; set; } = true;

    /// <summary>
    /// Gets or sets the vertical menu position.
    /// </summary>
    [Parameter]
    public VerticalPosition VerticalPosition { get; set; } = VerticalPosition.Bottom;

    /// <summary>
    /// Gets or sets a value indicating whether the region overlaps the anchor on the vertical axis.
    /// </summary>
    [Parameter]
    public bool VerticalInset { get; set; } = false;

    /// <summary>
    /// Gets or sets the width of this menu.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Raised when the <see cref="Open"/> property changed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Draw the menu below the component clicked (true) or
    /// using the mouse coordinates (false).
    /// </summary>
    [Parameter]
    public bool Anchored { get; set; } = true;

    /// <summary>
    /// Gets or sets how short the space allocated to the default position has to be before the tallest area is selected for layout.
    /// </summary>
    [Parameter]
    public int VerticalThreshold { get; set; } = 0;

    /// <summary>
    /// Gets or sets how narrow the space allocated to the default position has to be before the widest area is selected for layout.
    /// </summary>
    [Parameter]
    public int HorizontalThreshold { get; set; } = 200;

    protected override void OnInitialized()
    {
        if (Anchored && string.IsNullOrEmpty(Anchor))
        {
            Anchored = false;
        }

        _menuService = ServiceProvider?.GetService<IMenuService>();
        if (MenuService != null && DrawMenuWithService)
        {
            MenuService.Add(this);
        }

        base.OnInitialized();
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (Trigger != MouseButton.None)
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));

                _dotNetHelper = DotNetObjectReference.Create(this);

                if (Anchor is not null)
                {
                    // Add LeftClick event
                    if (Trigger == MouseButton.Left)
                    {
                        await _jsModule.InvokeVoidAsync("addEventLeftClick", Anchor, _dotNetHelper);
                    }

                    // Add RightClick event
                    if (Trigger == MouseButton.Right)
                    {
                        _contextMenu = true;
                        await _jsModule.InvokeVoidAsync("addEventRightClick", Anchor, _dotNetHelper);
                    }
                }
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MenuChangeEventArgs))]
    public FluentMenu()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Close the menu.
    /// </summary>
    /// <returns></returns>
    public async Task CloseAsync()
    {
        Open = false;

        await OpenChanged.InvokeAsync(Open);

    }

    /// <summary>
    /// Method called from JavaScript to get the current mouse ccordinates.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    [JSInvokable]
    public async Task OpenAsync(int x, int y)
    {
        _clickedPoint = new Point(x, y);
        Open = true;

        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }

        StateHasChanged();
    }

    internal void Register(FluentMenuItem item)
    {
        items.Add(item.Id!, item);
    }

    internal void Unregister(FluentMenuItem item)
    {
        items.Remove(item.Id!);
    }

    /// <summary />
    private bool DrawMenuWithoutService
    {
        get
        {
            return MenuService is not null && UseMenuService == true && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Anchor) && Anchored == true
                ? false     // Use the MenuService to draw the menu
                : true;     // Use the default way to draw the menu
        }
    }

    /// <summary />
    private bool DrawMenuWithService => !DrawMenuWithoutService;

    /// <summary />
    private async Task UpdateMenuProviderAsync()
    {
        if (MenuService == null || DrawMenuWithoutService)
        {
            return;
        }

        if (string.IsNullOrEmpty(Id))
        {
            throw new ArgumentNullException(nameof(Id), $"The {nameof(Id)} attribute is required.");
        }

        await MenuService.RefreshMenuAsync(Id, Open);
    }

    /// <summary />
    internal async Task SetOpenAsync(bool value)
    {
        Open = value;
        StateHasChanged();

        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
    }

    /// <summary>
    /// Dispose this menu.
    /// </summary>
    public void Dispose()
    {
        _dotNetHelper?.Dispose();
    }
}
