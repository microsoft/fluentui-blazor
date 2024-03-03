using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentAppBar : FluentComponentBase
{
    private const string OVERFLOW_SELECTOR = ".fluent-appbar-item";
    private readonly Dictionary<string, FluentAppBarItem> _apps = [];
    private DotNetObjectReference<FluentAppBar>? _dotNetHelper = null;
    private IJSObjectReference _jsModuleOverflow = default!;
    private bool _showMoreItems = false;
    private string? _searchTerm = string.Empty;
    private IEnumerable<FluentAppBarItem> _searchResults = [];

    private FluentSearch? _appSearch;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets if the popover shows the search box.
    /// </summary>
    [Parameter]
    public bool PopoverShowSearch { get; set; } = true;

    /// <summary>
    /// Event to be called when the visibility of the popover changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> PopoverVisibilityChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to display (the app bar items, <see cref="FluentAppBarItem"/>).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets all app items with <see cref="FluentAppBarItem.Overflow"/> assigned to True.
    /// </summary>
    public IEnumerable<FluentAppBarItem> AppsOverflow => _apps.Where(i => i.Value.Overflow == true).Select(v => v.Value);

    internal string? ClassValue => new CssBuilder("nav-menu-container")
        .AddClass(Class)
        .Build();

    internal string? StyleValue => new StyleBuilder(Style)
        .AddStyle("display: flex")
        .Build();

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
            _jsModuleOverflow = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Overflow/FluentOverflow.razor.js");

            await _jsModuleOverflow.InvokeVoidAsync("FluentOverflowInitialize", _dotNetHelper, Id, false, OVERFLOW_SELECTOR);
        }
    }

    public FluentAppBar()
    {
        Id = Identifier.NewId();
    }

    internal void Register(FluentAppBarItem app)
    {
        _apps.Add(app.Id!, app);
    }

    internal void Unregister(FluentAppBarItem app)
    {
        if (_apps.Count > 0)
        {
            _apps.Remove(app.Id!);
        }
    }

    /// <summary />
    [JSInvokable]
    public async Task OverflowRaisedAsync(string value)
    {
        var items = JsonSerializer.Deserialize<OverflowItem[]>(value);

        if (items == null)
        {
            return;
        }

        foreach (var item in items)
        {
            var tab = _apps.FirstOrDefault(i => i.Value.Id == item.Id).Value;
            tab?.SetProperties(item.Overflow);
        }

        await InvokeAsync(StateHasChanged);
    }

    private void TogglePopover()
    {
        _showMoreItems = !_showMoreItems;
    }

    private Task TogglePopoverAsync() => HandlePopoverToggleAsync(!_showMoreItems);

    private async Task HandlePopoverKeyDownAsync(FluentKeyCodeEventArgs args)
    {
            if (args.TargetId != $"appbar-more-{Id}")
        {
            return;
        }
        Task handler = args.Key switch
        {
            KeyCode.Enter => HandlePopoverToggleAsync(!_showMoreItems),
            KeyCode.Right => HandlePopoverToggleAsync(true),
            KeyCode.Left => HandlePopoverToggleAsync(false),
            _ => Task.CompletedTask
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

        //if (_showMoreItems)
        //{
            //StateHasChanged();
            //_appSearch?.FocusAsync();
        //}

        await Task.CompletedTask;
    }

    private void HandleSearch()
    {
        if (string.IsNullOrWhiteSpace(_searchTerm))
        {
            _searchResults = AppsOverflow;
        }
        else
        {
            _searchTerm = _searchTerm.ToLower();

            if (_searchTerm.Length > 0)
            {
                var temp = AppsOverflow.Where(i => i.Text.Contains(_searchTerm, StringComparison.CurrentCultureIgnoreCase)).ToList();
                if (temp.Count > 0)
                {
                    _searchResults = temp;
                }
            }
        }
    }
}
