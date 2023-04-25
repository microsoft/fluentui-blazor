
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;


namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentOverflow : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.Fast.Components.FluentUI/Components/Overflow/FluentOverflow.razor.js";
    private readonly List<FluentOverflowItem> _items = new();
    private RenderFragment? _childContent = null;
    private DotNetObjectReference<FluentOverflow>? _dotNetHelper = null;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-overflow")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .Build();

    /// <summary />
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary />
    private IJSObjectReference Module { get; set; } = default!;

    /// <summary>
    /// Content to display. All first HTML elements are included in the items flow.
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

            if (Module != null && _dotNetHelper != null)
            {
                bool isHorizontal = Orientation == Orientation.Horizontal;
                InvokeAsync(async () => await Module.InvokeVoidAsync("FluentOverflowInitialize", _dotNetHelper, Id, isHorizontal, null));
            }
        }
    }

    /// <summary>
    /// Template to display <see cref="ItemsOverflow"/> elements.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentOverflow>? OverflowTemplate { get; set; }

    /// <summary>
    /// Template to display the More button.
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

        Module = await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        _dotNetHelper = DotNetObjectReference.Create(this);

        bool isHorizontal = Orientation == Orientation.Horizontal;
        await Module.InvokeVoidAsync("FluentOverflowInitialize", _dotNetHelper, Id, isHorizontal, null);
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

    /// <summary />
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (Module is not null)
        {
            await Module.DisposeAsync();
        }

        if (_dotNetHelper is not null)
        {
            _dotNetHelper.Dispose();
        }
    }

    internal void AddItem(FluentOverflowItem item)
    {
        _items.Add(item);
    }
}
