// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentDragContainer<TItem> : FluentComponentBase
{
    private const string JAVASCRIPT_FILE =
        FluentJSModule.JAVASCRIPT_ROOT + "Drag/FluentDragContainer.razor.js";

    private readonly Dictionary<string, FluentDropZone<TItem>> _zones = [];
    private DotNetObjectReference<FluentDragContainer<TItem>>? _dotNetRef;
    private bool _touchInitialized;

    /// <summary />
    public FluentDragContainer(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets a value indicating whether items can be dragged using touch (long press, then move).
    /// This is required on devices such as iPhone and iPad where HTML5 drag and drop is not available.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool EnableTouchDrag { get; set; } = true;

    /// <summary>
    /// Gets or sets the time (in milliseconds) a touch must be held before a drag starts.
    /// Default is 200.
    /// </summary>
    [Parameter]
    public int TouchDragDelay { get; set; } = 200;

    /// <summary />
    protected virtual string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected virtual string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// This event is fired when the user starts dragging an element.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDragStart { get; set; }

    /// <summary>
    /// This event is fired when the drag operation ends (such as releasing a mouse button or hitting the Esc key).
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDragEnd { get; set; }

    /// <summary>
    /// This event is fired when a dragged element enters a valid drop target.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDragEnter { get; set; }

    /// <summary>
    /// This event is fired when an element is being dragged over a valid drop target.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDragOver { get; set; }

    /// <summary>
    /// This event is fired when a dragged element leaves a valid drop target.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDragLeave { get; set; }

    /// <summary>
    /// This event is fired when an element is dropped on a valid drop target.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDragEventArgs<TItem>> OnDropEnd { get; set; }

    /// <summary>
    /// property to keep the zone currently dragged.
    /// </summary>
    internal FluentDropZone<TItem>? StartedZone { get; private set; }

    /// <summary />
    internal void SetStartedZone(FluentDropZone<TItem>? value)
    {
        StartedZone = value;
        StateHasChanged();
    }

    /// <summary />
    internal void RegisterZone(FluentDropZone<TItem> zone)
    {
        if (!string.IsNullOrEmpty(zone.Id))
        {
            _zones[zone.Id] = zone;
        }
    }

    /// <summary />
    internal void UnregisterZone(FluentDropZone<TItem> zone)
    {
        if (!string.IsNullOrEmpty(zone.Id) && _zones.TryGetValue(zone.Id, out var registered) && ReferenceEquals(registered, zone))
        {
            _zones.Remove(zone.Id);
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && EnableTouchDrag)
        {
            if (!await JSModule.TryImportJavaScriptModuleAsync(JAVASCRIPT_FILE))
            {
                return;
            }

            _dotNetRef = DotNetObjectReference.Create(this);
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DragContainer.Initialize", Id, _dotNetRef, TouchDragDelay);
            _touchInitialized = true;
        }
    }

    /// <summary />
    public override async ValueTask DisposeAsync()
    {
        if (_touchInitialized && JSModule.Imported)
        {
            try
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DragContainer.Dispose", Id);
            }
            catch (Exception ex) when (ex is JSDisconnectedException or OperationCanceledException or ObjectDisposedException)
            {
                // The JS runtime may already be gone.
            }
        }

        _dotNetRef?.Dispose();
        _dotNetRef = null;
        await base.DisposeAsync();
    }

    /// <summary />
    [JSInvokable]
    public Task TouchDragStartAsync(string sourceId) => RunOnZoneAsync(sourceId, async zone => await zone.StartDragAsync());

    /// <summary />
    [JSInvokable]
    public Task TouchDragEnterAsync(string sourceId, string targetId) => RunOnZoneAsync(targetId, zone => zone.DragEnterAsync());

    /// <summary />
    [JSInvokable]
    public Task TouchDragOverAsync(string sourceId, string targetId) => RunOnZoneAsync(targetId, zone => zone.DragOverAsync());

    /// <summary />
    [JSInvokable]
    public Task TouchDragLeaveAsync(string sourceId, string targetId) => RunOnZoneAsync(targetId, zone => zone.DragLeaveAsync());

    /// <summary />
    [JSInvokable]
    public Task TouchDropAsync(string sourceId, string targetId) => RunOnZoneAsync(targetId, zone => zone.DropAsync());

    /// <summary />
    [JSInvokable]
    public Task TouchDragEndAsync(string sourceId) => RunOnZoneAsync(sourceId, async zone =>
    {
        await zone.EndDragAsync();
        if (StartedZone != null)
        {
            SetStartedZone(value: null);
        }
    });

    private Task RunOnZoneAsync(string zoneId, Func<FluentDropZone<TItem>, Task> action)
    {
        return InvokeAsync(async () =>
        {
            if (_zones.TryGetValue(zoneId, out var zone))
            {
                await action(zone);
                zone.Refresh();
            }
        });
    }
}
