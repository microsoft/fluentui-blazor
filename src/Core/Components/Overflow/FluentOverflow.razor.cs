// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentOverflow : FluentComponentBase
{
    private readonly List<OverflowItem> _items = [];
    private int _overflowCount;

    /// <summary />
    protected virtual string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-overflow")
        .Build();

    /// <summary />
    protected virtual string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary />
    [DynamicDependency(nameof(OnOverflowChangedAsync))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(OverflowChangedEventArgs))]
    public FluentOverflow(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the template to display <see cref="ItemsOverflow"/> elements.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentOverflow>? OverflowTemplate { get; set; }

    /// <summary>
    /// Gets or sets whether overflow items are visible immediately on load.
    /// Set to <see langword="false"/> to hide items until the component is fully loaded,
    /// preventing a flickering effect. Defaults to <see langword="true"/>.
    /// </summary>
    [Parameter]
    public bool VisibleOnLoad { get; set; } = true;

    /// <summary>
    /// Gets or sets the template to display the overflow trigger content.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentOverflow>? MoreTemplate { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the items flow.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the CSS selector of direct children to include in the overflow.
    /// If null or empty, all direct children except the built-in More button are considered.
    /// </summary>
    [Parameter]
    public string? Selector { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the maximum number of overflow items returned to the Blazor wrapper.
    /// Values less than or equal to zero return all overflow items. Defaults to zero (unlimited).
    /// </summary>
    [Parameter]
    public int MaxOverflowItems { get; set; }

    /// <summary>
    /// Gets or sets whether the tooltip is displayed using the TooltipService.
    /// </summary>
    [Parameter]
    public bool UseTooltipService { get; set; }

    /// <summary>
    /// Event raised when overflow items change.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<OverflowItem>> OnOverflowRaised { get; set; }

    /// <summary>
    /// Gets or sets the content to display.
    /// All first level HTML elements are included in the items flow.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the rendered overflow items returned from the web component.
    /// </summary>
    public IEnumerable<OverflowItem> ItemsOverflow => _items;

    /// <summary>
    /// Gets the total number of overflowed items.
    /// </summary>
    public int OverflowCount => _overflowCount;

    /// <summary>
    /// Gets the unique identifier associated to the more button ([Id]-more).
    /// </summary>
    public string IdMoreButton => $"{Id}-more";

    /// <summary />
    protected virtual string? MoreButtonStyleValue => new StyleBuilder()
        .AddStyle("anchor-name", $"--{IdMoreButton}")
        .Build();

    /// <summary>
    /// Requests an overflow recalculation. State changes are delivered through the overflow event.
    /// </summary>
    public async Task RefreshAsync()
    {
        if (JSRuntime is null)
        {
            return;
        }

        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Overflow.refreshOverflow", Id);
    }

    /// <summary />
    public async Task OverflowRaisedAsync(OverflowItem[] items)
    {
        _items.Clear();
        _items.AddRange(items);
        _overflowCount = items.Length;

        if (OnOverflowRaised.HasDelegate)
        {
            await OnOverflowRaised.InvokeAsync(ItemsOverflow);
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task OnOverflowChangedAsync(OverflowChangedEventArgs args)
    {
        if (!string.Equals(args.Id, Id, StringComparison.Ordinal))
        {
            return;
        }

        SetOverflowItems(args.Items, args.OverflowCount);

        if (OnOverflowRaised.HasDelegate)
        {
            await OnOverflowRaised.InvokeAsync(ItemsOverflow);
        }

        await InvokeAsync(StateHasChanged);
    }

    private void SetOverflowItems(IEnumerable<OverflowChangedItem>? items, int overflowCount)
    {
        _items.Clear();
        _overflowCount = Math.Max(overflowCount, 0);

        if (items is null)
        {
            return;
        }

        _items.AddRange(items
            .Select(item => new OverflowItem
            {
                Id = item.Id,
                Text = item.Text,
                Index = item.Index,
            }));
    }
}
