// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentOverflow : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Overflow/FluentOverflow.razor.js";
    private readonly List<FluentOverflowItem> _items = [];
    private DotNetObjectReference<FluentOverflow>? _dotNetHelper;

    /// <summary />
    protected virtual string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-overflow")
        .Build();

    /// <summary />
    protected virtual string? StyleValue => DefaultStyleBuilder
        .AddStyle("visibility", "hidden", !VisibleOnLoad)
        .Build();

    /// <summary />
    public FluentOverflow(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    internal FluentOverflow(LibraryConfiguration configuration, List<FluentOverflowItem> items) : this(configuration)
    {
        _items = items;
    }

    /// <summary>
    /// Gets or sets the template to display <see cref="ItemsOverflow"/> elements.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentOverflow>? OverflowTemplate { get; set; }

    /// <summary>
    /// To prevent a flickering effect, set this property to False to hide the overflow items until the component is fully loaded.
    /// </summary>
    [Parameter]
    public bool VisibleOnLoad { get; set; } = true;

    /// <summary>
    /// Gets or sets the template to display the More button.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentOverflow>? MoreButtonTemplate { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the items flow.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the CSS selectors of the items to include in the overflow.
    /// </summary>
    [Parameter]
    public string? Selectors { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets if the tooltip should be displayed by using the TooltipService
    /// </summary>
    [Parameter]
    public bool UseTooltipService { get; set; } = false;

    /// <summary>
    /// Event raised when a <see cref="FluentOverflowItem"/> enter or leave the current panel.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<FluentOverflowItem>> OnOverflowRaised { get; set; }

    /// <summary>
    /// Gets or sets the content to display.
    /// All first level HTML elements are included in the items flow.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets all items with <see cref="FluentOverflowItem.Overflow"/> assigned to True.
    /// </summary>
    public IEnumerable<FluentOverflowItem> ItemsOverflow => _items.Where(i => i.Overflow == true);

    /// <summary>
    /// Gets the unique identifier associated to the more button ([Id]-more).
    /// </summary>
    public string IdMoreButton => $"{Id}-more";

    private bool IsHorizontal => Orientation == Orientation.Horizontal;

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            _dotNetHelper = DotNetObjectReference.Create(this);
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Overflow.Initialize", _dotNetHelper, Id, IsHorizontal, Selectors, 25);
            VisibleOnLoad = true;
        }
    }

    /// <summary>
    /// Asynchronously refreshes the overflow state of the associated UI element.
    /// </summary>
    /// <remarks>Call this method to update the overflow indicators when the content or layout of the element
    /// changes. This method has no effect if the underlying JavaScript module is not loaded.</remarks>
    /// <returns>A task that represents the asynchronous refresh operation.</returns>
    public async Task RefreshAsync()
    {
        if (JSModule is not null)
        {
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Overflow.Refresh", _dotNetHelper, Id, IsHorizontal, Selectors, 25);
        }
    }

    /// <summary />
    [JSInvokable]
    public async Task OverflowRaisedAsync(OverflowItem[] items)
    {

        if (items == null || items.Length == 0)
        {
            return;
        }

        // Update Item components
        foreach (var item in items)
        {
            var element = _items.FirstOrDefault(i => string.Equals(i.Id, item.Id, StringComparison.OrdinalIgnoreCase));
            element?.SetProperties(item.Overflow, item.Text);
        }

        // Raise event
        if (OnOverflowRaised.HasDelegate)
        {
            await OnOverflowRaised.InvokeAsync(ItemsOverflow);
        }

        await InvokeAsync(StateHasChanged);
    }
    internal async Task RegisterAsync(FluentOverflowItem item)
    {
        await InvokeAsync(() => _items.Add(item));
    }

    internal async Task UnregisterAsync(FluentOverflowItem item)
    {
        _items.Remove(item);
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Overflow.Dispose", item.Id);
    }

    /// <inheritdoc />
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Overflow.Dispose", Id);
        _dotNetHelper?.Dispose();
    }

    /// <summary>
    /// Represents an item that may be subject to overflow handling, typically used in scenarios where content or data
    /// exceeds a predefined limit.
    /// </summary>
    public class OverflowItem
    {
        /// <summary />
        public string? Id { get; set; }

        /// <summary />
        public bool? Overflow { get; set; }

        /// <summary />
        public string? Text { get; set; }
    }
}
