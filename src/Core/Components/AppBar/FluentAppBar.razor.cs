// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using static Microsoft.FluentUI.AspNetCore.Components.FluentOverflow;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentAppBar component is a native Blazor component that allows users to have an app bar like the one in Teams.
/// It is a container for app bar items, which can be either <see cref="FluentAppBarItem"/> or any other component that implements <see cref="IAppBarItem"/>.
/// AppBar items can overflow into a popover (with search capabilities) when there is not enough space to display them all.
/// </summary>
public partial class FluentAppBar : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Overflow/FluentOverflow.razor.js";
    private const string OVERFLOW_SELECTOR = ".fluent-appbar-item";

    private readonly InternalAppBarContext _internalAppBarContext;
    private DotNetObjectReference<FluentAppBar>? _dotNetHelper;
    private bool _showMoreItems;
    private string? _searchTerm = string.Empty;
    private IEnumerable<IAppBarItem> _searchResults = [];
    private Orientation _previousOrientation;

    // ToDo: Implement focus on popup

    /// <summary />
    public FluentAppBar(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
        _internalAppBarContext = new(this);
        _previousOrientation = Orientation;
    }

    /// <summary>
    /// Gets or sets if the popover shows the search box.
    /// </summary>
    [Parameter]
    public bool PopoverShowSearch { get; set; } = true;

    /// <summary>
    /// Gets or sets the <see cref="AspNetCore.Components.Orientation"/> of the app bar.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    /// <summary>
    /// Event to be called when the visibility of the popover changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> PopoverVisibilityChanged { get; set; }

    /// <summary>
    /// Gets or sets the collections of app bar items.
    /// Use either this or ChildContent to define the content of the app bar.
    /// </summary>
    [Parameter]
    public IEnumerable<IAppBarItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the content to display (the app bar items, <see cref="FluentAppBarItem"/>).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets all app items with <see cref="IAppBarItem.Overflow"/> assigned to True.
    /// </summary>
    internal IEnumerable<IAppBarItem> AppsOverflow => _internalAppBarContext.Apps.Where(i => i.Value.Overflow == true).Select(v => v.Value);

    /// <summary />
    protected virtual string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-appbar")
        .Build();

    /// <summary />
    protected virtual string? StyleValue => DefaultStyleBuilder
        .AddStyle("display", "flex")
        .AddStyle("flex-direction", "row", Orientation == Orientation.Horizontal)
        .AddStyle("flex-direction", "column", Orientation == Orientation.Vertical)
        .AddStyle("height", "100%", Orientation == Orientation.Vertical)
        .AddStyle("width", "100%", Orientation == Orientation.Horizontal)
        .AddStyle("gap", "2px")
        .Build();

    /// <summary />
    protected override void OnInitialized()
    {
        _searchResults = AppsOverflow;
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetHelper = DotNetObjectReference.Create(this);
            // Overflow
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            await InitializeOverflowAsync();
        }
    }

    /// <summary />
    protected override async Task OnParametersSetAsync()
    {
        if (_previousOrientation != Orientation && JSModule.Imported)
        {
            _previousOrientation = Orientation;
            await InitializeOverflowAsync();
        }
    }

    /// <summary />
    [JSInvokable]
    public async Task OverflowRaisedAsync(OverflowItem[] items)
    {
        if (items is null || items.Length == 0)
        {
            return;
        }

        foreach (var item in items)
        {
            if (item.Id is not null)
            {
                var app = _internalAppBarContext.Apps[item.Id];
                app.Overflow = item.Overflow;
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task InitializeOverflowAsync()
    {
        if (JSModule is not null)
        {
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Overflow.Initialize", _dotNetHelper, Id, Orientation == Orientation.Horizontal, OVERFLOW_SELECTOR, 0);
            //await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Overflow.Refresh", _dotNetHelper, Id, Orientation == Orientation.Horizontal, OVERFLOW_SELECTOR, 0);
        }
    }

    private Task TogglePopoverAsync() => HandlePopoverToggleAsync(!_showMoreItems);

    private async Task HandlePopoverKeyDownAsync(FluentKeyCodeEventArgs args)
    {
        if (!string.Equals(args.TargetId, $"appbar-more-{Id}", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var handler = args.Key switch
        {
            KeyCode.Enter => HandlePopoverToggleAsync(!_showMoreItems),
            KeyCode.Right => HandlePopoverToggleAsync(value: true),
            KeyCode.Left => HandlePopoverToggleAsync(value: false),
            _ => Task.CompletedTask,
        };
        await handler;
    }

    private async Task HandlePopoverToggleAsync(bool value)
    {
        if (value == _showMoreItems)
        {
            return;
        }

        _showMoreItems = value;

        if (PopoverVisibilityChanged.HasDelegate)
        {
            await PopoverVisibilityChanged.InvokeAsync(_showMoreItems);
        }

        await Task.CompletedTask;
    }

    private void HandleSearch()
    {
        if (string.IsNullOrEmpty(_searchTerm))
        {
            _searchResults = AppsOverflow;
        }
        else
        {
            var filteredAppBarItems = AppsOverflow.Where(i => i.Text.Contains(_searchTerm, StringComparison.CurrentCultureIgnoreCase)).ToList();
            _searchResults = filteredAppBarItems;
        }
    }
}
