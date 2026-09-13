// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentOverflow<TItem> : FluentComponentBase
{
    private IReadOnlyList<TItem> _sourceItems = [];
    private IReadOnlyList<TItem> _overflowItems = [];
    private IReadOnlyList<OverflowItem> _renderedOverflowItems = [];
    private int _measuredOverflowCount;
    private int[] _overflowIndices = [];

    private int RenderedItemCount => MaxRenderedItems > 0 ? Math.Min(MaxRenderedItems, _sourceItems.Count) : _sourceItems.Count;
    private int PreOverflowCount => _sourceItems.Count - RenderedItemCount;
    private OverflowContext<TItem> OverflowContext => new(_overflowItems, ItemsOverflow, OverflowCount, IdMoreButton);

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
    public RenderFragment<OverflowContext<TItem>>? OverflowTemplate { get; set; }

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
    public RenderFragment<OverflowContext<TItem>>? MoreTemplate { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the default overflow trigger is activated.
    /// Custom MoreTemplate content handles its own interaction.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnMoreClick { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the items flow.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the source items, in display order.
    /// </summary>
    [Parameter]
    public IEnumerable<TItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of source items rendered for measurement when <see cref="Items"/> is supplied.
    /// Remaining source items are included in the typed overflow context without being rendered.
    /// Values less than or equal to zero are unlimited. Defaults to zero.
    /// </summary>
    [Parameter]
    public int MaxRenderedItems { get; set; }

    /// <summary>
    /// Gets or sets the CSS selector of direct children to include in overflow.
    /// Applies only when Items is not supplied. Null or empty selects all direct children.
    /// </summary>
    [Parameter]
    public string? Selector { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the item template. Each item must produce exactly one root HTML element.
    /// Used when <see cref="Items"/> is supplied.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the text selector used by the default overflow tooltip.
    /// Defaults to the string representation of each source item.
    /// </summary>
    [Parameter]
    public Func<TItem, string>? ItemText { get; set; }

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
    /// Gets or sets direct child content, rendered once when Items is not supplied.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the rendered overflow records returned by the web component.
    /// Typed source items, including those omitted from the DOM, are available through the template context's Items.
    /// </summary>
    public IReadOnlyList<OverflowItem> ItemsOverflow => _renderedOverflowItems;

    /// <summary>
    /// Gets the total number of overflowed items.
    /// </summary>
    public int OverflowCount => Items is null ? _measuredOverflowCount : _overflowItems.Count;

    /// <summary>
    /// Gets the unique identifier associated to the more button ([Id]-more).
    /// </summary>
    public string IdMoreButton => $"{Id}-more";

    private bool HasDefaultMoreAction => MoreTemplate is null && OnMoreClick.HasDelegate;

    /// <summary />
    protected virtual string? MoreButtonStyleValue => new StyleBuilder()
        .AddStyle("anchor-name", $"--{IdMoreButton}")
        .Build();

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (Items is null)
        {
            _sourceItems = [];
            _overflowItems = [];
            return;
        }

        _sourceItems = Items?.ToArray() ?? [];
        _overflowIndices = _overflowIndices.Where(index => index >= 0 && index < RenderedItemCount).ToArray();
        UpdateOverflowItems();
    }

    private void UpdateOverflowItems()
    {
        _overflowItems = _overflowIndices
            .Where(index => index >= 0 && index < RenderedItemCount)
            .Distinct()
            .Order()
            .Select(index => _sourceItems[index])
            .Concat(_sourceItems.Skip(RenderedItemCount))
            .ToArray();
    }

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
        _renderedOverflowItems = items;
        _measuredOverflowCount = items.Length;
        _overflowIndices = items.Select(item => item.Index).ToArray();
        UpdateOverflowItems();

        if (OnOverflowRaised.HasDelegate)
        {
            await OnOverflowRaised.InvokeAsync(ItemsOverflow);
        }

        await InvokeAsync(StateHasChanged);
    }

    private async Task OnMoreKeyDownAsync(KeyboardEventArgs args)
    {
        if (HasDefaultMoreAction && args.Key is "Enter" or " ")
        {
            await OnMoreClick.InvokeAsync(new MouseEventArgs());
        }
    }

    private async Task OnOverflowChangedAsync(OverflowChangedEventArgs args)
    {
        if (!string.Equals(args.Id, Id, StringComparison.Ordinal))
        {
            return;
        }

        _renderedOverflowItems = args.Items?.Select(item => new OverflowItem
        {
            Id = item.Id,
            Text = item.Text,
            Index = item.Index,
        }).ToArray() ?? [];
        _measuredOverflowCount = Math.Max(0, args.OverflowCount);
        _overflowIndices = _renderedOverflowItems.Select(item => item.Index).ToArray();
        UpdateOverflowItems();

        if (OnOverflowRaised.HasDelegate)
        {
            await OnOverflowRaised.InvokeAsync(ItemsOverflow);
        }

        await InvokeAsync(StateHasChanged);
    }
}
