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
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Drag/FluentDragContainer.razor.js";

    private readonly Dictionary<string, FluentDropZone<TItem>> _zones = new(StringComparer.Ordinal);
    private DotNetObjectReference<FluentDragContainer<TItem>>? _dotNetRef;
    private FluentDragGroup<TItem>? _registeredGroup;
    private FluentDropZone<TItem>? _startedZone;
    private FluentDropZone<TItem>? _touchSource;
    private bool _touchInitialized;

    /// <summary />
    public FluentDragContainer(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the time (in milliseconds) a touch must be held before a drag starts.
    /// Default is 100.
    /// </summary>
    [Parameter]
    public int TouchDragDelay { get; set; } = 100;

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
    /// Gets or sets the group shared with other containers of the same item type.
    /// Containers using the same group instance allow moving items from one container to another.
    /// The zone identifiers must be unique across the containers of the group.
    /// </summary>
    [Parameter]
    public FluentDragGroup<TItem>? Group { get; set; }

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
    internal FluentDropZone<TItem>? StartedZone => Group is null ? _startedZone : Group.StartedZone;

    /// <summary />
    internal void SetStartedZone(FluentDropZone<TItem>? value)
    {
        if (Group is null)
        {
            _startedZone = value;
            StateHasChanged();
            return;
        }

        Group.SetStartedZone(value);
    }

    /// <summary />
    internal void Refresh() => StateHasChanged();

    /// <summary />
    internal bool TryGetLocalZone(string zoneId, out FluentDropZone<TItem> zone)
    {
        return _zones.TryGetValue(zoneId, out zone!);
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
    protected override void OnParametersSet()
    {
        if (ReferenceEquals(_registeredGroup, Group))
        {
            return;
        }

        _registeredGroup?.Unregister(this);
        Group?.Register(this);
        _registeredGroup = Group;
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        if (!await JSModule.TryImportJavaScriptModuleAsync(JAVASCRIPT_FILE))
        {
            return;
        }

        _dotNetRef = DotNetObjectReference.Create(this);
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DragContainer.Initialize", Id, _dotNetRef, TouchDragDelay);
        _touchInitialized = true;
    }

    /// <summary />
    public override async ValueTask DisposeAsync()
    {
        if (_touchInitialized && JSModule.Imported)
        {
            await JSModule.ObjectReference.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.DragContainer.Dispose", Id);
        }

        _registeredGroup?.Unregister(this);
        _registeredGroup = null;
        _dotNetRef?.Dispose();
        _dotNetRef = null;
        await base.DisposeAsync();
    }

    /// <summary />
    [JSInvokable]
    public Task TouchDragStartAsync(string sourceId) => RunOnZoneAsync(sourceId, async zone =>
    {
        _touchSource = zone;
        await zone.StartDragAsync();
    });

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
    public Task TouchDragEndAsync(string sourceId) => InvokeAsync(async () =>
    {
        // Use the zone captured at drag start: the drop callback may have moved the item,
        // so the same id can now belong to a new zone in another container.
        var source = _touchSource;
        _touchSource = null;

        if (source != null)
        {
            await source.EndDragAsync();
            if (TryGetLocalZone(sourceId, out var registered) && ReferenceEquals(registered, source))
            {
                source.Refresh();
            }
        }

        if (StartedZone != null)
        {
            SetStartedZone(value: null);
        }
    });

    private FluentDropZone<TItem>? FindZone(string zoneId)
    {
        return TryGetLocalZone(zoneId, out var zone) ? zone : Group?.FindZone(zoneId);
    }

    private Task RunOnZoneAsync(string zoneId, Func<FluentDropZone<TItem>, Task> action)
    {
        return InvokeAsync(async () =>
        {
            var zone = FindZone(zoneId);
            if (zone != null)
            {
                await action(zone);
                zone.Refresh();
            }
        });
    }
}
