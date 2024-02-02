
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentOverflow : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Overflow/FluentOverflow.razor.js";
    private readonly List<FluentOverflowItem> _items = [];
    private RenderFragment? _childContent = null;
    private DotNetObjectReference<FluentOverflow>? _dotNetHelper = null;
    private IJSObjectReference _jsModule = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-overflow")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content to display. 
    /// All first HTML elements are included in the items flow.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent
    {
        get
        {
            return _childContent;
        }

        set
        {
            _childContent = value;

            if (_jsModule != null && _dotNetHelper != null)
            {
                var isHorizontal = Orientation == Orientation.Horizontal;
                InvokeAsync(async () => await _jsModule.InvokeVoidAsync("FluentOverflowInitialize", _dotNetHelper, Id, isHorizontal, null));
            }
        }
    }

    /// <summary>
    /// Gets or sets the template to display <see cref="ItemsOverflow"/> elements.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentOverflow>? OverflowTemplate { get; set; }

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
    /// Event raised when a <see cref="FluentOverflowItem"/> enter or leave the current panel.
    /// </summary>
    [Parameter]
    public EventCallback<IEnumerable<FluentOverflowItem>> OnOverflowRaised { get; set; }

    /// <summary>
    /// Gets all items with <see cref="FluentOverflowItem.Overflow"/> assigned to True.
    /// </summary>
    public IEnumerable<FluentOverflowItem> ItemsOverflow => _items.Where(i => i.Overflow == true);

    /// <summary>
    /// Gets the unique identifier associated to the more button ([Id]-more).
    /// </summary>
    public string IdMoreButton => $"{Id}-more";

    public FluentOverflow()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        _dotNetHelper = DotNetObjectReference.Create(this);

        var isHorizontal = Orientation == Orientation.Horizontal;
        await _jsModule.InvokeVoidAsync("FluentOverflowInitialize", _dotNetHelper, Id, isHorizontal, null);
    }

    /// <summary />
    [JSInvokable]
    public async Task OverflowRaisedAsync(string value)
    {
        OverflowItem[]? items = JsonSerializer.Deserialize<OverflowItem[]>(value);

        if (items == null)
        {
            return;
        }

        // Update Item components
        foreach (var item in items)
        {
            var element = _items.FirstOrDefault(i => i.Id == item.Id);
            element?.SetProperties(item.Overflow, item.Text);
        }

        // Raise event
        if (OnOverflowRaised.HasDelegate)
        {
            await OnOverflowRaised.InvokeAsync(ItemsOverflow);
        }

        await InvokeAsync(() => StateHasChanged());
    }

    internal void AddItem(FluentOverflowItem item)
    {
        _items.Add(item);
    }

    internal void RemoveItem(FluentOverflowItem item)
    {
        _items.Remove(item);
        StateHasChanged();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            _dotNetHelper?.Dispose();

            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
