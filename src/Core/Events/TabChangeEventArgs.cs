// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentTabs ActiveId changed event.
/// </summary>
// This type is public because it is included in the public FluentUIJsonSerializerContext.
// It can be made internal again if the serializer context can be made internal in the future.
public class TabChangeEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the ID of the tab list.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets new active tab ID.
    /// </summary>
    public string? ActiveId { get; set; }

    /*
       This `IsAttributeSynchronized` value is computed in the browser when the event arguments are created by comparing the event's
       active tab ID with the fluent-tablist element's activeid attribute.
       It is a snapshot for this event, not additional selection state maintained between renders.
       
       - True means both values already match. This occurs when a programmatic attribute update
       triggers a change notification, or when refreshing the tab collection re-emits the existing selection.
       It does not guarantee that the value still matches the server's requested selection when the event arrives.
       
       - False means the values differ. For example, a click or keyboard navigation changes the
       Web Component's active tab property and raises the event before FAST asynchronously reflects that property
       into the HTML attribute. This identifies a selection not yet reflected in the attribute; it is not proof
       that a human initiated the change.
       
       - FluentTabs ignores a notification only when this value is true and
       ActiveId differs from its latest requested selection. For example, an old notification for
       setup must not overwrite a newer programmatic request for results.
       Otherwise, accepting the old value would rewrite the attribute and generate another notification,
       potentially causing an endless selection feedback loop. New client selections and confirmations of
       the current request remain eligible for normal selection handling.
    */

    /// <summary>
    /// Gets or sets whether the active ID matches the tab list's activeid attribute when the event is raised.
    /// </summary>
    public bool IsAttributeSynchronized { get; set; }
}
