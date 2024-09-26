// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentOverflow : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Overflow/FluentOverflow.razor.js";
    private readonly List<FluentOverflowItem> _items = [];
    private DotNetObjectReference<FluentOverflow>? _dotNetHelper = null;
    private IJSObjectReference _jsModule = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-overflow")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("visibility", "hidden", VisibleOnLoad == false)
        .Build();

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content to display.
    /// All first HTML elements are included in the items flow.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
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

    private bool IsHorizontal => Orientation == Orientation.Horizontal;

    public FluentOverflow()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            _dotNetHelper = DotNetObjectReference.Create(this);
            await _jsModule.InvokeVoidAsync("fluentOverflowInitialize", _dotNetHelper, Id, IsHorizontal, Selectors);
            VisibleOnLoad = true;
        }
    }

    public async Task RefreshAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("fluentOverflowRefresh", _dotNetHelper, Id, IsHorizontal, Selectors);
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

        StateHasChanged();
    }
    internal void Register(FluentOverflowItem item)
    {
        _items.Add(item);
    }

    internal void Unregister(FluentOverflowItem item)
    {
        _items.Remove(item);
        _jsModule?.InvokeVoidAsync("FluentOverflowDispose", item.Id);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            _dotNetHelper?.Dispose();

            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("FluentOverflowDispose", Id);
                await _jsModule.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
