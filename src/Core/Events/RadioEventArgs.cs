// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentTabs ActiveId changed event.
/// </summary>
// This type is public because it is included in the public FluentUIJsonSerializerContext.
// It can be made internal again if the serializer context can be made internal in the future.
public class RadioEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the ID of the dialog.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets new active tab ID.
    /// </summary>
    public string? Value { get; set; }
}
