// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Shares the drag state between several <see cref="FluentDragContainer{TItem}"/> components,
/// so that items can be moved from one container to another.
/// Create one instance and assign it to the <see cref="FluentDragContainer{TItem}.Group"/> parameter of each container.
/// The identifiers of the zones must be unique across all containers of the group.
/// </summary>
/// <typeparam name="TItem">Type of the items moved between the containers.</typeparam>
public sealed class FluentDragGroup<TItem>
{
    private readonly List<FluentDragContainer<TItem>> _containers = [];

    /// <summary>
    /// Gets the unique identifier of the group.
    /// </summary>
    internal string Id { get; } = Identifier.NewId();

    /// <summary>
    /// Gets the zone currently dragged, whatever the container it belongs to.
    /// </summary>
    internal FluentDropZone<TItem>? StartedZone { get; private set; }

    /// <summary />
    internal void Register(FluentDragContainer<TItem> container)
    {
        if (!_containers.Contains(container))
        {
            _containers.Add(container);
        }
    }

    /// <summary />
    internal void Unregister(FluentDragContainer<TItem> container)
    {
        _containers.Remove(container);
    }

    /// <summary />
    internal void SetStartedZone(FluentDropZone<TItem>? value)
    {
        StartedZone = value;

        foreach (var container in _containers.ToArray())
        {
            container.Refresh();
        }
    }

    /// <summary />
    internal FluentDropZone<TItem>? FindZone(string zoneId)
    {
        foreach (var container in _containers)
        {
            if (container.TryGetLocalZone(zoneId, out var zone))
            {
                return zone;
            }
        }

        return null;
    }
}
