// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentAppBar component is a native Blazor component that allows users to have an app bar like the one in Teams.
/// It is a container for app bar items, which can be either <see cref="FluentAppBarItem"/> or any other component that implements <see cref="IAppBarItem"/>.
/// AppBar items can overflow into a popover (with search capabilities) when there is not enough space to display them all.
/// </summary>
public partial class FluentAppBar : FluentComponentBase
{
    private readonly InternalAppBarContext _internalAppBarContext;
    private bool _showMoreItems;
    private string? _searchTerm = string.Empty;
    private IEnumerable<IAppBarItem> _searchResults = [];

    /// <summary />
    [DynamicDependency(nameof(OnOverflowChangedAsync))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(OverflowChangedEventArgs))]
    public FluentAppBar(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
        _internalAppBarContext = new(this);
    }

    /// <summary>
    /// Gets or sets whether the popover shows a search box to filter overflowed items.
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
    /// Gets or sets whether to hide the active bar on the side/bottom of the active item.
    /// </summary>
    [Parameter]
    public bool? HideActiveIndicator { get; set; }

    /// <summary>
    /// Gets or sets the content to display (the app bar items, <see cref="FluentAppBarItem"/>).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    internal IEnumerable<IAppBarItem> AppsOverflow => _internalAppBarContext.Apps.Where(i => i.Value.Overflow == true).Select(v => v.Value);
    private string OverflowElementId => $"{Id}-overflow";

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
        .Build();

    /// <summary />
    protected virtual string? OverflowStyleValue => new StyleBuilder()
        .AddStyle("flex", "1 1 auto")
        .AddStyle("min-height", "0", Orientation == Orientation.Vertical)
        .AddStyle("min-width", "0", Orientation == Orientation.Horizontal)
        .AddStyle("height", "100%", Orientation == Orientation.Vertical)
        .AddStyle("width", "100%", Orientation == Orientation.Horizontal)
        .AddStyle("gap", "2px")
        .Build();

    /// <summary />
    protected override void OnInitialized()
    {
        _searchResults = AppsOverflow;
    }

    private async Task OnOverflowChangedAsync(OverflowChangedEventArgs args)
    {
        if (!string.Equals(args.Id, OverflowElementId, StringComparison.Ordinal))
        {
            return;
        }

        ApplyOverflowState(args.FirstOverflowIndex, args.OrderedItemIds);
        await InvokeAsync(StateHasChanged);
    }

    /// <summary />
    public async Task OverflowRaisedAsync(OverflowItem[] items)
    {
        foreach (var item in items)
        {
            if (item.Id is not null && _internalAppBarContext.Apps.TryGetValue(item.Id, out var app))
            {
                app.Overflow = item.Overflow;
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    private void ApplyOverflowState(int firstOverflowIndex, IReadOnlyList<string>? orderedItemIds)
    {
        foreach (var app in _internalAppBarContext.Apps.Values)
        {
            app.Overflow = false;
        }

        if (orderedItemIds is null || orderedItemIds.Count == 0 || firstOverflowIndex < 0)
        {
            return;
        }

        for (var index = 0; index < orderedItemIds.Count; index++)
        {
            if (!_internalAppBarContext.Apps.TryGetValue(orderedItemIds[index], out var app))
            {
                continue;
            }

            app.Overflow = index >= firstOverflowIndex;
        }
    }

    internal Task TogglePopoverAsync() => HandlePopoverToggleAsync(!_showMoreItems);

    private async Task HandlePopoverKeyDownAsync(FluentKeyCodeEventArgs args)
    {
        if (!string.Equals(args.TargetId, $"appbar-more-{Id}", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var handler = args.Key switch
        {
            KeyCode.Enter => HandlePopoverToggleAsync(!_showMoreItems),
            KeyCode.Right when Orientation == Orientation.Vertical => HandlePopoverToggleAsync(value: true),
            KeyCode.Left when Orientation == Orientation.Vertical => HandlePopoverToggleAsync(value: false),
            KeyCode.Down when Orientation == Orientation.Horizontal => HandlePopoverToggleAsync(value: true),
            KeyCode.Up when Orientation == Orientation.Horizontal => HandlePopoverToggleAsync(value: false),
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
    }

    private void HandleSearch()
    {
        if (string.IsNullOrEmpty(_searchTerm))
        {
            _searchResults = AppsOverflow;
        }
        else
        {
            _searchResults = AppsOverflow.Where(i => i.Text.Contains(_searchTerm, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
    }
}
