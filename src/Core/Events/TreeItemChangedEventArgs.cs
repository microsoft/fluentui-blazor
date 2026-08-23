// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for a FluentTreeItem selection change.
/// </summary>
// This type is public because it is included in the public FluentUIJsonSerializerContext.
// It can be made internal again if the serializer context can be made internal in the future.
public class TreeItemChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the ID of the tree item.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is selected.
    /// </summary>
    public bool Selected { get; set; }
}
